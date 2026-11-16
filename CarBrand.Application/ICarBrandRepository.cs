using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarBrand.Domain;

namespace CarBrand.Application
{
    public interface ICarBrandRepository
    {
        Task<PagedResult<Domain.CarBrand>> GetAllCarBrandsAsync(int page, int pageSize);
        Task<Domain.CarBrand?> GetCarBrandByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name, int? excludingId = null);
        Task<Domain.CarBrand> AddNewCarBrandAsync(Domain.CarBrand carBrand);
        Task<Domain.CarBrand?> UpdateCarBrandAsync(int id, Domain.CarBrand carBrand);
        Task<bool> DeleteCarBrandAsync(int id);
    }
}
