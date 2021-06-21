﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotNetLab2.Data;
using dotNetLab2.Models;
using dotNetLab2.ViewModels.ForViewMovies;
using Microsoft.AspNetCore.Authorization;
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
            private readonly ApplicationDbContext _context;
            private readonly ILogger<ForViewMoviesController> _logger;
            private readonly IMapper _mapper;
            private readonly UserManager<ApplicationUser> _userManager;


            public ForViewMoviesController(ApplicationDbContext context, ILogger<ForViewMoviesController> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _logger = logger;
                _mapper = mapper;
                _userManager = userManager;
            }
        
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPost]
            public async Task<ActionResult> PlaceForViewMovie(NewForViewRequest newForViewRequest)
            {
                var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);


            List<Movie> viewedMovies = new List<Movie>();
            newForViewRequest.ViewedMoviesIds.ForEach(pid =>
            {
                var movieWithId = _context.Movies.Find(pid);
                if (movieWithId != null)
                {
                    viewedMovies.Add(movieWithId);
                }
            });

            if (viewedMovies.Count == 0)
            {
                return BadRequest();
            }

            var order = new ForViewMovie
            {
                ApplicationUser = user,
                WatchDateTime = newForViewRequest.ViewDateTime.GetValueOrDefault(),
                Movies = viewedMovies
            };

            _context.ForViewMovies.Add(order);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ForViewMoviesForUserResponse>>> GetAll()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user == null)
            {
                return NotFound();
            }

            var result = _context.ForViewMovies.Where(f => f.ApplicationUser.Id == user.Id).Include(f => f.Movies).ToList();
            return _mapper.Map<List<ForViewMovie>, List<ForViewMoviesForUserResponse>>(result);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        public async Task<ActionResult> EditForViewMovie(EditForViewMovieForUser editForViewMovieRequest)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            ForViewMovie ForViewMovies = _context.ForViewMovies.Where(f => f.Id == editForViewMovieRequest.Id && f.ApplicationUser.Id == user.Id).Include(f => f.Movies).FirstOrDefault();

            if (ForViewMovies == null)
            {
                return BadRequest("There is no ForViewMovie with this ID.");
            }

            editForViewMovieRequest.MovieIDs.ForEach(mid =>
            {
                var movie = _context.Movies.Find(mid);
                if (movie != null && !ForViewMovies.Movies.Contains(movie))
                {
                    ForViewMovies.Movies.ToList().Add(movie);
                }
            });

            _context.Entry(ForViewMovies).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        public async Task<IActionResult> DeleteForViewMovies(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var ForViewMovies = _context.ForViewMovies.Where(f => f.ApplicationUser.Id == user.Id && f.Id == id).FirstOrDefault();

            if (ForViewMovies == null)
            {
                return NotFound();
            }

            _context.ForViewMovies.Remove(ForViewMovies);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
    }
