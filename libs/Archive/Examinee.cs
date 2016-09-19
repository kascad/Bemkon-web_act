using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.Linq;
using System.Data.Linq;

namespace Archive
{
    /// <summary>
    /// Класс одного обследуемого
    /// </summary>
    public class Examinee
    {
        private Archive archive;

        private int id;
        public int Id
        {
            get { return id; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string tests;    // readonly  
        public string Tests
        {
            get { return tests; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private List<ExamineeTest> examineeTests;
        public List<ExamineeTest> ExamineeTests
        {
            get { return examineeTests; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Examinee(int id, string name, string description, Archive archive, string tests)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.tests = tests;
            this.archive = archive;
        }

        public Examinee(int id, string name, string description, Archive archive, List<ExamineeTest> examineeTests)
            : this(id, name, description, archive, "")
        {
            this.examineeTests = examineeTests;
        }

        public void DeleteExaminee()
        {
            XmlNode findNode = archive.xmlDocument.DocumentElement.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.id + "]");
            findNode.ParentNode.RemoveChild(findNode);

            archive.SaveArchive();
        }

        public bool SaveExaminee(string name, string description)
        {
            if (archive.ArchExaminees.Count(ex => this.id != ex.id &&
                ex.Name.Trim().ToLower() == name.Trim().ToLower()) == 0)
            {
                XmlNode findNode = archive.xmlDocument.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.id + "]");
                findNode["Name"].InnerText = name;
                findNode["Description"].InnerText = description;

                archive.SaveArchive();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddTest(int testID, string testName)
        {
            XmlNode findNode = archive.xmlDocument.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.id + "]/Tests");
            if (findNode == null)
            {
                findNode = archive.xmlDocument.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.id + "]");
                findNode.AppendChild(archive.xmlDocument.CreateElement("Tests"));
                findNode = archive.xmlDocument.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.id + "]/Tests");
            }

            findNode.AppendChild(archive.xmlDocument.CreateElement("Test"));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("ID"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(testID.ToString()));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("Name"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(testName));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("FullTime"));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("Date"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode(DateTime.Now.ToString()));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("IsFinished"));
            findNode.LastChild.LastChild.AppendChild(archive.xmlDocument.CreateTextNode("false"));

            findNode.LastChild.AppendChild(archive.xmlDocument.CreateElement("Results"));

            archive.SaveArchive();

            this.examineeTests.Add(new ExamineeTest(archive, this.id, testID, testName));
        }

        private void ReadTests()
        {
            XmlNode findNode = archive.xmlDocument.DocumentElement.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + this.id + "]");
            if (findNode != null)
            {
                examineeTests = new List<ExamineeTest>();

                foreach (XmlNode item in findNode.SelectNodes("Tests/Test"))
                {
                    DateTime dateOfTest;
                    if (!DateTime.TryParse(item["Date"].InnerText, out dateOfTest))
                        dateOfTest = DateTime.Now;
                    examineeTests.Add(new ExamineeTest(archive, id, Convert.ToInt32(item["ID"].InnerText),
                        item["Name"].InnerText, item["FullTime"].InnerText,
                        dateOfTest, bool.Parse(item["IsFinished"].InnerText),
                        new List<TestResult>()));
                }
            }
        }

        public ExamineeTest GetTest(int testID)
        {
            ReadTests();
            var examTestVar = this.examineeTests.Where(t => t.TestId == testID);
            if (examTestVar.Count() > 0)
            {
                return examTestVar.First();
            }
            else
            {
                return null;
            }
        }
    }
}
