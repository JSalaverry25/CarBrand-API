using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBrand.Application
{
    public interface ICarBrandService
    {
        Task<PagedResult<Domain.CarBrand>> GetAllCarBrandsAsync(int page, int pageSize);
        Task<Domain.CarBrand?> GetCarBrandByIdAsync(int id);
        Task<Domain.CarBrand> CreateCarBrandAsync(Domain.CarBrand carBrand);
        Task<Domain.CarBrand?> UpdateCarBrandAsync(int id, Domain.CarBrand carBrand);
        Task<bool> DeleteCarBrandAsync(int id);
    }
}
