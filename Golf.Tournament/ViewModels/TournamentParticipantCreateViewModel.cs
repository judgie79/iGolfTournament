using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TournamentParticipantCreateViewModel
    {
        public string TournamentId { get; set; }

        public string PlayerId { get; set; }

        public PlayerCollection Players { get; set; }

        public string TeeboxId { get; set; }

        public TeeboxCollection Teeboxes { get; set; }
    }

    public class TournamentParticipantEditViewModel
    {
        public TournamentParticipant Participant { get; set; }
    }
}