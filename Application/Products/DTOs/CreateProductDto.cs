namespace Application.Products.DTOs;

public class CreateProductDto
{
    public int CategoryId {get; set;}
    public string Name {get; set;}=string.Empty;
    public string? Description {get; set;}
    public decimal Price {get; set;}
    public int Quantity {get; set;}
}
