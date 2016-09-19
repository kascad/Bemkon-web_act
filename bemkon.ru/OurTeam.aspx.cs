using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProfessorTesting
{
    public partial class OurTeam : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Наша команда. " + Core.Site.titleProgram;
        }

        protected void TeamMembers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;

                Image img = (Image)e.Item.FindControl("MemberPhoto");
                string photo = row["ImageFile"].ToString().Trim();
                if (!String.IsNullOrEmpty(photo))
                {
                    img.ImageUrl = String.Format("{0}Personalities/{1}", css_MasterPage.DBImageRoot(), photo);
                    img.Visible = true;
                }
                else
                {
                    img.Visible = false;
                }

                HyperLink mb = (HyperLink)e.Item.FindControl("MoreButton");
                mb.Visible = !String.IsNullOrEmpty(row["HiddenInfo"].ToString().Trim());
                if (mb.Visible)
                {
                    Panel p = (Panel)e.Item.FindControl("MoreInfo");
                    mb.Attributes["onclick"] = "$('#" + mb.ClientID + "').hide();$('#" + p.ClientID + "').show(300);";
                    HyperLink hi = (HyperLink)e.Item.FindControl("HideInfo");
                    hi.Attributes["onclick"] = "$('#" + mb.ClientID + "').show();$('#" + p.ClientID + "').hide(300);";
                }
            }
        }
    }
}