using ApiSrez.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiSrez.DataBaseContext;
using ApiSrez.Interfaces;
using ApiSrez.Requests;

namespace ApiSrez.Service
{
    public class UsersService : IUsersService
    {
        private readonly ContextDb _context;

        public UsersService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return new OkObjectResult(new
            {
                data = new { users = users },
                status = true
            });
        }

        public class CreateUser
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public bool isAdmin { get; set; }
        }
        public async Task<IActionResult> CreateNewUserAsync([FromBody] CreateUser newUser)
        {
            var logincheck = await _context.Users.FirstOrDefaultAsync(a => a.Login == newUser.Login);
            var emailcheck = await _context.Users.FirstOrDefaultAsync(a => a.Email == newUser.Email);

            if (emailcheck is null || logincheck is null)
            {
                var createuser = new Users()
                {
                    Name = newUser.Name,
                    LastName = newUser.LastName,
                    isAdmin = newUser.isAdmin,
                    Login = newUser.Login,
                    Password = newUser.Password,
                    Email = newUser.Email
                };

                await _context.Users.AddAsync(createuser);
                await _context.SaveChangesAsync();


                var log = await _context.Users.FirstOrDefaultAsync(a => a.Login == newUser.Login && a.Password == newUser.Password);
                if (log is null)
                {
                    return new UnauthorizedObjectResult(new { status = false });
                }

                else
                {
                    var claims = new List<Claim>
                        {
                            new("id_User", Convert.ToString(log.id_User)),
                            new("Name", log.Name),
                            new("LastName", log.LastName),
                            new("Login", log.Login),
                            new("Password", log.Password),
                            new("Email", log.Email),
                            new("isAdmin", Convert.ToString(log.isAdmin))
                        };
                    var claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);

                    var now = DateTime.UtcNow;
                    var jwt = new JwtSecurityToken(
                            issuer: "APIServer",
                            audience: "BlazorApp",
                            notBefore: now,
                            claims: claims,
                            expires: now.Add(TimeSpan.FromMinutes(10)),
                            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("UgZd07mr8o5gvtFUUUGcjT4e8q08mEuB")), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    return new OkObjectResult(new { status = true, token = encodedJwt });
                }
            }
            return new BadRequestObjectResult(new { status = false });
        }

        public async Task<IActionResult> AuthorizationAsync([FromBody] UserAuth userAuth)
        {
            var login = await _context.Users.FirstOrDefaultAsync(a => a.Login == userAuth.Login && a.Password == userAuth.Password);
            if (login is null)
            {
                return new UnauthorizedObjectResult(new { status = false });
            }

            else
            {
                var claims = new List<Claim>
                {
                     new("id_User", Convert.ToString(login.id_User)),
                            new("Name", login.Name),
                            new("LastName", login.LastName),
                            new("Login", login.Login),
                            new("Password", login.Password),
                            new("Email", login.Email),
                            new("isAdmin", Convert.ToString(login.isAdmin))
                };
                var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                        issuer: "APIServer",
                        audience: "BlazorApp",
                        notBefore: now,
                        claims: claims,
                        expires: now.Add(TimeSpan.FromMinutes(10)),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("UgZd07mr8o5gvtFUUUGcjT4e8q08mEuB")), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new OkObjectResult(new { status = true, token = encodedJwt });
            }
        }

        public async Task<IActionResult> DeleteUserAsync(int Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.id_User == Id);

            if (user is null)
            {
                return new NotFoundObjectResult(new { status = false, MessageContent = "Пользователь не найден" });
            }

            try
            {
                _context.Remove(user);
            }
            catch { return new NotFoundObjectResult(new { status = false, MessageContent = "Логин пользователя не найден" }); }

            await _context.SaveChangesAsync();
            return new OkObjectResult(new { status = true });
        }

        public async Task<IActionResult> EditUserAsync([FromBody] Users userInfo)
        {
            var checkuser = await _context.Users.FirstOrDefaultAsync(s => s.id_User == userInfo.id_User);

            if (checkuser != null)
            {
                checkuser.Name = userInfo.Name;
                checkuser.LastName = userInfo.LastName;
                checkuser.Login = userInfo.Login;
                checkuser.Password = userInfo.Password;
                checkuser.Email = userInfo.Email;
                checkuser.isAdmin = userInfo.isAdmin;

                await _context.SaveChangesAsync();
                return new OkObjectResult(new { status = true, checkuser });
            }
            else
            {
                return new BadRequestObjectResult(new { status = false });
            }
        }

        public async Task<IActionResult> GetUserAsync([FromQuery] int Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.id_User == Id);

            if (user != null) return new OkObjectResult(new { status = true, user });
            else return new BadRequestObjectResult(new { status = false });
        }

    }
}
