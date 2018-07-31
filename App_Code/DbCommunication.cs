using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for DbCommunication
/// </summary>
public class DbCommunication
{
    public DbCommunication()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// This method return sql connection
    /// </summary>
    /// <returns></returns>
    public SqlConnection getConnection()
    {
        return  new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnection"].ToString());
    }

    /// <summary>
    /// This method return DataTable from Query
    /// </summary>
    /// <param name="strQuery"></param>
    /// <returns></returns>
    public DataTable GetDataTable(string strQuery)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection conDt = getConnection())
            {
                using (SqlDataAdapter da = new SqlDataAdapter(strQuery, conDt))
                {
                    da.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            LogWrite("GetDataTable " + ex.Message.ToString());
        }
        return dt;
    }

    /// <summary>
    /// This Method Execute Query
    /// </summary>
    /// <param name="strQuery"></param>
    /// <returns></returns>
    public int ExecuteQuery(string strQuery)
    {
        int i = 0;
        try
        {
            using (SqlConnection conQ = getConnection())
            {
                conQ.Open();
                using (SqlCommand cmd = new SqlCommand(strQuery, conQ))
                {
                    i = cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            i = 0;
            LogWrite("ExecuteQuery " + ex.Message.ToString());
        }
        return i;
    }

    /// <summary>
    /// This Method Return List of Pending Transaction
    /// </summary>
    /// <returns></returns>
    public DataTable GetPendingTransaction()
    {
        DataTable dt = new DataTable();
        try
        {
            string query = "select TxnId from PayuRequestLog where Status!='SUCCESS' and cast(requestTime as date) " +
                " between cast(DATEADD (day, -2, getdate()) as date) and cast(getdate() as date)";
            dt = GetDataTable(query);
            LogWrite("Total Pending Transaction= " + dt.Rows.Count);
        }
        catch (Exception ex)
        {
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// This method is used to write log in text file
    /// </summary>
    /// <param name="strLog"></param>
    public void LogWrite(string strLog)
    {
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        try
        {
            string filepath = ConfigurationManager.AppSettings["path"];
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            filepath = filepath + "//" + DateTime.Today.ToString("yyyy");
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            filepath = filepath + "//" + DateTime.Today.ToString("MMM");
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            filepath = filepath + "//" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine("--------------------------------*Event Start*------------------------------------------");
                sw.WriteLine("-----------Event Time: " + " " + DateTime.Now.ToString() + "-----------------");
                sw.WriteLine(strLog);
                sw.WriteLine("--------------------------------*Event End*------------------------------------------");
                sw.Flush();
                sw.Close();
            }
        }
        catch
        {
        }
    }

}