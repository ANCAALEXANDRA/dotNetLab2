using AutoMapper;
using dotNetLab2.Data;
using dotNetLab2.Models;
using dotNetLab2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using dotNetLab2.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace dotNetLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMoviesService _moviesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(MoviesService moviesService, UserManager<ApplicationUser> userManager)
        {
            _moviesService = moviesService;
            _userManager = userManager;
        }

        //private readonly ApplicationDbContext _context;
        //private readonly ILogger<MoviesController> _logger;
        //private readonly IMapper _mapper;

        //public MoviesController(ApplicationDbContext context, ILogger<MoviesController> logger, IMapper mapper)
        //{
        //    _context = context;
        //    _logger = logger;
        //    _mapper = mapper;
        //}

        /// <summary>
        /// Return All Movies
        /// </summary>
        /// <response code="200">Get a list of movies</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> GetMovies()
        {
            //var movies = await _context.Movies.Select(m => _mapper.Map<MovieViewModel>(m)).ToListAsync();
            //return movies;
            var moviesServiceResult = await _moviesService.GetMovies();

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Filter movies by date added
        /// </summary>
        /// <response code="200">Filter movies by date added</response>
        /// <response code="400">Unable to get the movie due to validation error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("filter")]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate)
        {
            var moviesServiceResult = await _moviesService.FilterMoviesByDateAdded(fromDate, toDate);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return Ok(moviesServiceResult.ResponseOk);
        }


        /// <summary>
        /// Get a movie by id
        /// </summary>
        /// <response code="200">Get a movie by id</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieViewModel>> GetMovie(int id)
        {
            var moviesServiceResult = await _moviesService.GetMovie(id);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Get the Comments for Movies
        /// </summary>
        /// <response code="200">Get movie with comments</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}/Comments")]
        public async Task<ActionResult<IEnumerable<MovieWithCommentsViewModel>>> GetCommentsforMovie(int id)
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
            /*
            var query = _context.Movies.Where(m => m.Id == id).Include(m => m.Comments).Select(m => _mapper.Map<MovieWithCommentsViewModel>(m));
            var queryForCommentMovieId = _context.Comments;

            _logger.LogInformation(queryForCommentMovieId.ToList()[0].MovieId.ToString());
             _logger.LogInformation(query.ToQueryString());

            return query.ToList();
            */

            var moviesServiceResult = await _moviesService.GetCommentsForMovie(id);

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Add comment for id movie
        /// </summary>
        /// <response code="201">Add a new comment to movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPost("{id}/Comments")]
        public async Task<ActionResult> PostCommentForMovie(int movieId, CommentViewModel commentRequest)
        {
           
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PostCommentForMovie(movieId, commentRequest);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            var movie = moviesServiceResult.ResponseOk;

            return CreatedAtAction("GetMovieWithComments", new { id = movie.Id }, "New comment successfully added");
        }

        /// <summary>
        /// Change movie info
        /// </summary>
        /// <response code="204">Amend a movie</response>
        /// <response code="400">Unable to amend the movie due to validation error</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieViewModel movie)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PutMovie(id, movie);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPost]
        public async Task<ActionResult<MovieViewModel>> PostMovie(MovieViewModel movieRequest)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PostMovie(movieRequest);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            var movie = moviesServiceResult.ResponseOk;

            return CreatedAtAction("GetMovie", new { id = movie.Id }, "New movie successfully created");
        }
        /// <summary>
        /// Delete a movie by id
        /// </summary>
        /// <response code="204">Delete a movie</response>
        /// <response code="404">Movie not found</response>
        // DELETE: api/Movies/5
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            if (!_moviesService.MovieExists(id))
            {
                return NotFound();
            }

            var moviesServiceResult = await _moviesService.DeleteMovie(id);

            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return NoContent();
        }


        /// <summary>
        /// Updates a comment
        /// </summary>
        /// <param name="commentId">The comment ID</param>
        /// <param name="comment">the comment</param>
        /// <returns>If comment updates: NoContent, BadRequest if the ID is not valid, or NotFound if comment was not found</returns>
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPut("{id}/Comments/{commentId}")]
        public async Task<IActionResult> PutComment(int commentId, CommentViewModel comment)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PutComment(commentId, comment);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return NoContent();
        }


        /// <summary>
        /// Deletes a comment
        /// </summary>
        /// <response code="204">Delete a movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpDelete("{id}/Comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            if (!_moviesService.CommentExists(commentId))
            {
                return NotFound();
            }

            var moviesServiceResult = await _moviesService.DeleteComment(commentId);

            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return NoContent();
        }

    }
}
