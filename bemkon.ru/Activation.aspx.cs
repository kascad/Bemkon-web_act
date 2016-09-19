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

public partial class Activation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Активация пользователя. " + Core.Site.titleProgram;

        string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
        int userID = Core.Converting.ConvertToInt(sid);
        string aid = HttpContext.Current.Request.QueryString[Core.Consts.reqAId];

        Controls_ActivationPanel ctrl = (Controls_ActivationPanel)Core.FindControlRecursive(Page, "ActivationPanel1");
        if (ctrl != null)
        {
            ctrl.UserID = userID;
            ctrl.AID = aid;
        }
    }
}
