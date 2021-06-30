using dotNetLab2.Data;
using dotNetLab2.Models;
using dotNetLab2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using AutoMapper;

namespace dotNetLab2.Services
{
    public class MoviesService : IMoviesService
    {
        public readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private ApplicationDbContext context;

        public MoviesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public MoviesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Movie> GetAllAboveRating(int minRating)
        {
            return _context.Movies.Where(m => m.Rating >= minRating).ToList();
        }


        public async Task<ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityManagementError>>> GetMovies()
        {
            var movies = await _context.Movies
                .Select(m => _mapper.Map<MovieViewModel>(m))
                .ToListAsync();

            var serviceResponse = new ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityManagementError>>();
            serviceResponse.ResponseOk = movies;
            return serviceResponse;
        }

        public async Task<ServiceResponse<MovieViewModel, string>> GetMovie(long id)
        {
            var serviceResponse = new ServiceResponse<MovieViewModel, string>();
            var movie = await _context.Movies
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                serviceResponse.ResponseError = "No movie found";
                return serviceResponse;
            }

            var movieResponse = _mapper.Map<MovieViewModel>(movie);
            serviceResponse.ResponseOk = movieResponse;
            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<MovieWithCommentsViewModel>, IEnumerable<EntityManagementError>>> GetCommentsForMovie(long id)
        {
            var moviesWithComments = await _context.Movies
                .Where(m => m.Id == id)
                .Include(m => m.Comments)
                .Select(m => _mapper.Map<MovieWithCommentsViewModel>(m))
                .ToListAsync();

            var serviceResponse = new ServiceResponse<IEnumerable<MovieWithCommentsViewModel>, IEnumerable<EntityManagementError>>();
            serviceResponse.ResponseOk = moviesWithComments;

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityManagementError>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityManagementError>>();
            var errors = new List<EntityManagementError>();

            if (!fromDate.HasValue || !toDate.HasValue)
            {
                errors.Add(new EntityManagementError { Code = "", Description = "Both dates are required" });
                serviceResponse.ResponseError = errors;
                return serviceResponse;
            }

            if (fromDate >= toDate)
            {
                errors.Add(new EntityManagementError { Code = "", Description = "fromDate is not before toDate" });
                serviceResponse.ResponseError = errors;
                return serviceResponse;
            }

            var filteredMovies = await _context.Movies
                .Where(m => m.DateAdded >= fromDate && m.DateAdded <= toDate)
                .OrderByDescending(m => m.YearOfRelease)
                .Include(m => m.Comments)
                .Select(m => _mapper.Map<MovieViewModel>(m))
                .ToListAsync();

            serviceResponse.ResponseOk = filteredMovies;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Movie, IEnumerable<EntityManagementError>>> PostMovie(MovieViewModel movieRequest)
        {
            var movie = _mapper.Map<Movie>(movieRequest);
            _context.Movies.Add(movie);

            var serviceResponse = new ServiceResponse<Movie, IEnumerable<EntityManagementError>>();

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = movie;
            }
            catch (Exception e)
            {
                var errors = new List<EntityManagementError>();
                errors.Add(new EntityManagementError { Code = e.GetType().ToString(), Description = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Comment, IEnumerable<EntityManagementError>>> PostCommentForMovie(long movieId, CommentViewModel commentRequest)
        {
            var serviceResponse = new ServiceResponse<Comment, IEnumerable<EntityManagementError>>();

            var commentDB = _mapper.Map<Comment>(commentRequest);

            var movie = await _context.Movies
                .Where(m => m.Id == movieId)
                .Include(m => m.Comments)
                .FirstOrDefaultAsync();

            try
            {
                movie.Comments.Add(commentDB);
                _context.Entry(movie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = commentDB;
            }
            catch (Exception e)
            {
                var errors = new List<EntityManagementError>();
                errors.Add(new EntityManagementError { Code = e.GetType().ToString(), Description = e.Message });
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<Movie, IEnumerable<EntityManagementError>>> PutMovie(long id, MovieViewModel movieRequest)
        {
            var serviceResponse = new ServiceResponse<Movie, IEnumerable<EntityManagementError>>();

            var movie = _mapper.Map<Movie>(movieRequest);

            try
            {
                _context.Entry(movie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = movie;
            }
            catch (Exception e)
            {
                var errors = new List<EntityManagementError>();
                errors.Add(new EntityManagementError { Code = e.GetType().ToString(), Description = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<EntityManagementError>>> DeleteMovie(long id)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityManagementError>>();

            try
            {
                var movie = await _context.Movies.FindAsync(id);
                _context.Movies.Remove(movie);
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

        public bool MovieExists(long id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        public async Task<ServiceResponse<Comment, IEnumerable<EntityManagementError>>> PutComment(long commentId, CommentViewModel commentRequest)
        {
            var serviceResponse = new ServiceResponse<Comment, IEnumerable<EntityManagementError>>();

            var comment = _mapper.Map<Comment>(commentRequest);

            try
            {
                _context.Entry(comment).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = comment;
            }
            catch (Exception e)
            {
                var errors = new List<EntityManagementError>();
                errors.Add(new EntityManagementError { Code = e.GetType().ToString(), Description = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<EntityManagementError>>> DeleteComment(long id)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityManagementError>>();

            try
            {
                var comment = await _context.Comments.FindAsync(id);
                _context.Comments.Remove(comment);
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

        public bool CommentExists(long id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }

       
    }
}
