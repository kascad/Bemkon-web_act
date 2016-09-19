using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpret.Rules
{
    class Rule_02 : Rule
    {
        public Rule_02(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        public static bool isItRule(string ruleTxt)
        {
            //«-02@TestShortName», или ««#@TestShortName S».
            if (ruleTxt.IndexOf("-02" )== 0)
            {
                string[] tres = ruleTxt.Split(new string[] { "-02" }, StringSplitOptions.None);
                if (tres.Length == 2 && tres[1].Trim() == tres[1])
                    return true;
            }
            if (ruleTxt.IndexOf("#") == 0)
            {
                string[] tres = ruleTxt.Split(new string[] { "#" }, StringSplitOptions.None);
                if (tres.Length == 2)
                {
                    string[] tres1 = tres[1].Split(' ');
                    if (tres1.Length == 2 && tres1[1] == @"S")
                        return true;
                }
                    
            }
            return false;
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
