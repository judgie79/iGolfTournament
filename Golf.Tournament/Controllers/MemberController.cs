using Golf.Tournament.Models;
using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class MemberController : BaseController
    {
        // GET: Participant
        [Route("tournaments/{tournamentId}/teams/{teamId}/members")]
        public async Task<ActionResult> Index(string tournamentId, string teamId)
        {
            var tournamentTask = loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
 
            await Task.WhenAll(tournamentTask);

            var tournament = tournamentTask.Result;
            var team = tournament.Teams.First(t => t.Id == teamId);


            return View(new ViewModels.MemberListViewModel(tournament, team));
        }

        // GET: Participant/Details/5
        [Route("tournaments/{tournamentId}/teams/{teamId}/members/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            return View();
        }

        // GET: Participant/Create
        [Route("tournaments/{tournamentId}/teams/{teamId}/members/create")]
        public async Task<ActionResult> Create(string tournamentId)
        {
            return View();
        }

        // POST: Participant/Create
        [HttpPost]
        [Route("tournaments/{tournamentId}/teams/{teamId}/members/create")]
        public async Task<ActionResult> Create(string tournamentId, string teamId, MemberCreateViewModel createViewModel)
        {
            //ModelState.Clear();

            if (ModelState.IsValid)
            {
                //var player = await loader.LoadAsync<Models.Player>("players/" + createViewModel.PlayerId);
                await loader.PostAsync<Models.TournamentParticipant, Models.TeamTournament>("tournaments/" + tournamentId + "/teams/" + teamId + "/members", new Models.TournamentParticipant()
                {
                    Id = createViewModel.ParticipantId,
                });

                return RedirectToAction("Index");
            }


            var tournament = loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            
            await Task.WhenAll(tournament);
            var participants = new TournamentParticipantCollection(tournament.Result.Participants.Where(o => !tournament.Result.Teams.Any(t => t.Members.Any(p => p.Id == o.Id))));

            var team = tournament.Result.Teams.First(t => t.Id == teamId);
            
            createViewModel.Participants = participants;
            createViewModel.Team = team;
            createViewModel.Tournament = tournament.Result;
            return View(createViewModel);
        }

        // GET: Participant/Delete/5
        [Route("tournaments/{tournamentId}/teams/{teamId}/members/{id}/delete")]
        public async Task<ActionResult> Delete(string tournamentId, string teamId, string id)
        {
            var tournament = await loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            var team = tournament.Teams.Single(p => p.Id == teamId);
            var member = team.Members.SingleOrDefault(p => p.Id == id);


            return View(new MemberDeleteViewModel()
            {
                Tournament = tournament,
                Member = member,
                Team = team
            });
        }

        // POST: Participant/Delete/5
        [HttpPost]
        [Route("tournaments/{tournamentId}/teams/{teamId}/members/{id}/delete")]
        public async Task<ActionResult> Delete(string tournamentId, string teamId, string id, MemberDeleteViewModel deleteViewModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tournamentId) && !string.IsNullOrWhiteSpace(id))
                {
                    await loader.DeleteAsync<Models.TeamTournament>("tournaments/" + tournamentId + "/teams/" + teamId + "/members/" + id);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(deleteViewModel);
            }
        }
    }
}