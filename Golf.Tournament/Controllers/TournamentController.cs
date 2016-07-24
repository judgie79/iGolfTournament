﻿using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class TournamentController : BaseController
    {

        // GET: Tournament
        [Route("tournaments")]
        public ActionResult Index()
        {
            var tournaments = loader.Load<IEnumerable<Models.Tournament>>("tournaments");

            return View(new TournamentListViewModel(tournaments));
        }

        // GET: Tournament/Details/5
        [Route("tournaments/{id}")]
        public ActionResult Details(string id)
        {
            var tournament = loader.Load<Models.Tournament>("tournaments/" + id);

            var viewModel = new ViewModels.TournamentDetailsViewModel()
            {
                Tournament = tournament
            }; 

            return View(viewModel);
        }

        // GET: Tournament/Create
        [Route("tournaments/create")]
        public ActionResult Create(string clubId = null, string courseId = null)
        {
            var viewModel = new Models.TournamentCreateViewModel()
            {
                Clubs = loader.Load<IEnumerable<Models.Club>>("clubs"),
                Courses = loader.Load<IEnumerable<Models.Course>>("courses"),
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
        public ActionResult Create([ModelBinder(typeof(Models.TournamentCreateViewModelBinder))]Models.TournamentCreateViewModel tournament)
        {
            tournament.Clubs = loader.Load<IEnumerable<Models.Club>>("clubs");
            tournament.Courses = loader.Load<IEnumerable<Models.Course>>("courses");

            if (ModelState.IsValid)
            {

                loader.Post<Models.Tournament>("tournaments/", tournament.Tournament);

                return RedirectToAction("Index");
            } else
            {
                return View(tournament);
            }
        }

        // GET: Tournament/Edit/5
        [Route("tournaments/{id}/edit")]
        public ActionResult Edit(string id)
        {
            var tournament = loader.Load<Models.Tournament>("tournaments/" + id);

            var viewModel = new Models.TournamentEditViewModel()
            {
                Tournament = tournament
            };

            return View(viewModel);
        }

        // POST: Tournament/Edit/5
        [HttpPost]
        [Route("tournaments/{id}/edit")]
        public ActionResult Edit(string id, [ModelBinder(typeof(Models.TournamentEditViewModelBinder))]Models.TournamentEditViewModel tournamentViewModel)
        {
            var tournament = loader.Load<Models.Tournament>("tournaments/" + id);
            tournamentViewModel.Tournament.Participants = tournament.Participants;
            tournamentViewModel.Tournament.Club = tournament.Club;
            tournamentViewModel.Tournament.Course = tournament.Course;

            ModelState.Clear();
            TryValidateModel(tournamentViewModel.Tournament);

            if (ModelState.IsValid)
            {
                

                

                loader.Put<Models.Tournament>("tournaments/" + id, tournamentViewModel.Tournament);

                return RedirectToAction("Index");
            }
            else
            {
                return View(tournamentViewModel);
            }
        }

        // GET: Tournament/Delete/5
        [Route("tournaments/{id}/delete")]
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: Tournament/Delete/5
        [HttpPost]
        [Route("tournaments/{id}/delete")]
        public ActionResult Delete(string id, FormCollection collection)
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
    }
}
