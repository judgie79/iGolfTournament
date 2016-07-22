using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TournamentParticipantDeleteViewModel
    {
        public Models.Tournament Tournament { get; set; }

        public Models.TournamentParticipant Participant { get; set; }
    }
}