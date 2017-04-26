<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeBehind="Receipt.aspx.cs" Inherits="Bookstore.Pages.Receipt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <!--<script src="../JavaScript.js"></script>-->
    <link href="../MyStyles.css" rel="stylesheet" type="text/css" />     
    <style type="text/css">
        .addressPanel {
            display: inline-block;
            border-style: none;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="wholepanel" runat="server" Height="105%">
        <asp:Panel ID="BSPPanels" runat="server" Height="29%">
            <asp:Panel ID="BillingLabelPanel" runat="server" Style="border-width: 2px 2px 2px 2px" Width="100%" Height="25px" BorderStyle="Solid" BorderColor="Black">
                <asp:Label ID="BillingAddressLabel" runat="server" Text="Billing Address:" Style="text-align: left;border-width: 0px" Font-Bold="True" Font-Size="Large" Width="100%"></asp:Label>
            </asp:Panel>
              <asp:Panel ID="BillingAddressPanel" runat="server" Width="100%" Height="55%">           
                  <asp:Panel ID="FirstHalf" runat="server" CssClass="addressPanel" Height="100%" Style="table-layout: auto; box-sizing: border-box; border-spacing: 0px; border-width: 0px 0px 0px 0px" Width="50%">
                    <asp:Label ID="FullNameLabelB" runat="server" Text="Default Name" Width="100%" Font-Size="Medium"></asp:Label>
                  <asp:Label ID="addressLabelB" runat="server" Text="default address" Width="100%" Font-Size="Medium"></asp:Label>
                      
                          
                          <asp:Label ID="citystateBLabel" runat="server" Text="defualt b citystatezip" Width="100%" Font-Size="Medium"></asp:Label>
                      
                  </asp:Panel>
                <asp:Panel ID="SecondHalf" runat="server" Width="49%" Height="100%" CssClass="addressPanel" Style="vertical-align:top">                    
                    <asp:Panel ID="Panel8" runat="server" Width="100%" Height="100%" BorderStyle="None" Style="padding-bottom: 10px">
                          <asp:Label ID="PhoneNumberLabelB"  runat="server" Text="Default p number" Width="100%" Font-Size="Medium" ></asp:Label>                       
                        <asp:Label ID="EmailAdressLabelB" runat="server" Text="defualt email address" Width="100%" Font-Size="Medium"></asp:Label>                       
                        
                    </asp:Panel>
                </asp:Panel>
                 
           </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" Style="border-width: 2px 2px 2px 2px" Width="100%" Height="25px" BorderStyle="Solid" BorderColor="Black">
                <asp:Label ID="ShippingAddressLabel" runat="server" Text="Shipping Address:" Style="text-align: left;border-width: 0px" Font-Bold="True" Font-Size="Large" Width="100%"></asp:Label>
            </asp:Panel>
              <asp:Panel ID="Panel2" runat="server" Width="100%" Height="55%">           
                  <asp:Panel ID="Panel3" runat="server" CssClass="addressPanel" Height="55px" Style="table-layout: auto; box-sizing: border-box; border-spacing: 0px; border-width: 0px 0px 0px 0px" Width="50%">
                    <asp:Label ID="FullNameLabel2" runat="server" Text="Default Name" Width="100%" Font-Size="Medium"></asp:Label>
                  <asp:Label ID="AddressLabel3" runat="server" Text="default address" Width="100%" Font-Size="Medium"></asp:Label>
                      
                          
                          <asp:Label ID="citystateLabel4" runat="server" Text="defualt b ctiy state zip" Width="100%" Font-Size="Medium"></asp:Label>
                      
                  </asp:Panel>
                <asp:Panel ID="Panel4" runat="server" Width="49%" Height="55px" CssClass="addressPanel" Style="vertical-align:top">                    
                    <asp:Panel ID="Panel5" runat="server" Width="100%" Height="100%" BorderStyle="None" Style="padding-bottom: 10px">
                          <asp:Label ID="PhoneNumberLabel5"  runat="server" Text="Default p number" Width="100%" Font-Size="Medium" ></asp:Label>                       
                        <asp:Label ID="EmailLabel6" runat="server" Text="defualt email address" Width="100%" Font-Size="Medium"></asp:Label>                       
                        
                    </asp:Panel>
                </asp:Panel>
                 
           </asp:Panel>
               <asp:Panel ID="Panel6" runat="server" Style="border-width: 2px 2px 2px 2px" Width="100%" Height="25px" BorderStyle="Solid" BorderColor="Black">
                <asp:Label ID="PaymentLabel1" runat="server" Text="Payment Method:" Style="text-align: left;border-width: 0px" Font-Bold="True" Font-Size="Large" Width="100%"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="Panel7" runat="server" Width="100%">           
                    <asp:Label ID="PaymentMethodLabel1" runat="server" Text="Default Payment Method" Width="100%" Font-Size="Medium"></asp:Label>
                </asp:Panel>
       <asp:Panel ID="ItemsPanel" runat="server" Width="100%" Height="145%">
                       <asp:Panel ID="Panel9" runat="server" Style="border-width: 2px 2px 2px 2px" Width="100%" Height="25px" BorderStyle="Solid" BorderColor="Black">

                         <asp:GridView ID="GridView1" runat="server" CellPadding="3" GridLines="None" AutoGenerateColumns="false" style="float:left;padding-top:50px">
                             <Columns>
                                 <asp:BoundField DataField="ItemName" ItemStyle-Width ="200px" HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                 <asp:BoundField DataField="ItemAtuhor" ItemStyle-Width ="200px" HeaderText="Author" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                 <asp:BoundField DataField="ItemISBN" ItemStyle-Width ="200px" HeaderText="ISBN" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                 <asp:BoundField DataField="ItemType" ItemStyle-Width ="200px" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                 <asp:BoundField DataField="ItemQuantity" ItemStyle-Width ="200px" HeaderText="Quantity" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                 <asp:BoundField DataField="ItemUnitPrice" ItemStyle-Width ="100px" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                             </Columns>
                         </asp:GridView>
                          </asp:Panel>
                           </asp:Panel>
                    
        <asp:Panel ID="TotalsPanel" runat="server" Style="border-width: 2px 0px 0px 0px" Width="100%" Height="100%" BorderStyle="Solid" BorderColor="Black">
        
        <asp:Panel ID="Panel10" Width="100%" Style="display: inline-block; padding-bottom: 10px" runat="server"> 
            <asp:Label ID="SubTotalLabel1" runat="server" Style="display: inline-block; text-align: right;" Text="Subtotal: $" Width="90%" ></asp:Label>
            <asp:Label ID="ActualSubtotalLabel" runat="server" float="right" Text="0.00" ></asp:Label>

            <asp:Label ID="taxLabel2" runat="server" Style="display: inline-block; text-align:right;" Text="7% tax: $" Width="90%"></asp:Label>  
            <asp:Label ID="ActualTaxLabel" runat="server" float="right" Text="0.00"></asp:Label>
              
            <asp:Label ID="ShippingLabel" runat="server" Style="display: inline-block; text-align:right;" Text="Shipping: $" Width="90%"></asp:Label>
            <asp:Label ID="ActualShippingLabel" runat="server" float="right" Text="0.00"></asp:Label>
          
        </asp:Panel>
         
            <asp:Panel ID="Panel12" runat="server" BorderStyle="Solid" Height="25px" Style="border-width: 2px 0px 0px 0px" Width="100%">
<asp:Label ID="totalLabel4" runat="server" Style="display: inline-block; text-align:right; " Text="Total: $" Width="90%"></asp:Label>
                <asp:Label ID="ActualTotalLabel" runat="server" float="right" Text="0.00"></asp:Label>
                
            </asp:Panel>
            <asp:Panel ID="Panel11" runat="server" Style="display: inline-block; padding-left: 40px; padding-bottom: 10px " Width="100%">
            </asp:Panel>
    </asp:Panel>
         
           </asp:Panel>
        </asp:Panel>
</asp:Content>
