using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpret.Rules
{
    class Rule_01 : Rule
    {
        public Rule_01(string ruleTxt, StringBuilder resultSB) : base(ruleTxt, resultSB) { }
        
        public override bool IsValid
        {   
            get { return false; }
        }

        public override void Run()
        {           
        }
    }
}
