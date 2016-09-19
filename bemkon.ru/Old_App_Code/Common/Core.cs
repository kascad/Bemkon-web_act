using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.Mail;
using System.Collections.Generic;
using System.IO;
using ProfessorTesting;

/// <summary>
/// Общие функции, классы и тыды.
/// </summary>
public static class Core
{

    public static class FileEx
    {
        public static string extArchive = "arh";
        public static string separator = @"\";

        public static void DownloadFile(HttpResponse Response, string filepath)
        {
            // Identify the file name.
            string filename = System.IO.Path.GetFileName(filepath);

            Response.Clear();

            // Specify the Type of the downloadable file.
            Response.ContentType = "application/octet-stream";

            // Set the Default file name in the FileDownload dialog box.
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

            Response.Flush();

            // Download the file.
            Response.TransmitFile(filepath);
            Response.End();
        }

        public static string GetPathArchivesCurUser()
        {
            return GetPathArchivesUser(Core.Site.CurUserId);
        }
        public static string GetPathFSArchivesCurUser()
        {
            return HttpContext.Current.Server.MapPath(GetPathArchivesUser(Core.Site.CurUserId));
        }

        public static string GetPathArchivesUser(int id)
        {
            if (id != 0)
                return Core.Site.pathDirArchives + id;
            return Core.Site.pathDirArchives;
        }
        public static string GetPathFSArchivesUser(int id)
        {
            if (id != 0)
                return HttpContext.Current.Server.MapPath(Core.Site.pathDirArchives + id);
            return "";
        }

        public static void CreateDirArchivesCurUser()
        {
            CreateDirArchives(Core.Site.CurUserId);
        }
        public static void CreateDirArchives(int id)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath(GetPathArchivesUser(id));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch { }
        }

        public static string GetFullPathFSArchivesCurUser()
        {
            if (Core.Site.CurrUserInfo == null || Core.Site.CurrUserInfo.Id == 0 || Core.Site.CurrUserInfo.LastArchive == "")
                return "";

            string path = Core.FileEx.GetPathFSArchivesCurUser();
            return Path.Combine(path, Core.Site.CurrUserInfo.LastArchive + "." + Core.FileEx.extArchive);
        }
        public static string GetPathFSData(string file)
        {
            return HttpContext.Current.Server.MapPath(Core.Site.pathDirData + file);
        }


