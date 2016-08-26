using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class HoleCreateViewModel
    {
        public Club Club { get; internal set; }
        public Hole Hole { get; set; }
    }
}