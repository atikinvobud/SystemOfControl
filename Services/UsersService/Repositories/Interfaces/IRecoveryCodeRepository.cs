using BackEnd.Models;

namespace BackEnd.Repositories;

public interface IRecoveryCodeRepository
{
    Task<bool> SaveRecoveryCodeAsync(string email, string code, RecoveryData data, TimeSpan expiry);
    Task<RecoveryData?> GetRecoveryCodeAsync(string email, string code);
    Task<bool> UpdateRecoveryCodeAsync(string email, string code, RecoveryData data, TimeSpan expiry);
    Task<bool> AddToRecoverySetAsync(string email, string code, TimeSpan expiry);
    Task<bool> RemoveFromRecoverySetAsync(string email, string code);
    Task<bool> SaveResetTokenAsync(string token, string email, TimeSpan expiry);
    Task<string?> GetEmailByResetTokenAsync(string token);
    Task<bool> DeleteResetTokenAsync(string token);
}