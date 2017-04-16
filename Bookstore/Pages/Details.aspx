<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="Bookstore.Pages.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
        <asp:Panel style="padding:25px" ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="700px" Width="100%">
            <asp:Panel ID="Panel2" runat="server" Height="100%" Width="100%">

                


                <asp:Panel ID="BookInfoPanel" runat="server" Height ="300px" Width="100%" style="float:left;padding-bottom:30px" >
                    <asp:Panel ID="BookImage" runat="server" Width="200px" Height="300px" style="float:left">
                        <asp:Image ID="CoverImage" runat="server" Height="300px" Width="200px"/>
                    </asp:Panel>

                     <asp:Panel ID="Panel3" runat="server" Width="700px" style="float:right;padding-top:25px">
                        <asp:Panel ID="TitlePanel" runat="server" Width="100%" style="padding-bottom:15px">
                            <asp:Label ID="TitleText" runat="server" Font-Bold="True" Font-Size="X-Large" Text="title goes here:"/>
                        </asp:Panel>

                         <asp:Panel ID="Panel4" runat="server" Width="100%" Height="50px" style="padding-bottom:30px">
                             <asp:Panel ID="FixedText" runat="server" Height="50px" Width="75px" style="float:left">
                                <asp:Label ID="ISBNLabel" runat="server" Text="ISBN:" Font-Size="Medium"/>
                                    <br>
                                <asp:Label ID="AuthorLabel" runat="server" Text="Author(s):" Font-Size="Medium" />
                            </asp:Panel>

                            <asp:Panel ID="TitleDetailsPanel" runat="server"  Height ="50px" Width="450px" style="float:left">
                                <asp:Label ID="ISBNText" runat="server" Text="isbn goes here:" Font-Size="Medium"/>
                                <br>
                                <asp:Label ID="AuthorText" runat="server" Text="author(s) goes here:" Font-Size="Medium" />
                            </asp:Panel>
                        </asp:Panel>
                        
                         <asp:Panel ID="Panel5" runat="server" Width="100%" >
                            <asp:Panel ID="CartPanel" runat="server" Height ="100px" Width="400px" style="padding:10px">
                                <asp:Panel ID="Panel6" runat="server" Height ="100%" Width="100%" >
                                    <asp:DropDownList ID="DDList1" runat="server" AutoPostBack="true" style="padding:12px" Width="180px" Font-Size="Large"/>
                                    <asp:Button ID="CartButton" runat="server" Text="Add to Cart" style="float:right;padding:12px" Width="170px" Font-Size="Large"/>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>



                <asp:Panel ID="BookDetailsPanel" runat="server" Width="100%" Height="350px">

                    <asp:Panel ID="Panel7" runat="server" Width="100%" Height="100px" style="float:left;padding-bottom:50px">
                        <asp:Panel ID="FixedText2" runat="server" Height="100px" Width="60px" style="float:left;padding-right:35px">
                            <asp:Label ID="BookDescriptionLabel" runat="server" Text="Description:" Font-Size="Medium" />
                        </asp:Panel>

                        <asp:Panel ID="BookDescriptionPanel" runat="server" Height ="100px" Width="900px" style="float:left">
                            <asp:Label ID="DescriptionText" runat="server" Text="book description goes here:" Font-Size="Medium"/>
                        </asp:Panel>
                    </asp:Panel>
                    
                   

                    <asp:Panel ID="CoursesPanel" runat="server" Width="695px">
                         <asp:GridView ID="GridView1" runat="server" CellPadding="3" GridLines="None" AutoGenerateColumns="false" style="float:right">
                             <Columns>
                                 <asp:BoundField DataField="Course" ItemStyle-Width ="200px" HeaderText="Course/Section:" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                 <asp:BoundField DataField="CRN" ItemStyle-Width ="100px" HeaderText="CRN:" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                 <asp:BoundField DataField="Professor" ItemStyle-Width ="175px" HeaderText="Professor:" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                 <asp:BoundField DataField="Requirement" ItemStyle-Width ="100px" HeaderText="Book is:" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                             </Columns>
                         </asp:GridView>
                    </asp:Panel>


                </asp:Panel>
                        


            </asp:Panel>
        </asp:Panel>

    
    
</asp:Content>
