<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestingExamineePanel.ascx.cs" 
Inherits="ProfessorTesting.TestingExamineePanel" %>


<div>
<div style="padding-bottom:20px;"><asp:Label ID="LabelTitle" runat="server" 
Text="Тест: {0}" CssClass="header_block" >
</asp:Label>
</div>

<div runat="server" id="beginTestPanel">
    <div runat="server" id="beginPanel" visible="false">
        <div runat="server" id="Preamble">
        </div>
        <center><div style="padding-bottom:20px;" runat="server" id="BeginTest">
            <asp:LinkButton ID="LinkButtonBegin" CssClass="button_begin" runat="server">
            Начать тестирование</asp:LinkButton>&nbsp;&nbsp;&nbsp;
            <asp:HyperLink runat="server" ID="ButtonClose" Visible="false">Закрыть</asp:HyperLink>
        </div></center>
    </div>

    <center><div runat="server" id="messagePanel" visible="false" style="width:500px;">
        <div style="text-align:center; color:Red; font-weight:bold;padding-bottom:10px;">
        Внимание!
        </div>
        <div runat="server" id="message">
        </div>
        <div style="padding-top:10px;">
            <asp:LinkButton ID="LinkButtonYes" runat="server" Visible="false">Продолжить тестирование</asp:LinkButton>&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButtonNo" runat="server" Visible="false">Начать тестирование заново</asp:LinkButton>&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButtonCancel" runat="server" Visible="false">Отмена</asp:LinkButton>
        </div>
    </div></center>

</div>

<div runat="server" id="errorMsgPanel" visible="false" style="padding:20px;">
    <div>
    <asp:Label runat="server" ID="LabelError" ForeColor="Red" Text="Тестирование невозможно по следующей причине: <br />Обследуемый не выбран!"></asp:Label>
    </div>
    <div style="padding-top:20px;">
    <asp:HyperLink runat="server" ID="ButtonSelectExam" NavigateUrl="~/User/">Выбрать обследуемого</asp:HyperLink>
    </div>
</div>

</div>