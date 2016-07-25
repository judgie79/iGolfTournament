using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Models
{
    public class TournamentCreateViewModel
    {
        public TournamentCreateViewModel()
        {
            Tournament = new Tournament();
        }

        public Tournament Tournament { get; set; }

        public IEnumerable<Club> Clubs { get; set; }

        public string ClubId { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public string CourseId { get; set; }
    }

    
}