using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class ClubEditViewModel
    {
        public ClubEditViewModel()
        {
            Courses = new List<Course>();
        }

        public Club Club { get; set; }

        public IEnumerable<Course> Courses { get; set; }
    }
}