using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ProfessorTesting.Old_App_Code
{
    public class ExportTable
    {
        private Table table;

        public ExportTable()
        {
            table = new Table();
            table.GridLines = GridLines.Both;
        }

        public Table Table
        {
            get { return table; }
        }

        public TableRow AddRow()
        {
            TableRow row = new TableRow();
            table.Rows.Add(row);
            return row;
        }

        public TableHeaderRow AddHeadRow()
        {
            TableHeaderRow row = new TableHeaderRow();
            table.Rows.Add(row);
            return row;
        }

        public TableHeaderCell AddHeadCell(TableHeaderRow headRow, string text)
        {
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = text;
            headRow.Cells.Add(cell);
            return cell;
        }

        public TableCell AddCell(TableRow row, string text)
        {
            TableCell cell = new TableCell();
            cell.Text = text;
            row.Cells.Add(cell);
            return cell;
        }
    }
}
