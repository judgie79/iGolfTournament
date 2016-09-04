using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class CourseCreateViewModel : CourseViewModel
    {
        public Club Club { get; set; }
        public IEnumerable<Club> Clubs { get; set; }
    }
}