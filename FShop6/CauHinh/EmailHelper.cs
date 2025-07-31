using System.Net;
using System.Net.Mail;

public class EmailHelper
{
    public static void GuiMaOTP(string email, string otp)
    {
        var fromEmail = "test@fshop6.com"; // địa chỉ bất kỳ, không cần là thật khi dùng Mailtrap
        var username = "6dd40b855d60af";   // username từ Mailtrap
        var password = "3d52b166b0bf55"; // password từ Mailtrap
        var host = "sandbox.smtp.mailtrap.io"; // SMTP host của Mailtrap

        var message = new MailMessage();
        message.From = new MailAddress(fromEmail, "FShop6");
        message.To.Add(email);
        message.Subject = "Mã OTP đặt lại mật khẩu";
        message.Body = $"<h3>Mã OTP: <b>{otp}</b></h3><p>Vui lòng nhập mã này để đặt lại mật khẩu.</p>";
        message.IsBodyHtml = true;

        var smtp = new SmtpClient(host, 587)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        smtp.Send(message);
    }
}
