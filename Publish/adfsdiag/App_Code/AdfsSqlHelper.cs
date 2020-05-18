using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


/// <summary>
/// Summary description for AdfsSqlHelper
/// </summary>
public class AdfsSqlHelper
{
    public class FarmInfo
    {
        public string Farmname { get; set; }
    }
    public string GetFarmName()
    {
        string AdfsFarmName = null;
        try
        {
            //using (var connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;User Instance=True")) //with SQLExpress.
            using (var connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True"))

            {
                connection.Open();
                string sql = "SELECT TOP 1 FarmName FROM Configuration";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AdfsFarmName = reader["FarmName"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            return AdfsFarmName;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }

    /*
    public List<FarmInfo> GetFarmName()
    {
        var ListOfFarm = new List<FarmInfo>();

        using (var connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;User Instance=True"))
        {
            connection.Open();
            string sql = "SELECT * FROM Configuration";
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var FarmNameList = new FarmInfo();
                        FarmNameList.Farmname = reader["FarmName"].ToString();
                        ListOfFarm.Add(FarmNameList);
                    }
                }
            }
            connection.Close();
        }
        return ListOfFarm;
    }//GetFarmName
    */

}