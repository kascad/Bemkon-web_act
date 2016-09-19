<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArchiveMenu.ascx.cs" 
Inherits="ProfessorTesting.ArchiveMenu" %>

Архив:&nbsp;&nbsp;
<span style="background:url('../Images/Open_16x16.png') left no-repeat; padding-left:20px;">
<asp:HyperLink ID="OpenArchives" runat="server" NavigateUrl="~/User/Archives.aspx?mod=open" >
Открыть</asp:HyperLink></span>
&nbsp;&nbsp;

<span style="background:url('../Images/Archive_16x16.png') left no-repeat; padding-left:20px;">
<asp:HyperLink ID="NewArchives" runat="server" NavigateUrl="~/User/Archives.aspx?mod=new">
Новый</asp:HyperLink></span>
&nbsp;&nbsp;

<span style="background:url('../Images/Upload16_16.png') left no-repeat; padding-left:20px;">
<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/User/Archives.aspx?mod=upload">
Загрузить</asp:HyperLink></span>

<span style="background:url('../Images/Download16_16.png') left no-repeat; padding-left:20px;">
<asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/User/Archives.aspx?mod=download">
Выгрузить</asp:HyperLink></span>
