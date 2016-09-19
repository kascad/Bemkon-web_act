using System;
using System.Data;
using System.Configuration;
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
    public class Helper
    {

        public static Old_App_Code.TestDataClassesDataContext CurrentDB
        {
            get
            {
                return new Old_App_Code.TestDataClassesDataContext(CurrConnectionString);
            }
        }
        
        public static string CurrConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.
                    ConnectionStrings["MyConnectionString"].ConnectionString;
            }
        }
    }
}
