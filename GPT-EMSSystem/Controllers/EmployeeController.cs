using GPT_EMSSystem.Data;
using GPT_EMSSystem.Models;
using GPT_EMSSystem.Models.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GPT_EMSSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class EmployeeController : Controller
    {
        private readonly EmployeeManagementDbContext _dbContext;
        //private readonly IPasswordHasher<Employee> _passwordHasher;

        public EmployeeController(EmployeeManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        // [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateModel model)
        {
            var existing =  await _dbContext.Employees.Where(x => x.Email == model.Email).FirstOrDefaultAsync();

            if (existing != null)
            {
                return BadRequest("Email address is already in use");
            }

            var department = await _dbContext.Departments.FindAsync(model.DepartmentId);
            if (department == null)
            {
                return BadRequest("Invalid department ID");
            }

            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                DepartmentId = model.DepartmentId,
                Role = model.Role,
                Phone = model.Phone
            };

            // Manually generate a password for the employee
            // var hashedPassword = user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);//_passwordHasher.HashPassword(employee, GeneratePassword());

            employee.Password = GeneratePassword();

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return Ok(employee);
        }

        private string GeneratePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }

        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.Phone = model.Phone;
            employee.DepartmentId = model.DepartmentId;

            _dbContext.Entry(employee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


    }
}
