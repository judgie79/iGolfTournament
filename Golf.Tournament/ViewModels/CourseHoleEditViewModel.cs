using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;
using System.Web.Mvc;
using Golf.Models;

namespace Golf.Tournament.ViewModels
{
    public class CourseHoleEditViewModel
    {
        public CourseHoleEditViewModel()
        {

        }

        public CourseHoleEditViewModel(Club club, Course course, TeeBox teebox, CourseHoles courseHoles, HoleCollection holes)
            : this()
        {
            this.Course = course;
            this.Club = club;
            this.Teebox = teebox;
            this.CourseHoles = courseHoles;
            Holes = holes;
        }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public TeeBox Teebox { get; set; }

        public CourseHoles CourseHoles { get; set; }

        public HoleCollection Holes { get; set; }

        public CourseHole ViewModel { get; set; }
    }
}