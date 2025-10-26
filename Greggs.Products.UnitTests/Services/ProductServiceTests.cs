using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Mappings;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProductsAsync_ReturnsMappedProductDtos()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Name = "Sausage Roll", PriceInPounds = 1.0m },
            new Product { Name = "Steak Bake", PriceInPounds = 1.2m }
        };

        var dataAccessMock = new Mock<IDataAccess<Product>>();
        dataAccessMock
            .Setup(d => d.ListAsync(It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync(products);

        var services = new ServiceCollection()
            .AddLogging()
            .AddAutoMapper(cfg => cfg.AddProfile<ProductProfile>());
        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        var service = new ProductService(dataAccessMock.Object, mapper);

        // Act
        var result = (await service.GetProductsAsync(null, null)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Sausage Roll", result[0].Name);
        Assert.Single(result[0].Prices);
        Assert.Equal(1.0m, result[0].Prices[0].Price);
        Assert.Equal("GBP", result[0].Prices[0].Currency);
        Assert.Equal("Steak Bake", result[1].Name);
        Assert.Single(result[1].Prices);
        Assert.Equal(1.2m, result[1].Prices[0].Price);
    }
}