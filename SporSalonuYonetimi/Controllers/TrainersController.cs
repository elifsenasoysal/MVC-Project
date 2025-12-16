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
            // DEĞİŞİKLİK: Antrenörleri getirirken verdikleri hizmetleri de (Services) yanına ekle
            return View(await _context.Trainers.Include(t => t.Services).ToListAsync());
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
        public async Task<IActionResult> Create([Bind("TrainerId,FullName,Specialization")] Trainer trainer, int[] selectedServices)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            // DEĞİŞİKLİK: Mevcut hizmetleriyle beraber getiriyoruz
            var trainer = await _context.Trainers
                                        .Include(t => t.Services)
                                        .FirstOrDefaultAsync(t => t.TrainerId == id);
            if (trainer == null)
            {
                return NotFound();
            }

            // Tüm hizmetleri View'a gönder (Checkboxlar için)
            ViewBag.Services = _context.Services.ToList();
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DEĞİŞİKLİK: Güncelleme mantığı değişti
        public async Task<IActionResult> Edit(int id, [Bind("TrainerId,FullName,Specialization")] Trainer trainer, int[] selectedServices)
        {
            if (id != trainer.TrainerId)
            {
                return NotFound();
            }

            ModelState.Remove("Services");

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Veritabanındaki asıl antrenörü ilişkileriyle beraber çek
                    var trainerToUpdate = await _context.Trainers
                        .Include(t => t.Services)
                        .FirstOrDefaultAsync(t => t.TrainerId == id);

                    if (trainerToUpdate == null) return NotFound();

                    // 2. Bilgileri güncelle
                    trainerToUpdate.FullName = trainer.FullName;
                    trainerToUpdate.Specialization = trainer.Specialization;

                    // 3. İlişkileri Güncelle (Eskileri sil, yenileri ekle)
                    trainerToUpdate.Services.Clear(); // Önceki dersleri temizle
                    
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

                    // 4. Kaydet
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

            ViewBag.Services = _context.Services.ToList();
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