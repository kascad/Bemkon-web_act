<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" Inherits="RegisterUser" Title="Untitled Page" Codebehind="Registration.aspx.cs" %>
<%@ Reference  VirtualPath="~/Controls/EditUserPanel.ascx" %>
<%@ Register TagPrefix="uc" TagName="MenuLogin" Src="~/Controls/MenuLogin.ascx" %>
<%@ Register TagPrefix="uc" TagName="EditUserPanel" Src="~/Controls/EditUserPanel.ascx" %>

<asp:Content ID="Content6" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/Registration.aspx">
    Регистрация
    </asp:HyperLink>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Авторизация
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuLogin id="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:EditUserPanel id="EditUserPanel1" runat="server" />
</asp:Content>
