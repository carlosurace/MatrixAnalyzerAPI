using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixAnalyzer.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealershipController : ControllerBase
    {
        private readonly carofferDBContext _context;

        public DealershipController(carofferDBContext context)
        {
            _context = context;
        }

        // GET: api/DCandidate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dealership>>> GetDCandidates()
        {
            return await _context.Dealerships.ToListAsync();
        }

        // GET: api/DCandidate/5
        [HttpGet("{DealershipID}")]
        public async Task<ActionResult<Dealership>> GetDCandidate(int DealershipID)
        {
            var Dealership = await _context.Dealerships.FindAsync(DealershipID);

            if (Dealership == null)
            {
                return NotFound();
            }

            return Dealership;
        }

        // PUT: api/DCandidate/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{DealershipID}")]
        public async Task<IActionResult> PutDCandidate(int DealershipID, Dealership Dealership)
        {
            Dealership.DealershipID = DealershipID;

            _context.Entry(Dealership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealershipExists(DealershipID))
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

        // POST: api/DCandidate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Dealership>> PostDCandidate(Dealership Dealership)
        {
            _context.Dealerships.Add(Dealership);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDCandidate", new { DealershipID = Dealership.DealershipID }, Dealership);
        }

        // DELETE: api/DCandidate/5
        [HttpDelete("{DealershipID}")]
        public async Task<ActionResult<Dealership>> DeleteDCandidate(int DealershipID)
        {
            var Dealership = await _context.Dealerships.FindAsync(DealershipID);
            if (Dealership == null)
            {
                return NotFound();
            }

            _context.Dealerships.Remove(Dealership);
            await _context.SaveChangesAsync();

            return Dealership;
        }

        private bool DealershipExists(int DealershipID)
        {
            return _context.Dealerships.Any(e => e.DealershipID == DealershipID);
        }
    }
}