using CarBrand.Api.Controllers;
using CarBrand.Api.Dtos;
using CarBrand.Application;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBrand.Tests.Controllers
{
    public class CarBrandControllerTests
    {
        private readonly Mock<ICarBrandService> _serviceMock;
        private readonly CarBrandController _controller;

        public CarBrandControllerTests()
        {
            _serviceMock = new Mock<ICarBrandService>();

            _controller = new CarBrandController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithCarBrands()
        {
            // Arrange
            var brands = new List<Domain.CarBrand>
        {
            new Domain.CarBrand
            {
                Id = 1,
                BrandName = "Toyota",
                CountryOrigin = "Japan",
                Year = 1937
            },
            new Domain. CarBrand
            {
                Id = 2,
                BrandName = "Ford",
                CountryOrigin = "USA",
                Year = 1903
            },
            new Domain.CarBrand
            {
                Id = 3,
                BrandName = "Volkswagen",
                CountryOrigin = "Germany",
                Year = 1937
            }
        };

            var pagedResult = new PagedResult<Domain.CarBrand>
            {
                Items = brands,
                Page = 1,
                PageSize = 10,
                TotalCount = 3
            };

            _serviceMock
                .Setup(service => service.GetAllCarBrandsAsync(1, 10))
                .ReturnsAsync(pagedResult);

            var query = new CarBrandQueryParameters
            {
                Page = 1,
                PageSize = 10
            };

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<PagedResponse<CarBrandResponse>>(
                okResult.Value
            );

            Assert.Equal(1, response.Page);
            Assert.Equal(10, response.PageSize);
            Assert.Equal(3, response.TotalCount);
            Assert.Equal(3, response.Items.Count());

            _serviceMock.Verify(
                service => service.GetAllCarBrandsAsync(1, 10),
                Times.Once
            );
        }


        [Fact]
        public async Task GetById_ShouldReturnOk_WhenCarBrandExists()
        {
            // Arrange
            var brand = new Domain.CarBrand
            {
                Id = 1,
                BrandName = "Toyota",
                CountryOrigin = "Japan",
                Year = 1937
            };

            _serviceMock
                .Setup(service => service.GetCarBrandByIdAsync(1))
                .ReturnsAsync(brand);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<CarBrandResponse>(okResult.Value);

            Assert.Equal(1, response.Id);
            Assert.Equal("Toyota", response.Name);
            Assert.Equal("Japan", response.Country);
            Assert.Equal(1937, response.Year);

            _serviceMock.Verify(
                service => service.GetCarBrandByIdAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenCarBrandDoesNotExist()
        {
            // Arrange
            _serviceMock
                .Setup(service => service.GetCarBrandByIdAsync(99))
                .ReturnsAsync((Domain.CarBrand?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

            _serviceMock.Verify(
                service => service.GetCarBrandByIdAsync(99),
                Times.Once
            );
        }


        [Fact]
        public async Task Create_ShouldReturnCreated_WhenCarBrandIsValid()
        {
            // Arrange
            var request = new CreateCarBrandRequest
            {
                Name = "BMW",
                Country = "Germany",
                Year = 1916
            };

            var createdBrand = new Domain.CarBrand
            {
                Id = 4,
                BrandName = "BMW",
                CountryOrigin = "Germany",
                Year = 1916
            };

            _serviceMock
                .Setup(service => service.CreateCarBrandAsync(
                    It.IsAny<Domain.CarBrand>()))
                .ReturnsAsync(createdBrand);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            var response = Assert.IsType<CarBrandResponse>(createdResult.Value);

            Assert.Equal(4, response.Id);
            Assert.Equal("BMW", response.Name);

            _serviceMock.Verify(
                service => service.CreateCarBrandAsync(
                    It.Is<Domain.CarBrand>(b =>
                        b.BrandName == "BMW" &&
                        b.CountryOrigin == "Germany" &&
                        b.Year == 1916)),
                Times.Once
            );
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenCarBrandExists()
        {
            // Arrange
            var request = new UpdateCarBrandRequest
            {
                Name = "Toyota Updated",
                Country = "Japan",
                Year = 1937
            };

            var updatedBrand = new Domain.CarBrand
            {
                Id = 1,
                BrandName = "Toyota Updated",
                CountryOrigin = "Japan",
                Year = 1937
            };

            _serviceMock
                .Setup(service => service.UpdateCarBrandAsync(
                    1, It.IsAny<Domain.CarBrand>()))
                .ReturnsAsync(updatedBrand);

            // Act
            var result = await _controller.Update(1, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<CarBrandResponse>(okResult.Value);

            Assert.Equal(1, response.Id);
            Assert.Equal("Toyota Updated", response.Name);

            _serviceMock.Verify(
                service => service.UpdateCarBrandAsync(
                    1, It.IsAny<Domain.CarBrand>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenCarBrandDoesNotExist()
        {
            // Arrange
            var request = new UpdateCarBrandRequest
            {
                Name = "Toyota Updated",
                Country = "Japan",
                Year = 1937
            };

            _serviceMock
                .Setup(service => service.UpdateCarBrandAsync(
                    99, It.IsAny<Domain.CarBrand>()))
                .ReturnsAsync((Domain.CarBrand?)null);

            // Act
            var result = await _controller.Update(99, request);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenCarBrandExists()
        {
            // Arrange
            _serviceMock
                .Setup(service => service.DeleteCarBrandAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            _serviceMock.Verify(
                service => service.DeleteCarBrandAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenCarBrandDoesNotExist()
        {
            // Arrange
            _serviceMock
                .Setup(service => service.DeleteCarBrandAsync(99))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _serviceMock.Verify(
                service => service.DeleteCarBrandAsync(99),
                Times.Once
            );
        }


    }
}
