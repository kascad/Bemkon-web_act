<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" 
Inherits="Activation" Title="Активация пользователя" Codebehind="Activation.aspx.cs" %>

<%@ Reference  VirtualPath="~/Controls/ActivationPanel.ascx" %>
<%@ Register TagPrefix="uc" TagName="MenuLogin" Src="~/Controls/MenuLogin.ascx" %>
<%@ Register TagPrefix="uc" TagName="ActivationPanel" Src="~/Controls/ActivationPanel.ascx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/Activation.aspx">
    Активация пользователя
    </asp:HyperLink>
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HedearLeftMenu">
    Авторизация
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentLeftMenu">
    <uc:MenuLogin ID="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="MainContent">
    <uc:ActivationPanel ID="ActivationPanel1" runat="server" />
</asp:Content>