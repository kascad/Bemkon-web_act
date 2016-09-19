using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Shared;

namespace ProfessorTesting
{
    public partial class SelectTestExamineePanel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Init();
        }

        private new void Init()
        {
            try
            {
                Testing.TestingDataContext dc = new Testing.TestingDataContext(GlobalOptions.RemoteConnectionString);

                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerHtml = "<div class='test_title_item_battery'><a name='battery'>"
                    + "Батареи тестов</a></div>";
                HtmlTableRow row = new HtmlTableRow();
                row.Cells.Add(cell);
                listTest.Rows.Add(row);

                cell = new HtmlTableCell();
                cell.InnerHtml = "<div class='test_title_item_battery'><a href='#battery'>" 
                    + "Батареи тестов</a></div>";
                row = new HtmlTableRow();
                row.Cells.Add(cell);
                listTitleTest.Rows.Add(row);

                foreach (var bt in dc.Batteries)
                {
                    cell = new HtmlTableCell();
                    cell.InnerHtml = "<div class='test_item_battery'>"
                        + @"<a href='/User/TestExaminee.aspx?id_battery=" + bt.BatteryID 
                        + "&from=" + (int)FromType.Examinee + "'>" 
                        + bt.BatteryName + "</a></div>";
                    row = new HtmlTableRow();
                    row.Cells.Add(cell);
                    listTest.Rows.Add(row);
                }

                foreach (var cat in dc.Categories)
                {
                    cell = new HtmlTableCell();
                    cell.InnerHtml = "<div class='test_title_item'><a name='" + cat.CategoryID + "'>" + cat.Name + "</a></div>";
                    row = new HtmlTableRow();
                    row.Cells.Add(cell);
                    listTest.Rows.Add(row);

                    cell = new HtmlTableCell();
                    cell.InnerHtml = "<div class='test_title_item_index'><a href='#" + cat.CategoryID + "'>"
                        + cat.Name +  "</a></div>";
                    row = new HtmlTableRow();
                    row.Cells.Add(cell);
                    listTitleTest.Rows.Add(row);

                    foreach (var t in cat.Tests)
                    {
                        cell = new HtmlTableCell();
                        cell.InnerHtml = "<div class='test_item'>"
                            + @"<a href='/User/TestExaminee.aspx?id_test=" + t.TestID 
                            + "&from=" + (int)FromType.Examinee + "'>"
                            + t.FullName + "</a></div>";
                        row = new HtmlTableRow();
                        row.Cells.Add(cell);
                        listTest.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Core.Site.RedirectError(Response, ex.Message);
            }
        }
    }
}