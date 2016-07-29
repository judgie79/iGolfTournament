using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TeamDeleteViewModel
    {
        public Models.Tournament Tournament { get; set; }

        public Models.Team Team { get; set; }
    }
}