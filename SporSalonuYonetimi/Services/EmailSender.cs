using Microsoft.AspNetCore.Identity.UI.Services;

namespace SporSalonuYonetimi.Services
{
    // Bu sınıf IEmailSender arayüzünü (interface) uygular
    // Amaç: Sistem "Mail servisi nerede?" dediğinde burayı göstermek.
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Gerçekten mail atmıyoruz, sadece görevi tamamlandı olarak işaretliyoruz.
            // İstersen buraya Console.WriteLine ile log basabilirsin.
            return Task.CompletedTask;
        }
    }
}