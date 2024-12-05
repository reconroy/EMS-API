using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMS.Data;
using EMS.Models;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public BanksController(EMSDbContext context)
        {
            _context = context;
        }

        // GET: api/Banks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Banks>>> GetBanks()
        {
            return await _context.Banks.ToListAsync();
        }

        // GET: api/Banks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Banks>> GetBank(int id)
        {
            var bank = await _context.Banks.FindAsync(id);

            if (bank == null || !bank.IsActive)
            {
                return NotFound();
            }

            return bank;
        }

        // PUT: api/Banks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBank(int id, Banks bank)
        {
            if (id != bank.BankId)
            {
                return BadRequest();
            }

            // Check if bank name already exists
            var existingBank = await _context.Banks
                .Where(b => b.BankName == bank.BankName && b.BankId != id && b.IsActive)
                .FirstOrDefaultAsync();

            if (existingBank != null)
            {
                return Conflict("Bank name already exists");
            }

            _context.Entry(bank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/Banks
        [HttpPost]
        public async Task<ActionResult<Banks>> PostBank(Banks bank)
        {
            // Check if bank name already exists
            var existingBank = await _context.Banks
                .Where(b => b.BankName == bank.BankName && b.IsActive)
                .FirstOrDefaultAsync();

            if (existingBank != null)
            {
                return Conflict("Bank name already exists");
            }

            bank.IsActive = true;

            _context.Banks.Add(bank);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBank), new { id = bank.BankId }, bank);
        }

        // DELETE: api/Banks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBank(int id)
        {
            var bank = await _context.Banks.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }

            bank.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankExists(int id)
        {
            return _context.Banks.Any(e => e.BankId == id);
        }
    }
}
