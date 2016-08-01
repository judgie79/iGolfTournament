using Golf.Tournament.Models;
using System.Linq;

namespace Golf.Tournament.ViewModels
{
    public class MemberListViewModel
    {
        public Models.TeamTournament Tournament { get; set; }
        public Models.Team Team { get; set; }

        public MemberListViewModel()
            : this(new TeamTournament(), new Team())
        {
        }

        public MemberListViewModel(Models.TeamTournament tournament, Team team)
        {
            Tournament = tournament;
            Team = team;

            this.MemberCreateViewModel = new MemberCreateViewModel()
            {
                Team = team,
                TournamentId = tournament.Id,
                Participants = new TournamentParticipantCollection(tournament.Participants.Where(o => !tournament.Teams.Any(t => t.Members.Any(p => p.Id == o.Id))))

            };
        }

        public TournamentParticipant LabelModel = new TournamentParticipant();

        public MemberCreateViewModel MemberCreateViewModel { get; set; }
    }
}