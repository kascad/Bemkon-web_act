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

public partial class Controls_MenuUser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserPrivGroup priv = Core.Site.IsAuth(Page);
        if (priv == UserPrivGroup.InterpetUser)
        {
            TestGroup.Visible = false;
        }
        else
        {
            TestGroup.Visible = true;
        }
    }
}
