<%@ Page Language="C#" AutoEventWireup="true" 
Inherits="EditCompany" MasterPageFile="~/Main.master" Codebehind="EditCompany.aspx.cs" %>

<%@ Register TagPrefix="uc" TagName="MenuAdmin" Src="~/Controls/MenuAdmin.ascx" %>
<%@ Register TagPrefix="uc" TagName="EditCompanyPanel" Src="~/Controls/EditCompanyPanel.ascx" %>

<asp:Content ID="Content6" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/Admin/">
    Компании
    </asp:HyperLink>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Меню Администратора
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuAdmin id="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:EditCompanyPanel id="EditCompanyPanel1" runat="server" />
</asp:Content>
