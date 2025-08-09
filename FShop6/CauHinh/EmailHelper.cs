using System.Net.Mail;
using System.Net;

public static class EmailHelper
{
    public static async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using (var smtp = new SmtpClient("smtp.gmail.com", 587))
        {
            smtp.Credentials = new NetworkCredential("taiptpk04158@gmail.com", "esvu qupo qryq peop"); // App password Gmail
            smtp.EnableSsl = true;

            using (var message = new MailMessage("taiptpk04158@gmail.com", toEmail, subject, body))
            {
                message.IsBodyHtml = true;
                await smtp.SendMailAsync(message);
            }
        }
    }
}
