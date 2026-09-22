using Application.Products.DTOs;
using Application.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _service = productService;

    [HttpGet]
    public async Task<IActionResult> GetAllProductsAsync([FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 10)
    {
        var res = await _service.GetAllProductsAsync(pageNumber, pageSize);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategoryAsync(int categoryId)
    {
        var res = await _service.GetProductsByCategoryAsync(categoryId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProductsAsync([FromQuery]string name)
    {
        var res = await _service.SearchProductsAsync(name);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        var res = await _service.GetProductByIdAsync(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(CreateProductDto product)
    {
        var res = await _service.CreateProductAsync(product);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductAsync(int id, UpdateProductDto product)
    {
        var res = await _service.UpdateProductAsync(id, product);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        var res = await _service.DeleteProductAsync(id);
        return StatusCode(res.StatusCode, res);
    }
}
