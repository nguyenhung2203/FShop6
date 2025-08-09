using System.Net.Mail;
using System.Net;

public static class EmailHelper
{
    public static async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("taiptpk04158@gmail.com", "esvu qupo qryq peop"),
            EnableSsl = true
        };

        var message = new MailMessage("taiptpk04158@gmail.com", toEmail, subject, body);
        await smtp.SendMailAsync(message);
    }
}