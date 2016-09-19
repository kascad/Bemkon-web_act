using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Testing.Data;

namespace Interpret.Rules
{
    public class Rule_10 : Rule
    {

        public Rule_10(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        public Rule_10(string ruleTxt, StringBuilder resultSB, Archive.Examinee examinee, int ruleID) : base(ruleTxt, resultSB, examinee, ruleID) { }
        public static bool isItRule(string ruleTxt)
        {
            return isItRule(ruleTxt, null);
        }
        public bool isMultipleRulep
        {
            get
            {
                return isMultipleRule(ruleTxt, resultSB);
            }
        }
        private static bool isMultipleRule(string ruleTxt, StringBuilder resultSB)
        {
            string pattern = @"10" + tTestShortName;
            //            ruleTxt = Helper.Helper.prepareRule(ruleTxt);
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "0");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"T" + tTestShortName + "";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count > 1)
                {
                    if (resultSB != null)
                    {
                        int ik = 1;
                        Helper.Helper.addParameterValue(resultSB, "count", matches.Count.ToString());
                        foreach (Match item in matches)
                        {
                            Helper.Helper.addParameterValue(resultSB, "match" + ik.ToString(), ruleTxt.Substring(item.Index, (ik == matches.Count ? text.Length - item.Index : matches[ik].Index - item.Index)).Trim());
                            ik++;
                        }
                    }
                    return true;
                }
            }
            else if (matches.Count > 1)
            {
                if (resultSB != null)
                {
                    int ik = 1;
                    Helper.Helper.addParameterValue(resultSB, "count", matches.Count.ToString());
                    foreach (Match item in matches)
                    {
                        Helper.Helper.addParameterValue(resultSB, "match" + ik.ToString(), ruleTxt.Substring(item.Index, (ik == matches.Count ? text.Length - item.Index : matches[ik].Index - item.Index)).Trim());
                        ik++;
                    }
                }
                return true;
            }
            return false;
        }
        private static bool testRule(string ruleTxt, StringBuilder resultSB)
        {
            bool test = isIt10_1(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_2(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_3(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_4(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_5(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_6(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_7(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_8(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_9(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_10(ruleTxt, resultSB);
            if (test)
                return true;
            test = isIt10_11(ruleTxt, resultSB);
            if (test)
                return true;
            return false;
        }
        public bool isItRule(string ruleTxt, StringBuilder resultSB, int tt)
        {
            return isItRule(ruleTxt, resultSB);
        }
        public static bool isItRule(string ruleTxt, StringBuilder resultSB)
        {
            ruleTxt = Helper.Helper.prepareRule(ruleTxt);
            bool test = isMultipleRule(ruleTxt, resultSB);
            if (test)
            {
                if (resultSB != null)
                {
                    int rulecount = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "count"));
                    string rulename = "";
                    StringBuilder stb = new StringBuilder();
                    for (int i = 1; i <= rulecount; i++)
                    {
                        rulename = Helper.Helper.getParameterValue(resultSB.ToString(), "match" + i.ToString());
                        test = testRule("-" + rulename, stb);
                        if (!test)
                            return false;
                    }
                }
            }
            else
                test = testRule(ruleTxt, resultSB);
            return test;
        }
        private static bool isIt10_1(string ruleTxt, StringBuilder resultSB)
        {
            //«-04@TestShortName@QuestNum  @Text», или «#@TestShortName @QuestNum @Text»
            string pattern = @"^-10" + tTestShortName + @"$";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "1");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"^T" + tTestShortName + @"$";
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
                        //resultSB.Remove(0, resultSB.Length);
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    }
                    return true;
                }
            return false;
        }
        private static bool isIt10_2(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum @Oper@AnsNum» или 
            //«T@TestShortName Q@QuestNum@Oper@AnsNum».

            string pattern = @"^-10" + tTestShortName + @"\ *41" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + "$";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "2");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"^T" + tTestShortName + @"\ *Q" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + "$";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1 && matches[0].Groups.Count == 5 && matches[0].Groups["TestShortName"].Length == 3 && Helper.Helper.isNumeric(matches[0].Groups["QuestNum"].Value) && Helper.Helper.isNumeric(matches[0].Groups["AnsNum"].Value))
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "QuestNum", matches[0].Groups["QuestNum"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper", matches[0].Groups["Oper"].Value);
                        Helper.Helper.addParameterValue(resultSB, "AnsNum", matches[0].Groups["AnsNum"].Value);
                    }
                    return true;
                }
            }
            else
                if (matches.Count == 1 && matches[0].Groups.Count == 5 && matches[0].Groups["TestShortName"].Length == 3 && Helper.Helper.isNumeric(matches[0].Groups["QuestNum"].Value) && Helper.Helper.isNumeric(matches[0].Groups["AnsNum"].Value))
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "QuestNum", matches[0].Groups["QuestNum"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper", matches[0].Groups["Oper"].Value);
                        Helper.Helper.addParameterValue(resultSB, "AnsNum", matches[0].Groups["AnsNum"].Value);
                    }
                    return true;
                }
            return false;
        }
        private static bool isIt10_3(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum1  @Oper1@AnsNum1 41@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »

            string pattern = @"^-10" + tTestShortName + @"41";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "3");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {// TPE1 Q35 = 1 Q17 = 1
                pattern = @"^T" + tTestShortName + @"\ *Q";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    }
                    pattern = @"(?<casd>Q" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @")";//сканируем на группы
                    VRegExp = new Regex(pattern);
                    matches = VRegExp.Matches(text);
                    if (matches.Count >= 2)
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
                                Helper.Helper.addParameterValue(resultSB, "QuestNum" + ik.ToString(), match.Groups["QuestNum"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                                Helper.Helper.addParameterValue(resultSB, "AnsNum" + ik.ToString(), match.Groups["AnsNum"].Value);
                            }
                            if (!Helper.Helper.isNumeric(match.Groups["QuestNum"].Value) || !Helper.Helper.isNumeric(match.Groups["AnsNum"].Value))
                                return false;
                            ik++;
                        }
                        if (lastlen < text.Length)
                            return false;
                        return true;
                    }
                }
            }
            else
            {
                if (resultSB != null)
                {
                    Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                }
                pattern = @"(?<casd>41" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @")";//сканируем на группы
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count >= 2)
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
                            Helper.Helper.addParameterValue(resultSB, "QuestNum" + ik.ToString(), match.Groups["QuestNum"].Value);
                            Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                            Helper.Helper.addParameterValue(resultSB, "AnsNum" + ik.ToString(), match.Groups["AnsNum"].Value);
                        }
                        if (!Helper.Helper.isNumeric(match.Groups["QuestNum"].Value) || !Helper.Helper.isNumeric(match.Groups["AnsNum"].Value))
                            return false;
                        ik++;
                    }
                    if (lastlen < text.Length)
                        return false;
                    return true;
                }
            }
            return false;
        }
        private static bool isIt10_4(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum1  @Oper1@AnsNum1 41@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »
            string pattern = @"^-10" + tTestShortName + @"41";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "4");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {// TPE1 Q35 = 1 Q17 = 1
                pattern = @"^T" + tTestShortName + @"\ *Q";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    }
                    pattern = @"(?<casd>Q" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @")";//сканируем на группы
                    VRegExp = new Regex(pattern);
                    matches = VRegExp.Matches(text);
                    if (matches.Count >= 1)
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
                                Helper.Helper.addParameterValue(resultSB, "QuestNum" + ik.ToString(), match.Groups["QuestNum"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                                Helper.Helper.addParameterValue(resultSB, "AnsNum" + ik.ToString(), match.Groups["AnsNum"].Value);
                            }
                            if (!Helper.Helper.isNumeric(match.Groups["QuestNum"].Value) || !Helper.Helper.isNumeric(match.Groups["AnsNum"].Value))
                                return false;
                            ik++;
                        }
                        if (lastlen < text.Length)
                        {
                            pattern = @"Q" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @"\ *" + tText;
                            VRegExp = new Regex(pattern);
                            matches = VRegExp.Matches(text);
                            if (matches.Count == 1)
                            {
                                Helper.Helper.addParameterValue(resultSB, "Text", matches[0].Groups["Text"].Value);
                            }
                            else
                                return false;
                        }
                        return true;
                    }
                }
                //return true;
            }
            else
            {
                if (resultSB != null)
                {
                    Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                }
                pattern = @"(?<casd>41" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @")";//сканируем на группы
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count >= 1)
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
                            Helper.Helper.addParameterValue(resultSB, "QuestNum" + ik.ToString(), match.Groups["QuestNum"].Value);
                            Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                            Helper.Helper.addParameterValue(resultSB, "AnsNum" + ik.ToString(), match.Groups["AnsNum"].Value);
                        }
                        if (!Helper.Helper.isNumeric(match.Groups["QuestNum"].Value) || !Helper.Helper.isNumeric(match.Groups["AnsNum"].Value))
                            return false;
                        ik++;
                    }
                    if (lastlen < text.Length)
                    {
                        pattern = @"41" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @"\ *" + tText;
                        VRegExp = new Regex(pattern);
                        matches = VRegExp.Matches(text);
                        if (matches.Count == 1)
                        {
                            Helper.Helper.addParameterValue(resultSB, "Text", matches[0].Groups["Text"].Value);
                        }
                        else
                            return false;
                    }
                    return true;
                }
            }
            return false;
        }
        private static bool isIt10_5(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum1  @Oper1@AnsNum1 41@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »
            string pattern = @"^-10" + tTestShortName + @"41";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "5");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {// TPE1 Q35 = 1 Q17 = 1
                pattern = @"^T" + tTestShortName + @"\ *Q";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    }
                    pattern = @"(?<casd>Q" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @")";//сканируем на группы
                    VRegExp = new Regex(pattern);
                    matches = VRegExp.Matches(text);
                    if (matches.Count >= 1)
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
                                Helper.Helper.addParameterValue(resultSB, "QuestNum" + ik.ToString(), match.Groups["QuestNum"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                                Helper.Helper.addParameterValue(resultSB, "AnsNum" + ik.ToString(), match.Groups["AnsNum"].Value);
                            }
                            if (!Helper.Helper.isNumeric(match.Groups["QuestNum"].Value) || !Helper.Helper.isNumeric(match.Groups["AnsNum"].Value))
                                return false;
                            ik++;
                        }
                        if (lastlen < text.Length)
                        {
                            pattern = @"Q" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @"\ *#\ *!$";
                            VRegExp = new Regex(pattern);
                            matches = VRegExp.Matches(text);
                            if (matches.Count == 1)
                            {
                                return true;
                            }
                            else
                                return false;
                        }
                        return true;
                    }
                }
            }
            else
            {
                if (resultSB != null)
                {
                    Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                }
                pattern = @"(?<casd>41" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @")";//сканируем на группы
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count >= 1)
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
                            Helper.Helper.addParameterValue(resultSB, "QuestNum" + ik.ToString(), match.Groups["QuestNum"].Value);
                            Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                            Helper.Helper.addParameterValue(resultSB, "AnsNum" + ik.ToString(), match.Groups["AnsNum"].Value);
                        }
                        if (!Helper.Helper.isNumeric(match.Groups["QuestNum"].Value) || !Helper.Helper.isNumeric(match.Groups["AnsNum"].Value))
                            return false;
                        ik++;
                    }
                    if (lastlen < text.Length)
                    {
                        pattern = @"41" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @"\ *92\ *!$";
                        VRegExp = new Regex(pattern);
                        matches = VRegExp.Matches(text);
                        if (matches.Count == 1)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private static bool isIt10_6(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum1  @Oper1@AnsNum1 41@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »
            string pattern = @"^-10" + tTestShortName + @"\ *41" + tQuestNum + @"\ *" + tOper + @"\ *" + tAnsNum + @"\ *91\ *(?<RuleNum>\d+)$";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "6");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {

            }
            else
            {
                if (matches.Count == 1 && Helper.Helper.isNumeric(matches[0].Groups["QuestNum"].Value) && Helper.Helper.isNumeric(matches[0].Groups["AnsNum"].Value) && Helper.Helper.isNumeric(matches[0].Groups["RuleNum"].Value))
                {
                    Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    Helper.Helper.addParameterValue(resultSB, "QuestNum", matches[0].Groups["QuestNum"].Value);
                    Helper.Helper.addParameterValue(resultSB, "Oper", matches[0].Groups["Oper"].Value);
                    Helper.Helper.addParameterValue(resultSB, "AnsNum", matches[0].Groups["AnsNum"].Value);
                    Helper.Helper.addParameterValue(resultSB, "RuleNum", matches[0].Groups["RuleNum"].Value);
                    return true;
                }
            }
            return false;
        }
        private static bool isIt10_7(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum1  @Oper1@AnsNum1 41@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »
            string pattern = @"^-10" + tTestShortName + @"\ *42" + tQuestNum + @"";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "7");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {

                pattern = @"^T" + tTestShortName + @"\ *Q" + tQuestNum + @"";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);

                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "QuestNum", matches[0].Groups["QuestNum"].Value);
                    }
                    pattern = @"(?<casd>" + tOper + @"\ *" + tAnsNum + @")";//сканируем на группы
                    VRegExp = new Regex(pattern);
                    matches = VRegExp.Matches(text);
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "testcount", matches.Count.ToString());
                    }
                    if (matches.Count >= 2)
                    {
                        int ik = 1;
                        foreach (Match match in matches)
                        {
                            if (resultSB != null)
                            {
                                Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                                Helper.Helper.addParameterValue(resultSB, "AnsNum" + ik.ToString(), match.Groups["AnsNum"].Value);
                            }
                            if (!Helper.Helper.isNumeric(match.Groups["AnsNum"].Value))
                                return false;
                            ik++;

                        }
                        return true;
                    }
                }
            }
            else
            {
                if (resultSB != null)
                {
                    Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    Helper.Helper.addParameterValue(resultSB, "QuestNum", matches[0].Groups["QuestNum"].Value);
                }
                pattern = @"(?<casd>\ *" + tOper + @"\ *" + tAnsNum + @")";//сканируем на группы
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (resultSB != null)
                {
                    Helper.Helper.addParameterValue(resultSB, "testcount", matches.Count.ToString());
                }
                if (matches.Count >= 2)
                {
                    int ik = 1;
                    foreach (Match match in matches)
                    {
                        if (resultSB != null)
                        {
                            Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                            Helper.Helper.addParameterValue(resultSB, "AnsNum" + ik.ToString(), match.Groups["AnsNum"].Value);
                        }
                        if (!Helper.Helper.isNumeric(match.Groups["AnsNum"].Value))
                            return false;
                        ik++;

                    }
                    return true;
                }
            }
            /////////////////////

            return false;
        }
        private static bool isIt10_8(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName21@QuestNum1  @Oper1@AnsNum1 21@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »
            string pattern = @"^-10" + tTestShortName + @"\ *2[12]";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "8");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {

                pattern = @"^T" + tTestShortName + @"\ *S";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);

                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    }
                    pattern = @"S" + tScaleShortName + @"\ *" + tOper + @"\ *" + tBall + @"";
                    VRegExp = new Regex(pattern);
                    matches = VRegExp.Matches(text);

                    if (matches.Count >= 1)
                    {
                        if (resultSB != null)
                        {
                            Helper.Helper.addParameterValue(resultSB, "testcount", matches.Count.ToString());
                        }
                        int ik = 1;
                        int lastlen = 0;
                        foreach (Match match in matches)
                        {
                            lastlen = match.Length + match.Index;
                            if (resultSB != null)
                            {
                                Helper.Helper.addParameterValue(resultSB, "ScaleShortName" + ik.ToString(), match.Groups["ScaleShortName"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Ball" + ik.ToString(), match.Groups["Ball"].Value);
                            }
                            if (!Helper.Helper.isNumeric(match.Groups["Ball"].Value))
                                return false;
                            ik++;
                        }
                        //if (lastlen < text.Length)
                        //{
                        //    return false;
                        //}
                        return true;
                    }
                }
            }
            else
            {
                if (matches.Count == 1)
                {
                    bool result = false;
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                    }

                    int testcount = 0;
                    int ik = 1;
                    // exp 21
                    pattern = "21" + tScaleShortName + @"\ *" + tOper + @"\ *" + tBall + @"";
                    text = text.Substring(matches[0].Length - 2);
                    VRegExp = new Regex(pattern, RegexOptions.Singleline);
                    matches = VRegExp.Matches(text);
                    if (matches.Count >= 1)
                    {
                        testcount += matches.Count;

                        int lastlen = 0;
                        foreach (Match match in matches)
                        {
                            lastlen = match.Length + match.Index;
                            if (resultSB != null)
                            {
                                Helper.Helper.addParameterValue(resultSB, "ScaleShortName" + ik.ToString(), match.Groups["ScaleShortName"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Ball" + ik.ToString(), match.Groups["Ball"].Value);
                            }
                            if (!Helper.Helper.isNumeric(match.Groups["Ball"].Value))
                                return false;
                            ik++;
                        }
                        //if (lastlen < text.Length)
                        //{
                        //    return false;
                        //}
                        result = true;
                    }

                    // exp 22
                    pattern = "22" + tScaleShortName + @"\ *" + tOper1 + @"\ *" + tBall1 + @"\ *" + tOper2 + @"\ *" + tBall2 + @"";
                    VRegExp = new Regex(pattern, RegexOptions.Singleline);
                    matches = VRegExp.Matches(text);
                    if (matches.Count >= 1)
                    {
                        testcount += matches.Count * 2;

                        int lastlen = 0;
                        foreach (Match match in matches)
                        {
                            lastlen = match.Length + match.Index;
                            if (resultSB != null)
                            {
                                Helper.Helper.addParameterValue(resultSB, "ScaleShortName" + ik.ToString(), match.Groups["ScaleShortName"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper1"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Ball" + ik.ToString(), match.Groups["Ball1"].Value);
                                ik++;
                                Helper.Helper.addParameterValue(resultSB, "ScaleShortName" + ik.ToString(), match.Groups["ScaleShortName"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Oper" + ik.ToString(), match.Groups["Oper2"].Value);
                                Helper.Helper.addParameterValue(resultSB, "Ball" + ik.ToString(), match.Groups["Ball2"].Value);
                            }
                            if (!Helper.Helper.isNumeric(match.Groups["Ball1"].Value) || !Helper.Helper.isNumeric(match.Groups["Ball2"].Value))
                                return false;
                            ik++;
                        }
                        //if (lastlen < text.Length)
                        //{
                        //    return false;
                        //}
                        result = true;
                    }
                    if (testcount > 0 && resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "testcount", testcount.ToString());
                    }
                    return result;

                }

            }
            return false;
        }
        private static bool isIt10_9(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum1  @Oper1@AnsNum1 41@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »
            string pattern = @"^-10" + tTestShortName + @"\ *22" + tScaleShortName + @"\ *" + tOper1 + @"\ *" + tBall1 + @"\ *" + tOper2 + @"\ *" + tBall2 + @"";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "9");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"^T" + tTestShortName + @"\ *S" + tScaleShortName + @"\ *" + tOper1 + @"\ *" + tBall1 + @"\ *" + tOper2 + @"\ *" + tBall2 + @"$";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1 && Helper.Helper.isNumeric(matches[0].Groups["Ball1"].Value) && Helper.Helper.isNumeric(matches[0].Groups["Ball2"].Value))
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName", matches[0].Groups["ScaleShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper1", matches[0].Groups["Oper1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Ball1", matches[0].Groups["Ball1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper2", matches[0].Groups["Oper2"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Ball2", matches[0].Groups["Ball2"].Value);

                    }
                    return true;
                }
            }
            else
            {
                if (matches.Count == 1 && Helper.Helper.isNumeric(matches[0].Groups["Ball1"].Value) && Helper.Helper.isNumeric(matches[0].Groups["Ball2"].Value))
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName", matches[0].Groups["ScaleShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper1", matches[0].Groups["Oper1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Ball1", matches[0].Groups["Ball1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper2", matches[0].Groups["Oper2"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Ball2", matches[0].Groups["Ball2"].Value);

                    }

                    return true;
                }

            }
            return false;
        }
        private static bool isIt10_10(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum1  @Oper1@AnsNum1 41@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »
            string pattern = @"^-10" + tTestShortName + @"\ *31" + tScaleShortName1 + @"\ *" + tOper + @"\ *" + tScaleShortName2 + @"";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "10");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"^T" + tTestShortName + @"\ *S" + tScaleShortName1 + @"\ *" + tOper + @"\ *S" + tScaleShortName2 + @"$";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName1", matches[0].Groups["ScaleShortName1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName2", matches[0].Groups["ScaleShortName2"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper", matches[0].Groups["Oper"].Value);
                    }
                    return true;
                }
            }
            else
            {
                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName1", matches[0].Groups["ScaleShortName1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName2", matches[0].Groups["ScaleShortName2"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper", matches[0].Groups["Oper"].Value);
                    }
                    return true;
                }


            }
            return false;
        }
        private static bool isIt10_11(string ruleTxt, StringBuilder resultSB)
        {
            //«-10@TestShortName41@QuestNum1  @Oper1@AnsNum1 41@QuestNum2  @Oper2@AnsNum2 » или
            //«T@TestShortName Q@QuestNum1@Oper1@AnsNum1 Q@QuestNum2@Oper2@AnsNum2 »
            string pattern = @"^-10" + tTestShortName + @"\ *32" + tScaleShortName1 + @"\ *" + tOper1 + @"\ *" + tScaleShortName2 + @"\ *" + tOper2 + @"\ *" + tScaleShortName3 + @"$";
            string text = ruleTxt;
            MatchCollection matches;
            if (resultSB != null)
            {
                resultSB.Remove(0, resultSB.Length);
                Helper.Helper.addParameterValue(resultSB, "format", "11");
            }
            var VRegExp = new Regex(pattern);
            matches = VRegExp.Matches(text);
            if (matches.Count == 0)
            {
                pattern = @"^T" + tTestShortName + @"\ *S" + tScaleShortName1 + @"\ *" + tOper1 + @"\ *S" + tScaleShortName2 + @"\ *" + tOper2 + @"\ *S" + tScaleShortName3 + @"$";
                VRegExp = new Regex(pattern);
                matches = VRegExp.Matches(text);
                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName1", matches[0].Groups["ScaleShortName1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName2", matches[0].Groups["ScaleShortName2"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName3", matches[0].Groups["ScaleShortName3"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper1", matches[0].Groups["Oper1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper2", matches[0].Groups["Oper2"].Value);
                    }
                    return true;
                }
            }
            else
            {
                if (matches.Count == 1)
                {
                    if (resultSB != null)
                    {
                        Helper.Helper.addParameterValue(resultSB, "TestShortName", matches[0].Groups["TestShortName"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName1", matches[0].Groups["ScaleShortName1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName2", matches[0].Groups["ScaleShortName2"].Value);
                        Helper.Helper.addParameterValue(resultSB, "ScaleShortName3", matches[0].Groups["ScaleShortName3"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper1", matches[0].Groups["Oper1"].Value);
                        Helper.Helper.addParameterValue(resultSB, "Oper2", matches[0].Groups["Oper2"].Value);
                    }

                    return true;
                }

            }
            return false;
        }
        public override bool IsValid
        {
            get
            {
                //if (examinee != null && Helper.Helper.isInRules(examinee.Tests, ruleTxt))
                if (examinee != null && Helper.Helper.isInRules((examinee.Tests.Trim() != "" ? examinee.Tests.Trim() : Helper.Helper.getTestString(examinee)), ruleTxt))
                {
                    string format = Helper.Helper.getParameterValue(resultSB.ToString(), "format");
                    if (format == "0")
                        return IsValid0();
                    else
                        if (format == "1")
                            return true;
                        else
                            if (format == "2" || format == "6")
                            {
                                string TestShortName = Helper.Helper.getParameterValue(resultSB.ToString(), "TestShortName");
                                this.tests.Clear();
                                this.tests.Add(TestShortName);
                                int QuestNum = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "QuestNum"));
                                int AnsNum = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "AnsNum"));
                                string Oper = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper");
                                Archive.ExamineeTest testItem = getTest(TestShortName);
                                if (testItem == null || !testItem.IsFinished)
                                    return false;

                                return Helper.Helper.isIdentity(Helper.Helper.getAnsNumber(testItem, QuestNum), AnsNum, Oper);
                            }
                            else
                                if (format == "3" || format == "5" || format == "4")
                                {
                                    string TestShortName = Helper.Helper.getParameterValue(resultSB.ToString(), "TestShortName");
                                    this.tests.Clear();
                                    this.tests.Add(TestShortName);
                                    int testcount = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "testcount"));
                                    bool testsuccess = true;
                                    int QuestNum = 0;
                                    int AnsNum = 0;
                                    string Oper = "";
                                    for (int i = 1; i <= testcount; i++)
                                    {
                                        QuestNum = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "QuestNum" + i.ToString()));
                                        AnsNum = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "AnsNum" + i.ToString()));
                                        Oper = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper" + i.ToString());
                                        Archive.ExamineeTest testItem = getTest(TestShortName);
                                        if (testItem == null || !testItem.IsFinished)
                                            return false;

                                        testsuccess = testsuccess && Helper.Helper.isIdentity(Helper.Helper.getAnsNumber(testItem, QuestNum), AnsNum, Oper);
                                    }
                                    return testsuccess;

                                }
                                else if (format == "7")
                                {
                                    string TestShortName = Helper.Helper.getParameterValue(resultSB.ToString(), "TestShortName");
                                    this.tests.Clear();
                                    this.tests.Add(TestShortName);
                                    int testcount = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "testcount"));
                                    bool testsuccess = true;
                                    int QuestNum = 0;
                                    int AnsNum = 0;
                                    string Oper = "";
                                    QuestNum = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "QuestNum"));
                                    for (int i = 1; i <= testcount; i++)
                                    {

                                        AnsNum = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "AnsNum" + i.ToString()));
                                        Oper = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper" + i.ToString());
                                        Archive.ExamineeTest testItem = getTest(TestShortName);
                                        if (testItem == null || !testItem.IsFinished)
                                            return false;

                                        testsuccess = testsuccess && Helper.Helper.isIdentity(Helper.Helper.getAnsNumber(testItem, QuestNum), AnsNum, Oper);
                                    }
                                    return testsuccess;
                                }
                                else if (format == "8")
                                {
                                    string TestShortName = Helper.Helper.getParameterValue(resultSB.ToString(), "TestShortName");
                                    this.tests.Clear();
                                    this.tests.Add(TestShortName);
                                    int testcount = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "testcount"));
                                    bool testsuccess = true;
                                    string ScaleShortName = "";
                                    double Ball = 0;
                                    string Oper = "";
                                    for (int i = 1; i <= testcount; i++)
                                    {
                                        ScaleShortName = (Helper.Helper.getParameterValue(resultSB.ToString(), "ScaleShortName" + i.ToString()));

                                        Ball = Helper.Helper.getSafeDouble(Helper.Helper.getParameterValue(resultSB.ToString(), "Ball" + i.ToString()));


                                        Oper = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper" + i.ToString());
                                        testsuccess = testsuccess && Helper.Helper.isIdentity(getScore1(TestShortName, ScaleShortName), Ball, Oper, false);
                                    }
                                    return testsuccess;
                                }
                                else if (format == "9")
                                {
                                    string TestShortName = Helper.Helper.getParameterValue(resultSB.ToString(), "TestShortName");
                                    this.tests.Clear();
                                    this.tests.Add(TestShortName);
                                    bool testsuccess = true;
                                    string ScaleShortName = (Helper.Helper.getParameterValue(resultSB.ToString(), "ScaleShortName"));
                                    double Ball1 = Helper.Helper.getSafeDouble(Helper.Helper.getParameterValue(resultSB.ToString(), "Ball1"));
                                    double Ball2 = Helper.Helper.getSafeDouble(Helper.Helper.getParameterValue(resultSB.ToString(), "Ball2"));
                                    string Oper1 = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper1");
                                    string Oper2 = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper2");

                                    double score = getScore1(TestShortName, ScaleShortName);
                                    testsuccess = Helper.Helper.isIdentity(score, Ball1, Oper1, false) && Helper.Helper.isIdentity(score, Ball2, Oper2, false);
                                    return testsuccess;
                                }
                                else if (format == "10")
                                {
                                    string TestShortName = Helper.Helper.getParameterValue(resultSB.ToString(), "TestShortName");
                                    this.tests.Clear();
                                    this.tests.Add(TestShortName);
                                    bool testsuccess = true;
                                    string ScaleShortName1 = (Helper.Helper.getParameterValue(resultSB.ToString(), "ScaleShortName1"));
                                    string ScaleShortName2 = (Helper.Helper.getParameterValue(resultSB.ToString(), "ScaleShortName2"));
                                    string Oper = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper");

                                    double b1 = getScore1(TestShortName, ScaleShortName1);
                                    double b2 = getScore1(TestShortName, ScaleShortName2);
                                    testsuccess = Helper.Helper.isIdentity(b1, b2, Oper, false);
                                    return testsuccess;
                                }
                                else if (format == "11")
                                {
                                    string TestShortName = Helper.Helper.getParameterValue(resultSB.ToString(), "TestShortName");
                                    this.tests.Clear();
                                    this.tests.Add(TestShortName);
                                    bool testsuccess = true;
                                    string ScaleShortName1 = (Helper.Helper.getParameterValue(resultSB.ToString(), "ScaleShortName1"));
                                    string ScaleShortName2 = (Helper.Helper.getParameterValue(resultSB.ToString(), "ScaleShortName2"));
                                    string ScaleShortName3 = (Helper.Helper.getParameterValue(resultSB.ToString(), "ScaleShortName3"));
                                    string Oper1 = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper1");
                                    string Oper2 = Helper.Helper.getParameterValue(resultSB.ToString(), "Oper2");

                                    double b1 = getScore1(TestShortName, ScaleShortName1);
                                    double b2 = getScore1(TestShortName, ScaleShortName2);
                                    double b3 = getScore1(TestShortName, ScaleShortName3);
                                    testsuccess = Helper.Helper.isIdentity(b1, b2, Oper1, false);
                                    testsuccess = testsuccess && Helper.Helper.isIdentity(b2, b3, Oper2, false);
                                    return testsuccess;
                                }
                    return true;
                }
                else
                    return false;

            }
        }
        public void runIt()
        {
            string format = Helper.Helper.getParameterValue(resultSB.ToString(), "format");
            if (format == "0" || format == "1" || format == "2" || format == "3" || format == "7" || format == "8" || format == "9" || format == "10" || format == "11")
            {
                string TestShortName = Helper.Helper.getParameterValue(this.resultSB.ToString(), "TestShortName");
                this.tests.Clear();
                this.tests.Add(TestShortName);
                resultSB.Remove(0, resultSB.Length);

                if (format != "0")
                {
                    Archive.ExamineeTest testItem = getTest(TestShortName);
                    if (testItem == null || !testItem.IsFinished)
                        return;
                }


                //ProfessorTestingDataContext db = new ProfessorTestingDataContext();
                
                //var conseq = from f in db.Conseqs
                //             where f.RuleID == ruleID
                //             select f.ConseqText;


                
                string ConseqText = "";

                using (var dbRepository = DbRepositoryFactory.GetRepository())
                {
                    //foreach (var item in conseq)
                    {
                        var cons = dbRepository.GetConseqsByRule(ruleID).FirstOrDefault();

                        ConseqText = cons != null ? cons.ConseqText : "";
                    }
                }
                if (ConseqText.Length > 0)
                {
                    resultSB.Append("<p class='rule_conseq'>");
                    resultSB.Append(ConseqText);
                    resultSB.Append("</p>");
                }
            }
            else if (format == "4")
            {
                string textvalue = Helper.Helper.getParameterValue(this.resultSB.ToString(), "text");
                string TestShortName = Helper.Helper.getParameterValue(this.resultSB.ToString(), "TestShortName");
                this.tests.Clear();
                this.tests.Add(TestShortName);
                int testcount = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "testcount"));
                StringBuilder sb = new StringBuilder();

                Archive.ExamineeTest testItem = getTest(TestShortName);
                if (testItem == null || !testItem.IsFinished)
                    return;

                int QuestNum = 0;
                if (textvalue.Substring(0, 1) == '"'.ToString())
                {
                    sb.Append("<h1>");
                    sb.Append(textvalue.Substring(1, textvalue.Length - 2));
                    sb.Append("</h1>");
                }
                //string st;
                if (testcount > 0)
                { 
                    for (int i = 1; i <= testcount; i++)
                    {
                        QuestNum = Convert.ToInt32(Helper.Helper.getParameterValue(this.resultSB.ToString(), "QuestNum" + i.ToString()));

                        string st = Helper.Helper.getAnsText(examinee, TestShortName, QuestNum);
                        if (st == null)
                            continue;
                        
                        if (st == "" && textvalue.Substring(0, 1) != '"'.ToString())
                        {
                            sb.Append("<p>");
                            sb.Append(textvalue);
                            sb.Append("</p>");
                        }
                        else
                            if (st != "" && textvalue.Substring(0, 1) == '"'.ToString())
                            {
                                sb.Append("<p>");
                                sb.Append(" " + st);
                                sb.Append("</p>");
                            }
                            else if (textvalue.Substring(0, 1) != '"'.ToString())
                            {
                                sb.Append("<p>");
                                sb.Append(textvalue + " " + st);
                                sb.Append("</p>");
                            }
                    }
                }
                resultSB.Remove(0, resultSB.Length);
                resultSB = sb;

            }
            else if (format == "5")
            {
                string TestShortName = Helper.Helper.getParameterValue(this.resultSB.ToString(), "TestShortName");
                this.tests.Clear();
                this.tests.Add(TestShortName);
                resultSB.Remove(0, resultSB.Length);
                Archive.ExamineeTest testItem = getTest(TestShortName);
                if (testItem == null || !testItem.IsFinished)
                    return;

                Helper.Helper.addParameterValue(resultSB, "task", "remove");
            }
            else if (format == "6")
            {
                string RuleNum = Helper.Helper.getParameterValue(this.resultSB.ToString(), "RuleNum");
                string TestShortName = Helper.Helper.getParameterValue(this.resultSB.ToString(), "TestShortName");
                this.tests.Clear();
                this.tests.Add(TestShortName);
                resultSB.Remove(0, resultSB.Length);
                Archive.ExamineeTest testItem = getTest(TestShortName);
                if (testItem == null || !testItem.IsFinished)
                    return;

                Helper.Helper.addParameterValue(resultSB, "task", "goto=" + RuleNum);
            }
            else
            {
                resultSB.Remove(0, resultSB.Length);
            }



        }
        private bool IsValid0()
        {
            int rulecount = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "count"));
            string rulename = "";
            StringBuilder stb = new StringBuilder();
            bool test = true;
            string tstb = resultSB.ToString();
            for (int i = 1; i <= rulecount; i++)
            {
                rulename = Helper.Helper.getParameterValue(tstb, "match" + i.ToString());
                testRule("-" + rulename, resultSB);
                test = test && IsValid;
                if (!test)
                {
                    resultSB.Remove(0, resultSB.Length);
                    return false;
                }
            }
            return true;
        }

        public override void Run()
        {
            bool isrule = isItRule(ruleTxt, this.resultSB);
            if (!isrule)
            {
                resultSB.Remove(0, resultSB.Length);
                return;
            }
            string format = Helper.Helper.getParameterValue(resultSB.ToString(), "format");
            if (format == "0")//-10 multiple
            {
                int rulecount = Convert.ToInt32(Helper.Helper.getParameterValue(resultSB.ToString(), "count"));
                string rulename = "";
                StringBuilder stb = new StringBuilder();
                bool test = true;
                string tstb = resultSB.ToString();
                for (int i = 1; i <= rulecount; i++)
                {
                    rulename = Helper.Helper.getParameterValue(tstb, "match" + i.ToString());
                    testRule("-" + rulename, resultSB);
                    test = test && IsValid;
                    if (!test)
                    {
                        resultSB.Remove(0, resultSB.Length);
                        return;
                    }
                }
            }
            else
                if (!IsValid)
                {
                    resultSB.Remove(0, resultSB.Length);
                    return;
                }
            runIt();

        }
    }
}
