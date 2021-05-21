using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LikarKrapkaComEntities.Models;
using LikarKrapkaComEntities.ViewModel;

namespace LikarKrapkaComAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly DBDoctorsContext _context;
        private readonly ServerHub _serverHub;

        public HospitalsController(DBDoctorsContext context, ServerHub serverHub)
        {
            _context = context;
            _serverHub = serverHub;
        }

        // GET: api/Hospitals
        [HttpGet("GetHospitals")]
        [ProducesResponseType(200, Type = typeof(Hospital))]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetHospitals()
        {
            var hospitals =  await _context.Hospitals.ToListAsync();
            return Ok(new List<Hospital>(hospitals));
        }

        // GET: api/Hospitals/5
        [HttpGet("GetHospital/{id}")]
        [ProducesResponseType(200, Type = typeof(Hospital))]
        public async Task<ActionResult<Hospital>> GetHospital(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);

            if (hospital == null)
            {
                return BadRequest();
            }

            return Ok(hospital);
        }

        // PUT: api/Hospitals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateHospital")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutHospital( Hospital hospital)
        {
            var hospitals = _context.Hospitals.Where(h => h.Name == hospital.Name && h.Address == hospital.Address && h.Id != hospital.Id).ToList();

            if (hospitals.Count != 0) return BadRequest();
            _context.Entry(hospital).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HospitalExists(hospital.Id))
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

        // POST: api/Hospitals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertHospital")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Hospital>> PostHospital(Hospital hospital)
        {
            if (_context.Hospitals.Where(h => h.Name == hospital.Name && h.Address == hospital.Address).ToList().Count != 0) return BadRequest();

            _context.Hospitals.Add(hospital);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHospital", new { id = hospital.Id }, hospital);
        }

        // DELETE: api/Hospitals/5
        [HttpDelete("{id}")]

        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);
            var doctors =  _context.Doctors.Where(d => d.HospitalId == id).Select(d => d.Id).ToList();
            if (doctors.Count != 0)
            {
                return BadRequest();
            }

            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool HospitalExists(int id)
        {
            return _context.Hospitals.Any(e => e.Id == id);
        }
    }
}
