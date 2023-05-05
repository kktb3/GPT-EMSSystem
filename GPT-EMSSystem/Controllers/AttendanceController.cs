using GPT_EMSSystem.Data;
using GPT_EMSSystem.Models;
using GPT_EMSSystem.Models.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPT_EMSSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly EmployeeManagementDbContext _context;

        public AttendanceController(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Attendance>> RecordAttendance(AttendanceInputModel model)
        {
            try
            {
                // Get the employee ID from the authenticated user's claims
                var employeeIdClaim = User.FindFirst("EmployeeId");
                if (employeeIdClaim == null)
                {
                    return Unauthorized();
                }

                int employeeId = int.Parse(employeeIdClaim.Value);

                // Set the attendance properties
                var attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    Date = model.Date.Date,
                    IsPresent = model.IsPresent
                };

                if (attendance.IsPresent)
                {
                    attendance.CheckInTime = DateTime.Now;
                    attendance.CheckOutTime = DateTime.Now;
                }

                // Add the attendance record to the database
                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
