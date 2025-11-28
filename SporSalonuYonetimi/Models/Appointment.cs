using System.ComponentModel.DataAnnotations;

namespace SporSalonuYonetimi.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi")]
        public DateTime Date { get; set; }

        [Display(Name = "Onay Durumu")]
        public bool IsConfirmed { get; set; } = false; // Varsayılan onaysız

        // İlişkiler (Foreign Keys)
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        // Randevuyu alan üyenin ID'si (Identity User ID string tutulur)
        public string UserId { get; set; }
    }
}