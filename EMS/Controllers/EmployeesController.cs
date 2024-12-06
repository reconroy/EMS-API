using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMS.Data;
using EMS.Models;
using EMS.Services;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using EMS.Encryptions;
using System.Configuration;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EMSDbContext _context;
        private readonly IConfiguration _configuration;

        public EmployeesController(EMSDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmpID)
            {
                return BadRequest();
            }

            // Validate Mobile Number
            if (await _context.Employees.AnyAsync(e => 
                e.Mobile1 == employee.Mobile1 && e.EmpID != employee.EmpID))
            {
                return BadRequest(new { errors = new { Mobile = "This mobile number is already registered" } });
            }

            // Validate Aadhaar Number
            if (await _context.Employees.AnyAsync(e => 
                e.AadhaarNumber == employee.AadhaarNumber && e.EmpID != employee.EmpID))
            {
                return BadRequest(new { errors = new { Aadhaar = "This Aadhaar number is already registered" } });
            }

            // Validate PAN Number
            if (await _context.Employees.AnyAsync(e => 
                e.PanNumber == employee.PanNumber && e.EmpID != employee.EmpID))
            {
                return BadRequest(new { errors = new { PAN = "This PAN number is already registered" } });
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            // Validate Mobile Number
            if (await _context.Employees.AnyAsync(e => e.Mobile1 == employee.Mobile1))
            {
                return BadRequest(new { errors = new { Mobile = "This mobile number is already registered" } });
            }

            // Validate Aadhaar Number
            if (await _context.Employees.AnyAsync(e => e.AadhaarNumber == employee.AadhaarNumber))
            {
                return BadRequest(new { errors = new { Aadhaar = "This Aadhaar number is already registered" } });
            }

            // Validate PAN Number
            if (await _context.Employees.AnyAsync(e => e.PanNumber == employee.PanNumber))
            {
                return BadRequest(new { errors = new { PAN = "This PAN number is already registered" } });
            }

            // If all validations pass, proceed with creating the employee
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            if(employee.RoleID == 1 || employee.RoleID == 2)
            {
                string generatedpassword = PasswordGenerate.GeneratePassword();
                string encpass = Sha256Hasher.ComputeSHA256Hash(generatedpassword);
                EmpAuth empAuth = new EmpAuth()
                {
                    EmpID = employee.EmpID,
                    Password = encpass
                };
                _context.EmpAuths.Add(empAuth);
                await _context.SaveChangesAsync();
                string emailBody = $@"
                <div style=""text-align: center; background-color: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); border: 2px solid black; min-width: 200px; max-width: 300px; width: 100%; margin: 50px auto;"">
                    <h2 style=""color: blue;"">Login Credentials <hr /></h2>
                     <p>
                        <strong>Username:</strong><br /> {employee.Email}
                    </p>
                    <p>
                        <strong>Password:</strong><br /> {generatedpassword}
                    </p>
                    <p style=""color: #F00;"">
                        Please change the password immediately after login.
                    </p>
                    <a href="""" style=""display: inline-block; padding: 10px 20px; background-color: #007BFF; color: #fff; text-decoration: none; border-radius: 5px; margin-top: 15px;"">Login Here</a>
                </div>";

                var result = new EmailService(_context, _configuration).SendEmail(employee.Email, "Welcome to CUPL!", emailBody);
            }

            return CreatedAtAction("GetEmployee", new { id = employee.EmpID }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmpID == id);
        }
    }
}
