using AutoMapper;
using dotNetLab2.Data;
using dotNetLab2.Models;
using dotNetLab2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace dotNetLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MoviesController> _logger;
        private readonly IMapper _mapper;

        public MoviesController(ApplicationDbContext context, ILogger<MoviesController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Return All Movies
        /// </summary>
        /// <returns></returns>
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
             return await _context.Movies.ToListAsync();
        }

        //// GET: api/Movies/filter
        //[HttpGet("startDate & endDate")]
        //[Route("filter")]
        ////public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        //public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesFilterDate(DateTime startDate, DateTime endDate)
        //{

        //    if (startDate == null || endDate == null)
        //    {
        //        return _context.Movies.ToList();
        //    }
        //    var movie = _context.Movies.Where(m => m.DateAdded >= startDate && m.DateAdded <= endDate).ToList();


        //    //IQueryable<Movie> result = _context.Movies;
        //    return movie.OrderByDescending(m => m.YearOfRelease).ToList();

        //}

        /// <summary>
        /// Get the Comments for Movies
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Comments")]
        public ActionResult<IEnumerable<MovieWithCommentsViewModel>> GetCommentsforMovie(int id)
        {
            //var query = _context.Comments.Where(c => c.Movie.Id == id).Include(c => c.Movie).Select(c => new MovieWithCommentsViewModel
            //{
            //    Id = (int)c.Movie.Id,
            //    Title = c.Movie.Title,
            //    Gender = c.Movie.Gender,
            //    YearOfRelease = c.Movie.YearOfRelease,
            //    Director = c.Movie.Director,
            //    Rating = c.Movie.Rating,
            //     Comments = c.Movie.Comments.Select(mc => new CommentViewModel
            //    {
            //        Id = mc.Id,
            //        Text = mc.Text,
            //        Important = mc.Important
            //    }).ToList()
            //});

            //var movieWithComments = query.ToList();
            //return Ok(movieWithComments);

            var query = _context.Movies.Where(m => m.Id == id).Include(m => m.Comments).Select(m => _mapper.Map<MovieWithCommentsViewModel>(m));
            var queryForCommentMovieId = _context.Comments;

            _logger.LogInformation(queryForCommentMovieId.ToList()[0].MovieId.ToString());
             _logger.LogInformation(query.ToQueryString());

            return query.ToList();

        }

        /// <summary>
        /// Add Comments for Movies
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost("{id}/Comments")]
        public IActionResult PostCommentForMovie(int id, Comment comment)
        {
            var movie = _context.Movies.Where(m => m.Id == id).Include(m => m.Comments).FirstOrDefault();
            if (movie == null)
            {
                return NotFound();
            }

            movie.Comments.Add(comment);
            _context.Entry(movie).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Get movies by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieViewModel>> GetMovie(long id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var movieViewModel = _mapper.Map<MovieViewModel>(movie);

            return movieViewModel;
        }

        /// <summary>
        /// Change movie info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(long id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

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
        /// <summary>
        /// Add Movie 
        /// </summary>
        /// <param name="movieRequest"></param>
        /// <returns></returns>
        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieViewModel>> PostMovie(MovieViewModel movieRequest)
        {
            Movie movie = _mapper.Map<Movie>(movieRequest);
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(long id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(long id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
