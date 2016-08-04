using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    public class ExcelController : BaseController
    {
        // GET: Excel
        [Route("tournaments/{id}/excel")]
        public async Task<FileResult> Index(string id)
        {
            String savePath = ControllerContext.HttpContext.Server.MapPath("~/scorecards/");
            var spreadsheetSaveLocation = Path.Combine(savePath, string.Format("{}.xlsx", id));

            var xlBytes = await ReadAsync(spreadsheetSaveLocation);
            string fileName = "Scorecards.xlsx";


            return File(xlBytes, "application/vnd.ms-excel", fileName);
        }

        // GET: Excel
        [Route("tournaments/{id}/excel/create")]
        public async Task<FileResult> Create(string id)
        {
            var tournament = await loader.LoadAsync<Models.Tournament>("tournaments/" + id);

            String savePath = ControllerContext.HttpContext.Server.MapPath("~/scorecards/");
            var spreadsheetSaveLocation = Path.Combine(savePath, string.Format("{0}.xlsx", id));
            String path = ControllerContext.HttpContext.Server.MapPath("~/bin/");

            var spreadsheetLocation = Path.Combine(path, "Template.xlsx");

            if (tournament.TournamentType == Models.TournamentType.Team)
            {
                using (Golf.Excel.TournamentWorkbook wb = new Excel.TournamentWorkbook())
                {
                    wb.Open(spreadsheetLocation);

                    wb.SetCourse(tournament.Club, tournament.Course);
                    wb.SetTeeboxes(tournament.Course.TeeBoxes);

                    foreach (var teebox in tournament.Course.TeeBoxes)
                    {
                        wb.SetParticipants(new Models.TournamentParticipantCollection(tournament.Participants.Where(t => t.TeeboxId == teebox.Id)), teebox);
                    }

                    foreach (var participant in tournament.Participants)
                    {
                        wb.SetScoresheet(participant, tournament.Course.TeeBoxes.FirstOrDefault(p => p.Id == participant.TeeboxId));
                    }

                    wb.SaveAs(spreadsheetSaveLocation);
                }
            } else if (tournament.TournamentType == Models.TournamentType.Single)
            {
                using (Golf.Excel.TeamTournamentWorkbook wb = new Excel.TeamTournamentWorkbook())
                {

                    var teamTournament = (Models.TeamTournament)tournament;
                    wb.Open(spreadsheetLocation);

                    wb.SetCourse(tournament.Club, tournament.Course);
                    wb.SetTeeboxes(tournament.Course.TeeBoxes);

                    wb.SetTeams(teamTournament);
                    foreach (var team in teamTournament.Teams)
                    {
                        wb.SetScoresheet(team, tournament.Course.TeeBoxes.FirstOrDefault(t => t.Id == team.TeeboxId));
                    }

                    wb.SaveAs(spreadsheetSaveLocation);
                }
            }

            

            

            var xlBytes = await ReadAsync(spreadsheetSaveLocation);
            string fileName = "Scorecards.xlsx";
            
            return File(xlBytes, "application/vnd.ms-excel", fileName);
        }

        private async Task<byte[]> ReadAsync(string filePath)
        {
            using (var file = System.IO.File.OpenRead(filePath))
            {
                using (var ms = new MemoryStream())
                {
                    byte[] buff = new byte[file.Length];
                    await file.ReadAsync(buff, 0, (int)file.Length);
                    return buff;
                }
            }
        }
    }
}