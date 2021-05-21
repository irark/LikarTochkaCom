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
    public class PatientsController : ControllerBase
    {
        private readonly DBDoctorsContext _context;

        public PatientsController(DBDoctorsContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet("GetPatients")]
        [ProducesResponseType(200, Type = typeof(Patient))]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(new List<Patient>(patients));
        }

        // GET: api/Patients/5
        [HttpGet("GetPatient/{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return BadRequest();
            }

            return Ok(patient);
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdatePatient")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutPatient(Patient patient)
        {
            if (_context.Patients.Where(d => d.FirstName == patient.FirstName &&  patient.LastName == d.LastName && d.PhoneNumber == patient.PhoneNumber && d.Id != patient.Id).ToList().Count != 0) return BadRequest();

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patient.Id))
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

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertPatient")]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            if (_context.Patients.Where(d => d.FirstName == patient.FirstName && patient.LastName == d.LastName && d.PhoneNumber == patient.PhoneNumber).ToList().Count != 0) return BadRequest();

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        }

        // DELETE: api/Patients/5
        [HttpDelete("DeletePatient/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            var records = _context.Records.Where(r => r.PatientId == id).ToList();
            if (records.Count != 0)
            {
                return BadRequest();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
