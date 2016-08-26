using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class HoleDetailsViewModel
    {
        public Club Club { get; set; }

        public Hole Hole { get; set; }
    }
}