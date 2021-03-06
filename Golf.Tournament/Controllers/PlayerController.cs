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
    public class PlayerController : BaseController
    {

        // GET: Player
        [Route("players")]
        public async Task<ActionResult> Index()
        {
            var players = await loader.LoadPlayers();

            return View(players);
        }

        // GET: Player/Details/5
        [Route("players/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var player = await loader.LoadPlayer(id);

            return View(player);
        }

        // GET: Player/Create
        [Route("players/create")]
        public async Task<ActionResult> Create()
        {
            var playerCreateViewModel = new PlayerCreateViewModel();

            playerCreateViewModel.Clubs = await loader.LoadAsync<IEnumerable<HomeClub>>("clubs");

            return View(playerCreateViewModel);
        }

        // POST: Player/Create
        [Route("players/create")]
        [HttpPost]
        public async Task<ActionResult> Create(PlayerCreateViewModel playerCreateViewModel)
        {
            if (ModelState.IsValid)
            {


                string playerId = await loader.CreatePlayer(playerCreateViewModel.Player);

                return RedirectToAction("Index");
            }

            playerCreateViewModel.Clubs = await loader.LoadAsync<IEnumerable<HomeClub>>("clubs");
            return View(playerCreateViewModel);
        }

        // GET: Player/Edit/5
        [Route("players/{id}/edit")]
        public async Task<ActionResult> Edit(string id)
        {
            var playerEditViewModel = new PlayerEditViewModel();

            var clubs = loader.LoadClubs();
            var player = loader.LoadPlayer(id);

            await Task.WhenAll(clubs, player);

            playerEditViewModel.Clubs = clubs.Result;
            playerEditViewModel.Player = player.Result;

            return View(playerEditViewModel);
        }

        // POST: Player/Edit/5
        [Route("players/{id}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(string id, PlayerEditViewModel playerEditViewModel)
        {
            if (playerEditViewModel.AvatarFile.HasFile())
            {
                string avatar = HtmlFileUploadHelper.StoreFile(ControllerContext.HttpContext, "~/avatars/", "/avatars", id, playerEditViewModel.AvatarFile);

                playerEditViewModel.Player.Avatar = avatar;
            }

            try
            {
                playerEditViewModel.Player = await loader.UpdatePlayer(playerEditViewModel.Player);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Participant/Delete/5
        [Route("players/{id}/delete")]
        public async Task<ActionResult> Delete(string id)
        {
            var player = await loader.LoadPlayer(id);
            return View(player);
        }

        // POST: Player/Delete/5
        [HttpPost]
        [Route("players/{id}/delete")]
        public async Task<ActionResult> Delete(string id, FormCollection form)
        {
            try
            {
                await loader.DeletePlayer(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
