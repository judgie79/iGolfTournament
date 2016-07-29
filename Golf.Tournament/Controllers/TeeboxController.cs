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
    public class TeeboxController : BaseController
    {
        // GET: Club
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes")]
        public async Task<ActionResult> Index(string clubId, string courseId)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            return View(new TeeboxListViewModel(club.Result, course.Result));
        }

        // GET: Club/Details/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}")]
        public async Task<ActionResult> Details(string clubId, string courseId, string id)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            return View(new TeeboxDetailsViewModel()
            {
                Club = club.Result,
                Course = course.Result,
                Teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // GET: Club/Create
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/create")]
        public async Task<ActionResult> Create(string clubId, string courseId)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);
            return View(new TeeboxCreateViewModel()
            {
                Club = club.Result,
                Course = course.Result
            });
        }

        // POST: Club/Create
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/create")]
        public async Task<ActionResult> Create(string clubId, string courseId, [ModelBinder(typeof(TeeboxCreateViewModelBinder))]TeeboxCreateViewModel teeboxCreateViewModel)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            ModelState.Clear();
            TryValidateModel(teeboxCreateViewModel.Teebox);

            if (ModelState.IsValid)
            {

                teeboxCreateViewModel.Course = course.Result;
                teeboxCreateViewModel.Course.TeeBoxes.Add(teeboxCreateViewModel.Teebox);
                await loader.Put<Course>("courses/" + courseId, teeboxCreateViewModel.Course);
                return RedirectToAction("Index");
            }
            else
            {
                teeboxCreateViewModel.Club = club.Result;
                teeboxCreateViewModel.Course = course.Result;

                return View(teeboxCreateViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string courseId, string id)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);
            var courses = loader.Load<IEnumerable<Course>>("clubs/" + clubId + "/courses");

            await Task.WhenAll(club, course, courses);

            return View(new TeeboxEditViewModel()
            {
                Club = club.Result,
                Course = course.Result,
                Teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string courseId, string id, [ModelBinder(typeof(TeeboxEditViewModelBinder))]TeeboxEditViewModel teeboxEditViewModel)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);

            await Task.WhenAll(club, course);

            ModelState.Clear();
            TryValidateModel(teeboxEditViewModel.Teebox);

            if (ModelState.IsValid)
            {
                teeboxEditViewModel.Course = course.Result;
                var teebox = teeboxEditViewModel.Course.TeeBoxes.SingleOrDefault(t => t.Id == id);


                teeboxEditViewModel.Course.TeeBoxes[teeboxEditViewModel.Course.TeeBoxes.IndexOf(teebox)] = teeboxEditViewModel.Teebox;

                await loader.Put<Course>("courses/" + courseId, teeboxEditViewModel.Course);
                return RedirectToAction("Index");
            }
            else
            {
                teeboxEditViewModel.Course = course.Result;
                teeboxEditViewModel.Club = club.Result;


                return View(teeboxEditViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string courseId, string id)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);
            var courses = loader.Load<IEnumerable<Course>>("clubs/" + clubId + "/courses");

            await Task.WhenAll(club, course, courses);

            return View(new TeeboxDeleteViewModel()
            {
                Club = club.Result,
                Course = course.Result,
                Teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // GET: Club/Delete/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string courseId, string id, FormCollection form)
        {
            var club = loader.Load<Club>("clubs/" + clubId);
            var course = loader.Load<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            try
            {
                course.Result.TeeBoxes.Remove(course.Result.TeeBoxes.First(t => t.Id == id));
                await loader.Put<Course>("courses/" + courseId, course.Result);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete");
            }
        }
    }
}
