<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditExamineePanel.ascx.cs" Inherits="ProfessorTesting.Controls.EditExamineePanel" %>

<style type="text/css">
    .auto-style1 {
        width: 520px;
    }
    .auto-style2 {
        width: 88px;
    }
</style>
<script type="text/javascript" language="javascript">
function CopyToClipboard() { 
    var control = document.getElementById('<%=TestingLink.ClientID%>');
    if (window.clipboardData && window.clipboardData.setData)
        window.clipboardData.setData('Text', control.value);
    else {
        control.select();
        document.execCommand("copy");
        control.deselectAll();
    }
}
</script>
<asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" SelectCommand="SELECT * FROM [Batteries]"></asp:SqlDataSource>

<div style="padding-bottom:20px;">
<asp:Label ID="LabelTitle" runat="server" 
Text="Редактирование обследуемого" CssClass="header_block" >
</asp:Label>
</div>

<div>
    <table border="0" cellpadding="5" class="auto-style1">
        <tr>
            <td colspan="3" style="color:Red;">
                <asp:Label runat="server" ID="labelError"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="labelID">Номер:</asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="textBoxID" ReadOnly="True" Width="79px"></asp:TextBox>
            </td>
            <td rowspan="2" align="right" valign="top">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/User_48x48.png" />
            </td>
        </tr>
        
        <tr>
            <td>
                <asp:Label runat="server" ID="labelName">Имя:</asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="textBoxName" Width="263px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldName" runat="server" 
                    ControlToValidate="textBoxName" ErrorMessage="Введите имя обследуемого">*</asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label runat="server" ID="labelTests">Результаты:</asp:Label>
            </td>
            <td colspan="2">
                <asp:Repeater runat="server" ID="listViewTests" ></asp:Repeater>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label runat="server" ID="labelComments">Комментарии:</asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="textBoxComments" Rows="3" TextMode="MultiLine" 
                    Width="384px"></asp:TextBox>
            </td>            
        </tr>
                    <tr id="BatteryGroup" runat="server">
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="BatteryList" ID="BatteryLabel">Батарея тестов:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:DropDownList ID="BatteryList"  runat="server" DataSourceID="SqlDataSource" DataValueField="BatteryID" AppendDataBoundItems="True" DataTextField="BatteryName" >
                                <asp:ListItem Value="0">Не указана...</asp:ListItem>
                            </asp:DropDownList>                        
                        </td>                      
                    </tr>

        <tr>
            <td>
                <asp:Label runat="server" ID="label2">Прямая ссылка:</asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="TestingLink" Rows="1" TextMode="SingleLine" 
                    Width="291px" ReadOnly="True"></asp:TextBox>      
                <asp:Button ID="CopyLinkButton" runat="server" Text="Скопировать в буфер" OnClientClick="CopyToClipboard()" />
            </td>
            <td><asp:Button ID="CreateLinkButton" runat="server" Text="Создать/Обновить" OnClick="CreateLinkButton_Click" /></td>                
        </tr>

        <tr>
            <td colspan="3">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </td>
        </tr>

        <tr>
            <td colspan="3" align="center" style="padding-top:20px;">
                <asp:LinkButton runat="server" ID="buttonEditSave">Сохранить</asp:LinkButton>
                &nbsp;&nbsp;&nbsp;
                <asp:HyperLink runat="server" ID="buttonClose" NavigateUrl="~/User/">Отмена</asp:HyperLink>
            </td>
        </tr>
    </table>
</div>

