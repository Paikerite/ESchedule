using MimeKit;
using MailKit.Net.Smtp;
using ESchedule.Services.Interfaces;

namespace ESchedule.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email,string fullname, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Підтвердження пошти", "admin@eschedule.com"));
            emailMessage.To.Add(new MailboxAddress(fullname, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("norkov900@gmail.com", "wttwcsefffytmkpz");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
