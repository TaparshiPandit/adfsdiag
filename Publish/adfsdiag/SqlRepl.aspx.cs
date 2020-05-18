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

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //Colour Gridview.
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if ((drv["Status"].ToString().Equals("In progress/Synchronizing")) || (drv["Status"].ToString().Equals("Waiting 60 second(s) before polling for further changes.")) || (drv["Status"].ToString().Equals("Completed")))
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

    protected void Publications_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        Label2.Text = "";
        TextBox2.Text = "";
        string DiscoveryServer = TextBox1.Text;
        string DiscoveryConnection = "server=" + DiscoveryServer + ";database=AdfsConfigurationV3;Integrated Security=sspi";
        SqlConnection DiskConnection = new SqlConnection(DiscoveryConnection);
        try
        {
            DiskConnection.Open();
            SqlDataReader DiskReader = null;
            SqlCommand SubCommand = new SqlCommand("Select * From dbo.sysmergesubscriptions where subscriber_type = 1", DiskConnection);
            DiskReader = SubCommand.ExecuteReader();
            while (DiskReader.Read())
            {
                TextBox2.Text = DiskReader["Subscriber_Server"].ToString();
            }
        }

        catch (SqlException ex) //
        {
            Label1.Text = ex.State.ToString();
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        DiskConnection.Close();

        string PublisherServerName = TextBox2.Text;
        string ConnectionString = "server=" + PublisherServerName + ";database=distribution;Integrated Security=sspi";
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        try
        {
            myConnection.Open();
            Label1.Text = myConnection.State.ToString();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(@"EXEC sp_replmonitorhelppublication", myConnection);

            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                //Section for Publication Information
                DataTable tempTablePublicationInfo = new DataTable();
                tempTablePublicationInfo.Columns.Add("Publisher");
                tempTablePublicationInfo.Columns.Add("Status");
                tempTablePublicationInfo.Columns.Add("PublisherDatabase");
                tempTablePublicationInfo.Columns.Add("PublicationName");
                DataRow rowPublicationInfo = tempTablePublicationInfo.NewRow();

                rowPublicationInfo["Publisher"] = myReader["publisher"].ToString();
                string PublicationStatus = myReader["status"].ToString();
                switch (PublicationStatus)
                {
                    case "6":
                        rowPublicationInfo["Status"] = "Failed/Error";
                        break;
                    case "5":
                        rowPublicationInfo["Status"] = "Retrying";
                        break;
                    case "4":
                        rowPublicationInfo["Status"] = "Idle";
                        break;
                    case "3":
                        rowPublicationInfo["Status"] = "In progress/Synchronizing";
                        break;
                    case "2":
                        rowPublicationInfo["Status"] = "Succeeded";
                        break;
                    case "1":
                        rowPublicationInfo["Status"] = "Started";
                        break;
                    default:
                        break;
                }
                rowPublicationInfo["PublisherDatabase"] = myReader["publisher_db"].ToString();
                rowPublicationInfo["PublicationName"] = myReader["publication"].ToString();
                tempTablePublicationInfo.Rows.Add(rowPublicationInfo);
                GridView1.DataSource = tempTablePublicationInfo;
                GridView1.DataBind();
            }
            myConnection.Close();
            Label1.Text = myConnection.State.ToString();
        }
        catch (SqlException myConnectionError)
        {
            Label2.Text = myConnectionError.Message.ToString();
            Label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
    }//Button Publiaction end.

    protected void SubscriptionWatchList_Click(object sender, EventArgs e)
    {
        Label3.Text = "";
        if (TextBox2.Text == "")
        {
            Label3.Text = "Please provive the name of a valid SQL server in the farm above and click Publications to view Subscriptions.";
            Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            string PublisherServerName = TextBox2.Text;
            string ConnectionString = "server=" + PublisherServerName + ";database=distribution;Integrated Security=sspi";
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                Label1.Text = "Open";
                SqlDataReader myReader = null;
                //SqlDataReader myReader2 = null;
                SqlCommand myCommand = new SqlCommand(@"
                EXEC sp_replmonitorhelpsubscription @publisher = NULL, @publication_type = 2", myConnection);
                myReader = myCommand.ExecuteReader();
                DataTable tempTable = new DataTable();
                tempTable.Columns.Add("Subscriber");
                tempTable.Columns.Add("Status");
                tempTable.Columns.Add("Connection");
                tempTable.Columns.Add("Delivery Rate");
                tempTable.Columns.Add("Duration");
                tempTable.Columns.Add("Performance");

                while (myReader.Read())
                {
                    if (myReader.HasRows)
                    {
                        DataRow row = tempTable.NewRow();
                        row["Subscriber"] = myReader["Subscriber"].ToString();
                        string ReplicationStatus = myReader["Status"].ToString();
                        switch (ReplicationStatus)
                        {
                            case "6":
                                row["Status"] = "Failed/Error";
                                //GridView1.ForeColor = System.Drawing.Color.Red;
                                break;
                            case "5":
                                row["Status"] = "Retrying";
                                break;
                            case "2":
                                row["Status"] = "Stopped";
                                break;
                            case "4":
                                row["Status"] = "Idle";
                                break;
                            case "3":
                                row["Status"] = "In progress/Synchronizing";
                                break;
                            case "1":
                                row["Status"] = "Started";
                                break;
                            default:
                                break;
                        }
                        string ConnectionType = myReader["mergeconnectiontype"].ToString();
                        switch (ConnectionType)
                        {
                            case "1":
                                row["Connection"] = "LAN";
                                break;
                            case "2":
                                row["Connection"] = "dial-up network connection";
                                break;
                            case "3":
                                row["Connection"] = "Web synchronization";
                                break;
                            default:
                                break;
                        }
                        row["Delivery Rate"] = myReader["mergerunspeed"].ToString() + " Rows/Sec";
                        row["Duration"] = myReader["mergerunduration"].ToString();
                        row["Performance"] = myReader["mergePerformance"].ToString();
                        tempTable.Rows.Add(row);
                    }
                }//end while
                GridView2.DataSource = tempTable;
                GridView2.DataBind();
                myConnection.Close();
                Label1.Text = "Closed";
            }
            catch (Exception myConnectionError)
            {
                Label3.Text = myConnectionError.Message.ToString();
                Label3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
        }
    }//Subscription watch list button end.

    protected void Agents_Click(object sender, EventArgs e)
    {
        Label4.Text = "";
        if (TextBox2.Text == "")
        {
            Label4.Text = "Please provive the name of a valid SQL server in the farm above and click Publications to view Agent status.";
            Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            string PublisherServerName = TextBox2.Text;
            string ConnectionString = "server=" + PublisherServerName + ";database=distribution;Integrated Security=sspi";

            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                Label1.Text = "Open";
                SqlDataReader myReader = null;
                //SqlDataReader myReader2 = null;
                SqlCommand myCommand = new SqlCommand(@"
                SELECT * FROM dbo.MSsnapshot_history WHERE time=(SELECT max(time) FROM dbo.MSsnapshot_history)", myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    //Section Agent info
                    DataTable tempTableAgentInfo = new DataTable();
                    tempTableAgentInfo.Columns.Add("Status");
                    tempTableAgentInfo.Columns.Add("Last Action");
                    DataRow rowAgentInfo = tempTableAgentInfo.NewRow();
                    //rowAgentInfo["Status"] = myReader["runstatus"].ToString();
                    string AgentStatus = myReader["runstatus"].ToString();
                    switch (AgentStatus)
                    {
                        case "6":
                            rowAgentInfo["Status"] = "Failed/Error";
                            break;
                        case "5":
                            rowAgentInfo["Status"] = "Retrying";
                            break;
                        case "4":
                            rowAgentInfo["Status"] = "Idle";
                            break;
                        case "3":
                            rowAgentInfo["Status"] = "Running";
                            break;
                        case "2":
                            rowAgentInfo["Status"] = "Completed";
                            break;
                        case "1":
                            rowAgentInfo["Status"] = "Started";
                            break;
                        default:
                            break;
                    }
                    rowAgentInfo["Last Action"] = myReader["comments"].ToString();

                    tempTableAgentInfo.Rows.Add(rowAgentInfo);
                    GridView3.DataSource = tempTableAgentInfo;
                    GridView3.DataBind();
                }

                myConnection.Close();
                Label1.Text = "Closed";
            }
            catch (Exception myConnectionError)
            {
                //Label1.Text = myConnectionError.ToString();
                Label4.Text = myConnectionError.Message.ToString();
                Label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");

            }
        }
    }//Agents button end.

    protected void GetSubscribers_Click(object sender, EventArgs e)
    {
        CheckBoxList1.Items.Clear();
        Label5.Text = "";
        if (TextBox2.Text == "")
        {
            Label5.Text = "Please provive the name of a valid SQL server in the farm above and click Publications to get a list of Subscribers";
            Label5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            string DiscoveryServer = TextBox2.Text;
            string DiscoveryConnection = "server=" + DiscoveryServer + ";database=AdfsConfigurationV3;Integrated Security=sspi";
            SqlConnection DiskConnection = new SqlConnection(DiscoveryConnection);

            try
            {
                DiskConnection.Open();
                SqlDataReader DiskReader = null;
                SqlCommand SubCommand = new SqlCommand("Select * From dbo.sysmergesubscriptions where subscriber_type = 2", DiskConnection);
                DiskReader = SubCommand.ExecuteReader();
                while (DiskReader.Read())
                {
                    //Populate checkboxlist2 here
                    //TextBox1.Text = DiskReader["Subscriber_Server"].ToString();
                    ListItem item = new ListItem();
                    item.Text = DiskReader["Subscriber_Server"].ToString();
                    CheckBoxList1.Items.Add(item);
                }
            }
            catch (Exception DiskConnectionError)
            {
                Label5.Text = DiskConnectionError.Message.ToString();
                Label5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
            DiskConnection.Close();
        }
    }//Get Subscribers end.

    protected void ViewSyncStatus_Click(object sender, EventArgs e)
    {
        if ((CheckBoxList1.Items.Cast<ListItem>().Count(li => li.Selected)) == 0)
        {
            Label5.Text = "Please select Subscriber servers to view synchronization status";
            Label5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else
        {
            Label5.Text = "";
            var selecteditems = CheckBoxList1.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.ToString()).ToArray();
            DataTable SubtempTable = new DataTable();
            SubtempTable.Columns.Add("Subscriber");
            SubtempTable.Columns.Add("Status");

            foreach (object SubscriberServerName in selecteditems)
            {
                string SubConnectionString = "server=" + SubscriberServerName + ";database=AdfsConfigurationV3;Integrated Security=sspi";
                SqlConnection SubConnection = new SqlConnection(SubConnectionString);
                DataRow row = SubtempTable.NewRow();
                try
                {
                    SubConnection.Open();
                    SqlDataReader SubReader = null;
                    SqlCommand SubCommand = new SqlCommand("SELECT * FROM MSmerge_history WHERE timestamp=(SELECT max(timestamp) FROM MSmerge_history);", SubConnection);
                    SubReader = SubCommand.ExecuteReader();
                    while (SubReader.Read())
                    {
                        row["Status"] = SubReader["comments"].ToString();
                        row["Subscriber"] = SubscriberServerName;
                    }
                }
                catch (SqlException SubConnectionError)
                {
                    row["Subscriber"] = SubscriberServerName;
                    row["Status"] = SubConnectionError.Message.ToString();
                }

                SubtempTable.Rows.Add(row);
                SubConnection.Close();

            } //end foreach
            GridView4.DataSource = SubtempTable;
            GridView4.DataBind();
        }
    }
}
