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

namespace ProfessorTesting
{
    public partial class TestsPanel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GreateTestList();
        }

        private void GreateTestList()
        {
            int cnt = 0;
            try
            {
                Testing.TestingDataContext dc = new Testing.TestingDataContext(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);//Testing.GlobalOptions.ConnectionStr);
                foreach (var cat in dc.Categories)
                {
                    HtmlTableCell cell = new HtmlTableCell();
                    cell.InnerHtml = "<div class='test_group_item'>"
                        + cat.Name
                        + "</div>";
                    cell.ColSpan = 4;
                    HtmlTableRow row = new HtmlTableRow();
                    row.Cells.Add(cell);
                    listTest.Rows.Add(row);


                    foreach (var t in cat.Tests)
                    {
                        row = new HtmlTableRow();
                        cell = new HtmlTableCell();
                        cell.InnerHtml = "<a href='/User/TestExaminee.aspx?id_test="
                            + t.TestID + "&from=" + (int)FromType.Test + "'>" + t.FullName + "</a>";
                        row.Cells.Add(cell);

                        cell = new HtmlTableCell();
                        cell.InnerHtml = "<span class='test_nogroup_item'>" + t.ShortName + "</span>";
                        row.Cells.Add(cell);

                        cell = new HtmlTableCell();
                        cell.InnerHtml = t.Author;
                        row.Cells.Add(cell);

                        cell = new HtmlTableCell();
                        if (t.Date.HasValue)
                        {
                            cell.InnerHtml = t.Date.Value.ToShortDateString();
                        }
                        row.Cells.Add(cell);

                        listTest.Rows.Add(row);
                        cnt++;
                    }
                }
                labelAllTests.Text = string.Format(labelAllTests.Text, cnt.ToString());

            }
            catch (Exception ex)
            {
                Core.Site.RedirectError(Response, ex.Message);
            }

        }

    }
}