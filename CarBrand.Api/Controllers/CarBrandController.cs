using CarBrand.Api.Dtos;
using CarBrand.Application;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarBrand.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarBrandController : ControllerBase
    {
        private readonly ICarBrandService _service;

        public CarBrandController(ICarBrandService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<CarBrandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResponse<CarBrandResponse>>> GetAll([FromQuery] CarBrandQueryParameters query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.GetAllCarBrandsAsync(query.Page, query.PageSize);

            return Ok(new PagedResponse<CarBrandResponse>
            {
                Items = result.Items.Select(b => b.ToResponse()).ToList(),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            });
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CarBrandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarBrandResponse>> GetById(int id)
        {
            var brand = await _service.GetCarBrandByIdAsync(id);
            if (brand is null)
            {
                return NotFound();
            }

            return Ok(brand.ToResponse());
        }

        [HttpPost]
        [ProducesResponseType(typeof(CarBrandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CarBrandResponse>> Create([FromBody] CreateCarBrandRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carBrand = new Domain.CarBrand
            {
                BrandName = request.Name,
                CountryOrigin = request.Country,
                Year = request.Year
            };

            var created = await _service.CreateCarBrandAsync(carBrand);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CarBrandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CarBrandResponse>> Update(int id, [FromBody] UpdateCarBrandRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carBrand = new Domain.CarBrand
            {
                BrandName = request.Name,
                CountryOrigin = request.Country,
                Year = request.Year
            };

            var updated = await _service.UpdateCarBrandAsync(id, carBrand);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated.ToResponse());
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteCarBrandAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
