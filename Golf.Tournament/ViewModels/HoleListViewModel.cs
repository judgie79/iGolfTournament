using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class HoleListViewModel
    {
        public HoleListViewModel()
        {

        }

        public HoleListViewModel(Club club, HoleCollection holes)
        {
            this.Club = club;
            this.Holes = holes;
        }
        public HoleCollection Holes { get; set; }

        public Club Club { get; set; }

        public Hole ViewModel = new Hole();
    }
}