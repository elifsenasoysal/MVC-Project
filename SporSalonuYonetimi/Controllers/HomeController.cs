using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // BU EKSİK OLABİLİR
using SporSalonuYonetimi.Data;       // BU EKSİK OLABİLİR
using SporSalonuYonetimi.Models;

namespace SporSalonuYonetimi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // 1. Veritabanı Bağlantısını Tanımlıyoruz
    private readonly ApplicationDbContext _context;

    // 2. Constructor (Yapıcı Metot) içinde bağlantıyı alıyoruz
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context; // Bağlantıyı içeri aldık
    }

    public IActionResult Index()
    {
        // Layout'ta tam ekran görünmesi için
        ViewData["TamEkran"] = true;

        // Veritabanından Salon Ayarlarını çekiyoruz
        var config = _context.SalonConfigs.FirstOrDefault();

        // Eğer veritabanı boşsa hata vermesin diye varsayılan oluşturuyoruz
        if (config == null)
        {
            config = new SalonConfig();
        }

        // Sayfaya bu ayarları gönderiyoruz
        return View(config);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}