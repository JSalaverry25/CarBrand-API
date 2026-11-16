using Microsoft.EntityFrameworkCore;



namespace CarBrand.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Domain.CarBrand> CarBrands => Set<Domain.CarBrand>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.CarBrand>(entity =>
            {
                entity.ToTable("CarBrand");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CountryOrigin).IsRequired().HasMaxLength(100);
                entity.Property(e => e.BrandName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Year).IsRequired();
                entity.HasIndex(e => e.BrandName).IsUnique();
            });

            modelBuilder.Entity<Domain.CarBrand>().HasData(
                new Domain.CarBrand { Id = 1, BrandName = "Audi", CountryOrigin = "Germany", Year = 1932 },
                new Domain.CarBrand { Id = 2, BrandName = "Chevrolet", CountryOrigin = "USA", Year = 1911 },
                new Domain.CarBrand { Id = 3, BrandName = "Porche", CountryOrigin = "Germany", Year = 1931 }
            );

        }
    }
}
