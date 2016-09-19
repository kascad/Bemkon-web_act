using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Testing.Data;
using Testing.Data.Entities;
using System.Web;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace Interpret
{
    public class Interpretation
    {
        public bool IsProUser = true;

        private Archive.Examinee examinee;
        //private int interpretID;

        public static List<ScaleBall> scaleBalls;

        private StringBuilder resultSB;
        //private loggEngine.fileLog fileLog1 = null;

        //---------- Methods -------------

        public Interpretation(Archive.Examinee examinee)
        {
            this.examinee = examinee;
            //this.interpretID = -1;
            //fileLog1 = new loggEngine.fileLog(AppDomain.CurrentDomain.BaseDirectory + "Temp" + @"\", "log1.txt");
            scaleBalls = null;//new List<ScaleBall>();

            CalculateScaleBalls(false);
        }

        /// <summary>
        /// Calculate scale balls and fill results in scaleBalls fields   
        /// </summary>
        public void CalculateScaleBalls()
        {
            CalculateScaleBalls(true);
        }

        private double getScore(Archive.ExamineeTest testItem, int ScaleID)
        {
            double res = 0;
            List<int> ansIDs = Helper.Helper.getAnsIds(testItem);
            int anscount = ansIDs.Count;
            int i = 0;
            int limit = 100;
            while (i < anscount)
            {
                //var tRes = from p in db.ScaleWeights
                //           where ansIDs.GetRange(i, (anscount - limit > i ? limit : anscount - i)).Contains((int)p.AnsID) && p.ScaleID == ScaleID
                //           select p.Weight.HasValue ? p.Weight : 0;

                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {
                    var tRes = from p in dbRepository.GetScaleWeightForScale(ScaleID)
                               where ansIDs.GetRange(i, (anscount - limit > i ? limit : anscount - i)).Contains((int)p.AnsID)
                               select p.Weight.HasValue ? p.Weight : 0;

                    foreach (var item in tRes)
                    {
                        res += (double)(tRes).Sum(); //(double)(tRes).Sum();
                        break;
                    }
                    i += limit;
                }
            }
            return res;
        }

        public StringBuilder MakeReport()
        {
            StringBuilder res = new StringBuilder();
            foreach (var item in examinee.ExamineeTests)
            {
                if (!item.IsFinished)
                    continue;
                res.Append(MakeReport(item));
            }
            return res;
        }

        private StringBuilder MakeReport(Archive.ExamineeTest testitem)
        {
            return MakeReport(testitem, true);
        }

        private StringBuilder MakeReport(Archive.ExamineeTest testitem, bool needtime)
        {
            StringBuilder newTable = new StringBuilder();
            if (testitem == null)
                return newTable;

            //var gbuilder = new GraphBuilder.GBuilder();

            List<string> scaleShortNames = new List<string>();
            List<string> scaleNames = new List<string>();
            List<double> seriesScale = new List<double>();
            List<double> seriesTime = new List<double>();
            // double minScale, maxScale;
            //double maxTime;
            bool warnRawScales = false; // для вывода предупреждения о сырых значениях

            var uniqId = examinee.Id + "_" + testitem.TestId;

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                var test = dbRepository.GetTest(testitem.TestId);
                var testscales = dbRepository.GetScalesForTest(testitem.TestId);

                if (testscales.Count() > 0)
                {
                    newTable.Append("<div id='tableDiv" + uniqId + "' class='toggleTable'>" + testitem.Name + " таблица значений\r\n");
                    newTable.Append("<table id='table" + uniqId + "' class='tablesorter'><thead><tr>\r\n");
                    newTable.Append("<th>Тест </th>");
                    newTable.Append("<th>Шкала</th>");
                    newTable.Append("<th>Название</th>");
                    newTable.Append("<th>Сырое значение</th>");
                    newTable.Append("<th>Стандартное значение</th>");
                    newTable.Append("<th class='{sorter: false}'>MIN</th>");
                    newTable.Append("<th class='{sorter: false}'>MAX</th>");
                    newTable.Append("<th class='{sorter: false}'>Point0</th>");
                    newTable.Append("<th class='{sorter: false}'>Point1</th>");
                    newTable.Append("<th class='{sorter: false}'>Point2</th>");
                    newTable.Append("<th class='{sorter: false}'>Point3</th>");
                    newTable.Append("<th class='{sorter: false}'>Point4</th>");
                    newTable.Append("<th>Среднее время ответа на один вопрос</th>");
                    newTable.Append("</tr></thead><tbody>");
                    foreach (var itemScale in testscales)
                    {
                        var score = getScore(testitem, itemScale.ScaleID);
                        var correctScore = score;

                        if (itemScale.BallMin.HasValue && itemScale.BallMax.HasValue && itemScale.Point0.HasValue)
                        {
                            correctScore = GetCorrectBall(score, itemScale.BallMin.Value, itemScale.BallMax.Value,
                                itemScale.Point0.Value, itemScale.Point1.Value, itemScale.Point2.Value,
                                itemScale.Point3.Value, itemScale.Point4.Value);
                            warnRawScales = (itemScale.Point0.Value == itemScale.Point4.Value); // если не указаны значения для перевода в стандартные
                        }
                        else
                            warnRawScales = false; // молчим, это SD


                        //gbuilder.Addparam(itemScale.Point0.Value, itemScale.Point4.Value, correctScore, itemScale.ScaleShortName + " - " + itemScale.ScaleName);
                        newTable.Append("<tr>");
                        newTable.Append("<td>" + testitem.Name + "</td>");
                        newTable.Append("<td>" + itemScale.ScaleShortName + "</td>");
                        newTable.Append("<td>" + itemScale.ScaleName + "</td>");
                        newTable.Append("<td>" + Math.Round(score, 2).ToString() + "</td>");
                        newTable.Append("<td>" + Math.Round(correctScore, 2) + "</td>");
                        if (itemScale.BallMin.HasValue)
                        {
                            newTable.Append("<td>" + Math.Round(itemScale.BallMin.Value, 2) + "</td>");
                            newTable.Append("<td>" + Math.Round(itemScale.BallMax.Value, 2) + "</td>");
                        }
                        else
                        {
                            newTable.Append("<td>NULL</td>");
                            newTable.Append("<td>NULL</td>");
                        }
                        if (itemScale.Point0.HasValue)
                        {
                            newTable.Append("<td>" + itemScale.Point0.Value.ToString() + "</td>");
                            newTable.Append("<td>" + itemScale.Point1.Value.ToString() + "</td>");
                            newTable.Append("<td>" + itemScale.Point2.Value.ToString() + "</td>");
                            newTable.Append("<td>" + itemScale.Point3.Value.ToString() + "</td>");
                            newTable.Append("<td>" + itemScale.Point4.Value.ToString() + "</td>");
                        }
                        else
                        {
                            newTable.Append("<td>NULL</td>");
                            newTable.Append("<td>NULL</td>");
                            newTable.Append("<td>NULL</td>");
                            newTable.Append("<td>NULL</td>");
                            newTable.Append("<td>NULL</td>");
                        }

                        bool showAtChart = correctScore > (itemScale.BallMin.HasValue ? itemScale.BallMin.Value : 0.0);
                        if (needtime)
                        {
                            var scaleWeights = dbRepository.GetScaleWeightForScale(itemScale.ScaleID).Select(w => w.AnsID).ToArray();
                            var times = testitem.TestResults.Where(t => scaleWeights.Any(s => s.HasValue && s.Value == t.AnsID));
                            if (times.Any())
                            {
                                TimeSpan scaleTime = new TimeSpan();
                                foreach (var t in times)
                                {
                                    scaleTime += TimeSpan.Parse(t.Time);
                                }
                                newTable.Append("<td>");
                                newTable.Append(scaleTime.ToString());
                                newTable.Append("</td>");

                                if (scaleTime.TotalSeconds > 0 || showAtChart)
                                {
                                    seriesTime.Add(scaleTime.TotalSeconds);
                                    showAtChart = true;
                                }
                            }
                            else
                            {
                                newTable.Append("<td></td>");

                                seriesTime.Add(0);
                            }
                        }
                        else
                        {
                            newTable.Append("<td>" + "</td>");

                        }
                        if (showAtChart)
                        {
                            scaleShortNames.Add(itemScale.ScaleShortName);
                            scaleNames.Add(itemScale.ScaleName + " (" + itemScale.ScaleShortName + ")");
                            seriesScale.Add(Math.Round(correctScore * 100) / 100);
                        }
                        newTable.Append("</tr>");


                    }
                    newTable.Append("</tbody></table>\r\n");
                    newTable.Append("</div>\r\n");
                }
            }

            //gbuilder.MakeGraph();
            //var graphImgHtml = "<img class='img_result' src='../" + gbuilder.GraphPath + "'/><br/><br/>\r\n";           
            //newTable.Append("<br/><br/>");
            //newTable.Append("<div id='imageDiv' class='toggleImg'>\r\n");
            //newTable.Append(graphImgHtml);
            //newTable.Append("</div>\r\n");

            if (seriesScale.Any())
            {
                DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart_" + uniqId)
                    .InitChart(new Chart
                    {
                        Type = ChartTypes.Bar,
                        Height = Math.Max(seriesScale.Count() * 50, 300)
                    })
                    .SetTitle(new Title { Text = testitem.Name })
                    .SetPlotOptions(new PlotOptions
                    {
                        Bar = new PlotOptionsBar
                        {
                            DataLabels = new PlotOptionsBarDataLabels { Enabled = true }
                        }
                    })
                    .SetCredits(new Credits()
                    {
                        Enabled = false
                    })
                    .SetXAxis(new XAxis
                    {
                        Categories = scaleNames.ToArray(),
                        Title = null
                    })
                    .SetYAxis(new YAxis
                    {
                        Title = new YAxisTitle { Text = warnRawScales ? "\"Сырые\" значения шкал" : "Значения шкал", Align = AxisTitleAligns.High }
                    })
                    .SetSeries(new Series[]
                    {
                        new Series
                        {
                            Name = "Время",
                            Data = new Data(seriesTime.Cast<object>().ToArray()),
                            Color = Color.LightGreen
                        },
                        new Series
                        {
                            Name = warnRawScales ? "Шкалы (\"сырые\" значения)" : "Шкалы",
                            Data = new Data(seriesScale.Cast<object>().ToArray()),
                            Color = Color.MediumBlue
                        }
                    });

                string chartRes = chart.ToHtmlString();
                newTable.Append(chartRes);
            }

            return newTable;
        }

        public static double GetCorrectBall(double bal, int ScaleId)
        {
            //return (float)bal;
            //var scale = db.Scales.First(s => s.ScaleID == ScaleId);
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                var scale = dbRepository.GetScale(ScaleId);
                if (scale.BallMin.HasValue && scale.Point0.HasValue)
                {
                    double min = scale.BallMin.HasValue ? scale.BallMin.Value : 0.0;
                    double max = scale.BallMax.HasValue ? scale.BallMax.Value : 0.0;
                    double point0 = (scale.Point0.HasValue ? scale.Point0.Value : 0.0),
                           point1 = (scale.Point1.HasValue ? scale.Point1.Value : 0.0),
                           point2 = (scale.Point2.HasValue ? scale.Point2.Value : 0.0),
                           point3 = (scale.Point3.HasValue ? scale.Point3.Value : 0.0),
                           point4 = (scale.Point4.HasValue ? scale.Point4.Value : 0.0);

                    return GetCorrectBall(bal, min, max, point0, point1, point2, point3, point4);
                }
                return bal; // не совсем верно, иначе падает
            }
        }

        public static double GetCorrectBall(double bal, double min, double max,
            double point0, double point1, double point2, double point3, double point4)
        {
            // сам код алгоритму
            if ((point1 > point0 && point2 > point1 && point3 > point2 && point4 > point3) && max > min)
            {
                if (bal < min)
                    bal = min;
                else if (bal > max)
                    bal = max;

                double distance = (max - min) / 4;
                var mas1 = new List<double> { min, min + distance, min + distance * 2, min + distance * 3, max };
                var mas2 = new double[] { point0, point1, point2, point3, point4 };

                var correctBall = point0;
                if (bal > min)
                {
                    var k = mas1.FindIndex(t => t >= bal) - 1;
                    var koef = (bal - mas1[k]) / (mas1[k + 1] - mas1[k]);
                    correctBall = mas2[k] + koef * (mas2[k + 1] - mas2[k]);
                }
                return correctBall;
            }
            else
            {
                return bal;
            }
        }

        public void CalculateScaleBalls(bool force)
        {

            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            if (examinee == null)
                return;

            if (examinee.ExamineeTests == null)
                examinee.GetTest(0);

            if (scaleBalls == null)
                return;

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                foreach (var testItem in examinee.ExamineeTests)
                {
                    //var scales = from s in db.Scales
                    //             where s.TestID == testItem.TestId
                    //             select s.ScaleID;
                    if (!testItem.IsFinished) // выбрасываем незаконченные тесты
                        continue;

                    var scales = from s in dbRepository.GetScalesForTest(testItem.TestId) select s.ScaleID;

                    List<int> ansIDs = Helper.Helper.getAnsIds(testItem);
                    //List<int> askIDs = Helper.Helper.getAnsIds(testItem);
                    foreach (int itemScale in scales)
                    {
                        double res = 0;
                        //double askres = 0;
                        //int ansid = 0;
                        {
                            if (force)
                            {
                                //var tRes = from p in db.ScaleWeights
                                //           where ansIDs.Contains((int) p.AnsID) && p.ScaleID == itemScale
                                //           select p.Weight.HasValue ? p.Weight : 0;

                                var tRes = from p in dbRepository.GetScaleWeightForScale(itemScale)
                                           where ansIDs.Contains((int)p.AnsID)
                                           select p.Weight.HasValue ? p.Weight : 0;


                                foreach (var item in tRes)
                                {
                                    //if (!force && i)
                                    res += Interpretation.GetCorrectBall((double)(tRes).Sum(), (int)itemScale);
                                    //(double)(tRes).Sum();
                                    break;
                                }
                            }
                            else
                            {
                                //var tRes = from p in db.ScaleWeights
                                //           where ansIDs.Contains((int) p.AnsID) && p.ScaleID == itemScale
                                //           orderby p.AnsID
                                //           select new {p.Weight, p.AnsID};

                                var tRes = from p in dbRepository.GetScaleWeightForScale(itemScale)
                                           where ansIDs.Contains((int)p.AnsID)
                                           orderby p.AnsID
                                           select new { p.Weight, p.AnsID };


                                for (int i = 0; i < ansIDs.Count; i++)
                                {
                                    scaleBalls.Add(new ScaleBall(itemScale, 0, testItem.TestId, ansIDs[i]));
                                }
                                foreach (var item in tRes)
                                {
                                    res = Interpretation.GetCorrectBall((double)(item.Weight.HasValue ? item.Weight : 0), (int)itemScale);
                                    //(double)(item.Weight.HasValue ? item.Weight : 0);
                                    //if (ansid==0)
                                    //    ansid = (int)item.AnsID;
                                    if (item.AnsID.HasValue && item.AnsID > 0)
                                    {

                                        Helper.Helper.setBall(scaleBalls, itemScale, testItem.TestId, (int)item.AnsID, res);
                                        //scaleBalls.Add(new ScaleBall(itemScale, res, testItem.TestId, ansid));
                                        //res = 0;
                                        //ansid = (int)item.AnsID;
                                    }
                                    //else 

                                }
                                //scaleBalls.Add(new ScaleBall(itemScale, res, testItem.TestId, ansid));
                            }
                        }
                        if (force)
                            //if (scaleBalls.Count(cond => cond.ScaleID == itemScale) == 0)
                            scaleBalls.Add(new ScaleBall(itemScale, res, testItem.TestId));
                    }
                }
            }
        }
        public object GetRule(string ruleTxt, int ruleID, int test)
        {
            return GetRule(ruleTxt, ruleID);

        }
        private Rules.Rule GetRule(string ruleTxt, int ruleID)
        {

            if (Rules.Rule_04.isItRule(ruleTxt))
                return new Rules.Rule_04(ruleTxt, resultSB, examinee);
            if (Rules.Rule_02.isItRule(ruleTxt))
                return new Rules.Rule_02(ruleTxt, resultSB, examinee);
            if (Rules.Rule_03.isItRule(ruleTxt))
                return new Rules.Rule_03(ruleTxt, resultSB, examinee);
            if (Rules.Rule_10.isItRule(ruleTxt))
                return new Rules.Rule_10(ruleTxt, resultSB, examinee, ruleID);
            if (Rules.Rule_71.isItRule(ruleTxt))
                return new Rules.Rule_71(ruleTxt, resultSB, examinee, ruleID);
            if (Rules.Rule_72.isItRule(ruleTxt))
                return new Rules.Rule_72(ruleTxt, resultSB, examinee, ruleID);
            if (Rules.Rule_73.isItRule(ruleTxt))
                return new Rules.Rule_73(ruleTxt, resultSB, examinee, ruleID);

            Trace.TraceWarning("Unknown or ignored interpretation rule: " + ruleTxt, "Warning: Unknown interpretation rule");
            return null;
        }

        public void AnalizeInterpretRules(int interpretID)
        {
            System.Diagnostics.Debug.WriteLine("****************** FUUUUCK TI ID:" + interpretID);
            if (interpretID == 18)
            {
                System.Diagnostics.Debug.WriteLine("****************** TRACE");
            }
            resultSB = new StringBuilder();

            List<ruleInfo> rules = new List<ruleInfo>();
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                var rul_l = from d in dbRepository.GetInterpretRules(interpretID)
                            select new { d.RuleID, d.RuleText, d.CON_TXT_N };

                foreach (var item in rul_l)
                {
                    if (item == null || item.RuleText == null) // skip empty interpret rules
                        continue;

                    ruleInfo r1 = new ruleInfo
                    {
                        ruleID = Convert.ToInt32(item.RuleID),
                        ruleTxt = item.RuleText,
                        CON_TXT_N = (item.CON_TXT_N.HasValue ? item.CON_TXT_N.Value : 0)

                    };
                    rules.Add(r1);
                }

            }
            ///----- нужно для работа без теста

            List<string> results = new List<string>();
            StringBuilder tempResult;
            int nextrule = 0;
            int k = 0;
            string rulelist = (examinee.Tests.Trim() != "" ? examinee.Tests.Trim() : Helper.Helper.getTestString(examinee));
            foreach (var rule in rules)
            {
                if (Helper.Helper.isInRules(rulelist, rule.ruleTxt))//&& rule.ruleTxt.Contains("-72"))
                {
                    if (nextrule != 0 && nextrule != rule.CON_TXT_N)
                    {
                        nextrule = 0;
                        continue;
                    }
                    nextrule = 0;
                    Rules.Rule r = GetRule(rule.ruleTxt, rule.ruleID);
                    if (r != null)
                    {
                        if (IsProUser)
                            r.SetDumpAnswers();

                        r.Run();
                        k++;
                        tempResult = r.getResult();
                        if (tempResult != null && tempResult.ToString().Trim().Length > 0 && Helper.Helper.getParameterValue(tempResult.ToString(), "task") != "")//выполняем задачу
                        {
                            string task = Helper.Helper.getParameterValue(tempResult.ToString(), "task");
                            if (task == "remove")
                            {
                                if (results.Count > 0)
                                    results.RemoveAt(results.Count - 1);
                            }
                            else
                                if (task.Split('=').Length == 2)
                            {
                                if (task.Split('=')[0] == "goto")
                                    nextrule = Convert.ToInt32(task.Split('=')[1]);
                            }
                        }
                        else if (tempResult != null && tempResult.ToString().Trim().Length > 0)//получаем данные
                        {
                            string ruleCode = ""; // empty for usual user
                            if (IsProUser)
                            {
                                string ruleForRead = rule.ruleTxt;
                                ruleForRead = ruleForRead.Replace("\u0010", "►");
                                ruleForRead = ruleForRead.Replace("\u0011", "◄");
                                ruleCode = "<p class='rule_id'>(№ " + rule.ruleID.ToString() + " - " + ruleForRead + ")</p>";
                            }
                            results.Add(tempResult.ToString() + ruleCode);
                        }
                    }
                }
            }

            resultSB.Remove(0, resultSB.Length);
            resultSB.Append(string.Join("", results.ToArray()));
        }

        public string Result
        {
            get
            {
                if (resultSB != null)
                    return resultSB.ToString();
                else
                    return null;
            }
        }

        public List<ScaleBall> ScaleBalls
        {
            get { return scaleBalls; }
        }
    }
    public class ruleInfo
    {
        public string ruleTxt;
        public int ruleID;
        public int CON_TXT_N;
    }
}
