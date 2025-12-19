using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetimi.Data;
using SporSalonuYonetimi.Models;

namespace SporSalonuYonetimi.Controllers
{
    [Route("api/ajax")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AvailabilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Bu yardımcı sınıfı veri taşımak için kullanacağız
        public class TimeSlotDto
        {
            public string Time { get; set; }
            public bool IsFull { get; set; }
        }

        [HttpGet("check-hours")]
public IActionResult GetHours(int trainerId, DateTime date)
{
    try
    {
        // --- İŞTE ÇÖZÜM BU SATIRDA ---
        // PostgreSQL'e "Bu tarih UTC'dir, korkma" diyoruz.
        date = DateTime.SpecifyKind(date, DateTimeKind.Utc); 
        // ------------------------------

        // 1. Antrenör Kontrolü
        var trainer = _context.Trainers.FirstOrDefault(t => t.TrainerId == trainerId);
        if (trainer == null) return BadRequest("Hata: Antrenör bulunamadı.");

        // 2. Salon Ayarları
        var salonConfig = _context.SalonConfigs.FirstOrDefault();
        if (salonConfig == null) 
        {
            salonConfig = new SalonConfig { OpenHour = 9, CloseHour = 22, IsWeekendOpen = true };
        }

        // 3. Hafta Sonu Kontrolü
        if (!salonConfig.IsWeekendOpen && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
        {
            return Ok(new List<TimeSlotDto>());
        }

        // 4. Dolu Saatleri Çek (Hata burada çıkıyordu, artık çıkmayacak)
        var busyHours = _context.Appointments
            .Where(a => a.TrainerId == trainerId 
                        && a.AppointmentDate.Date == date.Date // Artık date UTC olduğu için sorun yok
                        && !a.IsCancelled 
                        && !a.IsRejected)
            .Select(a => a.AppointmentDate.Hour)
            .ToList();

        // 5. Müsaitlik Hesapla
        var allSlots = new List<TimeSlotDto>();

        for (int hour = salonConfig.OpenHour; hour < salonConfig.CloseHour; hour++)
        {
            if (hour >= trainer.WorkStartHour && hour < trainer.WorkEndHour)
            {
                bool isFull = busyHours.Contains(hour);
                
                // Buradaki DateTime.Now karşılaştırmasını da güvenli hale getirelim
                // date zaten UTC oldu, o yüzden UtcNow ile kıyaslayalım
                if (date.Date == DateTime.UtcNow.Date && hour <= DateTime.UtcNow.AddHours(3).Hour) 
                {
                    // Not: Türkiye saati UTC+3 olduğu için AddHours(3) ekledim ki
                    // "Geçmiş saat" kontrolü Türkiye saatine göre doğru çalışsın.
                    isFull = true; 
                }
                else if (date.Date < DateTime.UtcNow.Date) // Geçmiş günse tamamen kapat
                {
                     isFull = true;
                }

                allSlots.Add(new TimeSlotDto { Time = $"{hour:00}:00", IsFull = isFull });
            }
        }

        return Ok(allSlots);
    }
    catch (Exception ex)
    {
        return BadRequest("Sunucu Hatası Detayı: " + ex.Message);
    }
}
    }
}