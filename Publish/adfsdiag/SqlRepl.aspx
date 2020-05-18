<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="SqlRepl.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Sql Replication Health</h2>
    <p>
        Provide the name of any SQL server in the farm :</p>
    <p>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </p>
    <p>
        Discovered Publisher Server :<asp:Label ID="Label1" runat="server"></asp:Label>
        <asp:Label ID="Label2" runat="server"></asp:Label>
    </p>
    <p>
        <asp:TextBox ID="TextBox2" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
        <asp:Button ID="Publications" runat="server" onclick="Publications_Click" 
            Text="Publications" />
        <asp:Button ID="SubscriptionWatchList" runat="server" 
            onclick="SubscriptionWatchList_Click" Text="Subscription Watch List" />
        <asp:Button ID="Agents" runat="server" onclick="Agents_Click" Text="Agents" />
    </p>
    <p>
        Publication status :</p>
    <p>
        <asp:GridView ID="GridView1" runat="server" OnRowDataBound = "OnRowDataBound">
        </asp:GridView>
    </p>
    <p>
        Subscription watch list on Publisher :<asp:Label ID="Label3" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="GridView2" runat="server" OnRowDataBound = "OnRowDataBound">
        </asp:GridView>
    </p>
    <p>
        Agent status on Publisher :<asp:Label ID="Label4" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="GridView3" runat="server" OnRowDataBound = "OnRowDataBound">
        </asp:GridView>
    </p>
    <p>
        View synchronization status on each Subscriber :<asp:Label ID="Label5" 
            runat="server"></asp:Label>
    </p>
    <p>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
        </asp:CheckBoxList>
    </p>
    <p>
        <asp:Button ID="GetSubscribers" runat="server" onclick="GetSubscribers_Click" 
            Text="Get Subscribers" />
        <asp:Button ID="ViewSyncStatus" runat="server" onclick="ViewSyncStatus_Click" 
            Text="View Synchronization Status" />
    </p>
    <p>
        <asp:GridView ID="GridView4" runat="server" OnRowDataBound = "OnRowDataBound">
        </asp:GridView>
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
</asp:Content>
