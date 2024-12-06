using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMS.Data;
using EMS.Models;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansandAdvancesController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public LoansandAdvancesController(EMSDbContext context)
        {
            _context = context;
        }

        // GET: api/LoansandAdvances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoansandAdvance>>> GetLoansandAdvances()
        {
            return await _context.LoansandAdvances.ToListAsync();
        }

        // GET: api/LoansandAdvances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoansandAdvance>> GetLoansandAdvance(int id)
        {
            var loansandAdvance = await _context.LoansandAdvances.FindAsync(id);

            if (loansandAdvance == null)
            {
                return NotFound();
            }

            return loansandAdvance;
        }

        // PUT: api/LoansandAdvances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoansandAdvance(int id, LoansandAdvance loansandAdvance)
        {
            if (id != loansandAdvance.LnAID)
            {
                return BadRequest();
            }

            _context.Entry(loansandAdvance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoansandAdvanceExists(id))
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

        [HttpPost]
        public async Task<ActionResult<LoansandAdvance>> AddLoanAdvance(LoansandAdvance loanAdvance)
        {
            if (loanAdvance == null)
            {
                return BadRequest("Loan or advance details cannot be null.");
            }
            loanAdvance.MonthYear = DateTime.Now.ToString("MM-yyyy");
            loanAdvance.isActive = true;
            loanAdvance.IsDeductionComplete = false;

            // Add loan to the database
            _context.LoansandAdvances.Add(loanAdvance);
            await _context.SaveChangesAsync();

            // Record the opening balance in the balance table
            var initialBalance = new LnABalance
            {
                EmpID = loanAdvance.EmpID,
                MonthYear = DateTime.Now.ToString("MM-yyyy"),
                OpeningBalance = loanAdvance.AmountGiven,
                TotalDeduction = 0,
                ClosingBalance = loanAdvance.AmountGiven
            };

            _context.LnABalances.Add(initialBalance);
            await _context.SaveChangesAsync();

            return Ok(loanAdvance);
        }

        /*[HttpPost("UpdateLoanBalances")]
        public async Task<IActionResult> UpdateLoanBalances()
        {
            var currentMonthYear = DateTime.Now.ToString("MM-yyyy");

            // Get all active loans
            var activeLoans = await _context.LoansandAdvances
                .Where(l => l.isActive)
                .ToListAsync();

            foreach (var loan in activeLoans)
            {
                // Check if a balance entry already exists for the current month for this employee
                var existingBalance = await _context.LnABalances
                    .FirstOrDefaultAsync(b => b.EmpID == loan.EmpID && b.MonthYear == currentMonthYear);

                if (existingBalance != null)
                {
                    // Check if the opening balance is greater than the closing balance
                    if (existingBalance.OpeningBalance > existingBalance.ClosingBalance)
                    {
                        // Deduction has already been made; skip this loan
                        continue;
                    }
                }

                // Get the previous balance entry for this loan
                var previousBalance = await _context.LnABalances
                    .Where(b => b.EmpID == loan.EmpID)
                    .OrderByDescending(b => b.LnABID)
                    .FirstOrDefaultAsync();

                double openingBalance = previousBalance?.ClosingBalance ?? loan.AmountGiven;
                double deduction = loan.DeductionAmount;
                double closingBalance = openingBalance - deduction;

                // Create a new balance record for the current month
                var newBalance = new LnABalance
                {
                    EmpID = loan.EmpID,
                    MonthYear = currentMonthYear,
                    OpeningBalance = openingBalance,
                    TotalDeduction = deduction,
                    ClosingBalance = closingBalance
                };

                _context.LnABalances.Add(newBalance);

                // If loan is fully paid, mark it as inactive
                if (closingBalance <= 0)
                {
                    loan.isActive = false;
                    loan.IsDeductionComplete = true;
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Balances updated for all active loans.");
        }*/

        [HttpPost("UpdateLoanBalances")]
        public async Task<IActionResult> UpdateLoanBalances()
        {
            var currentMonthYear = DateTime.Now.ToString("MM-yyyy");

            // Get all active loans
            var activeLoans = await _context.LoansandAdvances
                .Where(l => l.isActive)
                .ToListAsync();

            foreach (var loan in activeLoans)
            {
                try
                {
                    // Check if a balance entry already exists for the current month for this employee
                    var existingBalance = await _context.LnABalances
                        .FirstOrDefaultAsync(b => b.EmpID == loan.EmpID && b.MonthYear == currentMonthYear);

                    if (existingBalance != null)
                    {
                        // Check if the opening balance is greater than the closing balance
                        if (existingBalance.OpeningBalance > existingBalance.ClosingBalance)
                        {
                            // Deduction has already been made; skip this loan
                            continue;
                        }
                    }

                    // Get the previous balance entry for this loan
                    var previousBalance = await _context.LnABalances
                        .Where(b => b.EmpID == loan.EmpID)
                        .OrderByDescending(b => b.LnABID)
                        .FirstOrDefaultAsync();

                    double openingBalance = previousBalance?.ClosingBalance ?? loan.AmountGiven;
                    double deduction = loan.DeductionAmount;
                    double closingBalance = openingBalance - deduction;

                    // Create a new balance record for the current month
                    var newBalance = new LnABalance
                    {
                        EmpID = loan.EmpID,
                        MonthYear = currentMonthYear,
                        OpeningBalance = openingBalance,
                        TotalDeduction = deduction,
                        ClosingBalance = closingBalance
                    };

                    _context.LnABalances.Add(newBalance);

                    // If loan is fully paid, mark it as inactive
                    if (closingBalance <= 0)
                    {
                        loan.isActive = false;
                        loan.IsDeductionComplete = true;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes (optional)
                    Console.WriteLine($"Error processing loan for EmpID: {loan.EmpID}. Details: {ex.Message}");

                    // Continue to the next loan
                    continue;
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Balances updated for all active loans.");
        }





        // DELETE: api/LoansandAdvances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoansandAdvance(int id)
        {
            var loansandAdvance = await _context.LoansandAdvances.FindAsync(id);
            if (loansandAdvance == null)
            {
                return NotFound();
            }

            _context.LoansandAdvances.Remove(loansandAdvance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoansandAdvanceExists(int id)
        {
            return _context.LoansandAdvances.Any(e => e.LnAID == id);
        }
    }
}
