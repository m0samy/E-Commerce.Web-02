using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;

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
            app.UseSwaggerUI();
            return app;
        }
    }
}
