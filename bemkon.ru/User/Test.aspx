<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="ProfessorTesting.User.Test"
    MasterPageFile="~/Main.master" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc" TagName="MenuUser" Src="~/Controls/MenuUser.ascx" %>
<%@ Register TagPrefix="uc" TagName="ArchiveMenu" Src="~/Controls/ArchiveMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="ExamineeMenu" Src="~/Controls/ExamineeMenu.ascx" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="Head" runat="Server">

    <script type="text/javascript">
        var hour_common = 0;
        var minute_common = 0;
        var second_common = 0;

        function SetMinuteCommon() {
            minute_common++;
            if (minute_common >= 60) {
                hour_common++;
                minute_common = 0;
            }
        }

        function SetTimeCommon() {
            second_common++;
            if (second_common >= 60) {
                SetMinuteCommon();
                second_common = 0;
            }
        }

        function WriteTimeCommon() {
            SetTimeCommon();
            var label = document.getElementById('<%=labelFullTime.ClientID%>');
            if (label != null) {
                label.value = "Общее время: " + GetNum(hour_common) + ':' + GetNum(minute_common)
        + ':' + GetNum(second_common);
            }
            label = document.getElementById('<%=hiddenFullTime.ClientID%>');
            if (label != null) {
                label.value = GetNum(hour_common) + ':' + GetNum(minute_common)
        + ':' + GetNum(second_common);
            }
            //    label = document.getElementById('fullTime');
            //    if (label != null) {
            //        label.value = GetNum(hour_common) + ':' + GetNum(minute_common) 
            //        + ':' + GetNum(second_common);
            //    }
        }

        var hour_one = 0;
        var minute_one = 0;
        var second_one = 0;

        function SetMinuteOne() {
            minute_one++;
            if (minute_one >= 60) {
                hour_one++;
                minute_one = 0;
            }
        }

        function SetTimeOne() {
            second_one++;
            if (second_one >= 60) {
                SetMinuteOne();
                second_one = 0;
            }
        }

        function WriteTimeOne() {
            SetTimeOne();
            var label = document.getElementById('<%=hiddenOneTime.ClientID%>');
            if (label != null) {
                label.value = GetNum(hour_one) + ':' + GetNum(minute_one)
        + ':' + GetNum(second_one);
            }
        }

        function InitCommonTimer() {
            //alert("ddd");
            var hidden = document.getElementById('<%=hiddenCurHour.ClientID%>');
            if (hidden != null) {
                hour_common = hidden.value;
            }
            hidden = document.getElementById('<%=hiddenCurMin.ClientID%>');
            if (hidden != null) {
                minute_common = hidden.value;
            }
            hidden = document.getElementById('<%=hiddenCurSec.ClientID%>');
            if (hidden != null) {
                second_common = hidden.value;
            }
        }
        function InitOneTimer() {
            var hidden = document.getElementById('<%=hiddenOneCurHour.ClientID%>');
            if (hidden != null) {
                hour_one = hidden.value;
            }
            hidden = document.getElementById('<%=hiddenOneCurMin.ClientID%>');
            if (hidden != null) {
                minute_one = hidden.value;
            }
            hidden = document.getElementById('<%=hiddenOneCurSec.ClientID%>');
            if (hidden != null) {
                second_one = hidden.value;
            }
        }

        var commonTimer = null;
        var oneTimer = null;
        var butPrev;

        window.onload = function() {
            InitCommonTimer();
            commonTimer = window.setInterval("WriteTimeCommon();", 1000);
            oneTimer = window.setInterval("WriteTimeOne();", 1000);

            var button = document.getElementById('<%=buttonPrev.ClientID%>');
            if (button != null)
                butPrev = button.disabled;

            window.scrollBy(200, 200);
        }

        function GetNum(num) {
            if (num > 9) return num;
            else return '0' + num;
        }

        function StopTimer() {
            window.clearInterval(commonTimer);
            window.clearInterval(oneTimer);
        }

        function Lock(pref_id, disabled) {
            var els = document.getElementsByTagName('input');
            for (var i = 0; i < els.length; i++) {
                if (els[i].type == 'radio' && els[i].id.indexOf(pref_id) != -1) {
                    els[i].disabled = disabled;
                }
            }
        }
        function Pause() {
            var btStart = document.getElementById('ImageButtonStart');
            if (btStart == null)
                return;
            if (btStart.style.visibility != "visible") {
                StopTimer();
                //var btPause = document.getElementById('ImageButtonPause');
                if (commonTimer != null && oneTimer != null) {
                    //btPause.style.visibility = "hidden";
                    btStart.style.visibility = "visible";
                }
                var bt = document.getElementById('<%=labelPause.ClientID%>');
                if (bt != null) {
                    bt.style.visibility = "visible";
                }
                var button = document.getElementById('<%=buttonPrev.ClientID%>');
                if (button != null)
                    button.disabled = "disabled";
                button = document.getElementById('<%=buttonNext.ClientID%>');
                if (button != null)
                    button.disabled = "disabled";

                var sav = false;
                var bt = document.getElementById('<%=hiddenSaveQuest.ClientID%>');
                if (bt != null) {
                    sav = bt.value == "1";
                }

                if (!sav) {
                    Lock("RadioButtonListQuest", "disabled");
                    Lock("radio_graph", "disabled");
                }
            }
            else {
                btStart.style.visibility = "hidden";
                //btPause.style.visibility = "visible";
                commonTimer = window.setInterval("WriteTimeCommon();", 1000);
                oneTimer = window.setInterval("WriteTimeOne();", 1000);
                var bt = document.getElementById('<%=labelPause.ClientID%>');
                if (bt != null) {
                    bt.style.visibility = "hidden";
                }
                var button = document.getElementById('<%=buttonPrev.ClientID%>');
                if (button != null)
                    button.disabled = butPrev;
                button = document.getElementById('<%=buttonNext.ClientID%>');
                if (button != null)
                    button.disabled = "";

                var sav = false;
                var bt = document.getElementById('<%=hiddenSaveQuest.ClientID%>');
                if (bt != null) {
                    sav = bt.value == "1";
                }

                if (!sav) {
                    Lock("RadioButtonListQuest", "");
                    Lock("radio_graph", "");
                }
            }
        }

        function add_time_tolink(link) {
            if (link != null) {
                StopTimer();
                var t = GetNum(hour_common) + GetNum(minute_common) + GetNum(second_common);
                link.href += "&timefull=" + t;
            }
        }

