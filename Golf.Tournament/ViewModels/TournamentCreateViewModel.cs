using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TournamentCreateViewModel<TTournament>
        where TTournament : Models.Tournament, new()
    {
        public TournamentCreateViewModel()
        {
            Tournament = new TTournament();
        }

        public TTournament Tournament { get; set; }

        public IEnumerable<Club> Clubs { get; set; }

        public string ClubId { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public string CourseId { get; set; }

        public TournamentType TournamenType { get; set; }
    }

    public enum TournamentType
    {
        Stableford_Single,
        Stableford_Team,
        //StrokePlay_Single,
        //StrokePlay_Team,
        //Other
    }
}