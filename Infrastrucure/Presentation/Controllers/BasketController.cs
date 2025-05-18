using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTO.BasketModuleDTos;

namespace Presentation.Controllers
{
   
    public class BasketController(IServiceManeger _serviceManeger) : ApiBaseController
    {
        //Get Basket
        //Get BaseUrl/api/Basket
        [HttpGet]
        public async Task<ActionResult<BasketDTo>> GetBasket(string Key)
        {
            var Basket = await _serviceManeger.BasketService.GetBasketAsync(Key);
            return Ok(Basket);
        }

        //Create OR Update Basket
        [HttpPost]
        public async Task<ActionResult<BasketDTo>> CreateOrUpdateBasket(BasketDTo basket)
        {
            var Basket = await _serviceManeger.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }

        //Delete Basket
        [HttpDelete("{Key}")]
        public async Task<ActionResult<bool>>  DeleteBasket(string Key)
        {
            var Result = await _serviceManeger.BasketService.DeleteBasketAsync(Key);
            return Ok(Result);
        }

    }
}
