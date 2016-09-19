using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Interpret.Rules
{
    public class Rule_04 : Rule
    {
        //private int testid;
        public Rule_04(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        public Rule_04(string ruleTxt, StringBuilder resultSB, Archive.Examinee examinee) : base(ruleTxt, resultSB, examinee) { }
        public static bool isItRule(string ruleTxt)
        {
            return isItRule(ruleTxt, null);
        }
        private static bool isItRule(string ruleTxt, StringBuilder resultSB)
        {
            //«-04@TestShortName@QuestNum  @Text», или «#@TestShortName @QuestNum @Text»
            string pattern = @"^-04" + tTestShortName + @"" + tQuestNum + @"\ " + tText;
            ruleTxt = Helper.Helper.prepareRule(ruleTxt);
            string text = ruleTxt;
            MatchCollection matches;
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"^#" + tTestShortName + @"\ " + tQuestNum + @"\ " + tText;
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1 && matches[0].Groups.Count == 4 && Helper.Helper.isNumeric(matches[0].Groups["QuestNum"].Value) && matches[0].Groups["TestShortName"].Length == 3 && matches[0].Groups["Text"].Length > 0)
                {
                    if (resultSB != null)
                    {
                        resultSB.Remove(0, resultSB.Length);
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "QuestNum", matches[0].Groups["QuestNum"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Text", matches[0].Groups["Text"].Value);
                    }
                    return true;
                }
            }
            else
                if (matches.Count == 1 && matches[0].Groups.Count == 4 && Helper.Helper.isNumeric(matches[0].Groups["QuestNum"].Value) && matches[0].Groups["TestShortName"].Length == 3 && matches[0].Groups["Text"].Length > 0)
                {
                    if (resultSB != null)
                    {
                        resultSB.Remove(0, resultSB.Length);
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "QuestNum", matches[0].Groups["QuestNum"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Text", matches[0].Groups["Text"].Value);
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

            if (isItRule(ruleTxt, this.resultSB))
            {
                string textvalue = Helper.Helper.getParameterValue(this.resultSB.ToString(), "text");
                string TestShortName = Helper.Helper.getParameterValue(this.resultSB.ToString(), "TestShortName");
                string QuestNum = Helper.Helper.getParameterValue(this.resultSB.ToString(), "QuestNum");

                this.tests.Clear();
                this.tests.Add(TestShortName);

                resultSB.Remove(0, resultSB.Length);

                string tag1 = "";
                if (textvalue.Substring(0, 1) == '"'.ToString())
                    tag1 = "<h1>";
                else
                    tag1 = "<p class='rule_conseq'>";

                string ansText = null;
                if (examinee != null && examinee.ExamineeTests != null) // bug in archive dll
                    ansText = Helper.Helper.getAnsText(examinee, TestShortName, Convert.ToInt32(QuestNum));
                else
                    ansText = Helper.Helper.getAnsText(getTest(TestShortName), Convert.ToInt32(QuestNum));

                if (ansText != null)
                {
                    resultSB.Append(tag1);
                    resultSB.Append(textvalue + " " + ansText);
                    resultSB.Append(tag1.Insert(1, @"/"));                    
                }
            }
            else
            {
                resultSB.Remove(0, resultSB.Length);
            }
        }
    }
}
