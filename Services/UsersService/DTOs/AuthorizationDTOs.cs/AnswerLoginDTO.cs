namespace BackEnd.DTOs;

public record AnswerLoginDTO
{
    public Guid UserId { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public string Token { get; set; } = string.Empty;
    public string CoockieName { get; set; } = string.Empty;
    public int ExpireMinutes { get; set; } = 0;
}