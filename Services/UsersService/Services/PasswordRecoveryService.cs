using System.Diagnostics.Eventing.Reader;
using BackEnd.Models;
using BackEnd.Repositories;
using BackEnd.Share;
using Sprache;

namespace BackEnd.Services;

public class PasswordRecoveryService : IPasswordRecoveryService
{
    private readonly IRecoveryCodeRepository repository;
    private readonly IEmailService emailService;
    private readonly IAuthorizationServic authorizationService;
    public PasswordRecoveryService(IRecoveryCodeRepository repository, IEmailService emailService, IAuthorizationServic authorizationService)
    {
        this.repository = repository;
        this.emailService = emailService;
        this.authorizationService = authorizationService;
    }
    public async Task<Result<string>> GenerateRecoveryCodeAsync(string email)
    {
        var code = new Random().Next(100000, 999999).ToString();
        var recoveryData = new RecoveryData
        {
            Code = code,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            IsUsed = false
        };
        bool flag = await repository.SaveRecoveryCodeAsync(email, code, recoveryData, TimeSpan.FromMinutes(15));
        if (!flag) Result<string>.Error(ErrorCode.RecoveryCodeError);
        flag = await repository.AddToRecoverySetAsync(email, code, TimeSpan.FromMinutes(15));
        if (!flag) Result<string>.Error(ErrorCode.RecoveryCodeError);
        await emailService.SendRecoveryCodeAsync(email, code);       
        return Result<string>.Success(code);
    }

    public async Task<bool> ValidateRecoveryCodeAsync(string email, string code)
    {
        var recoveryData = await repository.GetRecoveryCodeAsync(email, code);
        if (recoveryData == null) return false;
        var isValid = !recoveryData.IsUsed && 
                     recoveryData.CreatedAt > DateTime.UtcNow.AddMinutes(-15);
        return isValid;
    }

    public async Task<bool> MarkCodeAsUsedAsync(string email, string code)
    {
        var recoveryData = await repository.GetRecoveryCodeAsync(email, code);
        if (recoveryData == null || recoveryData.IsUsed)
            return false;
        recoveryData.IsUsed = true;
        recoveryData.UsedAt = DateTime.UtcNow;
        await repository.UpdateRecoveryCodeAsync(email, code, recoveryData, TimeSpan.FromMinutes(5));
        await repository.RemoveFromRecoverySetAsync(email, code);
        return true;
    }

    public async Task<Result<string?>> VerifyCodeAndGenerateTokenAsync(string email, string code)
    {
        var isValid = await ValidateRecoveryCodeAsync(email, code);
        if (!isValid) return Result<string?>.Error(ErrorCode.RecoveryCodeError);
        bool flag = await MarkCodeAsUsedAsync(email, code);
        if (!flag) return Result<string?>.Error(ErrorCode.RecoveryCodeError);
        var resetToken = Guid.NewGuid().ToString();
        await repository.SaveResetTokenAsync(resetToken, email, TimeSpan.FromMinutes(10));        
        return Result<string?>.Success(resetToken);
    }

    public async Task<Result<bool>> ResetPasswordAsync(string resetToken, string newPassword)
    {
        var email = await repository.GetEmailByResetTokenAsync(resetToken);
        if (string.IsNullOrEmpty(email)) return Result<bool>.Error(ErrorCode.RecoveryCodeError);
        if ( !await authorizationService.ChangePassword(email, newPassword)) return Result<bool>.Error(ErrorCode.SavePasswordError);
        bool flag = await repository.DeleteResetTokenAsync(resetToken);
        if (!flag) Result<bool>.Error(ErrorCode.RecoveryCodeError);
        return Result<bool>.Success(true);
    }
}