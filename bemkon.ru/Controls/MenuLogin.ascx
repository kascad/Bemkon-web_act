<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_MenuLogin" Codebehind="MenuLogin.ascx.cs" %>

<div>
    <asp:Login ID="Login1" runat="server" 
        TitleText="" 
        UserNameLabelText="Логин"
        PasswordLabelText="Пароль"
        RememberMeText="Запомнить меня"
        LoginButtonText="Вход" onauthenticate="Login1_Authenticate">
    </asp:Login>

<%--    
    <div class="admin_command" style="text-align:center; width:100%;">
    <asp:HyperLink ID="Registration" NavigateUrl="~/Registration.aspx" runat="server">Регистрация</asp:HyperLink>
    </div>
--%>

</div>

