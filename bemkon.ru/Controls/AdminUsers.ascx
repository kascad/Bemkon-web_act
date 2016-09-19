<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_AdminUsers" ClassName="AdminUsers"
    CodeBehind="AdminUsers.ascx.cs" %>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings: MyConnectionString %>"
    SelectCommand="SELECT * FROM [Users]" DeleteCommand="DELETE FROM Users WHERE [ID]=@ID">
    <DeleteParameters>
        <asp:Parameter Name="ID" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>
<div class="admin_header">
    Пользователи сайта
</div>
<div>
    <asp:GridView ID="GridView1" runat="server" Width="100%" DataSourceID="SqlDataSource1"
        AutoGenerateColumns="False" DataKeyNames="ID" OnRowDeleted="Grid_RowDeleted"
        CellPadding="7" GridLines="Horizontal" BorderColor="Transparent" HeaderStyle-BackColor="#4891C6"
        HeaderStyle-BorderColor="White" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="header_table"
        HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" RowStyle-BorderColor="Gray"
        OnDataBinding="GridView1_DataBinding" OnRowDataBound="GridView1_RowDataBound"
        CaptionAlign="Top">
        <RowStyle BorderColor="Gray"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="№">
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:TemplateField>
            <asp:BoundField HeaderText="Имя" DataField="nName">
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Логин" DataField="UserName">
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Пароль" DataField="Password">
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Права">
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
                <ItemTemplate>
                    <%# Eval("Priv").ToString() == "0" ? "Нет" : "" %>
                    <%# Eval("Priv").ToString() == "1" ? "Администратор"  : "" %>
                    <%# Eval("Priv").ToString() == "2" ? "Тестирование и интерпретация"  : "" %>
                    <%# Eval("Priv").ToString() == "3" ? "Разработчик"  : "" %>
                    <%# Eval("Priv").ToString() == "4" ? "Тестирование"  : "" %>
                    <%# Eval("Priv").ToString() == "5" ? "Интерпретация"  : "" %>
                    <%# Eval("Priv").ToString() == "6" ? "Оператор"  : "" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="Редактирование" DataNavigateUrlFormatString="~/Admin/EditUser.aspx?id={0}"
                Text="Редактировать">
                <ControlStyle CssClass="admin_command" Font-Underline="True" />
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:HyperLinkField>
            <asp:CommandField HeaderText="Удаление" ShowDeleteButton="true" DeleteText="Удалить">
                <ControlStyle CssClass="admin_command" Font-Underline="True" />
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:CommandField>
            <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="Блокирование" DataNavigateUrlFormatString="~/Admin/BanUser.aspx?id={0}"
                Text="Блокировать">
                <ControlStyle CssClass="admin_command" Font-Underline="True" />
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:HyperLinkField>
            <asp:BoundField HeaderText="" DataField="ID" ReadOnly="True" Visible="False">
                <HeaderStyle Width="0px" />
                <ItemStyle Width="0px" />
            </asp:BoundField>
            <asp:BoundField DataField="Ban" HeaderText="Ban" SortExpression="Ban" Visible="False">
                <HeaderStyle Width="0px" />
                <ItemStyle Width="0px" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Priv" DataField="Priv" ReadOnly="True" Visible="False">
                <HeaderStyle Width="0px" />
                <ItemStyle Width="0px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Ссылка">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("UserName", "http://l1k.ru/Login.aspx?TestUser={0}") %>' Text="ссылка"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="ID компании" DataField="CompanyID">
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Нзвание компании" DataField="CompanyName">
                <HeaderStyle BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                <ItemStyle CssClass="table_item" />
            </asp:BoundField>
        </Columns>
        <SelectedRowStyle BackColor="#82B9F2" />
        <HeaderStyle BackColor="#4891C6" BorderColor="White" BorderWidth="1px" BorderStyle="Solid"
            CssClass="header_table" ForeColor="White"></HeaderStyle>
        <EmptyDataTemplate>
            Пользователей нет
        </EmptyDataTemplate>
    </asp:GridView>
</div>
