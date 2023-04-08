using hwSDI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hwSDI.Controllers
{
    [Route("api/screenings")]
    [ApiController]
    public class ScreeningController : ControllerBase
    {
        private readonly ItemContext _context;

        public ScreeningController(ItemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScreeningDTO>>> GetScreenings()
        {
            if (_context.Screenings == null)
            {
                return NotFound();
            }

            return await _context.Screenings.Select(x => ScreeningToDTO(x)).ToListAsync();
        }

        [HttpGet("ticket")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }

            return await _context.Tickets.Select(x => TicketToDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScreeningWithGuestDTO>> GetScreening(int id)
        {
            if (_context.Screenings == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings.FirstOrDefaultAsync(x => x.ScreeningID == id);

            if (screening == null)
            {
                return NotFound();
            }

            var guest = await _context.Tickets.Where(x => x.ScreeningID == id).Select(x => x.Guest).ToListAsync();

            var movie = await _context.Movies.FindAsync(screening.MovieID);

            var screeningDTO = new ScreeningWithGuestDTO
            {
                ScreeningID = id,
                Location = screening.Location,
                Room = screening.Room,
                Seats = screening.Seats,
                Date = screening.Date,
                Time = screening.Time,
                Movie = MovieToDTO(movie),
                Guests = guest
        };

            return screeningDTO;
        }

        [HttpGet("get/orderedGuests")]
        public async Task<IActionResult> GetScreeningWithAvgGuestAge()
        {
            if (_context.Screenings == null)
            {
                return NotFound();
            }

            if (_context.Guests == null)
            {
                return NotFound();
            }

            var x = await (from tickets in _context.Tickets
                           join screenings in _context.Screenings on tickets.ScreeningID equals screenings.ScreeningID
                           join guests in _context.Guests on tickets.GuestID equals guests.GuestID
                           group guests by screenings into g
                           select new ScreeningWithAvgGuestAgeDTO
                           {
                               ScreeningID = g.Key.ScreeningID,
                               Location = g.Key.Location,
                               Room = g.Key.Room,
                               Seats = g.Key.Seats,
                               Date = g.Key.Date,
                               Time = g.Key.Time,
                               AvgGuestAge = g.Average(guest => guest.Age)
                           }
                           ).OrderBy(dto => dto.AvgGuestAge).ToListAsync();

            return Ok(x);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutScreening(int id, ScreeningDTO screeningDTO)
        {
            if (_context.Screenings == null)
            {
                return NotFound();
            }

            if (id != screeningDTO.ScreeningID)
            {
                return BadRequest();
            }

            var screening = await _context.Screenings.FindAsync(id);

            if (screening == null)
            {
                return NotFound();
            }

            if (_context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(screeningDTO.MovieID);

            if (movie == null)
            {
                return BadRequest();
            }

            screening.Location = screeningDTO.Location;
            screening.Room = screeningDTO.Room;
            screening.Seats = screeningDTO.Seats;
            screening.Date = screeningDTO.Date;
            screening.Time = screeningDTO.Time;
            screening.MovieID = screeningDTO.MovieID;
            screening.Movie = movie;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScreeningExists(id))
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

        [HttpPost("{id}/guests")]
        public async Task<ActionResult<TicketDTO>> PostScreeningWithGuest(int id, TicketDTO ticketDTO)
        {
            if (_context.Screenings == null)
            {
                return Problem();
            }

            if (id != ticketDTO.ScreeningID)
            {
                return BadRequest();
            }

            var guest = await _context.Guests.FindAsync(ticketDTO.GuestID);

            if (guest == null)
            {
                return BadRequest();
            }

            var screening = await _context.Screenings.FindAsync(id);

            if (screening == null)
            {
                return BadRequest();
            }

            var newTicket = new Ticket
            {
                ScreeningID = id,
                Screening = screening,

                GuestID = ticketDTO.GuestID,
                Guest = guest,

                Price = ticketDTO.Price,
                NumberOf = ticketDTO.NumberOf
            };

            try
            {
                _context.Tickets.Add(newTicket);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetTickets), TicketToDTO(newTicket));
        }

        [HttpPost("{id}/guestsList")]
        public async Task<ActionResult<TicketDTO>> PostScreeningWithListOfGuest(int id, TicketListDTO ticketDTO)
        {
            if (_context.Screenings == null)
            {
                return Problem();
            }

            var screening = await _context.Screenings.FindAsync(id);

            if (screening == null)
            {
                return BadRequest();
            }

            foreach (var t in ticketDTO.Tickets)
            {
                var guest = await _context.Guests.FindAsync(t.GuestID);

                if (guest == null)
                {
                    return BadRequest();
                }

                var newTicket = new Ticket
                {
                    ScreeningID = id,
                    Screening = screening,

                    GuestID = guest.GuestID,
                    Guest = guest,

                    Price = t.Price,
                    NumberOf = t.NumberOf
                };

                try
                {
                    _context.Tickets.Add(newTicket);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return BadRequest();
                }

            }
           
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ScreeningDTO>> PostScreening(ScreeningDTO screeningDTO)
        {
            if (_context.Screenings == null)
            {
                return Problem();
            }

            if (_context.Movies == null)
            {
                return Problem();
            }

            var movie = await _context.Movies.FindAsync(screeningDTO.MovieID);

            if (movie == null)
            {
                return BadRequest();
            }
                
            var screening = new Screening
            {
                Location = screeningDTO.Location,
                Room = screeningDTO.Room,
                Seats = screeningDTO.Seats,
                Date = screeningDTO.Date,
                Time = screeningDTO.Time,
                MovieID = screeningDTO.MovieID,
                Movie = movie
            };
           
            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetScreening), new { id = screening.ScreeningID }, ScreeningToDTO(screening));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreening(int id)
        {
            if (_context.Screenings == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings.FindAsync(id);

            if (screening == null)
            {
                return NotFound();
            }

            _context.Screenings.Remove(screening);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScreeningExists(int id)
        {
            return (_context.Screenings?.Any(x => x.ScreeningID == id)).GetValueOrDefault();
        }

        private static ScreeningDTO ScreeningToDTO(Screening screening)
        {
            return new ScreeningDTO
            {
                ScreeningID = screening.ScreeningID,
                Location = screening.Location,
                Room = screening.Room,
                Seats = screening.Seats,
                Date = screening.Date,
                Time = screening.Time,
                MovieID = screening.MovieID
            };
        }

        private static TicketDTO TicketToDTO(Ticket ticket)
        {
            return new TicketDTO
            {
                ScreeningID = ticket.ScreeningID,
                GuestID = ticket.GuestID,
                Price = ticket.Price,
                NumberOf = ticket.NumberOf
            };
        }

        private static MovieDTO MovieToDTO(Movie movie)
        {
            return new MovieDTO
            {
                MovieID = movie.MovieID,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                Genre = movie.Genre,
                Producer = movie.Producer,
                LengthMinutes = movie.LengthMinutes
            };
        }
    }
}
