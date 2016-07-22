using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
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
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        public void SetCourse(Course course)
        {
            MSExcel.Worksheet clubSheet = this.xlWorkBook.Worksheets["Course"];

            clubSheet.Range["A2:D2"].End[MSExcel.XlDirection.xlDown].Clear();

            clubSheet.Range["C2"].Value = course.Id;
            clubSheet.Range["D2"].Value = course.Name;
        }

        public void SetTeeboxes(TeeboxCollection teeboxes)
        {
            MSExcel.Worksheet clubSheet = this.xlWorkBook.Worksheets["Teeboxes"];

            clubSheet.Range["A2:F2"].End[MSExcel.XlDirection.xlDown].Clear();

            int rowCounter = 1;
            for (int i = 0; i < teeboxes.Count; i++)
            {
                var teebox = teeboxes[i];

                clubSheet.Cells[rowCounter, 1].Value = teebox.Color.Value;
                clubSheet.Cells[rowCounter, 2].Value = teebox.Name;
                clubSheet.Cells[rowCounter, 3].Value = teebox.CourseRating;
                clubSheet.Cells[rowCounter, 4].Value = teebox.SlopeRating;
                clubSheet.Cells[rowCounter, 5].Value = teebox.Par;
                clubSheet.Cells[rowCounter, 6].Value = teebox.Holes.Count;

                rowCounter++;
            }
        }

        public void SetScoresheet(Player player, TeeBox teebox)
        {
            MSExcel.Worksheet scorecardSheet = xlWorkBook.Worksheets.Add();
            scorecardSheet.Name = "Scoresheet_" + player.Lastname + "_" + player.Firstname;

            int playerCol = 1;
            int playerHcpCol = 2;

            int labelCol = 2;
            int holeColStart = 3;

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

            int holeColIndex = holeColStart;

            string playHcpCell = "$" + GetExcelColumnName(playerHcpCol) + "$" + holeStrokesRow;


            scorecardSheet.Cells[holeStrokesRow, playerCol].Value = string.Format("{0}, {1}", player.Lastname, player.Firstname);
            scorecardSheet.Cells[holeStrokesRow, playerHcpCol].Value = player.Hcp;

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

            for (int i = 0; i < teebox.Holes.Count; i++)
            {
                var hole = teebox.Holes[i];

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
                scorecardSheet.Cells[holeDistanceRow, holeColIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teebox.Color.Value));


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

                

                int sumStart = holeColStart;
                if (i == 8 || i == 17)
                {
                    if (i == 17)
                        sumStart = holeColStart + 10;

                    scorecardSheet.Cells[holeParRow, holeColIndex + 1].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeParRow + ":" + GetExcelColumnName(holeColIndex) + holeParRow +")";
                    scorecardSheet.Cells[holeParRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeParRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
                    scorecardSheet.Cells[holeParRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeParRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeParRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    ((MSExcel.Range)scorecardSheet.Cells[holeParRow, holeColIndex + 1]).Font.Bold = true;

                    scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeDistanceRow + ":" + GetExcelColumnName(holeColIndex) + holeDistanceRow + ")";
                    scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
                    scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(teebox.Color.Value));
                    scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1].ColumnWidth = 6.43;
                    ((MSExcel.Range)scorecardSheet.Cells[holeDistanceRow, holeColIndex + 1]).Font.Bold = true;

                    scorecardSheet.Cells[holeNumberRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeNumberRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;

                    scorecardSheet.Cells[holeHcpRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeHcpRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;

                    scorecardSheet.Cells[sepRow1, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[sepRow1, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;

                    scorecardSheet.Cells[sepRow2, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[sepRow2, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;

                    scorecardSheet.Cells[holeStrokesRow, holeColIndex + 1].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeStrokesRow + ":" + GetExcelColumnName(holeColIndex) + holeStrokesRow + ")";
                    scorecardSheet.Cells[holeStrokesRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStrokesRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
                    scorecardSheet.Cells[holeStrokesRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStrokesRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStrokesRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    ((MSExcel.Range)scorecardSheet.Cells[holeStrokesRow, holeColIndex + 1]).Font.Bold = true;

                    scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex + 1].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeStrokesNettoRow + ":" + GetExcelColumnName(holeColIndex) + holeStrokesNettoRow + ")";
                    scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
                    scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    ((MSExcel.Range)scorecardSheet.Cells[holeStrokesNettoRow, holeColIndex + 1]).Font.Bold = true;

                    scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex + 1].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeStbfPointsNettoRow + ":" + GetExcelColumnName(holeColIndex) + holeStbfPointsNettoRow + ")";
                    scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
                    scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    ((MSExcel.Range)scorecardSheet.Cells[holeStbfPointsNettoRow, holeColIndex + 1]).Font.Bold = true;

                    scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex + 1].FormulaLocal = "=SUMME(" + GetExcelColumnName(sumStart) + holeStbfPointsBruttoRow + ":" + GetExcelColumnName(holeColIndex) + holeStbfPointsBruttoRow + ")";
                    scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeLeft].Weight = MSExcel.XlBorderWeight.xlMedium;
                    scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeRight].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex + 1].Borders.Item[MSExcel.XlBordersIndex.xlEdgeTop].LineStyle = MSExcel.XlLineStyle.xlContinuous;
                    ((MSExcel.Range)scorecardSheet.Cells[holeStbfPointsBruttoRow, holeColIndex + 1]).Font.Bold = true;

                    holeColIndex += 2;
                }
                else 
                    holeColIndex++;
            }

            //set sum column
            int sumCol = holeColStart + 20;
            int sumFrontCol = holeColStart + 9;
            int sumBackCol = holeColStart + 19;
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


            //hide calc rows
            scorecardSheet.Rows[holeCalcHcpRow].EntireRow.Hidden = true;
            scorecardSheet.Rows[holeCalcStrokesRow].EntireRow.Hidden = true;
            scorecardSheet.Rows[holeStrokesNettoRow].EntireRow.Hidden = true;
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
