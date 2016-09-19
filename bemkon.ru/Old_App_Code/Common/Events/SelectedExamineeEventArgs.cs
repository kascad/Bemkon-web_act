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
    public class SelectedExamineeEventArgs
    {
        int idExaminee = 0;
        public int IdExaminee
        {
            get { return idExaminee; }
        }
        string nameExaminee = "";
        public string NameExaminee
        {
            get { return nameExaminee; }
        }

        public SelectedExamineeEventArgs(int idExaminee, string nameExaminee)
        {
            this.idExaminee = idExaminee;
            this.nameExaminee = nameExaminee;
        }
    }
}
