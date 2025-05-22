using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTO.OrderModuleDTos;

namespace ServiceAbstraction
{
    public interface IOrderService
    {
        //Create Order
        //Creating Order Will Take Basket Id , Shipping Address , Delivery Method Id , Customer Email
        //And Return Order Details (Id , UserEmail , OrderDate , Items (Product Name - Picture Url - Price - Quantity),
        // Address , Delivery Method Name , Order Status Value , Sub Total , Total Price  )
        Task<OrderToReturnDTo> CreateOrderAsync(OrderDTo orderDTo , string Email);

        //Get Delivery Method
        Task<IEnumerable<DeliveryMethodDTo>> GetDeliveryMethodsAsync();

        //Get All Orders
        Task<IEnumerable<OrderToReturnDTo>> GetAllOrdersAsync(string Email);

        //Get Order By Id
        Task<OrderToReturnDTo> GetOrderByIdAsync(Guid Id);
    }
}
