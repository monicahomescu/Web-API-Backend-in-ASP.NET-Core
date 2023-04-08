using hwSDI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static hwSDI.Controllers.Validations;
using hwSDI.Validation;

namespace hwSDI.Controllers
{
    [Route("api/guests")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly ItemContext _context;
        private readonly Valid _valid;

        public GuestController(ItemContext context, Valid valid)
        {
            _context = context;
            _valid = valid;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestDTO>>> GetGuests()
        {
            if (_context.Guests == null)
            {
                return NotFound();
            }

            return await _context.Guests.Select(x => GuestToDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuestWithScreeningDTO>> GetGuest(int id)
        {
            if (_context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.FirstOrDefaultAsync(x => x.GuestID == id);

            if (guest == null)
            {
                return NotFound();
            }

            var screening = await _context.Tickets.Where(x => x.GuestID == id).Select(x => x.Screening).ToListAsync();

            var guestDTO = new GuestWithScreeningDTO
            {
                GuestID = id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                Email = guest.Email,
                Age = guest.Age,
                Screenings = screening
            };

            return guestDTO;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuest(int id, GuestDTO guestDTO)
        {
            if (_context.Guests == null)
            {
                return NotFound();
            }

            if (id != guestDTO.GuestID)
            {
                return BadRequest();
            }

            var guest = await _context.Guests.FindAsync(id);

            if (guest == null)
            {
                return NotFound();
            }

            if (!_valid.ValidText(guestDTO.FirstName))
            {
                return BadRequest("FirstName should not be empty!");
            }
            if (!_valid.ValidText(guestDTO.LastName))
            {
                return BadRequest("LastName should not be empty!");
            }
            if (!_valid.ValidEmail(guestDTO.Email))
            {
                return BadRequest("Email should only contain letters and one '@' and one '.'!");
            }

            guest.FirstName = guestDTO.FirstName;
            guest.LastName = guestDTO.LastName;
            guest.PhoneNumber = guestDTO.PhoneNumber;
            guest.Email = guestDTO.Email;
            guest.Age = guestDTO.Age;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuestExists(id))
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

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> PostGuest(GuestDTO guestDTO)
        {
            if (_context.Guests == null)
            {
                return Problem();
            }

            if (!_valid.ValidText(guestDTO.FirstName))
            {
                return BadRequest("FirstName should not be empty!");
            }
            if (!_valid.ValidText(guestDTO.LastName))
            {
                return BadRequest("LastName should not be empty!");
            }
            if (!_valid.ValidEmail(guestDTO.Email))
            {
                return BadRequest("Email should only contain letters and one '@' and one '.'!");
            }

            var guest = new Guest
            {
                FirstName = guestDTO.FirstName,
                LastName = guestDTO.LastName,
                PhoneNumber = guestDTO.PhoneNumber,
                Email = guestDTO.Email,
                Age = guestDTO.Age
            };

            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGuest), new { id = guest.GuestID }, GuestToDTO(guest));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            if (_context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.FindAsync(id);

            if (guest == null)
            {
                return NotFound();
            }

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GuestExists(int id)
        {
            return (_context.Guests?.Any(x => x.GuestID == id)).GetValueOrDefault();
        }

        private static GuestDTO GuestToDTO(Guest guest)
        {
            return new GuestDTO
            {
                GuestID = guest.GuestID,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                Email = guest.Email,
                Age = guest.Age
            };
        }
    }
}