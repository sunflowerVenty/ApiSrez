using ApiSrez.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiSrez.Interfaces
{
    public interface IGamesOrderService
    {
        Task<IActionResult> GetAllGamesAsync();
        Task<IActionResult> GetAllOrdersAsync();
        Task<IActionResult> CreateOrderAsync([FromBody] string email, string namegame, DateTime timestart, DateTime timefinish);
        Task<IActionResult> EditOrderAsync([FromBody] Order orderуedit);
        Task<IActionResult> GetOrderAsync([FromQuery] int Id);
        Task<IActionResult> DeleteOrderAsync([FromQuery] int Id);

    }
}
