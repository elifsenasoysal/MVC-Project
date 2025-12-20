using System.ComponentModel.DataAnnotations;

namespace SporSalonuYonetimi.Models
{
    // spor salonunun sunduğu hizmetleri temsil eden model
    public class Service
    {
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string ServiceName { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Ücret")]
        public decimal Price { get; set; }

        // Bir hizmeti birden fazla antrenör verebilir
        public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
    }
}