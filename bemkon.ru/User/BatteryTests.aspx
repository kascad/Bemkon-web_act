﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatteryTests.aspx.cs" 
Inherits="ProfessorTesting.BatteryTests" MasterPageFile="~/Main.master" %>

<%@ Register TagPrefix="uc" TagName="MenuUser" Src="~/Controls/MenuUser.ascx" %>
<%@ Register TagPrefix="uc" TagName="BatteryTestsPanel" Src="~/Controls/BatteryTestsPanel.ascx" %>


<asp:Content ID="ContentHorMenuModule2" ContentPlaceHolderID="HorMenuModule2" Runat="Server">
    <asp:Label ID="labelArchName" runat="server">Нет открытого архива</asp:Label>
</asp:Content>

<asp:Content ID="ContentHorMenuModule3" ContentPlaceHolderID="HorMenuModule3" Runat="Server">
    <asp:Label ID="labelExamName" runat="server">Обследуемый не выбран</asp:Label>
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/User/BatteryTests.aspx">
    Батареи тестов
    </asp:HyperLink>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Меню Пользователя
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuUser id="BlockMenu1" runat="server" />
    
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:BatteryTestsPanel id="BatteryTestsPanel1" runat="server" />
</asp:Content>
