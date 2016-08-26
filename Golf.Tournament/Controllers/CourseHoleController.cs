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
    public class CourseHoleController : BaseController
    {
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/holes")]
        public async Task<ActionResult> Index(string clubId, string courseId, string teeboxId)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var course = loader.LoadAsync<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            return View(new CourseHoleListViewModel(club.Result,
                course.Result,
                course.Result.TeeBoxes.Single(t => t.Id == teeboxId)
                ));
        }

        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes")]
        public async Task<ActionResult> Select(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var course = loader.LoadAsync<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            return View(new CourseHoleListViewModel(club.Result,
                course.Result,
                course.Result.TeeBoxes.Single(t => t.Id == teeboxId)
                ));
        }

        // GET: Club/Details/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes/{id}")]
        public async Task<ActionResult> Details(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack, string id)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var course = loader.LoadAsync<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            var teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == teeboxId);
            var holes = frontOrBack == FrontOrBack.Front ? teebox.Holes.Front : teebox.Holes.Back;
            var hole = holes.Single(h => h.Id == id);
            return View(new CourseHoleDetailsViewModel()
            {
                Club = club.Result,
                Course = course.Result,
                Teebox = teebox,
                Hole = hole
            });
        }

        // GET: Club/Create
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes/create")]
        public async Task<ActionResult> Create(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var course = loader.LoadAsync<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            var teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == teeboxId);
            CourseHoleCollection holes = (frontOrBack == FrontOrBack.Front) ? teebox.Holes.Front : teebox.Holes.Back;

            return View(new CourseHoleCreateViewModel()
            {
                Club = club.Result,
                Course = course.Result
            });
        }

        // POST: Club/Create
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes/create")]
        public async Task<ActionResult> Create(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack, CourseHoleCreateViewModel courseHoleCreateViewModel)
        {
            ModelState.Clear();
            TryValidateModel(courseHoleCreateViewModel.Teebox);

            if (ModelState.IsValid)
            {

                await loader.PostAsync<TeeBox, Course>("courses/" + courseId + "/teeboxes", courseHoleCreateViewModel.Teebox);
                return RedirectToAction("Index");
            }
            else
            {
                var club = loader.LoadAsync<Club>("clubs/" + clubId);
                var course = loader.LoadAsync<Course>("courses/" + courseId);
                await Task.WhenAll(club, course);

                courseHoleCreateViewModel.Club = club.Result;
                courseHoleCreateViewModel.Course = course.Result;

                return View(courseHoleCreateViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack, string id)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var course = loader.LoadAsync<Course>("courses/" + courseId);
            var courses = loader.LoadAsync<IEnumerable<Course>>("clubs/" + clubId + "/courses");

            await Task.WhenAll(club, course, courses);

            return View(new CourseHoleEditViewModel()
            {
                Club = club.Result,
                Course = course.Result,
                Teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack, string id, CourseHoleEditViewModel courseHoleEditViewModel)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var course = loader.LoadAsync<Course>("courses/" + courseId);

            await Task.WhenAll(club, course);

            ModelState.Clear();
            TryValidateModel(courseHoleEditViewModel.Teebox);

            if (ModelState.IsValid)
            {
                courseHoleEditViewModel.Course = course.Result;
                var teebox = courseHoleEditViewModel.Course.TeeBoxes.SingleOrDefault(t => t.Id == id);


                courseHoleEditViewModel.Course.TeeBoxes[courseHoleEditViewModel.Course.TeeBoxes.IndexOf(teebox)] = courseHoleEditViewModel.Teebox;

                await loader.PutAsync<Course>("courses/" + courseId, courseHoleEditViewModel.Course);
                return RedirectToAction("Index");
            }
            else
            {
                courseHoleEditViewModel.Course = course.Result;
                courseHoleEditViewModel.Club = club.Result;


                return View(courseHoleEditViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack, string id)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var course = loader.LoadAsync<Course>("courses/" + courseId);
            var courses = loader.LoadAsync<IEnumerable<Course>>("clubs/" + clubId + "/courses");

            await Task.WhenAll(club, course, courses);

            return View(new CourseHoleDeleteViewModel()
            {
                Club = club.Result,
                Course = course.Result,
                Teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // GET: Club/Delete/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack, string id, FormCollection form)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var course = loader.LoadAsync<Course>("courses/" + courseId);
            await Task.WhenAll(club, course);

            try
            {
                course.Result.TeeBoxes.Remove(course.Result.TeeBoxes.First(t => t.Id == id));
                await loader.PutAsync<Course>("courses/" + courseId, course.Result);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete");
            }
        }
    }
}