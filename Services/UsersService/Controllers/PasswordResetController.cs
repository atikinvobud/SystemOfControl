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
        if (string.IsNullOrWhiteSpace(request.Email)) return Result<string>.Error(ErrorCode.EmptyName).ToHttpResult();
        var result = await recoveryService.GenerateRecoveryCodeAsync(request.Email);
        return result.ToHttpResult();
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Code)) return Result<string>.Error(ErrorCode.EmptyName).ToHttpResult();
        var result = await recoveryService.VerifyCodeAndGenerateTokenAsync(request.Email, request.Code);
        return result.ToHttpResult();
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ResetToken) || string.IsNullOrWhiteSpace(request.NewPassword)) return Result<string>.Error(ErrorCode.EmptyName).ToHttpResult();
        var result = await recoveryService.ResetPasswordAsync(request.ResetToken, request.NewPassword);
        return result.ToHttpResult();
    }
}