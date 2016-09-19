using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpret.helper
{
    class Helper
    {
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
        public static string getParameterValue(string myparams, string paramname, string paramseparate)
        {
            string[] parames = myparams.Split(new string[] { paramseparate }, StringSplitOptions.None);
            for (int i = 0; i < parames.Length; i ++)
            {
                if (parames[i].ToLower() == paramname)
                    return parames[i + 1];
            }
            return "";
        }
        public static void addParameterValue(System.Text.StringBuilder resultSB, string paramname, string paramvalue, string paramseparate)
        {
            //string[] parames = myparams.Split(new string[] { paramseparate }, StringSplitOptions.None);
            resultSB.Append(paramname);
            resultSB.Append(paramseparate);
            resultSB.Append(paramvalue);
            resultSB.Append(paramseparate);
            //return "";
        }
    }

}
