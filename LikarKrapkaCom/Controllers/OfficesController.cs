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
    public class OfficesController : ControllerBase
    {
        private readonly DBDoctorsContext _context;

        public OfficesController(DBDoctorsContext context)
        {
            _context = context;
        }

        // GET: api/Offices
        [HttpGet("GetOffices")]
        [ProducesResponseType(200, Type = typeof(Office))]
        public async Task<ActionResult<IEnumerable<Office>>> GetOffices()
        {
            var offices = await _context.Offices.ToListAsync();
            return Ok(new List<Office>(offices));
        }

        // GET: api/Offices/5
        [HttpGet("GetOffice/{id}")]
        [ProducesResponseType(200, Type = typeof(Office))]
        public async Task<ActionResult<Office>> GetOffice(int id)
        {
            var office = await _context.Offices.FindAsync(id);

            if (office == null)
            {
                return BadRequest();
            }

            return Ok(office);
        }

        // PUT: api/Offices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateOffice/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutOffice(Office office)
        {
            if (_context.Offices.Where(d => d.Name == office.Name && d.Id != office.Id).ToList().Count != 0) return BadRequest();


            _context.Entry(office).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficeExists(office.Id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Offices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertOffice")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Office>> PostOffice(Office office)
        {
            if (_context.Offices.Where(d => d.Name == office.Name).ToList().Count != 0) return BadRequest();
            _context.Offices.Add(office);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOffice", new { id = office.Id }, office);
        }

        // DELETE: api/Offices/5
        [HttpDelete("DeleteOffice/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteOffice(int id)
        {
            var office = await _context.Offices.FindAsync(id);
            var doctors = await _context.Doctors.Where(d => d.OfficeId == id).ToListAsync();
            if (doctors.Count != 0)
            {
                return BadRequest();
            }

            _context.Offices.Remove(office);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool OfficeExists(int id)
        {
            return _context.Offices.Any(e => e.Id == id);
        }
    }
}
