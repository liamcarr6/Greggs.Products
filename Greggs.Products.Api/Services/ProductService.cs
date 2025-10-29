using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services
{
    /// <summary>
    /// Business logic service for product operations with multi-currency support
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IDataAccess<Product> _data;
        private readonly ICurrencyConverter _currencyConverter;

        public ProductService(IDataAccess<Product> data, ICurrencyConverter currencyConverter)
        {
            _data = data;
            _currencyConverter = currencyConverter;
        }

        /// <summary>
        /// Retrieves products with prices converted to all supported currencies
        /// </summary>
        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int? pageStart, int? pageSize)
        {
            // Get paginated products from data layer
            var products = await _data.ListAsync(pageStart, pageSize);
            
            // Transform domain models to DTOs with multi-currency pricing
            return products.Select(product => new ProductDto
            {
                Name = product.Name,
                // Generate price list for each supported currency
                Prices = _currencyConverter.GetSupportedCurrencies()
                    .Select(currency => new PriceDto
                    {
                        Price = _currencyConverter.ConvertFromGbp(product.PriceInPounds, currency),
                        Currency = currency
                    }).ToList()
            });
        }
    }
}