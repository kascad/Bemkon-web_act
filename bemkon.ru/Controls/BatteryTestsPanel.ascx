<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BatteryTestsPanel.ascx.cs" 
Inherits="ProfessorTesting.BatteryTestsPanel" %>

<div>
<div>

    <asp:GridView ID="dataGridViewBattaries" runat="server" 
        DataKeyNames="BatteryID"
        AutoGenerateColumns="False" Width="100%"
        CellPadding="5" EmptyDataText="Батарей тестов нет"
        GridLines="Horizontal" 
        BorderColor="Transparent" 
        HeaderStyle-BackColor="#4891C6" HeaderStyle-BorderColor="White"
        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="header_table" 
        HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px"
        RowStyle-BorderColor="Gray"
        >
        <RowStyle BorderColor="Gray" />
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="BatteryID" 
                DataNavigateUrlFormatString="~/User/TestExaminee.aspx?id_battery={0}&from=2" 
                DataTextField="BatteryName" DataTextFormatString="{0}" HeaderText="Батарея" />
            <asp:BoundField DataField="BatteryTests" HeaderText="Тесты батареи" />
            <asp:BoundField DataField="Description" HeaderText="Описание" />
        </Columns>
        <HeaderStyle BackColor="#4891C6" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" CssClass="header_table" ForeColor="White" />
    </asp:GridView>

</div>

<div style="padding-top:20px;">
<asp:Label ID="labelBatteryCount" runat="server" Text="Всего батарей тестов: {0}"></asp:Label>
</div>

</div>


