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
            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayroll", new { id = payroll.PrID }, payroll);
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
