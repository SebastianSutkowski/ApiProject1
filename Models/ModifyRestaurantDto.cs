using System.ComponentModel.DataAnnotations;
namespace _ApiProject1_.Models
{
    public class ModifyRestaurantDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }
    }
}
