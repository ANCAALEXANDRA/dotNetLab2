using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.ViewModels
{
    public class MovieViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public int DurationInMinutes { get; set; }
        public string YearOfRelease { get; set; }
        public string Director { get; set; }
        public DateTime DateAdded { get; set; }
        public int Rating { get; set; }
        public Boolean Watched { get; set; }
        public List<CommentViewModel> Comments { get; set; }

    }
}
