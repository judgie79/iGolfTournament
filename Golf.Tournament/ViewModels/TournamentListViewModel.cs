using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class TournamentParticipantListViewModel
    {
        public TournamentParticipantListViewModel()
        {
            ParticipantCreateViewModel = new TournamentParticipantCreateViewModel();
        }

        public TournamentParticipantListViewModel(Models.Tournament tournament, PlayerCollection players)
        {
            ParticipantCreateViewModel = new TournamentParticipantCreateViewModel()
            {
                Players = new PlayerCollection(players.Where(o => !tournament.Participants.Any(p => p.Player.Id == o.Id))),
                TournamentId = tournament.Id,
                Teeboxes = tournament.Course.TeeBoxes
            };
            Participants = tournament.Participants;
            Tournament = tournament;
           
            this.Teeboxes = tournament.Course.TeeBoxes.ToDictionary(t => t.Id, t => t);
        }

        public Dictionary<string, TeeBox> Teeboxes { get; private set; }

        public TournamentParticipantCreateViewModel ParticipantCreateViewModel { get; set; }

        public Golf.Tournament.Models.Tournament Tournament { get; set; }

        public TournamentParticipantCollection Participants { get; set; }

        public Golf.Tournament.Models.TournamentParticipant LabelModel = new Models.TournamentParticipant();
    }

    public class TournamentListViewModel
    {
        public TournamentListViewModel()
        {

        }

        public TournamentListViewModel(IEnumerable<Models.Tournament> tournaments)
        {
            Tournaments = tournaments;
        }

        public IEnumerable<Models.Tournament> Tournaments { get; set; }

        public Golf.Tournament.Models.Tournament LabelModel = new Models.Tournament();

    }
}