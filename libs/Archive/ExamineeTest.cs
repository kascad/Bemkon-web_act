using System;
using System.Collections.Generic;
using System.Xml;

namespace Archive
{
    public class ExamineeTest
    {
        private Archive archive;
        int examineeID;

        private int testId; // readonly
        public int TestId
        {
            get { return testId; }
        }

        private string name; // readonly
        public string Name
        {
            get { return name; }
        }

        private DateTime date; // readonly
        public DateTime Date
        {
            get { return date; }
        }

        private string testTime;
        public string TestTime
        {
            get { return testTime; }
            set { testTime = value; }
        }

        private bool isFinished;
        public bool IsFinished
        {
            get { return isFinished; }
            set { isFinished = value; }
        }

        private List<TestResult> testResults; // readonly
        public List<TestResult> TestResults
        {
            get { return this.GetSavedTestResults(); }
        }

        public ExamineeTest(Archive archive, int examineeID, int testId, string name)
        {
            this.archive = archive;
            this.examineeID = examineeID;
            this.testId = testId;
            this.name = name;

            testResults = new List<TestResult>();
        }
        
        public ExamineeTest(Archive archive, int examineeID, int testId, string name, string testTime, 
            DateTime date, bool isFinished, List<TestResult> testResults) 
            : this(archive, examineeID, testId, name)
        {           
            this.testTime = testTime;
            this.date = date;
            this.isFinished = isFinished;
            this.testResults = testResults;
        }     

        public void AddTestResult(int questID, int ansID, string questText, string ansText, string time,
            int QuestNumber, int AnsNumber)
        {
            XmlNode findNode = archive.xmlDocument.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.examineeID + "]/Tests/Test[ID=" + this.testId + "]/Results");

            findNode.AppendChild(archive.xmlDocument.CreateElement("Result"));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("QuestID"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(questID.ToString()));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("QuestText"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(questText));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("AnsID"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(ansID.ToString()));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("AnsText"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(ansText));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("AnsNumber"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(AnsNumber.ToString()));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("Time"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(time));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("QuestNumber"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(QuestNumber.ToString()));

            archive.SaveArchive();
        }

        public List<TestResult> GetSavedTestResults()
        {
            List<TestResult> testResults = new List<TestResult>();
            XmlNode findNode = archive.xmlDocument.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.examineeID + "]/Tests/Test[ID=" + this.testId + "]/Results");
            foreach (XmlNode item in findNode)
            {
                testResults.Add(new TestResult(
                    Convert.ToInt32(item["QuestID"].InnerText),
                    item["QuestNumber"].InnerText != "" ? Convert.ToInt32(item["QuestNumber"].InnerText) : 0,
                    item["QuestText"].InnerText,
                    int.Parse(item["AnsID"].InnerText),
                    item["AnsText"].InnerText,
                    item["Time"].InnerText,
                    item["AnsNumber"].InnerText));
            }
            return testResults;
        }

        public void FinishTest(string time)
        {
            XmlNode findNode = archive.xmlDocument.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.examineeID + "]/Tests/Test[ID=" + this.testId + "]");
            findNode["IsFinished"].InnerText = "true";
            findNode["FullTime"].InnerText = time;

            archive.SaveArchive();
        }

        public void Delete()
        {
            XmlNode findNode = archive.xmlDocument.DocumentElement.
                SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.examineeID + "]/Tests/Test[ID=" + 
                this.testId + "]");
            findNode.ParentNode.RemoveChild(findNode);

            archive.SaveArchive();
        }     
    }
}
