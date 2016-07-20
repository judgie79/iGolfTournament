using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class CourseController : Controller
    {
        GolfLoader loader;

        public CourseController()
        {
            loader = new GolfLoader("http://localhost:8080/api/");
        }
        
        
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
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [Route("courses/create")]
        public ActionResult Create(Course course)
        {
            try
            {
                course = loader.Post<Course>("Courses/", course);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(course);
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
        public ActionResult Edit(string id, [ModelBinder(typeof(Models.CourseEditViewModelModelBinder))] CourseEditViewModel courseEditViewModel)
        {
            try
            {
                courseEditViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");
                courseEditViewModel.Course = loader.Put<Course>("courses/" + id, courseEditViewModel.Course);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(courseEditViewModel);
            }
        }

        // GET: Course/Delete/5
        [HttpPost]
        [Route("courses/{id}/delete")]
        public ActionResult Delete(string id)
        {
            try
            {
                loader.Delete<Course>("courses/" + id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
