using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IDataAccess<Product> _data;
        private readonly IMapper _mapper;

        public ProductService(IDataAccess<Product> data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int? pageStart, int? pageSize)
        {
            var products = await _data.ListAsync(pageStart, pageSize);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}