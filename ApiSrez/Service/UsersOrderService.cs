using ApiSrez.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestApi3K.DataBaseContext;

namespace ApiSrez.Service
{
    public class UsersOrderService
    {
        private readonly ContextDb _context;

        public UsersOrderService(ContextDb context)
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

        public async Task<IActionResult> GetAllGamesAsync()
        {
            var games = await _context.Games.ToListAsync();

            return new OkObjectResult(new
            {
                data = new { games = games },
                status = true
            });
        }

        public async Task<IActionResult> CreateNewUserAndLoginAsync([FromBody] CreateNewUserAndLogin newUser)
        {
            var emailcheck = await _context.Logins.FirstOrDefaultAsync(a => a.Email == newUser.Email);

            if (emailcheck == null)
            {
                var createuser = new Users()
                {
                    Name = newUser.Name,
                    AboutMe = newUser.AboutMe,
                    Admin = newUser.Admin
                };

                await _context.Users.AddAsync(createuser);
                await _context.SaveChangesAsync();

                var login = new Logins()
                {
                    User_id = createuser.id_User,
                    Email = newUser.Email,
                    Password = newUser.Password,
                };

                await _context.Logins.AddAsync(login);
                await _context.SaveChangesAsync();

                var log = await _context.Logins.FirstOrDefaultAsync(a => a.Email == newUser.Email && a.Password == newUser.Password);
                if (log is null)
                {
                    return new UnauthorizedObjectResult(new { status = false });
                }

                else
                {
                    var user = await _context.Users.FirstOrDefaultAsync(a => a.id_User == log.User_id);
                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new("id_User", Convert.ToString(user.id_User)),
                            new("Name", user.Name),
                            new("AboutMe", user.AboutMe),
                            new("isAdmin", Convert.ToString(user.Admin))
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
                    return new UnauthorizedObjectResult(new { status = false });
                }
            }
            else
            {
                return new UnauthorizedObjectResult(new { status = false });
            }
        }
    }
}
