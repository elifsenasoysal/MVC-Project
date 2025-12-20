using Microsoft.AspNetCore.Identity.UI.Services;

namespace SporSalonuYonetimi.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Gerçekten mail atmıyoruz, sadece görevi tamamlandı olarak işaretliyoruz.
            return Task.CompletedTask;
        }
    }
}