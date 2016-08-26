using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Golf.Excel
{
    public abstract class TournamentWorkbookBase : IDisposable
    {
        protected static readonly object misValue = System.Reflection.Missing.Value;

        protected MSExcel.Application xlApp;
        protected MSExcel.Workbook xlWorkBook;

        protected int holeNumberRow = 2;
        protected int holeParRow = 3;
        protected int holeHcpRow = 4;
        protected int holeDistanceRow = 5;

        protected int sepRow1 = 6;
        protected int holeStrokesRow = 7;
        protected int sepRow2 = 8;

        protected int holeCalcHcpRow = 9;
        protected int holeCalcStrokesRow = 10;
        protected int holeStrokesNettoRow = 11;

        protected int holeStbfPointsNettoRow = 12;
        protected int holeStbfPointsBruttoRow = 13;

        protected int playerCol = 1;
        protected int playerSpvgCol = 3;

        protected int slopeRatingLabelCol = 1;
        protected int slopeRatingLabelRow = 3;
        protected int slopeRatingCol = 2;
        protected int slopeRatingRow = 3;

        protected int teetimeLabelCol = 1;
        protected int teetimeLabelRow = 15;
        protected int teetimeCol = 2;
        protected int teetimeRow = 15;

        protected int courseRatingLabelCol = 1;
        protected int courseRatingLabelRow = 2;
        protected int courseRatingCol = 2;
        protected int courseRatinglRow = 2;

        protected int playerHcpRow = 12;
        protected int playerHcpCol = 1;

        protected int labelCol = 3;
        protected int holeColStart = 4;

        public TournamentWorkbookBase()
        {

        }

        public void Open(string fileName)
        {
            try
            {
                xlApp = new MSExcel.Application();
                xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, MSExcel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            }
            catch (Exception ex)
            {
                throw;
            }
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

        protected virtual MSExcel.Worksheet SetScoreSheet(string sheetName, string playername, float hcp, DateTime teetime, TeeBox teebox)
        {
            var teeboxSheet = this.xlWorkBook.Worksheets["Teeboxes"];
            MSExcel.Worksheet scorecardSheet = xlWorkBook.Worksheets.Add(After: teeboxSheet);
            scorecardSheet.Name = "Scoresheet_" + sheetName;

            int holeColIndex = holeColStart;

            string playSpvgCell = "$" + GetExcelColumnName(playerSpvgCol) + "$" + holeStrokesRow;

            MSExcel.Range playerCell = scorecardSheet.Cells[holeStrokesRow, playerCol];
            playerCell.Value = string.Format("{0}", playername);
            playerCell.Font.Size = 20;
            playerCell.Font.Bold = true;

            scorecardSheet.Cells[playerHcpRow, playerHcpCol].FormulaLocal = string.Format("=RUNDEN({0};1)", hcp.ToString().Replace(".", ","));
            scorecardSheet.Cells[holeStrokesRow, playerSpvgCol].FormulaLocal = string.Format("=RUNDEN({0}*({1}/113)-{2}+{3};1)", (GetExcelColumnName(playerHcpCol) + playerHcpRow), teebox.SlopeRating.ToString().Replace(".", ","), teebox.CourseRating.ToString().Replace(".", ","), teebox.Par);

            MSExcel.Range slopeLabelCell = scorecardSheet.Cells[slopeRatingLabelRow, slopeRatingLabelCol];
            slopeLabelCell.Value = "SlopeRating";
            slopeLabelCell.Font.Size = 8;
            slopeLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

            MSExcel.Range slopeCell = scorecardSheet.Cells[slopeRatingRow, slopeRatingCol];
            slopeCell.FormulaLocal = string.Format("=RUNDEN({0};1)", teebox.SlopeRating.ToString().Replace(".", ","));
            slopeCell.Font.Size = 8;
            slopeCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignLeft;

            MSExcel.Range courseLabelCell = scorecardSheet.Cells[courseRatingLabelRow, courseRatingLabelCol];
            courseLabelCell.Value = "CourseRating";
            courseLabelCell.Font.Size = 8;
            courseLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

            MSExcel.Range courseCell = scorecardSheet.Cells[courseRatinglRow, courseRatingCol];
            courseCell.FormulaLocal = string.Format("=RUNDEN({0};1)", teebox.CourseRating.ToString().Replace(".", ","));
            courseCell.Font.Size = 8;

            MSExcel.Range teetimeLabelCell = scorecardSheet.Cells[teetimeLabelRow, teetimeLabelCol];
            teetimeLabelCell.Value = "Teetime";
            teetimeLabelCell.Font.Size = 8;
            teetimeLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

            MSExcel.Range teetimeCell = scorecardSheet.Cells[teetimeRow, teetimeCol];
            teetimeCell.Value = teetime.ToString("hh:mm");
            teetimeCell.Font.Size = 8;

            MSExcel.Range holeLabelCell = scorecardSheet.Cells[holeNumberRow, labelCol];
            holeLabelCell.Value = "Hole";
            holeLabelCell.Font.Size = 8;
            holeLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

            MSExcel.Range parLabelCell = scorecardSheet.Cells[holeParRow, labelCol];
            parLabelCell.Value = "Par";
            parLabelCell.Font.Size = 8;
            parLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

            MSExcel.Range hcpLabelCell = scorecardSheet.Cells[holeCalcHcpRow, labelCol];
            hcpLabelCell.Value = "Hcp";
            hcpLabelCell.Font.Size = 8;
            hcpLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

            MSExcel.Range teeboxLabelCell = scorecardSheet.Cells[holeDistanceRow, labelCol];
            teeboxLabelCell.Value = teebox.Name;
            teeboxLabelCell.Cells[holeDistanceRow, labelCol].Font.Size = 8;
            teeboxLabelCell.Cells[holeDistanceRow, labelCol].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teebox.Color.Value));
            teeboxLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

            MSExcel.Range nettoLabelCell = scorecardSheet.Cells[holeStbfPointsNettoRow, labelCol];
            nettoLabelCell.Value = "Netto";
            nettoLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

            MSExcel.Range bruttoLabelCell = scorecardSheet.Cells[holeStbfPointsBruttoRow, labelCol];
            bruttoLabelCell.Value = "Brutto";
            bruttoLabelCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignRight;

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

            MSExcel.Range usedrange = scorecardSheet.UsedRange;
            usedrange.Columns.AutoFit();
            return scorecardSheet;
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

            MSExcel.Range usedrange = clubSheet.UsedRange;
            usedrange.Columns.AutoFit();
        }

        public void SetSums(MSExcel.Worksheet scorecardSheet, string teeboxColor, int sumIndex, int sumStart)
        {
            scorecardSheet.Cells[holeParRow, sumIndex].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeParRow + ":" + GetExcelColumnName(sumIndex - 1) + holeParRow + ")";
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeParRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            ((MSExcel.Range)scorecardSheet.Cells[holeParRow, sumIndex]).Font.Bold = true;

            scorecardSheet.Cells[holeDistanceRow, sumIndex].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeDistanceRow + ":" + GetExcelColumnName(sumIndex - 1) + holeDistanceRow + ")";
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            scorecardSheet.Cells[holeDistanceRow, sumIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teeboxColor));
            scorecardSheet.Cells[holeDistanceRow, sumIndex].ColumnWidth = 6.43;
            ((MSExcel.Range)scorecardSheet.Cells[holeDistanceRow, sumIndex]).Font.Bold = true;
            ((MSExcel.Range)scorecardSheet.Cells[holeDistanceRow, sumIndex]).Font.Size = 8;

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

        public void SetHole(MSExcel.Worksheet scorecardSheet, string teeboxColor, CourseHole hole, int holeNumberRow, int index, int holeColIndex, int sumStart, int last, string playHcpCell)
        {
            ((MSExcel.Range)scorecardSheet.Cells[holeNumberRow, holeColIndex]).EntireColumn.Insert(MSExcel.XlInsertShiftDirection.xlShiftToRight,
                MSExcel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);

            MSExcel.Range numberCell = scorecardSheet.Cells[holeNumberRow, holeColIndex];
            //hole numbers

            numberCell.Value = hole.Number;
            numberCell.Font.Bold = true;
            numberCell.Font.Size = 16;
            numberCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            numberCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            numberCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            numberCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            numberCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignCenter;
            //set column widths
            numberCell.ColumnWidth = 4.29;

            //hole par
            MSExcel.Range parCell = scorecardSheet.Cells[holeParRow, holeColIndex];
            parCell.Value = hole.Par;
            parCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            parCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            parCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            parCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            parCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignCenter;

            //hole hcp
            MSExcel.Range hcpCell = scorecardSheet.Cells[holeHcpRow, holeColIndex];
            hcpCell.Value = hole.Hcp;
            hcpCell.Font.Size = 8;
            hcpCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            hcpCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            hcpCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            hcpCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            hcpCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignCenter;

            //hole distance
            MSExcel.Range distanceCell = scorecardSheet.Cells[holeDistanceRow, holeColIndex];
            distanceCell.Value = hole.Distance;
            distanceCell.Font.Size = 8;
            distanceCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            distanceCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            distanceCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            distanceCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            distanceCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teeboxColor));
            distanceCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignCenter;

            //hole strokes
            MSExcel.Range strokeCell = scorecardSheet.Cells[holeStrokesRow, holeColIndex];
            strokeCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            strokeCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            strokeCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            strokeCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            strokeCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignCenter;

            scorecardSheet.Cells[holeCalcHcpRow, holeColIndex].Formula = "=" + playHcpCell + "-" + GetExcelColumnName(holeColIndex) + "$" + holeHcpRow;

            string holeCalcHcpCell = GetExcelColumnName(holeColIndex) + "" + holeCalcHcpRow;

            ((MSExcel.Range)scorecardSheet.Cells[holeCalcStrokesRow, holeColIndex]).FormulaLocal = string.Format("=WENN({0}<0;0;WENN({0}<18;1;WENN({0}<36;2;3)))", holeCalcHcpCell, holeCalcHcpCell, holeCalcHcpCell);

            scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex].Formula = "=" + GetExcelColumnName(holeColIndex) + "$" + holeParRow + "-" + GetExcelColumnName(holeColIndex) + "" + holeStrokesRow;

            string holeStrokesCell = GetExcelColumnName(holeColIndex) + "" + holeStrokesRow;
            string holeCalcStrokesCell = GetExcelColumnName(holeColIndex) + "" + holeCalcStrokesRow;
            string holeStrokesNettoCell = GetExcelColumnName(holeColIndex) + "" + holeStrokesNettoRow;

            MSExcel.Range nettoPointsCell = scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex];
            nettoPointsCell.FormulaLocal = string.Format("=WENN({0}<1;\"\";WENN((2+{1}+{3})>-1;(2+{2}+{4});0))", holeStrokesCell, holeStrokesNettoCell, holeStrokesNettoCell, holeCalcStrokesCell, holeCalcStrokesCell);
            nettoPointsCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            nettoPointsCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            nettoPointsCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            nettoPointsCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            nettoPointsCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignCenter;

            MSExcel.Range bruttoPointsCell = scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex];
            bruttoPointsCell.FormulaLocal = string.Format("=WENN({0}<1;\"\";WENN((2+{1})>-1;(2+{2});0))", holeStrokesCell, holeStrokesNettoCell, holeStrokesNettoCell);
            bruttoPointsCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            bruttoPointsCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            bruttoPointsCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            bruttoPointsCell.Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
            bruttoPointsCell.Style.HorizontalAlignment = MSExcel.XlHAlign.xlHAlignCenter;
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
