using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LikarKrapkaComEntities.Models;

namespace LikarKrapkaComAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly DBDoctorsContext _context;

        public SpecializationsController(DBDoctorsContext context)
        {
            _context = context;
        }

        // GET: api/Specializations
        [HttpGet("GetSpecializations")]
        [ProducesResponseType(200, Type = typeof(Specialization))]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations()
        {
            var specializations = await _context.Specializations.ToListAsync();
            return Ok(new List<Specialization>(specializations));
        }

        // GET: api/Specializations/5
        [HttpGet("GetSpecialization/{id}")]
        [ProducesResponseType(200, Type = typeof(Specialization))]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);

            if (specialization == null)
            {
                return BadRequest();
            }

            return Ok(specialization);
        }

        // PUT: api/Specializations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateSpecialization")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutSpecialization(Specialization specialization)
        {
            if (_context.Specializations.Where(d => d.Name == specialization.Name && d.Id != specialization.Id).ToList().Count != 0) return BadRequest();


            _context.Entry(specialization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecializationExists(specialization.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Specializations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertSpecialization")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Specialization>> PostSpecialization(Specialization specialization)
        {
            if (_context.Specializations.Where(d => d.Name == specialization.Name).ToList().Count != 0) return BadRequest();

            _context.Specializations.Add(specialization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpecialization", new { id = specialization.Id }, specialization);
        }

        // DELETE: api/Specializations/5
        [HttpDelete("DeleteSpecialization/{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            var doctors = await _context.Doctors.Where(d => d.SpecializationId == id).ToListAsync();
            if (doctors.Count != 0)
            {
                return BadRequest();
            }

            _context.Specializations.Remove(specialization);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.Id == id);
        }
    }
}
