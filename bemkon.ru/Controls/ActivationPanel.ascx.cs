using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

public partial class Controls_ActivationPanel : System.Web.UI.UserControl
{
    private int _userID = 0;
    public int UserID
    {
        get { return _userID; }
        set { _userID = value; }
    }
    private string _aid = "";
    public string AID
    {
        get { return _aid; }
        set { _aid = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
        _userID = Core.Converting.ConvertToInt(sid);
        _aid = HttpContext.Current.Request.QueryString[Core.Consts.reqAId];

        Activate();
    }
    private void Activate()
    {
        string errMsg = "";
        bool active = true;
        if (_userID != 0 && _aid != "")
        {
            SqlDataSource1.SelectCommand = "SELECT * FROM Users WHERE ID=" + _userID + " AND AID='" + _aid + "' AND Active=0";
            try
            {
                DataView row = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (row != null && row.Count > 0)
                {
                    int exp = Core.Converting.ConvertToInt(row[0][NameDB.Users.Expiration]);
                    // Аккаунт просрочен
                    if (exp == 0 || exp > Core.DateTimeEx.ConvertToIntCurrentDateTime())
                    {
                        try
                        {
                            SqlDataSource1.DeleteCommand = "DELETE FROM Users WHERE ID=" + _userID;
                            SqlDataSource1.Delete();

                            inputActivation.Visible = false;
                            LabelSuccess.Text = "Время активации аккаунта просрочено. Зарегистрируйтесь еще раз.";
                            LabelSuccess.Visible = true;
                            active = true;
                        }
                        catch (Exception err)
                        {
                            errMsg = err.Message;
                            active = false;
                        }
                    }
                    else
                    {
                        try
                        {
                            SqlDataSource1.UpdateCommand = "UPDATE Users SET Active=1, AID='', Expiration=0 WHERE ID=" + _userID;
                            SqlDataSource1.Update();
                            inputActivation.Visible = false;
                            LabelSuccess.Text = "Ваш аккаунт успешно подтвержден.";
                            LabelSuccess.Visible = true;
                            active = true;
                            // Создание папки архивов
                            Core.FileEx.CreateDirArchives(_userID);
                        }
                        catch (Exception err)
                        {
                            errMsg = err.Message;
                            active = false;
                        }
                    }
                }
                else
                {
                    inputActivation.Visible = false;
                    LabelSuccess.Text = "Пользователь или ключ активации не найден. Попробуйте активировать аккаунт вручную.";
                    LabelSuccess.Visible = true;
                    active = true;
                }
            }
            catch (Exception err)
            {
                errMsg = err.Message;
                active = false;
            }
            TextBoxID.Text = _userID.ToString();
            TextBoxKey.Text = _aid;
        }
        if (!active)
        {
            if (errMsg != "")
            {
                LabelError.Text = "Возникла следующая ошибка при подтверждении аккаунта: " + errMsg;
                LabelError.Visible = true;
            }
        }
    }
    protected void LinkButtonActivation_Click(object sender, EventArgs e)
    {
        _userID = Core.Converting.ConvertToInt(TextBoxID.Text);
        _aid = TextBoxKey.Text;

        Activate();
    }
}
