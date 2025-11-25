using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Share;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[ApiController]
[Route("reset")]
public class PasswordRecoveryController : ControllerBase
{
    private readonly IPasswordRecoveryService recoveryService;

    public PasswordRecoveryController(IPasswordRecoveryService recoveryService)
    {
        this.recoveryService = recoveryService;
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestRecovery([FromBody] RequestRecoveryDto request)
    {
        var result = await recoveryService.GenerateRecoveryCodeAsync(request);
        return result.ToHttpResult();
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto request)
    {
        var result = await recoveryService.VerifyCodeAndGenerateTokenAsync(request);
        return result.ToHttpResult();
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
    {
        var result = await recoveryService.ResetPasswordAsync(request);
        return result.ToHttpResult();
    }
}