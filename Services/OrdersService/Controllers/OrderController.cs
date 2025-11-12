using System.Runtime.CompilerServices;
using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Share;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[ApiController]
[Route("order")]
public class OrderController : ControllerBase
{
    private readonly ITokenAccessor tokenAccessor;
    private readonly IOrderService orderService;
    public OrderController(IOrderService orderService, ITokenAccessor tokenAccessor)
    {
        this.orderService = orderService;
        this.tokenAccessor = tokenAccessor;
    }
    [HttpGet("")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await orderService.GetOrders();
        return result.ToHttpResult();
    }

    [HttpGet("{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById([FromRoute] Guid Id)
    {
        var result = await orderService.GetOrderById(Id);
        return result.ToHttpResult();
    }
    [HttpGet("GetByUserId")]
    [Authorize]
    public async Task<IActionResult> GetByUserId([FromQuery] Guid? StatusId, [FromQuery] int Page, [FromQuery] int PageSize)
    {
        Guid? id = tokenAccessor.GetUserId();
        var result = Result<bool>.CheckToken(id);
        if (!result.isSuccess) return result.ToHttpResult();
        var rresult = await orderService.GetOrdersWithPagination(id!.Value, StatusId, Page, PageSize);
        return rresult.ToHttpResult();
    }

    [HttpPost("create")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Create()
    {
        Guid? id = tokenAccessor.GetUserId();
        var result = Result<bool>.CheckToken(id);
        if (!result.isSuccess) return result.ToHttpResult();
        var rresult = await orderService.Craeteorder(id!.Value);
        return rresult.ToHttpResult();
    }

    [HttpPut("ChangeStatus")]
    [Authorize(Roles = "Manager, Engineer")]
    public async Task<IActionResult> Update([FromQuery] Guid OrderId, [FromQuery] string newStatus)
    {
        var result = await orderService.ChangeStatus(OrderId, newStatus);
        return result.ToHttpResult();
    }

    [HttpPut("addProduct")]
    [Authorize]
    public async Task<IActionResult> AddProduct([FromBody] PostOrderProduct postOrderProduct)
    {
        Guid? id = tokenAccessor.GetUserId();
        var result = Result<bool>.CheckToken(id);
        if (!result.isSuccess) return result.ToHttpResult();
        var rresult = await orderService.AddProduct(postOrderProduct, id!.Value);
        return rresult.ToHttpResult();
    }
}