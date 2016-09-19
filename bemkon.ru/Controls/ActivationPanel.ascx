<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_ActivationPanel" Codebehind="ActivationPanel.ascx.cs" %>
<style type="text/css">
    .style1
    {
    }
    .style2
    {
        width: 202px;
    }
</style>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" >
    </asp:SqlDataSource>

<p>
<asp:Label ID="LabelTitle" runat="server" 
Text="Активация пользователя" CssClass="header_block" >
</asp:Label>
</p>

<p>
<asp:Label ID="LabelSuccess" runat="server" 
    Text="Ваш аккаунт успешно подтвержден!" Visible="False"></asp:Label>

</p>
<p>
    <asp:Label ID="LabelError" runat="server" Font-Bold="True" ForeColor="#CC3300" 
        Text="Аккаунт не подтвержден" Visible="False"></asp:Label>
</p>

<table style="width: 55%; margin-right: 0px;" runat="server" id="inputActivation">
    <tr>
        <td class="style2">
            &nbsp;
            <asp:Label ID="Label1" runat="server" Text="ID пользователя:"></asp:Label>
        </td>
        <td>
            &nbsp;
            <asp:TextBox ID="TextBoxID" runat="server" Width="230px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;
            <asp:Label ID="Label2" runat="server" Text="Ключ подтверждения:"></asp:Label>
        </td>
        <td>
            &nbsp;
            <asp:TextBox ID="TextBoxKey" runat="server" Width="230px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="center" class="style1" colspan="2">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="center" class="style1" colspan="2">
            &nbsp;
            &nbsp;
            <asp:LinkButton ID="LinkButtonActivation" runat="server" 
                onclick="LinkButtonActivation_Click">Подтвердить</asp:LinkButton>
        </td>
    </tr>
</table>


