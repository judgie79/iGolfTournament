using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;
using Golf.Models.Reports;

namespace Golf.Tournament.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }

        public PlayerCollection Players { get; set; }

        public IEnumerable<Models.Tournament> Tournaments { get; set; }

        public IEnumerable<ClubReport> ClubReports { get; set; }
    }
}