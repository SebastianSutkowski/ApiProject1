using _ApiProject1_.Models;
using _ApiProject1_.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace _ApiProject1_.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;

        }
        [HttpGet]
        //[Authorize(Policy = "IsOver18")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery]RestaurantQuery query )
        {
            var restaurants = _restaurantService.GetAll(query);
            return Ok(restaurants);
        }
        [HttpGet("{Id}")]
        [Authorize(Roles ="Manager,Admin")]
        public ActionResult<IEnumerable<RestaurantDto>> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);



            return Ok(restaurant);
        }
        [HttpDelete("{Id}")]
        public ActionResult<IEnumerable<RestaurantDto>> Delete([FromRoute] int id)
        {
            _restaurantService.DeleteRestaurant(id);

            return NoContent();

        }
        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(e => e.Type == ClaimTypes.NameIdentifier).Value);
            var restaurantId = _restaurantService.CreateRestaurant(dto);
            return Created($"/api/restaurant/{restaurantId}", null);
        }
        [HttpPut]
        public ActionResult Modify([FromBody] ModifyRestaurantDto dto)
        {
            
            _restaurantService.ModifyRestaurant(dto);
            return Ok();
        }
    }
}
