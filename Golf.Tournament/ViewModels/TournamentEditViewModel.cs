using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Models
{
    public class TournamentEditViewModel
    {
        public TournamentEditViewModel()
        {
            Tournament = new Tournament();
        }

        public Tournament Tournament { get; set; }
    }
}