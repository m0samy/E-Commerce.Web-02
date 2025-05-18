using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTO.ProductModuleDTos;

namespace Presentation.Controllers
{
    
    public class ProductsController(IServiceManeger _serviceManeger) : ApiBaseController
    {
        //Get All Products => Get BaseUrl/api/Products
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task< ActionResult<PaginatedResult<ProductDTo>> > GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
            var Products = await _serviceManeger.ProductService.GetAllProductsAsync(queryParams);
            return Ok(Products);
        }

        //Get Product By Id => Get BaseUrl/api/Products/id
        [HttpGet("{id:int}")]
        public async Task< ActionResult<ProductDTo> > GetProduct(int id)
        {
            var Product = await _serviceManeger.ProductService.GetProductByIdAsync(id);
            return Ok(Product);
        }

        //Get All Types => Get BaseUrl/api/Products/types
        [HttpGet("types")]
        public async  Task<ActionResult<IEnumerable<TypeDTo>>> GetTypes()
        {
            var Types = await _serviceManeger.ProductService.GetAllTypesAsync();
            return Ok(Types);
        }

        //Get AllBrands => Get BaseUrl/api/Products/brands
        [HttpGet("brands")]
        public async Task< ActionResult<IEnumerable<BrandDTo>> > GetBrands()
        {
            var Brands = await _serviceManeger.ProductService.GetAllBrandsAsync();
            return Ok(Brands);
        }

    }
}
