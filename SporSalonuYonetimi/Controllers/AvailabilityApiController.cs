using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetimi.Data;
using SporSalonuYonetimi.Models;

namespace SporSalonuYonetimi.Controllers
{
    [Route("api/[controller]")] // Adresi: /api/availability
    [ApiController]
    public class AvailabilityApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AvailabilityApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/availability/GetHours?trainerId=5&date=2023-12-25
        [HttpGet("GetHours")]
        public List<string> GetHours(int trainerId, DateTime date)
        {
            // 1. Antrenörü Bul (Yoksa boş liste dön)
            var trainer = _context.Trainers.FirstOrDefault(t => t.TrainerId == trainerId);
            if (trainer == null) return new List<string>();

            // 2. Salon Ayarlarını Çek
            var salonConfig = _context.SalonConfigs.FirstOrDefault() ?? new SalonConfig();

            // 3. Dolu Saatleri Bul (LINQ Sorgusu)
            var busyHours = _context.Appointments
                .Where(a => a.TrainerId == trainerId 
                            && a.AppointmentDate.Date == date.Date
                            && !a.IsCancelled 
                            && !a.IsRejected)
                .Select(a => a.AppointmentDate.Hour)
                .ToList();

            // 4. Müsait Saatleri Hesapla
            List<string> availableSlots = new List<string>();

            for (int hour = salonConfig.OpenHour; hour < salonConfig.CloseHour; hour++)
            {
                // Hoca mesaisinde mi? VE Dolu değil mi?
                if (hour >= trainer.WorkStartHour && hour < trainer.WorkEndHour && !busyHours.Contains(hour))
                {
                    // Geçmiş saat kontrolü
                    if (date.Date == DateTime.Now.Date && hour <= DateTime.Now.Hour) continue;

                    availableSlots.Add($"{hour:00}:00");
                }
            }

            return availableSlots;
        }
    }
}