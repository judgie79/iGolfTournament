using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TournamentParticipantEditViewModel
    {
        public TournamentParticipant Participant { get; set; }

        public Models.Tournament Tournament { get; set; }

        public TeeboxCollection Teeboxes { get; set; }

        public Course Course { get; internal set; }
    }
}