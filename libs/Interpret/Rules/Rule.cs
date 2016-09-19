using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Testing.Data;

namespace Interpret.Rules
{
    public abstract class Rule
    {
        /// static const
        /// 
        /// 
        public static string tDoubleNumber = @"[-|+]?[0-9]*[.]?[0-9]*";
        public static string tTestShortName = @"(?<TestShortName>[a-zA-ZА-Яа-яёЁ0-9]{3})";
        public static string tQuestNum = @"(?<QuestNum>\d+)";
        public static string tScaleShortName = @"(?<ScaleShortName>[a-zA-ZА-Яа-яёЁ0-9*-]{1,2})";
        public static string tScaleShortName1 = @"(?<ScaleShortName1>[a-zA-ZА-Яа-яёЁ0-9*-]{1,2})";
        public static string tScaleShortName2 = @"(?<ScaleShortName2>[a-zA-ZА-Яа-яёЁ0-9*-]{1,2})";
        public static string tScaleShortName3 = @"(?<ScaleShortName3>[a-zA-ZА-Яа-яёЁ0-9*-]{1,2})";
        public static string tOper = "(?<Oper>[»«►◄><=\u0010\u0011+]{1,2})";
        public static string tOper1 = "(?<Oper1>[»«►◄><=\u0010\u0011+]{1,2})";
        public static string tOper2 = "(?<Oper2>[»«►◄><=\u0010\u0011+]{1,2})";
        public static string tAnsNum = @"(?<AnsNum>\d+)";
        //public static string tBall3 = @"(?<Ball3>[0-9]+)";
        public static string tBall2 = @"(?<Ball2>" + tDoubleNumber + ")";
        public static string tBall1 = @"(?<Ball1>" + tDoubleNumber + ")";
        public static string tBall = @"(?<Ball>" + tDoubleNumber + ")";
        public static string tText = @"(?<Text>[А-Яа-яёЁ" + '"' + " (:)_-]+)$";
        public static string tScale = @"(?<Scale>\d+)";
        public static string tSpacer = @"\ *";
        protected string ruleTxt;
        protected StringBuilder resultSB;
        protected Archive.Examinee examinee = null;
        protected int ruleID = 0;
        protected List<string> tests = new List<string>();
        protected bool dumpAnswers = false;

        public Rule(string ruleTxt, StringBuilder resultSB)
        {
            this.resultSB = resultSB;
            this.ruleTxt = ruleTxt;
        }

        public Rule(string ruleTxt, StringBuilder resultSB, Archive.Examinee examinee, int ruleID)
        {
            this.resultSB = resultSB;
            this.ruleTxt = ruleTxt;
            this.examinee = examinee;
            this.ruleID = ruleID;
        }

        public Rule(string ruleTxt, StringBuilder resultSB, Archive.Examinee examinee)
        {
            this.resultSB = resultSB;
            this.ruleTxt = ruleTxt;
            this.examinee = examinee;
        }

        public void SetDumpAnswers()
        {
            dumpAnswers = true; 
        }


        public Archive.ExamineeTest getTest(string tname)
        {
            try
            {
                if (examinee != null)
                    examinee.GetTest(0);
                if (examinee != null && examinee.ExamineeTests == null)
                {
                    //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
                    using (var dbRepository = DbRepositoryFactory.GetRepository())
                    {
                        //var tests = from ss in db.Tests
                        //            where ss.ShortName.ToLower() == tname.ToLower()
                        //            select ss.TestID;


                        //int testid = tests.First();

                        var test = dbRepository.GetTest(tname.ToLower());
                        var testid = test != null ? test.TestID : 0;

                        return examinee.GetTest(testid);
                    }
                }
                else if (examinee != null && examinee.ExamineeTests != null)
                {
                    return Helper.Helper.getExamineeTest(examinee, tname);
                }
            }
            catch (Exception)
            {



            }

            return null;
        }
        public Archive.ExamineeTest getTest(int testid)
        {
            if (examinee != null)
            {
                return examinee.GetTest(testid);
            }
            else return null;
        }
        public double getScore1Old(string TestShortName, string ScaleShortName)
        {
            Archive.ExamineeTest testItem = getTest(TestShortName);
            if (testItem == null || !testItem.IsFinished)
            {
                //Trace.TraceError(String.Format("Test is not complete! TestShortName={0}\r\n", TestShortName));
                return 0.0; // тест не пройден
            }

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                var scales = from sa in dbRepository.GetScalesForTest(testItem.TestId)
                             where sa.ScaleShortName == ScaleShortName
                             select sa.ScaleID;
                int count = scales.Count();
                if (count == 0)
                {
                    Trace.TraceError(String.Format("Scale not found! TestShortName={0} ScaleShortName={1}\r\n", TestShortName, ScaleShortName));
                    return 0.0;
                }

                double res = 0;
                if (Interpretation.scaleBalls == null)
                {
                    foreach (int itemScale in scales)
                    {
                        foreach (var testResult in testItem.TestResults)
                        {
                            res += dbRepository.GetScaleWeightForScaleAndAnswer(itemScale, testResult.AnsID);
                        }
                    }
                }
                else
                {
                    int scaleId = scales.First();
                    foreach (var item in Interpretation.scaleBalls)
                    {
                        try
                        {
                            if (item.TestID == testItem.TestId && item.ScaleID == scaleId)
                                res += item.Ball;
                        }
                        catch (Exception)
                        {

                            //throw;
                        }

                    }
                }

                return res;
            }
        }
        public static double getScore1(Archive.ExamineeTest testItem, int ScaleID)
        {
            if (testItem == null || !testItem.IsFinished)
            {
                //Trace.TraceError(String.Format("Test is not complete! TestItemName={0}\r\n", testItem.Name));
                return 0.0; // тест не пройден
            }
            double res = 0;

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                if (Interpretation.scaleBalls == null)
                {
                    List<int> ansIDs = Helper.Helper.getAnsIds(testItem);
                    //var scales = from s in dbRepository.GetScalesForTest(testItem.TestId) where s.ScaleID == ScaleID select s.ScaleID;

                    var tRes = from p in dbRepository.GetScaleWeightForScale(ScaleID)
                               where ansIDs.Contains((int)p.AnsID)
                               select p.Weight.HasValue ? p.Weight : 0;

                    foreach (var item in tRes)
                    {
                        res += Interpretation.GetCorrectBall((double)(tRes).Sum(), ScaleID);
                        break;
                    }

                    //
                    //foreach (var testResult in testItem.TestResults)
                    //{
                    //    double scaleWeight = dbRepository.GetScaleWeightForScaleAndAnswer(ScaleID, testResult.AnsID);
                    //    res += (double)Interpretation.GetCorrectBall(scaleWeight, (int)ScaleID);
                    //}
                }
                else
                {
                    foreach (var item in Interpretation.scaleBalls)
                    {
                        if (item.TestID == testItem.TestId && item.ScaleID == ScaleID)
                            res += item.Ball;
                    }
                }
            }

