using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProfessorTesting
{
    public partial class Default : System.Web.UI.MasterPage
    {
        public string CartInfoText
        {
            get { return CartInfo.Text; }
            set { CartInfo.Text = value; }
        }

        public bool CartInfoVisible
        {
            get { return CartInfo.Visible; }
            set { CartInfo.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowCartInfo();
        }

        public void ShowCartInfo()
        {
            int quantityInCart = Convert.ToInt32(Session["quantityInCart"]);
            CartInfo.Visible = quantityInCart != 0;
            CartInfo.Text = string.Format("Книг в корзине: {0}", quantityInCart);
        }
    }
}