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
using System.Collections.Generic;

namespace ProfessorTesting
{
    public partial class ArchivePanel : System.Web.UI.UserControl
    {
        ArchiveModType mod;

        protected void Page_Load(object sender, EventArgs e)
        {
            string smod = HttpContext.Current.Request.QueryString[Core.Consts.reqMod];

            int idUser = Core.Site.CurrUserInfo.Id;

            if (smod == null || smod == "" || smod == "open")
                mod = ArchiveModType.Open;
            else if (smod == "new")
                mod = ArchiveModType.New;
            else if (smod == "upload")
                mod = ArchiveModType.Upload;
            else if (smod == "download")
                mod = ArchiveModType.Download;
            else
                mod = ArchiveModType.Open;

            string[] files;
            IList<FileArchive> listFiles;
            switch (mod)
            {
                case ArchiveModType.New:
                    LabelTitle.Text = "Создание нового архива";
                    panelCreate.Visible = true;
                    Core.FileEx.CreateDirArchivesCurUser();
                    LinkButtonNew.Click += new EventHandler(LinkButtonNew_Click);
                    break;

                case ArchiveModType.Open:
                    LabelTitle.Text = "Открытие архива";

                    Core.FileEx.CreateDirArchivesCurUser();
                    files = Directory.GetFiles(Core.FileEx.GetPathFSArchivesCurUser(), "*." + Core.FileEx.extArchive);
                    listFiles = new List<FileArchive>();
                    foreach (string file in files)
                    {
                        listFiles.Add(new FileArchive(Path.GetFileName(file)));
                    }
                    dataGridView.RowCreated += new GridViewRowEventHandler(dataGridView_RowCreated);
                    dataGridView.RowDataBound += new GridViewRowEventHandler(dataGridView_RowDataBound);                    
                    dataGridView.DataSource = listFiles;
                    dataGridView.DataBind();
                    LinkButtonOpen.Click += new EventHandler(LinkButtonOpen_Click);


                    panelOpen.Visible = true;
                    break;

                case ArchiveModType.Upload:
                    LabelTitle.Text = "Загрузка архива";
                    panelDownload.Visible = true;
                    Core.FileEx.CreateDirArchivesCurUser();
                    LinkButtonUp.Click += new EventHandler(LinkButtonUp_Click);
                    break;

                case ArchiveModType.Download:
                    LabelTitle.Text = "Выгрузка архива";
                    panelUpload.Visible = true;
                    files = Directory.GetFiles(Core.FileEx.GetPathFSArchivesCurUser(), 
                        "*." + Core.FileEx.extArchive);
                    listFiles = new List<FileArchive>();
                    foreach (string file in files)
                    {
                        listFiles.Add(new FileArchive(Path.GetFileName(file), idUser));
                    }
                    
                    GridViewDownload.DataSource = listFiles;
                    GridViewDownload.DataBind();
                    break;
            }
        }

        void GridViewDownload_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        void GridViewDownload_RowCreated(object sender, GridViewRowEventArgs e)
        {
        }
        void dataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            s(sender as Control, e.Row);
        }

        void dataGridView_RowCreated(object sender, GridViewRowEventArgs e)
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

        void LinkButtonOpen_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRow == null)
                return;
            string fileName = dataGridView.SelectedRow.Cells[0].Text;
            string path = Core.FileEx.GetPathFSArchivesCurUser() + Core.FileEx.separator + fileName;
            ChangeArchive(path, Path.GetFileNameWithoutExtension(fileName));
        }

        void RemoveSession()
        {
            Session.Remove("professorTest");
        }

        void LinkButtonUp_Click(object sender, EventArgs e)
        {
            if (FileUpload1.FileName == null || FileUpload1.FileName == "" || !FileUpload1.HasFile)
            {
                Core.Site.RedirectError(Response, "Файл для загрузки не выбран");
                return;
            }

            try
            {
                string servPath = Core.FileEx.GetPathFSArchivesCurUser() + Core.FileEx.separator
                    + Path.GetFileName(FileUpload1.FileName);
                FileUpload1.PostedFile.SaveAs(servPath);

                ChangeArchive(servPath, Path.GetFileNameWithoutExtension(FileUpload1.FileName));
            }
            catch (Exception err)
            {
                Core.Site.RedirectError(Response, err.Message);
                return;
            }
        }

        void LinkButtonNew_Click(object sender, EventArgs e)
        {
            try
            {
                string archFileName = Core.FileEx.GetPathFSArchivesCurUser()
                    + Core.FileEx.separator + TextBoxNameNew.Text + "." + Core.FileEx.extArchive;

                Core.FileEx.CreateDirArchivesCurUser();
                ChangeArchive(archFileName, TextBoxNameNew.Text);
            }
            catch (Exception err)
            {
                Core.Site.RedirectError(Response, err.Message);
                return;
            }
        }

        private void ChangeArchive(string path, string fileName)
        {
            Core.Site.CurrUserInfo.Archive = new Archive.Archive(path);
            Core.Site.NewArchive(fileName);
            RemoveSession();

            Response.Redirect("~/User/");
        }

        protected void Download_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ArchiveDownload")
            {
                string filepath = Core.FileEx.GetPathFSArchivesCurUser() + Core.FileEx.separator + e.CommandArgument;
                Core.FileEx.DownloadFile(Response, filepath);
            }
        }
    }

    public class FileArchive
    {
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public FileArchive(string fileName)
            : this(fileName, 0)
        {
        }
        public FileArchive(string fileName, int id)
        {
            this.fileName = fileName;
            this.id = id;
        }
    }
}