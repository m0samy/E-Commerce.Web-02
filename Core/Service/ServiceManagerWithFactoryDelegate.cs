﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceAbstraction;

namespace Service
{
    public class ServiceManagerWithFactoryDelegate(Func<IProductService> ProductFactory ,
        Func<IBasketService> BasketFactory ,
        Func<IAuthenticationService> AuthenticationFactory ,
        Func<IOrderService> OrderFactory) : IServiceManeger
    {
        public IProductService ProductService => ProductFactory.Invoke();

        public IBasketService BasketService => BasketFactory.Invoke();

        public IAuthenticationService AuthenticationService => AuthenticationFactory.Invoke();

        public IOrderService OrderService => OrderFactory.Invoke();
    }
}
