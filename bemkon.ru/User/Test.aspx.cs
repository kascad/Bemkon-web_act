using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Shared;
using Testing.Data;

namespace ProfessorTesting.User
{
    public partial class Test : BaseUserPage
    {
        private Testing.ProfessorTest professorTest;
        private int examineeID;
        private int idBattery;
        private int idTest;
        private int idQuestion;
        private int idAnswer;
        private ActionType actionType = ActionType.None;
        private string reqTime = "";
        private string reqTimeFull = "";
        private bool isInsideAction = false;

        private FromType from = FromType.None;
        private string redirectUrl = "";
        private bool isTested = false;
        //private List<RadioButton> listRadioGraph = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверка авторизован ли пользователь
            if (!AccessPage())
                return;

            OpenArchive(labelArchName, labelExamName);

            Page.Title = "Тестирование обследуемого. " + Core.Site.titleProgram;

            isInsideAction = false;
            string sfrom = HttpContext.Current.Request.QueryString[Core.Consts.reqFrom];
            FromType from = Core.Converting.ConvertToFromType(sfrom);
            switch (from)
            {
                case FromType.Examinee:
                    idcustom.Text = "Обследуемые";
                    idcustom.NavigateUrl = "~/User/";
                    break;
                case FromType.Battery:
                    idcustom.Text = "Батареи тестов";
                    idcustom.NavigateUrl = "~/User/BatteryTests.aspx";
                    break;
                case FromType.Test:
                    idcustom.Text = "Отдельные тесты";
                    idcustom.NavigateUrl = "~/User/Tests.aspx";
                    break;
                case FromType.TopTest:
                    idcustom.Text = "Топ 10 тестов";
                    idcustom.NavigateUrl = "~/User/TopTests.aspx";
                    break;
            }

            // Из строки запроса
            InitRequestedParam();

            // Получение информации о тесте
            InitProfessorTest();

            if (professorTest == null)
                return;

            buttonNext.Attributes.Add("onclick", "StopTimer();");
            buttonPrev.Attributes.Add("onclick", "StopTimer();");

            if (!Page.IsPostBack)
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                if (isTested && actionType == ActionType.None)
                {
                    foreach (Archive.TestResult res in professorTest.SavedTests)
                    {
                        DateTime dtt = Core.Converting.ToTime(res.Time);
                        TimeSpan v = new TimeSpan(dtt.Hour, dtt.Minute, dtt.Second);
                        dt += v;
                    }
                }
                else
                {
                    if (reqTime != "" && reqTime != null)
                    {
                        TimeSpan d = new TimeSpan(Core.Converting.ConvertToLong(reqTime));
                        dt += d;
                        //dt = Core.Converting.ToTime();
                    }
                    else if (reqTimeFull != "" && reqTimeFull != null)
                    {
                        dt = new DateTime(1, 1, 1,
                            Core.Converting.ConvertToInt(reqTimeFull.Substring(0, 2)),
                            Core.Converting.ConvertToInt(reqTimeFull.Substring(2, 2)),
                            Core.Converting.ConvertToInt(reqTimeFull.Substring(4, 2)));
                        //dt = Core.Converting.ToTime();
                    }
                    else
                    {
                        if (Session["fullTime"] != null)
                            dt = Core.Converting.ToTime(Session["fullTime"].ToString());
                    }
                    if (Session["oneTime"] != null)
                        dt1 = Core.Converting.ToTime(Session["oneTime"].ToString());
                }
                SetFullTime(dt);
                SetOneTime(dt1);
            }

            if (Page.IsPostBack)
            {
                TimeToSession();
            }

            if (Page.IsPostBack)
                return;


            if ((actionType == ActionType.Next || actionType == ActionType.None) && professorTest.CurrentQuest.IsSaved)
            {
                string full = hiddenFullTime.Value;
                string one = hiddenOneTime.Value;
                professorTest.NextQuest(professorTest.CurrentQuest.SelAnsID, one, full);
            }