            return res;
        }
        public double getScore1(string TestShortName, string ScaleShortName)
        {
            Archive.ExamineeTest testItem = getTest(TestShortName);
            if (testItem == null || !testItem.IsFinished)
            {
                //Trace.TraceError(String.Format("Test is not complete! TestShortName={0}\r\n", TestShortName));
                return 0.0; // тест не пройден
            }
            int scaleId = getScaleIdByName(testItem.TestId, ScaleShortName);
            return getScore1(testItem, scaleId);
        }
        public double getScore2(string TestShortName)
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();

            int testid = 0;
            Archive.ExamineeTest testItem = null;
            //Archive.ExamineeTest testItem = getTestByName(TestShortName);
            examinee.GetTest(0);
            if (examinee != null && examinee.ExamineeTests == null)
            {

                testid = getTestIDByName(TestShortName);
                testItem = getTest(testid);
            }
            //if (testid == 0)
            else
            {

                testItem = Helper.Helper.getExamineeTest
                        (
                            this.examinee,
                            TestShortName
                        );
                testid = (testItem != null ? testItem.TestId : 0);
            }
            double res = 0;
            if (testItem != null)
            {
                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {
                    if (Interpretation.scaleBalls == null)
                    {
                        //var scales = from sa in db.Scales
                        //             where sa.TestID == testid
                        //             select sa.ScaleID;

                        var scales = from sa in dbRepository.GetScalesForTest(testid)
                                     select sa.ScaleID;


                        foreach (int itemScale in scales)
                        {
                            foreach (var testResult in testItem.TestResults)
                            {
                                //var tRes = from p in db.ScaleWeights
                                //           where p.AnsID == testResult.AnsID && p.ScaleID == itemScale
                                //           select p.Weight.HasValue ? p.Weight : 0;

                                double scaleWeight = dbRepository.GetScaleWeightForScaleAndAnswer(itemScale, testResult.AnsID);
                                res += (double) Interpretation.GetCorrectBall(scaleWeight, (int) itemScale);
                            }
                        }
                    }
                    else
                    {

                        foreach (var item in Interpretation.scaleBalls)
                        {
                            try
                            {
                                if (item.TestID == testid)
                                    res += item.Ball;
                            }
                            catch (Exception)
                            {

                                //throw;
                            }

                        }
                    }
                }
            }
            return res;

        }
        public int getTestIDByName(string tname)
        {
            try
            {
                //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
                //var tests = from ss in db.Tests
                //            where ss.ShortName.ToLower() == tname.ToLower()
                //            select ss.TestID;
                //return tests.First();

                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {
                    var test = dbRepository.GetTest(tname.ToLower());
                    return test != null ? test.TestID : 0;
                }
                //return examinee.GetTest(testid);
            }
            catch (Exception)
            {

                //throw;
            }

            return 0;
        }
        public int getScaleIdByName(int testId, string ScaleShortName)
        {
            //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            //var scales = (from s in db.Scales
            //              where s.ScaleShortName == ScaleShortName
            //              select s.ScaleID);

            using (var dbRepository = DbRepositoryFactory.GetRepository())
            {
                var scales = dbRepository.GetScalesForTest(testId).Where(s => s.ScaleShortName == ScaleShortName);
                if (scales.Any())
                    return scales.First().ScaleID;
            }
            Trace.TraceError("Scale not found! TestID={0} ScaleShortName={1}\r\n", testId, ScaleShortName);
            return -1;
        }

        public StringBuilder getResult()
        {
            return resultSB;
        }
        public abstract bool IsValid { get; }

        public abstract void Run();
    }
}
