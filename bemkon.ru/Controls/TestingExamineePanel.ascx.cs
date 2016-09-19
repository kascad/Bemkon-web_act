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
using System.IO;
using Shared;

namespace ProfessorTesting
{
    public partial class TestingExamineePanel : System.Web.UI.UserControl
    {
        private Testing.ProfessorTest professorTest;
        private int examineeID;
        int idBattery;
        int idTest;
        FromType from = FromType.None;
        string redirectUrl = "";
        private bool isTested = false;
        private static Shared.TimeCounter timeCounter = new Shared.TimeCounter();
        private static Shared.QuestTimer currQuestTimer = new Shared.QuestTimer();

        protected void Page_Load(object sender, EventArgs e)
        {
            examineeID = Core.Site.CurrUserInfo.CurrExaminee != null ? Core.Site.CurrUserInfo.CurrExaminee.Id : 0;
            if (examineeID <= 0)
            {
                beginTestPanel.Visible = false;
                errorMsgPanel.Visible = true;
                LabelTitle.Visible = false;
                return;
            }

            LinkButtonYes.Click += new EventHandler(LinkButtonYes_Click);
            LinkButtonNo.Click += new EventHandler(LinkButtonNo_Click);
            LinkButtonCancel.Click += new EventHandler(LinkButtonCancel_Click);
            LinkButtonBegin.Click += new EventHandler(LinkButtonBegin_Click);

            string sfrom = HttpContext.Current.Request.QueryString[Core.Consts.reqFrom];
            from = Core.Converting.ConvertToFromType(sfrom);

            redirectUrl = "~/User/" + Core.Site.GetPageFrom(from);

            string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqIdBattery];
            idBattery = Core.Converting.ConvertToInt(sid);
            sid = HttpContext.Current.Request.QueryString[Core.Consts.reqIdTest];
            idTest = Core.Converting.ConvertToInt(sid);


            string batteryName = "";

            if (idBattery != 0)
            {
                Testing.TestingDataContext dc = new Testing.TestingDataContext(GlobalOptions.RemoteConnectionString);

                foreach (var bBat in dc.Batteries.Where(bt => bt.BatteryID == idBattery))
                {
                    if (bBat != null)
                    {
                        batteryName = bBat.BatteryName;
                        break;
                    }
                }

                bool find = false;
                bool find2 = false;
                foreach (var bTest in dc.BatteryTests.Where(bt => bt.BatteryID == idBattery).OrderBy(bt => bt.Number))
                {
                    if (idTest == 0)
                    {
                        Archive.Examinee exam = Core.Site.CurrUserInfo.Archive.getExaminee(examineeID);
                        Archive.ExamineeTest test2 = exam.GetTest(bTest.TestID);
                        if (test2 == null || !test2.IsFinished)
                        {
                            idTest = bTest.TestID;
                            find2 = true;
                            break;
                        }
                    }
                    else if (idTest == bTest.TestID)
                    {
                        find = true;
                        continue;
                    }
                    if (find)
                    {
                        idTest = bTest.TestID;
                        find2 = true;
                        break;
                    }
                }
                if (!find2)
                {
                    Response.Redirect(redirectUrl);
                    return;
                }

                //TODO TestUser
                //ButtonClose.Visible = true;
                //ButtonClose.NavigateUrl = "~/User/TestExaminee.aspx?id_battery=" + idBattery + "&id_test=" + idTest + "&from=" + (int)from;

            }

