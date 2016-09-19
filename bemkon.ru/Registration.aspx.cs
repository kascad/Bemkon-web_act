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

public partial class RegisterUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Регистрация пользователя. " + Core.Site.titleProgram;

        Controls_EditUserPanel ctrl = (Controls_EditUserPanel)Core.FindControlRecursive(Page, "EditUserPanel1");
        if (ctrl != null)
            ctrl.EditMode = EditMode.Add;
    }


}
