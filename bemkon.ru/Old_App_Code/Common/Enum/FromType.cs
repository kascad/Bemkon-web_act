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
    public enum FromType
    {
        None = 0,
        Examinee = 1,
        Battery = 2,
        Test = 3,
        TopTest = 4,
        Interpret = 5,
    }
}
