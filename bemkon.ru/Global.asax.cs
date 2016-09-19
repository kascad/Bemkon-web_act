using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ProfessorTesting
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["sessionId"] = Guid.NewGuid();
            Session["quantityInCart"] = 0;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            int q = Convert.ToInt32(Session["quantityInCart"]);
            if (q != 0)
            {
                using (SqlCommand cmd = new SqlCommand("delete from dbo.Cart where SessionId = @sessionId", new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString)))
                {
                    cmd.Parameters.AddWithValue("sessionId", Session["sessionId"]);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}