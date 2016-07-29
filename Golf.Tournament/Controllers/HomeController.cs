﻿using Golf.Models.Reports;
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
    public class HomeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var clubs = loader.Load<IEnumerable<Club>>("clubs");
            var players = loader.Load<PlayerCollection>("players");
            var tournaments = loader.Load<IEnumerable<Models.Tournament>>("tournaments");
            var clubReports =  loader.Load<IEnumerable<ClubReport>>("clubs/report");

            await Task.WhenAll(clubs, players, tournaments, clubReports);

            return View(new HomeViewModel() {
                ClubReports = clubReports.Result,
                Clubs = clubs.Result,
                Players = players.Result,
                Tournaments = tournaments.Result
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