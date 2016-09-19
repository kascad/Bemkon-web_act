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
    public partial class DeleteExamineePanel : System.Web.UI.UserControl
    {
        private int exmID;
        private Archive.Examinee exmam;

        protected void Page_Load(object sender, EventArgs e)
        {
            exmID = Core.Site.CurrUserInfo.CurrExaminee.Id;

            if (exmID > 0 && Core.Site.CurrUserInfo.Archive != null)
            {
                exmam = Core.Site.CurrUserInfo.Archive.getExaminee(exmID);
                LabelTitle.Text = "Удаление обследуемого: " + exmam.Name.Trim();
                confirm.Text = string.Format("Удалить обследуемого № {0} \"{1}\" и все его тесты?",
                    exmID, exmam.Name.Trim());
            }
            buttonDeleteSave.Click += new EventHandler(buttonDeleteSave_Click);
        }

        void buttonDeleteSave_Click(object sender, EventArgs e)
        {
            Archive.Examinee exam = Core.Site.CurrUserInfo.Archive.getExaminee(exmID);
            if (exam != null)
            {
                exam.DeleteExaminee();
                Response.Redirect("~/User/");
            }
        }
    }
}