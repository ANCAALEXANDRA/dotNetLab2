using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotNetLab2.Data;
using dotNetLab2.Models;
using dotNetLab2.Services;
using dotNetLab2.ViewModels.ForViewMovies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace dotNetLab2.Controllers
{
    
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        [ApiController]
        [Route("api/[controller]")]
        public class ForViewMoviesController : ControllerBase
        {
            //private readonly ApplicationDbContext _context;
            //private readonly ILogger<ForViewMoviesController> _logger;
            //private readonly IMapper _mapper;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ForViewMoviesService _forviewmoviesService;

        //public ForViewMoviesController(ApplicationDbContext context, ILogger<ForViewMoviesController> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        //    {
        //        _context = context;
        //        _logger = logger;
        //        _mapper = mapper;
        //        _userManager = userManager;
        //    }
        public ForViewMoviesController(ForViewMoviesService forviewmoviesService, UserManager<ApplicationUser> userManager)
        {
            _forviewmoviesService = forviewmoviesService;
            _userManager = userManager;
        }

        /// <summary>
        /// Add a new reservation
        /// </summary>
        /// <response code="201">Add a new reservation</response>
        /// <response code="400">Unable to add the reservation due to validation error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPost]
            public async Task<ActionResult> PlaceForViewMovie(NewForViewMoviesRequest newForViewRequest)
            {

            var user = new ApplicationUser();
            try
            {
                 user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }


            var forviewmovieServiceResult = await _forviewmoviesService.PlaceForViewMovie(newForViewRequest, user);
            if (forviewmovieServiceResult.ResponseError != null)
            {
                return BadRequest(forviewmovieServiceResult.ResponseError);
            }

            var forViewMovie = forviewmovieServiceResult.ResponseOk;

            return CreatedAtAction("GetForVIewMovies", new { id = forViewMovie.Id }, "New viewed movie successfully added");
        }

        /// <summary>
        /// Get view movies
        /// </summary>
        /// <response code="200">Get reservations</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ForViewMoviesForUserResponse>>> GetAllForViewMovies()
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var forviewmovieServiceResult = await _forviewmoviesService.GetAllForViewMovies(user);

            return Ok(forviewmovieServiceResult.ResponseOk);
        }

        /// <summary>
        /// Amend a reservation
        /// </summary>
        /// <response code="204">Amend a reservation</response>
        /// <response code="400">Unable to amend the reservation due to validation error</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        public async Task<ActionResult> EditForViewMovie(int id, NewForViewMoviesRequest editForViewMovieRequest)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var forviewmovieServiceResult = await _forviewmoviesService.EditForViewMovie(id, editForViewMovieRequest, user);
            if (forviewmovieServiceResult.ResponseError != null)
            {
                return BadRequest(forviewmovieServiceResult.ResponseError);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a reservation by id
        /// </summary>
        /// <response code="204">Delete a reservation</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        public async Task<IActionResult> DeleteForViewMovies(int id)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            if (!_forviewmoviesService.ForViewMovieExists(id))
            {
                return NotFound();
            }

            var forviewmovieServiceResult = await _forviewmoviesService.DeleteForViewMovie(id);
            if (forviewmovieServiceResult.ResponseError != null)
            {
                return BadRequest(forviewmovieServiceResult.ResponseError);
            }

            return NoContent();
        }

    }
    }
