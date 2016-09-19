using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public delegate void GraphChoiseDelegate(object sender, GraphChoiseEventArgs e);

    public class GraphChoiseEventArgs : EventArgs
    {
        public int ansID;

        public GraphChoiseEventArgs(int ansID)
        {
            this.ansID = ansID;
        }
    }
}
