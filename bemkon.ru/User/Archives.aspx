<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Archives.aspx.cs" 
Inherits="ProfessorTesting.Archives" MasterPageFile="~/Main.master" %>

<%@ Register TagPrefix="uc" TagName="MenuUser" Src="~/Controls/MenuUser.ascx" %>
<%@ Register TagPrefix="uc" TagName="ArchiveMenu" Src="~/Controls/ArchiveMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="ArchivePanel" Src="~/Controls/ArchivePanel.ascx" %>

<asp:Content ID="ContentHorMenu" ContentPlaceHolderID="HorMenu" Runat="Server">
    <uc:ArchiveMenu id="ArchiveMenu1" runat="server" />
</asp:Content>

<asp:Content ID="ContentHorMenuModule2" ContentPlaceHolderID="HorMenuModule2" Runat="Server">
    <asp:Label ID="labelArchName" runat="server"><a href="/User/Archives.aspx?mod=open">Нет открытого архива</a></asp:Label>
</asp:Content>

<asp:Content ID="ContentHorMenuModule3" ContentPlaceHolderID="HorMenuModule3" Runat="Server">
    <asp:Label ID="labelExamName" runat="server"><a href="/User/">Обследуемый не выбран</a></asp:Label>
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" NavigateUrl="~/User/">
    Обследуемые</asp:HyperLink>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Меню Пользователя
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuUser id="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:ArchivePanel id="ArchivePanel1" runat="server" />
</asp:Content>
