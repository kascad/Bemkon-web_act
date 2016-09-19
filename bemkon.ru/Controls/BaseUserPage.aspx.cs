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
using System.IO;

namespace ProfessorTesting
{
    public partial class BaseUserPage : BasePage
    {
        protected new void Page_Load()
        {
            Response.Redirect("~/Default.aspx");
        }

        protected virtual void OpenArchive(Label labelArchName, Label labelExamName)
        {
            if (Core.Site.CurrUserInfo.LastArchive == null || Core.Site.CurrUserInfo.LastArchive == "")
            {
                Core.Site.CurrUserInfo.LastArchive = Core.FileEx.GetFullPathFSArchivesCurUser();
                string path = Core.FileEx.GetPathArchivesCurUser();
                if (path != "" && !Directory.Exists(path))
                {
                    Core.FileEx.CreateDirArchives(Core.Site.CurrUserInfo.Id);
                }

                if (Core.Site.CurrUserInfo.LastArchive != "" && File.Exists(Core.Site.CurrUserInfo.LastArchive))
                {
                    try
                    {
                        Core.Site.CurrUserInfo.Archive = new Archive.Archive(Core.Site.CurrUserInfo.LastArchive);
                    }
                    catch (Exception err)
                    {
                        Core.Site.RedirectError(Response, err.Message);
                        //return;
                    }
                }
            }
        
            AfterOpenArchive(labelArchName);
            InitSelectExaminee(labelExamName);
        }

        protected virtual void InitSelectExaminee(Label labelExamName)
        {
            int exmId = Core.Site.CurrUserInfo.LastIdExaminee;
            SetSelectExaminee(exmId, labelExamName);
        }
        protected virtual void SetSelectExaminee(int exmId, Label labelExamName)
        {
            if (Core.Site.CurrUserInfo.Priv != UserPrivGroup.TestUser)
            {
                if (Core.Site.CurrUserInfo.Archive != null && exmId != 0)
                    Core.Site.CurrUserInfo.CurrExaminee = Core.Site.CurrUserInfo.Archive.getExaminee(exmId);
                else
                    Core.Site.CurrUserInfo.CurrExaminee = null;
            }
            AfterSelectExaminee(labelExamName);
        }
        protected virtual void SelectExaminee(int exmId, Label labelExamName)
        {
            if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.TestUser)
                return; 

            SetSelectExaminee(exmId, labelExamName);
            using (UserManager userManager = new UserManager())
            {
                userManager.EditLastIdExaminee(exmId, Core.Site.CurrUserInfo.Id);
            }
            Core.Site.CurrUserInfo.LastIdExaminee = exmId;
        }
        protected virtual void AfterSelectExaminee(Label labelExamName)
        {
            if (labelExamName == null)
                return;

            Archive.Examinee exam = Core.Site.CurrUserInfo.CurrExaminee;
            if (exam != null)
                labelExamName.Text = "Текущий обследуемый:&nbsp;<strong>" + exam.Name + "</strong>";
            else
                labelExamName.Text = @"<a href='/User/'>Обследуемый не выбран</a>";
        }

        protected virtual void AfterOpenArchive(Label labelArchName)
        {
            if (labelArchName == null)
                return;

            if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.TestUser)
            {
                labelArchName.Text = "";
                return;
            }

            if (Core.Site.CurrUserInfo.Archive != null)
                labelArchName.Text = "Текущий архив:&nbsp;<strong>" + Core.Site.CurrUserInfo.Archive.ArchName + "</strong>";
            else
                labelArchName.Text = @"<a href='/User/Archives.aspx?mod=open'>Нет открытого архива</a>";

        }
    }
}
