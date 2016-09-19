using System.Diagnostics;
using System.IO;
using ExcelLibrary.SpreadSheet;

namespace Shared.Excel
{
    public class ExcelExtension 
    {
        private readonly Workbook _workbook;
        private readonly Worksheet _worksheet;
        private readonly string _tempFile;

        public ExcelExtension()
        {
            _workbook = new Workbook();
            _worksheet = new Worksheet("Sheet");
            var tempFileName = string.Format("{0}.xls", Helper.GeneratePassword());
            _tempFile = Path.Combine(Path.GetTempPath(), tempFileName);

			AddBlankRows();
        }

	    private void AddBlankRows()
	    {
		    for (int i = 0; i < 150; i++)
		    {
			    for (int j = 0; j < 10; j++)
			    {
				    _worksheet.Cells[i, j] = new Cell(" ");
			    }
		    }
	    }

	    public void AddCellValue(int rowIndex, int columnIndex, string value)
        {
            _worksheet.Cells[rowIndex, columnIndex] = new Cell(value);
        }

        public void Close()
        {
            _workbook.Worksheets.Add(_worksheet);
            _workbook.Save(_tempFile);
            Process.Start(_tempFile);
        }
    }
}
