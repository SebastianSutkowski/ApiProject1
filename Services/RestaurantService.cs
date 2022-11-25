using _ApiProject1_.Models;
using _ApiProject1_.Models.Validators;
using _ApiProject1_.Exceptions;
using _ApiProject1_.Entities;
using _ApiProject1_.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data;
using System.Linq.Expressions;
namespace _ApiProject1_.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        PageResult<RestaurantDto> GetAll(RestaurantQuery query);
        int CreateRestaurant(CreateRestaurantDto dto);
        void DeleteRestaurant(int id);
        void ModifyRestaurant(ModifyRestaurantDto dto);

    }
    public class RestaurantService: IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, 
            IMapper mapper, ILogger<RestaurantService> logger, 
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }
        public RestaurantDto GetById(int id)
        {
            
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Adress)
                .Include(r => r.Dishes)
                .FirstOrDefault(e => e.Id == id);
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not Found");
            }
            else
                return restaurantDto;
        }
        public PageResult<RestaurantDto> GetAll(RestaurantQuery query)
        {
            var allRestaurants = _dbContext
                .Restaurants
                .Include(r => r.Adress)
                .Include(r => r.Dishes)
                .Where(r => query.SearchPhrase == null || (r.Description.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Name.ToLower().Contains(query.SearchPhrase.ToLower())));
            if (query.OrberBy is not null)
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name),r=>r.Name },
                    {nameof(Restaurant.Description),r=>r.Description },
                    {nameof(Restaurant.Category),r=>r.Category },
                };
                var selectedItem = columnsSelector[query.OrberBy];
                allRestaurants = query.SortDirection == Direction.ASC 
                    ? allRestaurants.OrderBy(selectedItem) 
                    : allRestaurants.OrderByDescending(selectedItem);
            }
            var restaurants = allRestaurants 
                .Skip(query.PageSize*(query.PageNumber-1))
                .Take(query.PageSize)
                .ToList();
            var restaurantsOfUser = _dbContext.Restaurants.ToList().FindAll(r => r.CreatedById == _userContextService.GetUserId).Count();
            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurants, new MinTwoRestaurantsUser(restaurantsOfUser)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }
            
            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
            var result = new PageResult<RestaurantDto>(restaurantsDtos, query.PageSize, query.PageNumber,allRestaurants.Count());
            return result;
        }
        public int CreateRestaurant(CreateRestaurantDto dto)
        {

            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();
            return restaurant.Id;
            
        }
        public void DeleteRestaurant(int id)
        {
            _logger.LogError($"Restaurant with id:{id} deleted");
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(e => e.Id == id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not Found");
            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new OperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }
            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
            
        }
        public void ModifyRestaurant(ModifyRestaurantDto dto)
        {
            
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(e => e.Id == dto.Id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not Found");
            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new OperationRequirement(ResourceOperation.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }
            restaurant.Name = (dto.Name != null)?dto.Name:restaurant.Name;
            restaurant.Description = (dto.Description != null)?dto.Description:restaurant.Description;
            restaurant.HasDelivery = (dto.HasDelivery!=null)?dto.HasDelivery:restaurant.HasDelivery;
            _dbContext.Restaurants.Update(restaurant);
            _dbContext.SaveChanges();

        }
    }
}
