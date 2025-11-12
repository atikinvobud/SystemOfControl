namespace BackEnd.DTOs;

public record SendEmailDTO
{
    public string EmailAddress { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Context { get; set; } = null!;
}