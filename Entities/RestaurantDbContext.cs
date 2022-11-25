using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace _ApiProject1_.Entities
    
{
    public class RestaurantDbContext :DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {

        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
            
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);
            

            modelBuilder.Entity<Dish>()
                .Property(d => d.Name)
                .IsRequired();
            modelBuilder.Entity<Adress>()
                .Property(r => r.City)
                .HasMaxLength(50);
            modelBuilder.Entity<Adress>()
                .Property(r => r.Street)
                .HasMaxLength(50);
            modelBuilder.Entity<User>()
               .Property(r => r.Email)
               .IsRequired();
            modelBuilder.Entity<Role>()
               .Property(r => r.Name)
               .IsRequired();
        }
        
    }
}
