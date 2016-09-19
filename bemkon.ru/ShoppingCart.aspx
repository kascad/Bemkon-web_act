<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="ProfessorTesting.ShoppingCartPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div style="margin-left:30px;margin-right:30px">
            <asp:Panel runat="server" ID="Completed" Visible="false">
                <center>
                    <span class="emphasis">Ваш заказ № <asp:Literal runat="server" ID="OrderNumber" Text="0"></asp:Literal> отправлен продавцу.</span><br />
                    Запишите номер вашего заказа, он потребуется вам для получения и оплаты.
                </center>
                <a href="javascript:void(0);" onclick="window.close();" class="button cart_button separator">Закрыть окно</a>
                <div class="separator"></div>
            </asp:Panel>
            <asp:Panel runat="server" ID="Processed">
                <div id="cart">
                    <center><h2>Корзина покупок</h2></center>
                    <asp:GridView runat="server" ID="Cart" DataSourceID="CartDS" AutoGenerateColumns="False" Width="100%" 
                        OnRowUpdated="Cart_RowUpdated" OnRowDeleted="Cart_RowDeleted" OnRowDataBound="Cart_RowDataBound" OnDataBound="Cart_DataBound" 
                        DataKeyNames="BookId,ReleaseFormatId" EmptyDataText="Корзина пуста" >
                        <Columns>
                            <asp:BoundField DataField="BookId" HeaderText="BookId" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="ReleaseFormatId" HeaderText="ReleaseFormatId" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="BookName" HeaderText="Книга" ReadOnly="True" />
                            <asp:BoundField DataField="Authors" HeaderText="Автор" ReadOnly="true" />
                            <asp:BoundField DataField="ReleaseFormatName" HeaderText="Формат выпуска" ReadOnly="True" />
                            <asp:BoundField DataField="Price" HeaderText="Цена" ReadOnly="True" />
                            <asp:BoundField DataField="Quantity" HeaderText="Количество" />
                            <asp:BoundField DataField="Cost" HeaderText="Сумма" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Управление">
                                <ItemTemplate>
                                    <asp:ImageButton ID="EditImageButton" runat="server"  ToolTip="Изменить количество" CommandName="Edit" ImageUrl="~/Images/edit.png" />
                                    <asp:ImageButton ID="DeleteImageButton" runat="server" ToolTip="Удалить" CommandName="Delete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Вы действительно хотите удалить эту запись?','Внимание');" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="UpdateImageButton" runat="server" ToolTip="Сохранить изменения" CommandName="Update" ImageUrl="~/Images/save.png" />
                                    <asp:ImageButton ID="CancelImageButton" runat="server" ToolTip="Отменить сделанные изменения" CommandName="Cancel" ImageUrl="~/Images/cancel.png" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div style="float:right">
                        Итого:&nbsp<span class="total"><asp:Literal runat="server" ID="TotalCart"></asp:Literal></span>
                        <div class="separator"></div>
                    </div>
                    <asp:HyperLink runat="server" ID="GoToOrder" Text="Оформить заказ" CssClass="button cart_button separator" NavigateUrl="javascript:void(0);"></asp:HyperLink>
                    <div class="separator"></div>
                </div>
                <div id="order" class="invisible">
                    <center><h2>Оформление заказа</h2></center>
                    <table class="layout_box" cellpadding=".5em">
                        <tbody>
                            <tr>
                                <td>Сумма заказа:</td>
                                <td><span id="orderSum" class="total"><asp:Literal runat="server" ID="OrderSum"></asp:Literal></span></td>
                            </tr>
                            <tr>
                                <td valign="top">Способ доставки:</td>
                                <td>
                                    <asp:RadioButton runat="server" ID="Pickup" GroupName="DeliveryGroup" Checked="true" 
                                        Text="У метро Алтуфьево -" /> <span class="emphasis">бесплатно</span><br />
                                    <asp:RadioButton runat="server" ID="BySubway" GroupName="DeliveryGroup" 
                                        Text="У станции метро" />
                                    <asp:DropDownList runat="server" ID="Station" DataSourceID="StationsDS" DataTextField="SubwayStationName" 
                                        DataValueField="SubwayStationName"></asp:DropDownList>
                                    -&nbsp;<span class="emphasis">150&nbsp;рублей</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Сумма к оплате:</td>
                                <td><span id="totalSum" class="total"><asp:Literal runat="server" ID="TotalSum"></asp:Literal></span></td>
                            </tr>
                            <tr>
                                <td valign="top">Способ оплаты:</td>
                                <td>
                                    <asp:RadioButton runat="server" ID="Cash" GroupName="PaymentGroup" 
                                        Text="Наличными при получении" Checked="true" /><br />
                                    <asp:RadioButton runat="server" ID="Card" GroupName="PaymentGroup" 
                                        Text="Перевод на карту Сбербанка № 6761 9600 0230 2315 74 (необходимо указать номер заказа)" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="separator cart_button button_bar">
                        <a href="javascript:void(0);" class="button" onclick="$('#order').hide();$('#cart').show();">Вернуться в корзину</a>
                        <asp:HyperLink runat="server" ID="GoToClient" NavigateUrl="javascript:void(0);" CssClass="button" Text="Продолжить оформление"></asp:HyperLink>
                    </div>
                    <div class="separator"></div>
                </div>
                <div id="client" class="invisible">
                    <center><h2>Ваши контакты</h2></center>
                    <table class="layout_box" cellpadding=".5em">
                        <tbody>
                            <tr>
                                <td>Ваше имя:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="ClientName"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="ClientNameRequired" CssClass="alert"
                                        ControlToValidate="ClientName" Text="Необходимо указать имя" EnableClientScript="true"
                                        Enabled="false">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Email:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="ClientMail"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="ClientMailRequired" CssClass="alert"
                                        ControlToValidate="ClientMail" Text="Необходимо указать email" EnableClientScript="true"
                                        Enabled="false">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="ClientMailValid" CssClass="alert"
                                        ControlToValidate="ClientMail" Text="Неверный формат адреса" EnableClientScript="true"
                                        ValidationExpression="^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,8}$" Enabled="false">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Контактный телефон:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="ClientPhone"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="ClientPhoneRequired" CssClass="alert"
                                        ControlToValidate="ClientPhone" Text="Необходимо указать контактный телефон" EnableClientScript="true"
                                        Enabled="false">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="ClientPhoneValid" CssClass="alert"
                                        ControlToValidate="ClientPhone" Text="Неверный формат номера телефона" EnableClientScript="true"
                                        ValidationExpression="^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$" Enabled="false">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="cart_button button_bar">
                        <asp:HyperLink runat="server" ID="BackToOrder" NavigateUrl="javascript:void(0);" CssClass="button" Text="Вернуться к заказу"></asp:HyperLink>
                        <asp:LinkButton runat="server" ID="Complete" Text="Завершить оформление" CssClass="button" OnClick="Complete_Click"></asp:LinkButton>
                    </div>
                    <div class="separator"></div>
                </div>
            </asp:Panel>
        </div>
    </div>

    <asp:SqlDataSource runat="server" ID="CartDS" ConnectionString='<%$ ConnectionStrings: MyConnectionString %>'
        SelectCommand="select c.*, b.BookName, b.Authors, rf.ReleaseFormatName, brf.Price, Cost = c.Quantity * brf.Price
	from dbo.Cart c
		inner join dbo.Books b on c.BookId = b.BookId
		inner join dbo.ReleaseFormats rf on c.ReleaseFormatId = rf.ReleaseFormatId
		inner join dbo.BooksReleaseFormats brf on brf.BookId = c.BookId and brf.ReleaseFormatId = c.ReleaseFormatId
	where c.SessionId = @sessionId
	order by b.BookName, rf.ReleaseFormatName"
        UpdateCommand="update dbo.Cart set Quantity = @quantity where SessionId = @sessionId and BookId = @bookId and ReleaseFormatId = @releaseFormatId"
        DeleteCommand="delete from dbo.Cart where SessionId = @sessionId and BookId = @bookId and ReleaseFormatId = @releaseFormatId">
        <SelectParameters>
            <asp:SessionParameter Name="sessionId" SessionField="sessionId" />
        </SelectParameters>
        <UpdateParameters>
            <asp:SessionParameter Name="sessionId" SessionField="sessionId" />
            <asp:ControlParameter Name="bookId" ControlID="Cart" PropertyName="SelectedDataKey[0]" />
            <asp:ControlParameter Name="releaseFormatId" ControlID="Cart" PropertyName="SelectedDataKey[1]" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:SessionParameter Name="sessionId" SessionField="sessionId" />
            <asp:ControlParameter Name="bookId" ControlID="Cart" PropertyName="SelectedDataKey[0]" />
            <asp:ControlParameter Name="releaseFormatId" ControlID="Cart" PropertyName="SelectedDataKey[1]" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="StationsDS" ConnectionString='<%$ ConnectionStrings:MyConnectionString %>'
        SelectCommand="select SubwayStationName from dbo.SubwayStations where SubwayStationId != 3 order by SubwayStationName">
    </asp:SqlDataSource>
</asp:Content>
