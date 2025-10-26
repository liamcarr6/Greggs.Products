using AutoMapper;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Models;
using System.Collections.Generic;

namespace Greggs.Products.Api.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Prices, opt => opt.MapFrom(src => new List<PriceDto>
                {
                    new PriceDto
                    {
                        Price = src.PriceInPounds,
                        Currency = "GBP"
                    }
                }));
        }
    }
}