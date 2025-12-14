using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using SporSalonuYonetimi.Services;
using SporSalonuYonetimi.Data;


var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Identity Ayarları
// DİKKAT: Scaffolding buraya bazen 'AddDefaultIdentity' ekler, onu kullanmıyoruz.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Giriş sayfasının doğru adresi:
    options.LoginPath = "/Identity/Account/Login";
    // Çıkış yapınca gidilecek yer:
    options.LogoutPath = "/Identity/Account/Logout";
    // Yetkisiz giriş denemesinde gidilecek yer:
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Scaffolding sayfaları için gerekli

var app = builder.Build();

// 3. Veritabanı Oluşturma ve Admin Ekleme
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Veritabanını güncelle (Tabloları oluştur)
        context.Database.Migrate();

        // Admin ve Rolleri ekle
        await DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        // Hata olursa konsola yaz ama uygulamayı durdurma
        Console.WriteLine("Veritabanı işlemleri sırasında hata: " + ex.Message);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Identity sayfaları için gerekli

app.Run();