using hwSDI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hwSDI.Validation;

namespace hwSDI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ItemContext _context;
        private readonly Valid _valid;

        public MovieController(ItemContext context, Valid valid)
        {
            _context = context;
            _valid = valid;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }

            return await _context.Movies.Select(x => MovieToDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Include(x => x.Screenings).FirstOrDefaultAsync(x => x.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        [HttpGet("filter/{releaseYear}")]
        public async Task<List<MovieDTO>> GetFilteredMoviesByReleaseYear(int releaseYear)
        {
            var movie = await _context.Movies.Where(x => x.ReleaseYear > releaseYear).Select(x => MovieToDTO(x)).ToListAsync();
            return movie;
        }

        [HttpGet("get/orderedScreenings")]
        public async Task<List<MovieWithAvgScreeningSeatNoDTO>> GetMovieWithAvgScreeningSeat()
        {
            var x = await (from screenings in _context.Screenings
                           join movies in _context.Movies on screenings.MovieID equals movies.MovieID
                           group screenings by movies into g
                           select new MovieWithAvgScreeningSeatNoDTO
                           {
                               MovieID = g.Key.MovieID,
                               Title = g.Key.Title,
                               ReleaseYear = g.Key.ReleaseYear,
                               Genre = g.Key.Genre,
                               Producer = g.Key.Producer,
                               LengthMinutes = g.Key.LengthMinutes,
                               AvgScreeningSeatNo = g.Average(screening => screening.Seats)
                           }
                           ).OrderBy(dto => dto.AvgScreeningSeatNo).ToListAsync();

            return x;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO movieDTO)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }

            if (id != movieDTO.MovieID)
            {
                return BadRequest();
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            if (!_valid.ValidText(movieDTO.Title))
            {
                return BadRequest("Title should not be empty!");
            }
            if (!_valid.ValidYear(movieDTO.ReleaseYear))
            {
                return BadRequest("ReleaseYear should be between 1500 and 2023!");
            }
            if (!_valid.ValidText(movieDTO.Genre))
            {
                return BadRequest("Genre should not be empty!");
            }
            if (!_valid.ValidText(movieDTO.Producer))
            {
                return BadRequest("Producer should not be empty!");
            }

            movie.Title = movieDTO.Title;
            movie.ReleaseYear = movieDTO.ReleaseYear;
            movie.Genre = movieDTO.Genre;
            movie.Producer = movieDTO.Producer;
            movie.LengthMinutes = movieDTO.LengthMinutes;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        public async Task<ActionResult<MovieDTO>> PostMovie(MovieDTO movieDTO)
        {
            if (!_valid.ValidText(movieDTO.Title))
            {
                return BadRequest("Title should not be empty!");
            }
            if (!_valid.ValidYear(movieDTO.ReleaseYear))
            {
                return BadRequest("ReleaseYear should be between 1500 and 2023!");
            }
            if (!_valid.ValidText(movieDTO.Genre))
            {
                return BadRequest("Genre should not be empty!");
            }
            if (!_valid.ValidText(movieDTO.Producer))
            {
                return BadRequest("Producer should not be empty!");
            }

            var movie = new Movie
            {
                MovieID = movieDTO.MovieID,
                Title = movieDTO.Title,
                ReleaseYear = movieDTO.ReleaseYear,
                Genre = movieDTO.Genre,
                Producer = movieDTO.Producer,
                LengthMinutes = movieDTO.LengthMinutes,
                Screenings = null!
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movieDTO.MovieID }, MovieToDTO(movie));
        }

        [HttpPost("{id}/screeningList")]
        public async Task<IActionResult> PostMovieWithScreeningList(int id, List<ScreeningDTO> screenings)
        {
            if (_context.Movies == null)
            {
                return Problem();
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return BadRequest();
            }

            foreach (var s in screenings)
            {
                var screening = new Screening
                {
                    ScreeningID = s.ScreeningID,
                    Location = s.Location,
                    Room = s.Room,
                    Seats = s.Seats,
                    Date = s.Date,
                    Time = s.Time,
                    MovieID = id
                };

                _context.Screenings.Add(screening);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(x => x.MovieID == id)).GetValueOrDefault();
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
