﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;

namespace Service
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            //Add Services
            Services.AddAutoMapper(typeof(Service.AssemblyReference).Assembly);
            Services.AddScoped<IServiceManeger, ServiceManager>();
            return Services;
        }
    }
}
