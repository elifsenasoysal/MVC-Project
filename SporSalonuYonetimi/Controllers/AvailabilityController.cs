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
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                var trainer = _context.Trainers.FirstOrDefault(t => t.TrainerId == trainerId);
                if (trainer == null) return BadRequest("Antrenör bulunamadı.");

                var salonConfig = _context.SalonConfigs.FirstOrDefault();
                if (salonConfig == null) salonConfig = new SalonConfig();

                List<int> validHours = new List<int>();

                // PAZAR KONTROLÜ
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (!salonConfig.IsSundayOpen)
                        return Ok(new List<TimeSlotDto>());
                }

                // HAFTA SONU TARİFESİ
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    int sabahSure = salonConfig.WeekendMorningEnd - salonConfig.WeekendMorningStart;
                    if (sabahSure > 0) validHours.AddRange(Enumerable.Range(salonConfig.WeekendMorningStart, sabahSure));

                    int aksamSure = salonConfig.WeekendEveningEnd - salonConfig.WeekendEveningStart;
                    if (aksamSure > 0) validHours.AddRange(Enumerable.Range(salonConfig.WeekendEveningStart, aksamSure));
                }
                // HAFTA İÇİ TARİFESİ
                else
                {
                    int sabahSure = salonConfig.WeekDayMorningEnd - salonConfig.WeekDayMorningStart;
                    if (sabahSure > 0) validHours.AddRange(Enumerable.Range(salonConfig.WeekDayMorningStart, sabahSure));

                    int aksamSure = salonConfig.WeekDayEveningEnd - salonConfig.WeekDayEveningStart;
                    if (aksamSure > 0) validHours.AddRange(Enumerable.Range(salonConfig.WeekDayEveningStart, aksamSure));
                }

                var busyHours = _context.Appointments
                    .Where(a => a.TrainerId == trainerId
                                && a.AppointmentDate.Date == date.Date
                                && !a.IsCancelled
                                && !a.IsRejected)
                    .Select(a => a.AppointmentDate.Hour)
                    .ToList();

                var allSlots = new List<TimeSlotDto>();

                foreach (var hour in validHours)
                {
                    // Hocanın çalışma saatleri kontrolü
                    if (hour >= trainer.WorkStartHour && hour < trainer.WorkEndHour)
                    {
                        bool isFull = busyHours.Contains(hour);

                        if (date.Date == DateTime.UtcNow.Date && hour <= DateTime.UtcNow.AddHours(3).Hour)
                        {
                            isFull = true;
                        }
                        else if (date.Date < DateTime.UtcNow.Date)
                        {
                            isFull = true;
                        }

                        allSlots.Add(new TimeSlotDto { Time = $"{hour:00}:00", IsFull = isFull });
                    }
                }

                return Ok(allSlots.OrderBy(x => x.Time).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest("Hata: " + ex.Message);
            }
        }
    }
}