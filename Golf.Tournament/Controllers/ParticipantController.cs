using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class ParticipantController : BaseController
    {
        // GET: Participant
        [Route("tournaments/{tournamentId}/participants")]
        public ActionResult Index(string tournamentId)
        {
            var tournament = loader.Load<Models.Tournament>("tournaments/" + tournamentId);
            var players = loader.Load<Models.PlayerCollection>("players");
            tournament.Participants = tournament.Participants ?? new Models.TournamentParticipantCollection();

            return View(new ViewModels.TournamentParticipantListViewModel(tournament, players));
        }

        // GET: Participant/Details/5
        [Route("tournaments/{tournamentId}/participants/{id}")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Participant/Create
        [Route("tournaments/{tournamentId}/participants/create")]
        public ActionResult Create(string tournamentId)
        {
            return View();
        }

        // POST: Participant/Create
        [HttpPost]
        [Route("tournaments/{tournamentId}/participants/create")]
        public ActionResult Create(string tournamentId, TournamentParticipantCreateViewModel createViewModel)
        {
            ModelState.Clear();
            TryValidateModel(createViewModel.PlayerId);
            TryValidateModel(createViewModel.TeeboxId);

            if (ModelState.IsValid)
            {
                var player = loader.Load<Models.Player>("players/" + createViewModel.PlayerId);
                Models.Tournament tournamentResult = loader.Post<Models.TournamentParticipant, Models.Tournament>("tournaments/" + tournamentId + "/participants", new Models.TournamentParticipant()
                {
                    Player = player,
                    TeebBoxId = createViewModel.TeeboxId
                });

                return RedirectToAction("Index");
            }


            var tournament = loader.Load<Models.Tournament>("tournaments/" + tournamentId);
            var players = loader.Load<Models.PlayerCollection>("players");

            createViewModel.Teeboxes = tournament.Course.TeeBoxes;
            createViewModel.Players = players;
            return View(createViewModel);
        }

        // GET: Participant/Edit/5
        [Route("tournaments/{tournamentId}/participants/{id}/edit")]
        public ActionResult Edit(string tournamentId, string id)
        {
            var tournament = loader.Load<Models.Tournament>("tournaments/" + tournamentId);

            return View(new TournamentParticipantEditViewModel()
            {
                Tournament = tournament,
                Participant = tournament.Participants.SingleOrDefault(p => p.Id == id),
                Teeboxes = tournament.Course.TeeBoxes,
                Course = tournament.Course
            });
        }

        // POST: Participant/Edit/5
        [HttpPost]
        [Route("tournaments/{tournamentId}/participants/{id}/edit")]
        public ActionResult Edit(string tournamentId, string id, [ModelBinder(typeof(TournamentParticipantEditViewModelBinder))]TournamentParticipantEditViewModel editViewModel)
        {
            ModelState.Clear();
            TryValidateModel(editViewModel.Participant);

            if (ModelState.IsValid)
            {
                var player = loader.Load<Models.Player>("players/" + editViewModel.Participant.Player.Id);
                var tournamentResult = loader.Put<Models.TournamentParticipant, Models.Tournament>("tournaments/" + tournamentId + "/" + "participants/" + editViewModel.Participant.Id, editViewModel.Participant);

                return RedirectToAction("Index");
            }
            else
            {
                var tournament = loader.Load<Models.Tournament>("tournaments/" + tournamentId);
                editViewModel.Tournament = tournament;
                editViewModel.Course = tournament.Course;
                editViewModel.Teeboxes = tournament.Course.TeeBoxes;
                return View(editViewModel);
            }
        }

        // GET: Participant/Delete/5
        [Route("tournaments/{tournamentId}/participants/{id}/delete")]
        public ActionResult Delete(string tournamentId, string id)
        {
            var tournament = loader.Load<Models.Tournament>("tournaments/" + tournamentId);
            var participant = tournament.Participants.SingleOrDefault(p => p.Id == id);
            return View(new TournamentParticipantDeleteViewModel()
            {
                Tournament = tournament,
                Participant = participant
            });
        }

        // POST: Participant/Delete/5
        [HttpPost]
        [Route("tournaments/{tournamentId}/participants/{id}/delete")]
        public ActionResult Delete(string tournamentId, string id, TournamentParticipantDeleteViewModel deleteViewModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tournamentId) && !string.IsNullOrWhiteSpace(id))
                {
                    var player = loader.Delete<Models.Player>("tournaments/" + tournamentId + "/participants/" + id);
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
