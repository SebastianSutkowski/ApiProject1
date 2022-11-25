using _ApiProject1_.Models;
using _ApiProject1_.Exceptions;
using _ApiProject1_.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
namespace _ApiProject1_.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById(int restaurantId, int dishId);
        IEnumerable<DishDto> GetDishes(int restaurantId);
        void DeleteDish(int restaurantId, int dishId);
    }
    public class DishService:IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public DishService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null)
                throw new NotFoundException("Restaurant not nound");

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;
            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();
            return dish.Id;
        }
        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = _dbContext
               .Restaurants
               .Include(r => r.Adress)
               .Include(r => r.Dishes)
               .FirstOrDefault(e => e.Id == restaurantId);
            var dishEntity = restaurant.Dishes.FirstOrDefault(d=>d.Id == dishId);
            var dish = _mapper.Map<DishDto>(dishEntity);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not Found");
            if (dishEntity is null)
                throw new NotFoundException("Dish not Found");
            return dish;

        }
        public IEnumerable<DishDto> GetDishes(int restaurantId)
        {
            var restaurant = _dbContext
               .Restaurants
               .Include(r => r.Adress)
               .Include(r => r.Dishes)
               .FirstOrDefault(e => e.Id == restaurantId);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not Found");
            var dishesEntity = restaurant.Dishes.ToList();
            var dishes = _mapper.Map<List<DishDto>>(dishesEntity);
            
            if (dishesEntity is null)
                throw new NotFoundException("Dishes not Found");
            return dishes;

        }
        public void DeleteDish(int restaurantId, int dishId)
        {
            var restaurant = _dbContext.Restaurants.Include(r => r.Dishes).FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null)
                throw new NotFoundException("Restaurant not nound");

            var dishEntity = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dishEntity is null)
                throw new NotFoundException("Dish not Found");
            _dbContext.Dishes.Remove(dishEntity);
            _dbContext.SaveChanges();
            
        }
    }
}
