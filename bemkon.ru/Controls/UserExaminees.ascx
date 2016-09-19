<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserExaminees.ascx.cs" 
Inherits="ProfessorTesting.UserExaminees" %>

<style type="text/css">
.row_item
{
	background-color:White;
	border-bottom:#CCC 1px solid;
	border-right:#FFF 1px solid;
}


.row_item:hover 
{
	background-color:#82B9F2;
}
</style>

<div>

<div>
    <asp:GridView ID="dataGridViewExaminees" runat="server" CellPadding="7"
        AutoGenerateColumns="False" BackColor="Transparent" DataKeyNames="Id"
        BorderColor="Transparent" HeaderStyle-BackColor="#4891C6" 
        HeaderStyle-BorderColor="White"
        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="header_table" 
        HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px"
        RowStyle-BorderColor="Gray" GridLines="Horizontal" 
        EmptyDataText="Обследуемых нет" Width="100%" 
        onrowcommand="dataGridViewExaminees_RowCommand">
        <RowStyle BorderColor="Gray" CssClass="row_item" />
        <Columns>
            <asp:BoundField HeaderText="№" DataField="Id">
            </asp:BoundField>
            <asp:HyperLinkField HeaderText="Обследуемый" 
            DataTextField="Name" DataTextFormatString="{0}"
            DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/User/EditExaminee.aspx?id={0}" >
            </asp:HyperLinkField>
            <asp:BoundField HeaderText="Пройденные тесты" DataField="Tests" >
            </asp:BoundField>
            <asp:BoundField HeaderText="Комментарий" DataField="Description" >
            </asp:BoundField>
            <asp:HyperLinkField HeaderText="Тестирование" Text="Тестировать"
                DataNavigateUrlFields="Id" HeaderStyle-HorizontalAlign="Left"
                DataNavigateUrlFormatString="~/User/SelectTestExaminee.aspx?from=1&id={0}">
            </asp:HyperLinkField>
            <asp:HyperLinkField HeaderText="Интерпретация" Text="Интерпретировать"
                DataNavigateUrlFields="Id" HeaderStyle-HorizontalAlign="Left"
                DataNavigateUrlFormatString="~/User/Interprets.aspx?id={0}">
            </asp:HyperLinkField>
            <asp:ButtonField CommandName="Select" Text="Выбрать" HeaderText="Выбор"  />
        </Columns>
        
        <SelectedRowStyle BackColor="#F0EBBE" />

        <HeaderStyle BackColor="#4891C6" ForeColor="White" BorderColor="White" 
            BorderStyle="Solid" BorderWidth="1px" CssClass="header_table" 
            Font-Bold="False" />
    </asp:GridView>
    
</div>
<div style="padding-top:20px;">
<asp:Label ID="labelExamCount" runat="server" Text="Всего обследуемых: 0"></asp:Label>
</div>

</div>
