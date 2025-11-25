using System.Diagnostics.Eventing.Reader;
using BackEnd.DTOs;
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
    public async Task<Result<string>> GenerateRecoveryCodeAsync(RequestRecoveryDto request)
    {
         if (string.IsNullOrWhiteSpace(request.Email)) return Result<string>.Error(ErrorCode.EmptyName);
        var code = new Random().Next(100000, 999999).ToString();
        var recoveryData = new RecoveryData
        {
            Code = code,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            IsUsed = false
        };
        bool flag = await repository.SaveRecoveryCodeAsync(request.Email, code, recoveryData, TimeSpan.FromMinutes(15));
        if (!flag) return Result<string>.Error(ErrorCode.RecoveryCodeError);
        flag = await repository.AddToRecoverySetAsync(request.Email, code, TimeSpan.FromMinutes(15));
        if (!flag) return Result<string>.Error(ErrorCode.RecoveryCodeError);
        await emailService.SendRecoveryCodeAsync(request.Email, code);       
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

    public async Task<Result<string?>> VerifyCodeAndGenerateTokenAsync(VerifyCodeDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Code)) return Result<string?>.Error(ErrorCode.EmptyName);
        var isValid = await ValidateRecoveryCodeAsync(request.Email, request.Code);
        if (!isValid) return Result<string?>.Error(ErrorCode.RecoveryCodeError);
        bool flag = await MarkCodeAsUsedAsync(request.Email, request.Code);
        if (!flag) return Result<string?>.Error(ErrorCode.RecoveryCodeError);
        var resetToken = Guid.NewGuid().ToString();
        await repository.SaveResetTokenAsync(resetToken, request.Email, TimeSpan.FromMinutes(10));        
        return Result<string?>.Success(resetToken);
    }

    public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ResetToken) || string.IsNullOrWhiteSpace(request.NewPassword)) return Result<bool>.Error(ErrorCode.EmptyName);
        var email = await repository.GetEmailByResetTokenAsync(request.ResetToken);
        if (string.IsNullOrEmpty(email)) return Result<bool>.Error(ErrorCode.RecoveryCodeError);
        if ( !await authorizationService.ChangePassword(email, request.NewPassword)) return Result<bool>.Error(ErrorCode.SavePasswordError);
        bool flag = await repository.DeleteResetTokenAsync(request.ResetToken);
        if (!flag) Result<bool>.Error(ErrorCode.RecoveryCodeError);
        return Result<bool>.Success(true);
    }
}