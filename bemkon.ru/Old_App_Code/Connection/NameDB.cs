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

/// <summary>
/// Названия полей в базе данных.
/// </summary>
public static class NameDB
{
    public static class Users
    {
        public static string ID = "ID";
        public static string Priv = "Priv";
        public static string Name = "nName";
        public static string UserName = "UserName";
        public static string Password = "Password";
        public static string Email = "Email";
        public static string Active = "Active";
        public static string AID = "AID";
        public static string Expiration = "Expiration";
        public static string LastArchive = "LastArchive";
        public static string LastIdExaminee = "LastIdExaminee";
        public static string ParentID = "ParentID";
        public static string CompanyID = "CompanyID";
        public static string CompanyName = "CompanyName";
    }
}

