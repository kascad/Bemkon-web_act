using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Drawing;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;

namespace ProfessorTesting.Controls
{
    public partial class EditExamineePanel : System.Web.UI.UserControl
    {
        private int exmID;
        private Archive.Examinee exmam;

        protected void Page_Load(object sender, EventArgs e)
        {
            labelError.Visible = false;
            string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
            exmID = Core.Converting.ConvertToInt(sid);

            if (exmID > 0 && Core.Site.CurrUserInfo.Archive != null)
            {
                exmam = Core.Site.CurrUserInfo.Archive.getExaminee(exmID);
                SetProperties();
                LabelTitle.Text = "Редактирование обследуемого: " + exmam.Name.Trim();
            }
            else
            {
                textBoxID.Visible = false;
                labelID.Visible = false;
                LabelTitle.Text = "Создание нового обследуемого";
            }
            buttonEditSave.Click += new EventHandler(buttonEditSave_Click);
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
        }

        void buttonEditSave_Click(object sender, EventArgs e)
        {
            labelError.Visible = false;
            bool result = exmID > 0 && exmam != null ? 
                exmam.SaveExaminee(textBoxName.Text, textBoxComments.Text) :
                Core.Site.CurrUserInfo.Archive.AddExaminee(textBoxName.Text, textBoxComments.Text);

            if (!result)
            {
                labelError.Visible = true;
                labelError.Text = "Обследуемый с таким именем уже существует!";
            }
            else
                Response.Redirect("~/User/");
        }

        private void SetProperties()
        {
            if (!Page.IsPostBack)
            {
                textBoxComments.Text = exmam.Description;
                textBoxID.Text = exmam.Id.ToString();
                textBoxName.Text = exmam.Name;
            }

            foreach (var et in exmam.ExamineeTests)
            {
                HyperLink hyp = new HyperLink();

                hyp.CssClass = "item_test";
                if (!et.IsFinished)
                {
                    hyp.Text = et.Name.Trim() + "(!) ";
                    hyp.ForeColor = Color.Red;
                }
                else
                {
                    hyp.Text = et.Name.Trim() + " ";
                }
                hyp.NavigateUrl = string.Format("~/User/ViewTest.aspx?id={0}&id_test={1}", exmID, et.TestId);

                listViewTests.Controls.Add(hyp);
            }
        }

        protected void CreateLinkButton_Click(object sender, EventArgs e)
        {
            if (exmID <= 0 || exmam == null) // not exists
                return;
            
            using (UserManager userManager = new UserManager())
            {
                int startBattery = 0;
                Int32.TryParse(BatteryList.SelectedValue, out startBattery);

                string link = userManager.CreateTestingUser(Core.Site.CurrUserInfo.Id, Core.Site.CurrUserInfo.LastArchive, exmID, startBattery, textBoxName.Text, "", 0);

                if (link.Length > 0)
                    TestingLink.Text = link;
            }
        }

    }
}