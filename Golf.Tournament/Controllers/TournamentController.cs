using Golf.Tournament.Utility;
using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class TournamentController : BaseController
    {

        // GET: Tournament
        [Route("tournaments")]
        public async Task<ActionResult> Index()
        {
            var tournaments = await loader.LoadAsync<IEnumerable<Models.Tournament>>("tournaments");
            return View("Index", new TournamentListViewModel(tournaments));
        }

        // GET: Tournament/Details/5
        [Route("tournaments/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var tournament = await loader.LoadAsync<Models.Tournament>("tournaments/" + id);

            if (tournament.TournamentType == Models.TournamentType.Single)
            {
                var viewModelSingleTournament = new TournamentDetailsViewModel()
                {
                    Tournament = tournament
                };

                return View("Details", viewModelSingleTournament);
            } else if (tournament.TournamentType == Models.TournamentType.Team)
            {
                var viewModelTeamTournament = new TeamTournamentDetailsViewModel()
                {
                    Tournament = (Models.TeamTournament)tournament
                };

                return View("DetailsTeam", viewModelTeamTournament);
            }




            

            var viewModel = new ViewModels.TournamentDetailsViewModel()
            {
                Tournament = tournament
            }; 

            return View(viewModel);
        }

        // GET: Tournament/Create
        [Route("tournaments/create")]
        public async Task<ActionResult> Create(string clubId = null, string courseId = null)
        {
            var clubs = loader.LoadAsync<IEnumerable<Models.Club>>("clubs");
            var courses = loader.LoadAsync<Models.CourseCollection>("courses");

            await Task.WhenAll(clubs, courses);
            await loader.SetHoles(courses.Result);
            var viewModel = new TournamentCreateViewModel<Models.Tournament>()
            {
                Clubs = clubs.Result,
                Courses = courses.Result,
                ClubId = clubId,
                CourseId = courseId,
                Tournament = new Models.Tournament()
                {

                }
            };

            return View(viewModel);
        }

        // POST: Tournament/Create
        [HttpPost]
        [Route("tournaments/create")]
        public async Task<ActionResult> Create([ModelBinder(typeof(TournamentCreateViewModelBinder))]TournamentCreateViewModel<Models.Tournament> tournament)
        {

            var clubs = loader.LoadAsync<IEnumerable<Models.Club>>("clubs");
            var courses = loader.LoadAsync<Models.CourseCollection>("courses");

            await Task.WhenAll(clubs, courses);
            await loader.SetHoles(courses.Result);
            tournament.Clubs = clubs.Result;
            tournament.Courses = courses.Result;

            if (ModelState.IsValid)
            {

                await loader.PostAsync<Models.Tournament>("tournaments/", tournament.Tournament);

                return RedirectToAction("Index");
            } else
            {
                return View(tournament);
            }
        }

        // GET: Tournament/Edit/5
        [Route("tournaments/{id}/edit")]
        public async Task<ActionResult> Edit(string id)
        {
            var tournament = await loader.LoadAsync<Models.Tournament>("tournaments/" + id);

            var viewModel = new TournamentEditViewModel<Models.Tournament>()
            {
                Tournament = tournament
            };

            return View(viewModel);
        }

        // POST: Tournament/Edit/5
        [HttpPost]
        [Route("tournaments/{id}/edit")]
        public async Task<ActionResult> Edit(string id, [ModelBinder(typeof(TournamentEditViewModelBinder))]TournamentEditViewModel<Models.Tournament> tournamentViewModel)
        {
            var tournament = await loader.LoadAsync<Models.Tournament>("tournaments/" + id);
            tournamentViewModel.Tournament.Participants = tournament.Participants;
            tournamentViewModel.Tournament.Club = tournament.Club;
            tournamentViewModel.Tournament.Course = tournament.Course;

            

            if(tournament.TournamentType == Models.TournamentType.Team)
            {
                ((Models.TeamTournament)tournamentViewModel.Tournament).Teams = ((Models.TeamTournament)tournament).Teams;
            }

            ModelState.Clear();
            TryValidateModel(tournamentViewModel.Tournament);

            if (ModelState.IsValid)
            {
                await loader.PutAsync<Models.Tournament>("tournaments/" + id, tournamentViewModel.Tournament);

                return RedirectToAction("Index");
            }
            else
            {
                return View(tournamentViewModel);
            }
        }

        // GET: Tournament/Delete/5
        [Route("tournaments/{id}/delete")]
        public async Task<ActionResult> Delete(string id)
        {
            return View();
        }

        // POST: Tournament/Delete/5
        [HttpPost]
        [Route("tournaments/{id}/delete")]
        public async Task<ActionResult> Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tournament/Edit/5
        [Route("tournaments/{id}/start")]
        public async Task<ActionResult> Start(string id)
        {
            var tournament = await loader.LoadAsync<Models.Tournament>("tournaments/" + id);

            

            if(tournament.TournamentType == Models.TournamentType.Single)
            {
                var viewModelSingleTournament = new TournamentEditViewModel<Models.Tournament>()
                {
                    Tournament = tournament
                };

                return View("Start", viewModelSingleTournament);
            } else if (tournament.TournamentType == Models.TournamentType.Team)
            {
                var viewModelTeamTournament = new TournamentEditViewModel<Models.TeamTournament>()
                {
                    Tournament = (Models.TeamTournament)tournament
                };

                return View("StartTeam", viewModelTeamTournament);
            }

            var viewModel = new TournamentEditViewModel<Models.Tournament>()
            {
                Tournament = tournament
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("tournaments/{id}/start")]
        public async Task<ActionResult> Start(string id, [ModelBinder(typeof(ViewModels.TournamentStartViewModelBinder))]TournamentEditViewModel<Models.Tournament> tournamentViewModel)
        {
            var tournament = await loader.LoadAsync<Models.Tournament>("tournaments/" + id);
            tournamentViewModel.Tournament = tournament;

            ModelState.Clear();
            TryValidateModel(tournamentViewModel.Tournament);

            if (ModelState.IsValid)
            {

                await loader.PutAsync<Models.Tournament>("tournaments/" + id + "/start", tournamentViewModel.Tournament);

                return RedirectToAction("Index");
    }
            else
            {
                return View(tournamentViewModel);
}
        }

        [Route("tournaments/{id}/scorecards")]
        public async Task<ActionResult> ScoreCards(string id)
        {
            var tournament = await loader.LoadAsync<Models.Tournament>("tournaments/" + id);

            return View(tournament);
        }

        [Route("tournaments/calendar")]
        public ActionResult Calendar()
        {
            return View();
        }

        [Route("tournaments/events")]
        public async Task<JsonResult> Events(long from=1470002400000, long to=1472680800000, int utc_offset_from=-120, int utc_offset_to=-120)
        {
            var tournaments = await loader.LoadAsync<IEnumerable<Models.Tournament>>("tournaments");

            var result = new
            {
                success = true,
                result = tournaments.Select(t => new {
                    id = t.Id,
                   title = t.Title,
                   url = Url.Action("Details", "Tournament", new { id = t.Id }),
                   @class = t.TournamentType == Models.TournamentType.Single ? "event-info" : "event-warning",
                   start= t.Date.ToUnixTime() * 1000, // Milliseconds
                   end = t.Date.AddHours(5).ToUnixTime() * 1000 // Milliseconds
                }).Where(t => t.start >= from && t.start <= to)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
