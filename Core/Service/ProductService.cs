using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DTO.ProductModuleDTos;

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

        public async Task<PaginatedResult<ProductDTo>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Repo = _unitOfWork.GetRepository<Product, int>();
            var specifications = new ProductWithBrandAndTypeSpecifications(queryParams);
            var Products = await Repo.GetAllAsync(specifications);
            var Data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTo>>(Products);
            var ProductCount = Products.Count();
            var TotalCount = await Repo.CountAsync(new ProductCountSpecification(queryParams));
            return new PaginatedResult<ProductDTo>(queryParams.PageIndex , ProductCount , TotalCount, Data);
        }

        public async Task<IEnumerable<TypeDTo>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType , int>().GetAllAsync();
            return _mapper.Map< IEnumerable<ProductType>, IEnumerable<TypeDTo> >(Types);
        }

        public async Task<ProductDTo> GetProductByIdAsync(int id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specifications);
            if(Product is null)
            {
                throw new ProductNotFoundException(id);
            }
            var ProductDTo = _mapper.Map<Product, ProductDTo>(Product);
            return ProductDTo;
        }


    }
}
