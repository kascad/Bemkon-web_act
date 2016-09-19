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

public partial class Controls_ChangePasswordPanel : System.Web.UI.UserControl
{
    DataRow curUserInfo;
    int id;
    string url;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
        url = HttpContext.Current.Request.QueryString["url"];
        string http = "";
        if (url != "" && url != null)
            http = "~/Admin/" + url + ".aspx";
        else
            http = "~/Admin/EditUser.aspx?" + Core.Consts.reqId + "=" + sid;
        HyperLinkCancel.NavigateUrl = http;

        id = Core.Converting.ConvertToInt(sid);
        using (UserManager userManager = new UserManager())
        {
            if (id != 0)
            {
                DataTable userInfo = userManager.GetUserInfo(id);
                if (userManager.Error.IsError)
                {
                    Core.Site.RedirectError(Response, userManager.Error.Message);
                    return;
                }
                if (userInfo == null || userInfo.Rows.Count <= 0)
                {
                    Core.Site.RedirectError(Response, "Такого пользователя нет в базе данных");
                    return;
                }

                curUserInfo = userInfo.Rows[0];
            }
        }
    }

    protected void LinkUpdate_Click(object sender, EventArgs e)
    {
        if (curUserInfo != null && PasswordTextBox.Text != curUserInfo[NameDB.Users.Password].ToString())
        {
            ValidationSummary1.Visible = false;
            ValidPass.InnerHtml = "Пароль не изменен<ul><li>Неправильный текущий пароль</li></ul>";
            ValidPass.Visible = true;
            return;
        }

        using (UserManager userManager = new UserManager())
        {
            userManager.ChangePassword(id, NewPasswordTextBox.Text);
            if (userManager.Error.IsError)
            {
                Core.Site.RedirectError(Response, userManager.Error.Message);
                return;
            }
        }
        string http = "";
        if (url != "" && url != null)
            http = "~/Admin/" + url + ".aspx";
        else
            http = "~/Admin/EditUser.aspx?" + Core.Consts.reqId + "=" + id;
        Response.Redirect(http);
    }
}
