using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SporSalonuYonetimi.Data;
using SporSalonuYonetimi.Models;
using System.Linq;

namespace SporSalonuYonetimi.Controllers
{
    [Authorize(Roles = "Admin")] // sadece Admin erişebilir
    public class SalonConfigController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var config = _context.SalonConfigs.FirstOrDefault();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SalonConfig salonConfig)
        {
            var existingConfig = await _context.SalonConfigs.FirstOrDefaultAsync();

            // Validasyon: Başlangıç saati Bitişten büyük olamaz kontrolü
            if (salonConfig.WeekDayMorningStart >= salonConfig.WeekDayMorningEnd)
            {
                TempData["Hata"] = "Hata: Başlangıç saati, bitiş saatinden büyük veya ona eşit olamaz!";
                return View("Index", salonConfig);
            }

            if (existingConfig != null)
            {
                //GÜNCELLEME
                salonConfig.Id = existingConfig.Id;
                _context.Entry(existingConfig).CurrentValues.SetValues(salonConfig);
            }
            else
            {
                // İLK KEZ OLUŞTURMA
                _context.Add(salonConfig);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Harika! Ayarlar başarıyla kaydedildi ve güncellendi. ✅";

            // Index sayfasına dön
            return RedirectToAction(nameof(Index));
        }
    }
}