using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SporSalonuYonetimi.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; }

        // IdentityUser'ın ID'si string olduğu için burayı string yapıyoruz
        public string? UserId { get; set; } 

        // Hazır gelen IdentityUser sınıfına bağlıyoruz
        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; }

        public bool IsConfirmed { get; set; } = false; // randevu onaylandı mı 

        public bool IsCancelled { get; set; } = false; // iptal edildi mi?

        public bool IsRejected { get; set; } = false; // Admin reddi

        public int TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public virtual Trainer? Trainer { get; set; }

        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }
    }
}