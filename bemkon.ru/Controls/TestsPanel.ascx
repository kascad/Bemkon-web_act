<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestsPanel.ascx.cs" 
Inherits="ProfessorTesting.TestsPanel" %>

<div>

<div>
    <table runat="server" id="listTest" width="100%" cellpadding="4">
        <tr style="background:#4891C6; color:White; font-weight:bold;">
            <td align="center">
            Полное имя теста
            </td>
            <td align="center">
            Тест
            </td>
            <td align="center">
            Автор
            </td>
            <td align="center">
            Дата
            </td>
        </tr>
    </table>
</div>

<div style="padding-top:20px;padding-bottom:20px;">
<asp:Label ID="labelAllTests" runat="server" Text="Всего тестов: {0}"></asp:Label>
</div>

</div>