<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/ASP.NET%20MVC-Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/PostgreSQL-16-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" />
  <img src="https://img.shields.io/badge/Entity%20Framework-Core%208-512BD4?style=for-the-badge&logo=nuget&logoColor=white" />
  <img src="https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" />
  <img src="https://img.shields.io/badge/Google%20Gemini-AI-4285F4?style=for-the-badge&logo=google&logoColor=white" />
</p>

<h1 align="center">🏋️ Spor Salonu Yönetim & Randevu Sistemi</h1>

<p align="center">
  <b>ASP.NET Core MVC ile geliştirilmiş, yapay zekâ destekli, tam kapsamlı spor salonu yönetim ve online randevu platformu.</b>
</p>

<p align="center">
  <i>Admin paneli, üye girişi, akıllı randevu motoru, AI antrenör koçu ve dinamik salon konfigürasyonu — tek çatı altında.</i>
</p>

---

## 📋 İçindekiler

- [Proje Hakkında](#-proje-hakkında)
- [Öne Çıkan Özellikler](#-öne-çıkan-özellikler)
- [Teknoloji Yığını](#-teknoloji-yığını)
- [Mimari Yapı](#-mimari-yapı)
- [Veritabanı Tasarımı](#-veritabanı-tasarımı)
- [Proje Yapısı](#-proje-yapısı)
- [Kurulum & Çalıştırma](#-kurulum--çalıştırma)
- [Kullanım Senaryoları](#-kullanım-senaryoları)
- [Ekran Görüntüleri](#-ekran-görüntüleri)
- [Geliştirici](#-geliştirici)

---

## 🎯 Proje Hakkında

Bu proje, modern bir spor salonunun ihtiyaç duyduğu tüm dijital yönetim süreçlerini kapsayan **full-stack bir web uygulamasıdır**. Kullanıcılar online üye olup randevu alabilir, admin tüm operasyonları tek panelden yönetebilir ve yapay zekâ destekli koç modülüyle kişiselleştirilmiş fitness programları oluşturulabilir.

### 🔑 Çözülen Problemler

| Problem | Çözüm |
|---------|-------|
| Manuel randevu takibi | Otomatik çakışma kontrolü ile akıllı randevu motoru |
| Antrenör müsaitlik karmaşası | AJAX tabanlı gerçek zamanlı saat slotu kontrolü |
| Salon çalışma saati yönetimi | Dinamik hafta içi / hafta sonu konfigürasyon paneli |
| Kişiselleştirilmiş program ihtiyacı | Google Gemini Vision API ile AI destekli koç |
| Yetki ve erişim kontrolü | ASP.NET Identity ile rol tabanlı güvenlik altyapısı |

---

## ✨ Öne Çıkan Özellikler

### 👤 Üye (Member) Paneli
- ✅ Kayıt olma ve güvenli giriş yapma
- ✅ Hizmet seçimi ile randevu oluşturma
- ✅ Antrenör seçimi ve müsait saat görüntüleme
- ✅ Randevu geçmişini görüntüleme ve durum takibi
- ✅ Randevu iptal etme
- ✅ AI Koç ile kişisel program oluşturma

### 🛡️ Admin Paneli
- ✅ Tüm randevuları görüntüleme ve yönetme (Onayla / Reddet / Sil)
- ✅ Hizmet CRUD işlemleri (Oluştur, Oku, Güncelle, Sil)
- ✅ Antrenör CRUD işlemleri + Profil fotoğrafı desteği
- ✅ Antrenör-Hizmet ilişkilendirme (Many-to-Many)
- ✅ Antrenör mesai saatleri yönetimi + Salon saati validasyonu
- ✅ Salon çalışma saatleri konfigürasyonu (Hafta içi / Hafta sonu / Pazar)

### 🤖 Yapay Zekâ Modülü (AI Koç)
- ✅ Kullanıcı bilgilerine göre kişisel diyet ve egzersiz programı
- ✅ Fotoğraf yükleme ile **vücut tipi analizi** (Google Gemini Vision)
- ✅ **Pollinations AI** ile motivasyon görseli oluşturma
- ✅ Türkçe, samimi koç diliyle program çıktısı

### ⚡ Akıllı Randevu Motoru
- ✅ AJAX ile **gerçek zamanlı** müsait saat kontrolü
- ✅ Çakışma önleme (aynı antrenör, aynı saat)
- ✅ Geçmiş tarihe randevu engeli
- ✅ Salon çalışma saatlerine uyum kontrolü
- ✅ Antrenör mesai saatleri ile kesişim hesaplama
- ✅ Dolu/Boş saat slotlarının renk kodlamasıyla gösterimi

---

## 🛠️ Teknoloji Yığını

### Backend
| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| **ASP.NET Core MVC** | 8.0 | Web uygulama framework'ü |
| **Entity Framework Core** | 8.0 | ORM — Veritabanı erişim katmanı |
| **ASP.NET Identity** | 8.0 | Kimlik doğrulama & yetkilendirme |
| **Npgsql** | 8.0 | PostgreSQL veritabanı sağlayıcısı |
| **Google Gemini API** | 1.5 Flash | Yapay zekâ metin & görsel analizi |
| **Pollinations AI** | — | AI tabanlı görsel üretimi |

### Frontend
| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| **Razor Views** | — | Sunucu taraflı HTML oluşturma |
| **Bootstrap** | 5.3 | Responsive UI bileşenleri |
| **Font Awesome** | 6.4 | İkon kütüphanesi |
| **Google Fonts (Poppins)** | — | Modern tipografi |
| **jQuery** | — | AJAX istekleri & DOM manipülasyonu |
| **Vanilla JavaScript** | ES6+ | Dinamik saat slotu yönetimi |

### Veritabanı & Altyapı
| Teknoloji | Kullanım Amacı |
|-----------|----------------|
| **PostgreSQL** | İlişkisel veritabanı |
| **EF Core Migrations** | Veritabanı versiyon kontrolü |
| **DbSeeder** | Başlangıç verileri & Admin oluşturma |
| **Code-First Approach** | Model tabanlı veritabanı tasarımı |

---

## 🏗️ Mimari Yapı

Proje, **ASP.NET Core MVC (Model-View-Controller)** mimarisi üzerine inşa edilmiştir. Sorumluluklar net bir şekilde katmanlara ayrılmıştır:

```
┌─────────────────────────────────────────────────────────────┐
│                      PRESENTATION LAYER                     │
│              Views (Razor) + Static Files (CSS/JS)          │
│         Bootstrap 5.3 · Font Awesome · Google Fonts         │
├─────────────────────────────────────────────────────────────┤
│                      CONTROLLER LAYER                       │
│   HomeController · AppointmentController · AIController     │
│   TrainersController · ServicesController · SalonConfig...  │
│   AvailabilityController (REST API / AJAX)                  │
├─────────────────────────────────────────────────────────────┤
│                       SERVICE LAYER                         │
│              EmailSender · Identity Services                │
├─────────────────────────────────────────────────────────────┤
│                        MODEL LAYER                          │
│   Appointment · Trainer · Service · SalonConfig             │
│         Data Annotations · Navigation Properties            │
├─────────────────────────────────────────────────────────────┤
│                      DATA ACCESS LAYER                      │
│       ApplicationDbContext (IdentityDbContext)               │
│     Entity Framework Core · Code-First Migrations           │
├─────────────────────────────────────────────────────────────┤
│                       DATABASE LAYER                        │
│                     PostgreSQL 16                            │
└─────────────────────────────────────────────────────────────┘
```

### 🔐 Güvenlik Mimarisi

```
                    ┌──────────────┐
                    │   Kullanıcı  │
                    └──────┬───────┘
                           │
                    ┌──────▼───────┐
                    │  Login/Register │
                    │  (Identity UI)  │
                    └──────┬───────┘
                           │
              ┌────────────┼────────────┐
              │                         │
       ┌──────▼───────┐         ┌──────▼───────┐
       │  Admin Rolü   │         │  Member Rolü  │
       │  [Authorize   │         │  [Authorize]  │
       │  Roles=Admin] │         │               │
       └──────┬───────┘         └──────┬───────┘
              │                         │
    ┌─────────┴──────────┐       ┌─────┴──────┐
    │ • Randevu Yönetimi │       │ • Randevu  │
    │ • Hizmet CRUD      │       │   Alma     │
    │ • Antrenör CRUD    │       │ • Randevu  │
    │ • Salon Ayarları   │       │   Takibi   │
    │ • Onayla/Reddet    │       │ • AI Koç   │
    └────────────────────┘       └────────────┘
```

---

## 🗄️ Veritabanı Tasarımı

### Entity-Relationship Diyagramı

```
┌──────────────────────┐       ┌──────────────────────┐
│      IdentityUser    │       │       Service        │
│──────────────────────│       │──────────────────────│
│ Id (PK)              │       │ ServiceId (PK)       │
│ UserName             │       │ ServiceName          │
│ Email                │       │ DurationMinutes      │
│ PasswordHash         │       │ Price                │
│ ...                  │       └───────────┬──────────┘
└───────────┬──────────┘                   │
            │                              │ M:N
            │ 1:N                          │
            │                   ┌──────────┴──────────┐
┌───────────▼──────────┐       │                      │
│     Appointment      │       │   TrainerServices    │
│──────────────────────│       │   (Junction Table)   │
│ AppointmentId (PK)   │       └──────────┬───────────┘
│ AppointmentDate      │                  │
│ CreatedDate          │                  │ M:N
│ UserId (FK)          │                  │
│ TrainerId (FK)       │       ┌──────────▼───────────┐
│ ServiceId (FK)       │       │       Trainer        │
│ IsConfirmed          │       │──────────────────────│
│ IsCancelled          │       │ TrainerId (PK)       │
│ IsRejected           │       │ FullName             │
└───────────┬──────────┘       │ Specialization       │
            │                  │ Image (byte[])       │
            │ N:1              │ WorkStartHour        │
            └──────────────────│ WorkEndHour          │
                               └──────────────────────┘

┌──────────────────────┐
│     SalonConfig      │
│──────────────────────│
│ Id (PK)              │
│ WeekDayMorningStart  │
│ WeekDayMorningEnd    │
│ WeekDayEveningStart  │
│ WeekDayEveningEnd    │
│ WeekendMorningStart  │
│ WeekendMorningEnd    │
│ WeekendEveningStart  │
│ WeekendEveningEnd    │
│ IsSundayOpen         │
└──────────────────────┘
```

### İlişkiler

| İlişki | Tür | Açıklama |
|--------|-----|----------|
| `User → Appointment` | 1:N | Bir kullanıcı birden fazla randevu alabilir |
| `Trainer → Appointment` | 1:N | Bir antrenörün birden fazla randevusu olabilir |
| `Service → Appointment` | 1:N | Bir hizmet birden fazla randevuda kullanılabilir |
| `Trainer ↔ Service` | M:N | Bir antrenör birden fazla hizmet verebilir, bir hizmeti birden fazla antrenör sunabilir |

---

## 📁 Proje Yapısı

```
SporSalonuYonetimi/
│
├── 📂 Areas/
│   └── 📂 Identity/                   # ASP.NET Identity scaffolded sayfalar
│       └── 📂 Pages/Account/
│           ├── Login.cshtml            # Giriş sayfası (özelleştirilmiş)
│           ├── Register.cshtml         # Kayıt sayfası (özelleştirilmiş)
│           └── Logout.cshtml           # Çıkış işlemi
│
├── 📂 Controllers/
│   ├── HomeController.cs              # Ana sayfa & salon ayarlarını gösterir
│   ├── AppointmentController.cs       # Randevu CRUD + Admin onayla/reddet
│   ├── TrainersController.cs          # Antrenör CRUD + hizmet atama
│   ├── ServicesController.cs          # Hizmet CRUD (scaffolded + düzenlenmiş)
│   ├── SalonConfigController.cs       # Salon çalışma saatleri konfigürasyonu
│   ├── AvailabilityController.cs      # REST API — AJAX saat slotu kontrolü
│   └── AIController.cs               # AI Koç — Gemini API entegrasyonu
│
├── 📂 Models/
│   ├── Appointment.cs                 # Randevu modeli (FK + durum bayrakları)
│   ├── Trainer.cs                     # Antrenör modeli (mesai + profil fotoğrafı)
│   ├── Service.cs                     # Hizmet modeli (ad, süre, ücret)
│   ├── SalonConfig.cs                 # Salon ayarları modeli (çalışma saatleri)
│   └── ErrorViewModel.cs             # Hata sayfası modeli
│
├── 📂 Data/
│   ├── ApplicationDbContext.cs        # EF Core DbContext (IdentityDbContext)
│   └── DbSeeder.cs                   # Başlangıç rolleri & Admin seed
│
├── 📂 Services/
│   └── EmailSender.cs                # IEmailSender implementasyonu
│
├── 📂 Views/
│   ├── 📂 Home/
│   │   └── Index.cshtml              # Landing page (salon bilgileri + çalışma saatleri)
│   ├── 📂 Appointment/
│   │   ├── Create.cshtml             # Randevu oluşturma (AJAX saat seçimi)
│   │   ├── Index.cshtml              # Üye randevu listesi
│   │   ├── AdminIndex.cshtml         # Admin randevu yönetim paneli
│   │   └── BookingPanel.cshtml       # Hizmet seçim ekranı
│   ├── 📂 Trainers/
│   │   ├── Index.cshtml              # Admin antrenör listesi
│   │   ├── Create.cshtml             # Antrenör ekleme + hizmet seçimi
│   │   ├── Edit.cshtml               # Antrenör düzenleme
│   │   ├── Details.cshtml            # Antrenör detay
│   │   ├── Delete.cshtml             # Antrenör silme onayı
│   │   └── List.cshtml               # Herkese açık antrenör vitrin sayfası
│   ├── 📂 Services/                  # Hizmet CRUD view'ları
│   ├── 📂 SalonConfig/               # Salon ayarları düzenleme
│   ├── 📂 AI/                        # AI Koç arayüzü
│   └── 📂 Shared/
│       ├── _Layout.cshtml            # Ana layout (navbar + footer)
│       └── _LoginPartial.cshtml      # Giriş/çıkış butonu partial
│
├── 📂 Migrations/                    # EF Core migration geçmişi (12 migration)
├── 📂 wwwroot/                       # Statik dosyalar
│   ├── 📂 css/site.css               # Özel CSS stilleri
│   ├── 📂 js/site.js                 # Özel JavaScript
│   └── 📂 lib/                       # Client-side kütüphaneler (jQuery)
│
├── Program.cs                        # Uygulama başlangıç noktası & DI konfigürasyonu
├── appsettings.json                  # Bağlantı dizesi & uygulama ayarları
└── SporSalonuYonetimi.csproj         # NuGet paket referansları
```

---

## 🚀 Kurulum & Çalıştırma

### Ön Gereksinimler

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/)
- [Google Gemini API Key](https://aistudio.google.com/app/apikey) *(AI Koç modülü için)*

### 1️⃣ Projeyi Klonlayın

```bash
git clone https://github.com/elifsenasoysal/MVC-Project.git
cd MVC-Project/SporSalonuYonetimi
```

### 2️⃣ Veritabanı Bağlantısını Yapılandırın

`appsettings.json` dosyasındaki bağlantı dizesini kendi PostgreSQL bilgilerinize göre güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=SporSalonuDB;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

### 3️⃣ AI Koç API Anahtarını Ekleyin *(Opsiyonel)*

`Controllers/AIController.cs` dosyasında API anahtarınızı güncelleyin:

```csharp
private readonly string _geminiApiKey = "YOUR_GOOGLE_GEMINI_API_KEY";
```

### 4️⃣ Veritabanını Oluşturun

```bash
dotnet ef database update
```

> 💡 **Not:** Uygulama ilk çalıştığında `DbSeeder` otomatik olarak **Admin** ve **Member** rollerini oluşturur ve varsayılan admin hesabını ekler.

### 5️⃣ Uygulamayı Çalıştırın

```bash
dotnet run
```

Uygulama varsayılan olarak `https://localhost:5001` adresinde çalışacaktır.

### 📌 Varsayılan Admin Hesabı

| Alan | Değer |
|------|-------|
| **E-posta** | `b231210059@sakarya.edu.tr` |
| **Şifre** | `sau` |

---

## 🎮 Kullanım Senaryoları

### 🧑‍💼 Admin Senaryosu

```
1. Admin hesabıyla giriş yapın
2. "Yönetim Paneli" → Hizmetleri Yönet → Yeni hizmet ekleyin (örn: Pilates, 60dk, 500₺)
3. "Yönetim Paneli" → Antrenörleri Yönet → Yeni antrenör ekleyin ve hizmet atayın
4. "Salon Ayarları" → Çalışma saatlerini yapılandırın
5. "Randevu Yönetimi" → Gelen randevuları onaylayın veya reddedin
```

### 🏃 Üye Senaryosu

```
1. Kayıt olun ve giriş yapın
2. "Randevu Al" butonuna tıklayın
3. İstediğiniz hizmeti seçin
4. Antrenör seçin → Tarih seçin → Müsait saatler otomatik yüklenir
5. Uygun bir saat seçin ve randevunuzu oluşturun
6. "Randevularım" sayfasından durumu takip edin
7. "AI Koç" ile kişisel fitness programınızı oluşturun
```

### 🤖 AI Koç Senaryosu

```
1. "AI Koç" sayfasına gidin
2. Yaş, kilo, boy, cinsiyet ve hedefinizi girin
3. (Opsiyonel) Fotoğraf yükleyin — AI vücut tipinizi analiz eder
4. "Program Oluştur" butonuna tıklayın
5. Kişiselleştirilmiş diyet ve egzersiz programınızı alın
```

---

## 📸 Ekran Görüntüleri

> 📌 *Ekran görüntüleri projenin `screenshots/` klasörüne eklenerek bu bölüm güncellenebilir.*

| Sayfa | Açıklama |
|-------|----------|
| 🏠 **Ana Sayfa** | Salon bilgileri, çalışma saatleri ve hızlı erişim butonları |
| 📅 **Randevu Oluşturma** | Antrenör seçimi, tarih seçimi, AJAX ile dinamik saat slotları |
| 🛡️ **Admin Paneli** | Tüm randevuları görüntüleme, onaylama ve reddetme |
| 🤖 **AI Koç** | Gemini AI ile kişiselleştirilmiş fitness programı |
| 👥 **Antrenör Vitrin** | Tüm antrenörleri ve uzmanlıklarını gösteren herkese açık sayfa |
| ⚙️ **Salon Ayarları** | Hafta içi/sonu çalışma saatleri konfigürasyon ekranı |

---

## 🔑 Teknik Detaylar & Tasarım Kararları

### Neden Code-First Yaklaşımı?
- Model değişiklikleri `Migration` ile versiyon kontrolünde takip ediliyor
- 12 aşamalı migration geçmişi, projenin evrimsel gelişimini belgeler
- Veritabanı şeması C# modelleri üzerinden yönetiliyor

### Neden AJAX Tabanlı Saat Kontrolü?
- Sayfa yenilenmeden anlık müsaitlik görüntüleme
- `AvailabilityController` REST API endpoint'i ile temiz ayrışma
- Salon saatleri + Antrenör mesaisi + Mevcut randevular → akıllı kesişim

### Neden Rol Tabanlı Yetkilendirme?
- `[Authorize]` ve `[Authorize(Roles = "Admin")]` ile dekoratif güvenlik
- Admin-only CRUD operasyonları korunuyor
- Üye sadece kendi randevularını görebiliyor

### Neden Google Gemini Vision API?
- Çoklu modalite desteği (metin + görsel analiz)
- Türkçe dil desteği ile doğal koç deneyimi
- Fotoğraf bazlı vücut tipi analizi ile kişiselleştirme

---

## 📊 Proje İstatistikleri

| Metrik | Değer |
|--------|-------|
| **Controller Sayısı** | 7 |
| **Model Sayısı** | 5 |
| **View Sayısı** | 20+ |
| **Migration Sayısı** | 12 |
| **Kullanılan NuGet Paket** | 6 |
| **API Entegrasyonu** | 2 (Google Gemini + Pollinations) |
| **Mimari Desen** | MVC + Repository Pattern |
| **Yetkilendirme** | Role-Based Access Control (RBAC) |

---

## 🧑‍💻 Geliştirici

<p align="center">
  <b>Elif Sena Soysal</b><br/>
</p>

<p align="center">
  <a href="https://github.com/elifsenasoysal">
    <img src="https://img.shields.io/badge/GitHub-elifsenasoysal-181717?style=for-the-badge&logo=github" />
  </a>
</p>


