using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProfessorTesting
{
    public partial class MailQuestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title= "Обращение с сайта. " + Core.Site.titleProgram;
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            string recipients = ConfigurationManager.AppSettings["MailQuestion"];
            css_MasterPage.SendMail(recipients, Sender.Text.Trim(), "Обращение с сайта bemkon.ru", Message.Text.Trim());
            Sent.Visible = true;
        }
    }
}