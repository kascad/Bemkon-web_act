<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExamineeMenu.ascx.cs" 
Inherits="ProfessorTesting.ExamineeMenu" %>

<span id="panelMenuUser" runat="server" visible="false">
<span style="margin-left:60px;">Обследуемые:</span>
<span style="background:url('../Images/User_16x16.png') left no-repeat; padding-left:20px;">
<asp:HyperLink ID="NewUser" runat="server" NavigateUrl="~/User/NewExaminee.aspx" 
>
Добавить
</asp:HyperLink></span>
&nbsp;&nbsp;
<span style="background:url('../Images/User_16x16_Delete1.png') left no-repeat; padding-left:20px;">
<asp:HyperLink ID="DeleteUser" runat="server" NavigateUrl="~/User/DeleteExaminee.aspx" >
Удалить
</asp:HyperLink></span>
</span>