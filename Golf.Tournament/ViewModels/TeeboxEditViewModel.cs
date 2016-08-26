using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TeeboxEditViewModel
    {
        public TeeboxEditViewModel()
        {

        }

        public TeeboxEditViewModel(Club club, Course course, TeeBox teebox)
            : this()
        {
            this.Course = course;
            this.Club = club;
            this.Teebox = teebox;
        }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public TeeBox Teebox { get; set; }
    }

    public class CourseHoleEditViewModel
    {
        public CourseHoleEditViewModel()
        {

        }

        public CourseHoleEditViewModel(Club club, Course course, TeeBox teebox, FrontOrBack frontOrBack, CourseHole hole)
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

    public enum FrontOrBack
    {
        Front,
        Back
    }



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

    public class CourseHoleCreateViewModel
    {
        public CourseHoleCreateViewModel()
        {

        }

        public CourseHoleCreateViewModel(Club club, Course course, TeeBox teebox, FrontOrBack frontOrBack)
            : this()
        {
            this.Course = course;
            this.Club = club;
            this.Teebox = teebox;
            this.FrontOrBack = frontOrBack;
        }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public TeeBox Teebox { get; set; }

        public FrontOrBack FrontOrBack { get; set; }

        public HoleCollection Holes { get; set; }

        public CourseHole Hole { get; set; }
    }

    public class CourseHoleDeleteViewModel
    {
        public CourseHoleDeleteViewModel()
        {

        }

        public CourseHoleDeleteViewModel(Club club, Course course, TeeBox teebox, FrontOrBack frontOrBack, CourseHole hole)
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

        public TeeBox Teebox { get; set; }

        public FrontOrBack FrontOrBack { get; set; }

        public CourseHole Hole { get; set; }
    }

    public class TeeboxDeleteViewModel
    {
        public TeeboxDeleteViewModel()
        {

        }

        public TeeboxDeleteViewModel(Club club, Course course, TeeBox teebox)
            : this()
        {
            this.Course = course;
            this.Club = club;
            this.Teebox = teebox;
        }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public FrontOrBack FrontOrBack { get; set; }

        public TeeBox Teebox { get; set; }
    }
}