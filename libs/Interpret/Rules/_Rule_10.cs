using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpret.Rules
{
    internal class Rule_10 : Rule
    {
        public Rule_10(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) {}

        public override bool IsValid
        {
            get { return true; }
        }

        public override void Run()
        {
          
        }
    }
}
