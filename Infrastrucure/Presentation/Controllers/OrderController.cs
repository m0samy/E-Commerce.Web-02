using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTO.OrderModuleDTos;

namespace Presentation.Controllers
{
    public class OrderController(IServiceManeger _serviceManeger) : ApiBaseController
    {
        //Create Order EndPoint
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTo>> CreateOrder(OrderDTo orderDTo)
        {
            var Order = await _serviceManeger.OrderService.CreateOrderAsync(orderDTo, GetEmailFromToken());
            return Ok(Order);
        }

        //Get Delivery Method
        [HttpGet("DeliveryMethods")] //Get BaseUrl/api/Orders/DeliveryMethods
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTo>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _serviceManeger.OrderService.GetDeliveryMethodsAsync();
            return Ok(DeliveryMethods);
        }

        //Get All Order By Email
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTo>>> GetAllOrders()
        {
            var Orders = await _serviceManeger.OrderService.GetAllOrdersAsync(GetEmailFromToken());
            return Ok(Orders);
        }

        //Get Order By Id
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTo>> GetOrderById(Guid id)
        {
            var Order = await _serviceManeger.OrderService.GetOrderByIdAsync(id);
            return Ok(Order);
        }

    }
}
