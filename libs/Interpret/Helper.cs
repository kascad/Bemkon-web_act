using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Archive;

namespace Helper
{
    public class Helper
    {
        //private static CultureInfo userCultureInfo;

        public static bool isNumeric(string s)
        {
            double num = -1;
            if (!double.TryParse(s, out num))
            {
                if (s.Contains("."))
                    return isNumeric(s.Replace(".", ","));
                return false;
            }
            return true;
        }
        public static double getSafeDouble(string s)
        {
            double num = -1;
            if (!double.TryParse(s, out num))
            {
                if (s.Contains("."))
                    return getSafeDouble(s.Replace(".", ","));
            }
            return num;

        }
        public static List<int> getAnsIds(Archive.ExamineeTest test)
        {
            List<int> res = new List<int>();
            if (test != null && test.TestResults != null)
                foreach (var item in test.TestResults)
                {
                    res.Add(item.AnsID);
                }

            return res;
        }
        public static void fillScaleNames(Testing.Data.IDbRepository dbRepository, List<Interpret.ScaleBall> list)
        {
            if (list != null)
            {
                var scaleIDs = new List<int>();
                foreach (var item in list)
                {
                    if (!scaleIDs.Contains(item.ScaleID))
                    {
                        scaleIDs.Add(item.ScaleID);
                    }
                }

                var scales = from p in dbRepository.GetScales(scaleIDs)
                             select new { p.ScaleShortName, p.ScaleID };

                foreach (var item in scales)
                {
                    List<Interpret.ScaleBall> tItems = list.FindAll(test => test.ScaleID == item.ScaleID);
                    for (int i = 0; i < tItems.Count; i++)
                    {
                        tItems[i].ScaleName = item.ScaleShortName;
                    }
                }
            }
        }
        public static void setBall(List<Interpret.ScaleBall> scaleBalls, int scaleID, int testID, int ansID, double ball)
        {
            if (scaleBalls != null)
            {
                //foreach (var item in scaleBalls)
                Interpret.ScaleBall item = null;
                for (int i = 0; i < scaleBalls.Count; i++)
                {
                    item = scaleBalls[i];
                    if (item == null) // TODO рассмотреть подробнее почему так
                        continue;
                    if (scaleID != 0 && scaleID != item.ScaleID)
                        continue;
                    if (testID != 0 && testID != item.TestID)
                        continue;
                    if (ansID != 0 && ansID != item.AnsID)
                        continue;
                    item.Ball += ball;
                }
            }
        }
        public static string getQuestText(Archive.Examinee exam, int ansid)
        {
            foreach (Archive.ExamineeTest test in exam.ExamineeTests)
            {
                foreach (var item in test.TestResults)
                {
                    if (item.AnsID == ansid)
                        return item.QuestText;
                }
            }
            return "";
        }
        public static string getTestString(Archive.Examinee exam)
        {
            string res = "";
            if (exam != null && exam.ExamineeTests != null)
                foreach (var item in exam.ExamineeTests)
                {
                    res += item.Name + " ";
                }
            return res.Trim();
        }
        public static string prepareRule(string ruletxt, int mode)
        {
            if (mode == 1)
                return getFreeSpaceString(prepareRule(ruletxt));
            return prepareRule(ruletxt);

        }
        public static string prepareRule(string ruletxt)
        {
            //string res = "";
            string c1 = " ---";
            string c2 = " !";
            string c3 = " 99";
            string tmp = "";
            for (int i = 0; i < ruletxt.Length; i++)
            {
                if (i <= ruletxt.Length - c1.Length)
                    tmp = ruletxt.Substring(i, c1.Length);
                if (tmp == c1)
                    return ruletxt.Remove(i).Trim();
                if (i <= ruletxt.Length - c2.Length)
                    tmp = ruletxt.Substring(i, c2.Length);
                if (i <= ruletxt.Length - c2.Length - 1 && tmp == c2)
                {
                    if (i > ("92" + c2).Length && ruletxt.Substring(i - "92".Length, ("92" + c2).Length) != ("92" + c2)
                    && i > ("#" + c2).Length && ruletxt.Substring(i - "#".Length, ("#" + c2).Length) != ("#" + c2))//rule 5.5
                    {
                        return ruletxt.Remove(i).Trim();
                    }
                }
                if (i <= ruletxt.Length - c3.Length)
                    tmp = ruletxt.Substring(i, c3.Length);
                if (tmp == c3)
                    return ruletxt.Remove(i).Trim();
            }
            return ruletxt;
        }
        static string getFreeSpaceString(string s)
        {
            string res = "";
            bool spaceFound = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ' && !spaceFound)
                    res += s[i];
                else if (s[i] == ' ') spaceFound = true;
                else if (s[i] != ' ')
                {
                    spaceFound = false;
                    res += s[i];
                }
            }
            return res;
        }
        public static bool isIdentity(double num1, double num2, string oper)//перед сравнением проверяет не явяются ли числа отрицательными
        {//><=    
            return isIdentity(num1, num2, oper, true);
        }
        public static bool isIdentity(double num1, double num2, string oper, bool safe)//перед сравнением проверяет не явяются ли числа отрицательными
        {//><=
            if (safe && (num1 < 0 || num2 < 0))
                return false;

            if (oper == ">")
                if (num1 > num2)
                    return true;
                else return false;
            if (oper == "<")
                if (num1 < num2)
                    return true;
                else return false;
            if (oper == "=")
                if (num1 == num2)
                    return true;
                else return false;
            if (oper == "»" || oper == "►" || oper == "\u0010" || oper == ">=" || oper == "+")//>=
                if (num1 >= num2)
                    return true;
                else return false;
            if (oper == "«" || oper == "◄" || oper == "\u0011" || oper == "<=") //<=
                if (num1 <= num2)
                    return true;
                else return false;
            return false;
        }
        public static Archive.Archive getArchive(string path)
        {
            Archive.Archive archive = null;
            try
            {
                archive = new Archive.Archive((path == "" || path == string.Empty ? @"d:\test\NewTest.arh" : path));
            }
            catch (Exception)
            {

                //throw;
            }
            return archive;
        }
        public static Archive.Examinee getExaminee(Archive.Archive archive, int examnum)
        {
            Archive.Examinee examinee = null;
            if (archive != null)
            {
                try
                {
                    examinee = archive.getExaminee(examnum);
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            return examinee;
        }
        public static Archive.ExamineeTest getExamineeTest(Archive.Examinee examinee, string TestShortName)
        {
            Archive.ExamineeTest extest = null;
            if (examinee != null)
            {
                try
                {
                    var testsSelect = examinee.ExamineeTests.Where(test => test.Name == TestShortName);
                    if (testsSelect.Any())
                        extest = testsSelect.FirstOrDefault();
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            return extest;
        }
        public static bool isInRules(string rules, string rule)
        {
            return isInRules(rules, rule, "(!) ");
        }
        public static bool isInRules(string rules, string rule, string delimiter)
        {
            //if (rule == null || rules == null)
             //   return false;

            string[] lRules = rules.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            if (rules.Length > 3 && lRules.Length == 1)
                lRules = rules.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in lRules)
            {
                if (rule.ToLower().Contains(item.ToString().ToLower()))
                    return true;
            }
            return false;
        }
        public static Archive.TestResult getTestResultByQuestNumber(Archive.ExamineeTest extest, int QuestNum)
        {
            Archive.TestResult result = null;
            if (extest != null)
                result = extest.TestResults.FirstOrDefault(res => res.QuestNumber == Convert.ToInt32(QuestNum));
            return result;
        }
        public static string getAnsText(Archive.ExamineeTest extest, int QuestNumber)
        {
            Archive.TestResult result = getTestResultByQuestNumber(extest, QuestNumber);
            if (result != null)
                return result.AnsText;
            return "";
        }
        public static string getAnsText(Archive.Examinee examinee, string TestShortName, int QuestNumber)
        {
            Archive.ExamineeTest extest = getExamineeTest(examinee, TestShortName);
            Archive.TestResult result = getTestResultByQuestNumber(extest, QuestNumber);
            if (result != null)
                return result.AnsText;
            return null;
        }
        public static string getAnsText(int examnum, string TestShortName, int QuestNumber)
        {
            Archive.Archive archive = getArchive("");
            Archive.Examinee examinee = getExaminee(archive, examnum);
            Archive.ExamineeTest extest = getExamineeTest(examinee, TestShortName);
            Archive.TestResult result = getTestResultByQuestNumber(extest, QuestNumber);
            if (result != null)
                return result.AnsText;
            return null;
        }
        public static int getAnsNumber(Archive.ExamineeTest extest, int QuestNumber)
        {
            //Archive.ExamineeTest extest = getExamineeTest(examinee, TestShortName);
            Archive.TestResult result = getTestResultByQuestNumber(extest, QuestNumber);
            if (result != null)
                return Convert.ToInt32(result.AnsNumber);
            return -1;
        }
        public static int getAnsNumber(Archive.Examinee examinee, string TestShortName, int QuestNumber)
        {
            Archive.ExamineeTest extest = getExamineeTest(examinee, TestShortName);
            Archive.TestResult result = getTestResultByQuestNumber(extest, QuestNumber);
            if (result != null)
                return Convert.ToInt32(result.AnsNumber);
            return -1;
        }
        public static int getAnsNumber(int examnum, string TestShortName, int QuestNumber)
        {
            Archive.Archive archive = getArchive("");
            Archive.Examinee examinee = getExaminee(archive, examnum);
            Archive.ExamineeTest extest = getExamineeTest(examinee, TestShortName);
            Archive.TestResult result = getTestResultByQuestNumber(extest, QuestNumber);
            if (result != null)
                return Convert.ToInt32(result.AnsNumber);
            return -1;
        }
        public static string getParameterValue(string myparams, string paramname)
        {
            return getParameterValue(myparams, paramname, @"/\-");
        }
        public static string getParameterValue(string myparams, string paramname, string paramseparate)
        {
            string[] parames = myparams.Split(new string[] { paramseparate }, StringSplitOptions.None);
            for (int i = 0; i < parames.Length; i++)
            {
                if (parames[i].ToLower() == paramname.ToLower())
                    return parames[i + 1];
            }
            return "";
        }
        public static void addParameterValue(System.Text.StringBuilder resultSB, string paramname, string paramvalue)
        {
            addParameterValue(resultSB, paramname, paramvalue, @"/\-");
        }
        public static void addParameterValue(System.Text.StringBuilder resultSB, string paramname, string paramvalue, string paramseparate)
        {
            //string[] parames = myparams.Split(new string[] { paramseparate }, StringSplitOptions.None);
            if (resultSB != null)
            {
                resultSB.Append(paramname);
                resultSB.Append(paramseparate);
                resultSB.Append(paramvalue);
                resultSB.Append(paramseparate);
            }
            //return "";
        }
    }

}
