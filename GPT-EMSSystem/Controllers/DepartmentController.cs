using GPT_EMSSystem.Data;
using GPT_EMSSystem.Models;
using GPT_EMSSystem.Models.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GPT_EMSSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize]
    public class DepartmentController : Controller
    {
        private readonly EmployeeManagementDbContext _dbContext;

        public DepartmentController(EmployeeManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = new Department { Name = model.Name, Description = model.Description };

            // Add the new department to the database
            _dbContext.Departments.Add(department);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetDepartmentById", new { id = department.DepartmentId }, department);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {
            var department = await _dbContext.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _dbContext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.Name = model.Name;
            department.Description = model.Description;

            _dbContext.Entry(department).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _dbContext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


    }
}
