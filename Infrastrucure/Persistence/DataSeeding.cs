using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext ,
        UserManager<ApplicationUser> _userManager , RoleManager<IdentityRole> _roleManager , StoreIdentityDbContext _identityDbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var PendingMigration = await _dbContext.Database.GetPendingMigrationsAsync();
                if (PendingMigration.Any())
                {
                   await _dbContext.Database.MigrateAsync();
                }


                if (!_dbContext.ProductBrands.Any())
                {
                    //Read Data
                    //var ProductBrandData = File.ReadAllText(@"..\Infrastrucure\Persistence\Data\DataSeed\brands.json");
                    var ProductBrandData = File.OpenRead(@"..\Infrastrucure\Persistence\Data\DataSeed\brands.json");
                    //Convert Data"String" => C# Objects  بعمل Deserializer
                    var ProductBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandData);
                    //Save To Database
                    if (ProductBrands is not null && ProductBrands.Any())
                        await _dbContext.ProductBrands.AddRangeAsync(ProductBrands);
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    var ProductTypeData = File.OpenRead(@"..\Infrastrucure\Persistence\Data\DataSeed\types.json");
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypeData);
                    if (ProductTypes is not null && ProductTypes.Any())
                        await _dbContext.ProductTypes.AddRangeAsync(ProductTypes);
                }
                if (!_dbContext.Products.Any())
                {
                    var ProductData = File.OpenRead(@"..\Infrastrucure\Persistence\Data\DataSeed\products.json");
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductData);
                    if (Products is not null && Products.Any())
                        await _dbContext.Products.AddRangeAsync(Products);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                //TODO
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Tarek",
                        PhoneNumber = "0123456789",
                        UserName = "MohamedTarek"
                    };
                    var User02 = new ApplicationUser()
                    {
                        Email = "Salma@gmail.com",
                        DisplayName = "Salma Mohamed",
                        PhoneNumber = "0123456789",
                        UserName = "SalmaMohamed"
                    };

                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");
                }

                await _identityDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }


        }
    }
}
