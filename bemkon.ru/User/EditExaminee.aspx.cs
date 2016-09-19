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
    public partial class EditExaminee : BaseUserPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверка авторизован ли пользователь
            if (!AccessPage())
                return;

            OpenArchive(labelArchName, labelExamName);

            Page.Title = "Редактирование обследуемого. " + Core.Site.titleProgram;

            string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
            int exmID = Core.Converting.ConvertToInt(sid);
            SelectExaminee(exmID, labelExamName);
        }
    }
}
