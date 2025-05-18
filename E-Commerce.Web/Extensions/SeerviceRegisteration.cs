using System.Text;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce.Web.Extensions
{
    public static class SeerviceRegisteration
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
        {
            //Add Services
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();

            return Services;
        }

        public static IServiceCollection AddWebApplicationServices(this IServiceCollection Services)
        {
            //Add Services
            Services.Configure<ApiBehaviorOptions>((Options) =>
            {
                Options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorsResponse;

            });

            return Services;
        }

        public static IServiceCollection AddJWTService(this IServiceCollection Services , IConfiguration configuration)
        {
            //دي الحاجات اللي هعمل فاليد عليها لما التوكن يجيلي عشان اشوف صح ولا غلط
            // محتاج اضيف في البروجرام app.UseAuthentication() , app.UseAuthorization()
            Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWTOptions:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["JWTOptions:Audience"],

                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTOptions:SecretKey"]))
                };
            });


            return Services;
        }
    }
}
