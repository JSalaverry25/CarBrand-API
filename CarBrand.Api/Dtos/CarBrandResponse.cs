using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBrand.Api.Dtos
{
    public class CarBrandResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Year { get; set; }
    }

    public static class CarBrandMappingExtensions
    {
        public static CarBrandResponse ToResponse(this Domain.CarBrand carBrand)
        {
            return new CarBrandResponse
            {
                Id = carBrand.Id,
                Name = carBrand.BrandName,
                Country = carBrand.CountryOrigin,
                Year = carBrand.Year
            };
        }
    }
}
