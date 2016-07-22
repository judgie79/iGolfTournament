using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Golf.Tournament.Controllers;
using System.IO;

namespace Golf.Excel.Tests
{
    [TestClass]
    [DeploymentItem("Template.xlsx")]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            GolfLoader loader = new GolfLoader("http://localhost:8080/api/");
            Golf.Tournament.Models.Tournament tournament = loader.Load<Golf.Tournament.Models.Tournament>("tournaments/" + "57862e8001728c0018267538");

            TournamentWorkbook wb = new TournamentWorkbook();

            var spreadsheetLocation = Path.Combine(Directory.GetCurrentDirectory(), "Template.xlsx");

            wb.Open(spreadsheetLocation);

            wb.SetCourse(tournament.Course);
            wb.SetTeeboxes(tournament.Course.TeeBoxes);
            wb.SetScoresheet(new Tournament.Models.Player()
            {
                Firstname = "Michael",
                Lastname = "Richter",
                Hcp = 32.2f,
                
            },
            tournament.Course.TeeBoxes[0]);

            var spreadsheetSaveLocation = Path.Combine(Directory.GetCurrentDirectory(), "Template2.xlsx");
            wb.SaveAs(spreadsheetSaveLocation);
            wb.CloseExcel(false);
        }
    }
}
