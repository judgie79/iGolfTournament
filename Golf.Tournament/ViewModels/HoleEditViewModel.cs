using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;

namespace Golf.Tournament.ViewModels
{
    public class HoleEditViewModel
    {
        public HoleEditViewModel()
        {
            Hole = new Hole();
        }

        public Club Club { get; set; }

        public Hole Hole { get; set; }

        public HttpPostedFileBase CourseImageFile { get; set; }
    }
}