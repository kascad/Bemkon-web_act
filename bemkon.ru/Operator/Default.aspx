﻿<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" Inherits="Operator_Default" Codebehind="Default.aspx.cs" %>

<%@ Register TagPrefix="uc" TagName="MenuOperator" Src="~/Controls/MenuOperator.ascx" %>
<%@ Register TagPrefix="uc" TagName="OperatorUsers" Src="~/Controls/OperatorUsers.ascx" %>


<asp:Content ID="Content6" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/Operator/">
    Пользователи
    </asp:HyperLink>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Меню Оператора
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuOperator id="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:OperatorUsers id="OperatorUsers1" runat="server" />
</asp:Content>


<%--<%@ Reference Control="~/Controls/OperatorUsers.ascx" %>--%>