using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMS.Data;
using EMS.Models;
using EMS.Models.NonDBModels;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollsController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public PayrollsController(EMSDbContext context)
        {
            _context = context;
        }

        // GET: api/Payrolls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payroll>>> GetPayrolls()
        {
            return await _context.Payrolls.ToListAsync();
        }

        // GET: api/Payrolls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payroll>> GetPayroll(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);

            if (payroll == null)
            {
                return NotFound();
            }

            return payroll;
        }

        // PUT: api/Payrolls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayroll(int id, Payroll payroll)
        {
            if (id != payroll.PrID)
            {
                return BadRequest();
            }

            _context.Entry(payroll).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayrollExists(id))
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

        // POST: api/Payrolls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payroll>> PostPayroll(Payroll payroll)
        {
            if (payroll == null)
            {
                return BadRequest();
            }
            payroll.MonthYear = DateTime.Now.ToString("MM-yyyy");
            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayroll", new { id = payroll.PrID }, payroll);
        }

        /*[HttpGet("GetNetSalary/{id}")]
        public async Task<ActionResult<Payroll>> GetNetSalary(int id)
        {
            var payroll =  _context.Payrolls.Where(u=>u.EmpID == id).OrderBy(u=>u.PrID).Last();
            var LnA =  _context.LoansandAdvances.Where(u => u.EmpID == id).OrderBy(u=>u.LnAID).Last();

            if (payroll == null)
            {
                return NotFound();
            }
            double netsalary = payroll.BasicSalary - (payroll.EPF+payroll.ESI+payroll.RD+payroll.HI);
            return Ok(netsalary);
        }*/

        /*[HttpGet("GetNetSalary/{id}")]
        public async Task<IActionResult> GetNetSalary(int id)
        {
            // Fetch the latest payroll entry
            var payroll = await _context.Payrolls
                .Where(u => u.EmpID == id)
                .OrderByDescending(u => u.PrID)
                .FirstOrDefaultAsync();

            if (payroll == null)
            {
                return NotFound("Payroll information not found for the specified employee.");
            }

            // Fetch active loans/advances for the employee
            var activeLoans = await _context.LoansandAdvances
                .Where(u => u.EmpID == id && u.isActive && !u.IsDeductionComplete)
                .ToListAsync();

            double totalDeduction = payroll.EPF + payroll.ESI + payroll.RD + payroll.HI;

            // Calculate additional deductions from loans/advances
            foreach (var loan in activeLoans)
            {
                if (DateOnly.FromDateTime(DateTime.Now) >= loan.DeductionStartDate)
                {
                    totalDeduction += loan.DeductionAmount;

                    // Mark deduction as complete if balance is reduced to zero
                    if (loan.DeductionAmount >= loan.AmountGiven)
                    {
                        loan.isActive = false;
                        loan.IsDeductionComplete = true;
                    }
                    else
                    {
                        loan.AmountGiven -= loan.DeductionAmount;
                    }
                }
            }

            // Save loan/advance updates
            _context.LoansandAdvances.UpdateRange(activeLoans);
            await _context.SaveChangesAsync();

            // Calculate net salary
            double netSalary = payroll.BasicSalary - totalDeduction;

            return Ok(new
            {
                NetSalary = netSalary,
                TotalDeductions = totalDeduction,
                ActiveLoans = activeLoans
            });
        }*/

        [HttpGet("GetNetSalary/{id}")]
        public async Task<IActionResult> GetNetSalary(int id)
        {
            // Fetch the latest payroll entry
            var payroll = await _context.Payrolls
                .Where(u => u.EmpID == id)
                .OrderByDescending(u => u.PrID)
                .FirstOrDefaultAsync();

            if (payroll == null)
            {
                return NotFound("Payroll information not found for the specified employee.");
            }

            double netSalary;
            double totalDeduction = payroll.EPF + payroll.ESI + payroll.RD + payroll.HI;
            var activeLoans = await _context.LoansandAdvances
                .Where(u => u.EmpID == id && u.isActive && !u.IsDeductionComplete)
                .ToListAsync();

            // Calculate additional deductions from loans/advances
            foreach (var loan in activeLoans)
            {
                if (DateOnly.FromDateTime(DateTime.Now) >= loan.DeductionStartDate)
                {
                    totalDeduction += loan.DeductionAmount;

                    // Mark deduction as complete if balance is reduced to zero
                    if (loan.DeductionAmount >= loan.AmountGiven)
                    {
                        loan.isActive = false;
                        loan.IsDeductionComplete = true;
                    }
                    else
                    {
                        loan.AmountGiven -= loan.DeductionAmount;
                    }
                }
            }

            if (payroll.PayrollType == "Fixed")
            {
                // Fixed salary calculation
                netSalary = payroll.BasicSalary - totalDeduction;
            }
            else if (payroll.PayrollType == "Variable")
            {

                // Fetch total hours worked (TotalDuration) for the employee for the current month
                var totalDuration = await _context.Attendances
                    .Where(a => a.EmpID == id && a.Date.Month == 9 && a.Date.Year == 2024)
                    .Select(a => a.TotalDuration)
                    .ToListAsync();

                if (totalDuration == null || totalDuration.Count == 0)
                {
                    return NotFound("Attendance information not found for the specified employee.");
                }

                // Sum up total hours worked
                double M5 =Math.Round(totalDuration.Sum(td => td.TotalHours));
                Console.WriteLine(M5);// Actual Hours Worked
                double K5 = payroll.BasicSalary; // Basic Salary
                double L5 = Math.Round(K5 / 240);
                Console.WriteLine(L5);// Basic Rate per Hour (rounded off)

                // Calculate Salary as per Hours
                if (M5 >= 600)
                {
                    netSalary = K5 * 3 + (M5 - 600) * L5;
                }
                else if (M5 >= 400)
                {
                    netSalary = K5 * 2 + (M5 - 400) * L5;
                }
                else if (M5 > 200)
                {
                    netSalary = K5 + (M5 - 200) * L5;
                }
                else if (M5 > 189)
                {
                    netSalary = K5;
                }
                else if (M5 >= 150)
                {
                    netSalary = K5 - (200 - M5) * L5;
                }
                else
                {
                    netSalary = L5 * M5;
                }

                // Subtract total deductions
                netSalary -= totalDeduction;
            }

            else
            {
                return BadRequest("Invalid payroll type.");
            }

            // Fetch active loans/advances for the employee
            

            // Save loan/advance updates
/*            _context.LoansandAdvances.UpdateRange(activeLoans);
            await _context.SaveChangesAsync();*/

            // Return net salary and details
            return Ok(new
            {
                NetSalary = netSalary,
                TotalDeductions = totalDeduction,
                ActiveLoans = activeLoans
            });
        }


        [HttpGet("GetLoanBalances/{empId}")]
        public async Task<ActionResult<LnABalance>> GetLoanBalances(int empId)
        {
            var balances =  _context.LnABalances
                .Where(b => b.EmpID == empId)
                .OrderByDescending(b => b.MonthYear)
                .First();

            if (balances ==null)
            {
                return NotFound("No balances found for the specified employee.");
            }

            return Ok(balances);
        }
        // DELETE: api/Payrolls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayroll(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);
            if (payroll == null)
            {
                return NotFound();
            }

            _context.Payrolls.Remove(payroll);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PayrollExists(int id)
        {
            return _context.Payrolls.Any(e => e.PrID == id);
        }
    }
}
