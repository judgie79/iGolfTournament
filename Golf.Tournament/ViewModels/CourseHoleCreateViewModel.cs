using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;
using System.Web.Mvc;
using Golf.Models;

namespace Golf.Tournament.ViewModels
{
    public class CourseHoleCreateViewModel
    {
        public CourseHoleCreateViewModel()
        {
            this.ViewModel = new CourseHole();
        }

        public CourseHoleCreateViewModel(Club club, Course course, TeeBox teebox)
            : this()
        {
            this.Course = course;
            this.Club = club;
            this.Teebox = teebox;
        }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public TeeBox Teebox { get; set; }

        public HoleCollection Holes { get; set; }

        public CourseHoles CourseHoles { get; set; }

        public CourseHole ViewModel { get; set; }
    }
}