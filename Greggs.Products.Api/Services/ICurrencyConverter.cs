using System.Collections.Generic;

namespace Greggs.Products.Api.Services;

public interface ICurrencyConverter
{
    decimal ConvertFromGbp(decimal gbpAmount, string targetCurrency);
    IEnumerable<string> GetSupportedCurrencies();
}