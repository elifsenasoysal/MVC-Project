using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetimi.Data;
using SporSalonuYonetimi.Models;

namespace SporSalonuYonetimi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trainers
        public async Task<IActionResult> Index()
        {
            // Antrenörleri çekerken, ilişkili oldukları "TrainerServices" ve onun içindeki "Service" tablosunu da getir.
            // NOT: Eğer projenizde ara tablo ismi farklıysa (örn: sadece Services) ona göre düzenleyin.
            // Genelde .Include(t => t.TrainerServices).ThenInclude(ts => ts.Service) şeklinde olur.

            var trainers = await _context.Trainers
                .Include(t => t.Services) // Direkt Services tablosunu çekiyoruz
                .ToListAsync();
            return View(trainers);
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(t => t.Services) // DEĞİŞİKLİK: Detay sayfasında hizmetleri de görelim
                .FirstOrDefaultAsync(m => m.TrainerId == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            // DEĞİŞİKLİK: Checkbox listesi için tüm hizmetleri View'a gönderiyoruz
            ViewBag.Services = _context.Services.ToList();
            return View();
        }

        // POST: Trainers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DEĞİŞİKLİK: int[] selectedServices parametresi eklendi
        public async Task<IActionResult> Create([Bind("TrainerId,FullName,Specialization,WorkStartHour,WorkEndHour")] Trainer trainer, int[] selectedServices)
        {
            // Salon Ayarlarını Çek
            var salonConfig = _context.SalonConfigs.FirstOrDefault();

            if (salonConfig != null)
            {
                // 1. Başlangıç Saati Kontrolü
                if (trainer.WorkStartHour < salonConfig.OpenHour)
                {
                    ModelState.AddModelError("WorkStartHour", $"Antrenör, salon açılış saatinden ({salonConfig.OpenHour}:00) önce işe başlayamaz!");
                }

                // 2. Bitiş Saati Kontrolü
                if (trainer.WorkEndHour > salonConfig.CloseHour)
                {
                    ModelState.AddModelError("WorkEndHour", $"Antrenör, salon kapanış saatinden ({salonConfig.CloseHour}:00) sonra çalışamaz!");
                }
            }
            // Gelen Hizmet ID'lerini antrenöre ekle
            if (selectedServices != null)
            {
                foreach (var id in selectedServices)
                {
                    var service = await _context.Services.FindAsync(id);
                    if (service != null)
                    {
                        trainer.Services.Add(service);
                    }
                }
            }

            // Services validasyon hatasını temizle (Zorunlu değilse)
            ModelState.Remove("Services");

            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa listeyi tekrar yükle
            ViewBag.Services = _context.Services.ToList();
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Antrenörü, mevcut dersleriyle (Services) birlikte çekiyoruz
            var trainer = await _context.Trainers
                .Include(t => t.Services) // BURASI ÇOK ÖNEMLİ
                .FirstOrDefaultAsync(m => m.TrainerId == id);

            if (trainer == null) return NotFound();

            // Tüm dersleri View'a gönderiyoruz ki checkbox listesi oluşturabilelim
            ViewBag.AllServices = await _context.Services.ToListAsync();

            return View(trainer);
        }

        // POST: Trainers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainerId,FullName,Specialization,WorkStartHour,WorkEndHour")] Trainer trainer, int[] selectedServices)
        {
            // 1. Güvenlik Kontrolü
            if (id != trainer.TrainerId)
            {
                return NotFound();
            }

            // 2. Salon Saatleri Kontrolü (Senin istediğin özellik)
            var salonConfig = _context.SalonConfigs.FirstOrDefault();
            if (salonConfig != null)
            {
                if (trainer.WorkStartHour < salonConfig.OpenHour)
                {
                    ModelState.AddModelError("WorkStartHour", $"Antrenör, salon açılış saatinden ({salonConfig.OpenHour}:00) önce işe başlayamaz!");
                }
                if (trainer.WorkEndHour > salonConfig.CloseHour)
                {
                    ModelState.AddModelError("WorkEndHour", $"Antrenör, salon kapanış saatinden ({salonConfig.CloseHour}:00) sonra çalışamaz!");
                }
            }

            // 3. Geçerlilik Kontrolü
            if (ModelState.IsValid)
            {
                try
                {
                    // --- KRİTİK NOKTA ---
                    // Formdan gelen 'trainer' nesnesini direkt kaydetmek yerine,
                    // Veritabanındaki GERÇEK kaydı çekip, onun üzerini yazıyoruz.
                    // Bu sayede ilişkiler (Services) bozulmaz.

                    var trainerToUpdate = await _context.Trainers
                        .Include(t => t.Services) // Mevcut servisleriyle beraber çek
                        .FirstOrDefaultAsync(t => t.TrainerId == id);

                    if (trainerToUpdate == null)
                    {
                        return NotFound();
                    }

                    // A. Basit Bilgileri Güncelle
                    trainerToUpdate.FullName = trainer.FullName;
                    trainerToUpdate.Specialization = trainer.Specialization;
                    trainerToUpdate.WorkStartHour = trainer.WorkStartHour;
                    trainerToUpdate.WorkEndHour = trainer.WorkEndHour;

                    // B. Hizmetleri (Dersleri) Güncelle
                    // Önce eskisini temizle, sonra yenileri ekle
                    trainerToUpdate.Services.Clear();

                    if (selectedServices != null)
                    {
                        foreach (var serviceId in selectedServices)
                        {
                            var service = await _context.Services.FindAsync(serviceId);
                            if (service != null)
                            {
                                trainerToUpdate.Services.Add(service);
                            }
                        }
                    }

                    // C. Kaydet
                    _context.Update(trainerToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.TrainerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa sayfayı tekrar yükle (Dersleri tekrar seçili getirmek için)
            // Burası View tarafında Checkboxları doldurmak için gerekli
            var allServices = _context.Services.ToList();
            var trainerServices = trainer.Services?.Select(s => s.ServiceId).ToList() ?? new List<int>();

            // View'a verileri tekrar gönderiyoruz ki ekran boş gelmesin
            ViewData["Services"] = allServices.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = s.ServiceId.ToString(),
                Text = s.ServiceName,
                Selected = selectedServices.Contains(s.ServiceId)
            }).ToList();

            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(t => t.Services) // Silmeden önce ne ders verdiğini görelim
                .FirstOrDefaultAsync(m => m.TrainerId == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.TrainerId == id);
        }
    }
}