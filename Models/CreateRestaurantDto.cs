using System.ComponentModel.DataAnnotations;
namespace _ApiProject1_.Models
{
    public class CreateRestaurantDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; }

        public string ContactNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        public int PostalCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string Street { get; set; }


    }
}
