<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTest.aspx.cs" 
Inherits="ProfessorTesting.ViewTest" MasterPageFile="~/Main.master" %>

<%@ Register TagPrefix="uc" TagName="MenuUser" Src="~/Controls/MenuUser.ascx" %>
<%@ Register TagPrefix="uc" TagName="ArchiveMenu" Src="~/Controls/ArchiveMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="ExamineeMenu" Src="~/Controls/ExamineeMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="ViewTestExamineePanel" Src="~/Controls/ViewTestExamineePanel.ascx" %>


<asp:Content ID="ContentHorMenu" ContentPlaceHolderID="HorMenu" Runat="Server">
    <uc:ArchiveMenu id="ArchiveMenu1" runat="server" />

    <span style="margin-left:60px;">
        <span style="background:url('../Images/Excel_16x16.png') left no-repeat; padding-left:20px;">
            <asp:LinkButton ID="ExportExcel" runat="server">
            Экспорт в Excel</asp:LinkButton>
        </span>
    </span>
</asp:Content>

<asp:Content ID="ContentHorMenuModule2" ContentPlaceHolderID="HorMenuModule2" Runat="Server">
    <asp:Label ID="labelArchName" runat="server">Нет открытого архива</asp:Label>
</asp:Content>

<asp:Content ID="ContentHorMenuModule3" ContentPlaceHolderID="HorMenuModule3" Runat="Server">
    <asp:Label ID="labelExamName" runat="server">Обследуемый не выбран</asp:Label>
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/User/">
    Обследуемые</asp:HyperLink>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Меню Пользователя
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuUser id="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:ViewTestExamineePanel id="ViewTestExamineePanel1" runat="server" />
</asp:Content>
