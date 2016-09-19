using System;
using System.Collections.Generic;
using System.Text;

namespace loggEngine
{
    class fileLog
    {
        System.IO.StreamWriter logWriter = null;
        string logName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "logs\\";
        bool notClose = false;
        public int getRandom()
        {
            Random r = new Random();
            return r.Next();
        }
        public System.IO.StreamWriter getStream()
        {
            return logWriter;
        }
        public fileLog(System.IO.StreamWriter sw)
        {
            logWriter = sw;
            notClose = true;
        }
        public fileLog()
        {
            if (!System.IO.Directory.Exists(logName))
                System.IO.Directory.CreateDirectory(logName);
            try
            {
                logWriter = new System.IO.StreamWriter(logName + "log.txt", true);
            }
            catch (Exception)
            {

                logWriter = new System.IO.StreamWriter(logName + "log" + getRandom().ToString() + ".txt", true);
            }

        }
        public fileLog(string flogname)
        {
            try
            {
                logWriter = new System.IO.StreamWriter(logName + flogname, true);
            }
            catch (Exception)
            {
                logWriter = new System.IO.StreamWriter(logName + getRandom().ToString() + flogname, true);
            }
        }
        public fileLog(string flogpath,string flogname)
        {
            logName = flogpath;
            if (!System.IO.Directory.Exists(logName))
                System.IO.Directory.CreateDirectory(logName);
            try
            {
                logWriter = new System.IO.StreamWriter(logName + flogname, true);
            }
            catch (Exception)
            {
                logWriter = new System.IO.StreamWriter(logName + getRandom().ToString() + flogname, true);
            }
        }
        public void putLog(string msg)
        {
            //string filler = new string("-",msg.le
            logWriter.WriteLine("<" + DateTime.Now.ToString() + ">: " + "-------------------------------------------------------------------- ");
            logWriter.WriteLine("[" + msg + "]");
            logWriter.WriteLine("</" + DateTime.Now.ToString() + ">: " + "-------------------------------------------------------------------- ");
            logWriter.Flush();
        }

        ~fileLog()
        {
            if (!notClose)
            {
                try
                {
                    logWriter.Close();
                }
                catch (Exception)
                {

                    // throw;
                }
                //if (logWriter.)

                try
                {
                    logWriter.Dispose();
                }
                catch (Exception)
                {

                    //throw;
                }
            }
        }

    }
}
