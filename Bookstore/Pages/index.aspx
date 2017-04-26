<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" Inherits="Pages_index" Codebehind="index.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../MyStyles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="IntroMessageLabel" runat="server" Text="Welcome to the Kennesaw State Bookstore. Start seaching by selecting the options below." Font-Bold="True" Font-Size="Larger">
    </asp:Label>
    <p></p>
    <table>
        <tr style="width:1100px">
                <td colspan="4" class="auto-style4">

                    <asp:DropDownList ID="DDList1" runat="server" AutoPostBack="true" style="padding:5px" OnSelectedIndexChanged="DDList1SelectedIndexChanged" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                    <asp:DropDownList ID="DDList2" runat="server" AutoPostBack="true" style="padding:5px" Visible="false" OnSelectedIndexChanged="DDList2SelectedIndexChanged" />

                    <asp:TextBox ID="SearchBox" runat="server" Visible="True" style="padding:5px" />

                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="SearchButton" runat="server" CssClass="btn" Style="font-size: small;padding:5px" Text="Search" Visible="True" OnClick="searchClicked"/>

                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="ProfsError" runat="server" style="display:inline-block;padding-bottom:10px" Text="Please select a professor" Visible="False" Font-Bold="True" 
                        Font-Size="Medium" Font-Underline="False" ForeColor="Red" />
 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    <asp:Label ID="CoursesError" runat="server" style="display:inline-block;padding-bottom:10px" Text="Please select a course" Visible="False" Font-Bold="True" 
                        Font-Size="Medium" Font-Underline="False" ForeColor="Red" />

                    <asp:Label ID="SearchBoxError" runat="server" style="display:inline-block;padding-bottom:10px" Text="Please enter a search value" Visible="False" Font-Bold="True"
                        Font-Size="Medium" Font-Underline="False" ForeColor="Red" />
                </td>
            </tr>
    </table>

<p>
    More welcoming things here. A picture of the bookstore perhaps. </p>
</asp:Content>

