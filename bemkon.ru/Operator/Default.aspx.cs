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

public partial class Operator_Default : BaseOperatorPage
{
    private DataTable userInfo;
    private UserInfo curUserInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Проверка авторизован ли пользователь
        if (!AccessPage())
            return;

        Page.Title = "Страница компании. " + Core.Site.titleProgram;

        using (UserManager userManager = new UserManager())
        {
            // Получение информации о текущем пользователе
            userInfo = userManager.GetUserInfo(Page.User.Identity.Name);
            if (userManager.Error.IsError)
            {
                Core.Site.RedirectError(Response, userManager.Error.Message);
                return;
            }

            // Информации о пользователе в базе нет
            if (userInfo == null || userInfo.Rows.Count <= 0)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            curUserInfo = new UserInfo(userInfo.Rows[0]);

            OperatorUsers1.CurUserInfo = curUserInfo;
        }
    }
}
