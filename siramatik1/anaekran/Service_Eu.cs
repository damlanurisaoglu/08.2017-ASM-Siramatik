using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//using System.Web.Configuration;

namespace anaekran
{
    public class Service_Eu
    { 

        public string GetPath_server_String()
        {
            return ConfigurationManager.ConnectionStrings["Path_server"].ConnectionString.ToString();
        }
        public SqlDataReader Select_AYARLAR_DataReader()
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("select * from  AYARLAR"));
            return dr; //products.ToString();
        } 
        #region DataRader 
        public SqlDataReader Select_Cabin_info_Connect_Adress_DataReader(string Cabin_Name_)
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("EXEC Sp_001_Select_Cabin_info_Connect_Adress '{0}'", Cabin_Name_));
            return dr; //products.ToString();
        }

        public string Select_Connected_II_String(string Ref)
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("EXEC Sp_001_Select_Connected_II '{0}'", Ref));
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    return dr["Call_Number"].ToString();
                }
            }
            else
            {

            }
            return "";
        }
        #endregion

        #region DataRader_Update_Insert

        public string insert_Update_Connected_DataReader(string Ref, string Convict_Ref, string Name_SurName, string Connected_Status, int Adress_Type, string Adress_, string Notes_, string Call_Number, string Call_Pass)
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("EXEC Sp_001_insert_Update_Connected '{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}'", Ref, Convict_Ref, Name_SurName, Connected_Status, Adress_Type, Adress_, Notes_, Call_Number, Call_Pass));
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr.GetString(0) == "1")
                    {
                        return "OK";
                    }
                    else
                    {
                        return "Hata Oluştu.";
                    }

                }
            }
            else
            {
                return "Hata Oluştu.";
            }
            return "Hata Oluştu.";
        } 

        #endregion

        #region DataSet_Update_Insert     

        public DataSet DataSet_imageTemp_select(string User_Ref, string ilan_Ref)
        {
            DataSet ds = anaekran.Db_Connect.Sql_DataSet(string.Format("EXEC sp_001_imageTemp_select '{0}','{1}'", User_Ref, ilan_Ref));
            return ds; //products.ToString();
        }
        #endregion

        #region DataTable_Update_Insert    
        public SqlDataReader  Kayitlar_insert_DataReader(int HastaID, int DoktorID, int SiraNo)
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("EXEC  sp_001_tblKayitlar_insert {0},{1},{2}", HastaID, DoktorID, SiraNo));
            return dr; //products.ToString();
        }
        public DataTable Select_Login_DataTable(string UserName,string Pass, int Type_)
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("EXEC sp_001_tblLogin_Select '{0}','{1}',{2}", UserName,Pass, Type_));
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt; //products.ToString();
        }
      
        public DataTable Select_Hastalar_DataTable(string TcKimlik)
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("EXEC  sp_001_tblHastalar_Select '{0}'", TcKimlik));
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt; //products.ToString();
        }
        public DataTable Select_Kayıtlar_DataTable(int DoktorID)
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("EXEC  sp_001_tblKayıtlar_Select {0}", DoktorID));
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt; //products.ToString();
        }
        public DataTable Select_Doktorlar_DataTable(int KullaniciID)
        {
            SqlDataReader dr = anaekran.Db_Connect.Sql_DataReader(string.Format("EXEC sp_001_tblDoktorlar_Select {0}", KullaniciID));
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt; //products.ToString();
        }
        #endregion
    }
}