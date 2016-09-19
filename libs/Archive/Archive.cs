using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Schema;

namespace Archive
{
    /// <summary>
    /// Класс всех обследуемых орхива
    /// </summary>
    public class Archive
    {
        //---- Fields ----------------------

        public XmlDocument xmlDocument;
        private string fileName;
        private string archName;

        /// <summary>
        /// Открывает архив и проверяет его на соответствие схеме данных
        /// </summary>
        /// <param name="fileName"></param>
        public Archive(string fileName)
        {
            archName = System.IO.Path.GetFileNameWithoutExtension(fileName);
            xmlDocument = new System.Xml.XmlDocument();
            this.fileName = fileName;

            bool loaded = false;
            do
            {
                try
                {
                    xmlDocument.Load(fileName);
                    loaded = true;
                }
                catch (FileNotFoundException e)
                {
                    xmlDocument.LoadXml("<?xml version='1.0' encoding='utf-8' ?><Archiv><name>" + fileName +
                                        "</name><Examinees></Examinees></Archiv>");
                    xmlDocument.Save(fileName);
                }
            } while (!loaded);
        }

        public string ArchName
        {
            get { return archName; }
        }

        public void SaveArchive()
        {
            bool saved = false;
            do
            {
                try
                {
                    xmlDocument.Save(fileName);
                    saved = true;
                }
                catch (Exception)
                {
                }
                
            } while (!saved);
        }

        /// <summary>
        /// Свойство дающее список всех обследуемых архива c данными id, name, tests, description
        /// </summary>
        public List<Examinee> ArchExaminees
        {
            get
            {
                List<Examinee> exs = new List<Examinee>();

                foreach (XmlNode item in xmlDocument.DocumentElement.SelectNodes("/Archiv/Examinees/Examinee"))
                {
                    string testStr = "";
                    foreach (XmlNode itemTests in item.SelectNodes("Tests/Test"))
                    {
                        testStr += itemTests["Name"].InnerText.Trim();
                        testStr += itemTests["IsFinished"].InnerText == "true" ? " " : "(!) ";
                    }

                    exs.Add(new Examinee(Convert.ToInt32(item.ChildNodes[0].InnerText),
                        item.ChildNodes[1].InnerText,
                        item.ChildNodes[2].InnerText,
                        this, testStr));
                }

                return exs;
            }
        }

        public Examinee getExaminee(int id)
        {
            XmlNode findNode = xmlDocument.DocumentElement.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + id + "]");
            if (findNode != null)
            {
                List<ExamineeTest> examineeTests = new List<ExamineeTest>();

                foreach (XmlNode item in findNode.SelectNodes("Tests/Test"))
                {
                    DateTime dateOfTest;
                    if (!DateTime.TryParse(item["Date"].InnerText, out dateOfTest))
                        dateOfTest = DateTime.Now;
                    examineeTests.Add(new ExamineeTest(this, id, Convert.ToInt32(item["ID"].InnerText),
                        item["Name"].InnerText, item["FullTime"].InnerText,
                        dateOfTest, bool.Parse(item["IsFinished"].InnerText),
                        new List<TestResult>()));
                }

                return new Examinee(id,
                    findNode["Name"].InnerText,
                    findNode["Description"].InnerText,
                    this, examineeTests);
            }
            else
            {
                return null;
            }
        }
        public int getExamineeIdByName(string name)
        {
            XmlNode findNode = xmlDocument.DocumentElement.SelectSingleNode("/Archiv/Examinees/Examinee[Name=\'" + name + "\']");
            if (findNode != null)
            {
                string idText = findNode["ID"].InnerText;
                return int.Parse(idText);
            }
            else
            {
                return -1;
            }
        }

        public bool AddExaminee(string name, string description)
        {
            if (this.ArchExaminees.Count(ex => ex.Name.Trim().ToLower() == name.Trim().ToLower()) == 0)
            {
                int MaxID = 0;
                foreach (XmlNode item in xmlDocument.DocumentElement.SelectNodes("/Archiv/Examinees/Examinee"))
                {
                    if (Convert.ToInt32(item["ID"].InnerText) > MaxID)
                    {
                        MaxID = Convert.ToInt32(item["ID"].InnerText);
                    }
                }

                XmlNode findNode = xmlDocument.DocumentElement.SelectSingleNode("/Archiv/Examinees");
                findNode.AppendChild(xmlDocument.CreateElement("Examinee"));

                findNode.LastChild.AppendChild(xmlDocument.CreateElement("ID"));
                findNode.LastChild.LastChild.AppendChild(xmlDocument.CreateTextNode((MaxID + 1).ToString()));

                findNode.LastChild.AppendChild(xmlDocument.CreateElement("Name"));
                findNode.LastChild.LastChild.AppendChild(xmlDocument.CreateTextNode(name));

                findNode.LastChild.AppendChild(xmlDocument.CreateElement("Description"));
                findNode.LastChild.LastChild.AppendChild(xmlDocument.CreateTextNode(description));

                findNode.LastChild.AppendChild(xmlDocument.CreateElement("Tests"));

                SaveArchive();
                return true;
            }
            else
            {
                return false;
            }
        }

        public ExamineeTest GetExamineeTest(int id, int testID)
        {
            XmlNode findNode = xmlDocument.DocumentElement.SelectSingleNode("/Archiv/Examinees/Examinee[ID=" + id + "]/Tests/Test[ID=" + testID + "]");
            List<TestResult> testResults = new List<TestResult>();

            foreach (XmlNode item in findNode.SelectNodes("Results/Result"))
            {
                testResults.Add(new TestResult(testID,
                    Convert.ToInt32(item["QuestNumber"].InnerText),
                    item["QuestText"].InnerText,
                    Convert.ToInt32(item["AnsID"].InnerText),
                    item["AnsText"].InnerText,
                    item["Time"].InnerText,
                    item["AnsNumber"].InnerText));
            }

            return new ExamineeTest(this,
                id,
                testID,
                findNode["Name"].InnerText,
                findNode["FullTime"].InnerText,
                Convert.ToDateTime(findNode["Date"].InnerText),
                findNode["IsFinished"].InnerText == "true" ? true : false,
                testResults);
        }
    }
}