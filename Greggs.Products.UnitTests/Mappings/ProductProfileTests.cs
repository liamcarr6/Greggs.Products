using AutoMapper;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Mappings;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 
using Xunit;

public class ProductMappingTests
{
    [Fact]
    public void Product_Maps_To_ProductDto_GbpPrice()
    {
        //Arrange
        var services = new ServiceCollection()
            .AddLogging() 
            .AddAutoMapper(cfg => cfg.AddProfile<ProductProfile>());
        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        //Act
        var dto = mapper.Map<ProductDto>(new Product { Name = "Steak Bake", PriceInPounds = 1.2m });

        //Assert
        Assert.Equal("Steak Bake", dto.Name);
        Assert.Single(dto.Prices);
        Assert.Equal(1.2m, dto.Prices[0].Price);
        Assert.Equal("GBP", dto.Prices[0].Currency);
    }
}
