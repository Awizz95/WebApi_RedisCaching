using Microsoft.EntityFrameworkCore;
using WebApi_RedisCaching.DataService;
using WebApi_RedisCaching.Models;

namespace WebApi_RedisCaching.Services
{
    public class CarService : ICarService
    {
        private readonly ApiDbContext _dbContext;

        public CarService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _dbContext.Cars.ToListAsync();
        }

        public async Task<Car>? GetCarByIdAsync(int id)
        {
            return await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Car> AddCarAsync(Car car)
        {
            _dbContext.Cars.Add(car);
            await _dbContext.SaveChangesAsync();

            return car;
        }

        public async Task<Car> UpdateCarAsync(int id, Car newCar)
        {
            var car = await GetCarByIdAsync(id);

            if (car is null)
            {
                return null;
            }

            car.Year = newCar.Year;
            car.Make = newCar.Make;
            car.Model = newCar.Model;
            car.Color = newCar.Color;

            await _dbContext.SaveChangesAsync();

            return car;
        }

        public async Task<Car> DeleteCarAsync(int id)
        {
            var car = await GetCarByIdAsync(id);

            if (car is null)
            {
                return null;
            }

            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();

            return car;
        }
    }
}
