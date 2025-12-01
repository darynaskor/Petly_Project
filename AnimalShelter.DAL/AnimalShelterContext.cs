using Microsoft.EntityFrameworkCore;
using AnimalShelter.DAL.Models;

namespace AnimalShelter.DAL
{
    public class AnimalShelterContext : DbContext
    {
        public DbSet<Shelter> shelters { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Pet> pets { get; set; }
        public DbSet<AdoptionRequest> adoption_requests { get; set; }

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
