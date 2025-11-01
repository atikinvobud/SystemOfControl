using BackEnd.Share;

namespace BackEnd.Services;

public interface IPasswordRecoveryService
{
    Task<Result<string>> GenerateRecoveryCodeAsync(string email);
    Task<bool> ValidateRecoveryCodeAsync(string email, string code);
    Task<bool> MarkCodeAsUsedAsync(string email, string code);
    Task<Result<string?>> VerifyCodeAndGenerateTokenAsync(string email, string code);
    Task<Result<bool>> ResetPasswordAsync(string resetToken, string newPassword);
}