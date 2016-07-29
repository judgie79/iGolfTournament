using Golf.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        public async Task<ActionResult> Clubs()
        {
            var clubReport = await loader.Load<IEnumerable<ClubReport>>("clubs/report");

            return PartialView(clubReport);
        }
    }
}