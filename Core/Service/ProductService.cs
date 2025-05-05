using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using ServiceAbstraction;
using Shared.DTO;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDTo>> GetAllBrandsAsync()
        {
            var Repo = _unitOfWork.GetRepository<ProductBrand , int>();
            var Brands = await Repo.GetAllAsync();
            var BrandsDto = _mapper.Map< IEnumerable<ProductBrand> , IEnumerable<BrandDTo> >(Brands);
            return BrandsDto;
        }

        public async Task<IEnumerable<ProductDTo>> GetAllProductsAsync()
        {
            //var Repo = _unitOfWork.GetRepository<Product ,int>();
            //var Products = await Repo.GetAllAsync();
            //var ProductsDto = _mapper.Map< IEnumerable<Product> , IEnumerable<ProductDTo> >(Products);
            //return ProductsDto;

            // طريقه مختصره
            var Products = await _unitOfWork.GetRepository<Product , int>().GetAllAsync();
            return _mapper.Map< IEnumerable<Product> , IEnumerable<ProductDTo> >(Products);
        }

        public async Task<IEnumerable<TypeDTo>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType , int>().GetAllAsync();
            return _mapper.Map< IEnumerable<ProductType>, IEnumerable<TypeDTo> >(Types);
        }

        public async Task<ProductDTo> GetProductByIdAsync(int id)
        {
            var Product = await _unitOfWork.GetRepository<Product , int>().GetByIdAsync(id);
            return _mapper.Map<Product , ProductDTo>(Product);
        }
    }
}
