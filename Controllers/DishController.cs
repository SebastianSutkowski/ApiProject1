using _ApiProject1_.Models;
using _ApiProject1_.Services;
using Microsoft.AspNetCore.Mvc;
namespace _ApiProject1_.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;

        }
        [HttpPost]
        public ActionResult Post([FromRoute]int restaurantId, [FromBody]CreateDishDto dto)
        {
            var dishId=_dishService.Create(restaurantId, dto);
            return Created($"api/restaurant/{restaurantId}/dish/{dishId}",null);
        }
        [HttpGet("{Id}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int Id)
        {
            var dish = _dishService.GetById(restaurantId, Id);
            return Ok(dish);
        }
        [HttpGet]
        public ActionResult <IEnumerable<DishDto>> GetAllDishes([FromRoute] int restaurantId)
        {
            var dishes = _dishService.GetDishes(restaurantId);
            return Ok(dishes);
        }
        [HttpDelete("{dishId}")]
        public ActionResult DeleteDish([FromRoute] int restaurantId, [FromRoute]int dishId)
        {
            _dishService.DeleteDish(restaurantId,dishId);
            return Ok();
        }



    }
}
