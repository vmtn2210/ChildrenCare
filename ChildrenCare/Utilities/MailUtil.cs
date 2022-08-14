using System.Net;
using System.Net.Mail;

namespace ChildrenCare.Utilities;

public class MailUtil
{
    public static async Task<CustomResponse> SendMail(string to, string body, string subject)
    {
        var response = new CustomResponse
        {
            IsSuccess = false
        };
        try
        {
            var from = GlobalVariables.Configuration.GetValue<string>("MailInfo:Username");
            var password = GlobalVariables.Configuration.GetValue<string>("MailInfo:Password");
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password)
            };
            var mail = new MailMessage
            {
                From = new MailAddress(from, from),
                Body = body,
                Subject = subject,
                IsBodyHtml = true
            };

            mail.To.Add(new MailAddress(to));

            await client.SendMailAsync(mail);
            response.IsSuccess = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            response.Message = "Send mail failed";
        }

        return response;
    }
}