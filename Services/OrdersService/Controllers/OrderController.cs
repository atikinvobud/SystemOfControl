using System.Runtime.CompilerServices;
using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Share;
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
    public async Task<IActionResult> GetAll()
    {
        var result = await orderService.GetOrders();
        return result.ToHttpResult();
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid Id)
    {
        var result = await orderService.GetOrderById(Id);
        return result.ToHttpResult();
    }
    [HttpGet("GetByUserId")]
    public async Task<IActionResult> GetByUserId([FromQuery] Guid? StatusId, [FromQuery] int Page, [FromQuery] int PageSize)
    {
        Guid? id = tokenAccessor.GetUserId();
        var result = Result<bool>.CheckToken(id);
        if (!result.isSuccess) return result.ToHttpResult();
        var rresult = await orderService.GetOrdersWithPagination(id!.Value, StatusId, Page, PageSize);
        return rresult.ToHttpResult();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PostOrder postOrder)
    {
        Guid? id = tokenAccessor.GetUserId();
        var result = Result<bool>.CheckToken(id);
        if (!result.isSuccess) return result.ToHttpResult();
        var rresult = await orderService.Craeteorder(id!.Value, postOrder);
        return rresult.ToHttpResult();
    }
}