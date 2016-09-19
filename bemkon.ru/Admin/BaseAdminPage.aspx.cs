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
    public partial class BaseAdminPage : BasePage
    {
        protected new void Page_Load()
        {
            Response.Redirect("~/Default.aspx");
        }

        protected override bool AccessPage()
        {
            UserPrivGroup priv = Core.Site.IsAuth(Page);
            if (priv == UserPrivGroup.None)
                return false;
            if (priv != UserPrivGroup.Administrator)
            {
                Response.Redirect("~/Default.aspx");
                return false;
            }
            return true;
        }
    }
}
