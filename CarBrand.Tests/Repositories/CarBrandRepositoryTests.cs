using CarBrand.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBrand.Tests.Repositories
{
    public class CarBrandRepositoryTests
    {
        // Cada prueba obtiene su propia base de datos.
        private static async Task<AppDbContext> CreateContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            // Crea la base de datos y carga los datos HasData.
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        [Fact]
        public async Task GetAllCarBrands_ShouldReturnPagedResults()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            // Act
            var result = await repository.GetAllCarBrandsAsync(1, 2);

            // Assert
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(1, result.Page);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(2, result.Items.Count);

            Assert.Equal("Audi", result.Items[0].BrandName);
            Assert.Equal("Chevrolet", result.Items[1].BrandName);
        }

        [Fact]
        public async Task GetCarBrandById_ShouldReturnBrand_WhenExists()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            // Act
            var result = await repository.GetCarBrandByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Audi", result.BrandName);
            Assert.Equal("Germany", result.CountryOrigin);
        }

        [Fact]
        public async Task GetCarBrandById_ShouldReturnNull_WhenDoesNotExist()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            // Act
            var result = await repository.GetCarBrandByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExistsByName_ShouldReturnTrue_WhenBrandExists()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            // Act
            var result = await repository.ExistsByNameAsync(" audi ");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsByName_ShouldIgnoreExcludedId()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            // Act
            var result = await repository.ExistsByNameAsync("Audi", 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddNewCarBrand_ShouldSaveBrand()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            var brand = new Domain.CarBrand
            {
                BrandName = "BMW",
                CountryOrigin = "Germany",
                Year = 1916
            };

            // Act
            var result = await repository.AddNewCarBrandAsync(brand);

            // Assert
            Assert.True(result.Id > 0);
            Assert.Equal("BMW", result.BrandName);

            var savedBrand = await context.CarBrands
                .FirstOrDefaultAsync(b => b.Id == result.Id);

            Assert.NotNull(savedBrand);
            Assert.Equal("Germany", savedBrand.CountryOrigin);
        }

        [Fact]
        public async Task UpdateCarBrand_ShouldUpdateBrand_WhenExists()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            var updatedBrand = new Domain.CarBrand
            {
                BrandName = "Toyota Updated",
                CountryOrigin = "Japan",
                Year = 1937
            };

            // Act
            var result = await repository.UpdateCarBrandAsync(1, updatedBrand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Toyota Updated", result.BrandName);

            var savedBrand = await context.CarBrands
                .FirstOrDefaultAsync(b => b.Id == 1);

            Assert.NotNull(savedBrand);
            Assert.Equal("Toyota Updated", savedBrand.BrandName);
        }

        [Fact]
        public async Task UpdateCarBrand_ShouldReturnNull_WhenDoesNotExist()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            var updatedBrand = new Domain.CarBrand
            {
                BrandName = "Unknown",
                CountryOrigin = "Unknown",
                Year = 2000
            };

            // Act
            var result = await repository.UpdateCarBrandAsync(99, updatedBrand);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteCarBrand_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            // Act
            var result = await repository.DeleteCarBrandAsync(1);

            // Assert
            Assert.True(result);

            var deletedBrand = await context.CarBrands
                .FirstOrDefaultAsync(b => b.Id == 1);

            Assert.Null(deletedBrand);
        }

        [Fact]
        public async Task DeleteCarBrand_ShouldReturnFalse_WhenDoesNotExist()
        {
            // Arrange
            using var context = await CreateContextAsync();
            var repository = new CarBrandRepository(context);

            // Act
            var result = await repository.DeleteCarBrandAsync(99);

            // Assert
            Assert.False(result);
        }
    }
}
