using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shared;
using Testing.Data;

namespace Interpret.Rules
{
    public class Rule_71 : Rule
    {

        public Rule_71(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        public Rule_71(string ruleTxt, StringBuilder resultSB, Archive.Examinee examinee, int ruleID) : base(ruleTxt, resultSB, examinee, ruleID) { }
        public static bool isItRule(string ruleTxt)
        {
            return isItRule(ruleTxt, null);
        }
        private StringBuilder tResultSB = null;
        double BaseScaleIndex = 0;
        double OppositeScaleIndex = 0;
        string BaseScaleName = "";
        string OppositeScaleName = "";
        List<string> OtherScaleName = new List<string>();
        List<double> OtherScaleIndex = new List<double>();
        List<WeightQuiestons> scalesQuestions = new List<WeightQuiestons>();
        List<WeightQuiestons> weightQuiestons = new List<WeightQuiestons>();
        Archive.ExamineeTest testItem = null;
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
            string pattern = @"^[-#]{1}71" + tSpacer + tTestShortName;
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
                    //if (matches.Count > 2)
                    //   return false;
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
                    //if (matches.Count > 2)
                    //    return false;
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
        public void runIt()
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            
            
            //var conseq = from f in db.Conseqs
            //             where f.RuleID == ruleID
            //             select f.ConseqText;
               using (var dbRepository = DbRepositoryFactory.GetRepository())
               {
                   var conseq = dbRepository.GetConseqsByRule(ruleID);

                   string ConseqText = "";
                   foreach (var item in conseq)
                   {
                       ConseqText = conseq.First().ConseqText;
                   }
                   if (ConseqText.Length > 0)
                   {
                       resultSB.Append("<p class='rule_conseq'>");
                       resultSB.Append(ConseqText);
                       resultSB.Append("</p>");
                   }
               }
        }
        private int CompareXY(ScaleBall x, ScaleBall y)
        {
            if (x.ScaleID > y.ScaleID)
            {
                return -1;
            }
            else if (x.ScaleID == y.ScaleID && x.AnsID > y.AnsID)
                return -1;
            else
                return 0;
        }
        public void fillScaleNames()
        {
            BaseScaleName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "ScaleShortName" + 1.ToString());
            string lastscale = "";
            for (int i = 0; i < Interpretation.scaleBalls.Count; i++)
            {
                ScaleBall item = Interpretation.scaleBalls[i];
                if (item.ScaleName == BaseScaleName) // && i < Interpretation.scaleBalls.Count
                {

                    if (item.AnsID > 0)
                    {
                        for (int j = i + 1; j < Interpretation.scaleBalls.Count; j++)
                        {
                            lastscale = item.ScaleName;
                            item = Interpretation.scaleBalls[j];
                            if (lastscale != item.ScaleName)
                            {
                                lastscale = item.ScaleName;
                                if (OppositeScaleName == "")
                                {
                                    OppositeScaleName = item.ScaleName;
                                    return;
                                }
                                //else
                                //{
                                //OtherScaleName.Add(item.ScaleName);
                                //}
                            }

                        }
                        break;
                    }
                }
                else 
                    break;

            }
        }
        public void fillOtherScaleIndex()
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            
            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            
            if (testItem == null)
                testItem = getTest(TestShortName);

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                //var scales = db.Scales.Where(s =>
                //                             s.ScaleShortName != BaseScaleName &&
                //                             s.ScaleShortName != OppositeScaleName
                //                             && s.TestID == testItem.TestId);

                var scales = dbRepository.GetScalesForTest(testItem.TestId).Where(s =>
                                             s.ScaleShortName != BaseScaleName &&
                                             s.ScaleShortName != OppositeScaleName);

