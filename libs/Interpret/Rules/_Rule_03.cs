using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpret.Rules
{
    class Rule_03 : Rule
    {
        public Rule_03(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        public static bool isItRule(string ruleTxt)
        {
            //«-03@TestShortName@Scale», или ««#@TestShortName S @Scale».
            ProfessorTestingDataContext db = new ProfessorTestingDataContext();
            var tests = from s in db.Tests
                        select s.ShortName;
            if (ruleTxt.IndexOf("-03") == 0)
            {
                string[] tres = ruleTxt.Split(new string[] { "-03" }, StringSplitOptions.None);
                if (tres.Length == 2 && tres[1].Trim() == tres[1])
                {

                    bool nextstep = false;
                    string testname = "";
                    foreach (string item in tests)//ищем совпадение короткого имя теста
                    {
                        if (item.ToLower() == tres[1].Substring(0, item.Length).ToLower())
                        {
                            nextstep = true;
                            testname = tres[1].Substring(0, item.Length).ToLower();
                            break;
                        }
                    }
                    if (nextstep && isNumeric(tres[1].Substring(testname.Length)))
                        return true;
                }
            }
            //#@TestShortName S @Scale
            if (ruleTxt.IndexOf("#") == 0)
            {
                string[] tres = ruleTxt.Split(new string[] { "#" }, StringSplitOptions.None);
                if (tres.Length == 2)
                {
                    string[] tres1 = tres[1].Split(' ');
                    if (tres1.Length == 3 && tres1[1] == @"S")
                    {

                        bool nextstep = false;
                        string testname = "";
                        foreach (string item in tests)//ищем совпадение короткого имя теста
                        {
                            if (item.ToLower() == tres1[0].ToLower())
                            {
                                nextstep = true;
                                testname = tres1[0].ToLower();
                                break;
                            }
                        }
                        if (nextstep && isNumeric(tres1[2]))
                            return true;
                        //return true;
                    }
                }

            }
            return false;
        }
        public static bool isNumeric(string s)
        {
            int num = -1;
            try
            {
                num = Convert.ToInt32(s);
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
            return true;
        }
        public override bool IsValid
        {
            get { return true; }
        }

        public override void Run()
        {
        }
    }
}
