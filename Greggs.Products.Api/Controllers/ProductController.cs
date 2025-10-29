using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.Services;
using System;

namespace Greggs.Products.Api.Controllers
{
    /// <summary>
    /// API Controller for product operations with multi-currency pricing
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] // Maps to /api/products
    public class ProductsController : ControllerBase
    {
        // Business logic service for product operations
        private readonly IProductService _service;

        /// <summary>
        /// Constructor injection of ProductService
        /// </summary>
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// GET endpoint for paginated product list with currency conversion
        /// Example: GET /api/products?pageStart=0&pageSize=10
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get([FromQuery] int? pageStart = null, [FromQuery] int? pageSize = null)
        {
            // Input validation - handled by ExceptionHandlingMiddleware
            if (pageStart < 0)
                throw new ArgumentException("Page start cannot be negative");
            
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than zero");

            // Prevent excessive page sizes
            if (pageSize > 100)
                throw new ArgumentException("Page size cannot exceed 100");

            // Service handles data access, currency conversion, and DTO mapping
            var items = await _service.GetProductsAsync(pageStart, pageSize);
            return Ok(items);
        }
    }
}