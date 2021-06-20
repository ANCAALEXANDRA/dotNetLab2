using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.ViewModels.ForViewMovies
{
    public class NewForViewRequest
    {
        public List<long> ViewedMoviesIds { get; set; }
        public DateTime? ViewDateTime { get; set; }
    }
}
