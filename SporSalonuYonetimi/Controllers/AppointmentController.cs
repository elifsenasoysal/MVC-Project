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

        // 2. KAYDET (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            // --- KRİTİK NOKTA ---
            // Giriş yapan kullanıcının ID'sini STRING olarak alıyoruz.
            // Appointment modelinde de UserId string olduğu için tam uyumlu!
            var userId = _userManager.GetUserId(User);
            appointment.UserId = userId;

            // Hata vermemesi için, formdan gelmeyen alanları validasyon dışı bırak
            ModelState.Remove("CreatedDate");
            ModelState.Remove("UserId");
            ModelState.Remove("Trainer");
            ModelState.Remove("Service");
            ModelState.Remove("User"); // IdentityUser nesnesi formdan gelmez

            // Tarih Kontrolü
            if (appointment.AppointmentDate < DateTime.Now)
            {
                ModelState.AddModelError("", "Geçmiş bir tarihe randevu alamazsınız.");
            }

            // Eğer veriler düzgünse kaydet
            if (ModelState.IsValid)
            {
                appointment.CreatedDate = DateTime.UtcNow;

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // Başarılı olursa Ana Sayfaya dön
                return RedirectToAction("Index", "Home");
            }

            // Hata varsa formu tekrar doldurup göster
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
    }
}