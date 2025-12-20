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
        public string Specialization { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public byte[]? Image { get; set; }

        [Display(Name = "Mesai Başlangıç Saati")]
        [Range(0, 23)]
        public int WorkStartHour { get; set; } = 9;

        [Display(Name = "Mesai Bitiş Saati")]
        [Range(0, 23)]
        public int WorkEndHour { get; set; } = 18;

        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}