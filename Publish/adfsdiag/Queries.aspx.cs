using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
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
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Management.Automation;

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void GetAdfs_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        CheckBoxList1.Items.Clear();
        GridView1.DataSource = null;
        GridView1.DataBind();
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
    }


    protected void Query_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0 || TextBox2.Text == "")
        {
            Label1.Text = "Please select ADFS servers and provide corelationid to query events.";
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        else
        {
            Label1.Text = "";
            var SelectedADFS = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();

            string CorID = TextBox2.Text;
            string xpathFilter = "*[System/Correlation[@ActivityID='{" + CorID + "}']]";

            DataTable QueryTable = new DataTable();
            QueryTable.Columns.Add("Server");
            QueryTable.Columns.Add("EventID");
            QueryTable.Columns.Add("Message");
            QueryTable.Columns.Add("Time");

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddCommand("invoke-command");
                PowerShellInstance.AddParameter("ComputerName", SelectedADFS);
                ScriptBlock filterAllWAPDBCon = ScriptBlock.Create("Get-WinEvent -LogName 'AD FS/Admin' -FilterXPath" + " " + "\"" + xpathFilter + "\"" + " -ErrorAction SilentlyContinue");
                PowerShellInstance.AddParameter("ScriptBlock", filterAllWAPDBCon);

                foreach (PSObject outputItemDCCon in PowerShellInstance.Invoke())
                {
                    DataRow row = QueryTable.NewRow();
                    row["Server"] = (outputItemDCCon.Members["PSComputerName"].Value).ToString();
                    row["EventID"] = (outputItemDCCon.Members["Id"].Value).ToString();
                    row["Message"] = (outputItemDCCon.Members["Message"].Value).ToString();
                    row["Time"] = (outputItemDCCon.Members["TimeCreated"].Value).ToString();
                    QueryTable.Rows.Add(row);
                }

                // if error was here
                if (PowerShellInstance.Streams.Error.Count > 0)
                {
                    DataRow row = QueryTable.NewRow();
                    var errors = PowerShellInstance.Streams.Error;
                    var PS = PowerShellInstance;
                    var sb = new StringBuilder();
                    foreach (var error in errors)
                    {
                        sb.Append(error); // changed here
                    }
                    string errorResult = sb.ToString();
                    //Label7.Text = errorResult;
                    row["Server"] = errorResult;
                    row["EventID"] = "N/A";
                    row["Message"] = "N/A";
                    row["Time"] = "N/A";
                    QueryTable.Rows.Add(row);
                }
            }
            GridView1.DataSource = QueryTable;
            GridView1.DataBind();

        }
    }
}
