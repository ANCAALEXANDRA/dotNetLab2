﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotNetLab2.Data;
using dotNetLab2.Models;
using dotNetLab2.ViewModels;

namespace dotNetLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        //[Route(@"daterange/{startDate:regex(^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{3}Z$)}/{endDate:regex(^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{3}Z$)}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        //public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(DateTime startDate, DateTime endDate)
        {
            
            IQueryable<Movie> result = _context.Movies;

            return await result.OrderByDescending(m => m.YearOfRelease).ToListAsync();
           
    }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieViewModel>> GetMovie(long id)
        {
            var movie = await _context.Movies.FindAsync(id);

            // look at automapper
            var movieViewModel = new MovieViewModel
            {
                Title = movie.Title,
                Description = movie.Description,
                Gender = movie.Gender,
                DurationInMinutes = movie.DurationInMinutes,
                YearOfRelease = movie.YearOfRelease,
                Director = movie.Director,
                DateAdded = movie.DateAdded,
                Rating = movie.Rating,
                Watched = movie.Watched,
               // Comments = movie.Comments
            };

            if (movie == null)
            {
                return NotFound();
            }

            return movieViewModel;
        }

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

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
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
