using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;

namespace Golf.Tournament.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }

        public PlayerCollection Players { get; set; }

        public IEnumerable<Models.Tournament> Tournaments { get; set; }
    }
}