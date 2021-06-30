using dotNetLab2.Models;
using dotNetLab2.Services;
using dotNetLab2.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNetLab2.Services
{
    public interface IMoviesService
    {
        Task<ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityManagementError>>> GetMovies();

        Task<ServiceResponse<MovieViewModel, string>> GetMovie(long id);

        Task<ServiceResponse<IEnumerable<MovieWithCommentsViewModel>, IEnumerable<EntityManagementError>>> GetCommentsForMovie(long id);

        Task<ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityManagementError>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate);

        Task<ServiceResponse<Movie, IEnumerable<EntityManagementError>>> PostMovie(MovieViewModel movieRequest);

        Task<ServiceResponse<Comment, IEnumerable<EntityManagementError>>> PostCommentForMovie(long movieId, CommentViewModel commentRequest);

        Task<ServiceResponse<Movie, IEnumerable<EntityManagementError>>> PutMovie(long id, MovieViewModel movieRequest);
        Task<ServiceResponse<Comment, IEnumerable<EntityManagementError>>> PutComment(long commentId, CommentViewModel commentRequest);

        Task<ServiceResponse<bool, IEnumerable<EntityManagementError>>> DeleteMovie(long id);
        Task<ServiceResponse<bool, IEnumerable<EntityManagementError>>> DeleteComment(long id);

        bool MovieExists(long id);
        bool CommentExists(long id);
    }
}
