using Golf.Tournament.Models;
using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var clubs = loader.Load<IEnumerable<Club>>("clubs");
            var players = loader.Load<PlayerCollection>("players");
            var tournaments = loader.Load<IEnumerable<Models.Tournament>>("tournaments");

            return View(new HomeViewModel() {
                    Clubs = clubs,
                    Players = players,
                    Tournaments = tournaments
            });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}