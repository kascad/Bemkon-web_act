using System;
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

public partial class _Default : System.Web.UI.Page 
{
    private DataTable userInfo;
    private UserInfo curUserInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Core.Site.titleProgram;
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        // Проверка авторизован ли пользователь
        if (Page.User == null || Page.User.Identity == null || Page.User.Identity.Name == "")
            return;

        using (UserManager userManager = new UserManager())
        {
            // Получение информации о текущем пользователе
            userInfo = userManager.GetUserInfo(Page.User.Identity.Name);
            if (userManager.Error.IsError)
            {
                Core.Site.RedirectError(Response, userManager.Error.Message);
                return;
            }

            curUserInfo = new UserInfo(userInfo.Rows[0]);

            // Подключение меню в зависимости от привилегий пользователя
            if (curUserInfo.Priv == UserPrivGroup.Administrator)
                Response.Redirect("~/Admin/");
            else if (curUserInfo.Priv == UserPrivGroup.Operator)
                Response.Redirect("~/Operator/");
            else if (curUserInfo.Priv == UserPrivGroup.User || curUserInfo.Priv == UserPrivGroup.InterpetUser || curUserInfo.Priv == UserPrivGroup.ProUser)
                Response.Redirect("~/User/");
        }

    }
}
