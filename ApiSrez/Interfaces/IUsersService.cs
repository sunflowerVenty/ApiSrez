using Microsoft.AspNetCore.Mvc;
using ApiSrez.DataBaseContext;
using ApiSrez.Model;
using static ApiSrez.Service.UsersService;
using ApiSrez.Requests;

namespace ApiSrez.Interfaces
{
    public interface IUsersService
    {
        Task<IActionResult> GetAllUsersAsync();
        Task<IActionResult> CreateNewUserAsync([FromBody] CreateUser newUser);
        Task<IActionResult> AuthorizationAsync([FromBody] UserAuth userAuth);
        Task<IActionResult> DeleteUserAsync(int Id);
        Task<IActionResult> EditUserAsync([FromBody] Users userInfo);
        Task<IActionResult> GetUserAsync([FromQuery] int Id);
    }
}
