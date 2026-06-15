using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;
using Microsoft.EntityFrameworkCore;
using StudentAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly StudentDBContext context;
        private readonly IConfiguration configuration;

        public AuthController(StudentDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register register)
        {
            if (await context.Users.AnyAsync(x => x.Email == register.Email))
            {
                return BadRequest("Email already exists");
            }

            var user = new User
            {
                Name = register.Name,
                Email = register.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = "User"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return Ok(new
            {
                Message = "User Registered Successfully"
            });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Email or Password"
                });
            }

            bool validPassword =
                BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

            if (!validPassword)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Email or Password"
                });
            }

            var token = GenerateToken(user);

            return Ok(new
            {
                Token = token,
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(configuration["Jwt:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}