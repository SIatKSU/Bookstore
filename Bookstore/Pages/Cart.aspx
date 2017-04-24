<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" Inherits="Pages_Cart" Codebehind="Cart.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../MyStyles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="CartHeaderLabel" runat="server" Text="Shopping Cart" Style="text-align: center" Width="1100px" Font-Bold="True" Font-Size="Large" BorderWidth="2px"></asp:Label>
    <br>
    <br>
    <asp:Label ID="ErrorLabel" runat="server" Text="Errors go here." Style="text-align: left; border-style: solid; border-color: black; border-width:2px 2px 0px 2px" Width="1100px" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>

    <asp:Panel ID="BottomPanel" runat="server" Width="900px" Style="float: left; padding-top: 8px; padding-bottom: 8px; padding-left: 100px; padding-right: 100px; text-align: center" BorderColor="Black" BorderWidth="2px">
        <asp:Button ID="ContinueShoppingBtn" runat="server" Text="Continue Shopping" Style="display: inline-block; float: left; width: 300px" Font-Size="Large" CssClass="btn" OnClick="ContinueShoppingBtn_Click" />
        <!--<asp:Panel ID="DividerPanel" runat="server" Style="display: inline-block" Width="300px">
        </asp:Panel>-->
        <asp:Button ID="CheckoutBtn" runat="server" Text="Proceed to Checkout" Style="width: 300px; float: right" Font-Size="Large" CssClass="btn" OnClick="CheckoutButton_Click" />
    </asp:Panel>
</asp:Content>

