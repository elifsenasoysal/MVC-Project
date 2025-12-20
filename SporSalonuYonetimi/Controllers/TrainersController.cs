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
            var trainers = await _context.Trainers
                .Include(t => t.Services)
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
                .Include(t => t.Services)
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
            ViewBag.Services = _context.Services.ToList();
            return View();
        }

        // POST: Trainers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainerId,FullName,Specialization,WorkStartHour,WorkEndHour")] Trainer trainer, int[] selectedServices)
        {
            // Salon Ayarlarını Çek
            var salonConfig = _context.SalonConfigs.FirstOrDefault();
            if (salonConfig != null)
            {
                int salonAcilis = salonConfig.WeekDayMorningStart;

                int salonKapanis = salonConfig.WeekDayEveningEnd;

                if (trainer.WorkStartHour < salonAcilis)
                {
                    ModelState.AddModelError("WorkStartHour", $"Antrenör mesaisi salon açılışından ({salonAcilis}:00) önce başlayamaz.");
                }

                if (trainer.WorkEndHour > salonKapanis)
                {
                    ModelState.AddModelError("WorkEndHour", $"Antrenör mesaisi salon kapanışından ({salonKapanis}:00) sonra bitemez.");
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

            ModelState.Remove("Services");

            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Services = _context.Services.ToList();
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // antrenörü mevcut dersleriyle birlikte çekiyoruz
            var trainer = await _context.Trainers
                .Include(t => t.Services) // BURASI ÇOK ÖNEMLİ
                .FirstOrDefaultAsync(m => m.TrainerId == id);

            if (trainer == null) return NotFound();

            // tüm dersleri view'a gönderiyoruz ki checkbox listesi oluşturabilelim
            ViewBag.AllServices = await _context.Services.ToListAsync();

            return View(trainer);
        }

        // POST: Trainers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainerId,FullName,Specialization,WorkStartHour,WorkEndHour")] Trainer trainer, int[] selectedServices)
        {
            if (id != trainer.TrainerId)
            {
                return NotFound();
            }

            var salonConfig = _context.SalonConfigs.FirstOrDefault();
            if (salonConfig != null)
            {
                // Salonun en erken açılışı
                int salonAcilis = salonConfig.WeekDayMorningStart;

                // Salonun en geç kapanışı
                int salonKapanis = salonConfig.WeekDayEveningEnd;

                if (trainer.WorkStartHour < salonAcilis)
                {
                    ModelState.AddModelError("WorkStartHour", $"Antrenör mesaisi salon açılışından ({salonAcilis}:00) önce başlayamaz.");
                }

                if (trainer.WorkEndHour > salonKapanis)
                {
                    ModelState.AddModelError("WorkEndHour", $"Antrenör mesaisi salon kapanışından ({salonKapanis}:00) sonra bitemez.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var trainerToUpdate = await _context.Trainers
                        .Include(t => t.Services) // Mevcut servisleriyle beraber çek
                        .FirstOrDefaultAsync(t => t.TrainerId == id);

                    if (trainerToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Basit Bilgileri Güncelle
                    trainerToUpdate.FullName = trainer.FullName;
                    trainerToUpdate.Specialization = trainer.Specialization;
                    trainerToUpdate.WorkStartHour = trainer.WorkStartHour;
                    trainerToUpdate.WorkEndHour = trainer.WorkEndHour;

                    // Hizmetleri Güncelle
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

                    // Kaydet
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

            var allServices = _context.Services.ToList();
            var trainerServices = trainer.Services?.Select(s => s.ServiceId).ToList() ?? new List<int>();

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
                .Include(t => t.Services) // silmeden önce ne ders verdiğini görelim
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

        // GET: Trainers/List
        // Bu sayfa normal kullanıcıların hocaları vitrin gibi göreceği sayfadır.
        [AllowAnonymous] // giriş yapmayanlar da hocaları görebilsin
        public async Task<IActionResult> List()
        {
            return View(await _context.Trainers.ToListAsync());
        }
    }
}