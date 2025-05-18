
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Service;
using ServiceAbstraction;
using Shared.ErrorModels;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            builder.Services.AddControllers();

           builder.Services.AddSwaggerServices();

            builder.Services.AddInfrastracureServices(builder.Configuration);
            builder.Services.AddApplicationServices();

            builder.Services.AddWebApplicationServices();

            builder.Services.AddJWTService(builder.Configuration);
            #endregion

            var app = builder.Build();


            //DataSeeding
            await app.SeedDataBaseAsync();


            #region Configure the HTTP request pipeline.
            app.USeCustomExceptionMiddleWare();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
