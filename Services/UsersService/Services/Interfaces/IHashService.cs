namespace BackEnd.Services;

public interface IHashService
{
    string Hash(string text);
    bool Verify(string hashText, string inputText);
}