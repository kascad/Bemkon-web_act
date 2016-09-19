<%@ Control Language="C#" AutoEventWireup="true" 
Inherits="Controls_MenuOperator" Codebehind="MenuOperator.ascx.cs" %>

    <div class="menu_item">
        <asp:HyperLink NavigateUrl="~/Operator/" ID="OperatorUsers" runat="server">Пользователи</asp:HyperLink>
    </div>
        <asp:HyperLink ID="Exit" NavigateUrl="~/LogOut.aspx" runat="server">Выход</asp:HyperLink>
    </div>
