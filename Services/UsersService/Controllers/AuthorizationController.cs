using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Share;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[ApiController]
[Route("auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationServic authorizationServic;
    private readonly ITokenAccessor tokenAccessor;
    public AuthorizationController(IAuthorizationServic authorizationServic, ITokenAccessor tokenAccessor)
    {
        this.authorizationServic = authorizationServic;
        this.tokenAccessor = tokenAccessor;
    }

    [HttpPost("registr")]
    public async Task<IActionResult> Registr([FromBody] RegistrDTO registrDTO)
    {
        Result<Guid?> result = await authorizationServic.RegistrUser(registrDTO);
        return result.ToHttpResult();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        Result<AnswerLoginDTO> result = await authorizationServic.Login(loginDTO);
        return result.ToHttpResult();
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Guid? id = tokenAccessor.GetUserId();
        var result = Result<bool>.CheckToken(id);
        if (!result.isSuccess) return result.ToHttpResult();
        result = await authorizationServic.Logout(id!.Value);
        return result.ToHttpResult();
    }
    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh()
    {
        Guid? id = tokenAccessor.GetUserId();
        var result = Result<bool>.CheckToken(id);
        if (!result.isSuccess) return result.ToHttpResult();
        var response = await authorizationServic.RefreshToken(id!.Value);
        return response.ToHttpResult();
    }
}