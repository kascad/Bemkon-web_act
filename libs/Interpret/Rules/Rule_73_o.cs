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
    class Rule_73_o : Rule
    {
        public Rule_73_o(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        public Rule_73_o(string ruleTxt, StringBuilder resultSB, Archive.Examinee examinee, int ruleID) : base(ruleTxt, resultSB, examinee, ruleID) { }
        public static bool isItRule(string ruleTxt)
        {
            return isItRule(ruleTxt, null);
        }
        private StringBuilder tResultSB = null;

        List<string> OtherScaleName = new List<string>();
        List<double> OtherScaleIndex = new List<double>();
        List<string> OtherScaleName2 = new List<string>();
        List<double> OtherScaleIndex2 = new List<double>();
        List<string> OtherScaleName3 = new List<string>();
        List<double> OtherScaleIndex3 = new List<double>();
        List<string> AddScales = new List<string>();
        List<int> StartIndex = new List<int>();
        List<int>[] QuestIndex = new List<int>[6];
        List<string> ScaleNameS = new List<string>();
        List<double> ScaleIndexeS = new List<double>();
        List<double>[] ScaleValueS = new List<double>[3];
        List<WeightQuiestons> scalesQuestions = new List<WeightQuiestons>();
        List<WeightQuiestons> weightQuiestons = new List<WeightQuiestons>();
        List<Charts.percentil> p1 = new List<Charts.percentil>();
        List<Charts.percentil> p2 = new List<Charts.percentil>();
        List<Charts.percentil> p3 = new List<Charts.percentil>();
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
            string pattern = @"^[-#]{1}73" + tSpacer + tTestShortName;
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
            for (int i = 1; i <= 6; i++)
            {
                ScaleNameS.Add(Helper.Helper.getParameterValue(this.tResultSB.ToString(), "ScaleShortName" + i.ToString()));
                ScaleIndexeS.Add(0);
                if ((i - 1) % 2 == 0)
                    ScaleValueS[(i - 1) % 2] = new List<double>();
            }
        }
        //private bool checkScale(string scalename, int ansid)
        //{
        //    ProfessorTestingDataContext db = new ProfessorTestingDataContext();
        //    int cc = db.ScaleWeights.Count(s => (s.Scale.ScaleShortName == scalename && s.AnsID == ansid));
        //    return (cc > 0);
        //}

        private bool checkScale(int testId, string scalename, int ansid)
        {
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {                
                var scales = (from s in dbRepository.GetScalesForTest(testId)
                              where s.ScaleShortName == scalename
                              select s.ScaleID).ToList();
                var cc = dbRepository.GetScaleWeightForScaleAndAnswer(scales, new List<int> { ansid }).Count;
                return (cc > 0);
            }
        }

        private void fillQuestIndexesAddIndex()
        {
            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            if (testItem == null)
                testItem = getTest(TestShortName);
            Archive.TestResult tr = testItem.TestResults[0];
            for (int i = 0; i < ScaleNameS.Count; i++)
            {
                StartIndex.Add(0);
            }
            //int index=0;
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {

                //int questCount = testItem.TestResults.Count/
                //                 db.Scales.Count(
                //                     s =>
                //                     (s.TestID == testItem.TestId && s.ScaleShortName != ScaleNameS[0] &&
                //                      s.ScaleShortName != ScaleNameS[1] && s.ScaleShortName != ScaleNameS[2] &&
                //                      s.ScaleShortName != ScaleNameS[3] && s.ScaleShortName != ScaleNameS[4] &&
                //                      s.ScaleShortName != ScaleNameS[5]));

                int questCount = testItem.TestResults.Count /
                                 dbRepository.GetScalesAll().Count(
                                     s =>
                                     (s.TestID == testItem.TestId && s.ScaleShortName != ScaleNameS[0] &&
                                      s.ScaleShortName != ScaleNameS[1] && s.ScaleShortName != ScaleNameS[2] &&
                                      s.ScaleShortName != ScaleNameS[3] && s.ScaleShortName != ScaleNameS[4] &&
                                      s.ScaleShortName != ScaleNameS[5]));


                //for (int j = 0; j < ScaleNameS.Count; j++)
                {
                    //index = 0;
                    int i = 0;
                    foreach (
                        var scalename in
                            dbRepository.GetScalesAll().Where(
                                s =>
                                (s.TestID == testItem.TestId && s.ScaleShortName != ScaleNameS[0] &&
                                 s.ScaleShortName != ScaleNameS[1] && s.ScaleShortName != ScaleNameS[2] &&
                                 s.ScaleShortName != ScaleNameS[3] && s.ScaleShortName != ScaleNameS[4] &&
                                 s.ScaleShortName != ScaleNameS[5])))
                    {
                        for (int index = 0; index < testItem.TestResults.Count; index += questCount)
                        {
                            if (index < questCount*ScaleNameS.Count)
                                tr = testItem.TestResults[index];
                            if (checkScale(testItem.TestId, scalename.ScaleShortName, tr.AnsID))
                            {
                                StartIndex.Add(index);
                                AddScales.Add(scalename.ScaleShortName);
                                //index += questCount;
                                break;
                            }
                            //if (index < questCount * ScaleNameS.Count)
                            //    tr = testItem.TestResults[index];
                        }
                        i++;
                    }
                }
            }
        }
        private void fillQuestIndexes()
        {
            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            if (testItem == null)
                testItem = getTest(TestShortName);
            if (testItem == null || !testItem.IsFinished)
                return;

            for (int i = 0; i < ScaleNameS.Count; i++)
            {
                StartIndex.Add(0);
                QuestIndex[i] = new List<int>();
            }

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                int questCount = testItem.TestResults.Count / dbRepository.GetScalesForTest(testItem.TestId).Count;
                for (int i = 0; i < ScaleNameS.Count; i++)
                {
                    var scales = (from s in dbRepository.GetScalesForTest(testItem.TestId)
                        where s.ScaleShortName == ScaleNameS[i]
                        select s.ScaleID).ToList();
                    
                    // Раскомментировал цикл

                    for (int index = 0; index < testItem.TestResults.Count; index++)
                    {
                        Archive.TestResult tr = testItem.TestResults[index];
                        int count = dbRepository.GetScaleWeightForScaleAndAnswer(scales, new List<int> {tr.AnsID}).Count;
                        if (count > 0)
                        {
                            QuestIndex[i].Add(index);
                            if (QuestIndex[i].Count == questCount) // small optimization
                                break;
                        }
                    }
                    StartIndex[i] = QuestIndex[i][0];
                    
                }
            }
        }
        /*
        private void fillOtherScaleIndex(List<string> otherScaleName, List<double> otherScaleIndex, int level)
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            if (testItem == null)
                testItem = getTest(TestShortName);

            //using (var dbRepository = DbRepositoryFactory.GetRepository())
            //{
            //    var scales =
            //        dbRepository.GetScalesForTest(testItem.TestId).Where(
            //            s =>
            //            s.ScaleShortName != baseScaleName && s.ScaleShortName != oppositeScaleName);

            //    if (otherScaleName == null)
            //        otherScaleName = new List<string>();
            //    if (otherScaleIndex == null)
            //        otherScaleIndex = new List<double>();
            //    try
            //    {
            //        foreach (var oScale in scales)
            //        {
            //            double Other1ScaleIndex = 0;
            //            var itemScales = scalesQuestions.Where(sc => sc.name == oScale.ScaleName);
            //            int i = level;
            //            foreach (var itemScale in itemScales)
            //            {
            //                WeightQuiestons itemWeight = weightQuiestons[i];
            //                Other1ScaleIndex += itemScale.ball*itemWeight.ball;
            //                i++;
            //            }
            //            otherScaleName.Add(oScale.ScaleName);
            //            otherScaleIndex.Add(Other1ScaleIndex);
            //        }
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
        }

        */

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
                //if (otherScaleIndex2 == null)
                //    otherScaleIndex2 = new List<double>();
                try
                {

                    //Шаг 2 считаем индексы 03-16 не основных шкал

                    List<double> testScaleIndex1 = new List<double>();
                    List<double> testScaleIndex2 = new List<double>();


                    foreach (var oScale in scales)
                    {
                        System.Diagnostics.Debug.WriteLine("##################");
                        System.Diagnostics.Debug.WriteLine("oScale Start:" + oScale.ScaleName);
                        double Other1ScaleIndex = 0;
                        double Other2ScaleIndex = 0;
                        var itemScales = scalesQuestions.Where(sc => sc.name == oScale.ScaleName);
                        //System.Diagnostics.Debug.WriteLine("oScale Middle:" + sc);
                        //System.Diagnostics.Debug.WriteLine("oScale Middle:" + itemScales);
                        int i = level;
                        //int j = weightQuiestons.Count / 2;
                        int k = weightQuiestons.Count / 3;
                        foreach (var itemScale in itemScales)
                        {

                            int j = i + 2* weightQuiestons.Count / 3;

                            if (i < k)
                            {
                                WeightQuiestons itemWeight = weightQuiestons[i];
                                Other1ScaleIndex += itemScale.ball * itemWeight.ball;
                                //WeightQuiestons itemWeight2 = weightQuiestons[j];
                                //Other2ScaleIndex += itemScale.ball * itemWeight2.ball;
                            }

                            i++;
                        }
                        otherScaleName.Add(oScale.ScaleName);
                        otherScaleIndex.Add(Other1ScaleIndex);
                        //otherScaleIndex2.Add(Other2ScaleIndex);
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
                //var scales = db.Scales.Where(s => s.ScaleShortName != baseScaleName
                //                                  && s.ScaleShortName != oppositeScaleName
                //                                  && s.TestID == testItem.TestId);

                //double Other2ScaleIndex = 0;

                // if (oppositeScaleName == "16")
                //{


                var scales = dbRepository.GetScalesForTest(testItem.TestId).Where(s => s.ScaleShortName != baseScaleName
                                                 && s.ScaleShortName != oppositeScaleName);

                if (otherScaleName2 == null)
                    otherScaleName2 = new List<string>();
                if (otherScaleIndex2 == null)
                    otherScaleIndex2 = new List<double>();
                try
                {

                    //Шаг 2 считаем индексы 03-16 не основных шкал

                    List<double> testScaleIndex1 = new List<double>();
                    List<double> testScaleIndex2 = new List<double>();


                    foreach (var oScale in scales)
                    {
                        double Other2ScaleIndex = 0;
                        //double Other2ScaleIndex = 0;
                        var itemScales = scalesQuestions.Where(sc => sc.name == oScale.ScaleName);
                        int i = level;
                        int k = 2 * weightQuiestons.Count / 3;
                        foreach (var itemScale in itemScales)
                        {


                            int j = i + 2 * weightQuiestons.Count / 3;

                            if (i < k)
                            {
                                WeightQuiestons itemWeight = weightQuiestons[i];
                                Other2ScaleIndex += itemScale.ball * itemWeight.ball;
                               // WeightQuiestons itemWeight2 = weightQuiestons[j];
                               // Other2ScaleIndex += itemScale.ball * itemWeight2.ball;
                            }
                            i++;
                        }
                        otherScaleName2.Add(oScale.ScaleName);
                        //System.Diagnostics.Debug.WriteLine("oScale Exit:" + oScale.ScaleName);
                        //otherScaleIndex.Add(Other1ScaleIndex);
                        otherScaleIndex2.Add(Other2ScaleIndex);
                    }
                }


                catch (Exception err)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR:" + err.Message);
                }

            }
        }



        private void fillOtherScaleIndex3(string baseScaleName, string oppositeScaleName, List<string> otherScaleName3, List<double> otherScaleIndex3, int level)
        {

            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            if (testItem == null)
                testItem = getTest(TestShortName);
            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {


                var scales = dbRepository.GetScalesForTest(testItem.TestId).Where(s => s.ScaleShortName != baseScaleName
                                                 && s.ScaleShortName != oppositeScaleName);

                if (otherScaleName3 == null)
                    otherScaleName3 = new List<string>();
                //if (otherScaleIndex == null)
                //    otherScaleIndex = new List<double>();
                if (otherScaleIndex3 == null)
                    otherScaleIndex3 = new List<double>();
                try
                {

                    //Шаг 2 считаем индексы 03-16 не основных шкал

                    List<double> testScaleIndex1 = new List<double>();
                    List<double> testScaleIndex2 = new List<double>();


                    foreach (var oScale in scales)
                    {

                        double Other3ScaleIndex = 0;
                        //double Other2ScaleIndex = 0;
                        var itemScales = scalesQuestions.Where(sc => sc.name == oScale.ScaleName);
                        int i = level;
                        int k = 2 * weightQuiestons.Count / 3;
                        foreach (var itemScale in itemScales)
                        {

                            int j = i + 2 * weightQuiestons.Count / 3;

                            if (i < k)
                            {
                                WeightQuiestons itemWeight = weightQuiestons[i];
                                Other3ScaleIndex += itemScale.ball * itemWeight.ball;
                                //WeightQuiestons itemWeight2 = weightQuiestons[j];
                                //Other3ScaleIndex += itemScale.ball * itemWeight2.ball;
                            }

                            i++;
                            
                        }
                        otherScaleName3.Add(oScale.ScaleName);
                        //System.Diagnostics.Debug.WriteLine("oScale Exit:" + oScale.ScaleName);
                        //otherScaleIndex.Add(Other1ScaleIndex);
                        otherScaleIndex3.Add(Other3ScaleIndex);
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
                    var anses = from p in dbRepository.GetQuestionAnswers(QuestID)
                                orderby p.AnsNum
                                select p.AnsID;
                    
                    //var anses = from p in db.Answers
                    //            where p.QuestID == QuestID
                    //            orderby p.AnsNum
                    //            select p.AnsID;

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
                    //                 where ansesInt.Contains((int) p.AnsID) && scaleids.Contains((int) p.ScaleID)
                    //                 select new {p.AnsID, p.Weight};


                    var scaleBalls = from p in dbRepository.GetScaleWeightForScaleAndAnswer(scaleids, ansesInt)
                                     select new { p.AnsID, p.Weight };

                    return scaleBalls.Count();
                }
                else
                {

                    int anscount = dbRepository.GetQuestionAnswers(QuestID).Count();
                    
                    //int anscount = db.Answers.Count(an => an.QuestID == QuestID);
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
            resultSB.Append("<tr><td nowrap=\"nowrap\" calspan=\"5\">1)</td></tr>");
            resultSB.Append("<tr><td  nowrap=\"nowrap\" colspan=\"5\">" + examinee.Name + "</td></tr>");
            resultSB.Append("<tr BGCOLOR=\"lightgray\"><td>s</td><td nowrap=\"nowrap\">КЛИЕНТ</td><td >ШКАЛА</td><td >ВОПРОС</td><td >ВЫБОР ИССЛЕДУЕМОГО</td></tr>");
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
                    //                   select
                    //                       new {p.AnsID, p.Weight, p.Scale.ScaleName, p.Scale.ScaleShortName, p.ScaleID};

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
                            scaleWeight += (double) Interpretation.GetCorrectBall((double) itemScaleBall.Weight, (int) itemScaleBall.ScaleID);
                                //(double)itemScaleBall.Weight;
                        }
                    }
                    if (scaleName != "")
                    {
                        resultSB.Append("<tr><td>" + testResult.QuestNumber + "</td><td>XXX</td><td>" + scaleName +
                                        "</td><td >" + testResult.QuestText + "</td><td >" + scaleWeight + "</td></tr>");
                        scalesQuestions.Add(new WeightQuiestons(scaleName, scaleWeight, QuestID));
                    }
                }
            }
            resultSB.Append("</table>");
            resultSB.Append("</p>");
        }
        public void CalcAndWriteBasicScales(int baseScaleIndex, int oppositeScaleIndex, bool printtable)
        {
            string baseScaleName = ScaleNameS[baseScaleIndex];
            string oppositeScaleName = ScaleNameS[oppositeScaleIndex];

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

                double baseScaleBall = 0;
                double oppositeScaleBall = 0;
                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {
                    if (testItem != null && baseScaleName != "")
                    {
                        int questCount = testItem.TestResults.Count / dbRepository.GetScalesForTest(testItem.TestId).Count();

                        Archive.TestResult item = null;
                        for (int i = 0; i < questCount; i++)
                        {
                            double fstc = 0;
                            for (int counter = 0; counter < 2; counter++)
                            {
                                bool isBaseScale = Convert.ToInt32(counter)%2 == 0 
                                    || oppositeScaleName == "" || oppositeScaleName == "00";
                                List<int> currentIndexes = isBaseScale ? QuestIndex[baseScaleIndex] : QuestIndex[oppositeScaleIndex];
                                int currentStartIndex = isBaseScale ? StartIndex[baseScaleIndex] : StartIndex[oppositeScaleIndex];
                                string currentScale = isBaseScale ? baseScaleName : oppositeScaleName;

                                item = testItem.TestResults[currentIndexes[i]];

                                int QuestID = item.QuestID;
                                List<int> ansesInt = (from p in dbRepository.GetQuestionAnswers(QuestID)
                                            orderby p.AnsNum
                                            select p.AnsID).ToList();

                                List<int> scaleids = (from p in dbRepository.GetScalesForTest(testItem.TestId)
                                              where p.ScaleShortName == currentScale
                                              select p.ScaleID).ToList();

                                var scaleBalls = from p in dbRepository.GetScaleWeightForScaleAndAnswer(scaleids, ansesInt)
                                                 select new { p.AnsID, p.Weight, p.ScaleID };

                                resultSB.Append("<tr>");
                                if (isBaseScale)
                                    resultSB.Append("<td nowrap=\"nowrap\">" + baseScaleName + "</td>");
                                else
                                    resultSB.Append("<td nowrap=\"nowrap\">" + oppositeScaleName + "</td>");

                                int c = 0;
                                foreach (var itemScaleBall in scaleBalls)
                                {
                                    c++;
                                    if (isBaseScale)
                                    {
                                        //red
                                        if (item.AnsID == itemScaleBall.AnsID)
                                        {
                                            baseScaleBall += (double)itemScaleBall.Weight;
                                            fstc = Convert.ToDouble(item.AnsNumber);
                                            resultSB.Append("<td nowrap=\"nowrap\">" + "<b style=\"color:red;\">" +  itemScaleBall.Weight.ToString() + "</b></td>");
                                        }
                                        else
                                            resultSB.Append("<td nowrap=\"nowrap\">" + itemScaleBall.Weight.ToString() + "</td>");
                                    }
                                    else if (oppositeScaleName != "" && oppositeScaleName != "00")
                                    {
                                        //blue
                                        if (item.AnsID == itemScaleBall.AnsID)
                                        {
                                            oppositeScaleBall += (double)itemScaleBall.Weight;
                                            fstc -= Convert.ToDouble(c);
                                            resultSB.Append("<td nowrap=\"nowrap\">" + "<b style=\"color:blue;\">" + itemScaleBall.Weight.ToString() + "</b></td>");
                                        }
                                        else                                        
                                            resultSB.Append("<td nowrap=\"nowrap\">" + itemScaleBall.Weight.ToString() + "</td>");

                                        
                                    }
                                }

                                if (c < ansCount)
                                {
                                    for (int jj = c; jj < ansCount; jj++)
                                    {
                                        resultSB.Append("<td nowrap=\"nowrap\">0</td>");
                                    }
                                }

                                resultSB.Append("<td>" + item.QuestText + "</td>");
                                if (isBaseScale)
                                {
                                    resultSB.Append("<td BGCOLOR=\"yellow\" rowspan=\"2\" nowrap=\"nowrap\">{$fstc}</td>");
                                    resultSB.Append("<td BGCOLOR=\"yellow\" rowspan=\"2\" nowrap=\"nowrap\">{$scndc}</td>");
                                    resultSB.Append("<td BGCOLOR=\"yellow\" rowspan=\"2\" nowrap=\"nowrap\">{$thrdc}</td>");
                                }
                                else
                                {
                                    resultSB.Replace("{$fstc}", (Math.Abs(fstc) + 1).ToString());
                                    resultSB.Replace("{$scndc}", (ansCount).ToString());
                                    resultSB.Replace("{$thrdc}", ((Math.Abs(fstc) + 1)/(ansCount)).ToString("N2"));
                                    weightQuiestons.Add(new WeightQuiestons("", ((Math.Abs(fstc) + 1)/(ansCount)), QuestID));
                                }
                                resultSB.Append("</tr>");
                                //counter++;
                            }

                            ScaleValueS[baseScaleIndex % 2].Add(fstc);
                        }

                        ScaleIndexeS[baseScaleIndex] = baseScaleBall;
                        ScaleIndexeS[oppositeScaleIndex] = oppositeScaleBall;
                    }

                }

                resultSB.Append("<tr><td nowrap=\"nowrap\">" + baseScaleName + "</td><td colspan=\"" + (ansCount + 1).ToString() + "\"> Итоговая сумма весов:</td><td><bold>" + ScaleIndexeS[baseScaleIndex] + "</bold></td></tr>");
                resultSB.Append("<tr><td nowrap=\"nowrap\">" + oppositeScaleName + "</td><td colspan=\"" + (ansCount + 1).ToString() + "\">Итоговая сумма весов:</td><td><bold>" + ScaleIndexeS[oppositeScaleIndex] + "</bold></td></tr>");
            }


            resultSB.Append("</table>");
            resultSB.Append("</p>");
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
            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"1\" width=\"700px\">");
            resultSB.Append("<tr BGCOLOR=\"lightgray\"><th>Пространство шкал " + ScaleNameS[0] + "-" + ScaleNameS[1] + " vs " + ScaleNameS[2] + "-" + ScaleNameS[3] + "</th><th>Ось" + ScaleNameS[0] + "-" + ScaleNameS[1] + "</th><th>Ось" + ScaleNameS[2] + "-" + ScaleNameS[3] + "</th><th>Ось" + ScaleNameS[4] + "-" + ScaleNameS[5] + "</th></tr>");
            StringBuilder sb1 = new StringBuilder();


            for (int i = 0; i < OtherScaleIndex.Count + 2; i++)
            {
                sb1.Append("<tr><td nowrap=\"nowrap\">{scalename." + i + "}</td><td nowrap=\"nowrap\">{scaleindex01-02." + i + "}</td><td nowrap=\"nowrap\">{scaleindex03-04." + i + "}</td><td nowrap=\"nowrap\">{scaleindex05-06." + i + "}</td></tr>");
            }
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

            List<string> scn3 = new List<string>();
            List<double> sci3 = new List<double>();
            scn3.Add(getFullScaleName(ScaleNameS[4])); scn3.Add(getFullScaleName(ScaleNameS[5]));
            sci3.Add(ScaleIndexeS[4]); sci3.Add(ScaleIndexeS[5]);
            for (int i = 0; i < OtherScaleIndex3.Count; i++)
            {
                scn3.Add(OtherScaleName3[i]);
                sci3.Add(OtherScaleIndex3[i]);
            }
            for (int i = 0; i < scn1.Count; i++)
            {
                sb1 = sb1.Replace("{scalename." + i + "}", scn1[i]).Replace("{scaleindex01-02." + i + "}", sci1[i].ToString("N2")).Replace("{scaleindex03-04." + i + "}", (getBallFromOther(scn2, sci2, scn1[i]).ToString("N2"))).Replace("{scaleindex05-06." + i + "}", (getBallFromOther(scn3, sci3, scn1[i]).ToString("N2")));
            }
            resultSB.Append(sb1);
            resultSB.Append("</table>");
            resultSB.Append("</p>");
        }
        void run4(string baseScaleName, string oppositeScaleName, double baseScaleIndex, double oppositeScaleIndex, List<string> otherScaleName, List<double> otherScaleIndex, List<Charts.percentil> pxy)
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            double DiffScaleIndex = (double)baseScaleIndex - (double)oppositeScaleIndex;
            //resultSB.Append("<p class='rule_conseq'>");

            resultSB.Append("<table border=\"1\" width=\"233px\">");
            //string TestShortName = Helper.Helper.getParameterValue(this.tResultSB.ToString(), "TestShortName");
            resultSB.Append("<tr><th>&nbsp;</th><th nowrap=\"nowrap\" colspan=\"2\">Шкалы " + baseScaleName + " - " + oppositeScaleName + "</th></tr>");
            resultSB.Append("<tr><td nowrap=\"nowrap\">" + getFullScaleName(baseScaleName) + "</td><td >100%</td></tr>");
            pxy.Add(new Charts.percentil(getFullScaleName(baseScaleName), 100));

            {
                for (int i = 0; i < otherScaleName.Count; i++)
                {

                    resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * ((otherScaleIndex[i] + Math.Abs(oppositeScaleIndex)) / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                    pxy.Add(new Charts.percentil(otherScaleName[i], (100 * Math.Abs(DiffScaleIndex != 0 ? ((otherScaleIndex[i] + Math.Abs(oppositeScaleIndex)) / (DiffScaleIndex)) : 0))));

                    //resultSB.Append("<tr><td nowrap=\"nowrap\"> " + otherScaleName[i] + "</td><td nowrap=\"nowrap\" colspan=\"1\">" + Math.Round((100 * Math.Abs(otherScaleIndex[i] / DiffScaleIndex)), 2).ToString() + "% </td></tr>");
                    //pxy.Add(new Charts.percentil(otherScaleName[i], (100 * Math.Abs(DiffScaleIndex != 0 ? (otherScaleIndex[i] / (DiffScaleIndex)) : 0))));
                }
            }
            if (oppositeScaleName != "" && oppositeScaleName != "00")
            {
                resultSB.Append("<tr><td nowrap=\"nowrap\">" + getFullScaleName(oppositeScaleName) + "</td><td >0%</td></tr>");
                pxy.Add(new Charts.percentil(getFullScaleName(oppositeScaleName), 0));
            }

            resultSB.Append("</table>");

            //resultSB.Append("</p>");
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
            var phisicalPath = GlobalOptions.DababaseMode == DbMode.Local
                                 ? GlobalOptions.InterpretsFolder
                                 : AppDomain.CurrentDomain.BaseDirectory;

            string virtPath = "ScaleGraphs/" + Shared.Helper.GeneratePassword(14) + ".png";
            
            string fname = Path.Combine(phisicalPath, virtPath.Replace("/", "\\"));

            List<Charts.percentilxyz8> l1 = new List<Charts.percentilxyz8>();
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

            List<string> scn3 = new List<string>();
            List<double> sci3 = new List<double>();
            scn3.Add(getFullScaleName(ScaleNameS[4])); scn3.Add(getFullScaleName(ScaleNameS[5]));
            sci3.Add(ScaleIndexeS[4]); sci3.Add(ScaleIndexeS[5]);
            for (int i = 0; i < OtherScaleIndex3.Count; i++)
            {
                scn3.Add(OtherScaleName3[i]);
                sci3.Add(OtherScaleIndex3[i]);
            }
            for (int i = 0; i < scn1.Count; i++)
            {
                l1.Add(new Charts.percentilxyz8((scn1[i]),
                   sci1[i],
                    getBallFromOther(scn2, sci2, scn1[i]),
                    getBallFromOther(scn3, sci3, scn1[i])
            ));
            }

            var gr8 = new Charts.sGraph8(800, 600, l1, 0, ScaleNameS, 1);

            var fileName = Shared.Helper.GeneratePassword(10) + ".png";
            virtPath = "ScaleGraphs/" + fileName;

            fname = Path.Combine(phisicalPath, "ScaleGraphs\\" + fileName);
            
            gr8.saveImage(fname);

            resultSB.Append("<p><img src=\"../" + virtPath + "\"/></p><br/><br/>");

            List<Charts.percentilxyz8> l2 = new List<Charts.percentilxyz8>();

            foreach (var item in p1)
            {
                l2.Add(new Charts.percentilxyz8((item.name), item.value, getBallFromOtherScale(p2, item.name), getBallFromOtherScale(p3, item.name)));
            }
            Charts.sGraph8 gr81 = new Charts.sGraph8(800, 600, l2, 0, ScaleNameS, 2);
            virtPath = "ScaleGraphs/" + Shared.Helper.GeneratePassword(14) + ".png";
            fname = Path.Combine(phisicalPath, virtPath.Replace("/", "\\"));
            gr81.saveImage(fname);
            resultSB.Append("<p><img src=\"../" + virtPath + "\"/></p><br/><br/>");
        }

        private string getFullScaleName(string sname)
        {
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
            fillQuestIndexes();
            // step 1
            //CalcAndWriteBasicScales(0, 1, true);
            //CalcAndWriteBasicScales(2, 3, true);
            //CalcAndWriteBasicScales(4, 5, true);
            // step 2
            fillOtherScaleIndex(ScaleNameS[0], ScaleNameS[1], OtherScaleName, OtherScaleIndex, 0);
            fillOtherScaleIndex2(ScaleNameS[2], ScaleNameS[3], OtherScaleName2, OtherScaleIndex2, 0);
            fillOtherScaleIndex3(ScaleNameS[4], ScaleNameS[5], OtherScaleName3, OtherScaleIndex3, 0);
            //fillOtherScaleIndex2(ScaleNameS[2], ScaleNameS[3], OtherScaleName2, OtherScaleIndex2, weightQuiestons.Count / 3);
            //fillOtherScaleIndex3(ScaleNameS[4], ScaleNameS[5], OtherScaleName3, OtherScaleIndex3, 2 * (weightQuiestons.Count / 3));
            CalcAndWriteScalesSpace();

            resultSB.Append("<p class='rule_conseq'>");
            resultSB.Append("<table border=\"0\" width=\"700px\">");
            resultSB.Append("<tr><td>");
            run4(ScaleNameS[0], ScaleNameS[1], ScaleIndexeS[0], ScaleIndexeS[1], OtherScaleName, OtherScaleIndex, p1);
            resultSB.Append("</td><td>");
            run4(ScaleNameS[2], ScaleNameS[3], ScaleIndexeS[2], ScaleIndexeS[3], OtherScaleName2, OtherScaleIndex2, p2);
            resultSB.Append("</td><td>");
            run4(ScaleNameS[4], ScaleNameS[5], ScaleIndexeS[4], ScaleIndexeS[5], OtherScaleName3, OtherScaleIndex3, p3);
            resultSB.Append("</td></tr>");
            resultSB.Append("</table>");
            resultSB.Append("</p>");
            //WriteGraphs();
        }
    }

}
