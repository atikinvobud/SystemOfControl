namespace BackEnd.Services;

public interface ITokenAccessor
{
    string? GetAccessToken();
    Guid? GetUserId();
}