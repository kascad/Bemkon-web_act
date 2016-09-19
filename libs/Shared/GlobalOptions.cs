using System;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace Shared
{
    public enum DbMode
    {
        None,
        Local,
        Remote,
    }

    public static class GlobalOptions
    {
        private const string LocalDb = "professor_testing.sdf";

        private const string DefaultSiteGraphPath = "ScaleGraphs"; 

        static GlobalOptions()
        {
            InitProperties();
        }

        private static bool IsWebApp
        {
            get { return HttpContext.Current != null; }
        }

        private static void InitProperties()
        {
            try
            {
                string settingFilePath;
                string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                ProgrammDataFolder = Path.Combine(appFolder, "Bemkon\\ProfessorTesting");

                if (!IsWebApp)
                {
                    if (!Directory.Exists(ProgrammDataFolder))
                        Directory.CreateDirectory(ProgrammDataFolder);
                    settingFilePath = Path.Combine(ProgrammDataFolder, "setting.xml");
                }
                else
                    settingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setting.xml");
                var setting = new XmlSetting(settingFilePath);

                var mode = setting.GetSetting("Mode");
                DababaseMode = (DbMode) Enum.Parse(typeof (DbMode), mode);

                RemoteConnectionString = setting.GetSetting("constr");

                LocalBdPath = Path.Combine(ProgrammDataFolder, LocalDb);
                LocalConnectionString = string.Format("Data Source={0};Max Database Size=2047", LocalBdPath);
                InterpretsFolder = Path.Combine(ProgrammDataFolder, "Interprets");

                InterpretsGrathsFolder = DababaseMode == DbMode.Local
                    ? Path.Combine(InterpretsFolder, DefaultSiteGraphPath)
                    : DefaultSiteGraphPath;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning(ex.ToString());
            }
        }

        public static string RemoteConnectionString { get; private set; }
        public static string ProgrammDataFolder { get; private set; }
        public static DbMode DababaseMode { get; private set; }
        public static string LocalConnectionString { get; private set; }
        public static string LocalBdPath { get; private set; }
        public static string InterpretsFolder { get; private set; }
        public static string InterpretsGrathsFolder { get; private set; }

        public static string ArchiveFolder
        {
            get
            {
                var setting = Shared.XmlSetting.GetProgramSetting();
                var archiveFolder = setting.GetSetting("ArchivesFolder");
                if (string.IsNullOrEmpty(archiveFolder))
                {
                    archiveFolder = Path.Combine(ProgrammDataFolder, "Archives");
                }
                return archiveFolder;
            }
        }
    }
}
