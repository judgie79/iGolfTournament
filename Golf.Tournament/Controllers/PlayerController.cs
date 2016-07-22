using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class PlayerController : BaseController
    {

        // GET: Player
        [Route("players")]
        public ActionResult Index()
        {
            var players = loader.Load<IEnumerable<Player>>("players");

            return View(players);
        }

        // GET: Player/Details/5
        [Route("players/{id}")]
        public ActionResult Details(string id)
        {
            var player = loader.Load<Player>("players/" + id);

            return View(player);
        }

        // GET: Player/Create
        [Route("players/create")]
        public ActionResult Create()
        {
            var playerCreateViewModel = new PlayerCreateViewModel();

            playerCreateViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");

            return View(playerCreateViewModel);
        }

        // POST: Player/Create
        [Route("players/create")]
        [HttpPost]
        public ActionResult Create(PlayerCreateViewModel playerCreateViewModel)
        {
            try
            {


                playerCreateViewModel.Player = loader.Post<Player>("players/", playerCreateViewModel.Player);

                return RedirectToAction("Index");
            }
            catch
            {
                playerCreateViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");
                return View(playerCreateViewModel);
            }
        }

        // GET: Player/Edit/5
        [Route("players/{id}/edit")]
        public ActionResult Edit(string id)
        {
            var playerEditViewModel = new PlayerEditViewModel();

            playerEditViewModel.Clubs = loader.Load<IEnumerable<Club>>("clubs");
            playerEditViewModel.Player = loader.Load<Player>("players/" + id);

            return View(playerEditViewModel);
        }

        // POST: Player/Edit/5
        [Route("players/{id}/edit")]
        [HttpPost]
        public ActionResult Edit(string id, PlayerEditViewModel playerEditViewModel)
        {
            try
            {
                playerEditViewModel.Player = loader.Put<Player>("players/" + id, playerEditViewModel.Player);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Player/Delete/5
        [HttpPost]
        [Route("players/{id}/delete")]
        public ActionResult Delete(string id)
        {
            try
            {
                loader.Delete<Player>("players/" + id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
