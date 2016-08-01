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
        [Route("courses")]
        public async Task<ActionResult> Index()
        {
            var courses = await loader.LoadAsync<IEnumerable<Course>>("courses");

            return View(courses);
        }

        [Route("clubs/{id}/courses")]
        public async Task<ActionResult> GetCoursesFromClub(string id)
        {
            var courses = await loader.LoadAsync<IEnumerable<Course>>("clubs/" + id + "/courses");

            return View("Index", courses);
        }

        // GET: Course/Details/5
        [Route("courses/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var course = await loader.LoadAsync<Course>("courses/" + id);
            var club = await loader.LoadAsync<Club>("clubs/" + course.ClubId);

            return View(new CourseDetailsViewModel()
            {
                Course = course,
                Club = club
            });
        }

        // GET: Course/Create
        [Route("courses/create")]
        public async Task<ActionResult> Create(string clubId)
        {
            CourseCreateViewModel courseCreateViewModel = new CourseCreateViewModel();

            courseCreateViewModel.Course = new Course();

            var clubs = await loader.LoadAsync<IEnumerable<Club>>("clubs");
            courseCreateViewModel.Clubs = clubs;

            if (!string.IsNullOrWhiteSpace(clubId))
                courseCreateViewModel.Course.ClubId = clubId;

            return View(courseCreateViewModel);
        }

        // POST: Course/Create
        [HttpPost]
        [Route("courses/create")]
        public async Task<ActionResult> Create([ModelBinder(typeof(CourseViewModelModelBinder<CourseCreateViewModel>))] CourseCreateViewModel courseCreateViewModel)
        {
            var clubs = loader.LoadAsync<IEnumerable<Club>>("clubs/");
            var course = loader.PostAsync<Course>("Courses/", courseCreateViewModel.Course);
            await Task.WhenAll(clubs, course);
            if (ModelState.IsValid)
            {
                courseCreateViewModel.Clubs = clubs.Result;
                courseCreateViewModel.Course = course.Result;

                return RedirectToAction("Index");
            }
            else
            {
                return View(courseCreateViewModel);
            }
        }

        // GET: Course/Edit/5
        [Route("courses/{id}/edit")]
        public async Task<ActionResult> Edit(string id)
        {
            CourseEditViewModel courseEditViewModel = new CourseEditViewModel();

            courseEditViewModel.Course = await loader.LoadAsync<Course>("courses/" + id);
            courseEditViewModel.Club = await loader.LoadAsync<Club>("clubs/" + courseEditViewModel.Course.ClubId);

            return View(courseEditViewModel);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [Route("courses/{id}/edit")]
        public async Task<ActionResult> Edit(string id, [ModelBinder(typeof(CourseViewModelModelBinder<CourseEditViewModel>))] CourseEditViewModel courseEditViewModel)
        {
            var tCourse = await loader.LoadAsync<Course>("courses/" + id);
            courseEditViewModel.Course.TeeBoxes = tCourse.TeeBoxes;

            if (ModelState.IsValid)
            {
                
                courseEditViewModel.Course = await loader.PutAsync<Course>("courses/" + id, courseEditViewModel.Course);

                return RedirectToAction("Index");
            }
            else
            {
                courseEditViewModel.Club = await loader.LoadAsync<Club>("clubs/" + courseEditViewModel.Course.ClubId);
                return View(courseEditViewModel);
            }
        }

        // GET: Course/Delete/5
        [Route("courses/{id}/delete")]
        public async Task<ActionResult> Delete(string id)
        {
            Course course = await loader.LoadAsync<Course>("courses/" + id);

            return View(course);
        }

        // GET: Course/Delete/5
        [HttpPost]
        [Route("courses/{id}/delete")]
        public async Task<ActionResult> Delete(string id, FormCollection form)
        {

            await loader.Delete<Course>("courses/" + id);
            return RedirectToAction("Index");

        }
    }
}
