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
        public string TournamentId { get; set; }

        public string Name { get; set; }

        [DisplayName("Teebox")]
        public string TeeboxId { get; set; }

        public TeeboxCollection Teeboxes { get; set; }
    }
}