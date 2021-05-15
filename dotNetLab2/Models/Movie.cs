using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.Models
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime YearOfRelease { get; set; }
        public string Director { get; set; }
        public DateTime DateAdded { get; set; }
        public int Rating { get; set; }
        public bool Watched { get; set; }


    }
}
