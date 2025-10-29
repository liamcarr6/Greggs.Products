using System;
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
    public async Task Get_ReturnsOk_WithProducts()
    {
        // Arrange
        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.GetProductsAsync(null, null))
               .ReturnsAsync(new List<ProductDto> { new ProductDto { Name = "Test Product" } });
        var controller = new ProductsController(mockService.Object);

        // Act
        var result = await controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
    }

    [Theory]
    [InlineData(-1, 10, "Page start cannot be negative")]
    [InlineData(0, 0, "Page size must be greater than zero")]
    [InlineData(0, 101, "Page size cannot exceed 100")]
    public async Task Get_WithInvalidInput_ThrowsArgumentException(int? pageStart, int? pageSize, string expectedMessage)
    {
        // Arrange
        var mockService = new Mock<IProductService>();
        var controller = new ProductsController(mockService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => controller.Get(pageStart, pageSize));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task Get_WithValidPagination_CallsServiceCorrectly()
    {
        // Arrange
        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.GetProductsAsync(5, 10)).ReturnsAsync(new List<ProductDto>());
        var controller = new ProductsController(mockService.Object);

        // Act
        await controller.Get(5, 10);

        // Assert
        mockService.Verify(s => s.GetProductsAsync(5, 10), Times.Once);
    }
}