<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" Inherits="Admin_ChangePassword" Title="Untitled Page" Codebehind="ChangePassword.aspx.cs" %>
<%@ Register TagPrefix="uc" TagName="MenuAdmin" Src="~/Controls/MenuAdmin.ascx" %>
<%@ Register TagPrefix="uc" TagName="ChangePasswordPanel" Src="~/Controls/ChangePasswordPanel.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Меню Администратора
</asp:Content>
<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;">
    </asp:HyperLink>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuAdmin id="BlockMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:ChangePasswordPanel id="ChangePasswordPanel1" runat="server" />
</asp:Content>

