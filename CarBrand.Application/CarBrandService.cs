using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBrand.Application
{
    public class CarBrandService : ICarBrandService
    {
        private readonly ICarBrandRepository _repository;
        public CarBrandService(ICarBrandRepository repository)
        {
            _repository = repository;
        }

        public async Task<Domain.CarBrand> CreateCarBrandAsync(Domain.CarBrand carBrand)
        {
            if (await _repository.ExistsByNameAsync(carBrand.BrandName))
            {
                throw new CarBrandNameAlreadyExistsException(carBrand.BrandName);
            }

            return await _repository.AddNewCarBrandAsync(carBrand);
        }

        public Task<bool> DeleteCarBrandAsync(int id)
        {
            return _repository.DeleteCarBrandAsync(id);
        }

        public Task<PagedResult<Domain.CarBrand>> GetAllCarBrandsAsync(int page, int pageSize)
        {
            return _repository.GetAllCarBrandsAsync(page, pageSize);
        }

        public Task<Domain.CarBrand?> GetCarBrandByIdAsync(int id)
        {
            return _repository.GetCarBrandByIdAsync(id);
        }

        public async Task<Domain.CarBrand?> UpdateCarBrandAsync(int id, Domain.CarBrand carBrand)
        {
            if (await _repository.ExistsByNameAsync(carBrand.BrandName, excludingId: id))
            {
                throw new CarBrandNameAlreadyExistsException(carBrand.BrandName);
            }

            return await _repository.UpdateCarBrandAsync(id, carBrand);
        }
    }
}
