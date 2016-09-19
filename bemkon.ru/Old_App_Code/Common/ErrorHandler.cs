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
/// Представляет контейнер ошибки выполнения
/// </summary>
public class ErrorHandler
{
    private Dictionary<ErorrCode, string> listError;
    public Dictionary<ErorrCode, string> ListError
    {
        get { return listError; }
    }
    public static string unknownError;
    private string message = "";
    public string Message
    {
        get { return message; }
    }

    public bool IsError
    {
        get { return message != ""; }
    }
	public ErrorHandler()
	{
        ResetError();

        InitListError();
        unknownError = listError[ErorrCode.UnknownError];
	}

    public enum ErorrCode
    {
        None,
        UnknownError = 1,
    }
    private void InitListError()
    {
        listError = new Dictionary<ErorrCode, string>();
        listError.Add(ErorrCode.UnknownError, "Неизвестная ошибка");
    }

    public void ResetError()
    {
        message = "";
    }

    internal void SetError(string errMsg)
    {
        message = errMsg;
    }
}
