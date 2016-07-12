using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class CourseEditViewModel
    {
        public Course Course { get; set; }

        public IEnumerable<Club> Clubs { get; set; }
    }

    public class CourseCreateViewModel
    {
        public Course Course { get; set; }

        public IEnumerable<Club> Clubs { get; set; }
    }
}