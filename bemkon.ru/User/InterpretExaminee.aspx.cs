using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using HtmlAgilityPack;

namespace ProfessorTesting.User
{
    public partial class InterpretExaminee : BaseUserPage
    {
        private int examineeID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверка авторизован ли пользователь
            if (!AccessPage())
                return;

            OpenArchive(labelArchName, labelExamName);

            Page.Title = "Интерпретация. " + Core.Site.titleProgram;

           // LabelTitle.Text = "Интерпретация";

            examineeID = Core.Site.CurrUserInfo.CurrExaminee != null ? Core.Site.CurrUserInfo.CurrExaminee.Id : 0;
            if (examineeID <= 0)
            {
                beginInterpretPanel.Visible = false;
                errorMsgPanel.Visible = true;
                ExportMenu.Visible = false;
                return;
            }

            ExportMenu.Visible = true;
            Archive.Archive arch = Core.Site.CurrUserInfo.Archive;// new Archive.Archive("arch");

            Archive.Examinee exam = arch.getExaminee(examineeID);// arch.;

            Testing.TestingDataContext dc = new Testing.TestingDataContext(
                ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);//Testing.GlobalOptions.ConnectionStr);

            string resp = "";
            UserPrivGroup priv = Core.Site.IsAuth(Page);

            Interpret.Interpretation interpret = new Interpret.Interpretation(exam);
            interpret.IsProUser = (priv == UserPrivGroup.ProUser);
            interpret.CalculateScaleBalls();

            resp += "<div class='header_block'>Пройденные тесты | Дата тестирования </div>";
            // int td = 0; 
            foreach (var ti1 in exam.ExamineeTests)
            {
               // if (td == 0)
              //  {
                  //  resp += "<div class='header_block'>Пройденные тесты: " + ti1.Name + " Дата тестирования:" + ti1.Date + "</div>";
                    resp += "<div class='header_block'>" + ti1.Name + " | " + ti1.Date + "</div>";
                //resp += "<div class='header_block'>Дата тестирования: " + ti1.Date + "</div>";
                //   td++;
                // }
                // td++;
            }

            foreach (var ti in dc.Interprets)
            {
                //System.Diagnostics.Debug.WriteLine("****************** FUUUUCK:" + ti.InterpretID);

                interpret.AnalizeInterpretRules(ti.InterpretID);

                if (interpret.Result != "")
                    resp += "<div class='header_block'>Интерпретация: " + ti.InterpretName + " (" + ti.InterpretShortName + ")</div>" + interpret.Result;
                
            }

            Interpret.Interpretation interpretForId1 = new Interpret.Interpretation(exam);
            interpretForId1.IsProUser = (priv == UserPrivGroup.ProUser);


           
            
            var intTable = interpretForId1.MakeReport().ToString();

            resp +=  intTable; // : GetClearResult(intTable);

            literalResult.Text = resp;
        }

        private string GetClearResult(string str)
        {
            var documentChanged = false; // флаг изменения документа после очистки
            var document = new HtmlDocument();
            document.LoadHtml(str);

            // удаляем таблицы
            var childNodes = document.DocumentNode.SelectNodes("table");
            if (childNodes != null)
            {
                foreach (var node in childNodes)
                {
                    node.ParentNode.RemoveChild(node);
                    documentChanged = true;
                }
            }

            // удаляем записи о тестах
            childNodes = document.DocumentNode.SelectNodes("p");
            if (childNodes != null)
            {
                foreach (var node in childNodes)
                {
                    if (node.InnerText.StartsWith("(№"))
                    {
                        node.ParentNode.RemoveChild(node);
                        documentChanged = true;
                    }
                }
            }

            if (documentChanged)
                str = document.DocumentNode.InnerHtml;

            //str = str.ToLower();
            // удаляем таблицы
            //str = System.Text.RegularExpressions.Regex.Replace(str, @"<table[^>]*>(.*)</table>", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // удаляем записи о тестах
            //str = System.Text.RegularExpressions.Regex.Replace(str, @"<p>\(№(.*)</p>", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // удаляем пустые теги
            //str = System.Text.RegularExpressions.Regex.Replace(str, @"<p>\s*</p>", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return str;
        }

        protected void ExportWord_Click(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            string fileName = "inter" + currentDateTime.Day.ToString() + currentDateTime.Month.ToString() +
                currentDateTime.Year.ToString() + currentDateTime.Hour.ToString() + currentDateTime.Minute.ToString() + ".doc";

            Response.Clear();
            Response.ContentType = "application/vnd.ms-word";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
            //literalResult.RenderControl(htmlWrite);
            //Response.Output.Write(stringWrite.ToString());
            //Response.End();

            Response.Write("<html>");
            Response.Write("<head>");
            Response.Write("<META HTTP-EQUIV=Content-Type CONTENT=text/html; charset=UTF-8>");
            Response.Write("<meta name=ProgId content=Word.Document>");
            Response.Write("<meta name=Generator content=Microsoft Word 9>");
            Response.Write("<meta name=Originator content=Microsoft Word 9>");
            Response.Write("<style>");
            Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:1.0in 1.25in 1.0in 1.25in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            Response.Write("div.Section1 {page:Section1;}");
            Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:landscape;margin:1.25in 1.0in 1.25in 1.0in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            Response.Write("div.Section2 {page:Section2;}");
            Response.Write("</style>");
            Response.Write("</head>");
            Response.Write("<body>");
            //Section1: Portrait, Section2: Landscape
            Response.Write(literalResult.Text);
            //Response.Write("</div>");
            Response.Write("</body>");
            Response.Write("</html>");
            Response.End();
        }
    }
}
