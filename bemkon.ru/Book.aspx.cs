using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProfessorTesting
{
    public partial class BookPage : System.Web.UI.Page
    {
        private int quantityInCart;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Научные работы и статьи. " + Core.Site.titleProgram;
            MaintainScrollPositionOnPostBack = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                sda.SelectCommand = new SqlCommand();
                sda.SelectCommand.CommandText = 
                    "select b.*, isnull(x.Quantity, 0) InCart " +
                        "from dbo.Books b " +
                            "left join (" +
                                "select BookId, sum(Quantity) Quantity " +
                                    "from dbo.Cart " +
                                    "where SessionId = @sessionId " +
                                    "group by BookId" +
                                ") x on b.BookId = x.BookId " +
                        "order by b.SortOrder, b.BookName";
                sda.SelectCommand.Parameters.AddWithValue("sessionId", Session["sessionId"]);
                sda.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
                DataTable tbl = new DataTable();
                sda.Fill(tbl);
                Books.DataSource = tbl;
                quantityInCart = 0;
                Books.DataBind();
                Session["quantityInCart"] = quantityInCart;
            }
            ((Default)Master).ShowCartInfo();
        }

        protected void Books_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;

                Image img = (Image)e.Item.FindControl("BookImage");
                string imageFile = row["BookImageFile"].ToString();
                if (!string.IsNullOrEmpty(imageFile))
                {
                    img.ImageUrl = string.Format("{0}Books/{1}", css_MasterPage.DBImageRoot(), imageFile);
                }
                Panel p = (Panel)e.Item.FindControl("PublisherPanel");
                p.Visible = row["Publisher"].ToString() != "";
                p = (Panel)e.Item.FindControl("FormatPanel");
                p.Visible = row["Format"].ToString() != "";
                p = (Panel)e.Item.FindControl("ISBNPanel");
                p.Visible = row["ISBN"].ToString() != "";
                p = (Panel)e.Item.FindControl("PagesPanel");
                p.Visible = !row["Pages"].Equals(DBNull.Value);
                HyperLink hl = (HyperLink)e.Item.FindControl("Fragment");
                string fragment = row["Fragment"].ToString();
                if (string.IsNullOrEmpty(fragment))
                {
                    hl.Visible = false;
                }
                else
                {
                    hl.Visible = true;
                    hl.NavigateUrl = string.Format("/Fragments/{0}", fragment);
                }

                string pdf = row["PDFForDownload"].ToString();
                if (string.IsNullOrEmpty(pdf))
                {
                    p = (Panel)e.Item.FindControl("ForDownloadPanel");
                    p.Visible = false;
                }
                else
                {
                    hl = (HyperLink)e.Item.FindControl("ForDownload");
                    hl.NavigateUrl = string.Format("/Fragments/{0}", pdf);
                }

                Literal l;
                string cm = row["Comment"].ToString();
                if (cm != "")
                {
                    l = (Literal)e.Item.FindControl("Comment");
                    l.Text = "*" + cm;
                }

                bool forSale = Convert.ToBoolean(row["ForSale"]);
                int inCart = Convert.ToInt32(row["InCart"]);
                quantityInCart += inCart;
                l = (Literal)e.Item.FindControl("ForSale");
                l.Visible = forSale && inCart == 0;
                img = (Image)e.Item.FindControl("InCart");
                img.Visible = forSale && inCart != 0;

                if (forSale)
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = new SqlCommand();
                        sda.SelectCommand.CommandText =
                            "select brf.BookId, brf.ReleaseFormatId, brf.Price, rf.ReleaseFormatName, b.Currency " +
                                "from dbo.BooksReleaseFormats brf " +
                                    "inner join dbo.ReleaseFormats rf on brf.ReleaseFormatId = rf.ReleaseFormatId " +
                                    "inner join dbo.Books b on brf.BookId = b.BookId " +
                                "where b.BookId = @bookId " +
                                "order by rf.SortOrder, rf.ReleaseFormatName";
                        sda.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
                        sda.SelectCommand.Parameters.Add("bookId", SqlDbType.Int).Value = row["BookId"];

                        sda.SelectCommand.Connection.Open();
                        try
                        {
                            DataTable tbl = new DataTable();
                            sda.Fill(tbl);

                            Repeater r = (Repeater)e.Item.FindControl("Formats");
                            r.DataSource = tbl;
                            r.DataBind();
                        }
                        finally
                        {
                            sda.SelectCommand.Connection.Close();
                        }
                    }
                }
            }
        }

        protected void Formats_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;

                if (!row["ReleaseFormatId"].Equals(1))
                {
                    Literal l = (Literal)e.Item.FindControl("Currency");
                    l.Text = l.Text.Replace("*", "");
                }
            }
        }

        protected void BuyButton_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName=="Buy")
            {
                string[] args = e.CommandArgument.ToString().Split(';');
                int bookId = Convert.ToInt32(args[0]);
                int releaseFormatId = Convert.ToInt32(args[1]);
                BuyBook(bookId, releaseFormatId);
            }
        }

        private void BuyBook(int bookId, int releaseFormatId)
        {
            using (SqlCommand cmd = new SqlCommand("AddToCart", new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString)))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("sessionId", Session["sessionId"]);
                cmd.Parameters.AddWithValue("bookId", bookId);
                cmd.Parameters.AddWithValue("releaseFormatId", releaseFormatId);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }
    }
}