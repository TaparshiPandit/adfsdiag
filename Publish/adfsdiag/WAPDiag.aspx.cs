using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
//using System.DirectoryServices;
using System.Reflection;
using System.ServiceModel.Security;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceModel.Channels;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using System.IdentityModel.Claims;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Web;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //Colour gridview
    //Metadata
    protected void OnRowDataBoundMetaCheck(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if ((drv["Response"].ToString().Equals("OK")))
            {
                e.Row.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.OrangeRed;
                e.Row.ForeColor = System.Drawing.Color.White;
            }
        }

    }
    protected void OnRowDataBoundWapHealthNDbCon(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if ((drv["HealthState"].ToString().Equals("OK")) || (drv["HealthState"].ToString().Equals("30 Sec Poll")))
            {
                e.Row.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.OrangeRed;
                e.Row.ForeColor = System.Drawing.Color.White;
            }
        }

    }
    
    protected void OnRowDataBoundPort(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if ((e.Row.Cells[i].Text).Contains("True") == true)
                {
                    e.Row.Cells[i].BackColor = System.Drawing.Color.LightGreen;
                }
                else if ((e.Row.Cells[i].Text).Contains("failed") == true)
                {
                    e.Row.Cells[i].BackColor = System.Drawing.Color.OrangeRed;
                    e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                }
            }
        }
    }

    protected void OnWAPTokenTestBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if ((drv["TokenResponse"].ToString().Contains("@")) || (drv["TokenResponse"].ToString().Contains("authentication")))
            {
                e.Row.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.Red;
                e.Row.ForeColor = System.Drawing.Color.White;
            }
        }
    }

    //OnRowDataBoundCertCheck
    protected void OnRowDataBoundCertCheck(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            //if ((drv["Status"].ToString().Contains("Bad")))
            if ((drv["Status"].ToString() != ""))
            {
                e.Row.BackColor = System.Drawing.Color.OrangeRed;
                e.Row.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.LightGreen;
            }
        }
    }
    //Colour gridview end

    protected void GetWAPs_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        CheckBoxList1.Items.Clear();
        string DiscoveryWAP = TextBox1.Text;
        try
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddCommand("invoke-command");
                PowerShellInstance.AddParameter("ComputerName", DiscoveryWAP);
                ScriptBlock filter2 = ScriptBlock.Create("Get-WebApplicationProxyConfiguration | select -ExpandProperty ConnectedServersName");
                PowerShellInstance.AddParameter("ScriptBlock", filter2);
                Collection<PSObject> AllWAps = PowerShellInstance.Invoke();

                foreach (Object WAP in AllWAps)
                {
                    if (WAP != null)
                    {
                        string WAPNameS = (WAP).ToString();
                        CheckBoxList1.Items.Add(WAPNameS);
                    }

                }
                if (PowerShellInstance.Streams.Error.Count > 0)
                {
                    var errors = PowerShellInstance.Streams.Error;
                    var sb = new StringBuilder();
                    foreach (var error in errors)
                    {
                        sb.Append(error);
                    }
                    string errorResult = sb.ToString();
                    Label1.Text = "Try a different server" + ": " + DiscoveryWAP + " " + errorResult;
                    Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                }

            } //PS Instance end
        }
        catch (Exception DiscoveryWAPError)
        {
            Label1.Text = DiscoveryWAPError.Message.ToString();
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        
    }

    protected void MetaCheck_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label2.Text = "Please select WAP servers to view Metadata response.";
            Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
        else
        {
            Label2.Text = "";
            AdfsSqlHelper stsName = new AdfsSqlHelper();
            string sts = stsName.GetFarmName();
            var SelectedWAPs = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable MetaCheckTable = new DataTable();
            MetaCheckTable.Columns.Add("WAP Server Name");
            MetaCheckTable.Columns.Add("IP");
            MetaCheckTable.Columns.Add("Response URL");
            MetaCheckTable.Columns.Add("Response");

            foreach (object WAP in SelectedWAPs)
            {
                IPAddress[] ipaddress = Dns.GetHostAddresses(WAP.ToString());

                DataRow row = MetaCheckTable.NewRow();
                row["WAP Server Name"] = WAP.ToString();
                foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    row["IP"] = (ip4.ToString());
                    //
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://" + sts + "/federationmetadata/2007-06/federationmetadata.xml");//url and Host header
                    FieldInfo field_ServicePoint_ProxyServicePoint = (typeof(ServicePoint))
                        .GetField("m_ProxyServicePoint", BindingFlags.NonPublic | BindingFlags.Instance);
                    req.Proxy = new WebProxy(ip4.ToString() + ":443");//server IP and port
                    field_ServicePoint_ProxyServicePoint.SetValue(req.ServicePoint, false);
                    //req.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials; 
                    try
                    {
                        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                        //Label9.Text = resp.Cookies.ToString();
                        row["Response URL"] = resp.ResponseUri.ToString();
                        row["Response"] = resp.StatusCode.ToString();
                    }
                    catch (Exception MetadataExp)
                    {
                        //Label9.Text = MetadataExp.ToString();
                        row["Response URL"] = MetadataExp.Message.ToString();
                        row["Response"] = MetadataExp.Message.ToString() + " Check Farname is correct under configuration tab and servers have correct SNI bindings.";
                        //Label1.Text = "Check Farname is correct under configuration tab and servers have correct SNI bindings.";
                        //Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                    }

                }
                MetaCheckTable.Rows.Add(row);
            }

            GridView1.DataSource = MetaCheckTable;
            GridView1.DataBind();
        }
    }
    protected void GetHealth_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        Label2.Text = "";
        GridView2.DataSource = null;
        GridView2.DataBind();
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label2.Text = "Please select WAP servers to view health.";
            Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            var SelectedWAPs = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable WAPtempTable = new DataTable();
            WAPtempTable.Columns.Add("WAP Server Name");
            WAPtempTable.Columns.Add("Component");
            WAPtempTable.Columns.Add("HealthState");
            WAPtempTable.Columns.Add("Heuristics");
            WAPtempTable.Columns.Add("CommandRunStatus");

            foreach (object WAP in SelectedWAPs)
            {
                using (PowerShell PowerShellInstance = PowerShell.Create())
                {
                    PowerShellInstance.AddCommand("invoke-command");
                    PowerShellInstance.AddParameter("ComputerName", WAP);
                    ScriptBlock filterAllWAPsHealth = ScriptBlock.Create("Get-WebApplicationProxyHealth | select RemoteAccessServer, Component, HealthState, Heuristics");
                    PowerShellInstance.AddParameter("ScriptBlock", filterAllWAPsHealth);
                    foreach (PSObject outputItem in PowerShellInstance.Invoke())
                    {
                        DataRow row = WAPtempTable.NewRow();

                        row["WAP Server Name"] = (outputItem.Members["PSComputerName"].Value).ToString();
                        row["Component"] = (outputItem.Members["Component"].Value).ToString();
                        row["HealthState"] = (outputItem.Members["HealthState"].Value).ToString();
                        if ((outputItem.Members["Heuristics"].Value).ToString() != null)
                        {
                            row["Heuristics"] = (outputItem.Members["Heuristics"].Value).ToString();
                        }
                        row["CommandRunStatus"] = "Success";

                        WAPtempTable.Rows.Add(row);
                    }

                    if (PowerShellInstance.Streams.Error.Count > 0)
                    {
                        DataRow row = WAPtempTable.NewRow();
                        var errors = PowerShellInstance.Streams.Error;
                        var sb = new StringBuilder();
                        foreach (var error in errors)
                        {
                            sb.Append(error);
                        }
                        string errorResult = sb.ToString();
                        row["WAP Server Name"] = WAP.ToString();
                        row["Component"] = errorResult;
                        row["HealthState"] = errorResult;
                        row["HealthState"] = errorResult;
                        row["CommandRunStatus"] = errorResult;
                        WAPtempTable.Rows.Add(row);

                    }

                } //Powershell Instance 1
            } //For each end
            GridView2.DataSource = WAPtempTable;
            GridView2.DataBind();
        }//Else End
    }//botton end

    protected void GetDbConnectivity_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        Label2.Text = "";
        GridView3.DataSource = null;
        GridView3.DataBind();
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label2.Text = "Please select WAP servers to view DB connectivity.";
            Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            var SelectedWAPs = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable WAPConTable = new DataTable();
            WAPConTable.Columns.Add("WAP Server Name");
            WAPConTable.Columns.Add("DBConnectivity");
            WAPConTable.Columns.Add("HealthState");
            WAPConTable.Columns.Add("CommandRunStatus");

            foreach (object WAP in SelectedWAPs)
            {
                using (PowerShell PowerShellInstance = PowerShell.Create())
                {
                    PowerShellInstance.AddCommand("invoke-command");
                    PowerShellInstance.AddParameter("ComputerName", WAP);
                    ScriptBlock filterAllWAPDBCon = ScriptBlock.Create("Get-WebApplicationProxyConfiguration | select ConfigurationVersion, ConfigurationChangesPollingIntervalSec -ErrorAction Stop");
                    PowerShellInstance.AddParameter("ScriptBlock", filterAllWAPDBCon);
                    
                    foreach (PSObject outputItemDCCon in PowerShellInstance.Invoke())
                    {
                        DataRow row = WAPConTable.NewRow();
                        row["WAP Server Name"] = (outputItemDCCon.Members["PSComputerName"].Value).ToString();
                        row["DBConnectivity"] = (outputItemDCCon.Members["ConfigurationVersion"].Value).ToString();
                        row["HealthState"] = (outputItemDCCon.Members["ConfigurationChangesPollingIntervalSec"].Value).ToString() + " Sec Poll";
                        row["CommandRunStatus"] = "Success";
                        WAPConTable.Rows.Add(row);
                    }

                    // if error was here
                    if (PowerShellInstance.Streams.Error.Count > 0)
                    {
                        DataRow row = WAPConTable.NewRow();
                        var errors = PowerShellInstance.Streams.Error;
                        var PS = PowerShellInstance;
                        var sb = new StringBuilder();
                        foreach (var error in errors)
                        {
                            sb.Append(error); // changed here
                        }
                        string errorResult = sb.ToString();
                        row["WAP Server Name"] = WAP.ToString(); //you were here trying to find the computer that generated the error while backend server was inaccessible
                        row["DBConnectivity"] = errorResult;
                        row["HealthState"] = "Error";
                        row["CommandRunStatus"] = "Error";
                        WAPConTable.Rows.Add(row);
                    }
                } //PowerShell instance
            }
            GridView3.DataSource = WAPConTable;
            GridView3.DataBind();
        }//Else end
    }//button end

    protected void PortCheck_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        Label2.Text = "";
        GridView4.DataSource = null;
        GridView4.DataBind();
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label2.Text = "Please select WAP servers to view Port status.";
            Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            var SelectedWAPs = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable PortConTable = new DataTable();
            PortConTable.Columns.Add("WAP Server Name");
            PortConTable.Columns.Add("Port 443");
            PortConTable.Columns.Add("Port 49443");

            foreach (object WapSrvr in SelectedWAPs)
            {
                DataRow row = PortConTable.NewRow();
                try
                {
                    System.Net.Sockets.TcpClient client443 = new TcpClient(WapSrvr.ToString(), 443);
                    row["WAP Server Name"] = WapSrvr.ToString();
                    row["Port 443"] = client443.Connected.ToString();
                    client443.Close();
                }

                catch (System.Net.Sockets.SocketException Port443ConExp)
                {
                    row["WAP Server Name"] = WapSrvr.ToString();
                    row["Port 443"] = Port443ConExp.Message;
                }
                try
                {
                    System.Net.Sockets.TcpClient client49443 = new TcpClient(WapSrvr.ToString(), 49443);
                    row["WAP Server Name"] = WapSrvr.ToString();
                    row["Port 49443"] = client49443.Connected.ToString();
                    client49443.Close();
                }
                catch (Exception Port49443ConExp)
                {
                    row["WAP Server Name"] = WapSrvr.ToString();
                    row["Port 49443"] = Port49443ConExp.Message;
                }
                PortConTable.Rows.Add(row);

            }
            GridView4.DataSource = PortConTable;
            GridView4.DataBind();
        } // Else end.


    }
    protected void CertCheck_Click(object sender, EventArgs e)
    {
        Label3.Text = "";
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label2.Text = "Please select WAP servers to view Cert status.";
            Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView5.DataSource = null;
            GridView5.DataBind();
        }
        else
        {
            var SelectedWAPs = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            Label3.Text = "";
            //Table 1
            DataTable BadCertTable = new DataTable();
            BadCertTable.Columns.Add("WAP Server Name");
            BadCertTable.Columns.Add("Non Self-Signed cert in Root");
            BadCertTable.Columns.Add("Issuer");
            BadCertTable.Columns.Add("Thumbprint");
            BadCertTable.Columns.Add("Status");
            //Table 2
            DataTable ExpTable = new DataTable();
            ExpTable.Columns.Add("WAP Server Name");
            ExpTable.Columns.Add("Non Self-Signed cert in Root");
            ExpTable.Columns.Add("Issuer");
            ExpTable.Columns.Add("Thumbprint");
            ExpTable.Columns.Add("Status");

            //Check if non Self-Signed Certificates exist in Root Store
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddCommand("invoke-command");
                PowerShellInstance.AddParameter("ComputerName", SelectedWAPs);
                ScriptBlock filterCert = ScriptBlock.Create("Get-ChildItem cert:\\localmachine\\Root |?{$_.Issuer -ne $_.Subject} |select Thumbprint, Subject, Issuer");
                PowerShellInstance.AddParameter("ScriptBlock", filterCert);
                Collection<PSObject> NonSelfCertInRoot = PowerShellInstance.Invoke();

                //Build bad cert table
                foreach (PSObject Cert in NonSelfCertInRoot)
                {
                    if (Cert != null)
                    {
                        DataRow row = BadCertTable.NewRow();
                        row["WAP Server Name"] = (Cert.Members["PSComputerName"].Value).ToString();
                        row["Non Self-Signed cert in Root"] = (Cert.Members["Subject"].Value).ToString();
                        row["Issuer"] = (Cert.Members["Issuer"].Value).ToString();
                        row["Thumbprint"] = (Cert.Members["Thumbprint"].Value).ToString();
                        row["Status"] = "Bad cert found";
                        BadCertTable.Rows.Add(row);
                    }
                }

                //Build PS Exception table.
                var errors = PowerShellInstance.Streams.Error;
                var PS = PowerShellInstance;
                foreach (var error in errors)
                {
                    DataRow Exprow = ExpTable.NewRow();
                    var sb = new StringBuilder();
                    sb.Append(error);
                    string errorResult = sb.ToString();
                    Exprow["WAP Server Name"] = errorResult;
                    Exprow["Non Self-Signed cert in Root"] = "error";
                    Exprow["Issuer"] = "error";
                    Exprow["Thumbprint"] = "error";
                    Exprow["Status"] = "Could not execute command";
                    ExpTable.Rows.Add(Exprow);
                }

            } //PS Instance End

            BadCertTable.Merge(ExpTable);
            BadCertTable.AcceptChanges();
            GridView5.DataSource = BadCertTable;
            GridView5.DataBind();

            if (GridView5.Rows.Count > 0)
            {
                Label3.Text = "Either incorrect certificates were found Or query failed on some WAP servers. Please check the report below for deails.";
                Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
            else
            {
                Label3.Text = "All WAP servers are clean.";
                Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#006400");
            }
        }// else end
    }//Cert check end.

    protected void GetWapsForToken_Click(object sender, EventArgs e)
    {
        Label4.Text = "";
        CheckBoxList2.Items.Clear();
        string DiscoveryWAP = TextBox2.Text;
        try
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddCommand("invoke-command");
                PowerShellInstance.AddParameter("ComputerName", DiscoveryWAP);
                ScriptBlock filter2 = ScriptBlock.Create("Get-WebApplicationProxyConfiguration | select -ExpandProperty ConnectedServersName");
                PowerShellInstance.AddParameter("ScriptBlock", filter2);
                Collection<PSObject> AllWAps = PowerShellInstance.Invoke();

                foreach (Object WAP in AllWAps)
                {
                    if (WAP != null)
                    {
                        string WAPNameS = (WAP).ToString();
                        CheckBoxList2.Items.Add(WAPNameS);
                    }
                }
                if (PowerShellInstance.Streams.Error.Count > 0)
                {
                    var errors = PowerShellInstance.Streams.Error;
                    var sb = new StringBuilder();
                    foreach (var error in errors)
                    {
                        sb.Append(error);
                    }
                    string errorResult = sb.ToString();
                    Label4.Text = "Try a different server" + ": " + DiscoveryWAP + " " + errorResult;
                    Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                }
            } //PS Instance end
        }
        catch (Exception DiscoveryWAPError)
        {
            Label4.Text = DiscoveryWAPError.Message.ToString();
            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
    }//Get WAPs for token test end.



    protected void SelectRP_Click(object sender, EventArgs e)
    {
        DropDownList1.Items.Clear();
        Label4.Text = "";
        //GridView11.DataSource = null;
        //GridView11.DataBind();
        if (TextBox3.Text != "")
        {
            try
            {
                System.Net.Sockets.TcpClient client = new TcpClient(TextBox3.Text, 5985);
                using (PowerShell PowerShellInstance = PowerShell.Create())
                {
                    string DiscoveryADFS = TextBox3.Text;
                    PowerShellInstance.AddCommand("invoke-command");
                    PowerShellInstance.AddParameter("ComputerName", DiscoveryADFS);
                    ScriptBlock filter = ScriptBlock.Create("Get-AdfsRelyingPartyTrust | select -ExpandProperty Identifier");
                    PowerShellInstance.AddParameter("ScriptBlock", filter);
                    Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                    foreach (Object Identifier in PSOutput)
                    {
                        if (Identifier != null)
                        {
                            string RPNameS = Identifier.ToString();
                            DropDownList1.Items.Add(RPNameS);
                            DropDownList1.Items[0].Selected = true;
                            Label4.Text = "Default selected RP Identifier " + DropDownList1.SelectedItem.Text;
                            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#008000");
                        }
                    }
                    if (PowerShellInstance.Streams.Error.Count > 0)
                    {
                        var errors = PowerShellInstance.Streams.Error;
                        var sb = new StringBuilder();
                        foreach (var error in errors)
                        {
                            sb.Append(error);
                        }
                        string errorResult = sb.ToString();
                        Label4.Text = errorResult;
                        Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                    }

                } // PS End

            }
            catch (Exception ExSelRP)
            {
                Label4.Text = ExSelRP.Message;
                Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
        }
        else
        {
            Label4.Text = "Please Provider an adfs server namespace to get rps";
            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
    }

    protected void GetTokenAsSystem_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList2.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label4.Text = "You have not selected a WAP server. Please select WAP server to view Token response.";
            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView6.DataSource = null;
            GridView6.DataBind();
        }

        if (DropDownList1.SelectedItem != null)
        {
            AdfsSqlHelper stsName = new AdfsSqlHelper();
            string FarmEndpoint = stsName.GetFarmName();

            var SelectedADFS = CheckBoxList2.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable TokenCheckTable = new DataTable();
            TokenCheckTable.Columns.Add("WAP Server Name");
            TokenCheckTable.Columns.Add("IP");
            TokenCheckTable.Columns.Add("Target Identifier");
            TokenCheckTable.Columns.Add("TokenResponse");

            foreach (object ADFS in SelectedADFS)
            {
                IPAddress[] ipaddress = Dns.GetHostAddresses(ADFS.ToString());
                DataRow row = TokenCheckTable.NewRow();
                row["WAP Server Name"] = ADFS.ToString();
                foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    row["IP"] = (ip4.ToString());
                    String appliesTo = DropDownList1.SelectedItem.Text;
                    String federationServer = ip4.ToString();
                    String endpoint = "https://" + federationServer + "/adfs/services/trust/2005/windowstransport";
                    //DateTime now = new DateTime();
                    //DateTime FinalTime = now.AddMinutes(60);

                    //Request body RST. Do not alter.
                    String RST = "<s:Envelope xmlns:s=" +
                    "\"http://www.w3.org/2003/05/soap-envelope\"" + " " +
                    "xmlns:a=" + "\"http://www.w3.org/2005/08/addressing\"" + " " +
                    "xmlns:u=" + "\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"" +
                    "><s:Header><a:Action s:mustUnderstand=" + "\"1\"" + ">" + "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue" +
                    "</a:Action><a:ReplyTo><a:Address>" + "http://www.w3.org/2005/08/addressing/anonymous" + "</a:Address></a:ReplyTo><a:To s:mustUnderstand=" +
                    "\"1\"" + ">{" + "0" + "}<" + "/a:To></s:Header><s:Body><t:RequestSecurityToken xmlns:t=" + "\"http://schemas.xmlsoap.org/ws/2005/02/trust\"" + "><wsp:AppliesTo xmlns:wsp=" +
                    "\"http://schemas.xmlsoap.org/ws/2004/09/policy\"" + "><a:EndpointReference><a:Address>" + "{" + "1" + "}</a:Address></a:EndpointReference></wsp:AppliesTo><t:KeySize>0</t:KeySize><t:KeyType>" +
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey" + "</t:KeyType><t:RequestType>" + "http://schemas.xmlsoap.org/ws/2005/02/trust/Issue" + "</t:RequestType><t:TokenType>" +
                    "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0" + "</t:TokenType></t:RequestSecurityToken></s:Body></s:Envelope>";

                    String ComputedRST = String.Format(RST, endpoint, appliesTo);

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://" + FarmEndpoint + "/adfs/services/trust/2005/windowstransport");//url and Host header
                    FieldInfo field_ServicePoint_ProxyServicePoint = (typeof(ServicePoint))
                        .GetField("m_ProxyServicePoint", BindingFlags.NonPublic | BindingFlags.Instance);
                    req.Proxy = new WebProxy(ip4.ToString() + ":443");//server IP and port
                    field_ServicePoint_ProxyServicePoint.SetValue(req.ServicePoint, false);
                    req.Referer = "https://" + FarmEndpoint;
                    req.Headers.Add("Name", "https://" + FarmEndpoint + "/adfs/services/trust/2005/windowstransport");
                    req.Method = "POST";
                    req.ContentType = "application/soap+xml";
                    //req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                    req.KeepAlive = true;
                    req.AllowAutoRedirect = false;
                    byte[] data = Encoding.UTF8.GetBytes(ComputedRST);
                    req.ContentLength = data.Length;
                    req.Credentials = CredentialCache.DefaultCredentials;
                    req.Credentials = CredentialCache.DefaultNetworkCredentials;
                    try
                    {
                        Stream dataStream = req.GetRequestStream();
                        dataStream.Write(data, 0, data.Length);
                        dataStream.Close();
                        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                        byte[] result = null;
                        int byteCount = Convert.ToInt32(resp.ContentLength);
                        using (BinaryReader reader = new BinaryReader(resp.GetResponseStream()))
                        {
                            result = reader.ReadBytes(byteCount);
                            row["Target Identifier"] = DropDownList1.SelectedItem.Value;

                            //Dirty way to extract claims
                            string claims = System.Text.Encoding.UTF8.GetString(result);
                            XmlDocument xmltest = new XmlDocument();
                            xmltest.LoadXml(claims);
                            string xmltoken = xmltest.InnerText;
                            if (xmltoken.Contains("SAMLV2"))
                            {
                                int first = xmltoken.IndexOf("@");
                                int last = xmltoken.LastIndexOf("windows");
                                string Finaltoken = xmltoken.Substring(first, last - first);
                                row["TokenResponse"] = Finaltoken;
                            }
                            else
                            {
                                row["TokenResponse"] = xmltoken;
                            }
                        }

                    }
                    catch (Exception TokenExp)
                    {
                        row["Target Identifier"] = DropDownList1.SelectedItem.Value;
                        row["TokenResponse"] = TokenExp.Message.ToString();
                    }
                }
                TokenCheckTable.Rows.Add(row);
            }
            GridView6.DataSource = TokenCheckTable;
            GridView6.DataBind();
        }
        else
        {
            Label4.Text = "You have not selected an RP Identifier.";
            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView6.DataSource = null;
            GridView6.DataBind();
        }
    }//Get token as system end.


    protected void GetTokenAsUser_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList2.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label4.Text = "You have not selected a WAP server. Please select WAP server to view Token response.";
            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView7.DataSource = null;
            GridView7.DataBind();
        }
        if (DropDownList1.SelectedItem != null)
        {
            AdfsSqlHelper stsName = new AdfsSqlHelper();
            string FarmEndpoint = stsName.GetFarmName();

            var SelectedADFS = CheckBoxList2.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable TokenCheckTable = new DataTable();
            TokenCheckTable.Columns.Add("WAP Server Name");
            TokenCheckTable.Columns.Add("IP");
            TokenCheckTable.Columns.Add("Target Identifier");
            TokenCheckTable.Columns.Add("TokenResponse");

            foreach (object ADFS in SelectedADFS)
            {
                IPAddress[] ipaddress = Dns.GetHostAddresses(ADFS.ToString());
                DataRow row = TokenCheckTable.NewRow();
                row["WAP Server Name"] = ADFS.ToString();
                foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    row["IP"] = (ip4.ToString());
                    String appliesTo = DropDownList1.SelectedItem.Text;
                    String federationServer = ip4.ToString();
                    String endpoint = "https://" + federationServer + "/adfs/services/trust/2005/usernamemixed";
                    DateTime now = new DateTime();
                    DateTime FinalTime = now.AddMinutes(60);
                    string username = TextBox4.Text;
                    string password = TextBox5.Text.Protect();

                    //Request body RST. Do not alter.
                    String RST = String.Format("<s:Envelope xmlns:s=" +
                        "\"http://www.w3.org/2003/05/soap-envelope\"" +
                        " " + "xmlns:a=" + "\"http://www.w3.org/2005/08/addressing\"" +
                        " " + "xmlns:u=" + "\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"" +
                        "><s:Header><a:Action s:mustUnderstand=" +
                        "\"1\"" + ">http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue" +
                        "</a:Action><a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo><a:To s:mustUnderstand=" +
                        "\"1\"" + ">" + endpoint + "</a:To><o:Security s:mustUnderstand=" +
                        "\"1\"" + " " + "xmlns:o=" + "\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"" +
                        "><o:UsernameToken><o:Username>" + username + "</o:Username><o:Password Type=" +
                        "\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\"" +
                        ">" + password.Unprotect() + "</o:Password></o:UsernameToken></o:Security></s:Header><s:Body><t:RequestSecurityToken xmlns:t=" +
                        "\"http://schemas.xmlsoap.org/ws/2005/02/trust\"" + "><wsp:AppliesTo xmlns:wsp=" +
                        "\"http://schemas.xmlsoap.org/ws/2004/09/policy\"" + "><a:EndpointReference><a:Address>" + appliesTo +
                        "</a:Address></a:EndpointReference></wsp:AppliesTo><t:KeySize>0</t:KeySize><t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType><t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType><t:TokenType>urn:oasis:names:tc:SAML:1.0:assertion</t:TokenType></t:RequestSecurityToken></s:Body></s:Envelope>");

                    //String RST = String.Format("<s:Envelope xmlns:s=" + "\"http://www.w3.org/2003/05/soap-envelope\"" + " " + "xmlns:a=" + "\"http://www.w3.org/2005/08/addressing\"" + " " + "xmlns:u=" + "\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"" + "><s:Header><a:Action s:mustUnderstand=" + "\"1\"" + ">http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue" + "</a:Action><a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo><a:To s:mustUnderstand=" + "\"1\"" + ">" + endpoint + "</a:To><o:Security s:mustUnderstand=" + "\"1\"" + " " + "xmlns:o=" + "\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"" + "><o:UsernameToken><o:Username>" + username + "</o:Username><o:Password Type=" + "\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\"" + ">" + password + "</o:Password></o:UsernameToken></o:Security></s:Header><s:Body><t:RequestSecurityToken xmlns:t=" + "\"http://schemas.xmlsoap.org/ws/2005/02/trust\"" + "><wsp:AppliesTo xmlns:wsp=" + "\"http://schemas.xmlsoap.org/ws/2004/09/policy\"" + "><a:EndpointReference><a:Address>" + appliesTo + "</a:Address></a:EndpointReference></wsp:AppliesTo><t:KeySize>0</t:KeySize><t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType><t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType><t:TokenType>urn:oasis:names:tc:SAML:1.0:assertion</t:TokenType></t:RequestSecurityToken></s:Body></s:Envelope>");

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://" + FarmEndpoint + "/adfs/services/trust/2005/usernamemixed");//url and Host header
                    FieldInfo field_ServicePoint_ProxyServicePoint = (typeof(ServicePoint))
                        .GetField("m_ProxyServicePoint", BindingFlags.NonPublic | BindingFlags.Instance);
                    req.Proxy = new WebProxy(ip4.ToString() + ":443");//server IP and port
                    field_ServicePoint_ProxyServicePoint.SetValue(req.ServicePoint, false);
                    req.Referer = "https://" + FarmEndpoint;
                    req.Headers.Add("Name", "https://" + FarmEndpoint + "/adfs/services/trust/2005/usernamemixed");
                    req.Method = "POST";
                    req.ContentType = "application/soap+xml";
                    //req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                    req.KeepAlive = true;
                    req.AllowAutoRedirect = false;
                    byte[] data = Encoding.UTF8.GetBytes(RST);
                    req.ContentLength = data.Length;

                    try
                    {
                        Stream dataStream = req.GetRequestStream();
                        dataStream.Write(data, 0, data.Length);
                        dataStream.Close();
                        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                        byte[] result = null;
                        int byteCount = Convert.ToInt32(resp.ContentLength);
                        using (BinaryReader reader = new BinaryReader(resp.GetResponseStream()))
                        {
                            result = reader.ReadBytes(byteCount);
                            row["Target Identifier"] = DropDownList1.SelectedItem.Value;
                            //Dirt way to extract claims
                            string claims = System.Text.Encoding.UTF8.GetString(result);
                            XmlDocument xmltest = new XmlDocument();
                            xmltest.LoadXml(claims);
                            string xmltoken = xmltest.InnerText;
                            if (xmltoken.Contains("SAML:1"))
                            {
                                int first = xmltoken.IndexOf("tc:SAML:1.0:cm");
                                int last = xmltoken.LastIndexOf("tc:SAML:1.0:cm");
                                string Finaltoken = xmltoken.Substring(first, last - first);
                                row["TokenResponse"] = Finaltoken;
                            }
                            else
                            {
                                row["TokenResponse"] = xmltoken;
                            }
                        }
                    }
                    catch (Exception TokenExp)
                    {
                        row["Target Identifier"] = DropDownList1.SelectedItem.Value;
                        row["TokenResponse"] = TokenExp.Message.ToString();
                    }
                }
                TokenCheckTable.Rows.Add(row);
            }
            GridView7.DataSource = TokenCheckTable;
            GridView7.DataBind();
        }
        else
        {
            Label4.Text = "You have not selected an RP Identifier.";
            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView7.DataSource = null;
            GridView7.DataBind();
        }
    }
}
