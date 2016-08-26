using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;

namespace Golf.Tournament.ViewModels
{
    public class HoleEditViewModel
    {
        public Club Club { get; internal set; }
        public Hole Hole { get; internal set; }

        public HttpPostedFileBase CourseImageFile { get; set; }
    }
}