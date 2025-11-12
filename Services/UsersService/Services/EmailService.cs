using BackEnd.DTOs;
using MimeKit;
using MailKit.Net.Smtp;
namespace BackEnd.Services;

public class EmailService : IEmailService
{
    private string emailAddress;
    private string emailPassword;
    public EmailService(IConfiguration configuration)
    {
        emailAddress = configuration["EmailSettings:Address"]!;
        emailPassword = configuration["EmailSettings:Password"]!;
    }

    public async Task SendEmailAsync(SendEmailDTO sendEmailDTO)
    {
        using MimeMessage emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("DefectFixer", emailAddress));
        emailMessage.To.Add(new MailboxAddress("", sendEmailDTO.EmailAddress));
        emailMessage.Subject = sendEmailDTO.Title;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = sendEmailDTO.Context
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.yandex.ru", 465, true);
            await client.AuthenticateAsync(emailAddress, emailPassword);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }

    public async Task SendRecoveryCodeAsync(string email, string code)
    {
        string title = "Восстановление пароля SystemOfControl";
        string context = $@"
            <html>
            <head>
                <meta charset='UTF-8'>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f9f9f9;
                        padding: 20px;
                    }}
                    .container {{
                        background-color: #ffffff;
                        border: 1px solid #dddddd;
                        border-radius: 5px;
                        padding: 30px;
                        max-width: 600px;
                        margin: auto;
                        text-align: center;
                    }}
                    .code {{
                        display: inline-block;
                        font-size: 24px;
                        font-weight: bold;
                        color: #d9534f;
                        padding: 10px 20px;
                        background-color: #f7f7f7;
                        border-radius: 5px;
                        margin: 20px 0;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Восстановление пароля</h2>
                    <p>Вы запросили восстановление пароля. Используйте следующий код для восстановления доступа:</p>
                    <div class='code'>{code}</div>
                    <p>Если вы не инициировали восстановление пароля, проигнорируйте это письмо.</p>
                    <p>С уважением,<br/>SystemOfControl</p>
                </div>
            </body>
            </html>";
        var sendEmailDTO = new SendEmailDTO
        {
            EmailAddress = email,
            Title = title,
            Context = context
        };

        await SendEmailAsync(sendEmailDTO); 
    }
}