namespace BackEnd.Services;

public interface ICoockieService
{
    bool SetCookie(string name, string token, int days);
    string? GetCookie(string key);
    void DeleteCookie(string key);
}