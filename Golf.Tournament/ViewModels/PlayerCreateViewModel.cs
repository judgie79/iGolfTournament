using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class PlayerCreateViewModel
    {
        public PlayerCreateViewModel()
        {
            Player = new Player();
        }

        public Player Player { get; set; }

        public IEnumerable<HomeClub> Clubs { get; set; }
    }
}