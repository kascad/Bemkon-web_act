using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace ProfessorTesting
{
    public partial class ViewTestExamineePanel : System.Web.UI.UserControl
    {
        private int exmID;
        private int testID;
        private Archive.Examinee exam;
        private List<Archive.TestResult> testResults;

        protected void Page_Load(object sender, EventArgs e)
        {
            string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
            exmID = Core.Converting.ConvertToInt(sid);

            sid = HttpContext.Current.Request.QueryString[Core.Consts.reqIdTest];
            testID = Core.Converting.ConvertToInt(sid);

            FillData();

            HyperLinkCloseBottom.NavigateUrl = string.Format(HyperLinkCloseBottom.NavigateUrl, exmID);
        }

        private void FillData()
        {
            exam = Core.Site.CurrUserInfo.Archive.getExaminee(exmID);
            Archive.ExamineeTest test = exam.GetTest(testID);

            textBoxDate.Text = test.Date.ToShortDateString();
            textBoxExamName.Text = exam.Name;
            textBoxFullTime.Text = test.TestTime;
            textBoxTestName.Text = test.Name;

            if (!test.IsFinished)
            {
                labelTestStatus.Text = "Тест не окончен!";
                labelTestStatus.ForeColor = Color.Red;
            }

            try
            {
                testResults = test.GetSavedTestResults();

                dataGridViewResults.AutoGenerateColumns = false;
                dataGridViewResults.DataSource = testResults;
                dataGridViewResults.DataBind();
            
                textBoxAllQuestCount.Text = testResults.Count.ToString();
            }
            catch (Exception err)
            {
                Core.Site.RedirectError(Response, err.Message);
            }
        }

        private Old_App_Code.TestDataClassesDataContext db = Helper.CurrentDB;

        protected string GetWeight(object ansIDObj)
        {
            int ansId = int.Parse(ansIDObj.ToString());

            //var tst = db.Tests.FirstOrDefault(t => t.TestID == testID);
            //if (tst != null)
            //{
            //    foreach (var scale in tst.Scales)
            //    {
            //        TemplateField col = new TemplateField();
            //        col.HeaderText = "Вес " + scale.ScaleName;

            //        dataGridViewResults.Columns.Add(col);
            //    }
            //}

            string res = "";
            var weights = from p in db.ScaleWeights where ansId == (int)p.AnsID select p;
            foreach (var weight in weights)
            {
                res += weight.Weight.HasValue ? weight.Weight.Value.ToString() + " (" + weight.Scale.ScaleShortName + ") " : "";
            }
            return res;
        }

        internal void ExportExcel()
        {

            Old_App_Code.ExportTable table = GetExportTable();

            string fileName = "test" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() +
                DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                DateTime.Now.Second.ToString() + ".xls";

            SvbxExtensions.Export.ExportTableToExcel(fileName, table.Table);
        }

        private Old_App_Code.ExportTable GetExportTable()
        {
            Old_App_Code.ExportTable table = new Old_App_Code.ExportTable();

            TableRow rowHead = table.AddRow();
            table.AddCell(rowHead, "Обследуемый: " + exam.Name);
            table.AddCell(rowHead, "Тест: " + textBoxTestName.Text);
            table.AddRow();


            string[] colNames = new string[] { "№", "Вопрос", "№ ответа", "Ответ", "Вес" };

            TableHeaderRow hr = table.AddHeadRow();
            foreach (string cn in colNames)
            {
                table.AddHeadCell(hr, cn);
            }

            foreach (Archive.TestResult res in testResults)
            {
                TableRow tr = table.AddRow();
                table.AddCell(tr, res.QuestNumber.ToString());
                table.AddCell(tr, res.QuestText);
                table.AddCell(tr, res.AnsNumber);
                table.AddCell(tr, res.AnsText);
                table.AddCell(tr, GetWeight(res.AnsID));
            }

            return table;
        }

    }
}