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

        public TeeBox Teebox { get; set; }
    }
}