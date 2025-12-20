using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetimi.Data;
using SporSalonuYonetimi.Models;

namespace SporSalonuYonetimi.Controllers
{
    [Authorize] // sadece giriş yapanlar randevu alabilir
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AppointmentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // get- ekranı göster
        [HttpGet]
        public async Task<IActionResult> Create(int? serviceId)
        {
            if (serviceId == null) return RedirectToAction("Index", "Home");

            var service = await _context.Services.FindAsync(serviceId);

            // verileri viewa taşı
            ViewBag.ServiceName = service.ServiceName;
            ViewBag.ServiceId = service.ServiceId;
            ViewBag.Price = service.Price;

            // antrenörleri listele linq
            var availableTrainers = await _context.Trainers
                .Where(t => t.Services.Any(s => s.ServiceId == serviceId))
                .ToListAsync();
            ViewBag.Trainers = new SelectList(availableTrainers, "TrainerId", "FullName");
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment, string SelectedTime)
        {
            var userId = _userManager.GetUserId(User);
            appointment.UserId = userId;

            ModelState.Remove("CreatedDate");
            ModelState.Remove("UserId");
            ModelState.Remove("Trainer");
            ModelState.Remove("Service");
            ModelState.Remove("User");

            // saat birleştirme işlemi
            if (!string.IsNullOrEmpty(SelectedTime))
            {
                // veri "14:00" formatında string
                var timeParts = SelectedTime.Split(':');
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);

                // tarihin üzerine saati ekle
                appointment.AppointmentDate = appointment.AppointmentDate.Date.AddHours(hour).AddMinutes(minute);

                // UTC Ayarı (Postgres için)
                appointment.AppointmentDate = DateTime.SpecifyKind(appointment.AppointmentDate, DateTimeKind.Utc);
            }
            else
            {
                // eğer saat seçilmemişse hata ekle
                ModelState.AddModelError("", "Lütfen bir saat seçiniz.");
            }

            // geçmiş tarih kontrol
            if (appointment.AppointmentDate < DateTime.Now)
            {
                ModelState.AddModelError("", "Geçmiş bir tarihe randevu alamazsınız.");
            }

            // çakışma kontrolü
            bool isTrainerBusy = await _context.Appointments.AnyAsync(a =>
                a.TrainerId == appointment.TrainerId &&
                a.AppointmentDate == appointment.AppointmentDate &&
                !a.IsCancelled && !a.IsRejected); // iptal ve red hariç diğer randevulara bak

            if (isTrainerBusy)
            {
                ModelState.AddModelError("", "Seçtiğiniz antrenörün bu saatte başka bir randevusu mevcut.");
            }

            // kaydetme
            if (ModelState.IsValid)
            {
                appointment.CreatedDate = DateTime.UtcNow;
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            // hata varsa sayfayı tekrar doldur
            var service = await _context.Services.FindAsync(appointment.ServiceId);
            ViewBag.ServiceName = service?.ServiceName;
            ViewBag.Price = service?.Price;
            ViewBag.ServiceId = appointment.ServiceId;
            ViewBag.Trainers = new SelectList(_context.Trainers, "TrainerId", "FullName");

            return View(appointment);
        }

        // randevularımı listele
        public async Task<IActionResult> Index()
        {
            // giriş yapan kullanıcının ID'si alınır
            var userId = _userManager.GetUserId(User);

            // ve sadece bu kullanıcıya ait randevuları bulup getiririz
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppointmentDate) // en yeni tarih en üstte olucak şekilde sıraladık
                .ToListAsync();

            return View(appointments);
        }

        // --- ADMİN PANELİ İÇİN (TÜM RANDEVULARI GÖRÜR) ---
        [Authorize(Roles = "Admin")] // Bu sayfaya sadece Admin girebilir!
        public async Task<IActionResult> AdminIndex()
        {
            // Tüm randevuları getiriyoruz (Kim almış, Hangi Hoca, Hangi Ders)
            var allAppointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .Include(a => a.User) // Randevuyu alan öğrenciyi de görmek istiyoruz
                .OrderByDescending(a => a.AppointmentDate) // En yeni en üstte
                .ToListAsync();

            return View(allAppointments);
        }

        // --- ADMİNİN RANDEVUYU ONAYLAMASI İÇİN ---
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment != null)
            {
                appointment.IsConfirmed = true;
                await _context.SaveChangesAsync();
            }

            // işlem bitince yine listeye dön
            return RedirectToAction(nameof(AdminIndex));
        }



        //KULLANICI İÇİN: İPTAL ETME
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.IsCancelled = true; // iptal kutusunu işaretle
                await _context.SaveChangesAsync();
            }

            if (User.IsInRole("Admin")) return RedirectToAction(nameof(AdminIndex));
            return RedirectToAction(nameof(Index));
        }

        //ADMİN İÇİN: REDDETME
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.IsRejected = true; // reddedildi olarak işaretle
                appointment.IsConfirmed = false; // onaylıysa onayını kaldır
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminIndex));
        }

        //RANDEVUYU TAMAMEN SİL
        [HttpPost]
        [Authorize(Roles = "Admin")] // Sadece Admin tamamen silebilir
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment); // komple sil
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminIndex)); // admin paneline dön
        }

        // --- RANDEVU ALMA GİRİŞ EKRANI ---
        [Authorize] // sadece giriş yapanlar görebilir girmeyen Login'e atılır
        public async Task<IActionResult> BookingPanel()
        {
            // veritabanındaki tüm hizmetleri çekip ekrana gönderiyoruz
            var services = await _context.Services.ToListAsync();
            return View(services);
        }
    }
}