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
    public class TournamentWorkbook : IDisposable
    {
        private static readonly object misValue = System.Reflection.Missing.Value;

        private MSExcel.Application xlApp;
        private MSExcel.Workbook xlWorkBook;

        int holeNumberRow = 2;
        int holeParRow = 3;
        int holeHcpRow = 4;
        int holeDistanceRow = 5;

        int sepRow1 = 6;
        int holeStrokesRow = 7;
        int sepRow2 = 8;

        int holeCalcHcpRow = 9;
        int holeCalcStrokesRow = 10;
        int holeStrokesNettoRow = 11;

        int holeStbfPointsNettoRow = 12;
        int holeStbfPointsBruttoRow = 13;

        int playerCol = 1;
        int playerSpvgCol = 2;

        int playerHcpRow = 2;
        int playerHcpCol = 1;

        int labelCol = 2;
        int holeColStart = 3;

        

        public TournamentWorkbook()
        {

        }

        public void Open(string fileName)
        {
            xlApp = new MSExcel.Application();
            xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, MSExcel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        }

        public void Save()
        {
            xlWorkBook.Save();
        }

        public void SaveAs(string fileName)
        {
            xlWorkBook.SaveCopyAs(fileName);
        }

        public void OpenFromTemplate(string fileName)
        {
            xlApp = new MSExcel.Application();
            xlWorkBook = xlApp.Workbooks.Add();

            MSExcel.Workbook xlWorkBookTemplate = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, MSExcel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            xlWorkBookTemplate.Worksheets.Copy(xlWorkBook.Worksheets);

            xlWorkBook.SaveAs("");


            xlWorkBookTemplate.Close(true, misValue, misValue);
            releaseObject(xlWorkBookTemplate);
        }

        public void CloseExcel(bool saveChanges)
        {
            xlWorkBook.Close(saveChanges, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        public void Dispose()
        {
            xlWorkBook.Close(false, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        public void SetCourse(Club club, Course course)
        {
            MSExcel.Worksheet clubSheet = this.xlWorkBook.Worksheets["Course"];

            clubSheet.Range["A2:D2"].End[MSExcel.XlDirection.xlDown].Clear();

            clubSheet.Range["A2"].Value = club.Id;
            clubSheet.Range["B2"].Value = club.Name;
            clubSheet.Range["C2"].Value = course.Id;
            clubSheet.Range["D2"].Value = course.Name;
        }

        public void SetTeeboxes(TeeboxCollection teeboxes)
        {
            MSExcel.Worksheet clubSheet = this.xlWorkBook.Worksheets["Teeboxes"];

            clubSheet.Range["A2:F2"].End[MSExcel.XlDirection.xlDown].Clear();
            object[,] values = new object[teeboxes.Count, 6];

            int rowCounter = 0;
            for (int i = 0; i < teeboxes.Count; i++)
            {
                var teebox = teeboxes[i];

                var currentRow = new object[1, 6];

                values[rowCounter, 0] = teebox.Color.Value;
                values[rowCounter, 1] = teebox.Name;
                values[rowCounter, 2] = teebox.CourseRating;
                values[rowCounter, 3] = teebox.SlopeRating;
                values[rowCounter, 4] = teebox.Par;
                values[rowCounter, 5] = teebox.Holes.Count;
                
                
                rowCounter++;
            }
            clubSheet.Range[string.Format("A2:F{0}", teeboxes.Count + 1)].Value = values;


        }

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
        }

       
        public void SetScoresheet(TournamentParticipant participant, TeeBox teebox)
        {
            var player = participant.Player;
            var teeboxSheet = this.xlWorkBook.Worksheets["Teeboxes"];
            MSExcel.Worksheet scorecardSheet = xlWorkBook.Worksheets.Add(After: teeboxSheet);
            scorecardSheet.Name = "Scoresheet_" + player.Lastname + "_" + player.Firstname;

            

            int holeColIndex = holeColStart;

            string playSpvgCell = "$" + GetExcelColumnName(playerSpvgCol) + "$" + holeStrokesRow;


            scorecardSheet.Cells[holeStrokesRow, playerCol].Value = string.Format("{0}, {1}", player.Lastname, player.Firstname);
            scorecardSheet.Cells[playerHcpRow, playerHcpCol].FormulaLocal = string.Format("=RUNDEN({0};1)", player.Hcp.ToString().Replace(".", ","));
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
            SetSums(scorecardSheet, teebox.Color.Value, sumBackCol, sumFrontCol+1);

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

        public void SetSums(MSExcel.Worksheet scorecardSheet, string teeboxColor, int sumIndex, int sumStart)
        {
            scorecardSheet.Cells[holeParRow, sumIndex].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeParRow + ":" + GetExcelColumnName(sumIndex-1) + holeParRow + ")";
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeParRow, sumIndex]).Font.Bold = true;

            scorecardSheet.Cells[holeDistanceRow, sumIndex].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeDistanceRow + ":" + GetExcelColumnName(sumIndex-1) + holeDistanceRow + ")";
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teeboxColor));
            scorecardSheet.Cells[holeDistanceRow, sumIndex].ColumnWidth = 6.43;
            ((MSExcel.Range)scorecardSheet.Cells[holeDistanceRow, sumIndex]).Font.Bold = true;

            scorecardSheet.Cells[holeNumberRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeNumberRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;

            scorecardSheet.Cells[holeHcpRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeHcpRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;

            scorecardSheet.Cells[sepRow1, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[sepRow1, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;

            scorecardSheet.Cells[sepRow2, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[sepRow2, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;

            scorecardSheet.Cells[holeStrokesRow, sumIndex].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeStrokesRow + ":" + GetExcelColumnName(sumIndex - 1) + holeStrokesRow + ")";
            scorecardSheet.Cells[holeStrokesRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
            scorecardSheet.Cells[holeStrokesRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeStrokesRow, sumIndex]).Font.Bold = true;

            scorecardSheet.Cells[holeStrokesNettoRow, sumIndex].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeStrokesNettoRow + ":" + GetExcelColumnName(sumIndex - 1) + holeStrokesNettoRow + ")";
            scorecardSheet.Cells[holeStrokesNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
            scorecardSheet.Cells[holeStrokesNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeStrokesNettoRow, sumIndex]).Font.Bold = true;

            scorecardSheet.Cells[holeStbfPointsNettoRow, sumIndex].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeStbfPointsNettoRow + ":" + GetExcelColumnName(sumIndex - 1) + holeStbfPointsNettoRow + ")";
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeStbfPointsNettoRow, sumIndex]).Font.Bold = true;

            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumIndex].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeStbfPointsBruttoRow + ":" + GetExcelColumnName(sumIndex - 1) + holeStbfPointsBruttoRow + ")";
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeStbfPointsBruttoRow, sumIndex]).Font.Bold = true;
        }

        public void SetHole(MSExcel.Worksheet scorecardSheet, string teeboxColor, Hole hole, int holeNumberRow, int index, int holeColIndex, int sumStart, int last, string playHcpCell)
        {

            ((MSExcel.Range)scorecardSheet.Cells[holeNumberRow, holeColIndex]).EntireColumn.Insert(MSExcel.XlInsertShiftDirection.xlShiftToRight,
                MSExcel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);

            //hole numbers
            scorecardSheet.Cells[holeNumberRow, holeColIndex].Value = hole.Number;
            ((MSExcel.Range)scorecardSheet.Cells[holeNumberRow, holeColIndex]).Font.Bold = true;
            ((MSExcel.Range)scorecardSheet.Cells[holeNumberRow, holeColIndex]).Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeNumberRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeNumberRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeNumberRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;


            //hole par
            scorecardSheet.Cells[holeParRow, holeColIndex].Value = hole.Par;
            scorecardSheet.Cells[holeParRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;

            //hole hcp
            scorecardSheet.Cells[holeHcpRow, holeColIndex].Value = hole.Hcp;
            scorecardSheet.Cells[holeHcpRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeHcpRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeHcpRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeHcpRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;

            //hole distance
            scorecardSheet.Cells[holeDistanceRow, holeColIndex].Value = hole.Distance;
            scorecardSheet.Cells[holeDistanceRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, holeColIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teeboxColor));


            //hole strokes
            scorecardSheet.Cells[holeStrokesRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStrokesRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;


            scorecardSheet.Cells[holeCalcHcpRow, holeColIndex].Formula = "=" + playHcpCell + "-" + GetExcelColumnName(holeColIndex) + "$" + holeHcpRow;

            string holeCalcHcpCell = GetExcelColumnName(holeColIndex) + "" + holeCalcHcpRow;

            ((MSExcel.Range)scorecardSheet.Cells[holeCalcStrokesRow, holeColIndex]).FormulaLocal = string.Format("=WENN({0}<0;0;WENN({0}<18;1;WENN({0}<36;2;3)))", holeCalcHcpCell, holeCalcHcpCell, holeCalcHcpCell);

            scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex].Formula = "=" + GetExcelColumnName(holeColIndex) + "$" + holeParRow + "-" + GetExcelColumnName(holeColIndex) + "" + holeStrokesRow;

            string holeStrokesCell = GetExcelColumnName(holeColIndex) + "" + holeStrokesRow;
            string holeCalcStrokesCell = GetExcelColumnName(holeColIndex) + "" + holeCalcStrokesRow;
            string holeStrokesNettoCell = GetExcelColumnName(holeColIndex) + "" + holeStrokesNettoRow;

            scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex].FormulaLocal = string.Format("=WENN({0}<1;\"\";WENN((2+{1}+{3})>-1;(2+{2}+{4});0))", holeStrokesCell, holeStrokesNettoCell, holeStrokesNettoCell, holeCalcStrokesCell, holeCalcStrokesCell);
            scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;

            scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex].FormulaLocal = string.Format("=WENN({0}<1;\"\";WENN((2+{1})>-1;(2+{2});0))", holeStrokesCell, holeStrokesNettoCell, holeStrokesNettoCell);
            scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;

            //set column widths
            ((MSExcel.Range)scorecardSheet.Cells[holeNumberRow, holeColIndex]).ColumnWidth = 4.29;
        }

        public static int ExcelColumnNameToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName");

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += (columnName[i] - 'A' + 1);
            }

            return sum;
        }

        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
