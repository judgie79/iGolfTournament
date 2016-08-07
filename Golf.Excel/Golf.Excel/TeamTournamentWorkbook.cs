using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Golf.Excel
{
    public class TeamTournamentWorkbook : TournamentWorkbookBase
    {
        public void SetTeams(TeamTournament tournament)
        {
            var teeboxSheet = this.xlWorkBook.Worksheets["Teeboxes"];

            var teeboxes = tournament.Course.TeeBoxes;

            MSExcel.Worksheet teamsSheet = this.xlWorkBook.Worksheets.Add(Before: teeboxSheet);
            teamsSheet.Name = "Teams";
            object[] header = { "Id", "Teetime", "Team", "Hcp", "Spvg", "Teebox", "Par", "CourseRating", "SlopeRating", "Strokes", "Brutto", "Netto" };
            teamsSheet.Range["A1:L1"].Value = header;

            int rowIndex = 2;
            foreach (var team in tournament.Teams)
            {
                var teebox = teeboxes.FirstOrDefault(t => t.Id == team.TeeboxId);

                teamsSheet.Range["A" + rowIndex].Value = team.Id;
                teamsSheet.Range["B" + rowIndex].Value = team.Teetime;
                teamsSheet.Range["C" + rowIndex].Value = team.Name;
                teamsSheet.Range["D" + rowIndex].FormulaLocal = string.Format("=RUNDEN({0};1)", team.Hcp.ToString().Replace(".", ","));
                teamsSheet.Range["E" + rowIndex].FormulaLocal = string.Format("=RUNDEN({0}*({1}/113)-{2}+{3};1)", ("D" + rowIndex), ("I" + rowIndex), ("H" + rowIndex), ("G" + rowIndex));
                teamsSheet.Range["F" + rowIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teebox.Color.Value));
                teamsSheet.Range["G" + rowIndex].Value = teebox.Par;
                teamsSheet.Range["H" + rowIndex].Value = teebox.CourseRating;
                teamsSheet.Range["I" + rowIndex].Value = teebox.SlopeRating;
                teamsSheet.Range["J" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"!{1}7\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 6));
                teamsSheet.Range["K" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"!{1}12\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 6));
                teamsSheet.Range["L" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"!{1}13\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 6));
                rowIndex++;
            }

            MSExcel.Range usedrange = teamsSheet.UsedRange;
            usedrange.Columns.AutoFit();
        }

        public void SetScoresheet(Team team, TeeBox teebox)
        {
            var scoreSheet = SetScoreSheet(team.Name, team.Name, team.Hcp, team.Teetime, teebox);


            MSExcel.Range teamMembersCell = scoreSheet.Cells[TeamMembersRow, TeamMembersCol];
            teamMembersCell.Value = string.Join("; ", team.Members.Select(m => string.Format("{0}, {1} ({2})", m.Player.Lastname, m.Player.Firstname[0], m.Player.Hcp)));
            teamMembersCell.Font.Size = 8;
            MSExcel.Style teamMembersCellStyle = teamMembersCell.Style;
            teamMembersCellStyle.WrapText = false;
        }

        protected int TeamMembersRow = 15;
        protected int TeamMembersCol = 4;
    }
}
