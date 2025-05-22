using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Service.Specifications.OrderModuleSpecifications;
using ServiceAbstraction;
using Shared.DTO.IdentityModuleDTos;
using Shared.DTO.OrderModuleDTos;

namespace Service
{
    public class OrderService(IMapper _mapper , IBasketRepository _basketRepository , IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDTo> CreateOrderAsync(OrderDTo orderDTo, string Email)
        {
            //Map Address To OrderAddress
            var OrderAddress = _mapper.Map<AddressDTo , OrderAddress>(orderDTo.Address);

            //Get Basket
            var Basket = await _basketRepository.GetBasketAsync(orderDTo.BasketId) ?? throw new BasketNotFoundException(orderDTo.BasketId);

            //Create OrderItem List 
            List<OrderItem> OrderItems = [];
            var ProductRepo = _unitOfWork.GetRepository<Product , int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);

                OrderItems.Add(CreateOrderItem(item, Product));
            }

            //Get Delivery Method
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod , int>().GetByIdAsync(orderDTo.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(orderDTo.DeliveryMethodId);

            //Calculate SubTotal
            var SubTotal = OrderItems.Sum(I => I.Quantity * I.Price);

            //Create Order 
            var Order = new Order(Email , OrderAddress , DeliveryMethod , OrderItems , SubTotal);
            await _unitOfWork.GetRepository<Order , Guid>().AddAsync(Order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order, OrderToReturnDTo>(Order);

        }

        private static OrderItem CreateOrderItem(BasketItem item, Product Product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered() { ProductId = Product.Id, PictureUrl = Product.PictureUrl, ProductName = Product.Name },
                Price = Product.Price,
                Quantity = item.Quantity
            };
        }

        public async Task<IEnumerable<DeliveryMethodDTo>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod , int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod> , IEnumerable<DeliveryMethodDTo>>(DeliveryMethods);
        }

        public async Task<IEnumerable<OrderToReturnDTo>> GetAllOrdersAsync(string Email)
        {
            var Spec = new OrderSpecifications(Email);
            var Orders = await _unitOfWork.GetRepository<Order , Guid>().GetAllAsync(Spec);
            return _mapper.Map<IEnumerable<Order> , IEnumerable<OrderToReturnDTo>>(Orders);
        }

        public async Task<OrderToReturnDTo> GetOrderByIdAsync(Guid Id)
        {
            var Spec = new OrderSpecifications(Id);
            var Orders = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(Spec);
            return _mapper.Map<Order ,  OrderToReturnDTo>(Orders);

        }
    }
}
