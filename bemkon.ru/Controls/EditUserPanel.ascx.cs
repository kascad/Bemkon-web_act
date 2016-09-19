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
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Collections.Generic;



public partial class Controls_EditUserPanel : System.Web.UI.UserControl
{
    private EditMode editMode = EditMode.Edit;
    public EditMode EditMode
    {
        get { return editMode; }
        set { editMode = value; }
    }
    private int id = 0;
    private string prevLogin = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string sid = HttpContext.Current.Request.QueryString[Core.Consts.reqId];
        id = Core.Converting.ConvertToInt(sid);
        SqlDataSource1.SelectCommand = "SELECT * FROM Users WHERE ID = " + id;
        SqlDataSource1.UpdateCommand = "UPDATE [Users] SET [UserName] = @UserName, [nName] = @nName, Priv = @Priv, [Email] = @Email, [LastBattery] = @LastBattery, [CompanyID] = @CompanyID, [CompanyName] = @CompanyName WHERE [ID] = " + id;
  
        if (id == -1)
            editMode = EditMode.Add;

        if (editMode == EditMode.Add)
        {
            LabelTitle.Text = "Регистрация пользователя";
            SqlDataSource1.InsertCommand = "INSERT INTO [Users] (UserName, Password, Priv, nName, Email, Active, LastIdExaminee, LastBattery, CompanyID, CompanyName) "
                + "VALUES (@UserName, @Password, @Priv, @nName, @Email, 1, 1, @LastBattery, @CompanyID, @CompanyName); "
                + "SELECT @ID = SCOPE_IDENTITY()";
            Repeater1.DefaultMode = FormViewMode.Insert;
        }
        if (editMode == EditMode.Customize)
        {
            using (UserManager userManager = new UserManager())
            {
                // Получение информации о текущем пользователе
                DataTable userInfo = userManager.GetUserInfo(Page.User.Identity.Name);
                if (userManager.Error.IsError)
                {
                    Response.Write(userManager.Error.Message);
                    return;
                }

                // Информации о пользователе в базе нет
                if (userInfo == null || userInfo.Rows.Count <= 0)
                {
                    FormsAuthentication.RedirectToLoginPage();
                    return;
                }


                
                UserInfo curUserInfo = new UserInfo(userInfo.Rows[0]);

                SqlDataSource3.SelectCommand = "SELECT * FROM Company WHERE ID = " + id;


                List<string> CompanyData = new List<string>();
                List<double> testScaleIndex2 = new List<double>();

                var CompData = SqlDataSource3.SelectCommand = "SELECT * FROM Company";

               // CompanyData = CompData;

                foreach (var comdata in CompData)
                    {
                        System.Diagnostics.Debug.WriteLine("##################");
                        System.Diagnostics.Debug.WriteLine("comdata:" + comdata);
                    }

                prevLogin = curUserInfo.Login;

                id = curUserInfo.Id;
                SqlDataSource1.SelectCommand = "SELECT * FROM Users WHERE ID = " + id;
                SqlDataSource1.UpdateCommand = "UPDATE [Users] SET [UserName] = @UserName, [nName] = @nName, Priv = @Priv, [Email] = @Email, [LastBattery] = @LastBattery, [CompanyID] = @CompanyID, [CompanyName] = @CompanyName WHERE [ID] = " + id;

                HyperLink ctrl = (HyperLink)Core.FindControlRecursive(Repeater1, "hlChangePassword");
                if (ctrl != null)
                    ctrl.NavigateUrl += "&url=Customize";
            }
        }
    }
    
    private void Success()
    {
        DropDownList type = Core.FindControlRecursive(Repeater1, "DropDownListUserType") as DropDownList;
        TextBox login = Core.FindControlRecursive(Repeater1, "UserNameTextBox") as TextBox;

        divRegistration.Visible = false;
        successRegistration.Visible = true;

        successRegistration.InnerText = "Пользователь успешно зарегистрирован! ";
        if (type.SelectedItem.Value == "4")
            successRegistration.InnerText += "Ссылка для использования: http://l1k.ru/Login.aspx?TestUser=" + login.Text;       
    }
    protected void FormView_ItemUpdated(Object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            Core.Site.RedirectError(Response, e.Exception.Message);
            e.ExceptionHandled = true;
            return;
        }

        if (editMode == EditMode.Customize)
        {
            string newLogin = e.NewValues[NameDB.Users.UserName].ToString();
            // Изменился логин текущего пользователя
            if (string.Compare(newLogin, prevLogin, true) != 0)
            {
                FormsAuthentication.SetAuthCookie(newLogin, true);
            }
            Core.Site.ResetUserCache();
        }

        if (editMode == EditMode.Add)
            Success();
        else
            Response.Redirect("~/Default.aspx");
    }
    protected void FormView_ItemUpdating(Object sender, FormViewUpdateEventArgs e)
    {
        if (!CheckBeforeSave())
        {
            e.Cancel = true;
            return;
        }
    }

    protected void FormView_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            Response.Redirect("~/Default.aspx");
            return;
        }
    }

    protected void FormView_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            Core.Site.RedirectError(Response, e.Exception.Message);
            e.ExceptionHandled = true;
            return;
        }

        if (editMode == EditMode.Add)
            Success();
        else
            Response.Redirect("~/Default.aspx");
    }
    protected void FormView_ItemInserting(Object sender, FormViewInsertEventArgs e)
    {
        if (!CheckBeforeSave())
        {
            e.Cancel = true;
            return;
        }
    }

    protected void SqlDataSource_OnInserted(Object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception == null && e.AffectedRows != 0)
        {
            id = Core.Converting.ConvertToInt(e.Command.Parameters["@ID"].Value);
            string email = e.Command.Parameters["@Email"].Value.ToString();
            string name = e.Command.Parameters["@nName"].Value.ToString();
            string type = e.Command.Parameters["@Priv"].Value.ToString();
            string pwd = e.Command.Parameters["@Password"].Value.ToString();
            id = Core.Converting.ConvertToInt(e.Command.Parameters["@CompanyID"].Value);
            if (pwd.Length == 0 || type == "4")
            {
                SqlDataSource1.UpdateCommand = "UPDATE [Users] SET Password = '' WHERE ID=" + id;
                SqlDataSource1.Update();
            }


            string host = Server.HtmlEncode("http://" + Request.Url.Authority + Request.ApplicationPath);

            //string errMsg = "";
            MD5 md5 = new MD5CryptoServiceProvider();
            string uniqCode = Convert.ToBase64String((md5.ComputeHash(Encoding.ASCII.GetBytes(email))));

            // Записать в базу
            //SqlDataSource1.UpdateCommand = "UPDATE [Users] SET AID='" + uniqCode + "', Expiration = " + Core.DateTimeEx.ConvertToIntCurrentDateTime() + " WHERE ID=" + id;
            //SqlDataSource1.Update();

            string subj = "Регистрация на сайте " + host;
            string body = "Это письмо отправлено из <a href=" + host + ">" + host + "</a>"
                + "<p>Вы получили это письмо, так как этот e-mail был использован для регистрации на нашем сайте. <br />"
                + "Если Вы не регистрировались, то просто проигнорируйте это письмо и удалите его</p>"
                + "<h3>Инструкции по активации</h3>"
                + "<p>Вы зарегистрировали пользователя " + name + "<br />"
                + "Для активации Вашего аккаунта, зайдите по следующей ссылке:</p>"
                + "<p><a href=" + host + "Activation.aspx?id=" + id + "&aid=" + uniqCode + ">" + host + "Activation.aspx?id=" + id + "&aid=" + uniqCode + "</a></p>"
                + "<p>Срок действия этой ссылки один день."
                + "Это требуется для защиты от нежелательных злоупотреблений и спама, для проверки того, что введённый e-mail адрес - реальный. </p>"
                + "<h3>Не сработало?</h3>"
                + "<p>Если у Вам ничего не получилось и Вы не смогли подтвердить свою регистрацию, зайдите по следующей ссылке:<br /></p>"
                + "<p><a href=" + host + "Activation.aspx>" + host + "Activation.aspx" + "</a></p>"
                + "<p>Далее следует указать ID пользователя и ключа подтверждения, указанные чуть ниже<br />"
                + "ID пользователя: " + id + "<br />"
                + "Ключ подтверждения: " + uniqCode + "</p>"
                + "<p>Произведите действия Копировать/Вставить или введите эти данные вручную, в соответствующие поля.<br />"
                + "Если и при этих действиях ничего не получилось, возможно истёк срок действия неактивированного аккаунта и "
                + "он удалён. В этом случае, попробуйте зарегистрироваться заново.</p>"
                + "<p>Благодарим Вас за регистрацию!</p>"
                + "<p>С уважением,<br />"
                + "Администрация сайта <a href=" + host + ">" + host + "</a></p>";
            // Рассылка письма с кодом (отключена)
            //try
            //{
            //    Core.Site.SendMail(email, Core.Site.Email.emailFrom, body, subj, "", out errMsg);
            //}
            //catch (Exception err)
            //{
            //    Core.Site.RedirectError(Response, "Письмо с кодом активации не отправлено ("+ err.Message + ")");
            //    throw new MailException();
            //}
        }
    }

    private bool CheckBeforeSave()
    {
        //try
        //{
        using (UserManager userManager = new UserManager())
        {
            TextBox login = Core.FindControlRecursive(Repeater1, "UserNameTextBox") as TextBox;
            if (login != null)
            {
                bool check = userManager.CheckLogin(login.Text, id);
                if (userManager.Error.IsError)
                {
                    Core.Site.RedirectError(Response, userManager.Error.Message);
                    return false;
                }
                if (!check)
                {
                    HtmlContainerControl ctr = Core.FindControlRecursive(Repeater1, "ValidPass") as HtmlContainerControl;
                    if (ctr != null)
                    {
                        ctr.Visible = true;
                        ctr.InnerHtml = (editMode == EditMode.Add ?
                            "Пользователь не зарегистрирован" : "Изменения не сохранены") +
                            "<ul><li>Такой логин уже есть в базе</li></ul>";
                    }
                    WebControl ctrs = Core.FindControlRecursive(Repeater1, "ValidationSummary1") as WebControl;
                    if (ctrs != null)
                    {
                        ctrs.Visible = false;
                    }
                    return false;
                }
            }

            DropDownList type = Core.FindControlRecursive(Repeater1, "DropDownListUserType") as DropDownList;

            TextBox email = Core.FindControlRecursive(Repeater1, "EmailTextBox") as TextBox;
            /*
            if (email != null && ((type != null && type.SelectedItem.Value != "4") || email.Text != ""))
            {
                bool check = userManager.CheckEmail(email.Text, id);
                if (userManager.Error.IsError)
                {
                    Core.Site.RedirectError(Response, userManager.Error.Message);
                    return false;
                }
                if (!check)
                {
                    HtmlContainerControl ctr = Core.FindControlRecursive(Repeater1, "ValidPass") as HtmlContainerControl;
                    if (ctr != null)
                    {
                        ctr.Visible = true;
                        ctr.InnerHtml = (editMode == EditMode.Add ?
                        "Пользователь не зарегистрирован" : "Изменения не сохранены") +
                        "<ul><li>Такой e-mail уже есть в базе</li></ul>";
                    }
                    WebControl ctrs = Core.FindControlRecursive(Repeater1, "ValidationSummary1") as WebControl;
                    if (ctrs != null)
                        ctrs.Visible = false;
                    return false;
                }
            }
            */
        }
        return true;
        //}
        //catch (Exception er)
        //{
        //    Core.Site.RedirectError(Response, er.Message);
        //    return false;
        //}
    }

    public object CheckBatteryIndex(object value)
    {
        DropDownList list = Core.FindControlRecursive(Repeater1, "BatteryList") as DropDownList;
        
        var input = list.Items.FindByValue(value.ToString());
        if (input == null)
        {
            value = -1;
        }

        //format the value and send it back
        return value;
    }

    public object CheckCompanyNameIndex(object value)
    {
        DropDownList list = Core.FindControlRecursive(Repeater1, "CompanyList") as DropDownList;

        var input = list.Items.FindByValue(value.ToString());
        if (input == null)
        {
            value = -1;
        }

        //format the value and send it back
        return value;
    }


    public class MailException : Exception
    {
    }

    protected void DropDownListUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox pwd = Core.FindControlRecursive(Repeater1, "ValidPass") as TextBox;
        DropDownList type = Core.FindControlRecursive(Repeater1, "DropDownListUserType") as DropDownList;
        if (type.SelectedValue == "4")
            pwd.Enabled = false;
        else
            pwd.Enabled = true;
    }

    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
}
