using System.ComponentModel.DataAnnotations;

namespace SporSalonuYonetimi.Models
{
    public class SalonConfig
    {
        [Key]
        public int Id { get; set; } // Genelde hep ID=1 olan tek kayıt duracak

        [Display(Name = "Salon Açılış Saati")]
        public int OpenHour { get; set; } = 8; 

        [Display(Name = "Salon Kapanış Saati")]
        public int CloseHour { get; set; } = 22;

        [Display(Name = "Hafta Sonu Açık mı?")]
        public bool IsWeekendOpen { get; set; } = true;
    }
}