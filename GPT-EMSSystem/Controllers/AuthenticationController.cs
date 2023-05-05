using GPT_EMSSystem.Data;
using GPT_EMSSystem.Models;
using GPT_EMSSystem.Models.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GPT_EMSSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {

        private readonly EmployeeManagementDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationController(EmployeeManagementDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            var employee = await _dbContext.Employees.SingleOrDefaultAsync(e => e.Email == model.Email);

            if (employee == null)
            {
                return Unauthorized();
            }

            //var result = _passwordHasher.VerifyHashedPassword(employee, employee.Password, model.Password);
            if (employee.Password != model.Password)
            {
                return Unauthorized();
            }

            var roles = new List<string>();
            if (!string.IsNullOrEmpty(employee.Role))
            {
                roles.Add(employee.Role);
            }

            var claims = new List<Claim>
            {
                new Claim("EmployeeId", employee.EmployeeId.ToString()),
                new Claim(ClaimTypes.Name, $"{employee.FirstName} {employee.LastName}"),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "Employee")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

    }
}
