using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.Extensions.Options;
using Shared.DTO;

namespace Service.MapingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTo>()
                .ForMember(dist => dist.BrandName, Options => Options.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dist => dist.TypeName, Options => Options.MapFrom(src => src.ProductType.Name))
                .ForMember(dist => dist.PictureUrl, Options => Options.MapFrom<PictureUrlResolver>());
                //.ForMember(dist => dist.PictureUrl , Options => Options.MapFrom(src => $"https://localhost:7105/{src.PictureUrl}"));


            CreateMap<ProductType, TypeDTo>();
            CreateMap<ProductBrand, BrandDTo>();
        }
    }
}
