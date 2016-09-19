<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Top10TestsPanel.ascx.cs" 
Inherits="ProfessorTesting.Top10TestsPanel" %>

<div>
<div style="padding-bottom:20px;"><asp:Label ID="LabelTitle" runat="server" 
Text="Топ 10 тестов" CssClass="header_block" >
</asp:Label>
</div>

<div>

    <asp:GridView ID="listViewTop10Tests" runat="server" 
        DataKeyNames="TestID"
        AutoGenerateColumns="False" Width="100%"
        CellPadding="5" EmptyDataText="Тестов нет"
        GridLines="Horizontal" 
        BorderColor="Transparent" 
        HeaderStyle-BackColor="#4891C6" HeaderStyle-BorderColor="White"
        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="header_table" 
        HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px"
        RowStyle-BorderColor="Gray"
        >
        <RowStyle BorderColor="Gray" />
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="TestID" 
                DataNavigateUrlFormatString="~/User/TestExaminee.aspx?id_test={0}&amp;from=4" 
                DataTextField="FullName" DataTextFormatString="{0}" 
                HeaderText="Полное название теста" />
            <asp:BoundField DataField="ShortName" HeaderText="Тест" />
            <asp:BoundField DataField="TestingCount" HeaderText="Протестирован (раз)" >
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
        </Columns>
        <HeaderStyle BackColor="#4891C6" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" CssClass="header_table" ForeColor="White" />
    </asp:GridView>

</div>


</div>