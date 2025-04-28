using ApiSrez.DataBaseContext;
using ApiSrez.Interfaces;
using ApiSrez.Model;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiSrez.Service
{
    public class GamesOrderService : IGamesOrderService
    {
        private readonly ContextDb _context;

        public GamesOrderService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetAllGamesAsync()
        {
            var games = await _context.Games.ToListAsync();

            return new OkObjectResult(new
            {
                data = new { games = games },
                status = true
            });
        }
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = from user in _context.Users
                         join order in _context.Order on user.id_User equals order.User_id
                         join games in _context.Games on order.Game_id equals games.id_Game
                         select new
                                  {
                                      user.Name,
                                      user.Email,
                                      games.NameGame,
                                      order.TimeStart,
                                      order.TimeFinish
                                  };

            return new OkObjectResult(orders.ToList());
        }

        public async Task<IActionResult> CreateOrderAsync([FromBody] string email, string namegame, DateTime timestart, DateTime timefinish)
        {
            var checkemail = await _context.Users.FirstOrDefaultAsync(a => a.Email == email);
            var checkgame = await _context.Games.FirstOrDefaultAsync(a => a.NameGame == namegame);

            var isOverlapping = await _context.Order
                .AnyAsync(order =>
                    order.Game_id == checkgame.id_Game &&
                    order.TimeStart < timefinish &&
                    order.TimeFinish > timestart);

            if (checkemail is null)
            {
                return new BadRequestObjectResult("Такой почты не зарегистрировано");
            }
            else if (checkgame is null)
            {
                return new BadRequestObjectResult("Такой игры не существует");
            }
            else if (isOverlapping == true)
            {
                return new BadRequestObjectResult("Игра уже забронирована в это время.");
            }
            else
            {
                try
                {
                    var createorder = new Order()
                    {
                        User_id = checkemail.id_User,
                        Game_id = checkgame.id_Game,
                        TimeStart = timestart,
                        TimeFinish = timefinish
                    };

                    // Сохраняем заказ в базу данных
                    _context.Order.Add(createorder);
                    await _context.SaveChangesAsync();

                    return new OkObjectResult(new { status = true });
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(new { status = false, ex });
                }
            }
        }

        public async Task<IActionResult> EditOrderAsync([FromBody] Order orderуedit)
        {
            var checkorder = await _context.Order.FirstOrDefaultAsync(a => a.id_Order == orderуedit.id_Order);

            if (orderуedit.TimeStart >= orderуedit.TimeFinish)
            {
                return new BadRequestObjectResult("Время начала должно быть меньше времени окончания.");
            }

            var isOverlapping = await _context.Order
                                .AnyAsync(order =>
                                order.id_Order != orderуedit.id_Order &&
                                order.Game_id == orderуedit.Game_id &&
                                order.TimeStart < orderуedit.TimeFinish &&
                                order.TimeFinish > orderуedit.TimeStart);

            if (isOverlapping == true)
            {
                return new BadRequestObjectResult("Игра уже забронирована в это время.");
            }
            else
            {
                try
                {
                    checkorder.User_id = orderуedit.User_id;
                    checkorder.Game_id = orderуedit.Game_id;
                    checkorder.TimeStart = orderуedit.TimeStart;
                    checkorder.TimeFinish = orderуedit.TimeFinish;

                    await _context.SaveChangesAsync();

                    return new OkObjectResult(new { status = true });
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(new { status = false, ex });
                }
            }
        }

        public async Task<IActionResult> GetOrderAsync([FromQuery] int Id)
        {
            var order = await _context.Order.FirstOrDefaultAsync(a => a.id_Order == Id);

            if (order != null) return new OkObjectResult(new { status = true, order });
            else return new BadRequestObjectResult(new { status = false });
        }
        public async Task<IActionResult> DeleteOrderAsync([FromQuery] int Id)
        {
            var order = await _context.Order.FirstOrDefaultAsync(a => a.id_Order == Id);

            if (order is null)
            {
                return new NotFoundObjectResult(new { status = false, MessageContent = "Заказ не найден" });
            }

            _context.Remove(order);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new { status = true });
        }
    }
}
