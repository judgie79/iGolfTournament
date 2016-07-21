﻿using Golf.Tournament.Models;
using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class ClubController : BaseController
    {
        GolfLoader loader;

        public ClubController()
        {
            loader = new GolfLoader("http://localhost:8080/api/");
        }
        
        
        // GET: Club
        [Route("clubs")]
        public ActionResult Index()
        {
            var clubs = loader.Load<IEnumerable<Club>>("clubs");

            return View(clubs);
        }

        // GET: Club/Details/5
        [Route("clubs/{id}")]
        public ActionResult Details(string id)
        {
            var club = loader.Load<Club>("clubs/" + id);

            var courses = loader.Load<IEnumerable<Course>>("clubs/" + id + "/courses");

            return View(new ClubDetailsViewModel()
            {
                Club = club,
                Courses = courses
            });
        }

        // GET: Club/Create
        [Route("clubs/create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Club/Create
        [HttpPost]
        [Route("clubs/create")]
        public ActionResult Create(ClubCreateViewModel clubCreateViewModel)
        {
            if (ModelState.IsValid)
                {
                    clubCreateViewModel.Club = loader.Post<Club>("clubs/", clubCreateViewModel.Club);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(clubCreateViewModel.Club);
                }
        }

        // GET: Club/Edit/5
        [Route("clubs/{id}/edit")]
        public ActionResult Edit(string id)
        {
            var club = loader.Load<Club>("clubs/" + id);

            return View(club);
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{id}/edit")]
        public ActionResult Edit(string id, ClubEditViewModel clubEditViewModel)
        {
            if (ModelState.IsValid)
            {
                clubEditViewModel.Club = loader.Put<Club>("clubs/" + id, clubEditViewModel.Club);

                return RedirectToAction("Index");
            }
            else
            {
                return View(clubEditViewModel.Club);
            }
        }

        // GET: Club/Delete/5
        [HttpPost]
        [Route("clubs/{id}/delete")]
        public ActionResult Delete(string id)
        {
            loader.Delete<Club>("clubs/" + id);

                return RedirectToAction("Index");
        }
    }
}
