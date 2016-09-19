using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Net.Mail;

public partial class css_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    //private void AddDynamicCss()
    //{
    //    HtmlLink cssLink = new HtmlLink();
    //    cssLink.Href = "~/Css/General.css";
    //    cssLink.Attributes.Add("rel", "stylesheet");
    //    cssLink.Attributes.Add("type", "text/css");
    //    Header.Controls.Add(cssLink);
    //}

    public static string DBImageRoot()
    {
        return "/Images/db/";
    }

    public static void SendMail(string recipients, string sender, string subject, string body)
    {
        string smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
        int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
        SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);

        string userName = ConfigurationManager.AppSettings["SMTPUser"];
        string password = ConfigurationManager.AppSettings["SMTPPassword"];
        smtp.Credentials = new System.Net.NetworkCredential(userName, password);

        MailMessage msg = new MailMessage(sender, recipients, subject, body);
        msg.IsBodyHtml = true;

        smtp.Send(msg);
    }
}
