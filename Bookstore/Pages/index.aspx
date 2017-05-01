<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" Inherits="Pages_index" Codebehind="index.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../MyStyles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style5 {
            height: 94px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center" >

    <asp:Label ID="IntroMessageLabel" runat="server" Text="Welcome to the Kennesaw State Bookstore. Start seaching by selecting from the options below." Font-Bold="True" Font-Size="Larger">
    </asp:Label>
    </asp:Panel>
    
    <asp:Panel ID="WholePanel" runat="server" Height="106px" Style="border-style: solid; border-width: 1px 1px 1px 1px; padding-bottom:0px; padding-left:350px; padding-right:0px; padding-top:0px" >
    <table>
        <tr style="width:1100px">
                <td class="auto-style5">
               
                    <asp:DropDownList ID="DDList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDList1SelectedIndexChanged" style="padding:5px"/>
                    <asp:DropDownList ID="DDList2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDList2SelectedIndexChanged" style="padding:5px" Visible="false"/>
                    <asp:TextBox ID="SearchBox" runat="server" style="padding:5px" Visible="True"/>
                    <asp:Button ID="SearchButton" runat="server" CssClass="btn" OnClick="searchClicked" Style="font-size: small;padding:5px" Text="Search" Visible="True"/>
                 
                    <asp:Label ID="CoursesError" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="False" ForeColor="Red" Height="23px" style="display:inline-block;" Text="Please select a course" Visible="False" Width="184px" />
                    <asp:Label ID="ProfsError" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="False" ForeColor="Red" style="display:inline-block;" Text="Please select a professor" Visible="False" />
                    <asp:Label ID="SearchBoxError" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="False" ForeColor="Red" style="display:inline-block;" Text="Please enter a search value" Visible="False" Width="235px" />
                </td>
            </tr>
    </table>
        
</asp:Panel>
    
    <p></p>
    <asp:Panel ID="Panel1" runat="server" Height="125px" HorizontalAlign="Center">
        <asp:Image runat="server" ImageURL="~/Images/book store 2.gif" Height="125px" Width="253px" ></asp:Image>
        </asp:Panel>
    
    </asp:Content>

