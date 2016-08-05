using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MSExcel = Microsoft.Office.Interop.Excel;

namespace Golf.Excel
{
    public class TournamentWorkbook : TournamentWorkbookBase
    {
        public void SetParticipants(TournamentParticipantCollection participants, TeeBox teebox)
        {
            var teeboxSheet = this.xlWorkBook.Worksheets["Teeboxes"];
            MSExcel.Worksheet participantsSheet = this.xlWorkBook.Worksheets.Add(Before: teeboxSheet);
            participantsSheet.Name = teebox.Name + "_" + "Particpants";
            //participantsSheet.Range["A2:K2"].End[MSExcel.XlDirection.xlDown].Clear();

            participantsSheet.Range["A1"].Value = "Id";
            participantsSheet.Range["B1"].Value = "Teetime";
            participantsSheet.Range["C1"].Value = "Lastname";
            participantsSheet.Range["D1"].Value = "Firstname";
            participantsSheet.Range["E1"].Value = "Hcp";
            participantsSheet.Range["F1"].Value = "Spvg";
            participantsSheet.Range["G1"].Value = "Club";
            participantsSheet.Range["H1"].Value = "Teebox";
            participantsSheet.Range["I1"].Value = "Par";
            participantsSheet.Range["J1"].Value = "CourseRating";
            participantsSheet.Range["K1"].Value = "SlopeRating";
            participantsSheet.Range["L1"].Value = "Strokes";
            participantsSheet.Range["M1"].Value = "Brutto";
            participantsSheet.Range["N1"].Value = "Netto";

            int rowIndex = 2;
            foreach (var participant in participants)
            {
                participantsSheet.Range["A" + rowIndex].Value = participant.Id;
                participantsSheet.Range["B" + rowIndex].Value = participant.Teetime;
                participantsSheet.Range["C" + rowIndex].Value = participant.Player.Lastname;
                participantsSheet.Range["D" + rowIndex].Value = participant.Player.Firstname;
                participantsSheet.Range["E" + rowIndex].FormulaLocal = string.Format("=RUNDEN({0};1)", participant.Player.Hcp.ToString().Replace(".", ","));
                participantsSheet.Range["F" + rowIndex].FormulaLocal = string.Format("=RUNDEN({0}*({1}/113)-{2}+{3};1)", ("E" + rowIndex), ("K" + rowIndex), ("J" + rowIndex), ("I" + rowIndex));
                participantsSheet.Range["G" + rowIndex].Value = participant.Player.HomeClub.Name;
                participantsSheet.Range["H" + rowIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teebox.Color.Value));
                participantsSheet.Range["I" + rowIndex].Value = teebox.Par;
                participantsSheet.Range["J" + rowIndex].Value = teebox.CourseRating;
                participantsSheet.Range["K" + rowIndex].Value = teebox.SlopeRating;
                participantsSheet.Range["L" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"_\" & $D{0} & \"!{1}7\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 5));
                participantsSheet.Range["M" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"_\" & $D{0} & \"!{1}12\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 5));
                participantsSheet.Range["N" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"_\" & $D{0} & \"!{1}13\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 5));
                rowIndex++;
            }

            MSExcel.Range usedrange = participantsSheet.UsedRange;
            usedrange.Columns.AutoFit();
        }

        public void SetScoresheet(TournamentParticipant participant, TeeBox teebox)
        {
            var player = participant.Player;
            var scoreSheet = SetScoreSheet(string.Format("{0}_{1}", player.Lastname, player.Firstname), string.Format("{0}, {1}", player.Lastname, player.Firstname), player.Hcp, participant.Teetime, teebox);
        }
    } 
}