            if (!Page.IsPostBack)
            {
                if (idTest != 0)
                {
                    beginPanel.Visible = true;
                    Testing.TestingDataContext dc = new Testing.TestingDataContext(GlobalOptions.RemoteConnectionString);
                    var test = dc.Tests.First(t => t.TestID == idTest);

                    if (idBattery != 0)
                        LabelTitle.Text = "Батарея тестов: " + batteryName + ". Тест: "
                                          + test.FullName + " (" + test.ShortName + ")";
                    else
                        LabelTitle.Text = "Тест: " + test.FullName + " (" + test.ShortName + ")";


                    Archive.Examinee exam = Core.Site.CurrUserInfo.Archive.getExaminee(examineeID);
                    Archive.ExamineeTest test2 = exam.GetTest(idTest);

                    if (test2 != null)
                    {
                        messagePanel.Visible = true;
                        BeginTest.Visible = false;

                        if (test2.IsFinished)
                        {

                            string mess = "Tест '" + test2.Name + "' уже пройден!<br/>";
                            message.InnerHtml = mess;
                            LinkButtonYes.Visible = false;
                        }
                        else
                        {
                            string mess = "Тестирование не завершено.<br/>";
                            message.InnerHtml = mess;
                            LinkButtonYes.Visible = true;
                            // TODO TestUser
                            //LinkButtonCancel.Visible = true;
                        }
                    }

                    if (test.Preamble != null && test.Preamble != "")
                    {
                        try
                        {
                            StreamReader reportStream = new StreamReader(Core.FileEx.GetPathFSData(test.Preamble));
                            string text = reportStream.ReadToEnd();
                            reportStream.Close();

                            text = text.Replace("\r\n", "<p>");
                            text = text.Replace("\n\r", "<p>");

                            Preamble.InnerHtml = "<div style='text-align:center;font-weight:bold;font-size:130%;padding-bottom:15px;'>Инструкция</div>"
                                                 + text;
                        }
                        catch (Exception)
                        {
                            System.Diagnostics.Trace.TraceError("Preamble text not found! TestID={0} Preamble={1}\r\n", test.TestID, test.Preamble);
                        }
                    }
                }
            }

        }

        void LinkButtonBegin_Click(object sender, EventArgs e)
        {
            if (idTest == 0)
                return;

            professorTest = new Testing.ProfessorTest(idTest, examineeID, Core.Site.CurrUserInfo.Archive);

            if (!isTested)
            {
                IncreaseTestCount(idTest);
            }

            if (idBattery != 0)
                Response.Redirect("~/User/Test.aspx?id_battery=" + idBattery + "&id_test=" + idTest 
                + "&from=" + (int)from);
            else
                Response.Redirect("~/User/Test.aspx?id_test=" + idTest 
                + "&from=" + (int)from);

        }
        void buttonNext_Click(object sender, ImageClickEventArgs e)
        {
        }

        private static void IncreaseTestCount(int testID)
        {
            Testing.TestingDataContext dc = new Testing.TestingDataContext(GlobalOptions.RemoteConnectionString);
            dc.Tests.First(t => t.TestID == testID).TestingCount++;
            dc.SubmitChanges();
        }

        void LinkButtonCancel_Click(object sender, EventArgs e)
        {
            RedirectCancel();
        }

        private void RedirectCancel()
        {
            if (idBattery != 0)
                Response.Redirect("~/User/TestExaminee.aspx?id_battery=" + idBattery 
                    + "&id_test=" + idTest
                    + "&from=" + (int)from);
            else
                Response.Redirect(redirectUrl);
        }
        void LinkButtonNo_Click(object sender, EventArgs e)
        {
            if (LinkButtonCancel.Visible)
            {
                if (Core.Site.CurrUserInfo.CurrExaminee != null)
                {
                    Archive.ExamineeTest test2 = Core.Site.CurrUserInfo.CurrExaminee.GetTest(idTest);
                    if (test2!=null)
                        test2.Delete();
                    Response.Redirect(Request.Url.ToString());
                }
            }
            else
            {
                RedirectCancel();
            }
        }

        private void LinkButtonYes_Click(object sender, EventArgs e)
        {
            isTested = true;
            if (idBattery != 0)
            {
                Response.Redirect("~/User/Test.aspx?id_battery=" + idBattery
                                  + "&id_test=" + idTest
                                  + "&from=" + (int) from + (isTested ? "&isTested=true" : ""));
            }
            else
                Response.Redirect("~/User/Test.aspx?id_test=" + idTest
                                  + "&from=" + (int) from + (isTested ? "&isTested=true" : "")); //isTested = true;
        }
    }
}