﻿using Cosmonaut.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TweetBook.Data;
using TweetBook.Domain;
using TweetBook.Options;

namespace TweetBook.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _dataContext;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters, DataContext dataContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new AuthenticationResult
                {
                    Errors = new[]
                    {
                        "User with this email does not exist"
                    }
                };

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if(!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[]
                    {
                        "User with this email and password does not exist"
                    }
                };
            }

            return await GenereateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(string Email, string Password)
        {
            var userExisting = await _userManager.FindByEmailAsync(Email);

            if (userExisting != null)
                return new AuthenticationResult
                {
                    Errors = new[]
                    {
                        "User with this email address already exists"
                    }
                };

            var newUser = new IdentityUser
            {
                Email = Email,
                UserName = Email,
            };

            var createdUser = await _userManager.CreateAsync(newUser, Password);

            if (!createdUser.Succeeded)
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description),
                };
            return await GenereateAuthenticationResultForUserAsync(newUser);
        }

        private async Task<AuthenticationResult> GenereateAuthenticationResultForUserAsync(IdentityUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id),
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = newUser.Id,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };

            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetClaimsPrincipalFromToken(token);

            if (validatedToken == null)
                return new AuthenticationResult
                {
                    Errors = new[] { "Invalid Token" }
                };

            // Get seconds from the validatedToken Claims
            var expiryDateUnix = long.Parse(validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            // Check if the expiry date is in future
            if(expiryDateTimeUtc > DateTime.UtcNow)
                return new AuthenticationResult
                {
                    Errors = new[] { "This token hasn't expired yet" }
                };

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if(storedRefreshToken == null)
                return new AuthenticationResult
                {
                    Errors = new[] { "This refresh token does not exist" }
                };

            if(DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new AuthenticationResult
                {
                    Errors = new[] { "This refresh token has already expired" }
                };

            if(storedRefreshToken.Invalidated)
                return new AuthenticationResult
                {
                    Errors = new[] {"This refresh token has been invalidated" }
                };

            if (storedRefreshToken.Used)
                return new AuthenticationResult
                {
                    Errors = new[] { "This refresh token has been used" }
                };

            if (storedRefreshToken.JwtId != jti)
                return new AuthenticationResult
                {
                    Errors = new[] { "This refresh token does not match this JWT" }
                };

            storedRefreshToken.Used = true;
            _dataContext.RefreshTokens.Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(
                validatedToken.Claims.Single(x => x.Type == "id").Value
                );

            return await GenereateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedtoken);

                return !IsJwtValidSecurityAlgorithm(validatedtoken) ? null : principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool IsJwtValidSecurityAlgorithm(SecurityToken validatedtoken)
        {
            return (validatedtoken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
