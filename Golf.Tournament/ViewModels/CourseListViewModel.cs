using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class CourseListViewModel
    {
        public Club Club { get; set; }

        public CourseCollection Courses { get; set; }
    }
}