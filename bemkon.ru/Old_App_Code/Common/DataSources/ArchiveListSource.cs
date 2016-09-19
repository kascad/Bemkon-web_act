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
using System.ComponentModel;

namespace ProfessorTesting
{
    public class ArchiveListSource : Component, IListSource
    {
        public ArchiveListSource() { }

        public ArchiveListSource(IContainer container)
        {
            container.Add(this);
        }

        #region IListSource Members

        bool IListSource.ContainsListCollection
        {
            get { return false; }
        }

        System.Collections.IList IListSource.GetList()
        {
            return Core.Site.CurrUserInfo.Archive.ArchExaminees;
        }

        #endregion
    }
}
