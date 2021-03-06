﻿using Golf.Tournament.Models;
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
            var club = loader.LoadClub(clubId);

            var holes = loader.LoadHolesOfClub(clubId);
            await Task.WhenAll(club, holes);

            return View(new HoleListViewModel(club.Result, holes.Result));
        }

        // GET: Club/Create
        [Route("clubs/{clubId}/holes/create")]
        public async Task<ActionResult> Create(string clubId)
        {
            var club = await loader.LoadClub(clubId);

            return View(new HoleCreateViewModel()
            {
                Club = club,
                Hole = new Hole()
                {
                    ClubId = clubId
                }
            });
        }

        // POST: Club/Create
        [HttpPost]
        [Route("clubs/{clubId}/holes/create")]
        public async Task<ActionResult> Create(string clubId, HoleCreateViewModel holeCreateViewModel)
        {
            ModelState.Clear();
            holeCreateViewModel.Hole.ClubId = clubId;
            TryValidateModel(holeCreateViewModel.Hole);

            if (ModelState.IsValid)
            {
                var holeId = await loader.CreateHole(clubId, holeCreateViewModel.Hole);

                if (holeCreateViewModel.Hole.CourseImageFile.HasFile())
                {
                    string courseImage = HtmlFileUploadHelper.StoreFile(ControllerContext.HttpContext, "~/avatars/holes/" + clubId, "/avatars/holes/" + clubId + "/", holeId, holeCreateViewModel.Hole.CourseImageFile);

                    holeCreateViewModel.Hole.Id = holeId;
                    holeCreateViewModel.Hole.CourseImage = courseImage;
                    await loader.UpdateHole(clubId, holeCreateViewModel.Hole);
                    
                }

                return RedirectToAction("Index");
            }
            else
            {
                var club = await loader.LoadClub(clubId);
                holeCreateViewModel.Club = club;
                return View(holeCreateViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/holes/{id}/edit")]
        public async Task<ActionResult> Edit(string clubId, string id)
        {
            var club = loader.LoadClub(clubId);
            var hole = loader.LoadHole(clubId, id);

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
            if (holeEditViewModel.Hole.CourseImageFile.HasFile())
            {
                string courseImage = HtmlFileUploadHelper.StoreFile(ControllerContext.HttpContext, "~/avatars/holes/" + clubId, "/avatars/holes/" + clubId + "/", id, holeEditViewModel.Hole.CourseImageFile);

                holeEditViewModel.Hole.CourseImage = courseImage;
            }

            ModelState.Clear();
            TryValidateModel(holeEditViewModel.Hole);

            if (ModelState.IsValid)
            {
                await loader.UpdateHole(clubId, holeEditViewModel.Hole);
                return RedirectToAction("Index");
            }
            else
            {
                var club = loader.LoadClub(clubId);
                await Task.WhenAll(club);
                
                holeEditViewModel.Club = club.Result;

                return View(holeEditViewModel);
            }
        }

        // GET: Club/Edit/5
        [Route("clubs/{clubId}/holes/{id}/delete")]
        public async Task<ActionResult> Delete(string clubId, string id)
        {
            var club = loader.LoadClub(clubId);
            var hole = loader.LoadHole(clubId, id);

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
                await loader.DeleteHole(clubId, id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete");
            }
        }
    }
}