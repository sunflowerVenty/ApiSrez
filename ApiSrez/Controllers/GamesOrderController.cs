using ApiSrez.Interfaces;
using ApiSrez.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiSrez.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesOrderController : ControllerBase
    {
        private readonly IGamesOrderService _orderService;

        public GamesOrderController(IGamesOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("getAllGames")]
        public async Task<IActionResult> GetAllGames()
        {
            return await _orderService.GetAllGamesAsync();
        }

        [HttpGet]
        [Route("getAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            return await _orderService.GetAllOrdersAsync();
        }

        [HttpGet]
        [Route("getOrder")]
        public async Task<IActionResult> GetOrder([FromQuery] int Id)
        {
            return await _orderService.GetOrderAsync(Id);
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] string email, string namegame, DateTime timestart, DateTime timefinish)
        {
            return await _orderService.CreateOrderAsync(email, namegame, timestart, timefinish);
        }

        [HttpPut]
        [Route("EditOrder")]
        public async Task<IActionResult> EditOrder([FromBody] Order orderуedit)
        {
            return await _orderService.EditOrderAsync(orderуedit);
        }


        [HttpDelete]
        [Route("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder([FromQuery] int Id)
        {
            return await _orderService.DeleteOrderAsync(Id);
        }
    }
}
