using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Controls_MenuLogin : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params.Get("TestUser") != null && Request.Params.Get("TestUser") != "")
        {
            Login1.UserName = Request.Params.Get("TestUser");
            Login1_Authenticate(sender, null);
        }
        else
            Login1.DestinationPageUrl = "~/" + Core.Consts.indexPage;
    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        string login = Login1.UserName;
        string pwd = Login1.Password;
        
        UserPrivGroup userPriv = Core.Site.Authenticate(login, pwd);
        if (userPriv != UserPrivGroup.None)
        {
            FormsAuthentication.RedirectFromLoginPage(login, Login1.RememberMeSet);
            if (userPriv == UserPrivGroup.Administrator)
                Response.Redirect("~/Admin/");
            else if (userPriv == UserPrivGroup.User || userPriv == UserPrivGroup.ProUser || userPriv == UserPrivGroup.InterpetUser)
                Response.Redirect("~/User/");
            else if (userPriv == UserPrivGroup.TestUser)
                Response.Redirect("~/User/TestExaminee.aspx?id_battery=13&from=0");
        }
        else
        {
            Login1.FailureText = "<b>Неправильный логин и/или пароль...</b>";
        }
    }
}
