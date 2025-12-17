using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetimi.Models;

namespace SporSalonuYonetimi.Data
{
    // IdentityDbContext kullanarak kullanıcı tablolarını (Users, Roles) otomatik getiriyoruz
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<SalonConfig> SalonConfigs { get; set; }
    }
}