using dotNetLab2.Models;
using dotNetLab2.Services;
using dotNetLab2.ViewModels;
using dotNetLab2.ViewModels.Pagination;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNetLab2.Services
{
    public interface IMoviesService
    {
        Task<ServiceResponse<PaginatedResultSet<MovieViewModel>, IEnumerable<EntityManagementError>>> GetMovies(int? page = 1, int? perPage = 20);

        Task<ServiceResponse<MovieViewModel, string>> GetMovie(int id);

        Task<ServiceResponse<IEnumerable<MovieWithCommentsViewModel>, IEnumerable<EntityManagementError>>> GetCommentsForMovie(int id);

        Task<ServiceResponse<PaginatedResultSet<Movie>, IEnumerable<EntityManagementError>>> FilterMoviesByDateAdded(string fromDate, string toDate, int? page = 1, int? perPage = 10);

        Task<ServiceResponse<Movie, IEnumerable<EntityManagementError>>> PostMovie(MovieViewModel movieRequest);

        Task<ServiceResponse<Comment, IEnumerable<EntityManagementError>>> PostCommentForMovie(int movieId, CommentViewModel commentRequest);

        Task<ServiceResponse<Movie, IEnumerable<EntityManagementError>>> PutMovie(int id, MovieViewModel movieRequest);
        Task<ServiceResponse<Comment, IEnumerable<EntityManagementError>>> PutComment(int commentId, CommentViewModel commentRequest);

        Task<ServiceResponse<bool, IEnumerable<EntityManagementError>>> DeleteMovie(int id);
        Task<ServiceResponse<bool, IEnumerable<EntityManagementError>>> DeleteComment(int id);

        bool MovieExists(int id);
        bool CommentExists(int id);
    }
}
