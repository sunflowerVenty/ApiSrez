using ApiSrez.Interfaces;
using ApiSrez.Model;
using ApiSrez.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ApiSrez.Service.UsersService;

namespace ApiSrez.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpPost]
        [Route("createNewUserAndLogin")]
        public async Task<IActionResult> CreateNewUserAndLogin([FromBody] CreateUser newUser)
        {
            return await _userService.CreateNewUserAsync(newUser);
        }

        [HttpPost]
        [Route("Authorization")]
        public async Task<IActionResult> Authorization([FromBody] UserAuth userAuth)
        {
            return await _userService.AuthorizationAsync(userAuth);
        }

        [HttpDelete]
        [Route("DeleteUser/{Id}")]
        public async Task<IActionResult> DeleteUer(int Id)
        {
            return await _userService.DeleteUserAsync(Id);
        }

        [HttpPut]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] Users userInfo)
        {
            return await _userService.EditUserAsync(userInfo);
        }

        [HttpGet]
        [Route("GetUser/{Id}")]
        public async Task<IActionResult> GetUser([FromQuery] int Id)
        {
            return await _userService.GetUserAsync(Id);
        }
    }
}
