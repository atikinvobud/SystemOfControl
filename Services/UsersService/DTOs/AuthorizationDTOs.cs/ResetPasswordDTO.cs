namespace BackEnd.DTOs;

public record ResetPasswordDto
{
    public string ResetToken { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}