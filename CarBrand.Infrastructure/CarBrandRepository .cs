using CarBrand.Application;
using Microsoft.EntityFrameworkCore;
using Npgsql;



namespace CarBrand.Infrastructure
{
    public class CarBrandRepository : ICarBrandRepository
    {
        private readonly AppDbContext _context;

        public CarBrandRepository(AppDbContext context)
        {
            _context = context;
        }

        private static bool IsUniqueNameViolation(DbUpdateException exception)
        {
            return exception.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation
            };
        }

        public async Task<PagedResult<Domain.CarBrand>> GetAllCarBrandsAsync(
            int page,
            int pageSize)
        {
            var query = _context.CarBrands
                .AsNoTracking()
                .OrderBy(b => b.Id);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Domain.CarBrand>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<Domain.CarBrand?> GetCarBrandByIdAsync(int id)
        {
            return await _context.CarBrands
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            int? excludingId = null)
        {
            var normalizedName = name.Trim().ToLower();

            var query = _context.CarBrands
                .AsNoTracking()
                .Where(b => b.BrandName.ToLower() == normalizedName);

            if (excludingId.HasValue)
            {
                query = query.Where(b => b.Id != excludingId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Domain.CarBrand> AddNewCarBrandAsync(Domain.CarBrand carBrand)
        {
            _context.CarBrands.Add(carBrand);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueNameViolation(ex))
            {
                throw new CarBrandNameAlreadyExistsException(carBrand.BrandName);
            }

            return carBrand;
        }

        public async Task<Domain.CarBrand?> UpdateCarBrandAsync(
            int id,
            Domain.CarBrand carBrand)
        {
            var existing = await _context.CarBrands
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existing is null)
            {
                return null;
            }

            existing.BrandName = carBrand.BrandName;
            existing.CountryOrigin = carBrand.CountryOrigin;
            existing.Year = carBrand.Year;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueNameViolation(ex))
            {
                throw new CarBrandNameAlreadyExistsException(carBrand.BrandName);
            }

            return existing;
        }

        public async Task<bool> DeleteCarBrandAsync(int id)
        {
            var existing = await _context.CarBrands
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existing is null)
            {
                return false;
            }

            _context.CarBrands.Remove(existing);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
