using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
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

        var currencyConverterMock = new Mock<ICurrencyConverter>();
        currencyConverterMock
            .Setup(c => c.GetSupportedCurrencies())
            .Returns(new[] { "GBP", "EUR" });
        currencyConverterMock
            .Setup(c => c.ConvertFromGbp(It.IsAny<decimal>(), "GBP"))
            .Returns<decimal, string>((amount, _) => amount);
        currencyConverterMock
            .Setup(c => c.ConvertFromGbp(It.IsAny<decimal>(), "EUR"))
            .Returns<decimal, string>((amount, _) => amount * 1.11m);

        var service = new ProductService(dataAccessMock.Object, currencyConverterMock.Object);

        // Act
        var result = (await service.GetProductsAsync(null, null)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Sausage Roll", result[0].Name);
        Assert.Equal(2, result[0].Prices.Count);
        Assert.Contains(result[0].Prices, p => p.Currency == "GBP" && p.Price == 1.0m);
        Assert.Contains(result[0].Prices, p => p.Currency == "EUR" && p.Price == 1.11m);
    }

    [Fact]
    public async Task GetProductsAsync_ReturnsMappedProductsWithPrices()
    {
        // Arrange
        var products = new List<Product> { new Product { Name = "Test Product", PriceInPounds = 1.0m } };
        
        var dataAccessMock = new Mock<IDataAccess<Product>>();
        dataAccessMock.Setup(d => d.ListAsync(null, null)).ReturnsAsync(products);
        
        var currencyConverterMock = new Mock<ICurrencyConverter>();
        currencyConverterMock.Setup(c => c.GetSupportedCurrencies()).Returns(new[] { "GBP", "EUR" });
        currencyConverterMock.Setup(c => c.ConvertFromGbp(1.0m, "GBP")).Returns(1.0m);
        currencyConverterMock.Setup(c => c.ConvertFromGbp(1.0m, "EUR")).Returns(1.11m);
        
        var service = new ProductService(dataAccessMock.Object, currencyConverterMock.Object);

        // Act
        var result = (await service.GetProductsAsync(null, null)).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Test Product", result[0].Name);
        Assert.Equal(2, result[0].Prices.Count);
        Assert.Contains(result[0].Prices, p => p.Currency == "GBP" && p.Price == 1.0m);
        Assert.Contains(result[0].Prices, p => p.Currency == "EUR" && p.Price == 1.11m);
    }
}