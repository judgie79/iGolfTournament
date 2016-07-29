using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class TeamController : BaseController
    {
        // GET: Team
        [Route("tournaments/{tournamentId}/teams")]
        public ActionResult Index(string tournamentId)
        {
            var teams = loader.Load<IEnumerable<Team>>("teamtournaments/" + tournamentId);
            return View();
        }
    }
}