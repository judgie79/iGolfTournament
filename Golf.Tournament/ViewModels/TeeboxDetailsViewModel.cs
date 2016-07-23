using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TeeboxDetailsViewModel
    {
        public TeeboxDetailsViewModel()
        {

        }

        public TeeboxDetailsViewModel(Club club, Course course, TeeBox teebox)
        {
            this.Club = club;
            this.Course = course;
            this.Teebox = teebox;
        }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public TeeBox Teebox { get; set; }
    }
}