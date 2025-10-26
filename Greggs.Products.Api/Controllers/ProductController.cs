using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using AutoMapper;

namespace Greggs.Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductsController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get([FromQuery] int? pageStart = null, [FromQuery] int? pageSize = null)
        {
            var items = await _service.GetProductsAsync(pageStart, pageSize);
            return Ok(items);
        }
    }
}