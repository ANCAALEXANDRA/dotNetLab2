using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.ViewModels.ForViewMovies
{
    public class EditForViewMovieForUser
    {

        public int Id { get; set; }
        public ApplicationUserViewModel User { get; set; }
        public List<long> MovieIDs { get; set; }
        public DateTime WatchDateTime { get; set; }
    }
}
