using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ProfessorTesting;

/// <summary>
/// Представляет информацию о текущем пользователе.
/// </summary>
public class UserInfo
{
    private DataRow info;

    private UserPrivGroup _priv;
    public UserPrivGroup Priv
    {
        get { return _priv; }
    }
    private int _id;
    public int Id
    {
        get { return _id; }
    }
    private string _lastArchive;
    public string LastArchive
    {
        get { return _lastArchive; }
        set { _lastArchive = value; }
    }
    private int _lastIdExaminee;
    public int LastIdExaminee
    {
        get { return _lastIdExaminee; }
        set { _lastIdExaminee = value; }
    }
    private string _login;
    public string Login
    {
        get { return _login; }
        set { _login = value; }
    }

    public ArchiveListSource ArchiveDataSource;

    private Archive.Archive _archive;
    public Archive.Archive Archive
    {
        get { return _archive; }
        set
        {
            _archive = value;
            ArchiveDataSource = new ArchiveListSource();
        }
    }
    void InitArchive(string fileName)
    {
        Archive = new Archive.Archive(fileName);
    }
    private Archive.Examinee _currExaminee;

    public Archive.Examinee CurrExaminee
    {
        get { return _currExaminee;  }
        set { _currExaminee = value;  }
    }
    private int _parentid;
    public int ParentId
    {
        get { return _parentid; }
    }

    private int _companyid;
    public int CompanyID
    {
        get { return _companyid; }
    }
    private string _companyname;
    public string CompanyName
    {
        get { return _companyname; }
        set { _companyname = value; }
    }


    public UserInfo(DataRow info)
	{
        this.info = info;
        this._priv = Core.Converting.ConvertToUserPrivGroup(info[NameDB.Users.Priv]);
        this._id = Core.Converting.ConvertToInt(info[NameDB.Users.ID]);
        this._lastArchive = info[NameDB.Users.LastArchive].ToString();
        this._lastIdExaminee = Core.Converting.ConvertToInt(info[NameDB.Users.LastIdExaminee]);
        this._login = info[NameDB.Users.UserName].ToString();
        this._parentid = Core.Converting.ConvertToInt(info[NameDB.Users.ParentID]);
        this._companyid = Core.Converting.ConvertToInt(info[NameDB.Users.CompanyID]);
        this._companyname = info[NameDB.Users.CompanyName].ToString();

        if (_priv == UserPrivGroup.TestUser)
        {
            if (this._parentid == 0)
                this._parentid = 29; // TEMP compatibility

            _lastArchive = Path.Combine(Core.FileEx.GetPathFSArchivesUser(this._parentid), this._lastArchive); // _login + ".arh"
            _archive = new Archive.Archive(_lastArchive + "." + Core.FileEx.extArchive);
            if (_lastIdExaminee == 0)
                _lastIdExaminee = _archive.getExamineeIdByName(_login);
            _currExaminee = _archive.getExaminee(_lastIdExaminee);
        }

	}
}
