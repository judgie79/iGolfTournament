using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;

namespace Golf.Tournament.ViewModels
{
    public class HoleDeleteViewModel
    {
        public Club Club { get; internal set; }
        public Hole Hole { get; internal set; }
    }
}