<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="VipDiag.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Virtual IP Diagnostics</h2>
    <p>
        Farm Name :
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"   
            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="3px"   
            CellPadding="3">  
            <Columns>  
                <asp:TemplateField HeaderText="Site">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBoxSite" runat="server" Text='<%# Bind("Site") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="LabelSite" runat="server" Text='<%# Bind("Site") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="VIP">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBoxVip" runat="server" Text='<%# Bind("VIP") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="LabelVip" runat="server" Text='<%# Bind("VIP") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Select">  
                    <EditItemTemplate>  
                        <asp:CheckBox ID="CheckBoxSiteVip" runat="server" />  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:CheckBox ID="CheckBoxSiteVip" runat="server" />  
                    </ItemTemplate>  
                </asp:TemplateField>
            </Columns>  
            <FooterStyle BackColor="White" ForeColor="#000066" />  
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />  
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />  
            <RowStyle ForeColor="#000066" />  
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />  
            <SortedAscendingCellStyle BackColor="#F1F1F1" />  
            <SortedAscendingHeaderStyle BackColor="#007DBB" />  
            <SortedDescendingCellStyle BackColor="#CAC9C9" />  
            <SortedDescendingHeaderStyle BackColor="#00547E" />  
        </asp:GridView>
    </p>
    <p>
        <asp:Button ID="VipMetaCheck" runat="server" onclick="VipMetaCheck_Click" 
            Text="VIP Meta Check" />
    </p>
    <p>
    <asp:GridView ID="GridView2" runat="server" OnRowDataBound = "OnRowDataBoundMetaCheck">
        </asp:GridView>
    </p>
        
    <p>
        VIP Token report (as SYSTEM ):
        <asp:Label ID="Label3" runat="server"></asp:Label>
    </p>
    <p>
        <asp:TextBox ID="TextBox1" runat="server" ToolTip="ADFS Server Name"></asp:TextBox>
        <asp:DropDownList ID="DropDownList1" runat="server" Width="230px">
        </asp:DropDownList>
        <asp:Button ID="SelectRP" runat="server" OnClick="SelectRP_Click" Text="Select RP" />
    </p>
    <p>
        <asp:Button ID="VIPTokenCheckasSystem" runat="server" OnClick="VIPTokenCheckasSystem_Click" Text="VIP Token Check as System" />
    </p>
    <p>
        <asp:GridView ID="GridView3" runat="server" OnRowDataBound="OnVipTokenTestBound">
        </asp:GridView>
    </p>
    <p>
       
        Get test token as User. Provide Domain\UserName and Password :
        <asp:Label ID="Label4" runat="server"></asp:Label>
       
    </p>
    <p>
       
        <asp:TextBox ID="TextBox2" runat="server" ToolTip="UserName in Domain\SAMAccountName format."></asp:TextBox>
        <asp:TextBox ID="TextBox3" runat="server" TextMode="Password" ToolTip="DPAPI Protected password."></asp:TextBox>
       
    </p>
    <p>
       
        <asp:Button ID="GetTokenasUser" runat="server" OnClick="GetTokenasUser_Click" Text="Get Token as User" />
       
    </p>
    <p>
       
        <asp:GridView ID="GridView4" runat="server" OnRowDataBound="OnvipTokenTestUserBound">
        </asp:GridView>
       
    </p>
    <p>
       
        &nbsp;</p>
</asp:Content>
