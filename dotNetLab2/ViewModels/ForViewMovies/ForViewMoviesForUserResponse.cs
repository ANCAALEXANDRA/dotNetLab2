using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.ViewModels.ForViewMovies
{
    public class ForViewMoviesForUserResponse
    {
        public ApplicationUserViewModel ApplicationUser { get; set; }
        public List<MovieViewModel> Movies { get; set; }
        public DateTime ViewDateTime { get; set; }
    }
}
