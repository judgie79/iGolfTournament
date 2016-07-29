using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TeamEditViewModel
    {
        public Team Team { get; set; }

        public Models.Tournament Tournament { get; set; }

        public TeeboxCollection Teeboxes { get; set; }

        public Course Course { get; internal set; }
    }
}