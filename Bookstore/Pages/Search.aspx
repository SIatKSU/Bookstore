<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Bookstore.Pages.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:GridView ID="GridView1" runat="server" CellPadding="3" BorderColor="Black" BorderWidth="2px" GridLines="Horizontal" AutoGenerateColumns="false" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:TemplateField HeaderText="Coverpage:" ItemStyle-Width="80px">
                <ItemTemplate>
                    <a href='<%#Eval("ISBN","/Pages/Details.aspx?isbn={0}") %>'>
                         <img src='<%#Eval("ISBN","/Images/{0}.jpg") %>' width="90" />
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField HeaderText ="Title" ItemStyle-Width="160px">
                <ItemTemplate>
                    <asp:HyperLink ID="Title"  runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("ISBN","/Pages/Details.aspx?isbn={0}") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Author" ItemStyle-Width ="100px" HeaderText="Author:"></asp:BoundField>
            <asp:BoundField DataField="ISBN" ItemStyle-Width="120px" HeaderText="ISBN:"></asp:BoundField>
            <asp:BoundField DataField="Description" ItemStyle-Width="310px" HeaderText="Description:" />
            <asp:BoundField DataField="Format" HtmlEncode="false" ItemStyle-Width ="70px" HeaderText="Format:"></asp:BoundField>
            <asp:BoundField DataField="New" HtmlEncode="false" ItemStyle-Width ="60px" HeaderText="New:"></asp:BoundField>
            <asp:BoundField DataField="Used" HtmlEncode="false" ItemStyle-Width ="60px" HeaderText="Used:"></asp:BoundField>
            <asp:BoundField DataField="Rental" HtmlEncode="false" ItemStyle-Width ="60px" HeaderText="Rental:"></asp:BoundField>
            <asp:BoundField DataField="eBook" HtmlEncode="false" ItemStyle-Width ="60px" HeaderText="eBook:"></asp:BoundField>

        </Columns>
        <HeaderStyle Width="500px" />

    </asp:GridView>


    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>



</asp:Content>
