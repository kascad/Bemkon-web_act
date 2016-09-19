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
using System.Drawing;
using System.IO;

public partial class Controls_AdminUsers : System.Web.UI.UserControl
{
    private UserInfo curUserInfo;
    public UserInfo CurUserInfo
    {
        get { return curUserInfo; }
        set { curUserInfo = value; }
    }
    private int number = 0;

    private static class Inds
    {
        public static int number = 0;
        public static int id = 8;
        public static int ban = 9;
        public static int del = 6;
        public static int btban = 7;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (curUserInfo != null)
            SqlDataSource1.SelectCommand = "SELECT * FROM Users WHERE ID <> '" + curUserInfo.Id + "'";

        GridView1.RowDeleting += new GridViewDeleteEventHandler(GridView1_RowDeleting);
    }

    void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = Core.Converting.ConvertToInt(e.Keys[0]);
        // Delete user folder with archives
        //if (id != 0)
        //{
        //    string path = Core.FileEx.GetPathFSArchivesUser(id);
        //    try
        //    {
        //        string[] files = Directory.GetFiles(path, "*." + Core.FileEx.extArchive);
        //        foreach (string file in files)
        //        {
        //            File.Delete(file);
        //        }
        //        Directory.Delete(Core.FileEx.GetPathFSArchivesUser(id));
        //    }
        //    catch (Exception)
        //    {
                
        //    }
        //}
    }

    void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
        }
    }

    protected void Grid_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
    }
    protected void GridView1_DataBinding(object sender, EventArgs e)
    {
        number = 1;
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((int)DataBinder.Eval(e.Row.DataItem, "Priv") != 4)
            {
                HyperLink linkTesting = (HyperLink) e.Row.FindControl("HyperLink1");
                if (linkTesting != null)
                    linkTesting.Visible = false;
            }
        }

        if (e.Row.RowIndex >= 0 && e.Row.Cells.Count > Inds.number)
        {
            e.Row.Cells[Inds.number].Text = number.ToString();
            number++;

            Color clr;
            bool bban = Core.Converting.ConvertToInt(e.Row.Cells[Inds.ban].Text) == 1;
            clr = (bban) ? Color.FromArgb(147, 147, 147) : Color.Black;

            for (int i = 0; i < e.Row.Cells.Count; i++)
                if (i != Inds.del)
                    e.Row.Cells[i].ForeColor = clr;

            string captionBan = bban ? "Разблокировать" : "Блокировать";
            HyperLink ctr = (HyperLink)e.Row.Cells[Inds.btban].Controls[0];
            if (ctr != null)
                ctr.Text = captionBan;

        }
    }
}
