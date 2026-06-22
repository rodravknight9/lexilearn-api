using Lexilearn.Application.Contracts.Identity;
using Lexilearn.Application.Models.Identity;
using Lexilearn.Identity.Persistence;
using Lexilearn.Identity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lexilearn.Identity
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection ConfigureIdentityService(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            if (environment.IsEnvironment("Testing"))
            {
                services.AddDbContext<LexilearnIdentityDbContext>(options =>
                    options.UseInMemoryDatabase("IdentityTestDb"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("IdentityDb");
                var serverVersion = ServerVersion.AutoDetect(connectionString);
                services.AddDbContext<LexilearnIdentityDbContext>(options =>
                    options.UseMySql(connectionString, serverVersion));
            }
            services.AddTransient<IAuthService, AuthService>();

            var jwtKey = configuration["JwtSettings:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (string.IsNullOrWhiteSpace(jwtKey) && environment.IsEnvironment("Testing"))
                jwtKey = "integration-test-signing-key-32chars!";
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("JWT signing key is not configured.");

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            return services;
        }
    }
}
