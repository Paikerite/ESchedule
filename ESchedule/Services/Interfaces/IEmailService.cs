namespace ESchedule.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string fullname, string subject, string message);
    }
}
