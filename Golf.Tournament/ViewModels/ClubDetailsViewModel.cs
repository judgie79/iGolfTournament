using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class ClubDetailsViewModel
    {
        public ClubDetailsViewModel()
        {
            Courses = new CourseCollection();
            Holes = new HoleCollection();
            Club = new Club();
        }

        public Club Club { get; set; }

        public CourseCollection Courses { get; set; }
        
        public HoleCollection Holes { get; set; }
    }

    
}