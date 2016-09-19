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
using ProfessorTesting;

public partial class User_Default : BaseUserPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Проверка авторизован ли пользователь
        if (!AccessPage())
            return;

        Page.Title = "Обследуемые. " + Core.Site.titleProgram;

        if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.TestUser)
        {
            userExaminees.Visible = false;
            LabelTitle.Visible = true;
            LabelTitle.Text = "Вы уже прошли тестирование. Благодарим за работу!";
        }
        else if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.User || Core.Site.CurrUserInfo.Priv == UserPrivGroup.InterpetUser)
        {
            ExamineeMenu1.Visible = false;
        }
        else
        {
            LabelTitle.Visible = false;
        }

        OpenArchive(labelArchName, labelExamName);

        userExaminees.selectedExaminee += new UserExaminees.SelectedExamineeEventHandler(userExaminees_selectedExaminee);
    }

    void userExaminees_selectedExaminee(object sender, SelectedExamineeEventArgs e)
    {
        SelectExaminee(e.IdExaminee, labelExamName);
    }

    protected override void AfterOpenArchive(Label labelArchName)
    {
        base.AfterOpenArchive(labelArchName);

        Control ctrl = Core.FindControlRecursive(ExamineeMenu1, "panelMenuUser");
        if (ctrl != null)
            ctrl.Visible = Core.Site.CurrUserInfo.Archive != null;
    }
}
