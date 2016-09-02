using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TournamentDetailsViewModel : TournamentDetailsViewModel<Models.Tournament>
    {
    }

    public class TeamTournamentDetailsViewModel : TournamentDetailsViewModel<Models.TeamTournament>
    {
    }

    public class TournamentDetailsViewModel<TTournament>
        where TTournament : Models.Tournament, new()
    {
        public TournamentDetailsViewModel()
        {
            Tournament = new TTournament();
        }

        public TTournament Tournament { get; set; }
    }
}