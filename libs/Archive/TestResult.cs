using System;

namespace Archive
{
    public class TestResult
    {
        private int questID;
        public int QuestID
        {
            get { return questID; }
        }

        private int questNumber;
        public int QuestNumber
        {
            get { return questNumber; }
        }

        private string questText;
        public string QuestText
        {
            get { return questText; }
        }

        private int ansID;
        public int AnsID
        {
            get { return ansID; }
        }

        private string ansText;
        public string AnsText
        {
            get { return ansText; }
        }

        private string ansNumber;
        public string AnsNumber
        {
            get { return ansNumber; }
        }

        private string time;
        public string Time
        {
            get { return time; }
        }

        public TestResult(int questID, int questNumber, string questText)
        {
            this.questID = questID;
            this.questNumber = questNumber;
            this.questText = questText;
        }

        public TestResult(int questID, int questNumber, string questText, int ansID, string ansText, string time, string ansNumber)
            : this(questID, questNumber, questText)
        {         
            this.ansID = ansID;
            this.ansText = ansText;
            this.time = time;
            this.ansNumber = ansNumber;
        }

    }
}
