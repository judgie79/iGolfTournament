using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TeamCreateViewModel
    {
        public TeamCreateViewModel()
        {
            this.Tournament = new TeamTournament();
        }

        public TeamCreateViewModel(Models.TeamTournament tournament)
        {
            this.Tournament = tournament;
            this.Teeboxes = tournament.Course.TeeBoxes;
        }

        public string TournamentId { get; set; }

        public string Name { get; set; }

        public TeamTournament Tournament { get; set; }

        [DisplayName("Teebox")]
        public string TeeboxId { get; set; }

        public TeeboxCollection Teeboxes { get; set; }
    }
}