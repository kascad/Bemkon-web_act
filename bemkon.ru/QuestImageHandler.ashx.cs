using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Drawing.Imaging;

namespace ProfessorTesting
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class QuestImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();

            if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
            {
                int id = Int32.Parse(context.Request.QueryString["id"]);

                var dc = new Testing.TestingDataContext(Helper.CurrConnectionString);
                var quest = dc.Questions.FirstOrDefault(q => q.QuestID == id);
                var image = Shared.ImageHelper.GetImage(quest.QuestImg);
                context.Response.ContentType = Shared.ImageHelper.GetMineImageFormat(image);
                image.Save(context.Response.OutputStream, image.RawFormat);
            }
            else
            {
                context.Response.ContentType = "text/html";
                context.Response.Write("<p>Need a valid id</p>");
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
