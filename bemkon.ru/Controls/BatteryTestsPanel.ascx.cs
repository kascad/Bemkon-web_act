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
using Shared;

namespace ProfessorTesting
{
    public partial class BatteryTestsPanel : System.Web.UI.UserControl
    {
        private Testing.TestingDataContext dc;

        protected void Page_Load(object sender, EventArgs e)
        {
            dc = new Testing.TestingDataContext(GlobalOptions.RemoteConnectionString);
            FillData();           
        }
        private void FillData()
        {
            try
            {
                dataGridViewBattaries.DataSource = dc.BatteriesViews;
                dataGridViewBattaries.DataBind();
                labelBatteryCount.Text = "Всего батарей тестов: " + dc.BatteriesViews.Count().ToString();
            }
            catch (Exception)
            {
            }
        }
    }
}