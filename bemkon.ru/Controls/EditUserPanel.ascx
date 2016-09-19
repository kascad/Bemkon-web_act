<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_EditUserPanel"
    CodeBehind="EditUserPanel.ascx.cs" %>
<style type="text/css">
    .auto-style4 {
        width: 37px;
    }
    .auto-style5 {
        width: 114px;
    }
</style>
<div style="padding-bottom: 20px;">
    <asp:Label ID="LabelTitle" runat="server" Text="Редактирование пользователя" CssClass="header_block">
    </asp:Label>    
</div>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" OnInserted="SqlDataSource_OnInserted"
    ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" DeleteCommand="DELETE FROM [Users] WHERE [ID] = @ID"
    InsertCommand="INSERT INTO [Users] ([UserName], [Password], [Priv], [nName], [Ban], [Email], [Active], [AID], [Expiration], [LastBattery], [CompanyID], [CompanyName]) VALUES (@UserName, @Password, @Priv, @nName, @Ban, @Email, @Active, @AID, @Expiration, @LastBattery, @CompanyID, @CompanyName)"
    SelectCommand="SELECT * FROM [Users] WHERE ([ID] = @ID)" UpdateCommand="UPDATE [Users] SET [UserName] = @UserName, Priv=@Priv, [nName] = @nName, [Email] = @Email, [LastBattery] = @LastBattery, [CompanyID] = @CompanyID, [CompanyName] = @CompanyName WHERE [ID] = @ID" OnSelecting="SqlDataSource1_Selecting">
    <UpdateParameters>
        <asp:Parameter Name="nName" Type="String" />
        <asp:Parameter Name="UserName" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Priv" Type="Int32" />
        <asp:Parameter Name="LastBattery" Type="Int32" />
        <asp:Parameter Name="CompanyID" Type="Int32" />
        <asp:Parameter Name="CompanyName" Type="String" />
    </UpdateParameters>
    <InsertParameters>

        <asp:Parameter Name="UserName" Type="String" />
        <asp:Parameter Name="Password" Type="String" />
        <asp:Parameter Name="nName" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="prev" Type="Int32" />
        <asp:Parameter Name="LastBattery" Type="Int32" />
        <asp:Parameter Name="ID" Type="Int32" Direction="Output" />
        <asp:Parameter Name="CompanyID" Type="Int32" />
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
                            <asp:Label runat="server" AssociatedControlID="DropDownListUserType" ID="TypeLabel">Права:</asp:Label>
                         </td>
                        <td class="auto-style4">
                            <asp:DropDownList ID="DropDownListUserType"  runat="server" 
                                SelectedValue='<%# Bind("Priv") %>' Ong="DropDownListUserType_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="Нет"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Тестирование и интерпретация"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Разработчик"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Тестирование"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Интерпретация"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Оператор"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="NameTextBox" ID="NameLabel">Имя:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:TextBox runat="server" ID="NameTextBox" Text='<%# Bind("nName") %>' />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" ErrorMessage="Введите Имя"
                                ToolTip="Обязательное поле" ID="NameRequired">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="UserNameTextBox" ID="UserNameLabel">Логин:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:TextBox runat="server" ID="UserNameTextBox" Text='<%# Bind("UserName") %>' />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserNameTextBox" ToolTip="Обязательное поле"
                                ErrorMessage="Введите Логин" ID="UserNameRequired">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" ID="PasswordLabel">Пароль:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <input id="Password1" type="password" readonly="readonly" value='<%# Eval("Password")%>' />
                        </td>
                        <td>
                            <asp:HyperLink ID="hlChangePassword" runat="server" NavigateUrl='<%# Eval("ID", "~/ChangePassword.aspx?id={0}") %>'
                                Text="Сменить пароль"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="EmailTextBox" ID="EmailLabel">E-mail:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:TextBox runat="server" ID="EmailTextBox" Text='<%# Bind("Email") %>' />
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="EmailValidator1" runat="server" ErrorMessage="Некорректный e-mail"
                                ControlToValidate="EmailTextBox" ValidationExpression=".*@.{2,}\..{2,}">
                        *</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="BatteryGroup" runat="server">
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="BatteryList" ID="BatteryLabel">Батарея тестов:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:DropDownList ID="BatteryList"  runat="server" DataSourceID="SqlDataSource2" DataValueField="BatteryID" AppendDataBoundItems="True" DataTextField="BatteryName" >
                                <asp:ListItem Value="0">Не указана...</asp:ListItem>
                            </asp:DropDownList>                        
                        </td>                      
                    </tr>
                    <tr id="CompanyGroup" runat="server">
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="CompanyList" ID="CompanyLabel">Список компаний:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:DropDownList ID="CompanyList"  runat="server" DataSourceID="SqlDataSource3" DataValueField="Id" AppendDataBoundItems="True" DataTextField="CompanyName" >
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
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" AssociatedControlID="DropDownListUserType" ID="TypeLabel">Права:</asp:Label>
                         </td>
                        <td>
                            <asp:DropDownList ID="DropDownListUserType"  runat="server" 
                                SelectedValue='<%# Bind("Priv") %>'>
                                <asp:ListItem Value="0" Text="Нет"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Тестирование и интерпретация"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Разработчик"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Тестирование"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Интерпретация"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Оператор"></asp:ListItem>
                            </asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr>
                        <td align="right" class="style8">
                            <asp:Label runat="server" AssociatedControlID="NameTextBox" ID="NameLabel">Имя:</asp:Label>
                        </td>
                        <td class="style5">
                            <asp:TextBox runat="server" ID="NameTextBox" Text='<%# Bind("nName")%>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" ErrorMessage="Введите Имя"
                                ToolTip="Обязательное поле" ID="NameRequired">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3">
                            <asp:Label runat="server" AssociatedControlID="UserNameTextBox" ID="UserNameLabel">Логин:</asp:Label>
                        </td>
                        <td class="style6">
                            <asp:TextBox runat="server" ID="UserNameTextBox" Text='<%# Bind("UserName")%>'></asp:TextBox>
                        </td>
                        <td class="style4">
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserNameTextBox" ErrorMessage="Введите Логин"
                                ToolTip="Обязательное поле" ID="UserNameRequired">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style8">
                            <asp:Label runat="server" ID="PasswordLabel">Пароль:</asp:Label>
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" Text='<%# Bind("Password")%>'></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style8">
                            <asp:Label runat="server" AssociatedControlID="EmailTextBox" ID="EmailLabel2">E-mail:</asp:Label>
                        </td>
                        <td class="style5">
                            <asp:TextBox runat="server" ID="EmailTextBox" Text='<%# Bind("Email")%>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="EmailValidator1" runat="server" ErrorMessage="Некорректный e-mail"
                                ControlToValidate="EmailTextBox" ValidationExpression=".*@.{2,}\..{2,}">
                        *</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="BatteryGroup" runat="server">
                        <td align="right">
                            <asp:Label runat="server" AssociatedControlID="BatteryList" ID="BatteryLabel">Батарея тестов:</asp:Label>
                        </td>
                        <td class="style5">
                            <asp:DropDownList ID="BatteryList"  runat="server" DataSourceID="SqlDataSource2" DataValueField="BatteryID" DataTextField="BatteryName" AppendDataBoundItems="True" >
                                <asp:ListItem Value="0" Selected="True">Не указана...</asp:ListItem>
                            </asp:DropDownList>                        
                        </td>                      
                    </tr>
                    <tr id="CompanyGroup" runat="server">
                        <td align="right" class="auto-style5">
                            <asp:Label runat="server" AssociatedControlID="CompanyList" ID="CompanyLabel">Список компаний:</asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:DropDownList ID="CompanyList"  runat="server" DataSourceID="SqlDataSource3" DataValueField="Id" AppendDataBoundItems="True" DataTextField="CompanyName" >
                                <asp:ListItem Value="0">Компания не указана...</asp:ListItem>
                            </asp:DropDownList>                        
                        </td>                      
                    </tr>
                </table>
                <br />
                <asp:Label ID="ID" runat="server" Visible="False" Text='<%# Bind("ID")%>'></asp:Label>
                <div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Пользователь не зарегистрирован" />
                    <div id="ValidPass" runat="server" visible="false" style="color: Red;">
                    </div>
                </div>
                <div style="text-align: center;">
                    <asp:LinkButton ID="UpdateButton" runat="server" Text="Регистрация" CommandName="Insert" />
                    &nbsp;&nbsp;
                    <asp:HyperLink ID="CancelUpdateButton" NavigateUrl="~/Default.aspx" runat="server">Отмена</asp:HyperLink>
                </div>
            </InsertItemTemplate>
        </asp:FormView>
    </div>
</div>
<div runat="server" id="successRegistration" visible="false">
    Пользователь успешно зарегистрирован<br />    
</div>
