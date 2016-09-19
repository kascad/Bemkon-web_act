<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewTestExamineePanel.ascx.cs"
    Inherits="ProfessorTesting.ViewTestExamineePanel" %>
<style type="text/css">
    .style1
    {
        width: 289px;
    }
    .style2
    {
        width: 191px;
    }
</style>
<div style="padding-bottom: 20px;">
    <asp:Label ID="LabelTitle" runat="server" Text="Тест" CssClass="header_block">
    </asp:Label>
</div>
<div>
    <table>
        <tr>
            <td class="style2">
                <asp:Label ID="Label1" runat="server" Text="Тест:"></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="textBoxTestName" runat="server" Width="79px" ReadOnly="True"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" Text="&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;Дата:"></asp:Label>
                <asp:TextBox ID="textBoxDate" runat="server" Width="136px" ReadOnly="True"></asp:TextBox>
            </td>
            <td rowspan="3" valign="top" align="center">
                <div>
                    <asp:Label ID="labelTestStatus" runat="server" Text="Тест завершен!"></asp:Label>
                </div>
                <div style="padding-top: 10px;">
                    <asp:Image ID="Image1" runat="server" Height="48px" ImageUrl="~/Images/Copy v2_48x48.png" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <asp:Label ID="Label2" runat="server" Text="Обследуемый:"></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="textBoxExamName" runat="server" Width="266px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <asp:Label ID="Label3" runat="server" Text="Отвеченных вопросов:"></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="textBoxAllQuestCount" runat="server" Width="41px" ReadOnly="True"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" Text="Общее время:"></asp:Label>
                <asp:TextBox ID="textBoxFullTime" runat="server" ReadOnly="True" Width="134px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div style="padding-top: 20px; padding-bottom: 20px;">
                    <asp:GridView ID="dataGridViewResults" runat="server" DataKeyNames="AnsID" AutoGenerateColumns="False"
                        CellPadding="5" EmptyDataText="Тестов нет" GridLines="Both" Width="100%" BorderColor="Transparent"
                        HeaderStyle-BackColor="#4891C6" HeaderStyle-BorderColor="White" HeaderStyle-ForeColor="White"
                        HeaderStyle-CssClass="header_table" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px"
                        RowStyle-BorderColor="Gray">
                        <Columns>
                            <asp:BoundField DataField="QuestNumber" HeaderText="№ вопр." />
                            <asp:BoundField DataField="QuestText" HeaderText="Вопрос" />
                            <asp:BoundField DataField="AnsText" HeaderText="Ответ" />
                            <asp:BoundField DataField="Time" HeaderText="Время" />
                            <asp:TemplateField HeaderText="Вес (шкала)" >
                                <ItemTemplate>
                                    <%# GetWeight(Eval("AnsID"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="right">
                <asp:HyperLink ID="HyperLinkCloseBottom" runat="server" NavigateUrl="~/User/EditExaminee.aspx?id={0}">Закрыть</asp:HyperLink>
            </td>
        </tr>
    </table>
</div>
