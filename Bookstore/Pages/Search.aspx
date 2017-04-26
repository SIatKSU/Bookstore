<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Bookstore.Pages.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../MyStyles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Label ID="SearchErrorLabel" runat="server" Text="Search Error" style="text-align:center;display:inline-block;padding-bottom:40px;padding-top:10px" Width="100%" Font-Bold="True" 
        Visible="false" ForeColor="#ff0000" Font-Size="Large" />

    <asp:Panel ID="SearchAgainPanel" runat="server" style="text-align:center" Visible="false">
        <asp:Button ID="SearchAgainButton" runat="server" CssClass="btn" Style="padding:7px" Text="Go Back To Search Page" OnClick="SearchAgainButton_Click"/>
    </asp:Panel>

    <asp:Label ID="SearchHeaderLabel" runat="server" Text="Search Results" style="text-align:center;display:inline-block;padding:5px" Width="1090px" Font-Bold="True" Font-Size="Large" BorderWidth="2px" />
    <br> <br>

    <asp:Panel ID="TitleDetailsPanel" runat="server" Width="1100px" style="float:left;text-align:center" BorderColor="Black" BorderWidth="2px">
        <asp:Label ID="ISBN" runat="server" Font-Size="Medium" Text="Sort By:" style="display:inline-block;padding:5px"/> &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="SortList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SortList_SelectedIndexChanged" style="padding:3px"/>
    </asp:Panel>
     
    <asp:GridView ID="GridView1" runat="server" CellPadding="3" BorderColor="Black" BorderWidth="2px" GridLines="Horizontal" AutoGenerateColumns="false" 
        PageIndex="10" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging" OnDataBound="GridView1_DataBound" ShowHeader="false" PagerStyle-HorizontalAlign="Center">
        
        <Columns>
            <asp:TemplateField HeaderText="Coverpage:" ItemStyle-Width="100px">
                <ItemTemplate>
                    <a href='<%#Eval("ISBN","/Pages/Details.aspx?isbn={0}") %>'>
                         <img src='<%#Eval("ISBN","/Images/{0}.jpg") %>' width="100" />
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField HeaderText ="Title" ItemStyle-Width="1000px" >
                <ItemTemplate>
                    <a href='<%#Eval("ISBN","/Pages/Details.aspx?isbn={0}") %>'>
                        <asp:Label ID="Title" runat="server" Text='<%# Bind("Title") %>' Font-Size="Large" Font-Bold="True" style="padding:2px"/>
                    </a>
                    <asp:Label ID="authorText" runat="server" Text="Author: " />
                    <asp:Label ID="Author" runat="server" Text='<%# Bind("Author") %>'/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="isbntext" runat="server" Text="ISBN: " />
                    <asp:Label ID="ISBN" runat="server" Text='<%# Bind("ISBN") %>' />
                    <br> 
                    <asp:Label ID="Description" runat="server" Text='<%# Bind("Description") %>' />
                    <br> 
                    <asp:Label ID="NewText" runat="server" Text="New: "  Font-Size="Smaller" Font-Bold="true"/>
                    <asp:Label ID="NewPrice" runat="server" Text='<%# Bind("New") %>' Font-Size="Smaller" Font-Bold="true"/>
                    <br> 
                    <asp:Label ID="UsedText" runat="server" Text="Used: " Font-Size="Smaller" Font-Bold="true"/>
                    <asp:Label ID="UsedPrice" runat="server" Text='<%# Bind("Used") %>' Font-Size="Smaller" Font-Bold="true"/>
                    <br> 
                    <asp:Label ID="RentalText" runat="server" Text="Rental: " Font-Size="Smaller" Font-Bold="true"/>
                    <asp:Label ID="RentalPrice" runat="server" Text='<%# Bind("Rental") %>' Font-Size="Smaller" Font-Bold="true"/>
                    <br> 
                    <asp:Label ID="eBookText" runat="server" Text="eBook: " Font-Size="Smaller" Font-Bold="true"/>
                    <asp:Label ID="eBookPrice" runat="server" Text='<%# Bind("eBook") %>' Font-Size="Smaller" Font-Bold="true"/>

                </ItemTemplate>
                
            </asp:TemplateField>

            <asp:BoundField DataField="Author" ItemStyle-Width ="100px" HeaderText="Author:" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="ISBN" ItemStyle-Width="125px" HeaderText="ISBN:" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="Description" ItemStyle-Width="305px" HeaderText="Description:" Visible="false"/>
            <asp:BoundField DataField="Format" HtmlEncode="false" ItemStyle-Width ="70px" HeaderText="Format:" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="New" HtmlEncode="false" ItemStyle-Width ="60px" HeaderText="New:" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="Used" HtmlEncode="false" ItemStyle-Width ="60px" HeaderText="Used:" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="Rental" HtmlEncode="false" ItemStyle-Width ="60px" HeaderText="Rental:" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="eBook" HtmlEncode="false" ItemStyle-Width ="60px" HeaderText="eBook:" Visible="false"></asp:BoundField>

        </Columns>
        
    </asp:GridView>
</asp:Content>
