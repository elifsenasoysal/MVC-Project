using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SporSalonuYonetimi.Data;
using SporSalonuYonetimi.Models;
using System.Linq;

namespace SporSalonuYonetimi.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin erişebilir
    public class SalonConfigController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ayar Sayfasını Göster
        public IActionResult Index()
        {
            // Veritabanındaki ayarı çek
            var config = _context.SalonConfigs.FirstOrDefault();

            // Eğer veritabanı boşsa (İlk kez çalışıyorsa) varsayılan bir kayıt oluştur
            if (config == null)
            {
                config = new SalonConfig
                {
                    OpenHour = 9,
                    CloseHour = 22,
                    IsWeekendOpen = true
                };
                _context.SalonConfigs.Add(config);
                _context.SaveChanges();
            }

            return View(config);
        }

        // POST: Ayarları Güncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(SalonConfig config)
        {
            if (ModelState.IsValid)
            {
                // Mantık Kontrolü: Açılış saati kapanıştan büyük olamaz
                if (config.OpenHour >= config.CloseHour)
                {
                    TempData["Hata"] = "Açılış saati, kapanış saatinden küçük olmalıdır!";
                    return View("Index", config);
                }

                _context.SalonConfigs.Update(config);
                _context.SaveChanges();
                
                TempData["Mesaj"] = "Salon çalışma saatleri başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }

            return View("Index", config);
        }
    }
}