using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

using System.Configuration;
using System.Text;
using System.IO;
namespace anaekran
{
    public class Db_Connect
    {
        private string Data_Source = "";
        private string Initial_Catalog = "";
        private string User_ID = "";
        private string Password = "";
        public static string CONN_String = ConfigurationManager.ConnectionStrings["baglan"].ConnectionString.ToString();
        public static string GetConnectionString()
        {  return ConfigurationManager.ConnectionStrings["baglan"].ConnectionString.ToString(); ;
        }
        public static void Sql_Command(string queryString)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static DataTable Sql_DataTable(string queryString)
        {
            SqlConnection.ClearAllPools();
            string query = queryString;

            SqlConnection sqlConn = new SqlConnection(GetConnectionString());
            sqlConn.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            sqlConn.Close();
            return dt;
        }

       public static SqlDataReader Sql_DataReader(string queryString)
        {
            SqlConnection.ClearAllPools();
        SqlDataReader dr = null;
        SqlConnection conn = new SqlConnection(GetConnectionString());

        SqlCommand cmd = new SqlCommand(queryString, conn);
        conn.Open();

        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
           return dr;
        }

       public static DataSet Sql_DataSet(string queryString)
       {
            SqlConnection.ClearAllPools();
            DataSet dataset = new DataSet();
           using (SqlConnection connection =
               new SqlConnection(GetConnectionString()))
           {
                SqlDataAdapter adapter = new SqlDataAdapter() { SelectCommand = new SqlCommand(queryString, connection) };
                adapter.Fill(dataset);
               return dataset;
           }
       }

       private void Xml_Read()
       { 
           XmlDocument doc = new XmlDocument();
           doc.Load("Settings.xml");

           XmlNodeList nodes = doc.DocumentElement.SelectNodes("/DBSettings/DBSetting"); 
           foreach (XmlNode node in nodes)
           {
               Data_Source = node.SelectSingleNode("Data_Source").InnerText;
               Initial_Catalog =  node.SelectSingleNode("Initial_Catalog").InnerText;
               User_ID = node.SelectSingleNode("User_ID").InnerText;
               Password = node.SelectSingleNode("Password").InnerText; 
           }  
       }
    }
}