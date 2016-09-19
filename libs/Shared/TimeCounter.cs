using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public class TimeCounter
    {
        protected int hour = 0;

        protected int minute = 0;
        private int Minute
        {
            get { return minute; }
            set
            {
                if (value == 60)
                {
                    minute = 0;
                    hour++;
                }
                else
                {
                    minute = value;
                }
            }
        }

        protected int second = 0;
        public int Second
        {
            get { return second; }
            set
            {
                if (value == 60)
                {
                    second = 0;
                    Minute++;
                }
                else
                {
                    second = value;
                }
            }
        }

        private string getNumWithNull(int num)
        {
            return num < 10 ? "0" + num.ToString() : num.ToString();
        }

        public string GetTime()
        {
            return getNumWithNull(hour) +
                ":" + getNumWithNull(minute) + ":" + getNumWithNull(second);
        }
    }
}
