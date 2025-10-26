namespace Greggs.Products.Api.Dtos;

public class PriceDto
{
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
}