using System.Collections.Generic;
using System.Threading.Tasks;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class ProductsControllerTests
{
    [Fact]
    public async Task Get_ReturnsOk_WithProductDtos()
    {
        // Arrange
        var dtos = new List<ProductDto>
        {
            new ProductDto { Name = "Yum Yum", Prices = new List<PriceDto> { new PriceDto { Price = 0.99m, Currency = "GBP" } } }
        };
        var serviceMock = new Mock<IProductService>();
        serviceMock.Setup(s => s.GetProductsAsync(null, null)).ReturnsAsync(dtos);

        var controller = new ProductsController(serviceMock.Object, null);

        // Act
        var result = await controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDtos = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
        Assert.Single(returnedDtos);
    }
}