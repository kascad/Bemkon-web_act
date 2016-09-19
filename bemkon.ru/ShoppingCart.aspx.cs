using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProfessorTesting
{
    public partial class ShoppingCartPage : System.Web.UI.Page
    {
        private const string MAC_ORDER_NUMBER = "#ORDER_NUMBER#";
        private const string MAC_BUYER = "#BUYER#";
        private const string MAC_PHONE_NUMBER = "#PHONE_NUMBER#";
        private const string MAC_BUYER_EMAIL = "#EMAIL#";
        private const string MAC_ORDER_LIST = "#ORDER_LIST#";
        private const string MAC_ORDER_SUM = "#ORDER_SUM#";
        private const string MAC_DELIVERY_POINT = "#DELIVERY_POINT#";
        private const string MAC_DELIVERY_COST = "#DELIVERY_COST#";
        private const string MAC_TOTAL_SUM = "#TOTAL_SUM#";
        private const string MAC_PAYMENT_METHOD = "#PAYMENT_METHOD#";
        private const string MAC_SALE_PHONE = "#SALE_PHONE#";

        private int total = 0;
        private int cntBooks = 0;
        private bool paperExists = false;
        private int orderNum;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Корзина покупок. " + Core.Site.titleProgram;
            GoToOrder.Attributes["onclick"] = "$('#cart').hide();$('#order').show();";
            Pickup.Attributes["onclick"] = "$('#totalSum').text($('#orderSum').text());";
            BySubway.Attributes["onclick"] =
                "var sum = parseInt($('#orderSum').text()) + 150;$('#totalSum').text(sum.toString());";
            GoToClient.Attributes["onclick"] = "$('#order').hide();$('#client').show();" +
                ClientNameRequired.ClientID + ".enabled=\"True\";" +
                ClientMailRequired.ClientID + ".enabled=\"True\";" +
                ClientMailValid.ClientID + ".enabled=\"True\";" +
                ClientPhoneRequired.ClientID + ".enabled=\"True\";" +
                ClientPhoneValid.ClientID + ".enabled=\"True\";";
            BackToOrder.Attributes["onclick"] = "$('#client').hide();$('#order').show();" +
                ClientNameRequired.ClientID + ".enabled=\"False\";" +
                ClientMailRequired.ClientID + ".enabled=\"False\";" +
                ClientMailValid.ClientID + ".enabled=\"False\";" +
                ClientPhoneRequired.ClientID + ".enabled=\"False\";" +
                ClientPhoneValid.ClientID + ".enabled=\"False\";";
        }

        protected void Cart_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            RecalcQuantity();
        }

        protected void Cart_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            RecalcQuantity();
        }

        protected void Cart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) { return; }

            DataRowView row = (DataRowView)e.Row.DataItem;
            if (row["ReleaseFormatId"].Equals(1))
            {
                paperExists = true;
            }
            total += Convert.ToInt32(row["Cost"]);
            cntBooks++;
        }

        protected void Cart_DataBound(object sender, EventArgs e)
        {
            SetTotalSum(total);
            if (!paperExists)
            {
                Pickup.Checked = false;
                BySubway.Checked = false;
                Card.Checked = true;
            }
            Pickup.Enabled = paperExists;
            BySubway.Enabled = paperExists;
            Cash.Enabled = paperExists;
            Card.Enabled = paperExists;
            GoToOrder.Visible = cntBooks > 0;
        }

        protected void Complete_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            GetOrderNumber();
            SendOrder();
            ClearCart();
            Processed.Visible = false;
            Completed.Visible = true;
            OrderNumber.Text = orderNum.ToString();
            ((Default)Master).ShowCartInfo();
        }

        private void RecalcQuantity()
        {
            using (SqlCommand cmd = new SqlCommand(
                "select sum(Quantity) Quantity from dbo.Cart where SessionId = @sessionId",
                new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString)))
            {
                cmd.Parameters.AddWithValue("sessionId", Session["sessionId"]);
                cmd.Connection.Open();
                try
                {
                    object q = cmd.ExecuteScalar();
                    if (q.Equals(DBNull.Value))
                    {
                        Session["quantityInCart"] = 0;
                    }
                    else
                    {
                        Session["quantityInCart"] = Convert.ToInt32(q);
                    }
                    ((Default)Master).ShowCartInfo();
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }

        private void SetTotalSum(int sum)
        {
            TotalCart.Text = sum.ToString();
            OrderSum.Text = sum.ToString();
            TotalSum.Text = ((paperExists && BySubway.Checked) ? sum + 150 : sum).ToString();
        }

        private void GetOrderNumber()
        {
            orderNum = 0;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "dbo.NewOrderNumber";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("newNumber", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);

                cmd.Connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    orderNum = Convert.ToInt32(cmd.Parameters["newNumber"].Value);
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }

        private void SendOrder()
        {
            string tmpFile = Request.PhysicalApplicationPath + "OrderTemplate.txt";
            string template = File.ReadAllText(tmpFile);

            string body = template.Replace(MAC_ORDER_NUMBER, orderNum.ToString());
            body = body.Replace(MAC_BUYER, ClientName.Text);
            body = body.Replace(MAC_PHONE_NUMBER, ClientPhone.Text);
            body = body.Replace(MAC_BUYER_EMAIL, ClientMail.Text);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
                cmd.CommandText = 
                    "select c.ReleaseFormatId, b.BookName, b.Authors, rf.ReleaseFormatName, brf.Price, c.Quantity, Cost = brf.Price * c.Quantity " +
                        "from dbo.Cart c " +
                            "inner join dbo.Books b on c.BookId = b.BookId " +
                            "inner join dbo.ReleaseFormats rf on c.ReleaseFormatId = rf.ReleaseFormatId " +
                            "inner join dbo.BooksReleaseFormats brf on c.BookId = brf.BookId and c.ReleaseFormatId = brf.ReleaseFormatId " +
                        "where c.SessionId = @sessionId " +
                        "order by b.BookName, rf.ReleaseFormatName";
                cmd.Parameters.AddWithValue("sessionId", Session["sessionId"]);

                StringBuilder sb = new StringBuilder(10240);
                int sum = 0;
                bool paper = false;
                cmd.Connection.Open();
                try
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    try
                    {
                        sb.Append("<table cellspacing=\"0\" cellpadding=\".5em\"><theader><tr><th>Книга</th><th>Автор</th><th>Форма выпуска</th><th>Цена</th><th>Кол-во</th><th>Сумма</th></tr></thead><tbody>");
                        while (rdr.Read())
                        {
                            sb.Append("<tr><td>");
                            sb.Append(rdr["BookName"].ToString());
                            sb.Append("</td><td>");
                            sb.Append(rdr["Authors"].ToString());
                            sb.Append("</td><td>");
                            sb.Append(rdr["ReleaseFormatName"].ToString());
                            sb.Append("</td><td align=\"right\">");
                            sb.Append(rdr["Price"].ToString());
                            sb.Append("</td><td align=\"right\">");
                            sb.Append(rdr["Quantity"].ToString());
                            sb.Append("</td><td align=\"right\">");
                            sb.Append(rdr["Cost"].ToString());
                            sb.Append("</td></tr>");

                            if (Convert.ToInt32(rdr["ReleaseFormatId"]) == 1)
                            {
                                paper = true;
                            }

                            sum += Convert.ToInt32(rdr["Cost"]);
                        }
                        sb.Append("</tbody></table>");
                    }
                    finally
                    {
                        rdr.Close();
                    }
                }
                finally
                {
                    cmd.Connection.Close();
                }
                body = body.Replace(MAC_ORDER_LIST, sb.ToString());
                body = body.Replace(MAC_ORDER_SUM, sum.ToString());

                if (paper)
                {
                    if (BySubway.Checked)
                    {
                        body = body.Replace(MAC_DELIVERY_POINT,
                            string.Format("Доставка к ст. метро <b>{0}</b>", Station.SelectedValue));
                        body = body.Replace(MAC_DELIVERY_COST, "150 руб.");
                        sum += 150;
                    }
                    else
                    {
                        body = body.Replace(MAC_DELIVERY_POINT, "Получение на ст. метро <b>Алтуфьево</b>");
                        body = body.Replace(MAC_DELIVERY_COST, "бесплатно");
                    }
                }
                else
                {
                    body = body.Replace(MAC_DELIVERY_POINT, "Доставка <b>в электронной форме</b>");
                    body = body.Replace(MAC_DELIVERY_COST, "бесплатно");
                }
                body = body.Replace(MAC_TOTAL_SUM, sum.ToString());
                body = body.Replace(MAC_PAYMENT_METHOD, Card.Checked || !paper ? "перевод на карту Сбербанка № 6761 9600 0230 2315 74 (необходимо указать номер заказа)" : "наличными при получении");
                body = body.Replace(MAC_SALE_PHONE, ConfigurationManager.AppSettings["SalePhone"]);

                string sender = ConfigurationManager.AppSettings["SMTPAddress"];
                string subject = string.Format("bemkon.ru Заказ № {0}", orderNum.ToString());
                css_MasterPage.SendMail(ConfigurationManager.AppSettings["MailOrder"], sender, subject, body);
                css_MasterPage.SendMail(ClientMail.Text, sender, subject, body);
            }
        }

        private void ClearCart()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "delete from dbo.Cart where SessionId = @sessionId";
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
                cmd.Parameters.AddWithValue("sessionId", Session["sessionId"]);

                cmd.Connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    Session["quantityInCart"] = 0;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }
    }
}