        internal static string GetPathDataImage()
        {
            return Core.Site.pathDirData + "images/";
        }
    }
    public static class DateTimeEx
    {
        private static DateTime beg = DateTime.Parse("1970-01-01 00:00:00");
        public static int ConvertToInt(DateTime dt)
        {
            TimeSpan span = dt.Subtract(beg);
            return (int)(span.TotalSeconds);
        }
        public static long ConvertToLong(DateTime dt)
        {
            TimeSpan span = dt.Subtract(beg);
            return (long)(span.TotalSeconds);
        }
        public static int ConvertToIntCurrentDateTime()
        {
            return ConvertToInt(DateTime.Now);
        }

    }
    public static class Site
    {
        public static string connectionString = "";
        public static string titleProgram = "Профессиональное тестирование";
        public static string pathDirArchives = "~/Archives/";
        public static string pathDirData = "~/bin/Data/";

        private static Dictionary<string, int> _userIds;
        private static Dictionary<int, UserInfo> _users;

        public static int CurUserId = 0;
        public static UserInfo CurrUserInfo
        {
            get
            {
                if (CurUserId == 0 || _users == null)
                    return null;
                if (!_users.ContainsKey(CurUserId))
                    return null;

                UserInfo userInfo;
                _users.TryGetValue(CurUserId, out userInfo);
                return userInfo;
            }
        }

        static Site()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            if (_userIds == null)
                _userIds = new Dictionary<string, int>();
            if (_users == null)
                _users = new Dictionary<int, UserInfo>();
        }

        public static void ResetUserCache()
        {
            _userIds.Clear();
            _users.Clear();
        }

        /// <summary>
        /// Представляет email, с которого отправляются письма с кодом активации аккаунта
        /// </summary>
        public static class Email
        {
            public static string emailFrom = "bemkoninfo@gmail.com";
            public static string login = "bemkoninfo@gmail.com";
            public static string password = "Bemkon12";
            public static string host = "smtp.gmail.com";
            public static string smtpServer = "smtp.gmail.com";
        }

        public static void RedirectError(HttpResponse Response, string errMsg)
        {
            Response.Write("<div style=\"width:100%;background:#FCBAC3;\">" + errMsg + "</div>");
        }
        public static void RedirectError(HttpResponse Response, string errMsg, HtmlContainerControl container)
        {
            if (container != null)
                container.InnerHtml = errMsg;
            Response.Write("<div style=\"width:100%;background:#FCBAC3;\">" + errMsg + "</div>");
        }

        public static UserPrivGroup Authenticate(string login, string pwd)
        {
            CurUserId = 0;

            // Получение прав доступа пользователя
            UserPrivGroup priv = UserPrivGroup.None;
            UserInfo user = null;

            // ищем в бд
            using (UserManager userManager = new UserManager())
            {
                if (!userManager.FindUser(login, pwd))
                    return UserPrivGroup.None;

                DataTable usi = userManager.GetUserInfo(login);
                if (userManager.Error.IsError || usi == null || usi.Rows.Count <= 0)
                    return UserPrivGroup.None;
                user = new UserInfo(usi.Rows[0]);
                userManager.LoginUser(user.Id);
            }

            //if (user != null)
            {
                CurUserId = user.Id;
                priv = user.Priv;

                if (!_userIds.ContainsKey(login))
                    _userIds.Add(login, CurUserId);
                if (!_users.ContainsKey(CurUserId))
                    _users.Add(CurUserId, user);
            }

            return priv;
        }
        public static UserPrivGroup IsAuth(Page page)
        {
            CurUserId = 0;
            if (page == null || page.User == null || page.User.Identity == null || page.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return UserPrivGroup.None;
            }

            // Получение прав доступа пользователя
            UserPrivGroup priv = UserPrivGroup.None;
            UserInfo user = null;

            // ищем в кэше
            if (_userIds.ContainsKey(page.User.Identity.Name))
            {
                int userId;
                _userIds.TryGetValue(page.User.Identity.Name, out userId);
                if (_users.ContainsKey(userId))
                    _users.TryGetValue(userId, out user);
            }

            // ищем в бд
            if (user == null)
            {
                using (UserManager userManager = new UserManager())
                {
                    DataTable usi = userManager.GetUserInfo(page.User.Identity.Name);
                    if (userManager.Error.IsError || usi == null || usi.Rows.Count <= 0)
                        return UserPrivGroup.None;

                    user = new UserInfo(usi.Rows[0]);
                }
            }

            CurUserId = user.Id;
            priv = user.Priv;

            if (!_userIds.ContainsKey(page.User.Identity.Name))
                _userIds.Add(page.User.Identity.Name, CurUserId);
            if (!_users.ContainsKey(CurUserId))
                _users.Add(CurUserId, user);

            return priv;
        }

        public static bool SendMail(string toAddress, string fromAdress, string body, string subject, string attach, out string errorMessage)
        {
            bool res = true;
            using (MailMessage message = new MailMessage(Email.emailFrom, toAddress))
            {
                message.Subject = subject;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Body = body;
                if (attach != "")
                {
                    message.Attachments.Add(new System.Net.Mail.Attachment(attach));
                }

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(Email.smtpServer, 25);
                client.DeliveryMethod = SmtpDeliveryMethod.Network; // определяет метод отправки сообщений
                client.EnableSsl = true; // отключает необходимость использования защищенного соединения с сервером
                client.UseDefaultCredentials = false; // отключение использования реквизитов авторизации "по-умолчанию"
                client.Credentials = new System.Net.NetworkCredential(Email.login, Email.password);
                    // указание нужных реквизитов (имени пользователя и пароля) для авторизации на SMTP-сервере

                client.Send(message);
                errorMessage = "";
            }
            //catch (Exception ex)
            //{
            //    res = false;
            //    errorMessage = ex.Message;
            //}
            //message.Dispose();
            return res;
        }

        public static string GetPageFrom(FromType from)
        {
            if (from == FromType.None || from == FromType.Examinee)
                return "";

            string ext = ".aspx";
            switch (from)
            {
                case FromType.Battery:
                    return "BatteryTests" + ext;
                case FromType.Test:
                    return "Tests" + ext;
                case FromType.TopTest:
                    return "TopTests" + ext;
            }
            return "";
        }

        internal static void NewArchive(string nameArchive)
        {
            if (Core.Site.CurrUserInfo.Priv == UserPrivGroup.TestUser)
                return;

            CurrUserInfo.LastIdExaminee = 0;
            CurrUserInfo.LastArchive = nameArchive;
            using (UserManager userManager = new UserManager())
            {
                userManager.EditLastArchive(CurrUserInfo.Id, nameArchive);
            }
        }

    }
    public static class Consts
    {
        public static string indexPage = "Default.aspx";

        public const string modAdminUsers = "AdminUsers";
        public const string modAdminCustomize = "AdminCustomize";

        public static string reqMod = "mod";
        public static string reqId = "id";
        public static string reqIdTest = "id_test";
        public static string reqIdBattery = "id_battery";
        public static string reqIdQuestion = "id_question";
        public static string reqIdAnswer = "id_answer";
        public static string reqBan = "ban";
        public static string reqAId = "aid";
        public static string reqFrom = "from";
        public static string reqAction = "action";
    }
    public static class Converting
    {
        public static UserPrivGroup ConvertToUserPrivGroup(object val)
        {
            if (val == null)
                return UserPrivGroup.None;
            if (Enum.IsDefined(typeof(UserPrivGroup), val))
                return (UserPrivGroup)val;
            return UserPrivGroup.None;
        }
        public static FromType ConvertToFromType(object val)
        {
            if (val == null)
                return FromType.None;
            int ival = Core.Converting.ConvertToInt(val);
            if (Enum.IsDefined(typeof(FromType), ival))
                return (FromType)ival;
            return FromType.None;
        }
        public static ActionType ConvertToActionType(object val)
        {
            if (val == null)
                return ActionType.None;
            int ival = Core.Converting.ConvertToInt(val);
            if (Enum.IsDefined(typeof(ActionType), ival))
                return (ActionType)ival;
            return ActionType.None;
        }
        public static int ConvertToInt(object val)
        {
            try { return Convert.ToInt32(val); }
            catch { return 0; }
        }
        public static long ConvertToLong(object val)
        {
            try { return Convert.ToInt64(val); }
            catch { return 0; }
        }

        public static DateTime ToTime(string val)
        {
            DateTime dt = new DateTime();
            try { dt = Convert.ToDateTime(val); }
            catch { }
            return dt;
        }

        public static string FormatTime(DateTime dt)
        {
            return GetPartTime(dt.Hour) + ":" + GetPartTime(dt.Minute) + ":" + GetPartTime(dt.Second);
        }
        private static string GetPartTime(int part)
        {
            if (part < 10)
                return "0" + part;
            return part.ToString();
        }
    }

    public static Control FindControlRecursive(Control rootControl, string controlID)
    {
        if (rootControl.ID == controlID) return rootControl;

        foreach (Control controlToSearch in rootControl.Controls)
        {
            Control controlToReturn =
                FindControlRecursive(controlToSearch, controlID);
            if (controlToReturn != null) return controlToReturn;
        }
        return null;
    }

}
