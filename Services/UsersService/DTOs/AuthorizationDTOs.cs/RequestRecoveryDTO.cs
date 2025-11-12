namespace BackEnd.DTOs;

public record RequestRecoveryDto
{
    public string Email { get; set; } = string.Empty;
}