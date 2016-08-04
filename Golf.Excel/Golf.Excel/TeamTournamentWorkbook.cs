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
            //participantsSheet.Range["A2:K2"].End[MSExcel.XlDirection.xlDown].Clear();

            teamsSheet.Range["A1"].Value = "Id";
            teamsSheet.Range["B1"].Value = "Teetime";
            teamsSheet.Range["C1"].Value = "Lastname";
            teamsSheet.Range["E1"].Value = "Hcp";
            teamsSheet.Range["F1"].Value = "Spvg";
            teamsSheet.Range["H1"].Value = "Teebox";
            teamsSheet.Range["I1"].Value = "Par";
            teamsSheet.Range["J1"].Value = "CourseRating";
            teamsSheet.Range["K1"].Value = "SlopeRating";
            teamsSheet.Range["L1"].Value = "Strokes";
            teamsSheet.Range["M1"].Value = "Brutto";
            teamsSheet.Range["N1"].Value = "Netto";

            int rowIndex = 2;
            foreach (var team in tournament.Teams)
            {
                var teebox = teeboxes.FirstOrDefault(t => t.Id == team.TeeboxId);

                teamsSheet.Range["A" + rowIndex].Value = team.Id;
                teamsSheet.Range["B" + rowIndex].Value = team.Teetime;
                teamsSheet.Range["C" + rowIndex].Value = team.Name;
                teamsSheet.Range["E" + rowIndex].FormulaLocal = string.Format("=RUNDEN({0};1)", team.Hcp.ToString().Replace(".", ","));
                teamsSheet.Range["F" + rowIndex].FormulaLocal = string.Format("=RUNDEN({0}*({1}/113)-{2}+{3};1)", ("E" + rowIndex), ("K" + rowIndex), ("J" + rowIndex), ("I" + rowIndex));
                teamsSheet.Range["H" + rowIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teebox.Color.Value));
                teamsSheet.Range["I" + rowIndex].Value = teebox.Par;
                teamsSheet.Range["J" + rowIndex].Value = teebox.CourseRating;
                teamsSheet.Range["K" + rowIndex].Value = teebox.SlopeRating;
                teamsSheet.Range["L" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"_\" & $D{0} & \"!{1}7\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 5));
                teamsSheet.Range["M" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"_\" & $D{0} & \"!{1}12\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 5));
                teamsSheet.Range["N" + rowIndex].FormulaLocal = string.Format("=INDIREKT(\"Scoresheet_\" & $C{0} & \"_\" & $D{0} & \"!{1}13\")", rowIndex, GetExcelColumnName(teebox.Holes.Count + 5));
                rowIndex++;
            }
        }

        public void SetScoresheet(Team team, TeeBox teebox)
        {
 
            var teeboxSheet = this.xlWorkBook.Worksheets["Teeboxes"];
            MSExcel.Worksheet scorecardSheet = xlWorkBook.Worksheets.Add(After: teeboxSheet);
            scorecardSheet.Name = "Scoresheet_" + team.Name;



            int holeColIndex = holeColStart;

            string playSpvgCell = "$" + GetExcelColumnName(playerSpvgCol) + "$" + holeStrokesRow;


            scorecardSheet.Cells[holeStrokesRow, playerCol].Value = string.Format("{0}, {1}", team.Name);
            scorecardSheet.Cells[playerHcpRow, playerHcpCol].FormulaLocal = string.Format("=RUNDEN({0};1)", team.Hcp.ToString().Replace(".", ","));
            scorecardSheet.Cells[holeStrokesRow, playerSpvgCol].FormulaLocal = string.Format("=RUNDEN({0}*({1}/113)-{2}+{3};1)", (GetExcelColumnName(playerHcpCol) + playerHcpRow), teebox.SlopeRating.ToString().Replace(".", ","), teebox.CourseRating.ToString().Replace(".", ","), teebox.Par);

            scorecardSheet.Cells[holeNumberRow, labelCol].Value = "Hole";
            scorecardSheet.Cells[holeParRow, labelCol].Value = "Par";
            scorecardSheet.Cells[holeHcpRow, labelCol].Value = "Hcp";
            scorecardSheet.Cells[holeDistanceRow, labelCol].Value = teebox.Name;
            scorecardSheet.Cells[holeDistanceRow, labelCol].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teebox.Color.Value));

            scorecardSheet.Cells[holeStbfPointsNettoRow, labelCol].Value = "Netto";
            scorecardSheet.Cells[holeStbfPointsBruttoRow, labelCol].Value = "Brutto";

            //Set seperator row heights
            ((MSExcel.Range)scorecardSheet.Rows[sepRow1]).RowHeight = 3.75;
            ((MSExcel.Range)scorecardSheet.Rows[sepRow2]).RowHeight = 3.75;

            var frontHoles = teebox.Holes.Front.OrderByDescending(h => h.Number).ToList();
            var backHoles = teebox.Holes.Back.OrderByDescending(h => h.Number).ToList();


            int sumFrontCol = holeColStart + frontHoles.Count;
            int sumBackCol = holeColStart + frontHoles.Count + backHoles.Count + 1;
            int sumCol = sumBackCol + 1;

            //front
            holeColIndex = holeColStart;
            for (int i = 0; i < frontHoles.Count; i++)
            {
                SetHole(scorecardSheet, teebox.Color.Value, frontHoles[i], holeNumberRow, i, holeColStart, holeColIndex, frontHoles.Count - 1, playSpvgCell);
                if (i == frontHoles.Count)
                {

                }
                else
                    holeColIndex++;
            }
            SetSums(scorecardSheet, teebox.Color.Value, sumFrontCol, holeColStart);

            //back
            for (int i = 0; i < backHoles.Count; i++)
            {
                SetHole(scorecardSheet, teebox.Color.Value, backHoles[i], holeNumberRow, i, sumFrontCol + 1, sumFrontCol + 1, backHoles.Count - 1, playSpvgCell);
                if (i == backHoles.Count)
                {

                }
                else
                    holeColIndex++;
            }
            SetSums(scorecardSheet, teebox.Color.Value, sumBackCol, sumFrontCol + 1);

            //set sum column
            scorecardSheet.Cells[holeParRow, sumCol].FormulaLocal = string.Format("={0}+{1}", GetExcelColumnName(sumFrontCol) + holeParRow, GetExcelColumnName(sumBackCol) + holeParRow);
            scorecardSheet.Cells[holeParRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeParRow, sumCol]).Font.Bold = true;
            scorecardSheet.Cells[holeParRow, sumCol].ColumnWidth = 6.43;

            scorecardSheet.Cells[holeDistanceRow, sumCol].FormulaLocal = string.Format("={0}+{1}", GetExcelColumnName(sumFrontCol) + holeDistanceRow, GetExcelColumnName(sumBackCol) + holeDistanceRow);
            scorecardSheet.Cells[holeDistanceRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeDistanceRow, sumCol]).Font.Bold = true;

            scorecardSheet.Cells[holeStrokesRow, sumCol].FormulaLocal = string.Format("={0}+{1}", GetExcelColumnName(sumFrontCol) + holeStrokesRow, GetExcelColumnName(sumBackCol) + holeStrokesRow);
            scorecardSheet.Cells[holeStrokesRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeStrokesRow, sumCol]).Font.Bold = true;

            scorecardSheet.Cells[holeStrokesNettoRow, sumCol].FormulaLocal = string.Format("={0}+{1}", GetExcelColumnName(sumFrontCol) + holeStrokesNettoRow, GetExcelColumnName(sumBackCol) + holeStrokesNettoRow);
            scorecardSheet.Cells[holeStrokesNettoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesNettoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesNettoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesNettoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeStrokesNettoRow, sumCol]).Font.Bold = true;

            scorecardSheet.Cells[holeStbfPointsNettoRow, sumCol].FormulaLocal = string.Format("={0}+{1}", GetExcelColumnName(sumFrontCol) + holeStbfPointsNettoRow, GetExcelColumnName(sumBackCol) + holeStbfPointsNettoRow);
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeStbfPointsNettoRow, sumCol]).Font.Bold = true;

            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumCol].FormulaLocal = string.Format("={0}+{1}", GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow);
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumCol].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeStbfPointsBruttoRow, sumCol]).Font.Bold = true;

            //draw borders

            //front
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(holeColStart) + holeNumberRow, GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(holeColStart) + holeNumberRow, GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlThick;

            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(holeColStart) + holeNumberRow, GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(holeColStart) + holeNumberRow, GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].Weight = MSExcel.XlBorderWeight.xlThick;

            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(holeColStart) + holeNumberRow, GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(holeColStart) + holeNumberRow, GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].Weight = MSExcel.XlBorderWeight.xlThick;

            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(holeColStart) + holeNumberRow, GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(holeColStart) + holeNumberRow, GetExcelColumnName(sumFrontCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].Weight = MSExcel.XlBorderWeight.xlThick;

            //back
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(sumFrontCol + 1) + holeNumberRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(sumFrontCol + 1) + holeNumberRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlThick;

            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(sumFrontCol + 1) + holeNumberRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(sumFrontCol + 1) + holeNumberRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].Weight = MSExcel.XlBorderWeight.xlThick;

            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(sumFrontCol + 1) + holeNumberRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(sumFrontCol + 1) + holeNumberRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].Weight = MSExcel.XlBorderWeight.xlThick;

            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(sumFrontCol + 1) + holeNumberRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Range[string.Format("{0}:{1}", GetExcelColumnName(sumFrontCol + 1) + holeNumberRow, GetExcelColumnName(sumBackCol) + holeStbfPointsBruttoRow)].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].Weight = MSExcel.XlBorderWeight.xlThick;


            ////hide calc rows
            scorecardSheet.Rows[holeCalcHcpRow].EntireRow.Hidden = true;
            scorecardSheet.Rows[holeCalcStrokesRow].EntireRow.Hidden = true;
            scorecardSheet.Rows[holeStrokesNettoRow].EntireRow.Hidden = true;
        }
    }
}
