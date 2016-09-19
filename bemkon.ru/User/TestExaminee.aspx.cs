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
    public partial class TestExaminee : BaseUserPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверка авторизован ли пользователь
            if (!AccessPage())
                return;

            OpenArchive(labelArchName, labelExamName);

            Page.Title = "Тестирование обследуемого. " + Core.Site.titleProgram;

            string sfrom = HttpContext.Current.Request.QueryString[Core.Consts.reqFrom];
            FromType from = Core.Converting.ConvertToFromType(sfrom);
            switch (from)
            {
                case FromType.Examinee:
                    idcustom.Text = "Обследуемые";
                    idcustom.NavigateUrl = "~/User/";
                    break;
                case FromType.Battery:
                    idcustom.Text = "Батареи тестов";
                    idcustom.NavigateUrl = "~/User/BatteryTests.aspx";
                    break;
                case FromType.Test:
                    idcustom.Text = "Отдельные тесты";
                    idcustom.NavigateUrl = "~/User/Tests.aspx";
                    break;
                case FromType.TopTest:
                    idcustom.Text = "Топ 10 тестов";
                    idcustom.NavigateUrl = "~/User/TopTests.aspx";
                    break;
            }
        }
    }
}
