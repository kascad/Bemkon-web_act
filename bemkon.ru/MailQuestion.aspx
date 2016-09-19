<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="MailQuestion.aspx.cs" Inherits="ProfessorTesting.MailQuestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div style="margin-left:30px;margin-right:30px">
            <center><h2>Обращение с сайта bemkon.ru</h2></center>
            <table width="100%">
                <tbody>
                    <tr>
                        <td style="width:10em">Email для ответа:<span class="alert">*</span></td>
                        <td>
                            <asp:TextBox runat="server" ID="Sender" Width="20em"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="SenderRequire" 
                                Text="Нужно указать адрес для ответа" CssClass="alert" ControlToValidate="Sender">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="SenderValidator" 
                                Text="Неверный формат адреса" CssClass="alert" ControlToValidate="Sender"
                                ValidationExpression="^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,8}$">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">Сообщение:<span class="alert">*</span></td>
                        <td>
                            <asp:TextBox runat="server" ID="Message" TextMode="MultiLine" Rows="10" Wrap="true" Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="MessageRequire" Text="Нельзя отправить пустое сообщение" CssClass="alert" ControlToValidate="Message"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:LinkButton runat="server" ID="SendButton" CssClass="button question_button" Text="Отправить" OnClick="SendButton_Click"></asp:LinkButton>
            <div class="separator"><span class="warning"><asp:Literal runat="server" ID="Sent" Text="Ваше сообщение отправлено" Visible="false"></asp:Literal></span></div>
        </div>
    </div>
</asp:Content>
