<%@ Control Language="C#" AutoEventWireup="true" 
Inherits="Controls_ChangePasswordPanel" Codebehind="ChangePasswordPanel.ascx.cs" %>

<style type="text/css">
    .style1
    {
        width: 175px;
    }
    .style2
    {
        width: 141px;
    }
    .style4
    {
        width: 175px;
        height: 26px;
    }
    .style5
    {
        width: 141px;
        height: 26px;
    }
    .style6
    {
        height: 26px;
    }
</style>

<asp:Label ID="LabelTitle" runat="server" 
Text="Смена пароля пользователя" CssClass="header_block" >
</asp:Label>
<br />
<br />
<table style="width: 51%;">
    <tr>
        <td align="right" class="style1">
            <asp:Label ID="Label1" runat="server" Text="Текущий пароль:"></asp:Label>
        </td>
        <td class="style2">
            <asp:TextBox ID="PasswordTextBox" runat="server" style="margin-bottom: 0px" 
                TextMode="Password"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="Введите Текущий пароль" ControlToValidate="PasswordTextBox" ToolTip="Обязательное поле">*</asp:RequiredFieldValidator>
                
        </td>
    </tr>
    <tr>
        <td align="right" class="style4">
            <asp:Label ID="Label2" runat="server" Text="Новый пароль:"></asp:Label>
        </td>
        <td class="style5">
            <asp:TextBox ID="NewPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
        </td>
        <td class="style6">
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                HeaderText="Пароль не изменен" />
            <div id="ValidPass" runat="server" visible="false" style="color:Red;"></div>

        </td>
    </tr>
    <tr>
        <td align="center" colspan="3">
            <asp:LinkButton ID="LinkUpdate" runat="server" onclick="LinkUpdate_Click">Изменить 
            пароль</asp:LinkButton>
&nbsp;&nbsp;&nbsp;
            <asp:HyperLink ID="HyperLinkCancel" runat="server">Отмена</asp:HyperLink>
        </td>
    </tr>
</table>
<br />







