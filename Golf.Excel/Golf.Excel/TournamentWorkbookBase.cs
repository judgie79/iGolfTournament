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
        protected int playerSpvgCol = 2;

        protected int playerHcpRow = 2;
        protected int playerHcpCol = 1;

        protected int labelCol = 2;
        protected int holeColStart = 3;

        public TournamentWorkbookBase()
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
