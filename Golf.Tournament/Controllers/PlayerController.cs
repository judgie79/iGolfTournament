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
    public class PlayerController : BaseController
    {

        // GET: Player
        [Route("players")]
        public async Task<ActionResult> Index()
        {
            var players = await loader.LoadAsync<IEnumerable<Player>>("players");

            return View(players);
        }

        // GET: Player/Details/5
        [Route("players/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var player = await loader.LoadAsync<Player>("players/" + id);

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


                playerCreateViewModel.Player = await loader.PostAsync<Player>("players/", playerCreateViewModel.Player);

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

            var clubs = loader.LoadAsync<IEnumerable<Club>>("clubs");
            var player = loader.LoadAsync<Player>("players/" + id);

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
                //save the file
                String savePath = ControllerContext.HttpContext.Server.MapPath("~/avatars/");
                string extension = Path.GetExtension(playerEditViewModel.AvatarFile.FileName);

                string fileName = string.Format("{0}{1}", id, extension);
                var avatarSaveLocation = Path.Combine(savePath, fileName);
                playerEditViewModel.AvatarFile.SaveAs(avatarSaveLocation);

                playerEditViewModel.Player.Avatar = string.Format("/avatars/{0}", fileName);
            }


            try
            {
                playerEditViewModel.Player = await loader.PutAsync<Player>("players/" + id, playerEditViewModel.Player);

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
            var player = await loader.LoadAsync<Player>("players/" + id);
            return View(player);
        }

        // POST: Player/Delete/5
        [HttpPost]
        [Route("players/{id}/delete")]
        public async Task<ActionResult> Delete(string id, FormCollection form)
        {
            try
            {
                await loader.Delete<Player>("players/" + id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
