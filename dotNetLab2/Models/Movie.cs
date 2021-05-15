using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace dotNetLab2.Models
{
    public class Movie
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        //[Contains(new[] { "action", "thriller", "comedy", "horror" })]
        public string Gender { get; set; }
        public int DurationInMinutes { get; set; }
        public string YearOfRelease { get; set; }
        public string Director { get; set; }
        public DateTime DateAdded { get; set; }
        [Range(1, 10)]
        public int Rating { get; set; }
        public Boolean Watched { get; set; }

    }
}
