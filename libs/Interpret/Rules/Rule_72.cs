using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shared;
using Testing.Data;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Drawing;


namespace Interpret.Rules
{
    class Rule_72 : Rule
    {

        public Rule_72(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        public Rule_72(string ruleTxt, StringBuilder resultSB, Archive.Examinee examinee, int ruleID) : base(ruleTxt, resultSB, examinee, ruleID) { }
        public static bool isItRule(string ruleTxt)
        {
            return isItRule(ruleTxt, null);
        }
        private StringBuilder tResultSB = null;

        List<string> OtherScaleName = new List<string>();
        List<double> OtherScaleIndex = new List<double>();
        List<string> OtherScaleName2 = new List<string>();
        List<double> OtherScaleIndex2 = new List<double>();
        List<double> otherScaleIndex2 = new List<double>();
        //List<double> Other2ScaleIndex = new List<double>();
        List<int> StartIndex = new List<int>();
        List<string> ScaleNameS = new List<string>();
        List<double> ScaleIndexeS = new List<double>();
        List<WeightQuiestons> scalesQuestions = new List<WeightQuiestons>();
        List<WeightQuiestons> weightQuiestons = new List<WeightQuiestons>();
        List<double> WeightQuiestons2 = new List<double>();
        List<Charts.percentil> s1 = new List<Charts.percentil>();
        List<Charts.percentil> s2 = new List<Charts.percentil>();
        List<Charts.percentil> p1 = new List<Charts.percentil>();
        List<Charts.percentil> p2 = new List<Charts.percentil>();
        //List<Charts.percentilxy> l = new List<Charts.percentilxy>();
        //List<Charts.percentilxy> l1 = new List<Charts.percentilxy>();
        //List<string> hc1 = new List<string>();
        //List<string> hc2 = new List<string>();
        //List<double> hcdata1 = new List<double>();
        //List<double> hcdata2 = new List<double>();
        Archive.ExamineeTest testItem = null;
        //double expInf = 0.0000000000001;
        /*
        public class SortByValue : IComparer<Charts.percentil>
        {
            int Compare(Charts.percentil ob1, Charts.percentil ob2)
            {
                Charts.percentil m1 = (Charts.percentil)ob1;
                Charts.percentil m2 = (Charts.percentil)ob2;
                if (m1.value > m2.value) return 1;
                else if (m1.value < m2.value) return -1;
                else return 0;
            }
        }
        */
        public bool isItRule(string ruleTxt, StringBuilder resultSB, int tt)
        {
            return isItRule(ruleTxt, resultSB);
        }
        public static bool isItRule(string ruleTxt, StringBuilder resultSB)
        {
            ruleTxt = Helper.Helper.prepareRule(ruleTxt, 1);
            //bool test = isMultipleRule(ruleTxt, resultSB);
            bool test = testRule(ruleTxt, resultSB);
            return test;
        }
        private static bool testRule(string ruleTxt, StringBuilder resultSB)
        {
            //«-04@TestShortName@QuestNum  @Text», или «#@TestShortName @QuestNum @Text»
            string pattern = @"^[-#]{1}72" + tSpacer + tTestShortName;
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                //Helper.Helper.addParameterValue(resultSB, "format", "1");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"^#SL-T" + tSpacer + "S" + tTestShortName;
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1 && matches[0].Groups.Count == 2 && matches[0].Groups["TestShortName"].Length == 3)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    }
                    pattern = @"(?<casd>S" + @"\ *" + tScaleShortName + @")";//сканируем на группы
                    VRegExp = new Regex(pattern);
                    matches = VRegExp.Matches(text);
                    //if (matches.Count >= 2)
                    //{
                    int ik = 1;
                    int lastlen = 0;
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "testcount", matches.Count.ToString());
                    }
                    foreach (Match match in matches)
                    {
                        lastlen = match.Index + match.Length;
                        if (resultSB != null)
                        {
                            Helper.Helper.addParameterValue(resultSB, "ScaleShortName" + ik.ToString(), match.Groups["ScaleShortName"].Value);
                        }
                        ik++;
                    }
                    if (lastlen < text.Length)
                        return false;
                    return true;
                }
            }
            else
                if (matches.Count == 1 && matches[0].Groups.Count == 2 && matches[0].Groups["TestShortName"].Length == 3)
            {
                if (resultSB != null)
                {
                    Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                }
                pattern = @"(?<casd>" + @"\ *" + tScaleShortName + @")";//сканируем на группы
                VRegExp = new Regex(pattern);
                text = text.Substring(matches[0].Length);
                matches = VRegExp.Matches(text);
                //if (matches.Count >= 2)
                {
                    int ik = 1;
                    int lastlen = 0;
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "testcount", matches.Count.ToString());
                    }
                    foreach (Match match in matches)
                    {
                        lastlen = match.Index + match.Length;
                        if (resultSB != null)
                        {
                            Helper.Helper.addParameterValue(resultSB, "ScaleShortName" + ik.ToString(), match.Groups["ScaleShortName"].Value);
                        }
                        ik++;
                    }
                    if (lastlen < text.Length)
                        return false;
                    return true;
                }
                //return true;
            }
            return false;
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }


        public void fillScaleNames()
        {
            for (int i = 1; i <= 4; i++)
            {
                ScaleNameS.Add(Helper.Helper.getParameterValue(this.tResultSB.ToString(), "ScaleShortName" + i.ToString()));
                ScaleIndexeS.Add(0);
            }
        }
        private bool checkScale(string scalename, int ansid)
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {

                //int cc = db.ScaleWeights.Count(s => (s.Scale.ScaleShortName == scalename && s.AnsID == ansid));

                var scales = (from s in dbRepository.GetScalesAll()
                              where s.ScaleShortName == scalename
                              select s.ScaleID).ToList();
                var cc = dbRepository.GetScaleWeightForScaleAndAnswer(scales, new List<int> { ansid }).Count;
                return (cc > 0);
            }
        }
        private void fillQuestIndexes()
        {
            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            if (testItem == null)
                testItem = getTest(TestShortName);
            Archive.TestResult tr = null;
            if (testItem.TestResults.Count > 0)
                tr = testItem.TestResults[0];
            else return;
            for (int i = 0; i < ScaleNameS.Count; i++)
            {
                StartIndex.Add(0);
            }
            //int index=0;
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            //int questCount = testItem.TestResults.Count / db.Scales.Count(s => s.TestID == testItem.TestId);

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                var scaleCount = dbRepository.GetScalesForTest(testItem.TestId).Count();

                if (scaleCount == 0)
                    return;

                int questCount = testItem.TestResults.Count / scaleCount;

                //for (int j = 0; j < ScaleNameS.Count; j++)
                {
                    //index = 0;
                    for (int i = 0; i < ScaleNameS.Count; i++)
                        for (int index = 0; index < testItem.TestResults.Count; index += questCount)
                        {
                            if (index < testItem.TestResults.Count)
                                tr = testItem.TestResults[index];
                            if (checkScale(ScaleNameS[i], tr.AnsID))
                            {
                                StartIndex[i] = index;
                                break;
                                //index += questCount;

                            }
                            //if (index < questCount * ScaleNameS.Count)
                            //    tr = testItem.TestResults[index];
                        }
                }
            }
        }
        private void fillOtherScaleIndex(string baseScaleName, string oppositeScaleName, List<string> otherScaleName, List<double> otherScaleIndex, int level)
        {

            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            if (testItem == null)
                testItem = getTest(TestShortName);
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {

                var scales = dbRepository.GetScalesForTest(testItem.TestId).Where(s => s.ScaleShortName != baseScaleName
                                                 && s.ScaleShortName != oppositeScaleName);

                if (otherScaleName == null)
                    otherScaleName = new List<string>();
                if (otherScaleIndex == null)
                    otherScaleIndex = new List<double>();
                if (otherScaleIndex2 == null)
                    otherScaleIndex2 = new List<double>();
                try
                {

                    //Шаг 2 считаем индексы 03-16 не основных шкал

                    List<double> testScaleIndex1 = new List<double>();
                    List<double> testScaleIndex2 = new List<double>();


                    foreach (var oScale in scales)
                    {
                        //System.Diagnostics.Debug.WriteLine("##################");
                        //System.Diagnostics.Debug.WriteLine("oScale Start:" + oScale.ScaleName);
                        double Other1ScaleIndex = 0;
                        double Other2ScaleIndex = 0;
                        var itemScales = scalesQuestions.Where(sc => sc.name == oScale.ScaleName);
                        //System.Diagnostics.Debug.WriteLine("oScale Middle:" + sc);
                        //System.Diagnostics.Debug.WriteLine("oScale Middle:" + itemScales);
                        int i = level;
                        //int j = weightQuiestons.Count / 2;
                        int k = weightQuiestons.Count / 2;
                        foreach (var itemScale in itemScales)
                        {

                            int j = i + weightQuiestons.Count / 2;

                            if (i < k)
                            {
                                WeightQuiestons itemWeight = weightQuiestons[i];
                                Other1ScaleIndex += itemScale.ball * itemWeight.ball;
                                WeightQuiestons itemWeight2 = weightQuiestons[j];
                                Other2ScaleIndex += itemScale.ball * itemWeight2.ball;
                            }

                            i++;

                        }
                        otherScaleName.Add(oScale.ScaleName);
                        //System.Diagnostics.Debug.WriteLine("oScale Exit:" + oScale.ScaleName);
                        otherScaleIndex.Add(Other1ScaleIndex);
                        otherScaleIndex2.Add(Other2ScaleIndex);
                    }
                }


                catch (Exception err)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR:" + err.Message);
                }


            }
        }

        private void fillOtherScaleIndex2(string baseScaleName, string oppositeScaleName, List<string> otherScaleName2, List<double> otherScaleIndex2, int level)
        {

            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            if (testItem == null)
                testItem = getTest(TestShortName);
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {

                var scales = dbRepository.GetScalesForTest(testItem.TestId).Where(s => s.ScaleShortName != baseScaleName
                                                 && s.ScaleShortName != oppositeScaleName);

                if (otherScaleName2 == null)
                    otherScaleName2 = new List<string>();
                //if (otherScaleIndex == null)
                //    otherScaleIndex = new List<double>();
                if (otherScaleIndex2 == null)
                    otherScaleIndex2 = new List<double>();
                try
                {

                    //Шаг 2 считаем индексы 03-16 не основных шкал

                    List<double> testScaleIndex1 = new List<double>();
                    List<double> testScaleIndex2 = new List<double>();


                    foreach (var oScale in scales)
                    {
                        //System.Diagnostics.Debug.WriteLine("##################");
                        //System.Diagnostics.Debug.WriteLine("oScale Start:" + oScale.ScaleName);
                        double Other1ScaleIndex = 0;
                        double Other2ScaleIndex = 0;
                        var itemScales = scalesQuestions.Where(sc => sc.name == oScale.ScaleName);
                        //System.Diagnostics.Debug.WriteLine("oScale Middle:" + sc);
                        //System.Diagnostics.Debug.WriteLine("oScale Middle:" + itemScales);
                        int i = level;
                        //int j = weightQuiestons.Count / 2;
                        int k = weightQuiestons.Count / 2;
                        foreach (var itemScale in itemScales)
                        {

                            int j = i + weightQuiestons.Count / 2;

                            if (i < k)
                            {
                                WeightQuiestons itemWeight = weightQuiestons[i];
                                Other1ScaleIndex += itemScale.ball * itemWeight.ball;
                                WeightQuiestons itemWeight2 = weightQuiestons[j];
                                Other2ScaleIndex += itemScale.ball * itemWeight2.ball;
                            }

                            i++;

                        }
                        otherScaleName2.Add(oScale.ScaleName);
                        otherScaleIndex2.Add(Other2ScaleIndex);
                    }
                }


                catch (Exception err)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR:" + err.Message);
                }

            }
        }


        private int getAnsCount(Archive.ExamineeTest testItem)
        {
            return getAnsCount(testItem, false);
        }
        private int getAnsCount(Archive.ExamineeTest testItem, bool mode)
        {
            Archive.TestResult item = item = testItem.TestResults[0];
            int QuestID = item.QuestID;
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                if (mode)
                {
                    //var anses = from p in db.Answers
                    //            where p.QuestID == QuestID
                    //            orderby p.AnsNum
                    //            select p.AnsID;

                    var anses = from p in dbRepository.GetQuestionAnswers(QuestID)
                                orderby p.AnsNum
                                select p.AnsID;

                    //var ScaleId = from p in db.Scales
                    //              where p.ScaleShortName == ScaleNameS[0]
                    //              select p.ScaleID;


                    var ScaleId = from p in dbRepository.GetScalesAll()
                                  where p.ScaleShortName == ScaleNameS[0]
                                  select p.ScaleID;

                    List<int> scaleids = new List<int>();
                    foreach (var sid in ScaleId)
                    {
                        scaleids.Add(sid);
                    }
                    List<int> ansesInt = new List<int>();
                    foreach (var ansItem in anses)
                    {
                        ansesInt.Add(ansItem);
                    }

                    //var scaleBalls = from p in db.ScaleWeights
                    //                 where ansesInt.Contains((int)p.AnsID) && scaleids.Contains((int)p.ScaleID)
                    //                 select new { p.AnsID, p.Weight };

                    var scaleBalls = from p in dbRepository.GetScaleWeightForScaleAndAnswer(scaleids, ansesInt)
                                     select new { p.AnsID, p.Weight };

                    return scaleBalls.Count();
                }
                else
                {
                    //int anscount = db.Answers.Count(an => an.QuestID == QuestID);

                    int anscount = dbRepository.GetQuestionAnswers(QuestID).Count();

                    return anscount;
                }
            }
        }
        public void WriteTestHeading()
        {
            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            resultSB.Append("<h1>");
            resultSB.Append("«РЕЗУЛЬТАТ ИНТЕРПРЕТАЦИИ» " + TestShortName);
            resultSB.Append("</h1>");
        }

        public void DumpTestAnswers()
        {
            Helper.Helper.fillScaleNames(Testing.Data.DbRepositoryFactory.GetRepository(), Interpretation.scaleBalls);
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            if (testItem == null)
                testItem = getTest(TestShortName);
            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"1\" width=\"700px\">");
            resultSB.Append("<tr><td nowrap=\"nowrap\" calspan=\"4\">1)</td></tr>");
            resultSB.Append("<tr><td  nowrap=\"nowrap\" colspan=\"4\">" + examinee.Name + "</td></tr>");
            resultSB.Append("<tr BGCOLOR=\"lightgray\"><td nowrap=\"nowrap\">КЛИЕНТ</td><td >ШКАЛА</td><td >ВОПРОС</td><td >ВЫБОР ИССЛЕДУЕМОГО</td></tr>");
            double scaleWeight;
            string scaleName = "";

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                foreach (var testResult in testItem.TestResults)
                {
                    int QuestID = testResult.QuestID;

                    var Answers = (from p in dbRepository.GetQuestionAnswers(QuestID)
                                   orderby p.AnsNum
                                   select p.AnsID).ToList();

                    //var ScaleWeights = from p in db.ScaleWeights
                    //                   where Answers.Contains((int) p.AnsID)
                    //                   select new {p.AnsID, p.Weight, p.Scale.ScaleName, p.ScaleID};

                    var scales = dbRepository.GetScalesAll();

                    var ScaleWeights = from p in dbRepository.GetScaleWeightForAnswers(Answers)
                                       let scale = scales.FirstOrDefault(t => t.ScaleID == p.ScaleID)
                                       let scleName = scale != null ? scale.ScaleName : string.Empty
                                       select new { p.AnsID, p.Weight, ScaleName = scleName, p.ScaleID };

                    scaleWeight = 0;
                    scaleName = "";
                    foreach (var itemScaleBall in ScaleWeights)
                    {
                        if (testResult.AnsID == itemScaleBall.AnsID)
                        {
                            scaleName = itemScaleBall.ScaleName;
                            scaleWeight +=
                                (double)
                                Interpretation.GetCorrectBall((double)itemScaleBall.Weight, (int)itemScaleBall.ScaleID);
                            //(double)itemScaleBall.Weight;
                        }
                    }
                    if (scaleName != "")
                    {
                        resultSB.Append("<tr><td>XXX</td><td>" + scaleName + "</td><td >" + testResult.QuestText +
                                        "</td><td >" + scaleWeight + "</td></tr>");
                        scalesQuestions.Add(new WeightQuiestons(scaleName, scaleWeight, QuestID));
                    }
                }
            }
            resultSB.Append("</table>");
            resultSB.Append("</p>");
        }

        public void CalcAndWriteWeightCoeffs(string baseScaleName, string oppositeScaleName, ref double baseScaleIndex, ref double oppositeScaleIndex, int baseIndex, int oppositeIndex)
        {

            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            resultSB.Append("<p class='rule_conseq'>");

            //int maxcount = 0;

            if (tResultSB != null)
            {
                resultSB.Append("<table border=\"1\" width=\"700px\">");

                string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
                if (testItem == null)
                    testItem = getTest(TestShortName);
                int ansCount = getAnsCount(testItem);
                resultSB.Append("<tr><td nowrap=\"nowrap\" colspan=\"" + (ansCount + 2).ToString() + "\">" +
                                "2) ВЕСОВЫЕ КОЭФФИЦИЕНТЫ ЗНАЧИМОСТИ СВОЙСТВ в ПРОСТРАНСТВЕ " + baseScaleName +
                                (oppositeScaleName.Length > 0 ? " - " + oppositeScaleName : "") +
                                "</td><td>сколько частей занимает свойство в пространстве ИССЛЕДУЕМОГО</td><td>сколько частей занимает само свойство</td><td>Весовой коэффициент каждого свойства</td></tr>");

                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {

                    //var scale =
                    //    dbRepository.GetScalesAll().First(s => s.ScaleShortName == baseScaleName || s.ScaleShortName == oppositeScaleName);

                    if (testItem != null && baseScaleName != "")
                    {
                        int questCount = testItem.TestResults.Count / dbRepository.GetScalesForTest(testItem.TestId).Count();
                        Archive.TestResult item = null;
                        double fstc;
                        double resultFstc;
                        for (int i = 0; i < questCount; i++)
                        {
                            fstc = 0;
                            for (int counter = 0; counter < 2; counter++)
                            {
                                item = testItem.TestResults[i + (counter % 2 == 0 ? baseIndex : oppositeIndex)];
                                int QuestID = item.QuestID;

                                var anses = from p in dbRepository.GetQuestionAnswers(QuestID)
                                            orderby p.AnsNum
                                            select p.AnsID;

                                var ScaleId = from p in dbRepository.GetScalesAll()
                                              where
                                                  p.ScaleShortName ==
                                                  ((counter) % 2 == 0 || oppositeScaleName == "" ||
                                                   oppositeScaleName == "00"
                                                       ? baseScaleName
                                                       : oppositeScaleName)
                                              select p.ScaleID;

                                List<int> scaleids = new List<int>();
                                foreach (var sid in ScaleId)
                                {
                                    scaleids.Add(sid);
                                }
                                List<int> ansesInt = new List<int>();
                                foreach (var ansItem in anses)
                                {
                                    ansesInt.Add(ansItem);
                                }

                                //var scaleBalls = from p in db.ScaleWeights
                                //                 where
                                //                     ansesInt.Contains((int) p.AnsID) &&
                                //                     scaleids.Contains((int) p.ScaleID)
                                //                 select new {p.AnsID, p.Weight, p.ScaleID};

                                var scaleBalls = from p in dbRepository.GetScaleWeightForScaleAndAnswer(scaleids, ansesInt)
                                                 select new { p.AnsID, p.Weight, p.ScaleID };

                                resultSB.Append("<tr>");
                                if (Convert.ToInt32(counter) % 2 == 0 || oppositeScaleName == "" ||
                                    oppositeScaleName == "00")
                                    resultSB.Append("<td nowrap=\"nowrap\">" + baseScaleName + "</td>");
                                else
                                    resultSB.Append("<td nowrap=\"nowrap\">" + oppositeScaleName + "</td>");
                                int c = 0;
                                foreach (var itemScaleBall in scaleBalls)
                                {
                                    c++;
                                    if ((Convert.ToInt32(counter)) % 2 == 0 || oppositeScaleName == "" ||
                                        oppositeScaleName == "00")
                                    {
                                        //red
                                        resultSB.Append("<td nowrap=\"nowrap\">" +
                                                        (item.AnsID != itemScaleBall.AnsID
                                                             ? itemScaleBall.Weight.ToString() + "</td>"
                                                             : "<b style=\"color:red;\">" +
                                                               itemScaleBall.Weight.ToString() + "</b></td>"));
                                        if (item.AnsID == itemScaleBall.AnsID)
                                        {
                                            baseScaleIndex +=
                                                (double)
                                                Interpretation.GetCorrectBall((double)itemScaleBall.Weight,
                                                                              (int)itemScaleBall.ScaleID);
                                            //(double)itemScaleBall.Weight;
                                            fstc = Convert.ToDouble(item.AnsNumber);
                                        }
                                    }
                                    else if (oppositeScaleName != "" && oppositeScaleName != "00")
                                    {
                                        //blue
                                        resultSB.Append("<td nowrap=\"nowrap\">" +
                                                        (item.AnsID != itemScaleBall.AnsID
                                                             ? itemScaleBall.Weight.ToString() + "</td>"
                                                             : "<b style=\"color:blue;\">" +
                                                               itemScaleBall.Weight.ToString() + "</b></td>"));
                                        if (item.AnsID == itemScaleBall.AnsID)
                                        {
                                            oppositeScaleIndex +=
                                                (double)
                                                Interpretation.GetCorrectBall((double)itemScaleBall.Weight,
                                                                              (int)itemScaleBall.ScaleID);
                                            //(double)itemScaleBall.Weight;
                                            fstc -= Convert.ToDouble(c);
                                        }
                                    }
                                }
                                //if (maxcount < c)
                                //    maxcount = c;
                                if (c < ansCount)
                                    for (int jj = c; jj < ansCount; jj++)
                                    {
                                        resultSB.Append("<td nowrap=\"nowrap\">0</td>");
                                    }
                                resultSB.Append("<td >" + item.QuestText + "</td>");
                                if ((Convert.ToInt32(counter)) % 2 == 0)
                                {
                                    resultSB.Append(
                                        "<td BGCOLOR=\"yellow\" rowspan=\"2\" nowrap=\"nowrap\">{$fstc}</td>");
                                    resultSB.Append(
                                        "<td BGCOLOR=\"yellow\" rowspan=\"2\" nowrap=\"nowrap\">{$scndc}</td>");
                                    resultSB.Append(
                                        "<td BGCOLOR=\"yellow\" rowspan=\"2\" nowrap=\"nowrap\">{$thrdc}</td>");
                                }
                                else
                                {
                                    resultSB.Replace("{$fstc}", (Math.Abs(fstc) + 1).ToString());
                                    resultSB.Replace("{$scndc}", (ansCount).ToString());
                                    resultSB.Replace("{$thrdc}", ((Math.Abs(fstc) + 1) / (ansCount)).ToString("N2"));
                                    weightQuiestons.Add(new WeightQuiestons("", ((Math.Abs(fstc) + 1) / (ansCount)),
                                                                            QuestID));
                                    resultFstc = ((Math.Abs(fstc) + 1) / (ansCount));

                                }
                                resultSB.Append("</tr>");

                            }
                        }
                    }

                }
                resultSB.Append("</table>");
                resultSB.Append("</p>");
            }
        }

        double getBallFromOther(List<string> scn, List<double> sci, string scalename)
        {
            for (int i = 0; i < sci.Count; i++)
            {
                if (scn[i] == scalename)
                    return sci[i];


            }
            return 0;
        }
        void CalcAndWriteScalesSpace()
        {

            StringBuilder sb1 = new StringBuilder();

            /*
            for (int i = 0; i < OtherScaleIndex.Count + 2; i++)
            {
                sb1.Append("<tr><td nowrap=\"nowrap\">{scalename." + i + "}</td><td nowrap=\"nowrap\">{scaleindex01-02." + i + "}</td><td nowrap=\"nowrap\">{scaleindex03-04." + i + "}</td></tr>");
            }
            */
            List<string> scn1 = new List<string>();
            List<double> sci1 = new List<double>();
            scn1.Add(getFullScaleName(ScaleNameS[0])); scn1.Add(getFullScaleName(ScaleNameS[1]));
            sci1.Add(ScaleIndexeS[0]); sci1.Add(ScaleIndexeS[1]);
            for (int i = 0; i < OtherScaleIndex.Count; i++)
            {
                scn1.Add(OtherScaleName[i]);
                sci1.Add(OtherScaleIndex[i]);
            }

            List<string> scn2 = new List<string>();
            List<double> sci2 = new List<double>();
            scn2.Add(getFullScaleName(ScaleNameS[2])); scn2.Add(getFullScaleName(ScaleNameS[3]));
            sci2.Add(ScaleIndexeS[2]); sci2.Add(ScaleIndexeS[3]);
            for (int i = 0; i < OtherScaleIndex2.Count; i++)
            {
                scn2.Add(OtherScaleName2[i]);
                sci2.Add(OtherScaleIndex2[i]);
            }

            for (int i = 0; i < scn1.Count; i++)
            {
                s1.Add(new Charts.percentil(scn1[i], Math.Round(sci1[i], 2)));
            }

            for (int i = 0; i < scn2.Count; i++)
            {
                s2.Add(new Charts.percentil(scn2[i], Math.Round(sci2[i], 2)));
            }

            s1.Sort();
            s1.Reverse();
            s2.Sort();
            s2.Reverse();
/*            
            for (int i = 0; i < scn1.Count; i++)
            {
                sb1 = sb1.Replace("{scalename." + i + "}", scn1[i]).Replace("{scaleindex01-02." + i + "}", sci1[i].ToString("N2")).Replace("{scaleindex03-04." + i + "}", (getBallFromOther(scn2, sci2, scn1[i]).ToString("N2")));

            }
*/
            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"1\" width=\"700px\">");
            resultSB.Append("<tr BGCOLOR=\"lightgray\"><th>Пространство шкал " + ScaleNameS[0] + "-" + ScaleNameS[1] + "</th><th>Ось" + ScaleNameS[0] + "-" + ScaleNameS[1] + "</th></tr>");



            foreach (var ppp0 in s1)
            {

                resultSB.Append("<tr><td nowrap=\"nowrap\">" + ppp0.name + "</td><td >" + ppp0.value + "</td></tr>");
            }
            resultSB.Append("</table>");
            resultSB.Append("</p>");

            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"1\" width=\"700px\">");
            resultSB.Append("<tr BGCOLOR=\"lightgray\"><th>Пространство шкал " + ScaleNameS[2] + "-" + ScaleNameS[3] + "</th><th>Ось" + ScaleNameS[2] + "-" + ScaleNameS[3] + "</th></tr>");


            foreach (var ppp1 in s2)
            {

                resultSB.Append("<tr><td nowrap=\"nowrap\">" + ppp1.name + "</td><td >" + ppp1.value + "</td></tr>");
            }
            resultSB.Append("</table>");
            //resultSB.Append(sb1);

            resultSB.Append("</p>");
        }
        void run4(string baseScaleName, string oppositeScaleName, double baseScaleIndex, double oppositeScaleIndex, List<string> otherScaleName, List<double> otherScaleIndex, List<Charts.percentil> pxy)
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            //List<Charts.percentil> pxyv = new List<Charts.percentil>();
            //List<double> pxys = new List<double>();
            //double[] pxys;

            double DiffScaleIndex = 0;

            if (baseScaleIndex > 0 && oppositeScaleIndex > 0)
            {
                //double n1 = Math.Max(Math.Abs(baseScaleIndex), Math.Abs(oppositeScaleIndex));
                //double n2 = Math.Min(Math.Abs(baseScaleIndex), Math.Abs(oppositeScaleIndex));
                DiffScaleIndex = Math.Max(Math.Abs(baseScaleIndex), Math.Abs(oppositeScaleIndex)) - Math.Min(Math.Abs(baseScaleIndex), Math.Abs(oppositeScaleIndex));

            }
            else if (baseScaleIndex < 0 && oppositeScaleIndex < 0)
            {
                DiffScaleIndex = Math.Max(Math.Abs(baseScaleIndex), Math.Abs(oppositeScaleIndex)) - Math.Min(Math.Abs(baseScaleIndex), Math.Abs(oppositeScaleIndex));
                //DiffScaleIndex = Math.Abs(baseScaleIndex) + Math.Abs(oppositeScaleIndex);
            }

            else
            {
                DiffScaleIndex = Math.Abs(baseScaleIndex) + Math.Abs(oppositeScaleIndex);
                //DiffScaleIndex = (double)baseScaleIndex - (double)oppositeScaleIndex;
            }

            //double DiffScaleIndex = (double)baseScaleIndex - (double)oppositeScaleIndex;
            bool invertindexperc = true;
            double scl1 = 0;
            //resultSB.Append("<p class='rule_conseq'>");
            if (baseScaleIndex > oppositeScaleIndex)
            {
                invertindexperc = false;
            }

            scl1 = Math.Min(baseScaleIndex, oppositeScaleIndex);
            double otr = -1;

            resultSB.Append("<table border=\"1\" width=\"350px\">");
            //string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            resultSB.Append("<tr><th>&nbsp;</th><th nowrap=\"nowrap\" colspan=\"2\">Шкалы " + baseScaleName + " - " + oppositeScaleName + "</th></tr>");
            //resultSB.Append("<tr><td nowrap=\"nowrap\">" + getFullScaleName(baseScaleName) + "</td><td >100%</td></tr>");

            if (invertindexperc == false)
            {
                //resultSB.Append("<tr><td nowrap=\"nowrap\">" + getFullScaleName(baseScaleName) + "</td><td >100%</td></tr>");
                pxy.Add(new Charts.percentil(getFullScaleName(baseScaleName), 100));

                for (int i = 0; i < otherScaleName.Count; i++)
                {
                    if (baseScaleIndex < 0 && oppositeScaleIndex < 0 && otherScaleIndex[i] < 0)
                    {
                        //resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * ((scl1 - otherScaleIndex[i]) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                        pxy.Add(new Charts.percentil(otherScaleName[i], Math.Round(100 * Math.Abs(DiffScaleIndex != 0 ? ((((scl1 - otherScaleIndex[i]) / DiffScaleIndex))) : 0), 2)));
                    }
                    else if (scl1 > 0 && otherScaleIndex[i] < scl1)
                    {
                        //resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * ((((otr) * scl1) - otherScaleIndex[i]) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                        //pxy.Add(new Charts.percentil(otherScaleName[i], (100 * Math.Abs(DiffScaleIndex != 0 ? ((100 * ((((otr) * scl1) - otherScaleIndex[i]) / DiffScaleIndex))) : 0))));
                        //resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * (((otr) * (scl1 - otherScaleIndex[i])) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                        pxy.Add(new Charts.percentil(otherScaleName[i], Math.Round(100 * (DiffScaleIndex != 0 ? ((((otr) * (scl1 - otherScaleIndex[i])) / DiffScaleIndex)) : 0), 2)));

                    }
                    else
                    {
                        //resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * ((otherScaleIndex[i] + Math.Abs(scl1)) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                        pxy.Add(new Charts.percentil(otherScaleName[i], Math.Round(100 * Math.Abs(DiffScaleIndex != 0 ? ((otherScaleIndex[i] + Math.Abs(scl1)) / (DiffScaleIndex)) : 0), 2)));
                    }
                }
                if (oppositeScaleName != "" && oppositeScaleName != "00")
                {
                    //resultSB.Append("<tr><td nowrap=\"nowrap\">" + getFullScaleName(oppositeScaleName) + "</td><td >0%</td></tr>");
                    pxy.Add(new Charts.percentil(getFullScaleName(oppositeScaleName), 0));
                }
            }

            if (invertindexperc == true)
            {
                //resultSB.Append("<tr><td nowrap=\"nowrap\">" + getFullScaleName(oppositeScaleName) + "</td><td >100%</td></tr>");
                pxy.Add(new Charts.percentil(getFullScaleName(oppositeScaleName), 100));

                for (int i = 0; i < otherScaleName.Count; i++)
                {
                    if (baseScaleIndex < 0 && oppositeScaleIndex < 0 && otherScaleIndex[i] < 0)
                    {
                        //resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * ((scl1 - otherScaleIndex[i]) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                        pxy.Add(new Charts.percentil(otherScaleName[i], Math.Round(100 * Math.Abs(DiffScaleIndex != 0 ? ((((scl1 - otherScaleIndex[i]) / DiffScaleIndex))) : 0), 2)));
                    }
                    else if (scl1 > 0 && otherScaleIndex[i] < scl1)
                    {
                        //resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * ((((otr) * scl1) - otherScaleIndex[i]) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                        //pxy.Add(new Charts.percentil(otherScaleName[i], (100 * Math.Abs(DiffScaleIndex != 0 ? ((100 * ((((otr) * scl1) - otherScaleIndex[i]) / DiffScaleIndex))) : 0))));
                        //resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * (((otr) * (scl1 - otherScaleIndex[i])) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                        pxy.Add(new Charts.percentil(otherScaleName[i], Math.Round(100 * (DiffScaleIndex != 0 ? ((((otr) * (scl1 - otherScaleIndex[i])) / DiffScaleIndex)) : 0), 2)));

                    }
                    else
                    {
                       // resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * ((otherScaleIndex[i] + Math.Abs(scl1)) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                        pxy.Add(new Charts.percentil(otherScaleName[i], Math.Round(100 * Math.Abs(DiffScaleIndex != 0 ? ((otherScaleIndex[i] + Math.Abs(scl1)) / (DiffScaleIndex)) : 0), 2)));
                    }
                }


                if (baseScaleName != "" && baseScaleName != "00")
                {
                    // resultSB.Append("<tr><td nowrap=\"nowrap\">" + getFullScaleName(baseScaleName) + "</td><td >0%</td></tr>");

                    pxy.Add(new Charts.percentil(getFullScaleName(baseScaleName), 0));
                }

            }

            pxy.Sort();
            pxy.Reverse();

        
            foreach (var ppp2 in pxy)
            {

                resultSB.Append("<tr><td nowrap=\"nowrap\">" + ppp2.name + "</td><td >" + ppp2.value + "</td></tr>");
            }
            
            resultSB.Append("</table>");
        }

        double getBallFromOtherScale(List<Charts.percentil> other, string scalename)
        {
            for (int i = 0; i < other.Count; i++)
            {

                if (other[i].name == scalename)
                    return other[i].value;
            }
            return 0;
        }
        void WriteGraphs()
        {
            List<Charts.percentilxy> l = new List<Charts.percentilxy>();
            List<Charts.percentilxy> l1 = new List<Charts.percentilxy>();
            List<Charts.percentilxy> l2 = new List<Charts.percentilxy>();
            List<string> hc1 = new List<string>();
            List<string> hc2 = new List<string>();


            List<string> scn1 = new List<string>();
            List<double> sci1 = new List<double>();
            scn1.Add(getFullScaleName(ScaleNameS[0])); scn1.Add(getFullScaleName(ScaleNameS[1]));
            sci1.Add(ScaleIndexeS[0]); sci1.Add(ScaleIndexeS[1]);
            for (int i = 0; i < OtherScaleIndex.Count; i++)
            {
                scn1.Add(OtherScaleName[i]);
                sci1.Add(Math.Round(OtherScaleIndex[i], 2));
            }

            List<string> scn2 = new List<string>();
            List<double> sci2 = new List<double>();
            scn2.Add(getFullScaleName(ScaleNameS[2])); scn2.Add(getFullScaleName(ScaleNameS[3]));
            sci2.Add(ScaleIndexeS[2]); sci2.Add(ScaleIndexeS[3]); // ScaleIndexeS[3] 
            for (int i = 0; i < OtherScaleIndex2.Count; i++)
            {
                scn2.Add(OtherScaleName2[i]);
                sci2.Add(Math.Round(OtherScaleIndex2[i], 2));
            }
            //List<Charts.percentilxy> l1 = new List<Charts.percentilxy>();
            List<string> k2 = new List<string> ();
            for (int i = 0; i < scn1.Count; i++)
            {
                l1.Add(new Charts.percentilxy(s1[i].name, s1[i].value, getBallFromOther(scn2, sci2, s1[i].name)));
            }
            // Собираем данные для наименованя шкал 
            k2.Add(scn1[0]);
            k2.Add(scn1[1]);
            k2.Add(scn2[0]);
            k2.Add(scn2[1]);
            

            foreach (var item in p1)
            {

                l.Add(new Charts.percentilxy(item.name, item.value, getBallFromOtherScale(p2, item.name)));
            }

            foreach (var item in p2)
            {

                l2.Add(new Charts.percentilxy(item.name, item.value, getBallFromOtherScale(p1, item.name)));
            }


            //Данные для построения графиков Highcharts
            foreach (var item in l)
            {
                hc1.Add(item.name);
            }

            foreach (var item2 in l1)
            {
                hc2.Add(item2.name);
            }

            List<double> hc3 = new List<double>();
            List<double> hc4 = new List<double>();
            List<double> hc5 = new List<double>();
            List<double> hc6 = new List<double>();

            foreach (var item3 in l)
            {
                hc3.Add(item3.valuex);
                hc4.Add(item3.valuey);
            }
            /*
            foreach (var item4 in l2)
            {
                hc4.Add(item4.valuex);
                //hc4.Add(item3.valuey);
            }*/

            foreach (var item4 in l1)
            {
                hc5.Add(item4.valuex);
                hc6.Add(item4.valuey);
            }

            Random random = new Random();
            int uniqId = random.Next(0, 100);
            //var uniqId = examinee.Id + test;
            
            List<string> result = new List<string>();

            Charts.hcGraph grhc = new Charts.hcGraph(k2, hc1, hc2, hc3, hc4, hc5, hc6, uniqId, ScaleNameS);
            grhc.runhc(result);

            foreach (var item4 in result)
            {
                resultSB.Append("</p>");
                resultSB.Append(item4);
                resultSB.Append("</p>");
            }
            /*
            var gr = new Charts.sGraph5(800, 400, l, k2, 20);

            var virtPath = "ScaleGraphs/" + Shared.Helper.GeneratePassword(10) + ".png";

            var phisicalPath = Shared.GlobalOptions.DababaseMode == DbMode.Local
                                   ? Shared.GlobalOptions.InterpretsFolder
                                   : AppDomain.CurrentDomain.BaseDirectory;

            var fname = Path.Combine(phisicalPath, virtPath.Replace("/", "\\"));
            gr.saveImage(fname);

            
            resultSB.Append("<p><img src=\"../" + virtPath + "\"/></p><br/><br/>");


            Charts.sGraph6 gr6 = new Charts.sGraph6(800, 400, l1, k2, 5, 5);
            virtPath = "ScaleGraphs/" + Shared.Helper.GeneratePassword(10) + ".png";

            fname = Path.Combine(phisicalPath, virtPath.Replace("/", "\\"));

            gr6.saveImage(fname);

            //resultSB.Append("<p><img src=\"https://upload.wikimedia.org/wikipedia/ru/5/53/Olga_Skorokhodova.jpg \"/></p><br/><br/>");
            resultSB.Append("<p><img src=\"../" + virtPath + "\"/></p><br/><br/>");
            */

        }

        private string getFullScaleName(string sname)
        {
            if (sname == "")
            {
                Trace.TraceWarning("Empty scale name",
                    "Warning: There is no scale name provided");
                return "";
            }

            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                var scales = dbRepository.GetScalesForTest(testItem.TestId).First(sc => sc.ScaleShortName == sname);
                return scales.ScaleName;
            }
        }


        public override void Run()
        {
            bool isrule = isItRule(ruleTxt, this.resultSB);

            if (resultSB != null)
            {
                tResultSB = new StringBuilder(resultSB.ToString());
                resultSB.Remove(0, resultSB.Length);
            }
            if (!isrule)
            {
                resultSB.Remove(0, resultSB.Length);
                return;
            }

            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            TestShortName = TestShortName.ToUpper();
            this.tests.Clear();
            this.tests.Add(TestShortName);
            if (testItem == null)
                testItem = getTest(TestShortName);
            if (testItem == null)
                return; // тест не найден
            if (testItem.TestResults.Count == 0)
                return; //выход при количестве результатов теста == 0

            WriteTestHeading();
            if (dumpAnswers)
                DumpTestAnswers();

            fillScaleNames();
            fillQuestIndexes();
            double i1 = 0, i2 = 0;
            CalcAndWriteWeightCoeffs(ScaleNameS[0], ScaleNameS[1], ref i1, ref i2, StartIndex[0], StartIndex[1]);

            ScaleIndexeS[0] = i1;
            ScaleIndexeS[1] = i2;
            i1 = i2 = 0;
            CalcAndWriteWeightCoeffs(ScaleNameS[2], ScaleNameS[3], ref i1, ref i2, StartIndex[2], StartIndex[3]);

            ScaleIndexeS[2] = i1;
            ScaleIndexeS[3] = i2;
            fillOtherScaleIndex(ScaleNameS[0], ScaleNameS[1], OtherScaleName, OtherScaleIndex, 0);
            //fillOtherScaleIndex(ScaleNameS[2], ScaleNameS[3], OtherScaleName2, OtherScaleIndex2, weightQuiestons.Count / 2);
            fillOtherScaleIndex2(ScaleNameS[2], ScaleNameS[3], OtherScaleName2, OtherScaleIndex2, 0);
            CalcAndWriteScalesSpace();

            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"0\" width=\"700px\">");
            resultSB.Append("<tr><td>");
            run4(ScaleNameS[0], ScaleNameS[1], ScaleIndexeS[0], ScaleIndexeS[1], OtherScaleName, OtherScaleIndex, p1);
            resultSB.Append("</td><td>");
            run4(ScaleNameS[2], ScaleNameS[3], ScaleIndexeS[2], ScaleIndexeS[3], OtherScaleName2, OtherScaleIndex2, p2);
            resultSB.Append("</td></tr>");
            resultSB.Append("</table>");
            resultSB.Append("</p>");
            
            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"0\" width=\"700px\">");
            resultSB.Append("<tr><td>");
      
            resultSB.Append("</td></tr>");
            resultSB.Append("</table>");
            resultSB.Append("</p>");
            WriteGraphs();
            resultSB.Append("</p>");
            //WriteGraphs2();

        }
    }

}
