<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteExamineePanel.ascx.cs" 
Inherits="ProfessorTesting.DeleteExamineePanel" %>

<div style="padding-bottom:20px;">
<asp:Label ID="LabelTitle" runat="server" 
Text="Удаление обследуемого" CssClass="header_block" >
</asp:Label>
</div>

<div>
<table>
    <tr>
        <td>
            <asp:Label runat="server" ID="confirm">Удалить обследуемого?</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center" valign="top">
            <div style="padding-top:20px;">
                <asp:LinkButton ID="buttonDeleteSave" runat="server">Удалить</asp:LinkButton>
                &nbsp;&nbsp;&nbsp;
                <asp:HyperLink runat="server" ID="buttonClose" NavigateUrl="~/User/">Отмена</asp:HyperLink>
            </div>    
        </td>
    </tr>
</table>
</div>