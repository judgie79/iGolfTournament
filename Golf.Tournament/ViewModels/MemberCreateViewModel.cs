using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class MemberCreateViewModel
    {
        public string TournamentId { get; set; }

        public Models.TeamTournament Tournament { get; set; }

        [DisplayName("Participant")]
        public string ParticipantId { get; set; }

        public Models.TournamentParticipantCollection Participants { get; set; }

        public Team Team { get; set; }
    }

    public class MemberEditViewModel
    {
        public Models.TeamTournament Tournament { get; set; }

        public Models.TournamentParticipant Member { get; set; }
    }

    public class MemberDeleteViewModel
    {
        public Models.TeamTournament Tournament { get; set; }

        public Models.Team Team { get; set; }

        public Models.TournamentParticipant Member { get; set; }
    }
}