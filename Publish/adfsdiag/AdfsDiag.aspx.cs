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
using System.ServiceModel.Channels;
//using Microsoft.IdentityModel.Protocols.WSTrust.WSTrustChannelFactory;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using System.IdentityModel.Claims;
//using System.ServiceModel.Security.WSTrustChannelFactory;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Web;
using System.Runtime.InteropServices;
using System.Management.Automation.Remoting;
using System.Xml.Serialization;
using System.Web.UI.WebControls.WebParts;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //colour grid view
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

    protected void OnServiceStatus(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if ((drv["ADFS Service Status"].ToString().Contains("Running")) && (drv["SQL Service Status"].ToString().Contains("Running")) && (drv["SQL Agent Service Status"].ToString().Contains("Running")))
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

    protected void OnADFSTokenTestBound(object sender, GridViewRowEventArgs e)
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
    //colour grid view end.

    protected void GetADFS_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        CheckBoxList1.Items.Clear();
        string SubscriberServerName = TextBox1.Text;
        string SubConnectionString = "server=" + SubscriberServerName + ";database=AdfsConfigurationV3;Integrated Security=sspi";
        SqlConnection SubConnection = new SqlConnection(SubConnectionString);
        try
        {
            SubConnection.Open();
            SqlDataReader SubReader = null;
            SqlCommand SubCommand = new SqlCommand("Select * From dbo.sysmergesubscriptions", SubConnection); // here 
            SubReader = SubCommand.ExecuteReader();
            while (SubReader.Read())
            {
                ListItem item = new ListItem();
                item.Text = SubReader["Subscriber_Server"].ToString();
                CheckBoxList1.Items.Add(item);
            }

        }
        catch (Exception SubConnectionError)
        {
            Label1.Text = SubConnectionError.Message.ToString();
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        SubConnection.Close();
    }//Get ADFS button end.


    protected void MetadataCheck_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label1.Text = "Please select ADFS servers to view Metadata response.";
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
        else
        {
            AdfsSqlHelper stsName = new AdfsSqlHelper();
            string sts = stsName.GetFarmName();
            var SelectedADFS = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable MetaCheckTable = new DataTable();
            MetaCheckTable.Columns.Add("ADFS Server Name");
            MetaCheckTable.Columns.Add("IP");
            MetaCheckTable.Columns.Add("Response URL");
            MetaCheckTable.Columns.Add("Response");

            foreach (object ADFS in SelectedADFS)
            {
                IPAddress[] ipaddress = Dns.GetHostAddresses(ADFS.ToString());
                DataRow row = MetaCheckTable.NewRow();
                row["ADFS Server Name"] = ADFS.ToString();
                foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    row["IP"] = (ip4.ToString());
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://" + sts + "/federationmetadata/2007-06/federationmetadata.xml");
                    FieldInfo field_ServicePoint_ProxyServicePoint = (typeof(ServicePoint))
                        .GetField("m_ProxyServicePoint", BindingFlags.NonPublic | BindingFlags.Instance);
                    req.Proxy = new WebProxy(ip4.ToString() + ":443");//server IP and port
                    field_ServicePoint_ProxyServicePoint.SetValue(req.ServicePoint, false);
                    try
                    {
                        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                        row["Response URL"] = resp.ResponseUri.ToString();
                        row["Response"] = resp.StatusCode.ToString();
                    }
                    catch (Exception MetadataExp)
                    {
                        row["Response URL"] = MetadataExp.Message.ToString();
                        row["Response"] = MetadataExp.Message.ToString();
                    }
                }
                MetaCheckTable.Rows.Add(row);
            }
            GridView1.DataSource = MetaCheckTable;
            GridView1.DataBind();
        }
    }
    protected void GetService_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label1.Text = "Please select ADFS servers to view health.";
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            Label1.Text = "";
            var selecteditems = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable GetSvcTable = new DataTable();
            GetSvcTable.Columns.Add("Server Name");
            GetSvcTable.Columns.Add("ADFS Service Status");
            GetSvcTable.Columns.Add("SQL Service Status");
            GetSvcTable.Columns.Add("SQL Agent Service Status");

            foreach (object Server in selecteditems)
            {
                ServiceController ADFS = new ServiceController("adfssrv", Server.ToString());
                ServiceController SQL = new ServiceController("MSSQLSERVER", Server.ToString());
                ServiceController SQLAgent = new ServiceController("SQLSERVERAGENT", Server.ToString());
                ADFS.ServiceName = "adfssrv";
                SQL.ServiceName = "MSSQLSERVER";
                SQLAgent.ServiceName = "SQLSERVERAGENT";

                DataRow row = GetSvcTable.NewRow();
                //Get ADFS Service
                try
                {
                    row["Server Name"] = Server.ToString();
                    row["ADFS Service Status"] = ADFS.Status.ToString();

                }
                catch (Exception GetADFSServiceError)
                {
                    row["Server Name"] = Server.ToString();
                    row["ADFS Service Status"] = GetADFSServiceError.InnerException.ToString();
                }
                //GET SQL Service
                try
                {
                    row["Server Name"] = Server.ToString();
                    row["SQL Service Status"] = SQL.Status.ToString();
                }
                catch (Exception GetSQLServiceError)
                {
                    row["Server Name"] = Server.ToString();
                    row["SQL Service Status"] = GetSQLServiceError.InnerException.ToString();
                }
                //Get SQL Agent Service
                try
                {
                    row["Server Name"] = Server.ToString();
                    row["SQL Agent Service Status"] = SQLAgent.Status.ToString();
                }
                catch (Exception GetSQLAgentServiceError)
                {
                    row["Server Name"] = Server.ToString();
                    row["SQL Agent Service Status"] = GetSQLAgentServiceError.InnerException.ToString();
                }
                GetSvcTable.Rows.Add(row);
            } //Main foreach
            GridView2.DataSource = GetSvcTable;
            GridView2.DataBind();
        } //Else end.
    }//Get service or Get health end.

    protected void PortChecks_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label1.Text = "Please select ADFS servers to view Port status.";
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            var SelectedADFS = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable PortConTable = new DataTable();
            PortConTable.Columns.Add("ADFS Server Name");
            PortConTable.Columns.Add("Port 443");
            PortConTable.Columns.Add("Port 49443");

            foreach (object WapSrvr in SelectedADFS)
            {
                DataRow row = PortConTable.NewRow();
                try
                {
                    System.Net.Sockets.TcpClient client443 = new TcpClient(WapSrvr.ToString(), 443);
                    row["ADFS Server Name"] = WapSrvr.ToString();
                    row["Port 443"] = client443.Connected.ToString();
                    client443.Close();
                }
                catch (System.Net.Sockets.SocketException Port443ConExp)
                {
                    row["ADFS Server Name"] = WapSrvr.ToString();
                    row["Port 443"] = Port443ConExp.Message;
                }
                try
                {
                    System.Net.Sockets.TcpClient client49443 = new TcpClient(WapSrvr.ToString(), 49443);
                    row["ADFS Server Name"] = WapSrvr.ToString();
                    row["Port 49443"] = client49443.Connected.ToString();
                    client49443.Close();
                }
                catch (Exception Port49443ConExp)
                {
                    row["ADFS Server Name"] = WapSrvr.ToString();
                    row["Port 49443"] = Port49443ConExp.Message;
                }
                PortConTable.Rows.Add(row);
            }
            GridView3.DataSource = PortConTable;
            GridView3.DataBind();

        } // Else end.
    }//ADFS port check end.


    protected void CertCheck_Click(object sender, EventArgs e)
    {
        Label2.Text = "";
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label1.Text = "Please select ADFS servers to view Cert status.";
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView4.DataSource = null;
            GridView4.DataBind();
        }
        else
        {
            var SelectedADFS = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            Label2.Text = "";
            //Label3.Text = "";
            //Table 1
            DataTable BadCertTable = new DataTable();
            BadCertTable.Columns.Add("ADFS Server Name");
            BadCertTable.Columns.Add("Non Self-Signed cert in Root");
            BadCertTable.Columns.Add("Issuer");
            BadCertTable.Columns.Add("Thumbprint");
            BadCertTable.Columns.Add("Status");
            //Table 2
            DataTable ExpTable = new DataTable();
            ExpTable.Columns.Add("ADFS Server Name");
            ExpTable.Columns.Add("Non Self-Signed cert in Root");
            ExpTable.Columns.Add("Issuer");
            ExpTable.Columns.Add("Thumbprint");
            ExpTable.Columns.Add("Status");

            //Check if non Self-Signed Certificates exist in Root Store          
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddCommand("invoke-command");
                PowerShellInstance.AddParameter("ComputerName", SelectedADFS);
                ScriptBlock filterCert = ScriptBlock.Create("Get-ChildItem cert:\\localmachine\\Root |?{$_.Issuer -ne $_.Subject} |select Thumbprint, Subject, Issuer");
                PowerShellInstance.AddParameter("ScriptBlock", filterCert);
                Collection<PSObject> NonSelfCertInRoot = PowerShellInstance.Invoke();

                //Build bad cert table
                foreach (PSObject Cert in NonSelfCertInRoot)
                {
                    if (Cert != null)
                    {
                        DataRow row = BadCertTable.NewRow();
                        row["ADFS Server Name"] = (Cert.Members["PSComputerName"].Value).ToString();
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
                    Exprow["ADFS Server Name"] = errorResult;
                    Exprow["Non Self-Signed cert in Root"] = "error";
                    Exprow["Issuer"] = "error";
                    Exprow["Thumbprint"] = "error";
                    Exprow["Status"] = "Could not execute command";
                    ExpTable.Rows.Add(Exprow);
                }

            } //PS Instance End
            BadCertTable.Merge(ExpTable);
            BadCertTable.AcceptChanges();
            GridView4.DataSource = BadCertTable;
            GridView4.DataBind();

            if (GridView4.Rows.Count > 0)
            {
                Label2.Text = "Either incorrect certificates were found Or query failed on some servers. Please check the report below for deails.";
                Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
            else
            {
                Label2.Text = "All servers are clean.";
                Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#006400");
            }

        }// else end
    }//Cert check end.


    protected void GetADFSForToken_Click(object sender, EventArgs e)
    {
        Label3.Text = "";
        CheckBoxList2.Items.Clear();
        string SubscriberServerName = TextBox2.Text;
        string SubConnectionString = "server=" + SubscriberServerName + ";database=AdfsConfigurationV3;Integrated Security=sspi";
        SqlConnection SubConnection = new SqlConnection(SubConnectionString);
        try
        {
            SubConnection.Open();
            SqlDataReader SubReader = null;
            SqlCommand SubCommand = new SqlCommand("Select * From dbo.sysmergesubscriptions", SubConnection); // here 
            SubReader = SubCommand.ExecuteReader();
            while (SubReader.Read())
            {
                ListItem item = new ListItem();
                item.Text = SubReader["Subscriber_Server"].ToString();
                CheckBoxList2.Items.Add(item);
            }

        }
        catch (Exception SubConnectionError)
        {
            Label3.Text = SubConnectionError.Message.ToString();
            Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        SubConnection.Close();
    }//GetADFSForToken_Click end.


    protected void SelectRP_Click(object sender, EventArgs e)
    {
        DropDownList1.Items.Clear();
        Label3.Text = "";
        GridView5.DataSource = null;
        GridView5.DataBind();
        try
        {
            System.Net.Sockets.TcpClient client = new TcpClient(TextBox2.Text, 5985);
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddCommand("invoke-command");
                PowerShellInstance.AddParameter("ComputerName", TextBox2.Text);
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
                        Label3.Text = "Default selected RP Identifier " + DropDownList1.SelectedItem.Text;
                        Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#008000");
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
                    Label3.Text = errorResult;
                    Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                }

            } // PS End
        }
        catch (Exception ExSelRP)
        {
            Label3.Text = ExSelRP.Message;
            Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
    }//SelectRP_Click end.


    protected void GetTokenAsSystem_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList2.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label3.Text = "You have not selected an ADFS server. Please select ADFS servers to view Token response.";
            Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView5.DataSource = null;
            GridView5.DataBind();
        }
        if (DropDownList1.SelectedItem != null)
        {
            AdfsSqlHelper stsName = new AdfsSqlHelper();
            string FarmEndpoint = stsName.GetFarmName();

            var SelectedADFS = CheckBoxList2.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable TokenCheckTable = new DataTable();
            TokenCheckTable.Columns.Add("ADFS Server Name");
            TokenCheckTable.Columns.Add("IP");
            TokenCheckTable.Columns.Add("Target Identifier");
            TokenCheckTable.Columns.Add("TokenResponse");

            foreach (object ADFS in SelectedADFS)
            {
                IPAddress[] ipaddress = Dns.GetHostAddresses(ADFS.ToString());
                DataRow row = TokenCheckTable.NewRow();
                row["ADFS Server Name"] = ADFS.ToString();
                foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    row["IP"] = (ip4.ToString());
                    String appliesTo = DropDownList1.SelectedItem.Text;
                    String federationServer = ip4.ToString();
                    String endpoint = "https://" + federationServer + "/adfs/services/trust/2005/windowstransport";

                    //Crafted RST. Do not alter.
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
                            //Dirt way to extract claims
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
            GridView5.DataSource = TokenCheckTable;
            GridView5.DataBind();
        }
        else
        {
            Label3.Text = "You have not selected an RP Identifier.";
            Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView5.DataSource = null;
            GridView5.DataBind();
        }
    }//GetTokenAsSystem_Click end.


    protected void GetTokenAsUser_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList2.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label3.Text = "You have not selected a ADFS server. Please select ADFS servers to view Token response.";
            Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView6.DataSource = null;
            GridView6.DataBind();
        }
        if (DropDownList1.SelectedItem != null)
        {
            AdfsSqlHelper stsName = new AdfsSqlHelper();
            string FarmEndpoint = stsName.GetFarmName();

            var SelectedADFS = CheckBoxList2.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable TokenCheckTable = new DataTable();
            TokenCheckTable.Columns.Add("ADFS Server Name");
            TokenCheckTable.Columns.Add("IP");
            TokenCheckTable.Columns.Add("Target Identifier");
            TokenCheckTable.Columns.Add("TokenResponse");

            foreach (object ADFS in SelectedADFS)
            {
                IPAddress[] ipaddress = Dns.GetHostAddresses(ADFS.ToString());
                DataRow row = TokenCheckTable.NewRow();
                row["ADFS Server Name"] = ADFS.ToString();
                foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    row["IP"] = (ip4.ToString());
                    String appliesTo = DropDownList1.SelectedItem.Text;
                    String federationServer = ip4.ToString();
                    String endpoint = "https://" + federationServer + "/adfs/services/trust/2005/usernamemixed";
                    DateTime now = new DateTime();
                    DateTime FinalTime = now.AddMinutes(60);
                    string username = TextBox3.Text;
                    string password = TextBox4.Text.Protect();

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
            GridView6.DataSource = TokenCheckTable;
            GridView6.DataBind();
        }
        else
        {
            Label3.Text = "You have not selected an RP Identifier.";
            Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView6.DataSource = null;
            GridView6.DataBind();
        }
    }//GetTokenAsUser_Click end.


}
