using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.ViewModels
{
    public class MovieWithCommentsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
  
        public string Gender { get; set; }
        public string YearOfRelease { get; set; }
        public string Director { get; set; }
        public int Rating { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

    }
}
