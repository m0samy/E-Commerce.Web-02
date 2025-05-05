using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTO;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")] //BaseUrl/api/Products
    public class ProductsController(IServiceManeger _serviceManeger) : ControllerBase 
    {
        //Get All Products => Get BaseUrl/api/Products
        [HttpGet]
        public async Task< ActionResult<IEnumerable<ProductDTo>> > GetAllProducts()
        {
            var Products = await _serviceManeger.ProductService.GetAllProductsAsync();
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
