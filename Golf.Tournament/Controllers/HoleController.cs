using Golf.Tournament.Models;
using Golf.Tournament.Utility;
using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class HoleController : BaseController
    {
        // GET: Club
        [Route("clubs/{clubId}/holes")]
        public async Task<ActionResult> Index(string clubId)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);

            var holes = loader.LoadAsync<HoleCollection>("clubs/" + clubId + "/holes");
            await Task.WhenAll(club, holes);

            return View(new HoleListViewModel(club.Result, holes.Result));
        }

        // GET: Club/Details/5
        [Route("clubs/{clubId}/holes/{id}")]
        public async Task<ActionResult> Details(string clubId, string id)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var hole = loader.LoadAsync<Hole>("clubs/" + clubId + "/holes/" + id);
            await Task.WhenAll(club, hole);

            return View(new HoleDetailsViewModel()
            {
                Club = club.Result,
                Hole = hole.Result
            });
        }

        // GET: Club/Create
        [Route("clubs/{clubId}/holes/create")]
        public async Task<ActionResult> Create(string clubId)
        {
            var clubs = loader.LoadAsync<IEnumerable<Club>>("clubs");
            await Task.WhenAll(clubs);
            return View(new HoleCreateViewModel()
            {
                Clubs = clubs.Result
            });
        }

        // POST: Club/Create
        [HttpPost]
        [Route("clubs/{clubId}/holes/create")]
        public async Task<ActionResult> Create(string clubId, HoleCreateViewModel holeCreateViewModel)
        {
            ModelState.Clear();
            TryValidateModel(holeCreateViewModel.Hole);

            if (ModelState.IsValid)
            {
                await loader.PostAsync<Hole, Hole>("clubs/" + clubId + "/holes", holeCreateViewModel.Hole);
                return RedirectToAction("Index");
            }
            else
            {
                var clubs = loader.LoadAsync<IEnumerable<Club>>("clubs");
                await Task.WhenAll(clubs);

                holeCreateViewModel.Clubs = clubs.Result;

                return View(holeCreateViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/holes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string id)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var hole = loader.LoadAsync<Hole>("clubs/" + clubId + "/holes/" + id);

            await Task.WhenAll(club, hole);

            return View(new HoleEditViewModel()
            {
                Club = club.Result,
                Hole = hole.Result
            });
        }

        // POST: Club/Edit/5
        [HttpPost]
        [Route("clubs/{clubId}/holes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string id, HoleEditViewModel holeEditViewModel)
        {
            if (holeEditViewModel.CourseImageFile.HasFile())
            {
                string courseImage = HtmlFileUploadHelper.StoreFile("~/holes/" + clubId, "/holes", id, holeEditViewModel.CourseImageFile);

                holeEditViewModel.Hole.CourseImage = courseImage;
            }

            ModelState.Clear();
            TryValidateModel(holeEditViewModel.Hole);

            if (ModelState.IsValid)
            {
                await loader.PutAsync<Hole>("clubs/" + clubId + "/holes/" + id, holeEditViewModel.Hole);
                return RedirectToAction("Index");
            }
            else
            {
                var club = loader.LoadAsync<Club>("clubs/" + clubId);
                await Task.WhenAll(club);
                
                holeEditViewModel.Club = club.Result;

                return View(holeEditViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/holes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string id)
        {
            var club = loader.LoadAsync<Club>("clubs/" + clubId);
            var hole = loader.LoadAsync<Hole>("clubs/" + clubId + "/holes/" + id);

            await Task.WhenAll(club, hole);

            return View(new HoleDeleteViewModel()
            {
                Club = club.Result,
                Hole = hole.Result
            });
        }

        // GET: Club/Delete/5
        [HttpPost]
        [Route("clubs/{clubId}/holes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string id, FormCollection form)
        {
            try
            {
                await loader.Delete<Hole>("clubs/" + clubId + "/holes/" + id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete");
            }
        }
    }
}