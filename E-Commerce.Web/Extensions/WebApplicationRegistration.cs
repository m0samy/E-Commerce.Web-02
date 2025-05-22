using System.Text.Json;
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task SeedDataBaseAsync(this WebApplication app)
        {
            using var Scoope = app.Services.CreateScope();
            var objectDataSeeding = Scoope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await objectDataSeeding.DataSeedAsync();
            await objectDataSeeding.IdentityDataSeedAsync();
        }

        public static IApplicationBuilder USeCustomExceptionMiddleWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleWare>();

            return app;
        }

        public static IApplicationBuilder UseSwaggerMiddleWares(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(Options =>
            {
                Options.ConfigObject = new ConfigObject()
                {
                    DisplayRequestDuration = true
                };

                Options.DocumentTitle = "My E-Commerce API";

                //Options.JsonSerializerOptions = new JsonSerializerOptions()
                //{
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //};

                Options.DocExpansion(DocExpansion.None);
                Options.EnableFilter();


            });
            return app;
        }
    }
}
