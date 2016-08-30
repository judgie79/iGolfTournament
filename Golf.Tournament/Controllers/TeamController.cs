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
    public class TeamController : BaseController
    {
        // GET: Team
        [Route("tournaments/{tournamentId}/teams")]
        public async Task<ActionResult> Index(string tournamentId)
        {
            var tournament = await loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            return View(new TeamListViewModel(tournament));
        }

        [Route("tournaments/{tournamentId}/teams/{id}")]
        public async Task<ActionResult> Details(string tournamentId, string id)
        {
            var tournament = await loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            var team = tournament.Teams.First(t => t.Id == id);

            return View(new TeamDetailsViewModel()
            {
                Tournament = tournament,
                Team = team
            });
        }

        // GET: Team Create
        [Route("tournaments/{tournamentId}/teams/create")]
        public async Task<ActionResult> Create(string tournamentId)
        {
            var tournament = await loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            return View(new TeamCreateViewModel(tournament));
        }

        [Route("tournaments/{tournamentId}/teams/create")]
        [HttpPost]
        public async Task<ActionResult> Create(string tournamentId, TeamCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await loader.PostAsync<Team>("tournaments/" + tournamentId + "/teams", new Team()
                {
                    Name = viewModel.Name,
                    TeeboxId = viewModel.TeeboxId
                });

                return RedirectToAction("Index");
            }

            var tournament = await loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            
            

            return View(new TeamCreateViewModel(tournament));
        }

        // GET: Team Create
        [Route("tournaments/{tournamentId}/teams/{id}/edit")]
        public async Task<ActionResult> Edit(string tournamentId, string id)
        {
            var tournament = await loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            var team = tournament.Teams.First(t => t.Id == id);

            return View(new TeamEditViewModel()
            {
                Tournament = tournament,
                Team = team,
                Course = tournament.Course,
                Teeboxes = tournament.Course.TeeBoxes
            });
        }

        [Route("tournaments/{tournamentId}/teams/{id}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(string tournamentId, string id, [ModelBinder(typeof(TeamEditViewModelBinder))] TeamEditViewModel viewModel)
        {
            ModelState.Clear();

            TryValidateModel(viewModel.Team);

            if (ModelState.IsValid)
            {
                await loader.PutAsync<Team>("tournaments/" + tournamentId + "/teams/" + id, viewModel.Team);

                return RedirectToAction("Index");
            }

            var tournament = await loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            var team = tournament.Teams.First(t => t.Id == id);
            return View(new TeamEditViewModel()
            {
                Tournament = tournament,
                Team = team,
                Course = tournament.Course,
                Teeboxes = tournament.Course.TeeBoxes
            });
        }

        // GET: Participant/Delete/5
        [Route("tournaments/{tournamentId}/teams/{id}/delete")]
        public async Task<ActionResult> Delete(string tournamentId, string id)
        {
            var tournament = await loader.LoadAsync<Models.TeamTournament>("tournaments/" + tournamentId);
            var team = tournament.Teams.SingleOrDefault(p => p.Id == id);


            return View(new TeamDeleteViewModel()
            {
                Tournament = tournament,
                Team = team
            });
        }

        // POST: Participant/Delete/5
        [HttpPost]
        [Route("tournaments/{tournamentId}/teams/{id}/delete")]
        public async Task<ActionResult> Delete(string tournamentId, string id, TeamDeleteViewModel deleteViewModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tournamentId) && !string.IsNullOrWhiteSpace(id))
                {
                    await loader.DeleteAsync<Models.Player>("tournaments/" + tournamentId + "/teams/" + id);
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