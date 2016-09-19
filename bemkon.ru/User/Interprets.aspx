<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Interprets.aspx.cs" 
Inherits="ProfessorTesting.User.Interprets" MasterPageFile="~/Main.master" %>

<%@ Register TagPrefix="uc" TagName="MenuUser" Src="~/Controls/MenuUser.ascx" %>

<asp:Content ID="ContentHorMenuModule2" ContentPlaceHolderID="HorMenuModule2" Runat="Server">
    <asp:Label ID="labelArchName" runat="server">Нет открытого архива</asp:Label>
</asp:Content>

<asp:Content ID="ContentHorMenuModule3" ContentPlaceHolderID="HorMenuModule3" Runat="Server">
    <asp:Label ID="labelExamName" runat="server">Обследуемый не выбран</asp:Label>
</asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" 
    NavigateUrl="~/User/Interprets.aspx">
    Интерпретация
    </asp:HyperLink>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" Runat="Server">
    Меню Пользователя
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <uc:MenuUser id="BlockMenu1" runat="server" />
    
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" Runat="Server">
    <div>
        <div style="padding-bottom:20px;"><asp:Label ID="LabelTitle" runat="server" 
        Text="Интерпретации" CssClass="header_block" >
        </asp:Label>
        </div>


        <div>
            <table runat="server" id="listInterpret" width="100%" cellpadding="4">
                <tr style="background:#4891C6; color:White; font-weight:bold;">
                    <td align="center">
                    Полное название
                    </td>
                    <td align="center">
                    Интерпретация
                    </td>
                </tr>
            </table>
        </div>

        <div style="padding-top:20px;padding-bottom:20px;">
        <asp:Label ID="labelAllInterpret" runat="server" Text="Всего интерпретаций: 0"></asp:Label>
        </div>

    </div>
</asp:Content>
