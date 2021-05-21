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
    public class RecordsController : ControllerBase
    {
        private readonly DBDoctorsContext _context;

        public RecordsController(DBDoctorsContext context)
        {
            _context = context;
        }

        [HttpGet("GetRecords")]
        [ProducesResponseType(200, Type = typeof(Record))]
        public async Task<ActionResult<IEnumerable<Record>>> GetRecords()
        {
            var @records = await _context.Records.ToListAsync();
            return Ok(new List<Record>(@records));
        }


        // GET: api/Records/5
        [HttpGet("GetRecord/{id}")]
        [ProducesResponseType(200, Type = typeof(Record))]
        public async Task<ActionResult<Record>> GetRecord(int id)
        {
            var @record = await _context.Records.FindAsync(id);

            if (@record == null)
            {
                return BadRequest();
            }

            return Ok(@record);
        }
        [HttpGet("GetRecordsByDoctorId/{id}")]
        [ProducesResponseType(200, Type = typeof(Record))]
        public async Task<ActionResult<IEnumerable<Record>>> GetRecordsByDoctorId(int id)
        {
            var @records = await _context.Records.Where(r => r.DoctorId == id).ToListAsync();
            return Ok(new List<Record>(@records));
        }
        [HttpGet("GetRecordsByPatientId/{id}")]
        [ProducesResponseType(200, Type = typeof(Record))]
        public async Task<ActionResult<IEnumerable<Record>>> GetRecordsByPatientId(int id)
        {
            var @records = await _context.Records.Where(r => r.PatientId == id).ToListAsync();
            return Ok(new List<Record>(@records));
        }
        [HttpGet("GetDateForDoctor/{id}")]
        [ProducesResponseType(200, Type = typeof(DateTime))]
        public async Task<ActionResult<DateTime>> GetDateForDoctor(int id)
        {
            var dates = await _context.Records.Where(r => r.DoctorId == id).Select(r => r.Date).ToListAsync();

            if (dates == null)
            {
                dates = new List<DateTime?>();
            }

            return Ok(dates);
        }
        [HttpGet("GetDateForPatient/{id}")]
        [ProducesResponseType(200, Type = typeof(DateTime))]
        public async Task<ActionResult<DateTime>> GetDateForPatient(int id)
        {
            var dates = await _context.Records.Where(r => r.PatientId == id).Select(r => r.Date).ToListAsync();

            if (dates == null)
            {
                dates = new List<DateTime?>();
            }

            return Ok(dates);
        }
        // PUT: api/Records/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateRecord")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutRecord(Record @record)
        {

            if (_context.Records.Where(r => r.PatientId == @record.PatientId && r.DoctorId == record.DoctorId && r.Date == record.Date && r.Id != @record.Id).ToList().Count != 0) return BadRequest();


            _context.Entry(@record).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(@record.Id))
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

        // POST: api/Records
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertRecord")]
        public async Task<ActionResult<Record>> PostRecord(Record @record)
        {
            if (_context.Records.Where(r => r.PatientId == @record.PatientId && r.DoctorId == record.DoctorId && r.Date == record.Date).ToList().Count != 0) return BadRequest();

            _context.Records.Add(@record);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecord", new { id = @record.Id }, @record);
        }

        // DELETE: api/Records/5
        [HttpDelete("DeleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var @record = await _context.Records.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }

            _context.Records.Remove(@record);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}