<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArchivePanel.ascx.cs" 
Inherits="ProfessorTesting.ArchivePanel" %>

<div>
<div style="padding-bottom:20px;">
    <asp:Label ID="LabelTitle" runat="server" 
Text="Архивы" CssClass="header_block" ></asp:Label>
</div>

<div runat="server" id="panelCreate" visible="false" style="width: 369px">
    <div>
        <asp:Label ID="LabelNameNew" runat="server">Имя архива</asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBoxNameNew" runat="server" Width="220px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ErrorMessage="Введите имя архива" ControlToValidate="TextBoxNameNew">*</asp:RequiredFieldValidator>
    </div>
    <div>
    
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
            HeaderText="Архив не создан" />
    
    </div>
    <div style="text-align:center;width:100%;padding-top:20px; padding-bottom:20px;">
        <asp:LinkButton ID="LinkButtonNew" runat="server">
        Создать</asp:LinkButton>&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="LinkButtonCancelNew" NavigateUrl="~/User/" runat="server">
        Отмена</asp:HyperLink>
    </div>
</div>

<div runat="server" id="panelOpen" visible="false">
    <div>
        <asp:GridView ID="dataGridView" runat="server" CellPadding="5"
        AutoGenerateColumns="False" BackColor="Transparent" 
        BorderColor="Transparent" HeaderStyle-BackColor="#4891C6" 
        HeaderStyle-BorderColor="White"
        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="header_table" 
        HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px"
        RowStyle-BorderColor="Gray" GridLines="Horizontal" Width="100%">
        <RowStyle BorderColor="Gray" CssClass="row_item" />
        <Columns>
            <asp:BoundField HeaderText="Название архива" DataField="FileName">
            </asp:BoundField>
            <asp:ButtonField CommandName="Select" Text="Выбрать" HeaderText="Выбор"  />
        </Columns>
        
        <SelectedRowStyle BackColor="#F0EBBE" />

        <HeaderStyle BackColor="#4891C6" ForeColor="White" BorderColor="White" 
            BorderStyle="Solid" BorderWidth="1px" CssClass="header_table" 
            Font-Bold="False" />
            <EmptyDataTemplate>Архивов нет</EmptyDataTemplate>
        </asp:GridView>
    </div>
    
    <div style="width:100%; text-align:center;padding-top: 20px; padding-bottom: 20px;">
        <asp:LinkButton runat="server" id="LinkButtonOpen">Открыть</asp:LinkButton>&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HyperLinkCancel" NavigateUrl="~/User/" runat="server">
        Отмена</asp:HyperLink>
    </div>
</div>

<div runat="server" id="panelDownload" visible="false" style="width: 407px">

    <div>
    <asp:Label ID="LabelNameUp" runat="server">Файл для загрузки:</asp:Label>
    &nbsp;&nbsp;&nbsp;
    <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="FileUpload1" ErrorMessage="Выберите файл для загрузки">*</asp:RequiredFieldValidator>
    </div>
    <div>
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" 
            HeaderText="Архив не загружен" />
    </div>
    <div style="text-align:center;width:100%;padding-top:20px; padding-bottom:20px;">
        <asp:LinkButton ID="LinkButtonUp" runat="server">
        Загрузить</asp:LinkButton>&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="LinkButtonCancelUp" NavigateUrl="~/User/" runat="server">
        Отмена</asp:HyperLink>
    </div>

</div>

<div runat="server" id="panelUpload" visible="false" style="width:50%;">
    <div>
        <asp:GridView ID="GridViewDownload" runat="server" CellPadding="5"
        AutoGenerateColumns="False" BackColor="Transparent" 
        BorderColor="Transparent" HeaderStyle-BackColor="#4891C6" 
        HeaderStyle-BorderColor="White"
        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="header_table" 
        HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px"
        RowStyle-BorderColor="Gray" GridLines="Horizontal" Width="100%"
        OnRowCommand="Download_RowCommand">
        <RowStyle BorderColor="Gray" CssClass="row_item" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="link"
                        CommandArgument='<%# Eval("FileName") %>' 
                        CommandName="ArchiveDownload">
                        <%# Eval("FileName") %></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:HyperLinkField HeaderText="Название архива" DataNavigateUrlFields="Id,FileName" 
            DataNavigateUrlFormatString="/Archives/{0}/{1}"
            DataTextField="FileName" DataTextFormatString="{0}" />
                --%>
            
            <%--<asp:BoundField HeaderText="Название архива" DataField="FileName">
            </asp:BoundField>--%>
        </Columns>
        
        <RowStyle CssClass="download_row" />
        
        <HeaderStyle BackColor="#4891C6" ForeColor="White" BorderColor="White" 
            BorderStyle="Solid" BorderWidth="1px" CssClass="header_table" 
            Font-Bold="False" />
            <EmptyDataTemplate>Архивов нет</EmptyDataTemplate>
        </asp:GridView>
    </div>
    
    <%--<div style="font-size:70%;margin-top:10px;">
    Если вы используете браузеры IE или Opera, то для выгрузки архива нажмите правой кнопкой мыши на архиве 
    и выберите пункт меню Сохранить объект как...
    </div>--%>
</div>


</div>
