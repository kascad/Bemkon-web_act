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
using System.ComponentModel;

namespace ProfessorTesting
{
    public partial class UserExaminees : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверка авторизован ли пользователь
            if (Core.Site.CurrUserInfo == null)
                return;

            dataGridViewExaminees.RowCreated += new GridViewRowEventHandler(dataGridViewExaminees_RowCreated);
            dataGridViewExaminees.RowDataBound += new GridViewRowEventHandler(dataGridViewExaminees_RowDataBound);

            if (Core.Site.CurrUserInfo.LastArchive != "")
            {
                OpenArhive();

                UserPrivGroup priv = Core.Site.IsAuth(Page);
                if (priv == UserPrivGroup.InterpetUser)
                {
                    dataGridViewExaminees.Columns[4].Visible = false;
                }
                else
                {
                    dataGridViewExaminees.Columns[4].Visible = true;
                }

                dataGridViewExaminees.SelectedIndexChanged += new EventHandler(dataGridViewExaminees_SelectedIndexChanged);
                if (Core.Site.CurrUserInfo.CurrExaminee != null)
                {
                    if (dataGridViewExaminees.Rows.Count > 0)
                    {
                        dataGridViewExaminees.SelectedIndex = 0;
                        dataGridViewExaminees_SelectedIndexChanged(dataGridViewExaminees, EventArgs.Empty);
                    }
                    //foreach (GridViewRow row in dataGridViewExaminees.Rows)
                    //{
                    //    if (Core.Converting.ConvertToInt(row.Cells[0].Text) != Core.Site.CurrUserInfo.CurrExaminee.Id)
                    //        continue;
                    //    dataGridViewExaminees.SelectedIndex = row.RowIndex;
                    //    break;
                    //}
                }
            }

        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        } 
        void dataGridViewExaminees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            s(sender as Control, e.Row);
        }

        void dataGridViewExaminees_RowCreated(object sender, GridViewRowEventArgs e)
        {
            s(sender as Control, e.Row);
        }
        private void s(Control sender, GridViewRow row)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                row.Attributes.Add("onclick",
                    Page.ClientScript.GetPostBackEventReference(sender, "Select$" + row.RowIndex.ToString()));
                row.Attributes.Add("onmouseout", "this.style.cursor='default';");
                row.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
            }
        }

        void dataGridViewExaminees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridViewExaminees.SelectedIndex < 0 || dataGridViewExaminees.SelectedRow == null)
                return;

            int id = Core.Converting.ConvertToInt(dataGridViewExaminees.SelectedDataKey.Value);
            string name = "";
            try { name = ((HyperLink)dataGridViewExaminees.SelectedRow.Cells[1].Controls[0]).Text; }
            catch { }
            OnSelectedExaminee(id, name);
        }

        public void OpenArhive()
        {
            try
            {
                dataGridViewExaminees.DataSource = Core.Site.CurrUserInfo.ArchiveDataSource ;// Core.Site.CurrUserInfo.Archive;
                dataGridViewExaminees.DataBind();
            }            catch (Exception err)
            {
                Core.Site.RedirectError(Response, err.Message);
                return;
            }

            labelExamCount.Text = "Всего обследуемых: " 
                + (Core.Site.CurrUserInfo.Archive != null ? Core.Site.CurrUserInfo.Archive.ArchExaminees.Count.ToString() : "0");
        }



        public delegate void SelectedExamineeEventHandler(object sender, SelectedExamineeEventArgs e);
        public event SelectedExamineeEventHandler selectedExaminee;
        protected void OnSelectedExaminee(int idExaminee, string NameExaminee)
        {
            if (selectedExaminee != null)
                selectedExaminee(this, new SelectedExamineeEventArgs(idExaminee, NameExaminee));
        }

        protected void dataGridViewExaminees_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

    }


}