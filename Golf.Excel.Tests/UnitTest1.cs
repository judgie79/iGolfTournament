using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Golf.Tournament.Controllers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Golf.Excel.Tests
{
    [TestClass]
    [DeploymentItem("Template.xlsx")]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {

            GolfLoader loader = new GolfLoader("http://localhost:8080/api/");
            IEnumerable<Golf.Tournament.Models.Tournament> tournaments = await loader.Load<IEnumerable<Golf.Tournament.Models.Tournament>>("tournaments/");
            var tournament = tournaments.First(t => t.HasStarted);

            using (TournamentWorkbook wb = new TournamentWorkbook())
            {

                var spreadsheetLocation = Path.Combine(Directory.GetCurrentDirectory(), "Template.xlsx");

                wb.Open(spreadsheetLocation);

                wb.SetCourse(tournament.Club, tournament.Course);
                wb.SetTeeboxes(tournament.Course.TeeBoxes);
                wb.SetScoresheet(
                    new Tournament.Models.TournamentParticipant()
                    {
                        Player = new Tournament.Models.Player()
                        {
                            Firstname = "Michael",
                            Lastname = "Richter",
                            Hcp = 32.2f,

                        }
                    },
                tournament.Course.TeeBoxes[0]);

                var spreadsheetSaveLocation = Path.Combine(Directory.GetCurrentDirectory(), "Template2.xlsx");
                wb.SaveAs(spreadsheetSaveLocation);
                wb.CloseExcel(false);
            }
        }
    }
}
