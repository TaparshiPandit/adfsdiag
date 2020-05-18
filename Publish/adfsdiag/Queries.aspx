<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Queries.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Query Activity ID</h2>
    <p>
        Provide the name of an ADFS server :
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </p>
    <p>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="GetAdfs" runat="server" onclick="GetAdfs_Click" 
            Text="Get ADFS" />
        <asp:TextBox ID="TextBox2" runat="server" Width="300px"></asp:TextBox>
        <asp:Button ID="Query" runat="server" onclick="Query_Click" Text="Query" />
    </p>
    <p>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
        </asp:CheckBoxList>
    </p>
    <p>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
</asp:Content>
