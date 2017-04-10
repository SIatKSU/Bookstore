<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Bookstore.Pages.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:GridView ID="GridView1" runat="server" BorderColor="Black" BorderWidth="2px" AutoGenerateColumns="false" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" >
        <Columns>
            
            <asp:TemplateField HeaderText="Image:">
                <ItemTemplate>
                    <img src='<%#Eval("ISBN","/Images/{0}.jpg") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            
           
            <asp:TemplateField HeaderText ="Titlee">
                <ItemTemplate>
                    <asp:HyperLink ID="Titlee" runat="server" Text='<%#Eval("Titlee") %>' NavigateUrl='<%#Eval("ISBN","/Pages/Details.aspx?isbn={0}") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Author" HeaderText="Author:"></asp:BoundField>
            <asp:BoundField DataField="ISBN" HeaderText="ISBN:"></asp:BoundField>


        </Columns>
        <HeaderStyle Width="500px" />

    </asp:GridView>


    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>



</asp:Content>
