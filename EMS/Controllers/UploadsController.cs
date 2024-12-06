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
    public class UploadsController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public UploadsController(EMSDbContext context)
        {
            _context = context;
        }

        // GET: api/Uploads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Uploads>>> GetUploads()
        {
            return await _context.Uploads.ToListAsync();
        }

        // GET: api/Uploads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Uploads>> GetUploads(int id)
        {
            var uploads = await _context.Uploads.FindAsync(id);

            if (uploads == null)
            {
                return NotFound();
            }

            return uploads;
        }

        // PUT: api/Uploads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUploads(int id, Uploads uploads)
        {
            if (id != uploads.UPID)
            {
                return BadRequest();
            }

            _context.Entry(uploads).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UploadsExists(id))
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

        // POST: api/Uploads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUploads([FromForm] UploadDTO uploadDTO)
        {
            if (uploadDTO == null || uploadDTO.EmpID == 0)
            {
                return BadRequest("Invalid upload request.");
            }

            // Retrieve the existing record if it exists
            var existingUpload = await _context.Uploads.FirstOrDefaultAsync(u => u.EmpID == uploadDTO.EmpID);

            if (existingUpload == null)
            {
                existingUpload = new Uploads { EmpID = uploadDTO.EmpID };
                _context.Uploads.Add(existingUpload);
            }

            // Base directory for storing files
            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Process and store each file if present in the DTO
            if (uploadDTO.Image != null)
            {
                existingUpload.ImagePath = await SaveFile(uploadDTO.Image, baseDirectory, "Images");
            }

            if (uploadDTO.Aadhar != null)
            {
                existingUpload.AadharPath = await SaveFile(uploadDTO.Aadhar, baseDirectory, "Aadhar");
            }

            if (uploadDTO.Pan != null)
            {
                existingUpload.PanPath = await SaveFile(uploadDTO.Pan, baseDirectory, "Pan");
            }

            if (uploadDTO.Passbook != null)
            {
                existingUpload.PassbookPath = await SaveFile(uploadDTO.Passbook, baseDirectory, "Passbook");
            }

            await _context.SaveChangesAsync();

            return Ok(existingUpload);
        }

        private async Task<string> SaveFile(IFormFile file, string baseDirectory, string folderName)
        {
            // Create the folder path
            var folderPath = Path.Combine(baseDirectory, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate a unique file name
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path
            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }


        // DELETE: api/Uploads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUploads(int id)
        {
            var uploads = await _context.Uploads.FindAsync(id);
            if (uploads == null)
            {
                return NotFound();
            }

            _context.Uploads.Remove(uploads);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UploadsExists(int id)
        {
            return _context.Uploads.Any(e => e.UPID == id);
        }
    }
}
