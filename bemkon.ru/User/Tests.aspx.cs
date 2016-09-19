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
    public partial class Tests : BaseUserPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверка авторизован ли пользователь
            if (!AccessPage())
                return;

            OpenArchive(labelArchName, labelExamName);

            Page.Title = "Отдельные тесты. " + Core.Site.titleProgram;

        }
    }
}
