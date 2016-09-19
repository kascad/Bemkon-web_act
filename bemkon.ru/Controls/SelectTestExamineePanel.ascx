<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectTestExamineePanel.ascx.cs"
Inherits="ProfessorTesting.SelectTestExamineePanel" %>

<div>
    <div style="padding-bottom:20px;"><asp:Label ID="LabelTitle" runat="server" 
    Text="Тестирование. Выбор теста" CssClass="header_block" >
    </asp:Label>
    </div>
</div>

<div>
    <table runat="server" id="listTitleTest">
    </table>
</div>

<hr />

<div style="margin-bottom:20px;">
<table runat="server" id="listTest">
</table>
</div>

