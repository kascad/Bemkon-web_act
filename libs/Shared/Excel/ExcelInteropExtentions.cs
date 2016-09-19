namespace Shared.Excel
{
    public class ExcelInteropExtentions
    {
        private Microsoft.Office.Interop.Excel._Application excelapp;
        private Microsoft.Office.Interop.Excel._Workbook wb;
        private Microsoft.Office.Interop.Excel._Worksheet ws;

        private System.Globalization.CultureInfo oldCI;

        public ExcelInteropExtentions()
        {
            excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = true;

            oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            wb = (Microsoft.Office.Interop.Excel._Workbook)excelapp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;

        }

        private void AddRageValue(string rangeLeftTop, string rangeRightBottom, string value)
        {
            Microsoft.Office.Interop.Excel.Range aRange = ws.get_Range(rangeLeftTop, rangeRightBottom);
            aRange.Value2 = value;
        }

        public void AddCellValue(string cellCoord, string value)
        {
            AddRageValue(cellCoord, cellCoord, value);
        }

        public void Close()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
        }
    }
}
