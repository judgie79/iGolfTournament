using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TournamentEditViewModel<TTournament>
         where TTournament : Models.Tournament, new()
    {
        public TournamentEditViewModel()
        {
            Tournament = new TTournament();
        }

        public TTournament Tournament { get; set; }
    }
}