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
    public partial class ViewTest : BaseUserPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверка авторизован ли пользователь
            if (!AccessPage())
                return;

            OpenArchive(labelArchName, labelExamName);

            Page.Title = "Тест. " + Core.Site.titleProgram;
            ExportExcel.Click += new EventHandler(ExportExcel_Click);
        }

        void ExportExcel_Click(object sender, EventArgs e)
        {
            ViewTestExamineePanel1.ExportExcel();
        }
    }
}
