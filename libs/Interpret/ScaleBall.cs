using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpret
{
    public class ScaleBall
    {
        private int scaleID;

        public int ScaleID
        {
            get { return scaleID; }
        }
        private string scaleName;

        public string ScaleName
        {
            get { return scaleName; }
            set { scaleName = value; }
        }

        private double ball;

        public double Ball
        {
            get { return ball; }
            set { ball = value;  } 
        }

        private int testID;

        public int TestID
        {
            get { return testID; }
        }

        private int ansID;

        public int AnsID
        {
            get { return ansID; }
            
        }
        public ScaleBall(int scaleID, double ball, int testID)
        {
            this.scaleID = scaleID;
            this.ball = ball;
            this.testID = testID;
        }
        public ScaleBall(int scaleID, double ball, int testID, int ansID)
        {
            this.scaleID = scaleID;
            this.ball = ball;
            this.testID = testID;
            this.ansID = ansID;
        }
        

    }
}
