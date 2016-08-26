using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TeeboxListViewModel
    {
        public TeeboxListViewModel()
        {

        }

        public TeeboxListViewModel(Club club, Course course)
        {
            this.Course = course;
            this.Club = club;
            this.Teeboxes = course.TeeBoxes;
        }
        public TeeboxCollection Teeboxes { get; set; }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public TeeBox ViewModel = new TeeBox();
    }

    

    public class CourseHoleListViewModel
    {
        public CourseHoleListViewModel()
        {

        }

        public CourseHoleListViewModel(Club club, Course course, TeeBox teebox)
        {
            this.Course = course;
            this.Club = club;
            this.Teebox = teebox;
            this.Holes = teebox.Holes;
        }
        public TeeBox Teebox { get; set; }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public CourseHoles Holes { get; set; }

        public CourseHole ViewModel = new CourseHole();
    }
}