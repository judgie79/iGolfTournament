using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;
using System.Web.Mvc;
using Golf.Models;

namespace Golf.Tournament.ViewModels
{
    public class CourseHoleDetailsViewModel
    {
        public CourseHoleDetailsViewModel()
        {

        }

        public CourseHoleDetailsViewModel(Club club, Course course, TeeBox teebox, FrontOrBack frontOrBack, CourseHole hole)
            : this()
        {
            this.Course = course;
            this.Club = club;
            this.Teebox = teebox;
            this.Hole = hole;
            this.FrontOrBack = frontOrBack;
        }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public FrontOrBack FrontOrBack { get; set; }

        public CourseHole Hole { get; set; }

        public TeeBox Teebox { get; set; }
    }
}