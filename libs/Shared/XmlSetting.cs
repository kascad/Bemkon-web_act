using System;
using System.IO;

namespace Shared
{
    public class XmlSetting
    {
        private readonly string _settingFile;
        private readonly System.Xml.XmlDocument _xmlDoc;

        public XmlSetting(string settingFile)
        {
            _settingFile = settingFile;

            // Init xmlDoc
            _xmlDoc = new System.Xml.XmlDocument();
            try
            {
                _xmlDoc.Load(settingFile);
            }
            catch (Exception)
            {
                _xmlDoc.LoadXml(@"<?xml version='1.0' encoding='utf-8'?><settings>" +
                   "<OpenLastArchive>True</OpenLastArchive><LastArchive></LastArchive>" +
                   "<ArchivesFolder>Archives</ArchivesFolder>" +
                   "<constr></constr>" +
                   "</settings>");
                SaveSetting();
            }

        }

        public string GetSetting(string settingName)
        {
            try
            {
                return _xmlDoc.DocumentElement.GetElementsByTagName(settingName)[0].InnerText;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public void SetSetting(string settingName, string settingValue)
        {
            try
            {
                _xmlDoc.DocumentElement.GetElementsByTagName(settingName)[0].InnerText = settingValue;
            }
            catch (Exception)
            {
            }
        }

        public void SaveSetting()
        {
            _xmlDoc.Save(_settingFile);
        }

        public static XmlSetting GetProgramSetting()
        {
            var programmDataFolder = GlobalOptions.ProgrammDataFolder;
            return new XmlSetting(Path.Combine(programmDataFolder, "setting.xml"));
        }

        public string GetEncryptSetting(string settingName)
        {
            string encryptSetting = GetSetting(settingName);
            if (encryptSetting != "")
            {
                return EncryptExtentions.DecryptDES(encryptSetting);
            }
            return "";
        }

        public void SetEncryptSetting(string settingName, string settingValue)
        {
            SetSetting(settingName, settingValue != "" ? EncryptExtentions.EncryptDES(settingValue) : "");
        }
    }
}