            if (actionType == ActionType.PrevNavigation || actionType == ActionType.NextNavigation || actionType == ActionType.Navigation)
            {
                professorTest.SetQuestByID(idQuestion);
            }

            hiddenSaveQuest.Value = professorTest.CurrentQuest.IsSaved ? "1" : "0";

            // Заполнение информации о тесте
            InitInfoTest();

            if (isTested)
                IncreaseTestCount(idTest);
            
            // Заполнение информации о вопросе
            InitInfoQuest();

            // Заполнение списка вопросов в левой панели
            FillQuestions(professorTest);

            if (professorTest != null)
                // Заполнение списка ответов
                MakeAnswers(professorTest.CurrentQuest);

            if (professorTest.HorisontalAnswers)
                RadioButtonListQuest.RepeatDirection = RepeatDirection.Horizontal;

            // Инициализация таймеров и запуск таймера
            InitTimer();
        }

        private void TimeToSession()
        {
            if (Session["fullTime"] == null)
                Session.Add("fullTime", "");
            Session["fullTime"] = hiddenFullTime.Value;
            if (Session["OneTime"] == null)
                Session.Add("OneTime", "");
            Session["OneTime"] = hiddenOneTime.Value;
        }
        private void SetFullTime(DateTime dt)
        {
            hiddenCurHour.Value = dt.Hour.ToString();
            hiddenCurMin.Value = dt.Minute.ToString();
            hiddenCurSec.Value = dt.Second.ToString();
            string tm = Core.Converting.FormatTime(dt);
            hiddenFullTime.Value = tm;
            labelFullTime.Text = "Общее время: " + tm;
        }
        private void SetOneTime(DateTime dt)
        {
            hiddenOneCurHour.Value = dt.Hour.ToString();
            hiddenOneCurMin.Value = dt.Minute.ToString();
            hiddenOneCurSec.Value = dt.Second.ToString();
            hiddenOneTime.Value = dt.Hour + ":" + dt.Minute + ":" + dt.Second;
        }

        protected void InitRequestedParam()
        {
            reqTime = HttpContext.Current.Request.QueryString["time"];
            reqTimeFull = HttpContext.Current.Request.QueryString["timefull"];

            string sfrom = HttpContext.Current.Request.QueryString[Core.Consts.reqFrom];
            from = Core.Converting.ConvertToFromType(sfrom);

            redirectUrl = "~/User/" + Core.Site.GetPageFrom(from);

            sfrom = HttpContext.Current.Request.QueryString[Core.Consts.reqAction];
            actionType = Core.Converting.ConvertToActionType(sfrom);

            string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqIdTest];
            idTest = Core.Converting.ConvertToInt(sid);

            sid = HttpContext.Current.Request.QueryString[Core.Consts.reqIdBattery];
            idBattery = Core.Converting.ConvertToInt(sid);

            sid = HttpContext.Current.Request.QueryString[Core.Consts.reqIdQuestion];
            idQuestion = Core.Converting.ConvertToInt(sid);

            sid = HttpContext.Current.Request.QueryString[Core.Consts.reqIdAnswer];
            idAnswer = Core.Converting.ConvertToInt(sid);

            examineeID = Core.Site.CurrUserInfo.CurrExaminee != null ? Core.Site.CurrUserInfo.CurrExaminee.Id : 0;

            sid = HttpContext.Current.Request.QueryString["isTested"];
            isTested = sid != null && sid != "";

            messPanel.Visible = false;

        }
        private void InitProfessorTest()
        {
            try
            {
                professorTest = new Testing.ProfessorTest(idTest, examineeID, Core.Site.CurrUserInfo.Archive);
                hiddenQuestId.Value = professorTest.CurrentQuest.Question.QuestID.ToString();
            }
            catch (Exception)
            {
                Core.Site.RedirectError(Response, "Не выбран пользователь или не указан тест");
            }
        }
        protected void InitInfoTest()
        {
            if (professorTest != null)
            {
                // Всего вопросов
                var testCount = Helper.CurrentDB.Questions.Count(t => t.TestID == professorTest.Test.TestID);
                labelAllQuest.InnerHtml = "Всего вопросов: " + testCount;
                // Название теста
                labelTestName.InnerHtml = "Тест: " + professorTest.Test.ShortName;
            }
        }
        private void InitInfoQuest()
        {
            if (professorTest != null)
            {
                var quest = professorTest.CurrentQuest.Question;
                // Вопрос
                try // TODO переделать без исключения
                {
                    if (quest.QuestImgByte == null)
                    {
                        labelQuestText.InnerHtml = quest.QuestText;
                    }
                    else
                    {
                        var image = Shared.ImageHelper.GetImage(quest.QuestImgByte);
                        //var fileName = GetTempImage(image, quest.QuestID);
                        const string imgPatt = "<img src='/QuestImageHandler.ashx?id={0}' alt=''/>";
                        labelQuestText.InnerHtml = "<p>" + quest.QuestText + "</p>" + string.Format(imgPatt, quest.QuestID);
                    }
                }
                catch (NullReferenceException)
                {
                    labelQuestText.InnerHtml = quest.QuestText;
                }

                // Кнопка Назад
                buttonPrev.Enabled = !professorTest.IsFirsQtuest;
                // Кол-во отвеченных
                int passedQuest = professorTest.CurrentQuest.IsSaved ? professorTest.SavedQuestCount :
                    professorTest.SavedQuestCount - 1;
                labelQuestAnswered.InnerHtml = "Пройдено: " + passedQuest.ToString();
            }
        }

        private string GetTempImage(System.Drawing.Image image, int questId)
        {
            var imgFolder = Server.MapPath("~/Images/QuestImgs");
            var format = GetFormat(image);
            const string fileNamePatt = "{0}\\{1}.{2}";
            var fileName = string.Format(fileNamePatt, imgFolder, questId, format);
            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);
            image.Save(fileName);
            return string.Format("/Images/QuestImgs/{0}.{1}", questId, format);
        }

        private string GetFormat(System.Drawing.Image image)
        {
            if (image.RawFormat.Equals(ImageFormat.Jpeg))
                return "jpg";
            else if (image.RawFormat.Equals(ImageFormat.Bmp))
                return "bmp";
            else if (image.RawFormat.Equals(ImageFormat.Emf))
                return "emf";
            else if (image.RawFormat.Equals(ImageFormat.Exif))
                return "exif";
            else if (image.RawFormat.Equals(ImageFormat.Gif))
                return "gif";
            else if (image.RawFormat.Equals(ImageFormat.Icon))
                return "icon";
            else if (image.RawFormat.Equals(ImageFormat.Png))
                return "png";
            else if (image.RawFormat.Equals(ImageFormat.Tiff))
                return "tiff";
            else if (image.RawFormat.Equals(ImageFormat.Wmf))
                return "wmf";
            return "";
        }

        private void InitTimer()
        {
        }
        private void IncreaseTestCount(int testID)
        {
            try
            {
                Testing.TestingDataContext dc = new Testing.TestingDataContext(
                    GlobalOptions.RemoteConnectionString);
                dc.Tests.First(t => t.TestID == testID).TestingCount++;
                dc.SubmitChanges();
            }
            catch (Exception err)
            {
                Core.Site.RedirectError(Response, err.Message);
            }
        }
        public void FillQuestions(Testing.ProfessorTest profTest)
        {
            if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.TestUser)
                return;
            if (profTest == null)
                return;

            container.Controls.Clear();
            foreach (Archive.TestResult t in profTest.SavedTests)
            {
                container.Controls.Add(CreateLinkLabel(t.QuestText, t.QuestID, container.Controls.Count));
            }
        }
        private Control CreateLinkLabel(string text, object tag, int pos)
        {
            Panel div = new Panel();
            div.CssClass = "quest_item";

            HyperLink link = new HyperLink();

            //LinkButton link = new LinkButton();
            link.Text = (pos + 1).ToString() + ". " + getCutString(text, 45);
            link.ID = "link" + tag.ToString();
            link.NavigateUrl = "~/User/Test.aspx?id_test=" + idTest
                        + (idBattery != 0 ? "&id_battery=" + idBattery : "")
                        + "&from=" + (int)from
                        + "&id_question=" + tag.ToString()
                        + "&action=" + (int)ActionType.Navigation;
            link.Attributes.Add("onclick", "add_time_tolink(this);");

            div.Controls.Add(link);

            return div;
        }
        private string getCutString(string str, int len)
        {
            return str.Length > len ? str.Substring(0, len - 3) + "..." : str;
        }
        private void MakeAnswers(Testing.ProfQuest profQuest)
        {
            if (profQuest.QuestType == Testing.QuestTypes.Text)
            {
                MakeAnswerText(profQuest);
                containerQuestGraph.Visible = false;
                containerQuestText.Visible = true;
            }
            else
            {
                MakeAnswerGraph(profQuest);
                containerQuestGraph.Visible = true;
                containerQuestText.Visible = false;
            }

            SetEnabledQuest(profQuest);
        }
        private void MakeAnswerText(Testing.ProfQuest profQuest)
        {
            RadioButtonListQuest.Items.Clear();
            //var listRadio = new List<Testing.Answer>();

            //foreach (var ans in profQuest.Question.Answers.OrderBy(a => a.AnsNum))
            //foreach (var ans in Helper.CurrentDB.Answers.Where(t => t.QuestID == profQuest.Question.QuestID).OrderBy(a => a.AnsNum))
            //{
            //    listRadio.Add(ans);
            //}

            var listRadio = Helper.CurrentDB.Answers.Where(t => t.QuestID == profQuest.Question.QuestID).OrderBy(a => a.AnsNum);

            RadioButtonListQuest.DataBound += new EventHandler(RadioButtonListQuest_DataBound);
            RadioButtonListQuest.DataSource = listRadio;
            RadioButtonListQuest.DataBind();
            containerQuestText.Visible = true;
            containerQuestGraph.Visible = false;
        }
        void RadioButtonListQuest_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem item in RadioButtonListQuest.Items)
            {
                if (Core.Converting.ConvertToInt(item.Value) == professorTest.CurrentQuest.SelAnsID)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
        private void MakeAnswerGraph(Testing.ProfQuest profQuest)
        {
            //var ansList = new List<Testing.Answer>();

            ////foreach (var pq in profQuest.Question.Answers.OrderBy(a => a.AnsNum))
            //foreach (var pq in Helper.CurrentDB.Answers.Where(t => t.QuestID == profQuest.Question.QuestID).OrderBy(a => a.AnsNum))
            //{
            //    if (professorTest.SavedTests.Count(q => q.AnsText == pq.AnsText) == 0)
            //    {
            //        ansList.Add(pq);
            //    }   
            //}

            // #3 для Люшера исключаем уже выбранные изображения
            int question = 0;
            if (profQuest.Question.QuestNum != null && profQuest.Question.QuestNum > 0)
                question = (int)profQuest.Question.QuestNum - 1;

            var oldAnsList = professorTest.SavedTests.Take(question).Select(a => a.AnsNumber); // получаем список первых выбранных ответов 
            var ansList = Helper.CurrentDB.Answers.Where(t => t.QuestID == profQuest.Question.QuestID).Where(a => !oldAnsList.Contains(a.AnsNum.ToString())).OrderBy(a => a.AnsNum);


            RepeaterGraph.DataBinding += new EventHandler(RepeaterGraph_DataBinding);
            //RepeaterGraph.DataSource = profQuest.question.Answers.OrderBy(a => a.AnsNum);
            RepeaterGraph.DataSource = ansList;
            RepeaterGraph.DataBind();

            for (int i = 0; i < RepeaterGraph.Items.Count; i++)
            {
                RepeaterItem radio = RepeaterGraph.Items[i];
                HtmlControl label = radio.FindControl("label_radio_graph") as HtmlControl;

                RadioButton but = radio.FindControl("radio_graph") as RadioButton;

                if (but != null && label != null)
                    label.Attributes["for"] = but.ClientID;


                if (but != null)
                {
                    HiddenField hid = radio.FindControl("graph_id") as HiddenField;
                    if (hid != null)
                    {
                        string sid = hid.Value;
                        int id = Core.Converting.ConvertToInt(sid);
                        if (id == professorTest.CurrentQuest.SelAnsID)
                        {
                            but.Checked = true;
                            break;
                        }
                    }
                }
            }
        }
        void RepeaterGraph_DataBinding(object sender, EventArgs e)
        {
            foreach (RepeaterItem radio in RepeaterGraph.Items)
            {
                RadioButton but = radio.FindControl("radio_graph") as RadioButton;
                if (but != null)
                {
                    HiddenField hid = radio.FindControl("graph_id") as HiddenField;
                    if (hid != null)
                    {
                        string sid = hid.Value;
                        int id = Core.Converting.ConvertToInt(sid);
                        if (id == professorTest.CurrentQuest.SelAnsID)
                        {
                            but.Checked = true;
                            break;
                        }
                    }
                }
            }
        }
        private void SetEnabledQuest(Testing.ProfQuest profQuest)
        {
            bool enabled = !profQuest.IsSaved;
            if (profQuest.QuestType == Testing.QuestTypes.Text)
            {
                RadioButtonListQuest.Enabled = enabled;
            }
            else
            {
                foreach (RepeaterItem radio in RepeaterGraph.Items)
                {
                    RadioButton but = radio.FindControl("radio_graph") as RadioButton;
                    if (but != null)
                    {
                        but.Enabled = enabled;
                    }
                }
            }
        }
        private void SetEnabledQuest(bool enabled)
        {
            if (containerQuestText.Visible)
            {
                RadioButtonListQuest.Enabled = enabled;
            }
            else
            {
                foreach (RepeaterItem radio in RepeaterGraph.Items)
                {
                    RadioButton but = radio.FindControl("radio_graph") as RadioButton;
                    if (but != null)
                    {
                        but.Enabled = enabled;
                    }
                }
            }
        }
        #region Обработчики

        protected void Timer1_Tick(object sender, EventArgs e)
        {
        }

        private int GetCurrentAnswer()
        {
            int SelectedAnsId = -1;
            if (RadioButtonListQuest.Items.Count > 0)
            {
                SelectedAnsId = Core.Converting.ConvertToInt(RadioButtonListQuest.SelectedValue);
            }
            else
            {
                foreach (RepeaterItem radio in RepeaterGraph.Items)
                {
                    RadioButton but = radio.FindControl("radio_graph") as RadioButton;
                    if (but != null)
                    {
                        if (but.Checked)
                        {
                            HiddenField idf = radio.FindControl("graph_id") as HiddenField;
                            if (idf != null)
                            {
                                string sid = idf.Value;
                                int id = Core.Converting.ConvertToInt(sid);
                                if (id > 0)
                                {
                                    SelectedAnsId = id;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (SelectedAnsId == 0)
                SelectedAnsId = -1;

            return SelectedAnsId;
        }
        protected void buttonNext_Click(object sender, ImageClickEventArgs ex)
        {
            if (professorTest == null)
                return;
            if (isInsideAction)
                return;

            isInsideAction = true;

            // Мы пришли по кнопке Назад, Вперед или левая панель навигации
            if (actionType == ActionType.PrevNavigation || actionType == ActionType.NextNavigation || actionType == ActionType.Navigation)
            {
                if (idQuestion != 0)
                {
                    var questions = Helper.CurrentDB.Questions.Where(t => t.TestID == professorTest.Test.TestID).OrderBy(q => q.QuestNum).ToList();
                    var questCount = questions.Count;

                    for (int i = 0; i < questCount; i++)
                    {
                        if (questions[i].QuestID == idQuestion)
                        {
                            if (i + 1 < questCount)
                            {
                                idQuestion = questions[i + 1].QuestID;
                            }
                            break;
                        }
                    }
                }

                bool find = false;
                foreach (Archive.TestResult res in professorTest.SavedTests)
                {
                    if (res.QuestID == idQuestion)
                    {
                        find = true;
                        break;
                    }
                }
                // Уже пройденный тест
                if (find)
                {
                    Response.Redirect("~/User/Test.aspx?id_test=" + idTest
                        + (idBattery != 0 ? "&id_battery=" + idBattery : "")
                        + "&from=" + (int)from
                        + "&id_question=" + idQuestion
                        + "&action=" + (int)ActionType.NextNavigation);
                    return;
                }
                // Неотвеченный тест
                else
                {
                    Response.Redirect("~/User/Test.aspx?id_test=" + idTest
                        + (idBattery != 0 ? "&id_battery=" + idBattery : "")
                        + "&from=" + (int)from);
                    return;
                }

            }

            int SelectedAnsId = GetCurrentAnswer();

            // Не по кнопке Следующий, но вопрос сохранен
            if (actionType != ActionType.NextNavigation && professorTest.CurrentQuest.IsSaved)
            {
                string full = hiddenFullTime.Value;
                string one = hiddenOneTime.Value;
                professorTest.NextQuest(professorTest.CurrentQuest.SelAnsID, one, full);
            }

            bool sav = professorTest.CurrentQuest.IsSaved;
            bool resetTime = false;

            // Есть выбранный ответ
            if (SelectedAnsId != -1)
            {
                bool finish = professorTest.IsFinishedQuest;
                if (finish)
                {
                    testPanel.Visible = false;
                    if (idBattery != 0)
                    {
                        Testing.TestingDataContext dc = new Testing.TestingDataContext(GlobalOptions.RemoteConnectionString);
                        var bat = dc.Batteries.First(c => c.BatteryID == idBattery);
                        bool find = false;
                        int idNextTest = 0;
                        string nameNextTest = "";
                        foreach (var test in bat.BatteryTests)
                        {
                            Archive.Examinee exam = Core.Site.CurrUserInfo.Archive.getExaminee(examineeID);
                            Archive.ExamineeTest test2 = exam.GetTest(test.TestID);
                            if (test2 != null && test2.IsFinished)
                                continue;

                            if (test.TestID == idTest)
                            {
                                find = true;
                                continue;
                            }
                            if (find)
                            {
                                idNextTest = test.TestID;
                                nameNextTest = test.Test.FullName + " (" + test.Test.ShortName + ")";
                                break;
                            }
                        }

                        LabelTitle.Visible = true;
                        LabelTitle.Text = "Батарея: " + bat.BatteryName;
                        LabelMess.Text = "Вы ответили на все вопросы теста. Благодарим за работу!\r\nДля завершения работы с Мастером тестирования нажмите на ссылку Выход.";

                        if (idNextTest > 0)
                        {
                            panelBattery.Visible = true;
                            ButtonNextTest.NavigateUrl = "~/User/TestExaminee.aspx?id_battery=" + idBattery
                                + "&id_test=" + idTest + "&from=" + (int)from;
                            ButtonNextTest.Text = "Перейти к следующему тесту батареи " + nameNextTest;
                        }

                        resetTime = true;
                    }
                    else
                        LabelMess.Text = "Вы ответили на все вопросы теста. Благодарим за работу!\r\nДля завершения работы с Мастером тестирования нажмите на ссылку Выход.";
                    LabelMess.ForeColor = Color.Black;
                    messPanel.Visible = true;
                }

                try
                {
                    string full = hiddenFullTime.Value;
                    string one = hiddenOneTime.Value;
                    professorTest.NextQuest(SelectedAnsId, one, full);
                    if (!finish && !sav)
                    {
                        DateTime dt = Core.Converting.ToTime(full);

                        Response.Redirect("~/User/Test.aspx?id_test=" + idTest
                            + (idBattery != 0 ? "&id_battery=" + idBattery : "")
                            + ("&from=" + ((int)from).ToString())
                            + "&action=" + (sav ? (int)ActionType.NextNavigation : (int)ActionType.Next)
                            + "&time=" + dt.Ticks);
                        return;
                    }
                }
                catch (Exception err)
                {
                    Core.Site.RedirectError(Response, err.Message);
                }

                // Заполнение информации о вопросе
                InitInfoQuest();

                // Заполнение списка вопросов в левой панели
                FillQuestions(professorTest);

                // Заполнение списка ответов
                MakeAnswers(professorTest.CurrentQuest);

                ResetOneTimer(); //currQuestTimer.Reset();
            }
            else
            {
                // Заполнение информации о вопросе
                InitInfoQuest();

                // Заполнение списка вопросов в левой панели
                FillQuestions(professorTest);

                // Заполнение списка ответов
                MakeAnswers(professorTest.CurrentQuest);

                LabelMess.Text = "Вам необходимо выбрать вариант ответа!";
                LabelMess.ForeColor = Color.Red;
                messPanel.Visible = true;
            }

            /*MODIFY*/
            if (resetTime)
            {
                Session.Remove("fullTime");
                Session.Remove("oneTime");
            }
            /*End MODIFY*/
            isInsideAction = false;

        }

        protected void buttonPrev_Click(object sender, ImageClickEventArgs e)
        {
            if (isInsideAction)
                return;

            isInsideAction = true;

            if (idQuestion == 0)
                idQuestion = professorTest.CurrentQuest.Question.QuestID;
            else
            {
                if (idQuestion != 0)
                {
                    var questions = Helper.CurrentDB.Questions.Where(t => t.TestID == professorTest.Test.TestID).OrderBy(q => q.QuestNum).ToList();
                    var questCount = questions.Count;

                    for (int i = 0; i < questCount; i++)
                    {
                        if (questions[i].QuestID == idQuestion)
                        {
                            if (i != 0)
                                idQuestion = questions[i - 1].QuestID;
                            break;
                        }
                    }
                }
            }

            Response.Redirect("~/User/Test.aspx?id_test=" + idTest
                + (idBattery != 0 ? "&id_battery=" + idBattery : "")
                + "&from=" + (int)from
                + "&id_question=" + idQuestion
                + "&action=" + (int)ActionType.PrevNavigation);
            return;

            // Заполнение информации о вопросе
            //InitInfoQuest();

            // Заполнение списка вопросов в левой панели
            //FillQuestions(professorTest);

            // Заполнение списка ответов
            //MakeAnswers(professorTest.CurrentQuest);

            //ResetOneTimer();
        }
        private void ResetOneTimer()
        {
            hiddenOneTime.Value = "00:00:00";
            if (Session["oneTime"] != null)
                Session.Remove("oneTime");
        }
        protected void ImageButtonSave_Click(object sender, ImageClickEventArgs e)
        {
            string sfrom = HttpContext.Current.Request.QueryString[Core.Consts.reqFrom];
            from = Core.Converting.ConvertToFromType(sfrom);

            redirectUrl = "~/User/" + Core.Site.GetPageFrom(from);

            Response.Redirect(redirectUrl);
        }
        private void SetEnablePause(bool enable)
        {
            buttonPrev.Enabled = enable;
            buttonNext.Enabled = enable;
            SetEnabledQuest(enable);

            labelPause.Visible = !enable;
        }

        #endregion

        protected void radio_graph_CheckedChanged(object sender, EventArgs e)
        {
            buttonNext_Click(sender, null);
        }

        protected void RadioButtonListQuest_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonNext_Click(sender, null);
        }
    }
}