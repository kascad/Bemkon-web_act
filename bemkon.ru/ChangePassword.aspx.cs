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

public partial class Admin_ChangePassword : BaseAdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Проверка авторизован ли пользователь
        if (!AccessPage())
            return;

        Page.Title = "Смена пароля пользователя. " + Core.Site.titleProgram;

        string sid = HttpContext.Current.Request.QueryString["url"];
        if (sid == null || sid == "")
        {
            idcustom.Text = "Пользователи";
            if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.Administrator)
                idcustom.NavigateUrl = "~/Admin/";
            else if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.User)
                idcustom.NavigateUrl = "~/User/";
        }
        else
        {
            idcustom.Text = "Настройки";
            if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.Administrator)
                idcustom.NavigateUrl = "~/Admin/Customize.aspx";
            else if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.User)
                idcustom.NavigateUrl = "~/User/Customize.aspx";
        }
    }
    protected override bool AccessPage()
    {
        // Проверка авторизован ли пользователь
        UserPrivGroup priv = Core.Site.IsAuth(Page);
        if (priv == UserPrivGroup.None)
            return false;
        return true;
    }

}
