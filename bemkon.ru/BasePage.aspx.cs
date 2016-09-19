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
    public partial class BasePage : System.Web.UI.Page
    {
        protected void Page_Load()
        {
            Response.Redirect("~/Default.aspx");
        }

        protected virtual bool AccessPage()
        {
            // Проверка авторизован ли пользователь
            UserPrivGroup priv = Core.Site.IsAuth(Page);
            if (priv == UserPrivGroup.None)
                return false;

            if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.TestUser)
            {
                if (Master != null)
                {
                    ContentPlaceHolder tb = (ContentPlaceHolder) Master.FindControl("hormenumodule");
                    if (tb != null)
                        tb.Visible = false;
                    tb = (ContentPlaceHolder) Master.FindControl("hormenu");
                    if (tb != null)
                        tb.Visible = false;
                    tb = (ContentPlaceHolder) Master.FindControl("leftmenu");
                    if (tb != null)
                        tb.Visible = false;
                }
            }

            return true;
        }
    }
}