                foreach (var oScale in scales)
                {

                    double Other1ScaleIndex = 0;
                    var itemScales = scalesQuestions.Where(sc => sc.name == oScale.ScaleName);
                    int i = 0;
                    foreach (var itemScale in itemScales)
                    {
                        WeightQuiestons itemWeight = weightQuiestons[i];
                        Other1ScaleIndex += itemScale.ball*itemWeight.ball;
                        i++;
                    }


                    OtherScaleName.Add(oScale.ScaleName);
                    OtherScaleIndex.Add(Other1ScaleIndex);
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
                    //              where p.ScaleShortName == BaseScaleName
                    //              select p.ScaleID;

                    var ScaleId = from p in dbRepository.GetScalesForTest(testItem.TestId)
                                  where p.ScaleShortName == BaseScaleName
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
                    //                 where ansesInt.Contains((int) p.AnsID) && scaleids.Contains((int) p.ScaleID)
                    //                 select new {p.AnsID, p.Weight};

                    var scaleBalls = from p in dbRepository.GetScaleWeightForScaleAndAnswer(scaleids, ansesInt)
                                     select new { p.AnsID, p.Weight };
                    
                    return scaleBalls.Count();
                }
                else
                {
                    int anscount = dbRepository.GetQuestionAnswers(QuestID).Count;
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

                    //var Answers = from p in db.Answers
                    //              where p.QuestID == QuestID
                    //              orderby p.AnsNum
                    //              select p.AnsID;

                    var Answers = (from p in dbRepository.GetQuestionAnswers(QuestID)
                                  orderby p.AnsNum
                                  select p.AnsID).ToList();

                    //var ScaleWeights = from p in db.ScaleWeights
                    //                   where Answers.Contains((int) p.AnsID)
                    //                   select new {p.AnsID, p.Weight, p.Scale.ScaleName, p.ScaleID};

                    var ScaleWeights = from p in dbRepository.GetScaleWeightForAnswers(Answers)
                                       let scale = p.ScaleID.HasValue ? dbRepository.GetScale(p.ScaleID.Value) : null
                                       let sName = scale != null ? scale.ScaleName : string.Empty
                                       select new { p.AnsID, p.Weight, ScaleName = sName, p.ScaleID };

                    scaleWeight = 0;
                    scaleName = "";
                    foreach (var itemScaleBall in ScaleWeights)
                    {
                        if (testResult.AnsID == itemScaleBall.AnsID)
                        {
                            scaleName = itemScaleBall.ScaleName;
                            scaleWeight +=
                                (double)
                                Interpretation.GetCorrectBall((double) itemScaleBall.Weight, (int) itemScaleBall.ScaleID);
                                //(double)itemScaleBall.Weight;
                        }
                    }
                    if (scaleName != "")
                    {
                        resultSB.Append("<tr><td >XXX</td><td >" + scaleName + "</td><td >" + testResult.QuestText +
                                        "</td><td >" + scaleWeight + "</td></tr>");
                        scalesQuestions.Add(new WeightQuiestons(scaleName, scaleWeight, QuestID));
                    }
                }
            }
            resultSB.Append("</table>");
            resultSB.Append("</p>");
        }

        public void run2()
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
                
                
                resultSB.Append("<tr><td nowrap=\"nowrap\" colspan=\"" + (ansCount + 2).ToString() + "\">" + "2) ВЕСОВЫЕ КОЭФФИЦИЕНТЫ ЗНАЧИМОСТИ СВОЙСТВ в ПРОСТРАНСТВЕ " + BaseScaleName + (OppositeScaleName.Length > 0 ? " - " + OppositeScaleName : "") + "</td><td>сколько частей занимает свойство в пространстве ИССЛЕДУЕМОГО</td><td>сколько частей занимает само свойство</td><td>Весовой коэффициент каждого свойства</td></tr>");

                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {

                    ////var scale = dbRepository.GetScale(BaseScaleName) ?? dbRepository.GetScale(OppositeScaleName);

                    //var scale =
                    //    db.Scales.First(s => s.ScaleShortName == BaseScaleName || s.ScaleShortName == OppositeScaleName);


                    if (testItem != null && BaseScaleName != "")
                    {

                        var scaleCount = dbRepository.GetScalesForTest(testItem.TestId).Count;
                        
                        //int questCount = testItem.TestResults.Count/db.Scales.Count(s => s.TestID == testItem.TestId);
                        int questCount = testItem.TestResults.Count / scaleCount;
                        
                        Archive.TestResult item = null;
                        double fstc;
                        for (int i = 0; i < questCount; i++)
                        {
                            fstc = 0;
                            for (int counter = 0; counter < 2; counter++)
                            {
                                item = testItem.TestResults[i + counter*questCount];
                                int QuestID = item.QuestID;

                                //var anses = from p in db.Answers
                                //            where p.QuestID == QuestID
                                //            orderby p.AnsNum
                                //            select p.AnsID;

                                var anses = from p in dbRepository.GetQuestionAnswers(QuestID)
                                            orderby p.AnsNum
                                            select p.AnsID;

                                var ScaleId = from p in dbRepository.GetScalesAll()
                                              where
                                                  p.ScaleShortName == ((counter) % 2 == 0 || OppositeScaleName == "" ||
                                                   OppositeScaleName == "00"
                                                       ? BaseScaleName
                                                       : OppositeScaleName)
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
                                //                 where ansesInt.Contains((int) p.AnsID) &&
                                //                     scaleids.Contains((int) p.ScaleID)
                                //                 select new {p.AnsID, p.Weight, p.ScaleID};

                                var scaleBalls = from p in dbRepository.GetScaleWeightForScaleAndAnswer(scaleids, ansesInt)
                                                 select new { p.AnsID, p.Weight, p.ScaleID };

                                resultSB.Append("<tr>");
                                if (Convert.ToInt32(counter)%2 == 0 || OppositeScaleName == "" ||
                                    OppositeScaleName == "00")
                                    resultSB.Append("<td nowrap=\"nowrap\">" + BaseScaleName + "</td>");
                                else
                                    resultSB.Append("<td nowrap=\"nowrap\">" + OppositeScaleName + "</td>");
                                int c = 0;
                                foreach (var itemScaleBall in scaleBalls)
                                {
                                    c++;
                                    if ((Convert.ToInt32(counter))%2 == 0 || OppositeScaleName == "" ||
                                        OppositeScaleName == "00")
                                    {
//red
                                        resultSB.Append("<td nowrap=\"nowrap\">" +
                                                        (item.AnsID != itemScaleBall.AnsID
                                                             ? itemScaleBall.Weight.ToString()
                                                             : "<b style=\"color:red;\">" +
                                                               itemScaleBall.Weight.ToString() + "</b></td>"));
                                        if (item.AnsID == itemScaleBall.AnsID)
                                        {
                                            BaseScaleIndex +=
                                                (double)
                                                Interpretation.GetCorrectBall((double) itemScaleBall.Weight,
                                                                              (int) itemScaleBall.ScaleID);
                                                //(double)itemScaleBall.Weight;
                                            fstc = Convert.ToDouble(item.AnsNumber);
                                        }
                                    }
                                    else if (OppositeScaleName != "" && OppositeScaleName != "00")
                                    {
                                        //blue
                                        resultSB.Append("<td nowrap=\"nowrap\">" +
                                                        (item.AnsID != itemScaleBall.AnsID
                                                             ? itemScaleBall.Weight.ToString()
                                                             : "<b style=\"color:blue;\">" +
                                                               itemScaleBall.Weight.ToString() + "</b></td>"));
                                        if (item.AnsID == itemScaleBall.AnsID)
                                        {
                                            OppositeScaleIndex +=
                                                (double)
                                                Interpretation.GetCorrectBall((double) itemScaleBall.Weight,
                                                                              (int) itemScaleBall.ScaleID);
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
                                if ((Convert.ToInt32(counter))%2 == 0)
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
                                    resultSB.Replace("{$thrdc}", ((Math.Abs(fstc) + 1)/(ansCount)).ToString("N2"));
                                    weightQuiestons.Add(new WeightQuiestons("", ((Math.Abs(fstc) + 1)/(ansCount)),
                                                                            QuestID));
                                }
                                resultSB.Append("</tr>");
                                //counter++;
                            }
                        }
                    }
                }
            }
            resultSB.Append("</table>");
            resultSB.Append("</p>");
        }


        void run3()
        {
            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"1\" width=\"700px\">");
            //int testcount = Convert.ToInt32(Helper.Helper.getParameterValue(tResultSB.ToString(), "testcount"));
            resultSB.Append("<tr><td  colspan=\"3\">" + "4) ИНДЕКСЫ ОЦЕНИВАЕМЫХ ПОНЯТИЙ В ВЫБРАННОМ СЕМАНТИЧЕСКОМ ПРОСТРАНСТВЕ" + "</td><td ></td></tr>");
            resultSB.Append("<tr><td nowrap=\"nowrap\">ИНДЕКС " + BaseScaleName + "</td><td nowrap=\"nowrap\">" + BaseScaleIndex.ToString() + "</td><td >ИНДЕКС БАЗОВОГО ПОНЯТИЯ</td></tr>");
            if (OppositeScaleName != "" && OppositeScaleName != "00")
                resultSB.Append("<tr><td nowrap=\"nowrap\">ИНДЕКС " + OppositeScaleName + "</td><td >" + OppositeScaleIndex.ToString() + "</td><td >ИДЕКС ПОНЯТИЯ, КОТОРОЕ ПРОТИВОПОЛОЖНО БАЗОВОМУ ПОНЯТИЮ</td></tr>");
            if (OtherScaleName.Count > 0)
                for (int i = 0; i < OtherScaleName.Count; i++)
                {
                    resultSB.Append("<tr><td nowrap=\"nowrap\">ИНДЕКС " + OtherScaleName[i] + "</td><td >" + "&nbsp;" + "</td><td >ИНДЕКС ОСТАЛЬНЫХ ПОНЯТТИЙ, КОТОРЫЕ НЕ ИСПОЛЬЗУЮТСЯ КАК ОСНОВНЫЕ ОСИ В ДАННОМ АЛГОРИТМЕ</td></tr>");
                }
            resultSB.Append("</table>");
            resultSB.Append("</p>");
        }
        void run5()
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            double DiffScaleIndex = (double)BaseScaleIndex - (double)OppositeScaleIndex;
            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"1\" width=\"700px\">");
            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            resultSB.Append("<tr><td  nowrap=\"nowrap\" colspan=\"2\">" + "5) ПРОЦЕНТИЛИ ПОНЯТИЙ" + "</td></tr>");
            resultSB.Append("<tr><td nowrap=\"nowrap\" colspan=\"2\">100%= " + BaseScaleName + " - " + OppositeScaleName + "</td></tr>");
            resultSB.Append("<tr><td nowrap=\"nowrap\" colspan=\"1\">100% </td><td >" + DiffScaleIndex + "</td></tr>");
            resultSB.Append("<tr><td nowrap=\"nowrap\">&nbsp;</td><td >Процентиль</td></tr>");
            resultSB.Append("<tr><td nowrap=\"nowrap\">" + BaseScaleName + "</td><td >100%</td></tr>");
            fillOtherScaleIndex();
            {
                for (int i = 0; i < OtherScaleName.Count; i++)
                {
                    resultSB.Append("<tr><td nowrap=\"nowrap\"> " + OtherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * Math.Abs(OtherScaleIndex[i] / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                }
            }
            if (OppositeScaleName != "" && OppositeScaleName != "00")
            {
                resultSB.Append("<tr><td nowrap=\"nowrap\">" + OppositeScaleName + "</td><td >0%</td></tr>");
            }
            resultSB.Append("</table>");
            resultSB.Append("</p>");
        }
        void run6()
        {
            double DiffScaleIndex = (double)BaseScaleIndex - (double)OppositeScaleIndex;
            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"1\" width=\"700px\">");
            resultSB.Append("<tr><td nowrap=\"nowrap\" colspan=\"3\">" + "6) ГРАФИК ИНДЕКСОВ ПОНЯТИЙ" + "</td><td ></td></tr>");
            resultSB.Append("<tr><td nowrap=\"nowrap\">" + "" + "</td><td nowrap=\"nowrap\">Процентиль</td><td nowrap=\"nowrap\">" + "ИНДЕКС" + "</td></tr>");
            resultSB.Append("<tr><td nowrap=\"nowrap\">" + BaseScaleName + "</td><td >100%</td><td nowrap=\"nowrap\">" + BaseScaleIndex.ToString() + "</td></tr>");

            if (OtherScaleName.Count > 0)
                for (int i = 0; i < OtherScaleName.Count; i++)
                {
                    resultSB.Append("<tr><td nowrap=\"nowrap\"> " + OtherScaleName[i] + "</td><td nowrap=\"nowrap\">" + Math.Round((100 * Math.Abs(OtherScaleIndex[i] / DiffScaleIndex)), 2).ToString() + "% </td><td nowrap=\"nowrap\">" + (OtherScaleIndex[i] > 1 ? OtherScaleIndex[i].ToString() : OtherScaleIndex[i].ToString("N2")) + "</td></tr>");
                }
            if (OppositeScaleName != "" && OppositeScaleName != "00")
            {
                resultSB.Append("<tr><td nowrap=\"nowrap\">" + OppositeScaleName + "</td><td >0%</td><td nowrap=\"nowrap\">" + OppositeScaleIndex.ToString() + "</td></tr>");
            }

            resultSB.Append("</table>");
            resultSB.Append("</p>");

            List<Charts.percentil> l = new List<Charts.percentil>();

            Charts.percentil p1 = new Charts.percentil
            {
                name = getFullScaleName(BaseScaleName),
                value = 100
            };
            l.Add(p1);
            if (OtherScaleName.Count > 0)
                for (int i = 0; i < OtherScaleName.Count; i++)
                {
                    Charts.percentil pOther = new Charts.percentil
                    {
                        name = OtherScaleName[i],
                        value = Math.Round((100 * Math.Abs(OtherScaleIndex[i] / DiffScaleIndex)), 2)
                    };
                    l.Add(pOther);
                }
            if (OppositeScaleName != "" && OppositeScaleName != "00")
            {
                Charts.percentil p2 = new Charts.percentil
                {
                    name = getFullScaleName(OppositeScaleName),
                    value = 0
                };
                l.Add(p2);
            }
            var gr = new Charts.sGraph2(800, 400, l, 20);

            var virtPath = "ScaleGraphs/" + Shared.Helper.GeneratePassword(11) + ".png";
            var fname = virtPath;

            var phisicalPath = GlobalOptions.DababaseMode == DbMode.Local
                                   ? GlobalOptions.InterpretsFolder
                                   : AppDomain.CurrentDomain.BaseDirectory;

            gr.saveImage(Path.Combine(phisicalPath, fname.Replace("/", "\\")));

            resultSB.Append("<p><img src=\"../" + virtPath + "\"/></p><br/><br/>");

            l = new List<Charts.percentil>();

            p1 = new Charts.percentil
            {
                name = getFullScaleName(BaseScaleName),
                value = BaseScaleIndex
            };
            l.Add(p1);
            if (OtherScaleName.Count > 0)
                for (int i = 0; i < OtherScaleName.Count; i++)
                {
                    var pOther = new Charts.percentil
                    {
                        name = OtherScaleName[i],
                        value = OtherScaleIndex[i]
                    };
                    l.Add(pOther);
                }
            if (OppositeScaleName != "" && OppositeScaleName != "00")
            {
                var p2 = new Charts.percentil
                {
                    name = getFullScaleName(OppositeScaleName),
                    value = OppositeScaleIndex
                };
                l.Add(p2);
            }
            var gr2 = new Charts.sGraph3(800, 400, l, 0);


            virtPath = "ScaleGraphs/" + Shared.Helper.GeneratePassword(12) + ".png";
            
            fname =  Path.Combine(phisicalPath, virtPath.Replace("/", "\\"));
            
            gr2.saveImage(fname);

            resultSB.Append("<p><img src=\"../" + virtPath + "\"/></p><br/><br/>");
            if (l.Count == 3)
            {
                Charts.sGraph4 gr4 = new Charts.sGraph4(800, 700, l, 0);
                virtPath = "ScaleGraphs/" + Shared.Helper.GeneratePassword(12) + ".png";
                fname = Path.Combine(phisicalPath, virtPath.Replace("/", "\\"));
                gr4.saveImage(fname);
                resultSB.Append("<p><img src=\"../" + virtPath + "\"/></p><br/><br/>");
            }
        }
        private string getFullScaleName(string sname)
        {
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
            this.tests.Clear();
            this.tests.Add(TestShortName);

            WriteTestHeading();
            if (dumpAnswers)
                DumpTestAnswers();

            fillScaleNames();
            run2();
            run3();
            run5();
            run6();
        }
    }
    public class WeightQuiestons
    {
        public WeightQuiestons(string name, double ball, int questid)
        {
            this.name = name;
            this.ball = ball;
            this.questid = questid;
        }
        public string name;
        public double ball;
        private int questid;

        public int Questid
        {
            get { return questid; }
            set { questid = value; }
        }

    }


}
