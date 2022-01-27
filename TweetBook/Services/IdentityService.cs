using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TweetBook.Domain;
using TweetBook.Options;

namespace TweetBook.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
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

            return GenereateAuthenticationResultForUser(user);
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
            return GenereateAuthenticationResultForUser(newUser);
        }

        private AuthenticationResult GenereateAuthenticationResultForUser(IdentityUser newUser)
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
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
            };
        }
    }
}
