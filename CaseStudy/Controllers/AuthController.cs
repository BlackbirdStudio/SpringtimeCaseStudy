using CaseStudy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaseStudy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private SpringTimeConfiguration _configuration;
        public AuthController(SpringTimeConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetDataManagerJwt()
        {
            var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration.Jwt.Subject),
            new Claim(JwtRegisteredClaimNames.Iss, _configuration.Jwt.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration.Jwt.Audience),
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds().ToString()),
            new Claim("role", Roles.ManageData)
        };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Jwt.Key));
            var token = new JwtSecurityToken(
                issuer: _configuration.Jwt.Issuer,
                audience: _configuration.Jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );

            var result = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(result);
        }
    }
}
