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
    public class EmpBanksController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public EmpBanksController(EMSDbContext context)
        {
            _context = context;
        }

        // GET: api/EmpBanks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpBank>>> GetEmpBanks()
        {
            return await _context.EmpBanks.ToListAsync();
        }

        // GET: api/EmpBanks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpBank>> GetEmpBank(int id)
        {
            var empBank = await _context.EmpBanks.FindAsync(id);

            if (empBank == null)
            {
                return NotFound();
            }

            return empBank;
        }

        // PUT: api/EmpBanks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpBank(int id, EmpBank empBank)
        {
            if (id != empBank.EmpBankID)
            {
                return BadRequest();
            }

            _context.Entry(empBank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpBankExists(id))
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

        // POST: api/EmpBanks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpBank>> PostEmpBank(EmpBank empBank)
        {
            _context.EmpBanks.Add(empBank);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpBank", new { id = empBank.EmpBankID }, empBank);
        }

        // DELETE: api/EmpBanks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpBank(int id)
        {
            var empBank = await _context.EmpBanks.FindAsync(id);
            if (empBank == null)
            {
                return NotFound();
            }

            _context.EmpBanks.Remove(empBank);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpBankExists(int id)
        {
            return _context.EmpBanks.Any(e => e.EmpBankID == id);
        }
    }
}
