using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonuYonetimi.Models
{
    public class SalonConfig
    {
        public int Id { get; set; }

        // hafta içi ayarları
        public int WeekDayMorningStart { get; set; } = 5;
        public int WeekDayMorningEnd { get; set; } = 9;
        public int WeekDayEveningStart { get; set; } = 17;
        public int WeekDayEveningEnd { get; set; } = 22;

        // hafta sonu ayarları (Cumartesi ve Pazar için Ortak)
        public int WeekendMorningStart { get; set; } = 5;
        public int WeekendMorningEnd { get; set; } = 9;

        public int WeekendEveningStart { get; set; } = 17;
        public int WeekendEveningEnd { get; set; } = 20;

        //pazar açık mı kapalı mı
        public bool IsSundayOpen { get; set; } = false;

        [NotMapped]
        public string WeekDayMorningText => $"{WeekDayMorningStart:00}:00 - {WeekDayMorningEnd:00}:00";
        [NotMapped]
        public string WeekDayEveningText => $"{WeekDayEveningStart:00}:00 - {WeekDayEveningEnd:00}:00";

        [NotMapped]
        public string WeekendMorningText => $"{WeekendMorningStart:00}:00 - {WeekendMorningEnd:00}:00";
        [NotMapped]
        public string WeekendEveningText => $"{WeekendEveningStart:00}:00 - {WeekendEveningEnd:00}:00";
    }
}