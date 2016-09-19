<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_EditCompanyPanel"
    CodeBehind="EditCompanyPanel.ascx.cs" %>
<style type="text/css">
    .auto-style4 {
        width: 37px;
    }
    .auto-style5 {
        width: 114px;
    }
</style>
<div style="padding-bottom: 20px;">
    <asp:Label ID="LabelTitle" runat="server" Text="Редактирование компании" CssClass="header_block">
    </asp:Label>    
</div>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" OnInserted="SqlDataSource_OnInserted"
    ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" DeleteCommand="DELETE FROM [Company] WHERE [ID] = @ID"
    InsertCommand="INSERT INTO [Company] ([ID], [CompanyName]]) VALUES (@ID, @CompanyName)"
    SelectCommand="SELECT * FROM [Company]" UpdateCommand="UPDATE [Company] SET [CompanyName] = @CompnanyName WHERE [ID] = @ID" OnSelecting="SqlDataSource1_Selecting">
    <UpdateParameters>
        <asp:Parameter Name="CompanyName" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="ID" Type="Int32" Direction="Output" />
        <asp:Parameter Name="CompanyName" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" SelectCommand="SELECT * FROM [Batteries]"></asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" SelectCommand="SELECT * FROM [Company]"></asp:SqlDataSource>
<div style="width: 500px;" runat="server" id="divRegistration">
    <div>
        <asp:FormView DefaultMode="Edit" ID="Repeater1" runat="server" DataKeyNames="ID"
            DataSourceID="SqlDataSource1" OnItemUpdated="FormView_ItemUpdated" OnItemInserted="FormView_ItemInserted"
            OnItemUpdating="FormView_ItemUpdating" OnItemInserting="FormView_ItemInserting">
            <EditItemTemplate>
                <table style="width: 100%; margin-right: 0px;">
                    <tr>
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="CompanyNameTextBox" ID="CompanyNameLabel">Название компании:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:TextBox runat="server" ID="NameTextBox" Text='<%# Bind("CompanyName") %>' />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="CompanyNameTextBox" ErrorMessage="Введите название"
                                ToolTip="Обязательное поле" ID="CompanyNameRequired">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                    <tr id="CompanyGroup" runat="server">
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="CompanyList" ID="CompanyLabel">Список компаний:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:DropDownList ID="CompanyList"  runat="server" DataSourceID="SqlDataSource1" DataValueField="Id" AppendDataBoundItems="True" DataTextField="CompanyName" >
                                <asp:ListItem Value="0">Компания не указана...</asp:ListItem>
                            </asp:DropDownList>                        
                        </td>                      
                    </tr>
                </table>
                <br />
                <asp:Label ID="ID" runat="server" Visible="False" Text='<%# Eval("ID") %>'></asp:Label>
                <div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Изменения не сохранены" />
                    <div id="ValidPass" runat="server" visible="false" style="color: Red;">
                    </div>
                </div>
                <div style="text-align: center;">
                    <asp:LinkButton ID="UpdateButton" runat="server" Text="Сохранить изменения" CommandName="Update"
                        CausesValidation="True" />
                    &nbsp;&nbsp;
                    <asp:HyperLink ID="CancelUpdateButton" NavigateUrl="~/Default.aspx" runat="server">Отмена</asp:HyperLink>
                </div>
            </EditItemTemplate>
            <InsertItemTemplate>
                <table style="width: 100%; margin-right: 0px;">
                    <tr id="CompanyGroup" runat="server">
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="CompanyList" ID="CompanyLabel">Список компаний:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:DropDownList ID="CompanyList"  runat="server" DataSourceID="SqlDataSource1" DataValueField="Id" AppendDataBoundItems="True" DataTextField="CompanyName" >
                                <asp:ListItem Value="0">Компания не указана...</asp:ListItem>
                            </asp:DropDownList>                        
                        </td>                      
                    </tr>
                </table>
                <br />
                <asp:Label ID="ID" runat="server" Visible="False" Text='<%# Bind("ID")%>'></asp:Label>
                <div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Компания не зарегистрирована" />
                    <div id="ValidPass" runat="server" visible="false" style="color: Red;">
                    </div>
                </div>
                <div style="text-align: center;">
                    <asp:LinkButton ID="UpdateButton" runat="server" Text="Добавление" CommandName="Insert" />
                    &nbsp;&nbsp;
                    <asp:HyperLink ID="CancelUpdateButton" NavigateUrl="~/Default.aspx" runat="server">Отмена</asp:HyperLink>
                </div>
            </InsertItemTemplate>
        </asp:FormView>
    </div>
</div>
<div runat="server" id="successRegistration" visible="false">
    Компания успешно добавлена<br />    
</div>
