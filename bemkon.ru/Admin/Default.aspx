<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" Inherits="Admin_Default" Codebehind="Default.aspx.cs" %>

<%@ Register TagPrefix="uc" TagName="MenuAdmin" Src="~/Controls/MenuAdmin.ascx" %>
<%@ Register TagPrefix="uc" TagName="AdminUsers" Src="~/Controls/AdminUsers.ascx" %>


<asp:Content ID="Content6" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/Admin/">
    Пользователи
    </asp:HyperLink>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Меню Администратора
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuAdmin id="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:AdminUsers id="AdminUsers1" runat="server" />
</asp:Content>


<%--<%@ Reference Control="~/Controls/AdminUsers.ascx" %>--%>