using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class ClubDetailsViewModel
    {
        public ClubDetailsViewModel()
        {
            Courses = new List<Course>();
        }

        public Club Club { get; set; }

        public IEnumerable<Course> Courses { get; set; }
    }
}