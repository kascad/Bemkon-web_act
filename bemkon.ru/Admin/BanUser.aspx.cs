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

public partial class Ban : BaseAdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Проверка авторизован ли пользователь
        if (!AccessPage())
            return;

        using (UserManager userManager = new UserManager())
        {
            string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
            int id = Core.Converting.ConvertToInt(sid);

            if (id != 0)
            {

                userManager.BanUser(id);
                if (userManager.Error.IsError)
                {
                    Core.Site.RedirectError(Response, userManager.Error.Message);
                    return;
                }
                Response.Redirect("~/Admin/");
                return;
            }
            else
            {
                Core.Site.RedirectError(Response, "Неправильный ИД пользователя");
                return;
            }
        }
    }
}
