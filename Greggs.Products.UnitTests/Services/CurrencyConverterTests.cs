using System;
using System.Linq;
using Greggs.Products.Api.Services;
using Xunit;

public class CurrencyConverterTests
{
    private readonly CurrencyConverter _converter = new();

    [Theory]
    [InlineData(1.0, "GBP", 1.0)]
    [InlineData(1.0, "EUR", 1.11)]
    [InlineData(0.7, "EUR", 0.78)]
    public void ConvertFromGbp_WithValidCurrency_ReturnsExpectedAmount(decimal amount, string currency, decimal expected)
    {
        // Act
        var result = _converter.ConvertFromGbp(amount, currency);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertFromGbp_WithUnsupportedCurrency_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _converter.ConvertFromGbp(1.0m, "USD"));
    }

    [Fact]
    public void GetSupportedCurrencies_ReturnsExpectedCurrencies()
    {
        // Act
        var currencies = _converter.GetSupportedCurrencies().ToList();

        // Assert
        Assert.Equal(2, currencies.Count);
        Assert.Contains("GBP", currencies);
        Assert.Contains("EUR", currencies);
    }
}