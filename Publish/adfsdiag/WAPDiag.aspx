<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="WAPDiag.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Web Application Proxy Diagnostics</h2>
    <p>
        Provide the name of a Web Application Proxy Server.
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </p>
    <p>
        <asp:TextBox ID="TextBox1" runat="server" ToolTip="WAP Server Name"></asp:TextBox>
        <asp:Button ID="GetWAPs" runat="server" onclick="GetWAPs_Click" 
            Text="Get WAPs" />
        <asp:Button ID="MetaCheck" runat="server" onclick="MetaCheck_Click" 
            Text="Meta Check" />
        <asp:Button ID="GetHealth" runat="server" onclick="GetHealth_Click" 
            Text="Get Health" />
        <asp:Button ID="GetDbConnectivity" runat="server" 
            onclick="GetDbConnectivity_Click" Text="Get Db Connectivity" />
        <asp:Button ID="PortCheck" runat="server" onclick="PortCheck_Click" 
            Text="Port Check" />
        <asp:Button ID="CertCheck" runat="server" onclick="CertCheck_Click" 
            Text="Cert Check" />
    </p>
    <p>
        <asp:Label ID="Label2" runat="server"></asp:Label>
    </p>
    <p>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
        </asp:CheckBoxList>
    </p>
    <p>
        Web Application Proxy Metadata check report :</p>
    <p>
        <asp:GridView ID="GridView1" runat="server" OnRowDataBound = "OnRowDataBoundMetaCheck">
        </asp:GridView>
    </p>
    <p>
        Web Application Proxy health report :</p>
    <p>
        <asp:GridView ID="GridView2" runat="server" OnRowDataBound = "OnRowDataBoundWapHealthNDbCon">
        </asp:GridView>
    </p>
    <p>
        Web Application Proxy Db connectivity report :</p>
    <p>
        <asp:GridView ID="GridView3" runat="server" OnRowDataBound = "OnRowDataBoundWapHealthNDbCon">
        </asp:GridView>
    </p>
    <p>
        Web Application Proxy Port check report :</p>
    <p>
        <asp:GridView ID="GridView4" runat="server" OnRowDataBound= "OnRowDataBoundPort">
        </asp:GridView>
    </p>
    <p>
        Web Application Proxy certificate report :
        <asp:Label ID="Label3" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="GridView5" runat="server" OnRowDataBound="OnRowDataBoundCertCheck">
        </asp:GridView>
    </p>
    <p>
        Web Application Proxy Token report : Provide the name of a Web Application proxy 
        server :
        <asp:Label ID="Label4" runat="server"></asp:Label>
    </p>
    <p>
        <asp:TextBox ID="TextBox2" runat="server" ToolTip="WAP Server Name"></asp:TextBox>
        <asp:Button ID="GetWapsForToken" runat="server" onclick="GetWapsForToken_Click" 
            Text="Get WAPs" />
        <asp:TextBox ID="TextBox3" runat="server" ToolTip="ADFS Server Name"></asp:TextBox>
        <asp:DropDownList ID="DropDownList1" runat="server" Width="250px">
        </asp:DropDownList>
        <asp:Button ID="SelectRP" runat="server" onclick="SelectRP_Click" 
            Text="Select RP" />
    </p>
    <p>
        <asp:CheckBoxList ID="CheckBoxList2" runat="server">
        </asp:CheckBoxList>
    </p>
    <p>
        Get test token as computer :</p>
    <p>
        <asp:Button ID="GetTokenAsSystem" runat="server" 
            onclick="GetTokenAsSystem_Click" Text="Get Token As System" />
    </p>
    <p>
        <asp:GridView ID="GridView6" runat="server" onrowdatabound="OnWAPTokenTestBound">
        </asp:GridView>
    </p>
    <p>
        Get test token as User. Provide Domain\UserName and Password :</p>
    <p>
        <asp:TextBox ID="TextBox4" runat="server" 
            ToolTip="UserName in Domain\SAMAccountName format."></asp:TextBox>
        <asp:TextBox ID="TextBox5" runat="server" TextMode="Password" 
            ToolTip="DPAPI Protected password."></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="GetTokenAsUser" runat="server" onclick="GetTokenAsUser_Click" 
            Text="Get Token As User" />
    </p>
    <p>
        <asp:GridView ID="GridView7" runat="server" OnRowDataBound="OnWAPTokenTestBound">
        </asp:GridView>
    </p>
    <p>
        
    </p>
    <p>
        
    </p>
</asp:Content>
