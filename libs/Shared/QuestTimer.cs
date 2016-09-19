using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public class QuestTimer : TimeCounter
    {
        public void Reset()
        {
            this.hour = 0;
            this.minute = 0;
            this.second = 0;
        }
    }
}
