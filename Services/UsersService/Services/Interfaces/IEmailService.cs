using BackEnd.DTOs;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IEmailService
{
    Task SendRecoveryCodeAsync(string email, string code);
    Task SendEmailAsync(SendEmailDTO sendEmailDTO);
}