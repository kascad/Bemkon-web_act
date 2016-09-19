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
using System.Collections.Generic;
using Shared;

namespace ProfessorTesting
{
    public partial class Top10TestsPanel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateTop10Tests();
        }

        private void CreateTop10Tests()
        {
            try
            {
                Testing.TestingDataContext dc = new Testing.TestingDataContext(GlobalOptions.RemoteConnectionString);
                int k = 0;
                IList<Testing.Test> list = new List<Testing.Test>();
                foreach (var test in dc.Tests.Where(t1 => t1.TestingCount > 0).OrderByDescending(t2 => t2.TestingCount))
                {
                    if (k++ < 10)
                        list.Add(test);
                    else
                        break;
                }
                listViewTop10Tests.DataSource = list;
                listViewTop10Tests.DataBind();
            }
            catch (Exception ex)
            {
                Core.Site.RedirectError(Response, ex.Message);
            }
        }

    }
}