using Microsoft.EntityFrameworkCore;
using AnimalShelter.DAL.Models;

namespace AnimalShelter.DAL
{
    public class AnimalShelterContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<VolunteerRequest> VolunteerRequests { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<User> Users { get; set; }

        public AnimalShelterContext() { }

        public AnimalShelterContext(DbContextOptions<AnimalShelterContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 🔹 Рядок підключення до PostgreSQL
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=animal_shelter;Username=postgres;Password=u30zd9CX");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
