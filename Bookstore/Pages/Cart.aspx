<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" Inherits="Pages_Cart" Codebehind="Cart.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../MyStyles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .gridView {
            border-style: solid;
            border-width: 2px 2px 0px 2px;
        }

        .grid1stColumn {
            padding-left: 20px;
            padding-right: 20px;
        }

        .gridColumn {
            padding-right: 20px;
        }

        .plusMinus {
            margin: 0px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="CartHeaderLabel" runat="server" Text="Shopping Cart" Style="text-align: center" Width="1100px" Font-Bold="True" Font-Size="Large" BorderWidth="2px"></asp:Label>
    <br>
    <br>
    <asp:Label ID="ErrorLabel" runat="server" Text="Errors go here." Style="text-align: left; border-style: solid; border-color: black; border-width:2px 2px 2px 2px" Width="1100px" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
    
                <!--<asp:HyperLinkField HeaderText="Title:" DataNavigateUrlFields="TitleURL" DataTextField="Title">
                <ItemStyle HorizontalAlign="Left" Width="350px" CssClass="gridColumn"/>
            </asp:HyperLinkField>-->

    <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" BorderColor="Black" Font-Size="Large" CssClass="gridView" GridLines="Horizontal" OnRowCommand="GridView1_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Coverpage:" ItemStyle-Width="100px" HeaderStyle-CssClass="grid1stColumn" ItemStyle-CssClass="grid1stColumn">
                <ItemTemplate>
                    <a href='<%#Eval("ISBN","/Pages/Details.aspx?isbn={0}") %>'>
                        <img src='<%#Eval("ISBN","/Images/{0}.jpg") %>' width="100" />
                    </a>
                </ItemTemplate>

<HeaderStyle CssClass="grid1stColumn"></HeaderStyle>

                <ItemStyle Width="100px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText ="Title & Author:" ItemStyle-Width="350px" ItemStyle-CssClass="gridColumn" >
                <ItemTemplate>
                    <a href='<%#Eval("ISBN","/Pages/Details.aspx?isbn={0}") %>'>
                        <asp:Label ID="Title" runat="server" Text='<%# Bind("Title") %>' Font-Size="Large" Font-Bold="True" style="padding-bottom:10px"/>
                    </a>
                    <asp:Label ID="Author" style="display:block; padding-top:10px" runat="server" Text='<%# Bind("Author") %>'/>
                </ItemTemplate>                

<ItemStyle CssClass="gridColumn" Width="350px"></ItemStyle>
            </asp:TemplateField>
            <asp:BoundField DataField="Format" HeaderText="Format:">
                <ItemStyle HorizontalAlign="Left" Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Price" HeaderText="Price:">
                <ItemStyle HorizontalAlign="Left" Font-Size="Large" Width="100px" />
            </asp:BoundField>

            <asp:TemplateField HeaderText= "&nbsp;&nbsp;&nbsp;Quantity" ItemStyle-Width="140px">
                <ItemTemplate>
                    <asp:Button ID="DecrementBtn" runat="server" Font-Size="Large" Font-Bold="true" Text="-" CommandName="Decrement" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"/>
                    <asp:TextBox ID="QuantityTextBox" Width="40px" Font-Size="Large" runat="server" Text='<%# Bind("Quantity") %>' style="text-align:center" />
                    <asp:Button ID="IncrementBtn" runat="server" Font-Size="Large" Font-Bold="true" Text="+" CommandName="Increment" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"/>
                </ItemTemplate>

<ItemStyle Width="140px"></ItemStyle>
            </asp:TemplateField>

            <asp:BoundField DataField="Total" HeaderText="Total:">
                <ItemStyle HorizontalAlign="Left" Width="140px" />
            </asp:BoundField>


            <asp:ButtonField CommandName="DeleteRow" Text="Delete" ItemStyle-Width="100px"/>
        </Columns>
        <HeaderStyle HorizontalAlign="Left" />
    </asp:GridView>
   
    
    <asp:Panel ID="TotalsPanel" runat="server" Width="1100px" Style="display: inline-block; float: left; padding-top: 8px; padding-bottom: 8px; border-style: solid; border-width:2px 2px 0px 2px" BorderColor="Black">
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

