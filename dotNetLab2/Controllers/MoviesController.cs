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
using dotNetLab2.ViewModels.Pagination;

namespace dotNetLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMoviesService _moviesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(IMoviesService moviesService, UserManager<ApplicationUser> userManager)
        {
            _moviesService = moviesService;
            _userManager = userManager;
        }


        /// <summary>
        /// Return All Movies
        /// </summary>
        /// <response code="200">Get a list of movies</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> GetMovies(int? page = 1, int? perPage = 20)
        {
            var moviesServiceResult = await _moviesService.GetMovies(page, perPage);

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
        public async Task<ActionResult<PaginatedResultSet<Movie>>> FilterMoviesByDateAdded(string fromDate, string toDate, int? page = 1, int? perPage = 20)
        {
            var result = await _moviesService.FilterMoviesByDateAdded(fromDate, toDate, page, perPage);
            return result.ResponseOk;
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
        [HttpPost("{id}/Comments")]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
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
        /// <response code="201">Adds a new movie</response>
        /// <response code="400">Unable to add the movie</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // POST: api/Movies
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        public async Task<ActionResult<MovieViewModel>> PostMovie([FromBody] MovieViewModel movieRequest)
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
