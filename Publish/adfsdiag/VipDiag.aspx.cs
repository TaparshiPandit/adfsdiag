using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Management;
using System.Collections.ObjectModel;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Reflection;
using System.ServiceModel.Security;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IdentityModel.Claims;
//using System.ServiceModel.Security.WSTrustChannelFactory;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Web.UI.WebControls.WebParts;
using System.Management.Automation;

public partial class _Default : System.Web.UI.Page
{
    //SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;User Instance=True");
    SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");

    protected void Page_Load(object sender, EventArgs e)
    {
        AdfsSqlHelper stsName = new AdfsSqlHelper();
        string sts = stsName.GetFarmName();
        Label2.Text = sts;
        if (sts != "")
        {
            Label2.Text = sts;
            Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#006400");
        }
        else
        {
            Label2.Text = "No farm name found. Add a farm name under configuration.";
            Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }

        if (!Page.IsPostBack)
        {
            refreshdata();
        }
    }
    public void refreshdata()
    {
        SqlCommand cmd = new SqlCommand("select * from vip", con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        con.Close();
    }

    //Colour Grid View
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

    protected void OnVipTokenTestBound(object sender, GridViewRowEventArgs e)
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

    protected void OnvipTokenTestUserBound(object sender, GridViewRowEventArgs e)
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
    //Colour grid view end.

    protected void VipMetaCheck_Click(object sender, System.EventArgs e)
    {
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //Handle TLS.
        System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        List<string> Selectedvips = new List<string>();

        DataTable MetaCheckTable = new DataTable();
        MetaCheckTable.Columns.Add("Site Name");
        MetaCheckTable.Columns.Add("IP");
        MetaCheckTable.Columns.Add("Response URL");
        MetaCheckTable.Columns.Add("Response");

        foreach (GridViewRow gvrow in GridView1.Rows)
        {
            var checkbox = gvrow.FindControl("CheckBoxSiteVip") as CheckBox;
            if (checkbox.Checked)
            {
                Label1.Text = "";
                AdfsSqlHelper stsName = new AdfsSqlHelper();
                string FarmEndpoint = stsName.GetFarmName();

                Selectedvips.Add((gvrow.FindControl("LabelVip") as Label).Text);
                DataRow row = MetaCheckTable.NewRow();
                foreach (object vip in Selectedvips)
                {
                    row["Site Name"] = (gvrow.FindControl("LabelSite") as Label).Text;
                    row["IP"] = vip.ToString();
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://" + FarmEndpoint + "/federationmetadata/2007-06/federationmetadata.xml");//url and Host header
                    FieldInfo field_ServicePoint_ProxyServicePoint = (typeof(ServicePoint))
                        .GetField("m_ProxyServicePoint", BindingFlags.NonPublic | BindingFlags.Instance);
                    req.Proxy = new WebProxy(vip.ToString() + ":443");//server IP and port
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
            else if (Selectedvips.Count == 0)
            {
                Label1.Text = "Please select a VIP";
                Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
            
        }//foreach
        GridView2.DataSource = MetaCheckTable;
        GridView2.DataBind();
    }//VipMetaCheck_Click end.

    protected void VIPTokenCheckasSystem_Click(object sender, EventArgs e)
    {
        List<string> Selectedvips = new List<string>();
        DataTable VipTokenTable = new DataTable();
        VipTokenTable.Columns.Add("Site Name");
        VipTokenTable.Columns.Add("VIP");
        VipTokenTable.Columns.Add("TargetIdentifier");
        VipTokenTable.Columns.Add("TokenResponse");

        if (DropDownList1.SelectedItem != null)
        {
            foreach (GridViewRow gvrow in GridView1.Rows)
            {
                var checkbox = gvrow.FindControl("CheckBoxSiteVip") as CheckBox;
                if (checkbox.Checked)
                {
                    Label1.Text = "";
                    AdfsSqlHelper stsName = new AdfsSqlHelper();
                    string FarmEndpoint = stsName.GetFarmName();

                    Selectedvips.Add((gvrow.FindControl("LabelVip") as Label).Text);
                    DataRow row = VipTokenTable.NewRow();
                    foreach (object vip in Selectedvips)
                    {
                        row["Site Name"] = (gvrow.FindControl("LabelSite") as Label).Text;
                        row["VIP"] = vip.ToString();

                        String appliesTo = DropDownList1.SelectedItem.Text;
                        String federationServer = vip.ToString();
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
                        req.Proxy = new WebProxy(vip.ToString() + ":443");//server IP and port
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
                                row["TargetIdentifier"] = DropDownList1.SelectedItem.Value;
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
                            row["TargetIdentifier"] = DropDownList1.SelectedItem.Value;
                            row["TokenResponse"] = TokenExp.Message.ToString();
                        }
                    }
                    VipTokenTable.Rows.Add(row);
                }
                else if (Selectedvips.Count == 0)
                {
                    Label1.Text = "Please select a VIP";
                    Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                }

            }//foreach
            GridView3.DataSource = VipTokenTable;
            GridView3.DataBind();
        }
        else
        {
            Label3.Text = "You have not selected an RP Identifier.";
            Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView3.DataSource = null;
            GridView3.DataBind();
        }

    }//VIPTokenCheckasSystem end.

    protected void SelectRP_Click(object sender, EventArgs e)
    {
        DropDownList1.Items.Clear();
        Label3.Text = "";
        GridView3.DataSource = null;
        GridView3.DataBind();
        try
        {
            System.Net.Sockets.TcpClient client = new TcpClient(TextBox1.Text, 5985);
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddCommand("invoke-command");
                PowerShellInstance.AddParameter("ComputerName", TextBox1.Text);
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
    }

    protected void GetTokenasUser_Click(object sender, EventArgs e)
    {
        List<string> Selectedvips = new List<string>();
        DataTable VipTokenTable = new DataTable();
        VipTokenTable.Columns.Add("Site Name");
        VipTokenTable.Columns.Add("VIP");
        VipTokenTable.Columns.Add("TargetIdentifier");
        VipTokenTable.Columns.Add("TokenResponse");

        if (DropDownList1.SelectedItem != null)
        {
            foreach (GridViewRow gvrow in GridView1.Rows)
            {
                var checkbox = gvrow.FindControl("CheckBoxSiteVip") as CheckBox;
                if (checkbox.Checked)
                {
                    Label1.Text = "";
                    AdfsSqlHelper stsName = new AdfsSqlHelper();
                    string FarmEndpoint = stsName.GetFarmName();

                    Selectedvips.Add((gvrow.FindControl("LabelVip") as Label).Text);
                    DataRow row = VipTokenTable.NewRow();
                    foreach (object vip in Selectedvips)
                    {
                        row["Site Name"] = (gvrow.FindControl("LabelSite") as Label).Text;
                        row["VIP"] = vip.ToString();

                        String appliesTo = DropDownList1.SelectedItem.Text;
                        String federationServer = vip.ToString();
                        String endpoint = "https://" + federationServer + "/adfs/services/trust/2005/usernamemixed";

                        string username = TextBox2.Text;
                        string password = TextBox3.Text.Protect();

                        //Crafted RST. Do not alter.
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

                        //String ComputedRST = String.Format(RST, endpoint, appliesTo);
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://" + FarmEndpoint + "/adfs/services/trust/2005/usernamemixed");//url and Host header
                        FieldInfo field_ServicePoint_ProxyServicePoint = (typeof(ServicePoint))
                            .GetField("m_ProxyServicePoint", BindingFlags.NonPublic | BindingFlags.Instance);
                        req.Proxy = new WebProxy(vip.ToString() + ":443");//server IP and port
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
                                row["TargetIdentifier"] = DropDownList1.SelectedItem.Value;
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
                            row["TargetIdentifier"] = DropDownList1.SelectedItem.Value;
                            row["TokenResponse"] = TokenExp.Message.ToString();
                        }
                    }
                    VipTokenTable.Rows.Add(row);
                }
                else if (Selectedvips.Count == 0)
                {
                    Label1.Text = "Please select a VIP";
                    Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                }

            }//foreach
            GridView4.DataSource = VipTokenTable;
            GridView4.DataBind();
        }
        else
        {
            Label4.Text = "You have not selected an RP Identifier.";
            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView4.DataSource = null;
            GridView4.DataBind();
        }
    }
}