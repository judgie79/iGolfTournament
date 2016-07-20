using System.Collections.Generic;

namespace Golf.Tournament.Models
{
    public class TournamentEditViewModel
    {
        public TournamentEditViewModel()
        {
        }

        public IEnumerable<Club> Clubs { get; set; }
        public IEnumerable<Course> Courses { get; set; }

        public Tournament Tournament { get; set; }
    }
}