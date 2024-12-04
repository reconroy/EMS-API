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
    public class EmpAuthsController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public EmpAuthsController(EMSDbContext context)
        {
            _context = context;
        }

        // GET: api/EmpAuths
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpAuth>>> GetEmpAuths()
        {
            return await _context.EmpAuths.ToListAsync();
        }

        // GET: api/EmpAuths/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpAuth>> GetEmpAuth(int id)
        {
            var empAuth = await _context.EmpAuths.FindAsync(id);

            if (empAuth == null)
            {
                return NotFound();
            }

            return empAuth;
        }

        // PUT: api/EmpAuths/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpAuth(int id, EmpAuth empAuth)
        {
            if (id != empAuth.EmpAuthID)
            {
                return BadRequest();
            }

            _context.Entry(empAuth).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpAuthExists(id))
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

        // POST: api/EmpAuths
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpAuth>> PostEmpAuth(EmpAuth empAuth)
        {
            _context.EmpAuths.Add(empAuth);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpAuth", new { id = empAuth.EmpAuthID }, empAuth);
        }

        // DELETE: api/EmpAuths/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpAuth(int id)
        {
            var empAuth = await _context.EmpAuths.FindAsync(id);
            if (empAuth == null)
            {
                return NotFound();
            }

            _context.EmpAuths.Remove(empAuth);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpAuthExists(int id)
        {
            return _context.EmpAuths.Any(e => e.EmpAuthID == id);
        }
    }
}
