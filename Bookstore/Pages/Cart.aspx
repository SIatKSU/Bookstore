<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" Inherits="Pages_Cart" Codebehind="Cart.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../MyStyles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="CartHeaderLabel" runat="server" Text="Shopping Cart" Style="text-align: center" Width="1100px" Font-Bold="True" Font-Size="Large" BorderWidth="2px"></asp:Label>
    <br>
    <br>
    <asp:Label ID="ErrorLabel" runat="server" Text="Errors go here." Style="text-align: left; border-style: solid; border-color: black; border-width:2px 2px 2px 2px" Width="1100px" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
    
    <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" GridLines="Horizontal">
        <Columns>
            <asp:TemplateField HeaderText="Coverpage:" ItemStyle-Width="100px">
                <ItemTemplate>
                    <a href='<%#Eval("ISBN","/Pages/Details.aspx?isbn={0}") %>'>
                        <img src='<%#Eval("ISBN","/Images/{0}.jpg") %>' width="100" />
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="TitleURL" DataTextField="Title">
                <ItemStyle HorizontalAlign="Left" Width="100px" />
            </asp:HyperLinkField>
            <asp:BoundField DataField="Format" HeaderText="Format">
                <ItemStyle HorizontalAlign="Left" Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                <ItemStyle HorizontalAlign="Left" Width="100px" />
            </asp:BoundField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left" />
    </asp:GridView>

    <!--
    <asp:GridView ID="GridView2" AutoGenerateColumns="false" runat="server" ShowHeader="False" GridLines="None">
        <Columns>
            <asp:BoundField DataField="BlankSpace" >
            <ItemStyle HorizontalAlign="Left" Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Label" HeaderText="Format" >
            <ItemStyle HorizontalAlign="Left" Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Amount" HeaderText="Quantity" >
            <ItemStyle HorizontalAlign="Left" Width="100px" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>  -->

    
    
    <asp:Panel ID="TotalsPanel" runat="server" Width="1100px" Style="display: inline-block; float: left; padding-top: 8px; padding-bottom: 8px" BorderColor="Black" BorderWidth="2px">
        <asp:Panel ID="Panel1" Width="200px" Style="display: inline-block; padding-bottom: 10px" runat="server">
            <asp:Label ID="Label1" runat="server" Style="display: block; text-align:right; padding-bottom: 10px" Text="Subtotal:"></asp:Label>
            <asp:Label ID="Label2" runat="server" Style="display: block; text-align:right; padding-bottom: 10px" Text="7% tax:"></asp:Label>
            <asp:Label ID="Label3" runat="server" Style="display: block; text-align:right; padding-bottom: 10px" Text="Shipping:"></asp:Label>
            <asp:Label ID="Label4" runat="server" Style="display: block; text-align:right; padding-bottom: 10px" Text="Total:"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="Panel2" Width="200px" Style="display: inline-block; padding-left: 40px; padding-bottom: 10px " runat="server">
            <asp:Label ID="SubtotalLabel" runat="server" Style="display: block; text-align:left; padding-bottom: 10px" Text=""></asp:Label>
            <asp:Label ID="TaxLabel" runat="server" Style="display: block; text-align:left; padding-bottom: 10px" Text=""></asp:Label>
            <asp:Label ID="ShippingLabel" runat="server" Style="display: block; text-align:left; padding-bottom: 10px" Text=""></asp:Label>
            <asp:Label ID="TotalLabel" runat="server" Style="display: block; text-align:left; padding-bottom: 10px" Text=""></asp:Label>
        </asp:Panel>
    </asp:Panel>
    
    <asp:Panel ID="BottomPanel" runat="server" Width="900px" Style="float: left; padding-top: 8px; padding-bottom: 8px; padding-left: 100px; padding-right: 100px; text-align: center" BorderColor="Black" BorderWidth="2px">
        <asp:Button ID="ContinueShoppingBtn" runat="server" Text="Continue Shopping" Style="display: inline-block; float: left; width: 300px" Font-Size="Large" CssClass="btn" OnClick="ContinueShoppingBtn_Click" />
        <!--<asp:Panel ID="DividerPanel" runat="server" Style="display: inline-block" Width="300px">
        </asp:Panel>-->
        <asp:Button ID="CheckoutBtn" runat="server" Text="Proceed to Checkout" Style="width: 300px; float: right" Font-Size="Large" CssClass="btn" OnClick="CheckoutButton_Click" />
    </asp:Panel>
</asp:Content>

