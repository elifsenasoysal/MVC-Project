using System.ComponentModel.DataAnnotations;

namespace SporSalonuYonetimi.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Display(Name = "Uzmanlık Alanı")]

        public string Specialization { get; set; } // Örn: Kas Geliştirme, Kilo Verme

        [Display(Name = "Mesai Başlangıç Saati")]
        [Range(0, 23)]
        public int WorkStartHour { get; set; } = 9; // Varsayılan sabah 9

        [Display(Name = "Mesai Bitiş Saati")]
        [Range(0, 23)]
        public int WorkEndHour { get; set; } = 18; // Varsayılan akşam 6

        // Antrenörün verebildiği hizmetler (Many-to-Many ilişki için)
        public ICollection<Service> Services { get; set; } = new List<Service>();

        // Randevular listesi
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}