﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using ServiceAbstraction;
using Shared.DTO.BasketModuleDTos;

namespace Service
{
    public class BasketService(IBasketRepository _basketRepository , IMapper _mapper) : IBasketService
    {
        public async Task<BasketDTo> CreateOrUpdateBasketAsync(BasketDTo basket)
        {
            var CustomerBasket = _mapper.Map<BasketDTo , CustomerBasket>(basket);
            var CreateOrUpdateasket = await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            if (CreateOrUpdateasket is not null)
                return await GetBasketAsync(basket.Id);
            else
                throw new Exception("Can Not Update or Create Basket Now , Try Again Later ");
        }

        public async Task<BasketDTo> GetBasketAsync(string Key)
        {
            var Basket = await _basketRepository.GetBasketAsync(Key);
            if (Basket is not null)
                return _mapper.Map<CustomerBasket, BasketDTo>(Basket);
            else
            {
                // Handle Exception
                throw  new BasketNotFoundException(Key);
            }

        }

        public async Task<bool> DeleteBasketAsync(string Key) => await _basketRepository.DeleteBasketAsync(Key);

    }
}
