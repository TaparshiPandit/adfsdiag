using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class About : System.Web.UI.Page
{
    //string connectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;User Instance=True";
    string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        AdfsSqlHelper stsName = new AdfsSqlHelper();
        string sts = stsName.GetFarmName();
        Label1.Text = sts;
        if (sts != "")
        {
            Label1.Text = sts;
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#006400");
        }
        else 
        {
            Label1.Text = "No farm name found. Add one.";
            Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        //
        if (!IsPostBack)
        {
            PopulateGridView();
        }
        //
    }

    protected void UpdateFarmName_Click(object sender, EventArgs e)
    {
            try
            {
                //SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;User Instance=True");
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
                con.Open();
                String SqlUpdate = "UPDATE Configuration SET FarmName= " + "'" + TextBox1.Text + "'";
                SqlCommand cmdset = new SqlCommand(SqlUpdate, con);
                var update = cmdset.ExecuteNonQuery();
                con.Close();
                Label1.Text = "Value UPDATED";
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
    }

    void PopulateGridView() //-->
    {
        DataTable dtbl = new DataTable();
        using (SqlConnection sqlcon = new SqlConnection(connectionString))
        {
            sqlcon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM vip", sqlcon);
            sqlDa.Fill(dtbl);
            //sqlcon.Close(); // --->
        }
        if (dtbl.Rows.Count > 0)
        {
            GridView2.DataSource = dtbl;
            GridView2.DataBind();
        }
        else
        {
            dtbl.Rows.Add(dtbl.NewRow());
            GridView2.DataSource = dtbl;
            GridView2.DataBind();
            GridView2.Rows[0].Cells.Clear();
            GridView2.Rows[0].Cells.Add(new TableCell());
            GridView2.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
            GridView2.Rows[0].Cells[0].Text = "No VIP info found. Click + to Add New.";
            GridView2.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("AddNew"))
            {
                if (((GridView2.FooterRow.FindControl("txtSiteFooter") as TextBox).Text.Trim()) == "") //Deny add when site is empty
                {
                    lblSuccessMessage.Text = "Site cannot be NULL value";
                    lblSuccessMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                }

                else if (((GridView2.FooterRow.FindControl("txtVIPFooter") as TextBox).Text.Trim()) == "") //Deny add when vip is empty
                {
                    lblSuccessMessage.Text = "VIP cannot be NULL value";
                    lblSuccessMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                }

                else
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        string query = "INSERT INTO vip (Site,VIP) VALUES (@Site,@VIP)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Site", (GridView2.FooterRow.FindControl("txtSiteFooter") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@VIP", (GridView2.FooterRow.FindControl("txtVIPFooter") as TextBox).Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        PopulateGridView();
                        lblSuccessMessage.Text = "New Site and VIP added";
                        lblSuccessMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#008000");
                        lblErrorMessage.Text = "";
                        sqlCon.Close();//--->>
                    }
                }

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = "";
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }

    }

    protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView2.EditIndex = e.NewEditIndex;
        PopulateGridView();
    }

    protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView2.EditIndex = -1;
        PopulateGridView();
    }

    protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if ((GridView2.Rows[e.RowIndex].FindControl("txtSite") as TextBox).Text.Trim() == "") //Add to main --->
        {
            lblSuccessMessage.Text = "Null Site cannot be updated";
            lblSuccessMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        else if ((GridView2.Rows[e.RowIndex].FindControl("txtVIP") as TextBox).Text.Trim() == "") //Add to main --->
        {
            lblSuccessMessage.Text = "Null VIP cannot be updated";
            lblSuccessMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }

        else
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE vip SET Site=@Site,VIP=@VIP WHERE id = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Site", (GridView2.Rows[e.RowIndex].FindControl("txtSite") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@VIP", (GridView2.Rows[e.RowIndex].FindControl("txtVIP") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    GridView2.EditIndex = -1;
                    PopulateGridView();
                    lblSuccessMessage.Text = "Selected Site and VIP updated";
                    lblSuccessMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#008000");
                    lblErrorMessage.Text = "";
                    sqlCon.Close();//--->
                }

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
        }

    }

    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM vip WHERE id = @id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Value.ToString()));
                sqlCmd.ExecuteNonQuery();
                PopulateGridView();
                lblSuccessMessage.Text = "Selected VIP deleted";
                lblSuccessMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF4500");
                lblErrorMessage.Text = "";
                sqlCon.Close();
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = "";
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
    }

    
}
