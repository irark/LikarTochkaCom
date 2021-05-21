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
    public class DoctorsController : ControllerBase
    {
        private readonly DBDoctorsContext _context;

        public DoctorsController(DBDoctorsContext context)
        {
            _context = context;
        }

        // GET: api/Doctors
        [HttpGet("GetDoctors")]
        [ProducesResponseType(200, Type = typeof(Doctor))]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            var doctors = await _context.Doctors.ToListAsync();
            return Ok(new List<Doctor>(doctors));
        }

        [HttpGet("GetDoctorsByHospitalId/{id}")]
        [ProducesResponseType(200, Type = typeof(Doctor))]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsByHospitalId(int Id)
        {
            var doctors =  await _context.Doctors.Where(d => d.HospitalId == Id).ToListAsync();
            return Ok(new List<Doctor>(doctors));
        }
        [HttpGet("GetDoctorsBySpecializationId/{id}")]
        [ProducesResponseType(200, Type = typeof(Doctor))]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsBySpecializationId(int Id)
        {
            var doctors =  await _context.Doctors.Where(d => d.SpecializationId == Id).ToListAsync();
            return Ok(new List<Doctor>(doctors));
        }
         [HttpGet("GetDoctorsByOfficeId/{id}")]
        [ProducesResponseType(200, Type = typeof(Doctor))]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsByOfficeId(int Id)
        {
            var doctors =  await _context.Doctors.Where(d => d.OfficeId == Id).ToListAsync();
            return Ok(new List<Doctor>(doctors));
        }

        // GET: api/Doctors/5
        [HttpGet("GetDoctor/{id}")]
        [ProducesResponseType(200, Type = typeof(Doctor))]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return BadRequest();
            }

            return Ok(doctor);
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateDoctor")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutDoctor( Doctor doctor)
        {
            if (_context.Doctors.Where(d => d.FirstName == doctor.FirstName && d.SpecializationId == doctor.SpecializationId && doctor.LastName == d.LastName && d.PhoneNumber == doctor.PhoneNumber && d.OfficeId == doctor.OfficeId && d.HospitalId == doctor.HospitalId && d.Id != doctor.Id).ToList().Count != 0) return BadRequest();

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(doctor.Id))
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

        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertDoctor")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            if (_context.Doctors.Where(d => d.FirstName == doctor.FirstName && d.SpecializationId == doctor.SpecializationId && doctor.LastName == d.LastName && d.PhoneNumber == doctor.PhoneNumber && d.OfficeId == doctor.OfficeId && d.HospitalId == doctor.HospitalId).ToList().Count != 0) return BadRequest();

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctor", new { id = doctor.Id }, doctor);
        }

        // DELETE: api/Doctors/5
        [HttpDelete("DeleteDoctor/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            var records = _context.Records.Where(r => r.DoctorId == id).ToList();
            if (records.Count != 0)
            {
                return BadRequest();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
