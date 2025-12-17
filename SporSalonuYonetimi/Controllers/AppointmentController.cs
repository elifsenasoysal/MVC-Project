using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetimi.Data;
using SporSalonuYonetimi.Models;

namespace SporSalonuYonetimi.Controllers
{
    [Authorize] // Sadece giriş yapanlar randevu alabilir
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AppointmentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. EKRANI GÖSTER (GET)
        [HttpGet]
        public async Task<IActionResult> Create(int? serviceId)
        {
            if (serviceId == null) return RedirectToAction("Index", "Home");

            var service = await _context.Services.FindAsync(serviceId);

            // Verileri View'a taşı
            ViewBag.ServiceName = service.ServiceName;
            ViewBag.ServiceId = service.ServiceId;
            ViewBag.Price = service.Price;

            // Antrenörleri listele (Dropdown için)
            ViewBag.Trainers = new SelectList(_context.Trainers, "TrainerId", "FullName");

            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        // ValidateAntiForgeryToken varsa kalsın
        // DİKKAT: Parametre kısmına 'string SelectedTime' ekledik!
        public async Task<IActionResult> Create(Appointment appointment, string SelectedTime)
        {
            var userId = _userManager.GetUserId(User);
            appointment.UserId = userId;

            // Formdan gelmeyen verileri validasyondan çıkar
            ModelState.Remove("CreatedDate");
            ModelState.Remove("UserId");
            ModelState.Remove("Trainer");
            ModelState.Remove("Service");
            ModelState.Remove("User");

            // 1. SAAT BİRLEŞTİRME İŞLEMİ (HATAYI ÇÖZEN KISIM)
            if (!string.IsNullOrEmpty(SelectedTime))
            {
                // Gelen veri "14:00" formatında string
                var timeParts = SelectedTime.Split(':');
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);

                // Tarihin üzerine saati ekle
                appointment.AppointmentDate = appointment.AppointmentDate.Date.AddHours(hour).AddMinutes(minute);

                // UTC Ayarı (Postgres için)
                appointment.AppointmentDate = DateTime.SpecifyKind(appointment.AppointmentDate, DateTimeKind.Utc);
            }
            else
            {
                // Eğer saat seçilmemişse hata ekle
                ModelState.AddModelError("", "Lütfen bir saat seçiniz.");
            }

            // 2. GEÇMİŞ TARİH KONTROLÜ
            if (appointment.AppointmentDate < DateTime.Now)
            {
                ModelState.AddModelError("", "Geçmiş bir tarihe randevu alamazsınız.");
            }

            // 3. ÇAKIŞMA KONTROLÜ
            bool isTrainerBusy = await _context.Appointments.AnyAsync(a =>
                a.TrainerId == appointment.TrainerId &&
                a.AppointmentDate == appointment.AppointmentDate &&
                !a.IsCancelled && !a.IsRejected); // İptal/Red hariç

            if (isTrainerBusy)
            {
                ModelState.AddModelError("", "Seçtiğiniz antrenörün bu saatte başka bir randevusu mevcut.");
            }

            // 4. KAYDETME
            if (ModelState.IsValid)
            {
                appointment.CreatedDate = DateTime.UtcNow;
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            // Hata varsa sayfayı tekrar doldur
            var service = await _context.Services.FindAsync(appointment.ServiceId);
            ViewBag.ServiceName = service?.ServiceName;
            ViewBag.Price = service?.Price;
            ViewBag.ServiceId = appointment.ServiceId;
            ViewBag.Trainers = new SelectList(_context.Trainers, "TrainerId", "FullName");

            return View(appointment);
        }

        // Randevularımı Listele (GET)
        public async Task<IActionResult> Index()
        {
            // 1. Giriş yapan kullanıcının ID'sini al
            var userId = _userManager.GetUserId(User);

            // 2. Sadece bu kullanıcıya ait randevuları bul
            // .Include() komutları sayesinde Hizmet ve Antrenör isimlerini de getiriyoruz
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .Where(a => a.UserId == userId) // Filtreleme: Sadece benimkiler
                .OrderByDescending(a => a.AppointmentDate) // En yeni tarih en üstte
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
                appointment.IsConfirmed = true; // Durumu "Onaylandı" yap
                await _context.SaveChangesAsync();
            }

            // İşlem bitince yine listeye dön
            return RedirectToAction(nameof(AdminIndex));
        }



        // 1. KULLANICI İÇİN: İPTAL ETME (Cancel)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.IsCancelled = true; // Sadece iptal kutusunu işaretle
                                                // (IsRejected'a dokunma, bu kullanıcının kararı)
                await _context.SaveChangesAsync();
            }

            if (User.IsInRole("Admin")) return RedirectToAction(nameof(AdminIndex));
            return RedirectToAction(nameof(Index));
        }

        // 2. ADMİN İÇİN: REDDETME (Reject) - YENİ
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.IsRejected = true; // Reddedildi olarak işaretle
                appointment.IsConfirmed = false; // Onaylıysa onayını kaldır
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminIndex));
        }

        // 2. RANDEVUYU TAMAMEN SİL (Veritabanından uçurur)
        [HttpPost]
        [Authorize(Roles = "Admin")] // Sadece Admin tamamen silebilir
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment); // Komple sil
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminIndex)); // Admin paneline dön
        }

        // --- RANDEVU ALMA GİRİŞ EKRANI (HİZMET SEÇİMİ) ---
        [Authorize] // Sadece giriş yapanlar görebilir! (Girmeyen Login'e atılır)
        public async Task<IActionResult> BookingPanel()
        {
            // Veritabanındaki tüm hizmetleri çekip ekrana gönderiyoruz
            var services = await _context.Services.ToListAsync();
            return View(services);
        }
    }
}