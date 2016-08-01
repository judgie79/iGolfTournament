using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TeamListViewModel
    {
        public TeamListViewModel(TeamTournament tournament)
        {
            this.Tournament = tournament;
            if (this.Tournament.Teams == null)
                this.Tournament.Teams = new TeamCollection();
            this.ViewModel = new Team();
        }
        
        public TeamTournament Tournament { get; private set; }

        public Team ViewModel { get; set; }
    }
}