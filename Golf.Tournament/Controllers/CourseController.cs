using Golf.Tournament.Models;
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

            return View(course);
        }

        // GET: Course/Create
        [Route("courses/create")]
        public ActionResult Create()
        {
            CourseCreateViewModel courseCreateViewModel = new CourseCreateViewModel();

            courseCreateViewModel.Course = new Course();
            courseCreateViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");

            return View(courseCreateViewModel);
        }

        // POST: Course/Create
        [HttpPost]
        [Route("courses/create")]
        public ActionResult Create([ModelBinder(typeof(Models.CourseViewModelModelBinder<CourseCreateViewModel>))] CourseCreateViewModel courseCreateViewModel)
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
            courseEditViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");

            return View(courseEditViewModel);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [Route("courses/{id}/edit")]
        public ActionResult Edit(string id, [ModelBinder(typeof(Models.CourseViewModelModelBinder<CourseCreateViewModel>))] CourseEditViewModel courseEditViewModel)
        {
            if (ModelState.IsValid)
            {
                courseEditViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");
                courseEditViewModel.Course = loader.Put<Course>("courses/" + id, courseEditViewModel.Course);

                return RedirectToAction("Index");
            }
            else
            {
                return View(courseEditViewModel);
            }
        }

        // GET: Course/Delete/5
        [HttpPost]
        [Route("courses/{id}/delete")]
        public ActionResult Delete(string id)
        {

            loader.Delete<Course>("courses/" + id);

            return RedirectToAction("Index");

        }
    }
}
