using Microsoft.AspNetCore.Mvc;
using WebApi_RedisCaching.Models;
using WebApi_RedisCaching.Services;
using WebApi_RedisCaching.Services.Caching;

namespace WebApi_RedisCaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IRedisCacheService _cache;

        public CarsController(ICarService carService, IRedisCacheService cache)
        {
            _carService = carService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var userId = Request.Headers["UserId"]; //чтобы у каждого пользователя был свой кэш
            var cachingKey = $"cars{userId}";
            var cars = _cache.GetData<IEnumerable<Car>>(cachingKey);

            if (cars is not null)
            {
                return Ok(cars);
            }

            cars = await _carService.GetAllCarsAsync();
            _cache.SetData(cachingKey, cars);

            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);

            if (car is null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] Car car)
        {
            var createdCar = await _carService.AddCarAsync(car);

            return CreatedAtAction(nameof(GetCarById), new { id = createdCar.Id }, createdCar);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
        {
            var updatedCar = await _carService.UpdateCarAsync(id, car);

            if (updatedCar is null)
            {
                return NotFound();
            }

            return Ok(updatedCar);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var deletedCar = await _carService.DeleteCarAsync(id);

            if (deletedCar is null)
            {
                return NotFound();
            }

            return Ok(deletedCar);
        }
    }
}
