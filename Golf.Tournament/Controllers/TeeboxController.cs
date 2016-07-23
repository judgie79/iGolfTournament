using Golf.Tournament.Models;
using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class TeeboxController : BaseController
    {
        // GET: Club
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes")]
        public ActionResult Index(string clubId, string courseId)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);


            return View(new TeeboxListViewModel(club, course));
        }

        // GET: Club/Details/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}")]
        public ActionResult Details(string clubId, string courseId, string id)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);

            return View(new TeeboxDetailsViewModel()
            {
                Club = club,
                Course = course,
                Teebox = course.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // GET: Club/Create
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/create")]
        public ActionResult Create(string clubId, string courseId)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);

            return View(new TeeboxCreateViewModel()
            {
                Club = club,
                Course = course
            });
        }

        // POST: Club/Create
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/create")]
        public ActionResult Create(string clubId, string courseId, TeeboxCreateViewModel teeboxCreateViewModel)
        {
            if (ModelState.IsValid)
            {

                teeboxCreateViewModel.Course = loader.Load<Course>("courses/" + courseId);
                teeboxCreateViewModel.Course.TeeBoxes.Add(teeboxCreateViewModel.Teebox);
                teeboxCreateViewModel.Course = loader.Put<Course>("courses/" + courseId, teeboxCreateViewModel.Course);
                return RedirectToAction("Index");
            }
            else
            {
                var club = loader.Load<Club>("clubs/" + clubId);
                var course = loader.Load<Course>("courses/" + courseId);
                teeboxCreateViewModel.Club = club;
                teeboxCreateViewModel.Course = course;

                return View(teeboxCreateViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/edit")]
        public ActionResult Edit(string clubId, string courseId, string id)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);

            var courses = loader.Load<IEnumerable<Course>>("clubs/" + id + "/courses");

            return View(new TeeboxEditViewModel()
            {
                Club = club,
                Course = course,
                Teebox = course.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/edit")]
        public ActionResult Edit(string clubId, string courseId, string id, TeeboxEditViewModel teeboxEditViewModel)
        {
            if (ModelState.IsValid)
            {
                teeboxEditViewModel.Course = loader.Load<Course>("courses/" + courseId);
                var teebox = teeboxEditViewModel.Course.TeeBoxes.SingleOrDefault(t => t.Id == id);


                teeboxEditViewModel.Course.TeeBoxes[teeboxEditViewModel.Course.TeeBoxes.IndexOf(teebox)] = teeboxEditViewModel.Teebox;

                teeboxEditViewModel.Course = loader.Post<Course>("clubs/", teeboxEditViewModel.Course);
                return RedirectToAction("Index");
            }
            else
            {
                var club = loader.Load<Club>("clubs/" + clubId);
                var course = loader.Load<Course>("courses/" + courseId);
                teeboxEditViewModel.Club = club;
                teeboxEditViewModel.Course = course;

                return View(teeboxEditViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/delete")]
        public ActionResult Delete(string clubId, string courseId, string id)
        {
            var club = loader.Load<Club>("clubs/" + id);

            return View(club);
        }

        // GET: Club/Delete/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/delete")]
        public ActionResult Delete(string clubId, string courseId, string id, FormCollection form)
        {
            loader.Delete<Club>("clubs/" + id);

            return RedirectToAction("Index");
        }
    }
}
