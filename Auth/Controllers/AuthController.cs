using Auth.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public IActionResult Get([FromBody] LoginDto login)
        {
            if (login.UserName != "admin" || login.Password != "123456")
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("QXJxdWl0ZWN0dXJhRGVNaWNyb1NlcnZpY2lvc0xhTWVqb3JUaWVuZGFEZUFhc2NvdGFzMjAyNCo=");
            var issuer = "petstore.com";
            var audience = "Public";
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, login.UserName!),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenValue = tokenHandler.WriteToken(token);
            var response = new TokenDto { Token = tokenValue, Expires = tokenDescriptor.Expires };
            return Ok(response);
        }
    }
}