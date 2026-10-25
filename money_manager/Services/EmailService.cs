using System.Net;
using System.Net.Mail;

namespace money_manager.Services;

public interface IEmailService
{
    Task SendEmailChangePasswordAsync(string recipientEmail, string link);
}

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmailChangePasswordAsync(string recipientEmail, string link)
    {
        string email = _configuration.GetValue<string>("EMAIL_SETTINGS:EMAIL") ?? string.Empty;
        string password = _configuration.GetValue<string>("EMAIL_SETTINGS:PASSWORD") ?? string.Empty;
        string host = _configuration.GetValue<string>("EMAIL_SETTINGS:HOST") ?? string.Empty;
        string port = _configuration.GetValue<string>("EMAIL_SETTINGS:PORT") ?? string.Empty;

        SmtpClient client = new(host, int.Parse(port))
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(email, password),
        };

        string subject = "Money Manager - Change Password";
        string htmlBody = @$"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                  <meta charset=""UTF-8"">
                  <title>Contraseña enviada</title>
                </head>
                <body style=""margin:0; padding:0; background-color:#f0f0f0; font-family:Arial, sans-serif;"">
                  <table align=""center"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width:600px; background-color:#ffffff; border-radius:8px; padding:20px; margin-top: 20px;"">
                    <tr>
                      <td align=""center"">
                        <h2 style=""color:#1a2a6c;"">Money Manager</h2>
                        <p style=""color:#333; font-size:15px;"">
                          Este mensaje le llega porque ha solicitado un cambio de contraseña.<br>
                          si esta solicitud no fue realizada por usted, no haga click en el siguiente enlace e ignore la solicitud.
                        </p>
                        <div style=""background-color:#ffcc00; color:#000; font-weight:bold; font-size:18px; padding:15px; border-radius:6px; margin:20px 0;"">
                           <a href=""{link}"" target=""_blank"">Cambiar contraseña</a>
                        </div>
                        <p style=""color:#555; font-size:13px;"">
                          Atentamente, el equipo de Money Manager.
                        </p>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>
        ";

        MailMessage message = new()
        {
            From = new MailAddress(email),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(recipientEmail);

        await client.SendMailAsync(message);
    }
}
