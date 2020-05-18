<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="configuration.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Configuration</h2>
    <p>
        Current
        Farm Name:
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </p>
<p>
        <asp:TextBox ID="TextBox1" runat="server" Width="500px"></asp:TextBox>
        <asp:Button ID="UpdateFarmName" runat="server" onclick="UpdateFarmName_Click" 
            Text="Update Farm Name" />
    </p>
<p>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </p>
    <p>
        Virtual IPs:
        <asp:Label ID="lblSuccessMessage" runat="server"></asp:Label>
        <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
        ShowFooter="True" DataKeyNames="id" ShowHeaderWhenEmpty="True" 
        OnRowCommand="GridView2_RowCommand" OnRowEditing="GridView2_RowEditing" 
        OnRowCancelingEdit="GridView2_RowCancelingEdit" 
        OnRowUpdating="GridView2_RowUpdating" OnRowDeleting="GridView2_RowDeleting" 
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="3px" 
        CellPadding="4" >
    
                <Columns>
                    <asp:TemplateField HeaderText="Site">
                        <ItemTemplate>
                            <asp:Label ID="Label1" Text='<%# Eval("Site") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSite" Text='<%# Eval("Site") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtSiteFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="VIP">
                        <ItemTemplate>
                            <asp:Label ID="Label2" Text='<%# Eval("VIP") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtVIP" Text='<%# Eval("VIP") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtVIPFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/edit.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px"/>
                            <asp:ImageButton ID="ImageButton2" ImageUrl="~/Images/delete.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="20px" Height="20px"/>
                        </ItemTemplate>
                    
                        <EditItemTemplate>
                            <asp:ImageButton ID="ImageButton3" ImageUrl="~/Images/save.png" runat="server" CommandName="Update" ToolTip="Update" Width="20px" Height="20px"/>
                            <asp:ImageButton ID="ImageButton4" ImageUrl="~/Images/cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px"/>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <asp:ImageButton ID="ImageButton5" ImageUrl="~/Images/addnew.png" runat="server" CommandName="AddNew" ToolTip="Add New" Width="20px" Height="20px"/>
                        </FooterTemplate>

                    </asp:TemplateField>

                </Columns>
                <FooterStyle BackColor="#CCCCCC" ForeColor="#003399" />
                <HeaderStyle BackColor="#336699" Font-Bold="True" ForeColor="#CCCCFF" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                <SortedDescendingHeaderStyle BackColor="#002876" />
    </asp:GridView>


    </p>
    <p>
        &nbsp;</p>
</asp:Content>
