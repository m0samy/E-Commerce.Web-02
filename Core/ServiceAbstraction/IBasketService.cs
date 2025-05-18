﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTO.BasketModuleDTos;

namespace ServiceAbstraction
{
    public interface IBasketService
    {
        Task<BasketDTo> GetBasketAsync(string Key);
        Task<BasketDTo> CreateOrUpdateBasketAsync(BasketDTo basket);
        Task<bool> DeleteBasketAsync(string Key);
    }
}
