<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="AdfsDiag.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Active Directory Federation Service Diagnostics</h2>
    <p>
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </p>
    <p>
        <asp:TextBox ID="TextBox1" runat="server" ToolTip="ADFS Server Name"></asp:TextBox>
        <asp:Button ID="GetADFS" runat="server" onclick="GetADFS_Click" 
            Text="Get ADFS" />
        <asp:Button ID="MetadataCheck" runat="server" onclick="MetadataCheck_Click" 
            Text="Metadata Check" />
        <asp:Button ID="GetService" runat="server" onclick="GetService_Click" 
            Text="Get Service" />
        <asp:Button ID="PortChecks" runat="server" onclick="PortChecks_Click" 
            Text="Port Checks" />
        <asp:Button ID="CertCheck" runat="server" onclick="CertCheck_Click" 
            Text="Cert Check" />
    </p>
    <p>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
        </asp:CheckBoxList>
    </p>
    <p>
        ADFS Metadata check report :</p>
    <p>
        <asp:GridView ID="GridView1" runat="server" OnRowDataBound = "OnRowDataBoundMetaCheck">
        </asp:GridView>
    </p>
    <p>
        ADFS and SQL service status report :</p>
    <p>
        <asp:GridView ID="GridView2" runat="server" OnRowDataBound = "OnServiceStatus">
        </asp:GridView>
    </p>
    <p>
        ADFS port check report :</p>
    <p>
        <asp:GridView ID="GridView3" runat="server" OnRowDataBound = "OnRowDataBoundPort">
        </asp:GridView>
    </p>
    <p>
        ADFS certificate check report :<asp:Label ID="Label2" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="GridView4" runat="server" OnRowDataBound = "OnRowDataBoundCertCheck">
        </asp:GridView>
    </p>
    <p>
        ADFS Token report : Provide the name of an ADFS Server :
        <asp:Label ID="Label3" runat="server"></asp:Label>
    </p>
    <p>
        <asp:TextBox ID="TextBox2" runat="server" ToolTip="ADFS Server Name"></asp:TextBox>
        <asp:Button ID="GetADFSForToken" runat="server" onclick="GetADFSForToken_Click" 
            Text="Get ADFS" />
        <asp:DropDownList ID="DropDownList1" runat="server" Width="230px">
        </asp:DropDownList>
        <asp:Button ID="SelectRP" runat="server" onclick="SelectRP_Click" 
            Text="Select RP" />
    </p>
    <p>
        <asp:CheckBoxList ID="CheckBoxList2" runat="server">
        </asp:CheckBoxList>
    </p>
    <p>
        <asp:Button ID="GetTokenAsSystem" runat="server" 
            onclick="GetTokenAsSystem_Click" Text="Get Token As System" />
    </p>
    <p>
        <asp:GridView ID="GridView5" runat="server" OnRowDataBound="OnADFSTokenTestBound">
        </asp:GridView>
    </p>
    <p>
        Get test token as User. Provide Domain\UserName and Password :</p>
    <p>
        <asp:TextBox ID="TextBox3" runat="server" ToolTip="UserName in Domain\SAMAccountName format."></asp:TextBox>
        <asp:TextBox ID="TextBox4" runat="server" TextMode="Password" ToolTip="DPAPI Protected password."></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="GetTokenAsUser" runat="server" onclick="GetTokenAsUser_Click" 
            Text="Get Token As User" />
    </p>
    <p>
        <asp:GridView ID="GridView6" runat="server" OnRowDataBound="OnADFSTokenTestBound">
        </asp:GridView>
    </p>
    <p>
        
    </p>
</asp:Content>
