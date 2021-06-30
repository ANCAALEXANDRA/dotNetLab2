
using dotNetLab2.Models;
using dotNetLab2.Services;
using dotNetLab2.ViewModels.ForViewMovies;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNetLab2.Services
{
    public interface IForViewMoviesService
    {
        Task<ServiceResponse<ForViewMovie, IEnumerable<EntityManagementError>>> PlaceForViewMovie(NewForViewMoviesRequest newForViewMovieRequest, ApplicationUser user);

        Task<ServiceResponse<ForViewMoviesForUserResponse, IEnumerable<EntityManagementError>>> GetAllForViewMovies(ApplicationUser user);

        Task<ServiceResponse<ForViewMovie, IEnumerable<EntityManagementError>>> UpdateForViewMovie(int id, NewForViewMoviesRequest updateForViewMovieRequest, ApplicationUser user);

        Task<ServiceResponse<bool, IEnumerable<EntityManagementError>>> DeleteForViewMovie(int id);

        bool ForViewMovieExists(int id);
    }
}
