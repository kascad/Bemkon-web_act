using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ProfessorTesting.User
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
