using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TweetBook.Authorization;
using TweetBook.Contracts.V1;
using TweetBook.Filters;
using TweetBook.Options;
using TweetBook.Services;

namespace TweetBook.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);

            services.AddSingleton(jwtSettings);

            services.AddScoped<IIdentityService, IdentityService>();

            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    // Add our validation filter to mvc to initilize it
                    options.Filters.Add<ValidationFilter>();
                })
                // Fluent Valdiation
                .AddFluentValidation(conf => conf.RegisterValidatorsFromAssemblyContaining<Startup>());
            //  .SetCompatibilityVersion(CompatibilityVersion.Latest);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                // Secret should be at least 64 characters long - of ASCII characters
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
            };

            services.AddSingleton(tokenValidationParameters);

            // Authentication for logging user in
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            // Authorization is for knowing which fuctions can user access
            services.AddAuthorization(options =>
            {
                // Authorization using claims
                options.AddPolicy(PolicyClaims.Tags.PolicyName, builder =>
                    builder.RequireClaim(PolicyClaims.Tags.Claim, PolicyClaims.Tags.Value));

                // Add a Policy for a Authorzation handler
                options.AddPolicy(PolicyClaims.WorksForCompany.PolicyName, policy =>
                {
                    policy.AddRequirements(new WorksForCompanyRequirement(PolicyClaims.WorksForCompany.Value));
                });
            });

            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();
        }
    }
}
