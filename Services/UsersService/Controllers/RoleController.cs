
using BackEnd.DTOs;
using BackEnd.Models.Entities;
using BackEnd.Services;
using BackEnd.Share;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[ApiController]
[Route("role")]

public class RoleController : ControllerBase
{
    private readonly IRoleService roleService;
    public RoleController(IRoleService roleService)
    {
        this.roleService = roleService;
    }

    [HttpGet("getAll")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await roleService.GetAll();
        return result.ToHttpResult();
    }

    [HttpGet("{Id}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetRoleById([FromRoute] Guid Id)
    {
        var result = await roleService.GetById(Id);
        return result.ToHttpResult();
    }

    [HttpPost("create")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> CreateRole([FromBody] PostRole postRole)
    {
        var result = await roleService.CreateRole(postRole);
        return result.ToHttpResult();
    }

    [HttpDelete("delete")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> DeleteRole([FromBody] DeleteRole deleteRole)
    {
        var result = await roleService.DeleteRole(deleteRole);
        return result.ToHttpResult();
    }

    [HttpPut("update")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> UpdateRole([FromBody] PutRole putRole)
    {
        var result = await roleService.UpdateRole(putRole);
        return result.ToHttpResult();
    }
}