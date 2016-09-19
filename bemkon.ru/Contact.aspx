<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="ProfessorTesting.ContactPage" %>

<asp:Content ID="Content6" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div style="margin-left: 30px; margin-right: 30px">
            <center><h2>Контактные данные</h2></center>
            <center>Вы можете связаться с нами одним из следующих способов:</center><br />
&nbsp;<div>
                <table cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="contact_prompt">                            
                            <strong>Телефон:</strong></td>
                        <td class="td_text">+7-926-138-44-13</td>
                    </tr>
                    <tr>
                        <td class="contact_prompt">
                            <strong>Электронная почта:</strong></td>
                        <td class="td_text"><a href="mailto://ooo.bemkon@mail.ru?subject=Вопрос%20по%20тестированию&body=%20">ooo.bemkon@mail.ru</a></td>
                    </tr>
                    <tr>
                        <td class="contact_prompt">                            
                            <strong>Адрес:</strong></td>
                        <td class="td_text"><a href="https://maps.yandex.ru/?text=Москва%2C%20Шенкурский%20проезд%2C%20д.11">Москва, Шенкурский проезд, д.11</a></td>
                    </tr>
                </table>
            </div>
            <br />
 
      
        </div>
    </div>
</asp:Content>
