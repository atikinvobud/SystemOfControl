using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Share;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("status")]
public class StatusController : ControllerBase
{
    private readonly IStatusService statusService;
    public StatusController(IStatusService statusService)
    {
        this.statusService = statusService;
    }

    [HttpGet("{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStatusById([FromRoute] Guid Id)
    {
        var result = await statusService.GetStatusById(Id);
        return result.ToHttpResult();
    }

    [HttpGet("")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStatuses()
    {
        var result = await statusService.GetAllStatuses();
        return result.ToHttpResult();
    }
    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] PostStatus postStatus)
    {
        var result = await statusService.CreateStatus(postStatus);
        return result.ToHttpResult();
    }

    [HttpPut("update")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] PutStatus putStatus)
    {
        var result = await statusService.UpdateStatus(putStatus);
        return result.ToHttpResult();
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStatus([FromBody] DeleteStatus deleteStatus)
    {
        var result = await statusService.DeleteStatus(deleteStatus);
        return result.ToHttpResult();
    }
}