using BackEnd.DTOs;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IPasswordRecoveryService
{
    Task<Result<string>> GenerateRecoveryCodeAsync(RequestRecoveryDto request);
    Task<bool> ValidateRecoveryCodeAsync(string email, string code);
    Task<bool> MarkCodeAsUsedAsync(string email, string code);
    Task<Result<string?>> VerifyCodeAndGenerateTokenAsync(VerifyCodeDto request);
    Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto request);
}