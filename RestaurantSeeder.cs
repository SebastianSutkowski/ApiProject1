using _ApiProject1_.Entities;
using Microsoft.EntityFrameworkCore;
namespace _ApiProject1_
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations()
                if(pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }
                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants()
                    _dbContext.Restaurants.AddRange(restaurants)
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {


            new Restaurant()
             {
                Name = "mc",
                Category = "fast food",
                Description = "mak donald sdasd",
                ContactEmail = "mak@mak.pl",
                HasDelivery = false,
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name="makrela",
                        Price=3.40M,
                    },
                    new Dish()
                    {
                        Name="makrale",
                        Price=2.50M,
                    },
                },
                Adress = new Adress()
                {
                    City = "bialystok",
                    Street = "kaczorowskiego",
                    PostalCode = 15302
                }
            },
            new Restaurant()
            {
                Name = "kfc",
                Category = "fast food",
                Description = "kurczaczki",
                ContactEmail = "kfc@kfc.pl",
                HasDelivery = false,
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name="longier",
                        Price=3.40M,
                    },
                    new Dish()
                    {
                        Name="frytki",
                        Price=2.50M,
                    },
                },
                Adress = new Adress()
                {
                    City = "bialystok",
                    Street = "muranskiego",
                    PostalCode = 15302
                }
              }
            };
            return restaurants;

        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {


                new Role()
                {
                 Name="User"

                },
            new Role()
            {
                Name = "Manager"
            },
                new Role()
                {
                 Name="Admin"
                },
             };
            return roles;

        }
}
}
