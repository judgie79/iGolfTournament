using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TournamentDetailsViewModel
    {
        public TournamentDetailsViewModel()
        {
            Tournament = new Models.Tournament();
        }

        public Models.Tournament Tournament { get; set; }
    }
}