using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Share;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUser([FromRoute] Guid Id)
    {
        var result = await userService.GetUser(Id);
        return result.ToHttpResult();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] string? role, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = await userService.GetUsersWithPagination(role, page, pageSize);
        return result.ToHttpResult();
    }
    [HttpPut("updateUserInfo")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserInfo([FromBody] PutUserInfo putUserInfo)
    {
        var result = await userService.UpdateUserInfo(putUserInfo);
        return result.ToHttpResult();
    }

    [HttpPost("appointRole")]
    public async Task<IActionResult> AppointRole([FromBody] PostUserRole postUserRole)
    {
        var result = await userService.AppointRole(postUserRole);
        return result.ToHttpResult();
    }
    
}