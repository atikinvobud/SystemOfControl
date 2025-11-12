using System.Text.Json;
using BackEnd.Models;
using StackExchange.Redis;

namespace BackEnd.Repositories;

public class RedisRepository : IRecoveryCodeRepository
{
    private readonly IConnectionMultiplexer redis;
    private readonly IDatabase database;
    public RedisRepository(IConnectionMultiplexer redis)
    {
        this.redis = redis;
        database = redis.GetDatabase();
    }
    public async Task<bool> SaveRecoveryCodeAsync(string email, string code, RecoveryData data, TimeSpan expiry)
    {
        var key = $"recovery:{email}:{code}";
        var value = JsonSerializer.Serialize(data);
        return await database.StringSetAsync(key, value, expiry);
    }

    public async Task<RecoveryData?> GetRecoveryCodeAsync(string email, string code)
    {
        var key = $"recovery:{email}:{code}";
        var value = await database.StringGetAsync(key);
        
        if (value.IsNullOrEmpty) return null;
        
        return JsonSerializer.Deserialize<RecoveryData>(value!);
    }

    public async Task<bool> UpdateRecoveryCodeAsync(string email, string code, RecoveryData data, TimeSpan expiry)
    {
        var key = $"recovery:{email}:{code}";
        var value = JsonSerializer.Serialize(data);
        return await database.StringSetAsync(key, value, expiry);
    }

    public async Task<bool> AddToRecoverySetAsync(string email, string code, TimeSpan expiry)
    {
        var key = $"recovery_codes:{email}";
        var added = await database.SetAddAsync(key, code);
        if (added)
        {
            await database.KeyExpireAsync(key, expiry);
        }
        return added;
    }

    public async Task<bool> RemoveFromRecoverySetAsync(string email, string code)
    {
        var key = $"recovery_codes:{email}";
        return await database.SetRemoveAsync(key, code);
    }

    public async Task<bool> SaveResetTokenAsync(string token, string email, TimeSpan expiry)
    {
        var key = $"password_reset:{token}";
        return await database.StringSetAsync(key, email, expiry);
    }

    public async Task<string?> GetEmailByResetTokenAsync(string token)
    {
        var key = $"password_reset:{token}";
        var value = await database.StringGetAsync(key);
        return value.IsNullOrEmpty ? null : value.ToString();
    }

    public async Task<bool> DeleteResetTokenAsync(string token)
    {
        var key = $"password_reset:{token}";
        return await database.KeyDeleteAsync(key);
    }
}