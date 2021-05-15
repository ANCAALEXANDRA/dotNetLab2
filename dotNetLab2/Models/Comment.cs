using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.Models
{
    public class Comment
    { 
       
        public long Id { get; set; }
        public string Text { get; set; }
        public Boolean Important { get; set; }
    }
}
