<%@ Page Language="C#" AutoEventWireup="true" Inherits="login" MasterPageFile="~/Main.master"  Codebehind="Login.aspx.cs" %>
<%@ Register TagPrefix="uc" TagName="MenuLogin" Src="~/Controls/MenuLogin.ascx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Head" Runat="Server"></asp:Content>

<asp:Content ID="ContentHorMenuModule" ContentPlaceHolderID="HorMenuModule" Runat="Server">
    <asp:HyperLink ID="idcustom" runat="server" style="font-weight:bold;" NavigateUrl="~/">Главная</asp:HyperLink>&nbsp;|&nbsp;
    <asp:HyperLink ID="idarticles" runat="server" style="font-weight:bold;" NavigateUrl="~/Book.aspx">Научные работы и статьи</asp:HyperLink>&nbsp;|&nbsp;
    <asp:HyperLink ID="miOurTeam" runat="server" style="font-weight:bold;" NavigateUrl="~/OurTeam.aspx">Наша команда</asp:HyperLink>&nbsp;|&nbsp;
    <asp:HyperLink ID="idcontacts" runat="server" style="font-weight:bold;" NavigateUrl="~/Contact.aspx">Наши контакты</asp:HyperLink>
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HedearLeftMenu">
    Авторизация
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentLeftMenu">
    <uc:MenuLogin id="BlockMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="MainContent">
<div style="background-color: #DAE9F3">
    <div style="margin-left: 30px; margin-right: 30px">
Успех деятельности любой компании напрямую зависит от людей, которые в ней работают. Любому руководителю хочется создать команду, где <strong>каждый</strong> стремится к реализации поставленных целей и <strong>делает все</strong> для их достижения. Где <strong>работа каждого сотрудника</strong> вносит неоценимый вклад в совместную деятельность и <strong>каждый</strong> готов много и честно зарабатывать как для себя, так и для развития своей фирмы. Но как же построить такую команду? Как не ошибиться и провести грамотный отбор нужного специалиста? И как потом удержать его в команде? Для решения этих вопросов мы предлагаем надежный, удобный в использовании, а главное, эффективный инструмент - систему тестирования персонала Бэмкон.

<h2>Система Бэмкон позволяет руководителю:</h2>
<ul>
<li>проводить грамотный отбор специалистов с учетом требуемых компетенций и личностных качеств, проводить сравнение кандидатов по единым критериям</li>
<li>проводить оценку профессионально-важных и личностных качеств сотрудников, оценивать соответствие этих качеств конкретной должности в организации</li>
<li>принимать решения о продвижении сотрудников</li>
<li>анализировать психологический климат в коллективе и отслеживать его в динамике</li>
<li>получать оперативные рекомендации ведущих специалистов по работе с коллективом</li>
<li>проводить анализ нематериальной мотивации сотрудников, получать рекомендации по стимулированию труда</li>
</ul>
<h2>Преимущества системы Бэмкон:</h2>
<ul>
<li>надежность: основана на ведущих зарекомендованных методиках оценки персонала</li>
<li>простота в использовании: для ее запуска требуется только наличие Интернета. Интерфейс интуитивно понятен</li>
<li>гибкость: в соответствии с целью опроса сотруднику/кандидату назначаются тесты, нацеленные на изучение конкретных личностных качеств и требуемых компетенций. Возможность контролировать время проведения тестирования</li>
<li>ясность и конкретность: в результате проведенных обследований Вы будете получать однозначные и понятные рекомендации</li>
<li>оперативная техническая поддержка</li>
<li>бесплатное обновление</li>
</ul>
<h2 style="text-align: center">Вы сможете без привлечения кадровых агентств, без значительных временных и материальных затрат сформировать сплоченную команду профессионалов, способную решать задачи именно Вашего предприятия оперативно и эффективно!</h2>
        </div>
</div>
</asp:Content>