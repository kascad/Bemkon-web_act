using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using Graphics;

namespace Interpret.Rules
{
    public class Rule_02 : Rule
    {
        public Rule_02(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        public Rule_02(string ruleTxt, StringBuilder resultSB, Archive.Examinee examinee) : 
            base(ruleTxt, resultSB, examinee) { }
        public static bool isItRule(string ruleTxt)
        {
            return isItRule(ruleTxt, null);
        }
        public static bool isItRule(string ruleTxt, StringBuilder resultSB)
        {
            //«-04@TestShortName@QuestNum  @Text», или «#@TestShortName @QuestNum @Text»
            string pattern = @"^-02" + tTestShortName + @"$";
            ruleTxt = Helper.Helper.prepareRule(ruleTxt);
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"^#" + tTestShortName + @"\ *S$";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1 && matches[0].Groups.Count == 2 && matches[0].Groups["TestShortName"].Length == 3)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    }
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
                    return true;
                }

            return false;
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public override void Run()
        {
            // Temporary ignore this rule
            return;
            
            //if (isItRule(ruleTxt, this.resultSB))
            //{
            //    string TestShortName = Helper.Helper.getParameterValue(resultSB.ToString(), "TestShortName");
            //    this.tests.Clear();
            //    this.tests.Add(TestShortName);
            //    resultSB.Remove(0, resultSB.Length);
            //    ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            //    int testid = getTestIDByName(TestShortName);
            //    //Archive.ExamineeTest testItem = getTestByName(TestShortName);
            //    if (testid == 0)
            //        return;
            //    var ss = from asd in db.Scales
            //             where asd.TestID == testid
            //             select asd;
            //    double BallAvr = 0;
            //    double BallMin = 0;
            //    double BallMax = 0;
            //    double BallValue = getScore2(TestShortName);
            //    string fname = "";
            //    foreach (var item in ss)
            //    {
            //        BallAvr = item.BallAVR.Value;
            //        BallMax = item.BallMax.Value;
            //        BallMin = item.BallMin.Value;
            //        //if (BallMax - BallMin > 0 && BallValue > 0)
            //        if (BallMax - BallMin > 0)
            //        {
            //            Charts.sGraph sg = new Charts.sGraph(500, 200, BallMax, BallMin, BallAvr, BallValue, 0, item.ScaleName);
            //            string virtPath = "ScaleGraphs//" + item.ScaleName.Replace("\\", "_").Replace("+", "_").Replace("/", "_") + this.GetHashCode().ToString() + ".png";
            //            fname = AppDomain.CurrentDomain.BaseDirectory + virtPath;

            //            //fname = @"c:\temp\" + item.ScaleName.Replace("\\", "_").Replace("+", "_").Replace("/", "_") + this.GetHashCode().ToString() + ".png";
            //            sg.saveImage(fname);
            //            resultSB.Append("<img src=\"/" + virtPath + "\"/><br/><br/>");
            //        }
            //    }
            //}
            //else
            //{
            //    resultSB.Remove(0, resultSB.Length);
            //}
        }
    }
}
