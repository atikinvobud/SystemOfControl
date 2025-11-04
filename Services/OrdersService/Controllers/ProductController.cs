using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Share;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;
    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet("{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetProductbyId([FromRoute] Guid Id)
    {
        var result = await productService.GetEntityById(Id);
        return result.ToHttpResult();
    }

    [HttpGet("")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetProducts()
    {
        var result = await productService.GetProducts();
        return result.ToHttpResult();
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromBody] PostProduct postProduct)
    {
        var result = await productService.CreateProduct(postProduct);
        return result.ToHttpResult();
    }

    [HttpPut("update")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct([FromBody] PutProduct putProduct)
    {
        var result = await productService.UpdateProduct(putProduct);
        return result.ToHttpResult();
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct([FromBody] DeleteProduct deleteProduct)
    {
        var result = await productService.DeleteProduct(deleteProduct);
        return result.ToHttpResult();
    }
}