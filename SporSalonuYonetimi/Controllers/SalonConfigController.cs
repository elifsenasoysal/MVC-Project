using Microsoft.EntityFrameworkCore;
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
                    WeekDayMorningStart = 7,
                    WeekDayMorningEnd = 12,
                    WeekDayEveningStart = 15,
                    WeekDayEveningEnd = 22,
                };
                _context.SalonConfigs.Add(config);
                _context.SaveChanges();
            }

            return View(config);
        }

        // POST: Ayarları Güncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SalonConfig salonConfig)
        {
            // Veritabanındaki mevcut ayarı bulmaya çalışıyoruz
            var existingConfig = await _context.SalonConfigs.FirstOrDefaultAsync();

            // Validasyon: Başlangıç saati Bitişten büyük olamaz kontrolü (Örnek: Sabah bloğu için)
            if (salonConfig.WeekDayMorningStart >= salonConfig.WeekDayMorningEnd)
            {
                TempData["Hata"] = "Hata: Başlangıç saati, bitiş saatinden büyük veya ona eşit olamaz!";
                return View("Index", salonConfig);
            }

            if (existingConfig != null)
            {
                // SENARYO 1: GÜNCELLEME
                // Veritabanındaki ID'yi koruyoruz, diğer verileri formdan gelenlerle değiştiriyoruz.
                salonConfig.Id = existingConfig.Id;
                _context.Entry(existingConfig).CurrentValues.SetValues(salonConfig);
            }
            else
            {
                // SENARYO 2: İLK KEZ OLUŞTURMA
                // Veritabanı boşsa (ilk kurulum), yeni kayıt olarak ekle.
                _context.Add(salonConfig);
            }

            await _context.SaveChangesAsync();

            // Başarı mesajını TempData'ya atıyoruz
            TempData["SuccessMessage"] = "Harika! Ayarlar başarıyla kaydedildi ve güncellendi. ✅";

            // İşlem bitince yine Index sayfasına dönüyoruz
            return RedirectToAction(nameof(Index));
        }
    }
}