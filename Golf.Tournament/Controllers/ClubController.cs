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
    public class ClubController : BaseController
    {
        // GET: Club
        [Route("clubs")]
        public async Task<ActionResult> Index()
        {
            var clubs = await loader.Load<IEnumerable<Club>>("clubs");
            
            return View(clubs);
        }

        // GET: Club/Details/5
        [Route("clubs/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var club = loader.Load<Club>("clubs/" + id);

            var courses = loader.Load<IEnumerable<Course>>("clubs/" + id + "/courses");

            await Task.WhenAll(club, courses);

            return View(new ClubDetailsViewModel()
            {
                Club = club.Result,
                Courses = courses.Result
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
        public async Task<ActionResult> Create(ClubCreateViewModel clubCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var club = await loader.Post<Club>("clubs/", clubCreateViewModel.Club);


                clubCreateViewModel.Club = club;
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubCreateViewModel.Club);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{id}/edit")]
        public async Task<ActionResult> Edit(string id)
        {
            var club = loader.Load<Club>("clubs/" + id);

            var courses = loader.Load<IEnumerable<Course>>("clubs/" + id + "/courses");

            await Task.WhenAll(club, courses);

            return View(new ClubEditViewModel()
            {
                Club = club.Result,
                Courses = courses.Result
            });
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{id}/edit")]
        public async Task<ActionResult> Edit(string id, ClubEditViewModel clubEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var club = await loader.Put<Club>("clubs/" + id, clubEditViewModel.Club);
                clubEditViewModel.Club = club;
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubEditViewModel.Club);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{id}/delete")]
        public async Task<ActionResult> Delete(string id)
        {
            var club = await loader.Load<Club>("clubs/" + id);

            return View(club);
        }

        // GET: Club/Delete/5
        [HttpPost]
        [Route("clubs/{id}/delete")]
        public async Task<ActionResult> Delete(string id, FormCollection form)
        {
            var success = await loader.Delete<Club>("clubs/" + id);

            return RedirectToAction("Index");
        }
    }
}
