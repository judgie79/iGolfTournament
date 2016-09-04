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
    public class CourseController : BaseController
    {
        // GET: Course

        [Route("clubs/{clubId}/courses")]
        public async Task<ActionResult> Index(string clubId)
        {
            var club = await loader.LoadClub(clubId);
            var courses = await loader.LoadCoursesOfClub(clubId);

            courses = new CourseCollection(await loader.SetHoles(courses));
            return View("Index", new CourseListViewModel()
            {
                Club = club,
                Courses = courses
            });
        }

        // GET: Course/Details/5
        [Route("clubs/{clubId}/courses/{id}")]
        public async Task<ActionResult> Details(string clubId, string id)
        {
            var course = await loader.LoadCourse(id);
            var club = await loader.LoadClub(course.ClubId);

            course = await loader.SetHoles(course);

            foreach (var teebox in course.TeeBoxes)
            {
                teebox.ClubId = clubId;
                teebox.CourseId = course.Id;
            }

            return View(new CourseDetailsViewModel()
            {
                Course = course,
                Club = club
            });
        }

        // GET: Course/Create
        [Route("clubs/{clubId}/courses/create")]
        public async Task<ActionResult> Create(string clubId)
        {
            var courseCreateViewModel = new CourseCreateViewModel();

            courseCreateViewModel.Course = new Course();

            var clubs = await loader.LoadClubs();
            courseCreateViewModel.Clubs = clubs;

            if (!string.IsNullOrWhiteSpace(clubId)) { 
                courseCreateViewModel.Course.ClubId = clubId;
                courseCreateViewModel.Club = clubs.SingleOrDefault(c => c.Id == clubId);
            }

            return View(courseCreateViewModel);
        }

        // POST: Course/Create
        [HttpPost]
        [Route("clubs/{clubId}/courses/create")]
        public async Task<ActionResult> Create(string clubId, [ModelBinder(typeof(CourseViewModelModelBinder<CourseCreateViewModel>))] CourseCreateViewModel courseCreateViewModel)
        { 
            if (ModelState.IsValid)
            {
                var course = await loader.UpdateCourse(courseCreateViewModel.Course);

                return RedirectToAction("Index");
            }
            else
            {
                var clubs = loader.LoadClubs();
                courseCreateViewModel.Clubs = clubs.Result;
                return View(courseCreateViewModel);
            }
        }

        // GET: Course/Edit/5
        [Route("clubs/{clubId}/courses/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string id)
        {
            var courseEditViewModel = new CourseEditViewModel();

            courseEditViewModel.Course = await loader.LoadCourse(id);
            await loader.SetHoles(courseEditViewModel.Course);
            courseEditViewModel.Club = await loader.LoadClub(courseEditViewModel.Course.ClubId);

            foreach (var teebox in courseEditViewModel.Course.TeeBoxes)
            {
                teebox.ClubId = clubId;
                teebox.CourseId = id;
            }
            return View(courseEditViewModel);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string id, [ModelBinder(typeof(CourseViewModelModelBinder<CourseEditViewModel>))] CourseEditViewModel courseEditViewModel)
        {
            if (ModelState.IsValid)
            {          
                courseEditViewModel.Course = await loader.UpdateCourse(courseEditViewModel.Course);

                return RedirectToAction("Index");
            }
            else
            {
                var tCourse = await loader.LoadCourse(id);
                courseEditViewModel.Course.TeeBoxes = tCourse.TeeBoxes;

                courseEditViewModel.Club = await loader.LoadClub(courseEditViewModel.Course.ClubId);
                return View(courseEditViewModel);
            }
        }

        // GET: Course/Delete/5
        [Route("clubs/{clubId}/courses/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string id)
        {
            Course course = await loader.LoadCourse(id);

            return View(course);
        }

        // GET: Course/Delete/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string id, FormCollection form)
        {
            await loader.DeleteCourse(id);
            return RedirectToAction("Index");

        }
    }
}
