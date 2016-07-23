using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;

namespace Golf.Tournament.ViewModels
{
    public class TeeboxCreateViewModel
    {
        public TeeboxCreateViewModel()
        {
            this.Teebox = new TeeBox();
        }

        public TeeboxCreateViewModel(Club club, Course course)
            : this()
        {
            this.Course = course;
            this.Club = club;
            
        }

        public Club Club { get; set; }

        public Course Course { get; set; }

        public TeeBox Teebox { get; set; }
    }

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
}