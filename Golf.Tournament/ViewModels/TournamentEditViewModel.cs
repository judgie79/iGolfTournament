using Golf.Tournament.ViewModels;
using System.Collections.Generic;

namespace Golf.Tournament.Models
{
    public class TournamentEditViewModel
    {
        public TournamentEditViewModel()
        {
            Tournament = new Tournament();
        }

        public Tournament Tournament { get; set; }
    }
}