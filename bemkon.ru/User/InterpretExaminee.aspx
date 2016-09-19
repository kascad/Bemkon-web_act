<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterpretExaminee.aspx.cs"
    Inherits="ProfessorTesting.User.InterpretExaminee" MasterPageFile="~/Main.master" %>

<%@ Register TagPrefix="uc" TagName="MenuUser" Src="~/Controls/MenuUser.ascx" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="Head" runat="Server">
<script>
    $(document).ready(function () {

        $('.tablesorter').tablesorter();

    });
</script>
</asp:Content>
<asp:Content ID="ContentHorMenu" runat="server" contentplaceholderid="HorMenu">
    <div runat="server" id="ExportMenu">
    <span style="margin-left:20px;"><span style="background: url('../Images/Word_16x16.png') left no-repeat; padding-left: 20px;">
    <asp:LinkButton ID="ExportWord" runat="server" OnClick="ExportWord_Click">
            Сохранить</asp:LinkButton>
    </span></span>
    </div>
    </asp:Content>
<asp:Content ID="ContentHorMenuModule2" ContentPlaceHolderID="HorMenuModule2" runat="Server">
    <asp:Label ID="labelArchName" runat="server">Нет открытого архива</asp:Label>
</asp:Content>
<asp:Content ID="ContentHorMenuModule3" ContentPlaceHolderID="HorMenuModule3" runat="Server">
    <asp:Label ID="labelExamName" runat="server">Обследуемый не выбран</asp:Label>
</asp:Content>
<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" Style="font-weight: bold;" NavigateUrl="~/User/Interprets.aspx">
    Интерпретация
    </asp:HyperLink>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" runat="Server">
    Меню Пользователя
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" runat="Server">
    <uc:MenuUser ID="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" runat="Server">
    <%--<div style="padding-bottom: 20px;">
        <asp:Label ID="LabelTitle" runat="server" Text="Тест: {0}" CssClass="header_block">
        </asp:Label>
    </div>--%>
    <asp:Literal ID="ltrChart" runat="server"></asp:Literal>

    <div runat="server" id="beginInterpretPanel">
        <asp:Literal runat="server" EnableViewState="false" ID="literalResult"></asp:Literal>
    </div>
    <div runat="server" id="errorMsgPanel" visible="false" style="padding: 20px;">
        <div>
            <asp:Label runat="server" ID="LabelError" ForeColor="Red" Text="Предварительно нужно выбрать обследуемого!"></asp:Label>
        </div>
        <div style="padding-top: 20px;">
            <asp:HyperLink runat="server" ID="ButtonSelectExam" NavigateUrl="~/User/">Выбрать обследуемого</asp:HyperLink>
        </div>
    </div>
    <div runat="server" id="infoExam">
    </div>
</asp:Content>

