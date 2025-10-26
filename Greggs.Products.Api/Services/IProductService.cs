using System.Collections.Generic;
using System.Threading.Tasks;
using Greggs.Products.Api.Dtos;

namespace Greggs.Products.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync(int? pageStart, int? pageSize);
    }
}
