using dotNetLab2.Data;
using dotNetLab2.Models;
using dotNetLab2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using dotNetLab2.ViewModels.ForViewMovies;

namespace dotNetLab2.Services
{
    public class ForViewMoviesService : IForViewMoviesService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForViewMoviesService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<ForViewMoviesForUserResponse, IEnumerable<EntityManagementError>>> GetAllForViewMovies(ApplicationUser user)
        {
            var ForViewMoviesFromDb = await _context.ForViewMovies
                .Where(o => o.ApplicationUser.Id == user.Id)
                .Include(o => o.Movies)
                .FirstOrDefaultAsync();

            var ForViewMoviesForUserResponse = _mapper.Map<ForViewMoviesForUserResponse>(ForViewMoviesFromDb);

            var serviceResponse = new ServiceResponse<ForViewMoviesForUserResponse, IEnumerable<EntityManagementError>>();
            serviceResponse.ResponseOk = ForViewMoviesForUserResponse;

            return serviceResponse;
        }

        public async Task<ServiceResponse<ForViewMovie, IEnumerable<EntityManagementError>>> PlaceForViewMovie(NewForViewMoviesRequest newForViewMoviesRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<ForViewMovie, IEnumerable<EntityManagementError>>();

            var viewedMovies = new List<Movie>();
            newForViewMoviesRequest.ViewedMoviesIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    viewedMovies.Add(movieWithId);
                }
            });

            var ForViewMovie = new ForViewMovie
            {
                ApplicationUser = user,
                WatchDateTime = newForViewMoviesRequest.ViewDateTime.GetValueOrDefault(),
                Movies = viewedMovies
            };

            _context.ForViewMovies.Add(ForViewMovie);

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = ForViewMovie;
            }
            catch (Exception e)
            {
                var errors = new List<EntityManagementError>();
                errors.Add(new EntityManagementError { Code = e.GetType().ToString(), Description = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ForViewMovie, IEnumerable<EntityManagementError>>> EditForViewMovie(int id, NewForViewMoviesRequest editForViewMovieRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<ForViewMovie, IEnumerable<EntityManagementError>>();

            var viewedMovies = new List<Movie>();
            editForViewMovieRequest.ViewedMoviesIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    viewedMovies.Add(movieWithId);
                }
            });

            var ForViewMovie = new ForViewMovie
            {
                Id = id,
                ApplicationUser = user,
                WatchDateTime = editForViewMovieRequest.ViewDateTime.GetValueOrDefault(),
                Movies = viewedMovies
            };

            _context.Entry(ForViewMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = ForViewMovie;
            }
            catch (Exception e)
            {
                var errors = new List<EntityManagementError>();
                errors.Add(new EntityManagementError { Code = e.GetType().ToString(), Description = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<EntityManagementError>>> DeleteForViewMovie(int id)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityManagementError>>();

            try
            {
                var forViewMovie = await _context.ForViewMovies.FindAsync(id);
                _context.ForViewMovies.Remove(forViewMovie);
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = true;
            }
            catch (Exception e)
            {
                var errors = new List<EntityManagementError>();
                errors.Add(new EntityManagementError { Code = e.GetType().ToString(), Description = e.Message });
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public bool ForViewMovieExists(int id)
        {
            return _context.ForViewMovies.Any(e => e.Id == id);
        }

       
        //public async Task<ServiceResponse<ForViewMovie, IEnumerable<EntityManagementError>>> UpdateForViewMovie(int id, NewForViewMoviesRequest updateForViewMovieRequest, ApplicationUser user)
        //{
        //    var serviceResponse = new ServiceResponse<ForViewMovie, IEnumerable<EntityManagementError>>();

        //    var viewedMovies = new List<Movie>();
        //    updateForViewMovieRequest.ViewedMoviesIds.ForEach(rid =>
        //    {
        //        var movieWithId = _context.Movies.Find(rid);
        //        if (movieWithId != null)
        //        {
        //            viewedMovies.Add(movieWithId);
        //        }
        //    });

        //    var ForViewMovie = new ForViewMovie
        //    {
        //        Id = id,
        //        ApplicationUser = user,
        //        WatchDateTime = updateForViewMovieRequest.ViewDateTime.GetValueOrDefault(),
        //        Movies = viewedMovies
        //    };

        //    _context.Entry(ForViewMovie).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //        serviceResponse.ResponseOk = ForViewMovie;
        //    }
        //    catch (Exception e)
        //    {
        //        var errors = new List<EntityManagementError>();
        //        errors.Add(new EntityManagementError { Code = e.GetType().ToString(), Description = e.Message });
        //    }

        //    return serviceResponse;
        //}

    }
}
