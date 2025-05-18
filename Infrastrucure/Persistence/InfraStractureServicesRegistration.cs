


using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Persistence.Identity;
using StackExchange.Redis;

namespace Persistence
{
    public static class InfraStractureServicesRegistration
    {
        public static IServiceCollection AddInfrastracureServices(this IServiceCollection Services , IConfiguration Configuration)
        {
            //Add Services
            Services.AddDbContext<StoreDbContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            Services.AddScoped<IDataSeeding, DataSeeding>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddSingleton<IConnectionMultiplexer>( (_) =>
            {
                return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnectionString")!); 
            });

            Services.AddDbContext<StoreIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });

            Services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();

            return Services;
        }
    }
}
