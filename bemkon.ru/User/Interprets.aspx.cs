using System;
using System.Configuration;
using System.Web;
using System.Web.UI.HtmlControls;

namespace ProfessorTesting.User
{
    public partial class Interprets : BaseUserPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверка авторизован ли пользователь
            if (!AccessPage())
                return;
           
            OpenArchive(labelArchName, labelExamName);

            Page.Title = "Интерпретация. " + Core.Site.titleProgram;

            string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
            int exmID = Core.Converting.ConvertToInt(sid);
            if (exmID != 0)
                SelectExaminee(exmID, labelExamName);

            Response.Redirect("InterpretExaminee.aspx");

            //CreateInterpretsList();
        }

        private void CreateInterpretsList()
        {
            int cnt = 0;
            try
            {
                Testing.TestingDataContext dc = new Testing.TestingDataContext(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);//Testing.GlobalOptions.ConnectionStr);
                HtmlTableCell cell = null;
                HtmlTableRow row = null;

                foreach (var t in dc.Interprets)
                {
                    row = new HtmlTableRow();
                    cell = new HtmlTableCell();
                    cell.InnerHtml = "<a href='/User/InterpretExaminee.aspx?id="
                        + t.InterpretID + "'>" + t.InterpretName + "</a>";
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerHtml = "<span class='test_nogroup_item'>" + t.InterpretShortName + "</span>";
                    row.Cells.Add(cell);

                    listInterpret.Rows.Add(row);
                    cnt++;
                }
                labelAllInterpret.Text = "Всего интерпретаций: " + cnt.ToString();

            }
            catch (Exception ex)
            {
                Core.Site.RedirectError(Response, ex.Message);
            }

        }

    }
}
