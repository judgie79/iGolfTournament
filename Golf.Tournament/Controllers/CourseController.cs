using Golf.Tournament.Models;
using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class CourseController : BaseController
    {
        // GET: Course
        [Route("courses")]
        public ActionResult Index()
        {
            var courses = loader.Load<IEnumerable<Course>>("courses");

            return View(courses);
        }

        [Route("clubs/{id}/courses")]
        public ActionResult GetCoursesFromClub(string id)
        {
            var courses = loader.Load<IEnumerable<Course>>("clubs/" + id + "/courses");

            return View("Index", courses);
        }

        // GET: Course/Details/5
        [Route("courses/{id}")]
        public ActionResult Details(string id)
        {
            var course = loader.Load<Course>("courses/" + id);
            var club = loader.Load<Club>("clubs/" + course.ClubId);
            return View(new CourseDetailsViewModel()
            {
                Course = course,
                Club = club
            });
        }

        // GET: Course/Create
        [Route("courses/create")]
        public ActionResult Create(string clubId)
        {
            CourseCreateViewModel courseCreateViewModel = new CourseCreateViewModel();

            courseCreateViewModel.Course = new Course();
            courseCreateViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");

            if (!string.IsNullOrWhiteSpace(clubId))
                courseCreateViewModel.Course.ClubId = clubId;

            return View(courseCreateViewModel);
        }

        // POST: Course/Create
        [HttpPost]
        [Route("courses/create")]
        public ActionResult Create([ModelBinder(typeof(CourseViewModelModelBinder<CourseCreateViewModel>))] CourseCreateViewModel courseCreateViewModel)
        {
            if(ModelState.IsValid)
            {
                courseCreateViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");
                courseCreateViewModel.Course = loader.Post<Course>("Courses/", courseCreateViewModel.Course);

                return RedirectToAction("Index");
            }
            else
            {
                return View(courseCreateViewModel);
            }
        }

        // GET: Course/Edit/5
        [Route("courses/{id}/edit")]
        public ActionResult Edit(string id)
        {
            CourseEditViewModel courseEditViewModel = new CourseEditViewModel();

            courseEditViewModel.Course = loader.Load<Course>("courses/" + id);
            courseEditViewModel.Club = loader.Load<Club>("clubs/" + courseEditViewModel.Course.ClubId);

            return View(courseEditViewModel);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [Route("courses/{id}/edit")]
        public ActionResult Edit(string id, [ModelBinder(typeof(CourseViewModelModelBinder<CourseEditViewModel>))] CourseEditViewModel courseEditViewModel)
        {
            if (ModelState.IsValid)
            {
                courseEditViewModel.Course = loader.Put<Course>("courses/" + id, courseEditViewModel.Course);

                return RedirectToAction("Index");
            }
            else
            {
                courseEditViewModel.Club = loader.Load<Club>("clubs/" + courseEditViewModel.Course.ClubId);
                return View(courseEditViewModel);
            }
        }

        // GET: Course/Delete/5
        [Route("courses/{id}/delete")]
        public ActionResult Delete(string id)
        {
            Course course = loader.Load<Course>("courses/" + id);

            return View(course);
        }

        // GET: Course/Delete/5
        [HttpPost]
        [Route("courses/{id}/delete")]
        public ActionResult Delete(string id, FormCollection form)
        {

            loader.Delete<Course>("courses/" + id);

            return RedirectToAction("Index");

        }
    }
}
