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
using System.Collections.Generic;

/// <summary>
/// Представляет менеджер пользователей.
/// </summary>
public class UserManager
    : IDisposable
{
    private ErrorHandler error = new ErrorHandler();
    public ErrorHandler Error
    {
        get { return error; }
    }
    ConnectionDBPool conn;

	public UserManager()
	{
        conn = new ConnectionDBPool();
	}

    public void LoginUser(int id)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ID", typeof(int), id));
        par.Add(new SqlParam("@Time", typeof(int), Core.DateTimeEx.ConvertToIntCurrentDateTime()));

        conn.ExecuteNoResult("LoginUser", par);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
    }
    public void LogoffUser(int id)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ID", typeof(int), id));

        conn.ExecuteNoResult("LogoffUser", par);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
    }

    public bool FindUser(string login, string passw)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@Name", typeof(string), login));
        par.Add(new SqlParam("@Password", typeof(string), passw));

        object res = conn.ExecuteScalar("FindUser", par);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
        return Core.Converting.ConvertToInt(res) != 0;
    }
    public DataTable GetUserInfo(string login)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@Name", typeof(string), login));

        DataTable res = conn.ExecuteReader("GetUserInfo", par);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
        return res;
    }
    public DataTable GetUserInfo(int id)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ID", typeof(int), id));

        DataTable res = conn.ExecuteReader("GetUserInfoById", par);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
        return res;
    }
    public DataTable GetUserList()
    {
        error.ResetError();

        DataTable res = conn.ExecuteReader("GetUserList", null);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
        return res;
    }
    public void BanUser(int id)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ID", typeof(int), id));

        conn.ExecuteNoResult("BanUser", par);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
    }
    public void ChangePassword(int id, string newPassword)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ID", typeof(int), id));
        par.Add(new SqlParam("@Password", typeof(string), newPassword));

        conn.ExecuteNoResult("ChangePassword", par);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
    }

    public bool CheckLogin(string login, int id)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ID", typeof(int), id));
        par.Add(new SqlParam("@UserName", typeof(string), login));

        object res = conn.ExecuteScalar("CheckLogin", par);
        if (conn.Error.IsError)
        {
            error.SetError(conn.Error.Message);
            return false;
        }
        int nres = Core.Converting.ConvertToInt(res);
        return (nres <= 0);
    }

    public bool CheckEmail(string email, int id)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ID", typeof(int), id));
        par.Add(new SqlParam("@Email", typeof(string), email));

        object res = conn.ExecuteScalar("CheckEmail", par);
        if (conn.Error.IsError)
        {
            error.SetError(conn.Error.Message);
            return false;
        }
        int nres = Core.Converting.ConvertToInt(res);
        return (nres <= 0);
    }

    #region IDisposable Members

    public void Dispose()
    {
        conn.Disconnect();
    }

    #endregion

    public void EditLastIdExaminee(int exmId, int idUser)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ID", typeof(int), idUser));
        par.Add(new SqlParam("@LastIdExaminee", typeof(int), exmId));

        conn.ExecuteNoResult("EditLastIdExaminee", par);
        if (conn.Error.IsError)
        {
            error.SetError(conn.Error.Message);
        }
    }

    internal void EditLastArchive(int idUser, string nameArchive)
    {
        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@LastArchive", typeof(string), nameArchive));
        par.Add(new SqlParam("@ID", typeof(int), idUser));

        conn.ExecuteNoResult("EditLastArchive", par);
        if (conn.Error.IsError)
        {
            error.SetError(conn.Error.Message);
        }
    }

    public string CreateTestingUser(int parentId, string archive, int idExamenee, int startBattery, string newUserName, string newPassword, int expiration)
    {
        string link = "";

        error.ResetError();

        List<SqlParam> par = new List<SqlParam>();
        par.Add(new SqlParam("@ParentID", typeof(int), parentId));
        par.Add(new SqlParam("@LastArchive", typeof(string), archive));
        par.Add(new SqlParam("@LastIdExaminee", typeof(int), idExamenee));
        par.Add(new SqlParam("@LastBattery", typeof(int), startBattery));
        par.Add(new SqlParam("@UserName", typeof(string), newUserName));
        par.Add(new SqlParam("@Password", typeof(string), newPassword));
        par.Add(new SqlParam("@Expiration", typeof(int), expiration));  

        conn.ExecuteNoResult("CreateTestUser", par);
        if (conn.Error.IsError)
        {
            error.SetError(conn.Error.Message);
        }
        else
        {
            link = "http://www.l1k.ru/Login.aspx?TestUser=";
            link += newUserName;
            if (!String.IsNullOrEmpty(newPassword))
            {
                link += "&password=";
                link += newPassword;
            }
        }

        return link;
    }

    // Работа со списком компании

    public DataTable GetCompanyList(int id, string companyname)
    {
        error.ResetError();

        DataTable res = conn.ExecuteReader("GetCompanyList", null);
        if (conn.Error.IsError)
            error.SetError(conn.Error.Message);
        return res;
    }

}
