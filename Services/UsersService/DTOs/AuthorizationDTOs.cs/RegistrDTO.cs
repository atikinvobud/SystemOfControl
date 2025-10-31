namespace BackEnd.DTOs;

public record RegistrDTO
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RepeatPassword { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
}