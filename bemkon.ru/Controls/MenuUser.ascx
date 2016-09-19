<%@ Control Language="C#" AutoEventWireup="true" 
Inherits="Controls_MenuUser" Codebehind="MenuUser.ascx.cs" %>

    <div class="menu_item">
        <asp:HyperLink NavigateUrl="~/User/" ID="HyperLinkExaminees" runat="server">
        Обследуемые
        </asp:HyperLink>
    </div>
    <div runat="server" id="TestGroup">
        <div class="menu_item">
            <asp:HyperLink NavigateUrl="~/User/BatteryTests.aspx" ID="HyperLinkBatteryTests" runat="server">
            Батареи тестов
            </asp:HyperLink>
        </div>
        <div class="menu_item">
            <asp:HyperLink NavigateUrl="~/User/Tests.aspx" ID="HyperLinkMenuTests" runat="server">
            Отдельные тесты
            </asp:HyperLink>
        </div>
        <div class="menu_item">
            <asp:HyperLink NavigateUrl="~/User/TopTests.aspx" ID="HyperLinkMenuTopTests" runat="server">
            Топ 10 тестов
            </asp:HyperLink>
        </div>
    </div>
    <div class="menu_item">
        <asp:HyperLink NavigateUrl="~/User/Interprets.aspx" ID="HyperLinkMenuInterprets" runat="server">
        Интерпретация
        </asp:HyperLink>
    </div>
    <!--
        <div class="menu_item">
            <asp:HyperLink NavigateUrl="~/User/Customize.aspx" ID="AdminCustomize" runat="server">
            Настройки
            </asp:HyperLink>
        </div>
    !-->
    <div>
        <asp:HyperLink ID="Exit" NavigateUrl="~/LogOut.aspx" runat="server">Выход</asp:HyperLink>
    </div>
