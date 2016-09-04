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
            var club = loader.LoadClub(clubId);
            var course = loader.LoadCourse(courseId);
            var holes = loader.LoadCourseHolesOfTeebox(clubId, courseId, teeboxId);
            var clubHoles = loader.LoadHolesOfClub(clubId);
            await Task.WhenAll(club, course, holes, clubHoles);

            var teebox = course.Result.TeeBoxes.Single(t => t.Id == teeboxId);
            teebox.Holes.Front = new CourseHoleCollection(holes.Result.Where(h => h.FrontOrBack == FrontOrBack.Front));
            teebox.Holes.Back = new CourseHoleCollection(holes.Result.Where(h => h.FrontOrBack == FrontOrBack.Back));

            return View(new CourseHoleListViewModel(club.Result,
                course.Result,
                teebox,
                clubHoles.Result
                ));
        }

        // GET: Club/Details/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/{frontOrBack}/holes/{id}")]
        public async Task<ActionResult> Details(string clubId, string courseId, string teeboxId, FrontOrBack frontOrBack, string id)
        {
            var club = loader.LoadClub(clubId);
            var course = loader.LoadCourse(courseId);
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
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/holes/create")]
        public async Task<ActionResult> Create(string clubId, string courseId, string teeboxId, int holeFrontCount, int holeBackCount)
        {
            var club = loader.LoadClub(clubId);
            var holes = loader.LoadHolesOfClub(clubId);
            var course = loader.LoadCourse(courseId);

            await Task.WhenAll(club, course, holes);

            var teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == teeboxId);

            var viewModel = new CourseHoleCreateViewModel()
            {
                Club = club.Result,
                Course = course.Result,
                Holes = holes.Result,
                Teebox = teebox,
                CourseHoles = new CourseHoles()
            };

            int holeCounter = 1;
            for (int i = 0; i < holeFrontCount; i++)
            {
                viewModel.CourseHoles.Front.Add(new CourseHole()
                {
                    ClubId = clubId,
                    Number = holeCounter,
                    FrontOrBack = FrontOrBack.Front
                });
                holeCounter++;
            }

            for (int i = 0; i < holeBackCount; i++)
            {
                viewModel.CourseHoles.Back.Add(new CourseHole()
                {
                    ClubId = clubId,
                    Number = holeCounter,
                    FrontOrBack = FrontOrBack.Back
                });
                holeCounter++;
            }

            return View(viewModel);
        }

        // POST: Club/Create
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/holes/create")]
        public async Task<ActionResult> Create(string clubId, string courseId, string teeboxId, CourseHoleCreateViewModel courseHoleCreateViewModel)
        {
            ModelState.Clear();
            TryValidateModel(courseHoleCreateViewModel.CourseHoles);

            if (ModelState.IsValid)
            {
                int holeCounter = 1;
                int frontHoleCount = courseHoleCreateViewModel.CourseHoles.Front.Count;

                for (int i = 0; i < frontHoleCount; i++)
                {
                    await loader.CreateHole(clubId, courseId, teeboxId, courseHoleCreateViewModel.CourseHoles.Front[i]);
                }
                int backHoleCount = courseHoleCreateViewModel.CourseHoles.Back.Count;

                for (int i = 0; i < backHoleCount; i++)
                {
                    await loader.CreateHole(clubId, courseId, teeboxId, courseHoleCreateViewModel.CourseHoles.Back[i]);
                }

                return RedirectToAction("Index");
            }
            else
            {
                var club = loader.LoadClub(clubId);
                var course = loader.LoadCourse(courseId);
                await Task.WhenAll(club, course);

                courseHoleCreateViewModel.Club = club.Result;
                courseHoleCreateViewModel.Course = course.Result;

                return View(courseHoleCreateViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/holes/edit")]
        public async Task<ActionResult> Edit(string clubId, string courseId, string teeboxId)
        {
            var club = loader.LoadClub(clubId);
            var holes = loader.LoadHolesOfClub(clubId);
            var course = loader.LoadCourse(courseId);
            var courseHolesCol = loader.LoadCourseHolesOfTeebox(clubId, courseId, teeboxId);
            await Task.WhenAll(club, course, holes, courseHolesCol);

            var courseHoles = new CourseHoles()
            {
                Front = new CourseHoleCollection(courseHolesCol.Result.Where(c => c.FrontOrBack == FrontOrBack.Front)),
                Back = new CourseHoleCollection(courseHolesCol.Result.Where(c => c.FrontOrBack == FrontOrBack.Back))
            };

            return View(new CourseHoleEditViewModel()
            {
                Club = club.Result,
                Course = course.Result,
                Teebox = course.Result.TeeBoxes.SingleOrDefault(t => t.Id == teeboxId),
                Holes = holes.Result,
                CourseHoles = courseHoles
            });
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/holes/edit")]
        public async Task<ActionResult> Edit(string clubId, string courseId, string teeboxId, CourseHoleEditViewModel courseHoleEditViewModel)
        {
            ModelState.Clear();
            TryValidateModel(courseHoleEditViewModel.CourseHoles);

            if (ModelState.IsValid)
            {
                int holeCounter = 1;
                int frontHoleCount = courseHoleEditViewModel.CourseHoles.Front.Count;

                for (int i = 0; i < frontHoleCount; i++)
                {
                    await loader.UpdateHole(clubId, courseId, teeboxId, courseHoleEditViewModel.CourseHoles.Front[i]);
                }
                int backHoleCount = courseHoleEditViewModel.CourseHoles.Back.Count;

                for (int i = 0; i < backHoleCount; i++)
                {
                    await loader.UpdateHole(clubId, courseId, teeboxId, courseHoleEditViewModel.CourseHoles.Back[i]);
                }

                return RedirectToAction("Index");
            }
            else
            {
                var club = loader.LoadClub(clubId);
                var course = loader.LoadCourse(courseId);
                await Task.WhenAll(club, course);

                courseHoleEditViewModel.Club = club.Result;
                courseHoleEditViewModel.Course = course.Result;

                return View(courseHoleEditViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/holes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string courseId, string teeboxId, string id)
        {
            var club = loader.LoadClub(clubId);
            var course = loader.LoadCourse(courseId);
            var courses = loader.LoadCoursesOfClub(clubId);

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
        [Route("clubs/{clubId}/courses/{courseId}/teeboxes/{teeboxId}/holes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string courseId, string teeboxId, string id, FormCollection form)
        {
            var club = loader.LoadClub(clubId);
            var course = loader.LoadCourse(courseId);
            await Task.WhenAll(club, course);

            try
            {
                course.Result.TeeBoxes.Remove(course.Result.TeeBoxes.First(t => t.Id == id));
                await loader.DeleteHole(clubId, courseId, teeboxId, id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete");
            }
        }
    }
}