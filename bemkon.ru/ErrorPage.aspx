<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" 
Inherits="ProfessorTesting.ErrorPage" MasterPageFile="~/Main.master" %>

<%@ Register TagPrefix="uc" TagName="MenuLogin" Src="~/Controls/MenuLogin.ascx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/Activation.aspx">
    Главная
    </asp:HyperLink>
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HedearLeftMenu">
    Авторизация
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentLeftMenu">
    <uc:MenuLogin id="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="MainContent">
    На странице произошла ошибка.
    <br />
    Попробуйте обновить страницу
</asp:Content>