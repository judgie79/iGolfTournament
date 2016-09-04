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
            var clubs = await loader.LoadClubs();
            
            return View(clubs);
        }

        // GET: Club/Details/5
        [Route("clubs/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var club = loader.LoadClub(id);

            var courses = loader.LoadCoursesOfClub(id);
            var holes = loader.LoadHolesOfClub(id);

            await Task.WhenAll(club, courses, holes);

            return View(new ClubDetailsViewModel()
            {
                Club = club.Result,
                Courses = courses.Result,
                Holes = holes.Result
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
                var clubId = await loader.CreateClub(clubCreateViewModel.Club);

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
            var club = loader.LoadClub(id);

            var courses = loader.LoadCoursesOfClub(id);
            var holes = loader.LoadHolesOfClub(id);

            await Task.WhenAll(club, courses, holes);

            return View(new ClubEditViewModel()
            {
                Club = club.Result,
                Courses = courses.Result,
                Holes = holes.Result
            });
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{id}/edit")]
        public async Task<ActionResult> Edit(string id, ClubEditViewModel clubEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var club = await loader.UpdateClub(clubEditViewModel.Club);
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
            var club = await loader.LoadClub(id);

            return View(club);
        }

        // GET: Club/Delete/5
        [HttpPost]
        [Route("clubs/{id}/delete")]
        public async Task<ActionResult> Delete(string id, FormCollection form)
        {
            var success = await loader.DeleteClub(id);

            return RedirectToAction("Index");
        }
    }
}
