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
using ProfessorTesting;

public partial class Customize : BaseAdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Проверка авторизован ли пользователь
        if (!AccessPage())
            return;

        Page.Title = "Настройки. " + Core.Site.titleProgram;

        Controls_EditUserPanel ctrl = (Controls_EditUserPanel)Core.FindControlRecursive(Page,
            "EditUserPanel1");
        if (ctrl != null)
            ctrl.EditMode = EditMode.Customize;
    }
}
