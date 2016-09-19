<%@ Control Language="C#" AutoEventWireup="true" 
Inherits="Controls_MenuAdmin" Codebehind="MenuAdmin.ascx.cs" %>

    <div class="menu_item">
        <asp:HyperLink NavigateUrl="~/Admin/" ID="AdminUsers" runat="server">Пользователи</asp:HyperLink>
    </div>
    <div class="menu_item">
        <asp:HyperLink NavigateUrl="~/Admin/EditUser.aspx?id=-1" ID="NewUser" runat="server">Новый пользователь</asp:HyperLink>
     </div>
     <div class="menu_item">
        <asp:HyperLink NavigateUrl="~/Admin/EditCompany.aspx?id=-1" ID="NewCompany" runat="server">Список компаний</asp:HyperLink>
    </div>
    <div class="menu_item">
        <asp:HyperLink NavigateUrl="~/Admin/Customize.aspx" ID="AdminCustomize" runat="server">Настройки</asp:HyperLink>
    </div>
    <div>
        <asp:HyperLink ID="Exit" NavigateUrl="~/LogOut.aspx" runat="server">Выход</asp:HyperLink>
    </div>