</script>

</asp:Content>
<asp:Content ID="ContentHorMenu" ContentPlaceHolderID="HorMenu" runat="Server">
    <uc:ArchiveMenu ID="ArchiveMenu1" runat="server" />
    <uc:ExamineeMenu ID="ExamineeMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentHorMenuModule2" ContentPlaceHolderID="HorMenuModule2" runat="Server">
    <asp:Label ID="labelArchName" runat="server">Нет открытого архива</asp:Label>
</asp:Content>
<asp:Content ID="ContentHorMenuModule3" ContentPlaceHolderID="HorMenuModule3" runat="Server">
    <asp:Label ID="labelExamName" runat="server">Обследуемый не выбран</asp:Label>
</asp:Content>
<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" Style="font-weight: bold;" NavigateUrl="~/User/">
    Обследуемые</asp:HyperLink>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="HedearLeftMenu" runat="Server">
    Меню Пользователя
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentLeftMenu" runat="Server">
    <uc:MenuUser ID="BlockMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:HiddenField runat="server" ID="hiddenQuestId" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenCurHour" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenCurMin" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenCurSec" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenFullTime" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenOneTime" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenOneCurHour" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenOneCurMin" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenOneCurSec" Value="0" />
    <asp:HiddenField runat="server" ID="hiddenSaveQuest" Value="0" />
    <%--Заголовок страницы--%>
    <asp:Label ID="LabelTitle" runat="server" Visible="false" Text="Тест" CssClass="header_block">
    </asp:Label>
    <%--Сообщение об окончании теста--%>
    <div runat="server" id="messPanel" visible="false" style="padding-bottom: 20px;">
        <asp:Label ID="LabelMess" runat="server" Text=""></asp:Label>
        <div runat="server" id="panelBattery" visible="false" style="padding-bottom: 20px;
            padding-top: 20px;">
            <asp:HyperLink runat="server" ID="ButtonNextTest">Перейти к следующему тесту батареи</asp:HyperLink>
        </div>
    </div>
    <%--Панель с тестами--%>
    <div runat="server" id="testPanel">
        <%--Шапка--%>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 45px;
            color: White; background: #548045 url(../Images/fon3.png) repeat-x; height: 45px;
            color: White;">
            <tr>
                <td valign="middle">
                    <div runat="server" id="labelTestName" style="padding-left: 20px; font-size: 18px;
                        font-weight: bold; vertical-align: middle;">
                        Тест</div>
                </td>
                <td valign="bottom" style="padding-bottom: 3px;">
                    <span runat="server" id="labelAllQuest" style="padding-left: 30px;">Всего вопросов:
                        0</span>
                </td>
                <td valign="bottom" style="padding-bottom: 3px;">
                    <span runat="server" id="labelQuestAnswered" style="padding-left: 30px;">Пройдено: 0</span>
                </td>
                <td valign="bottom" style="padding-bottom: 3px;">
                    <asp:TextBox runat="server" ID="labelFullTime" ReadOnly="true" CssClass="time_textbox"
                        Width="240">Общее время: 00:00:00</asp:TextBox>
                </td>
                <td align="right" valign="middle">
                    <span runat="server" id="labelPause" style="font-size: 18px; font-weight: bold; color: Yellow;
                        padding-right: 30px; visibility: hidden;">Пауза</span>
                </td>
            </tr>
        </table>
        <%--Основная часть--%>

        <script type="text/javascript">

       
    
        </script>

        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top" style="height: 50px;">
                <td rowspan="3" valign="top" style="background: #D8E1F2 url(../Images/fon2.png) no-repeat bottom;
                    width: 220px;">
                    <asp:Panel ScrollBars="Vertical" runat="server" ID="leftTestSidebar2" Height="500px">
                        <div runat="server" id="container" style="padding: 20px;">
                        </div>
                    </asp:Panel>
                </td>
                <td valign="top" colspan="3">
                    <div style="float: left; margin-left: 5px;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/fon4.png" />
                    </div>
                    <div runat="server" id="labelQuestText" style="border: #000 0px solid; height: 54px;
                        display: table-cell; vertical-align: middle; color: #008000; font-size: 16px;
                        font-weight: bold;">
                        Вопрос
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top">
                    <asp:Panel Style="padding: 20px;" runat="server" ID="panelQuest" Height="350px" ScrollBars="Vertical">
                        <div runat="server" id="containerQuestText" visible="false">
                            <asp:RadioButtonList ID="RadioButtonListQuest" AutoPostBack="true" runat="server"
                                DataTextField="AnsText" DataValueField="AnsID" OnSelectedIndexChanged="RadioButtonListQuest_SelectedIndexChanged">
                            </asp:RadioButtonList>
                        </div>
                        <div runat="server" id="containerQuestGraph" visible="false">

                            <script type="text/javascript">
                                function onCheckradiopButton(el) {
                                    var inps = document.getElementsByTagName('input')
                                    for (var i = 0; i < inps.length; i++) {
                                        if (inps[i].id.indexOf('_radio_graph') != -1) {
                                            inps[i].checked = "";
                                        }
                                    }
                                    el.checked = "checked";
                                }
                            </script>

                            <asp:Repeater runat="server" ID="RepeaterGraph">
                                <ItemTemplate>
                                    <table style="float: left">
                                        <tr>
                                            <td valign="middle">
                                                <asp:RadioButton runat="server" ID="radio_graph" onclick="onCheckradiopButton(this)"
                                                    OnCheckedChanged="radio_graph_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <label runat="server" id="label_radio_graph">
                                                    <asp:Image Style="cursor: pointer" runat="server" ID="radio_graphimage" ImageUrl='<%# Eval("AnsText", "~/Images/images/{0}") %>'
                                                        Height="161" Width="112" /></label>
                                                <asp:HiddenField runat="server" ID="graph_id" Value='<%# Eval("AnsID") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
            <tr style="height: 50px;">
                <td style="width: 50px;" valign="bottom">
                    <div id="ImageButtonPause" style="cursor: pointer; background: url(/Images/Pause_48x48.png) no-repeat;
                        width: 48px; height: 48px;" onclick="Pause();">
                        <div id="ImageButtonStart" style="cursor: pointer; visibility: hidden; background: url(/Images/Play_48x48.png) no-repeat;
                            width: 48px; height: 48px;">
                        </div>
                    </div>
                </td>
                <td align="right" valign="bottom">
                    <asp:ImageButton ID="buttonPrev" runat="server" Height="48px" ImageUrl="~/Images/Previous_48x48.png"
                        Width="48px" OnClick="buttonPrev_Click" />
                    <asp:ImageButton ID="buttonNext" runat="server" Height="48px" ImageUrl="~/Images/Next_48x48.png"
                        Width="48px" OnClick="buttonNext_Click" />
                </td>
            </tr>
        </table>
    </div>
    
    <asp:Image runat="server" ID="imgTest"  />
    
</asp:Content>
