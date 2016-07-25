using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TournamentParticipantCreateViewModel
    {
        public string TournamentId { get; set; }

        [DisplayName("Player")]
        public string PlayerId { get; set; }

        public PlayerCollection Players { get; set; }

        [DisplayName("Teebox")]
        public string TeeboxId { get; set; }

        public TeeboxCollection Teeboxes { get; set; }
    }
}