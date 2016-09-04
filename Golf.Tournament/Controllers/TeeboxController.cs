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
            var club = loader.LoadClub(clubId);
            var courseTask = loader.LoadCourse(courseId);
            await Task.WhenAll(club, courseTask);

            var course = courseTask.Result;
            course = await loader.SetHoles(course);

            return View(new TeeboxListViewModel(club.Result, course));
        }

        // GET: Club/Details/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}")]
        public async Task<ActionResult> Details(string clubId, string courseId, string id)
        {
            var club = loader.LoadClub(clubId);
            var courseTask = loader.LoadCourse(courseId);
            await Task.WhenAll(club, courseTask);

            var course = courseTask.Result;
            course = await loader.SetHoles(course);

            return View(new TeeboxDetailsViewModel()
            {
                Club = club.Result,
                Course = course,
                Teebox = course.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // GET: Club/Create
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/create")]
        public async Task<ActionResult> Create(string clubId, string courseId)
        {
            var club = loader.LoadClub(clubId);
            var courseTask = loader.LoadCourse(courseId);
            await Task.WhenAll(club, courseTask);

            var course = courseTask.Result;
            course = await loader.SetHoles(course);

            return View(new TeeboxCreateViewModel()
            {
                Club = club.Result,
                Course = course
            });
        }

        // POST: Club/Create
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/create")]
        public async Task<ActionResult> Create(string clubId, string courseId, [ModelBinder(typeof(TeeboxCreateViewModelBinder))]TeeboxCreateViewModel teeboxCreateViewModel)
        {
            ModelState.Clear();
            TryValidateModel(teeboxCreateViewModel.Teebox);

            if (ModelState.IsValid)
            {
                await loader.CreateTeebox(clubId, courseId, teeboxCreateViewModel.Teebox);

                return RedirectToAction("Index");
            }
            else
            {
                var club = loader.LoadClub(clubId);

                var courseTask = loader.LoadCourse(courseId);
                await Task.WhenAll(club, courseTask);

                var course = courseTask.Result;
                course = await loader.SetHoles(course);

                teeboxCreateViewModel.Club = club.Result;
                teeboxCreateViewModel.Course = course;

                return View(teeboxCreateViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string courseId, string id)
        {
            var club = loader.LoadClub(clubId);
            var courseTask = loader.LoadCourse(courseId);

            await Task.WhenAll(club, courseTask);

            var course = courseTask.Result;
            course = await loader.SetHoles(course);

            return View(new TeeboxEditViewModel()
            {
                Club = club.Result,
                Course = course,
                Teebox = course.TeeBoxes.SingleOrDefault(t => t.Id == id),
                
            });
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string courseId, string id, [ModelBinder(typeof(TeeboxEditViewModelBinder))]TeeboxEditViewModel teeboxEditViewModel)
        {
            ModelState.Clear();
            TryValidateModel(teeboxEditViewModel.Teebox);

            if (ModelState.IsValid)
            {
                await loader.UpdateTeebox(clubId, courseId, teeboxEditViewModel.Teebox);
                return RedirectToAction("Details", "Course", new { clubId = clubId, id = courseId });
            }
            else
            {
                var club = loader.LoadClub(clubId);
                var courseTask = loader.LoadCourse(courseId);

                await Task.WhenAll(club, courseTask);

                var course = courseTask.Result;
                course = await loader.SetHoles(course);

                teeboxEditViewModel.Course = course;
                teeboxEditViewModel.Club = club.Result;

                return View(teeboxEditViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string courseId, string id)
        {
            var club = loader.LoadClub(clubId);
            var courseTask = loader.LoadCourse(courseId);

            await Task.WhenAll(club, courseTask);

            var course = courseTask.Result;
            course = await loader.SetHoles(course);

            return View(new TeeboxDeleteViewModel()
            {
                Club = club.Result,
                Course = course,
                Teebox = course.TeeBoxes.SingleOrDefault(t => t.Id == id)
            });
        }

        // GET: Club/Delete/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string courseId, string id, FormCollection form)
        {
            try
            {
                await loader.DeleteTeebox(clubId, courseId, id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete");
            }
        }
    }
}
