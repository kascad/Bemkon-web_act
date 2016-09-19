<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Book.aspx.cs" Inherits="ProfessorTesting.BookPage" %>

<asp:Content ID="Content6" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main noselect" onmousedown="return false;" onclick="return true;" oncopy="return false;">
        <div style="margin-left:25px;margin-right:25px">
            <center><h2>Дорогие гости!</h2></center>
            <center><h2>Вашему вниманию предлагаются последние работы основателей системы Бэмкон.
                <span class="highlighted">С некоторыми из них вы можете ознакомиться совершенно бесплатно!</span></h2></center><br />
            <center><h2>Dear guests!</h2></center>
            <center><h2>We offer last works of founders of the unique system Bemkon. 
                <span class="highlighted">Some works are available absolutely free!</span></h2></center><br />
            <asp:Repeater runat="server" ID="Books" OnItemDataBound="Books_ItemDataBound">
                <HeaderTemplate>
                    <table cellspacing="0" cellpadding="0"><tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="td_img"><asp:Image runat="server" ID="BookImage" CssClass="book_img" ImageUrl="~/Images/noimage.png" /></td>
                        <td class="td_text">
                            <span class="book_name"><asp:Literal runat="server" ID="BookName" Text='<%# Eval("BookName") %>'></asp:Literal>.</span>
                            <span class="book_authors"><asp:Literal runat="server" ID="BookAuthors" Text='<%# Eval("Authors") %>'></asp:Literal>.</span>
                            <div class="book_description"><asp:Literal runat="server" ID="Description" Text='<%# Eval("Description") %>'></asp:Literal></div>
                            <asp:Panel runat="server" ID="PublisherPanel">
                                <asp:Literal runat="server" ID="PublisherPrompt" Text='<%# Eval("PublisherPrompt") %>'>
                                </asp:Literal>:&nbsp;<asp:Literal runat="server" ID="Publisher" Text='<%# Eval("Publisher") %>'>
                                </asp:Literal>,&nbsp;<asp:Literal runat="server" ID="Year" Text='<%# Eval("Year") %>'>
                                </asp:Literal>&nbsp;<asp:Literal runat="server" ID="YearPrompt" Text='<%# Eval("YearPrompt") %>'></asp:Literal>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="FormatPanel">
                                <asp:Literal runat="server" ID="FormatPrompt" Text='<%# Eval("FormatPrompt") %>'>
                                </asp:Literal>:&nbsp;<asp:Literal runat="server" ID="Format" Text='<%# Eval("Format") %>'></asp:Literal>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="ISBNPanel">
                                ISBN:&nbsp;<asp:Literal runat="server" ID="ISBN" Text='<%# Eval("ISBN") %>'></asp:Literal>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="PagesPanel">
                                <asp:Literal runat="server" ID="Pages" Text='<%# Eval("Pages") %>'>
                                </asp:Literal>&nbsp;<asp:Literal runat="server" ID="PagePrompt" Text='<%# Eval("PagePrompt") %>'></asp:Literal>
                            </asp:Panel>
                            <asp:HyperLink runat="server" ID="Fragment" CssClass="link_button" Text="Фрагмент из работы >>" Target="_blank"></asp:HyperLink>
                            <asp:Panel runat="server" ID="ForDownloadPanel">
                                <asp:HyperLink runat="server" ID="ForDownLoad" CssClass="link_button" Text="Загрузить фрагмент >>" Target="_blank"></asp:HyperLink>
                            </asp:Panel>
                            <div class="book_comment"><asp:Literal runat="server" ID="Comment"></asp:Literal></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <span class="sale_label"><asp:Literal runat="server" ID="ForSale" Text="Купить:"></asp:Literal></span>
                            <asp:Image runat="server" ID="InCart" AlternateText="Книга уже в корзине" ImageUrl="~/Images/button_ok.png" />
                        </td>
                        <td>
                            <asp:Repeater runat="server" ID="Formats" OnItemDataBound="Formats_ItemDataBound">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="BuyButton" CssClass="button buy_button" OnCommand="BuyButton_Command" CommandName="Buy" CommandArgument='<%# Eval("BookId").ToString() + ";" + Eval("ReleaseFormatId").ToString() %>' >
                                        <asp:Literal runat="server" ID="Format" Text='<%# Eval("ReleaseFormatName") %>'>
                                            </asp:Literal>&nbsp;<span class="book_price"><asp:Literal runat="server" ID="Price" Text='<%# Eval("Price") %>'>
                                            </asp:Literal>&nbsp;<asp:Literal runat="server" ID="Currency" Text='<%# Eval("Currency") %>'></asp:Literal></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr style="border-top:1px solid gray"><td>&nbsp;<br /><br />&nbsp;</td><td>&nbsp;</td></tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody></table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:SqlDataSource runat="server" ID="BooksDS" ConnectionString='<%$ ConnectionStrings: MyConnectionString %>'
                SelectCommand="select b.BookId, b.BookName, b.Authors, b.Description, b.PublisherPrompt, b.Publisher, b.Year, b.YearPrompt,
                    b.FormatPrompt, b.Format, b.ISBN, b.Pages, b.PagePrompt, b.BookImageFile, b.Comment, b.Fragment, b.PDFForDownload,
                    InCart = cast(case when exists(select 1 from dbo.Cart c where c.SessionId = @sessionId and c.BookId = b.BookId) then 1 else 0 end as bit)
                    from dbo.Books b order by b.SortOrder, b.BookName">
                <SelectParameters>
                    <asp:SessionParameter Name="sessionId" SessionField="sessionId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <div style="text-align:center">
                <a href="/MailQuestion.aspx" target="_blank"><span class="highlighted_link large_text">Будем рады ответить на Ваши вопросы<br />
                    We will be happy to answer any questions you may have on our books</span></a>
            </div>
        </div>
    </div>
</asp:Content>
