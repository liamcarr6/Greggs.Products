using System;
using System.Collections.Generic;

namespace Greggs.Products.Api.Services;

/// <summary>
/// Service for converting prices from GBP to supported currencies
/// </summary>
public class CurrencyConverter : ICurrencyConverter
{
    // Static exchange rates - in production this would come from an external API
    private static readonly Dictionary<string, decimal> ExchangeRates = new()
    {
        { "GBP", 1.0m },    // Base currency
        { "EUR", 1.11m }    // 1 GBP = 1.11 EUR
    };

    /// <summary>
    /// Converts GBP amount to target currency using predefined exchange rates
    /// </summary>
    public decimal ConvertFromGbp(decimal gbpAmount, string targetCurrency)
    {
        if (!ExchangeRates.TryGetValue(targetCurrency, out var rate))
            throw new ArgumentException($"Currency '{targetCurrency}' is not supported");

        // Round to 2 decimal places for currency precision
        return decimal.Round(gbpAmount * rate, 2);
    }

    /// <summary>
    /// Returns list of all supported currency codes
    /// </summary>
    public IEnumerable<string> GetSupportedCurrencies() => ExchangeRates.Keys;
}
