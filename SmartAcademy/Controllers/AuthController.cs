using Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartAcademy.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            // Step 1: Validate user (replace with DB validation later)
            var user = ValidateUser(dto.Username, dto.Password);
            if (user == null)
                return Unauthorized("Invalid username or password");

            // Step 2: Create JWT token
            var token = GenerateToken(user);

            return Ok(new { token });
        }

        // TEMPORARY METHOD — replace with DB lookup later
        private dynamic ValidateUser(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                return new
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@gmail.com"
                };
            }
            return null;
        }

        private string GenerateToken(dynamic user)
        {
            var jwt = _config.GetSection("Jwt");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims
            var claims = new[]
            {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("email", user.Email)
        };

            // Create JWT Token object
            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresInMinutes"]!)),
                signingCredentials: creds
            );

            // Convert token object → string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
