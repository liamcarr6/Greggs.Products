using System.Collections.Generic;

namespace Greggs.Products.Api.Dtos;
public class ProductDto
{
    public string Name { get; init; } = string.Empty;
    public List<PriceDto> Prices { get; set; } = new();
}