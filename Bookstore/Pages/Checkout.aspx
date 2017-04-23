<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="Bookstore.Pages.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--<script src="../JavaScript.js"></script>-->
    <link href="../MyStyles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .addressPanel {
            display: inline-block;
            border-style: none;
        }

        .addressFieldLabel {
            display: inline-block;
            width: 60px;
            margin-bottom: 5px;
        }

        .nameFieldLabel {
            display: inline-block;
            width: 80px;
            margin-bottom: 5px;
        }

        .paymentFieldLabel {
            display: inline-block;
            margin-bottom: 5px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel0" runat="server" Style="border-style: solid; border-spacing: 2px" Height="100%">
        <asp:Panel ID="Panel1" runat="server" Height="60%">
            <asp:Panel ID="Panel3" runat="server" Style="border-style: solid; border-width: 0px 0px 2px 0px" Width="100%" Height="25px">
                <asp:Label ID="CheckoutHeaderLabel" runat="server" Text="Checkout" Style="text-align: center;border-width: 0px" Font-Bold="True" Font-Size="Large" Width="100%"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="Panel4" runat="server" Width="100%" Height="55%">
                <asp:Panel ID="BillingAddressPanel" Style="table-layout: fixed; box-sizing: border-box; border-spacing: 0px; border-style: solid; border-width: 0px 2px 0px 0px" runat="server" Width="50%" Height="100%" CssClass="addressPanel">
                    <asp:Panel ID="Panel5" runat="server" Width="100%" Height="25px" BorderStyle="None">
                        <asp:Label ID="Label1" runat="server" Text="Billing Address" Style="text-align: center" Font-Bold="True" Width="100%"></asp:Label>
                    </asp:Panel>
                    <asp:Panel ID="Panel7" runat="server" Width="100%" Height="100%" Style="padding-bottom: 10px; border-style: none">
                        &nbsp;<asp:Label ID="BillingStreetLabel" CssClass="addressFieldLabel" runat="server" Text="Street:"></asp:Label>
                        <asp:TextBox ID="BillingStreetTextBox" runat="server" Width="80%"></asp:TextBox>
                        <br />
                        &nbsp;<asp:Label ID="BillingCityLabel" CssClass="addressFieldLabel" runat="server" Text="City:"></asp:Label>
                        <asp:TextBox ID="BillingCityTextBox" runat="server" Width="80%"></asp:TextBox>
                        <br />
                        &nbsp;<asp:Label ID="Label3" CssClass="addressFieldLabel" runat="server" Text="State:"></asp:Label>
                        <asp:DropDownList ID="BillingStateDropDown" runat="server" AutoPostBack="false">
                            <asp:ListItem Value="NONE">Select State or Territory</asp:ListItem>
                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
                            <asp:ListItem Value="AL">Alabama</asp:ListItem>
                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                            <asp:ListItem Value="CA">California</asp:ListItem>
                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                            <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                            <asp:ListItem Value="DE">Delaware</asp:ListItem>
                            <asp:ListItem Value="FL">Florida</asp:ListItem>
                            <asp:ListItem Value="GA">Georgia</asp:ListItem>
                            <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                            <asp:ListItem Value="IA">Iowa</asp:ListItem>
                            <asp:ListItem Value="ID">Idaho</asp:ListItem>
                            <asp:ListItem Value="IL">Illinois</asp:ListItem>
                            <asp:ListItem Value="IN">Indiana</asp:ListItem>
                            <asp:ListItem Value="KS">Kansas</asp:ListItem>
                            <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                            <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                            <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                            <asp:ListItem Value="MD">Maryland</asp:ListItem>
                            <asp:ListItem Value="ME">Maine</asp:ListItem>
                            <asp:ListItem Value="MI">Michigan</asp:ListItem>
                            <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                            <asp:ListItem Value="MO">Missouri</asp:ListItem>
                            <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                            <asp:ListItem Value="MT">Montana</asp:ListItem>
                            <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                            <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                            <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                            <asp:ListItem Value="NV">Nevada</asp:ListItem>
                            <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                            <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                            <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                            <asp:ListItem Value="NY">New York</asp:ListItem>
                            <asp:ListItem Value="OH">Ohio</asp:ListItem>
                            <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                            <asp:ListItem Value="OR">Oregon</asp:ListItem>
                            <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                            <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                            <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                            <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                            <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                            <asp:ListItem Value="TX">Texas</asp:ListItem>
                            <asp:ListItem Value="UT">Utah</asp:ListItem>
                            <asp:ListItem Value="VA">Virginia</asp:ListItem>
                            <asp:ListItem Value="VT">Vermont</asp:ListItem>
                            <asp:ListItem Value="WA">Washington</asp:ListItem>
                            <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                            <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                            <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                            <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                            <asp:ListItem Value="GU">Guam</asp:ListItem>
                            <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                            <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                            <asp:ListItem Value="VI">U.S. Virgin Islands</asp:ListItem>
                            <asp:ListItem Value="UM">U.S. Minor Outlying Islands</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        &nbsp;<asp:Label ID="Label4" CssClass="addressFieldLabel" runat="server" Text="Zip:"></asp:Label>
                        <asp:TextBox ID="BillingZipTextBox" runat="server" Style="margin-bottom: 10px" Width="20%"></asp:TextBox>
                        <br />
                        <!--&nbsp;<asp:Label ID="AddressCheckboxLabel" Text="Check if Shipping Address Same as Billing:" Style="display:inline-block;margin-bottom:5px" runat="server"></asp:Label>-->
                        &nbsp;<asp:CheckBox ID="AddressCheckBox" runat="server" Style="display:inline-block" Text="Shipping Address Same as Billing Address:" TextAlign="Left" AutoPostBack="True" OnCheckedChanged="AddressCheckBox_CheckedChanged"/>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="ShippingAddressPanel" runat="server" Width="49%" Height="100%" CssClass="addressPanel" Style="vertical-align:top">
                    <asp:Panel ID="Panel6" runat="server" Width="100%" Height="25px" BorderStyle="None">
                        <asp:Label ID="Label2" runat="server" Text="Shipping Address" Style="text-align: center" Font-Bold="True" Width="100%"></asp:Label>
                    </asp:Panel>
                    <asp:Panel ID="Panel8" runat="server" Width="100%" Height="100%" BorderStyle="None" Style="padding-bottom: 10px">
                        &nbsp;<asp:Label ID="Label5" CssClass="addressFieldLabel" runat="server" Text="Street:"></asp:Label>
                        <asp:TextBox ID="ShippingStreetTextBox" runat="server" Width="80%"></asp:TextBox>
                        <br />
                        &nbsp;<asp:Label ID="Label6" CssClass="addressFieldLabel" runat="server" Text="City:"></asp:Label>
                        <asp:TextBox ID="ShippingCityTextBox" runat="server" Width="80%"></asp:TextBox>
                        <br />
                        &nbsp;<asp:Label ID="Label7" CssClass="addressFieldLabel" runat="server" Text="State:"></asp:Label>
                        <asp:DropDownList ID="ShippingStateDropDown" runat="server" AutoPostBack="false">
                            <asp:ListItem Value="NONE">Select State or Territory</asp:ListItem>
                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
                            <asp:ListItem Value="AL">Alabama</asp:ListItem>
                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                            <asp:ListItem Value="CA">California</asp:ListItem>
                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                            <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                            <asp:ListItem Value="DE">Delaware</asp:ListItem>
                            <asp:ListItem Value="FL">Florida</asp:ListItem>
                            <asp:ListItem Value="GA">Georgia</asp:ListItem>
                            <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                            <asp:ListItem Value="IA">Iowa</asp:ListItem>
                            <asp:ListItem Value="ID">Idaho</asp:ListItem>
                            <asp:ListItem Value="IL">Illinois</asp:ListItem>
                            <asp:ListItem Value="IN">Indiana</asp:ListItem>
                            <asp:ListItem Value="KS">Kansas</asp:ListItem>
                            <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                            <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                            <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                            <asp:ListItem Value="MD">Maryland</asp:ListItem>
                            <asp:ListItem Value="ME">Maine</asp:ListItem>
                            <asp:ListItem Value="MI">Michigan</asp:ListItem>
                            <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                            <asp:ListItem Value="MO">Missouri</asp:ListItem>
                            <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                            <asp:ListItem Value="MT">Montana</asp:ListItem>
                            <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                            <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                            <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                            <asp:ListItem Value="NV">Nevada</asp:ListItem>
                            <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                            <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                            <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                            <asp:ListItem Value="NY">New York</asp:ListItem>
                            <asp:ListItem Value="OH">Ohio</asp:ListItem>
                            <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                            <asp:ListItem Value="OR">Oregon</asp:ListItem>
                            <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                            <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                            <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                            <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                            <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                            <asp:ListItem Value="TX">Texas</asp:ListItem>
                            <asp:ListItem Value="UT">Utah</asp:ListItem>
                            <asp:ListItem Value="VA">Virginia</asp:ListItem>
                            <asp:ListItem Value="VT">Vermont</asp:ListItem>
                            <asp:ListItem Value="WA">Washington</asp:ListItem>
                            <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                            <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                            <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                            <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                            <asp:ListItem Value="GU">Guam</asp:ListItem>
                            <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                            <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                            <asp:ListItem Value="VI">U.S. Virgin Islands</asp:ListItem>
                            <asp:ListItem Value="UM">U.S. Minor Outlying Islands</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        &nbsp;<asp:Label ID="Label8" CssClass="addressFieldLabel" runat="server" Text="Zip:"></asp:Label>
                        <asp:TextBox ID="ShippingZipTextBox" runat="server" Width="20%"></asp:TextBox>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="NameEmailPhonePanel" runat="server" Height="45%" Style="padding-top: 10px; margin-top: 0px; padding-bottom: 10px; border-style: solid; border-width: 2px 0px 0px 0px">
                &nbsp;<asp:Label ID="FullNameLabel" runat="server" CssClass="nameFieldLabel" Text="Full Name:"></asp:Label>
                <asp:TextBox ID="FullNameTextBox" runat="server" Width="80%"></asp:TextBox>
                <br />
                &nbsp;<asp:Label ID="BillingCityLabel0" runat="server" CssClass="nameFieldLabel" Text="Email:"></asp:Label>
                <asp:TextBox ID="EmailTextBox" runat="server" Width="80%"></asp:TextBox>
                <br />
                &nbsp;<asp:Label ID="Label9" runat="server" CssClass="nameFieldLabel" Text="Phone Number:" Width="110px"></asp:Label>
                <asp:TextBox ID="PhoneTextBox" runat="server" Width="20%"></asp:TextBox>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Height="217px">
            <asp:Panel ID="Panel9" runat="server" Height="25px" Width="100%" Style="border-style: solid; border-width: 2px 0px 2px 0px">
                <asp:Label ID="PaymentHeaderLabel" runat="server" Font-Bold="True" Style="text-align: center" Text="Payment" Width="100%"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="Panel10" runat="server" Height="50%" Style="padding-top: 15px; padding-bottom: 10px">
                &nbsp;<asp:Label ID="Label10" CssClass="paymentFieldLabel" Style="width: 180px" runat="server" Text="Select Payment Method:"></asp:Label>
                <asp:DropDownList ID="PaymentMethodDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PaymentMethodDropDown_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                &nbsp;<asp:Label ID="paymentLabel1" CssClass="paymentFieldLabel" Style="width: 120px" runat="server" Text="Card Number:"></asp:Label>
                <asp:TextBox ID="CardNumberTextBox" runat="server" onkeydown = "return (!((event.keyCode>=65 && event.keyCode <= 95) || event.keyCode >= 106) && event.keyCode!=32);" MaxLength="16" Columns="18"></asp:TextBox>
                <asp:TextBox ID="PayPalEmailTextBox" runat="server" Visible="False" Width="30%"></asp:TextBox>
                <asp:TextBox ID="KSULoginTextBox" runat="server" Visible="False" Width="12%"></asp:TextBox>
                <br />
                &nbsp;<asp:Label ID="paymentLabel2" CssClass="paymentFieldLabel" Style="width: 120px" runat="server" Text="Expiration Date:"></asp:Label>
                <asp:DropDownList ID="ExpMonthDropDown" runat="server" AutoPostBack="false">
                </asp:DropDownList>
                <asp:DropDownList ID="ExpYearDropDown" runat="server" AutoPostBack="false">
                </asp:DropDownList>
                <asp:TextBox ID="PayPalPasswordTextBox" runat="server" Visible="False" Width="20%" TextMode="Password"></asp:TextBox>
                <asp:TextBox ID="KSUPasswordTextBox" runat="server" Visible="False" Width="20%" TextMode="Password"></asp:TextBox>
                <br />
                &nbsp;<asp:Label ID="paymentLabel3" CssClass="paymentFieldLabel" Style="width: 120px" runat="server" Text="Security Code:"></asp:Label>
                <asp:TextBox ID="SecurityCodeTextBox" runat="server" onkeydown = "return (!((event.keyCode>=65 && event.keyCode <= 95) || event.keyCode >= 106) && event.keyCode!=32);" Columns="4" MaxLength="3" ></asp:TextBox>
                <br />
            </asp:Panel>
            <asp:Panel ID="Panel11" runat="server" Height="40%" Width="100%" Style="display:inline-block; overflow: hidden; border-style: solid; border-width: 2px 0px 0px 0px">
                <asp:Label ID="ErrorLabel" runat="server" visible="false" Font-Bold="True" Style="float: left; font-size:large; margin-left:30px; margin-top:11px" ForeColor="Red" Text="ErrorMessageGoesHere"></asp:Label>
                <asp:Button ID="PlaceOrderButton" runat="server" Text="Place Order" Style="font-size: large; float: right; margin-right: 50px; margin-top: 7px" CssClass="btn" OnClick="PlaceOrderButton_Click" />
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
