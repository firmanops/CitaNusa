using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Interface;
using System.Configuration;
using Npgsql;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Weighplatation.Repository
{
    public class UserRepo : IUser
    {
        private readonly string stringCon = ConfigurationManager.ConnectionStrings["dbcon"].ToString();
      
        public BusinessUnitModel GetUnitByCode(string UnitCode)
        {
            List<BusinessUnitModel> wBTRXes = new List<BusinessUnitModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select * from public.getbusinessunit(@UnitCode)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UnitCode", UnitCode);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessUnitModel entity = new BusinessUnitModel();
                                entity.UnitCode = (string)reader["UnitCode"];
                                entity.UnitName = (string)reader["UnitName"];
                                entity.UnitType = (string)reader["UnitType"];
                                entity.Province = reader["Province"] == DBNull.Value ? "" : (string)reader["Province"];
                                entity.City = reader["City"] == DBNull.Value ? "" : (string)reader["City"];
                                entity.Address1 = reader["Address1"] == DBNull.Value ? "" : (string)reader["Address1"];
                                entity.Address2 = reader["Address2"] == DBNull.Value ? "" : (string)reader["Address2"];
                                entity.UnitType = reader["UnitType"] == DBNull.Value ? "" : (string)reader["UnitType"];
                                entity.BPCode = (string)reader["BPCode"];
                                entity.Active = (bool)reader["Active"];
                                entity.oddoid = int.Parse(reader["oddoid"].ToString());
                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes[0];
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        

        public List<SYSUSERMODEL> GetUser()
        {
            try
            {
                List<SYSUSERMODEL> entitylist = new List<SYSUSERMODEL>();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select  ""userid"",""username"",""password"" ,""groupid"" ,""unitcode"", ""active"" from public.""SYSUSER""";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;                       
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                           

                            while (reader.Read())
                            {
                                SYSUSERMODEL entity = new SYSUSERMODEL();
                                entity.userid = (string)reader["userid"];
                                entity.username = (string)reader["username"];
                                entity.password = Decrypt((string)reader["password"]);
                                entity.groupid= int.Parse(reader["groupid"].ToString());
                                entity.unitcode = (string)reader["unitcode"];
                                entity.active = (bool)reader["active"];
                                entitylist.Add(entity);

                            }
                            cmd.Dispose();
                        }
                        
                    }

                    con.Close();

                    return entitylist;
                }
            }
            catch (NpgsqlException err)
            {
                throw new NpgsqlException(err.Message);
            }
        }

        public SYSUSERMODEL GetUserLogin(string UserID, string Password)
        {
            try
            {
                SYSUSERMODEL entity = new SYSUSERMODEL();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();

                    string p_Password = Encrypt(Password);
                    string strSql = @"select  * from public.""SYSUSER"" where ""userid"" = '" + UserID + "'" + @"and ""password"" ='" + p_Password + "' ";
                  
                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {

                            reader.Read();
                            if (!reader.HasRows)
                            {
                                throw new NpgsqlException("User or Password Not Found !, Please contact your Admin");
                            }


                            entity.userid = reader["userid"] == DBNull.Value ? "" :  (string)reader["userid"];
                            entity.username = reader["username"] ==  DBNull.Value ? "" : (string)reader["username"];                          
                            entity.password= reader["password"] ==  DBNull.Value ? "" : Decrypt((string)reader["password"]);
                            entity.groupid = reader["groupid"] == DBNull.Value ? 0 :  Int32.Parse(reader["groupid"].ToString());
                            entity.unitcode = reader["unitcode"] ==  DBNull.Value ? "" : (string)reader["unitcode"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return entity;
                }
            }
            catch (NpgsqlException err)
            {
                throw new NpgsqlException(err.Message);
            }
        }

        public List<SYSUSERGROUPMENUMODEL> GetMenuUser(int groupid)
        {
            try
            {
                List<SYSUSERGROUPMENUMODEL> sYSUSERGROUPMENUMODELs = new List<SYSUSERGROUPMENUMODEL>();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select  * from public.""SYSGROUPMENU"" where ""idgroup"" = '" + groupid + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;                      
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                SYSUSERGROUPMENUMODEL entity = new SYSUSERGROUPMENUMODEL();
                                entity.id=int.Parse(reader["id"].ToString());
                                entity.idgroup = int.Parse(reader["idgroup"].ToString());
                                entity.idmenu = int.Parse(reader["idmenu"].ToString());                                
                                sYSUSERGROUPMENUMODELs.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return sYSUSERGROUPMENUMODELs;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public SYSMENU GetMenu(int id)
        {
            try
            {
                SYSMENU entity = new SYSMENU();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select  * from public.""SYSMENU"" where ""id"" = '" + id + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            entity.id = int.Parse(reader["id"].ToString());
                            entity.menuname = (string)reader["menuname"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return entity;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public List<SYSMENU> GetMenuAll()
        {
            try
            {
                List<SYSMENU> lstMenu = new List<SYSMENU>() ;
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select  ""id"", ""menuname"" from public.""SYSMENU"" order by ""id"" asc";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SYSMENU entity = new SYSMENU();
                                entity.id = int.Parse(reader["id"].ToString());
                                entity.menuname = (string)reader["menuname"];
                                lstMenu.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return lstMenu;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public bool InsertUser(SYSUSERMODEL sYSUSERMODEL)
        {
                bool result = false;
            
              
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    string strSql = @"call public.sp_insertuser(
                                    :p_userid
                                    ,:p_username
                                    ,:p_password
									,:p_groupid
									,:p_unitcode
									,:p_active
									)";
                    try
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("p_userid", DbType.String).Value = sYSUSERMODEL.userid.Trim();
                            cmd.Parameters.AddWithValue("p_username", DbType.String).Value = sYSUSERMODEL.username.Trim();
                            cmd.Parameters.AddWithValue("p_password", DbType.Byte).Value = Encrypt(sYSUSERMODEL.password);
                            cmd.Parameters.AddWithValue("p_groupid", DbType.DateTime).Value = sYSUSERMODEL.groupid;
                            cmd.Parameters.AddWithValue("p_unitcode", DbType.Date).Value = sYSUSERMODEL.unitcode;
                            cmd.Parameters.AddWithValue("p_active", DbType.String).Value = sYSUSERMODEL.active;                      
                            cmd.CommandType = CommandType.Text;
                            con.Open();
                       
                                NpgsqlTransaction trans = con.BeginTransaction();
                                cmd.Transaction = trans;
                                int affetecdCount = cmd.ExecuteNonQuery();
                                trans.Commit();
                        }
                 
                        con.Close();
                        result = true;
                   
                    }                
                    catch (NpgsqlException err)
                    {
                        throw new NpgsqlException(err.Message);
                    }
                return result;
            }
        }

        public bool UpdatetUser(SYSUSERMODEL sYSUSERMODEL)
        {
            bool result = false;


            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                con.ConnectionString = stringCon;
                string strSql = @"call public.sp_updateuser(
                                    :p_userid
                                    ,:p_username
                                    ,:p_password
									,:p_groupid
									,:p_unitcode
									,:p_active
									)";
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_userid", DbType.String).Value = sYSUSERMODEL.userid.Trim();
                        cmd.Parameters.AddWithValue("p_username", DbType.String).Value = sYSUSERMODEL.username.Trim();
                        cmd.Parameters.AddWithValue("p_password", DbType.Byte).Value = Encrypt(sYSUSERMODEL.password);
                        cmd.Parameters.AddWithValue("p_groupid", DbType.DateTime).Value = sYSUSERMODEL.groupid;
                        cmd.Parameters.AddWithValue("p_unitcode", DbType.Date).Value = sYSUSERMODEL.unitcode;
                        cmd.Parameters.AddWithValue("p_active", DbType.Boolean).Value = sYSUSERMODEL.active;
                        cmd.CommandType = CommandType.Text;
                        con.Open();

                        NpgsqlTransaction trans = con.BeginTransaction();
                        cmd.Transaction = trans;
                        int affetecdCount = cmd.ExecuteNonQuery();
                        trans.Commit();
                    }

                    con.Close();
                    result = true;

                }
                catch (NpgsqlException err)
                {
                    throw new NpgsqlException(err.Message);
                }
                return result;
            }
        }

        public DBBUSINESSPARTNER GetLogo(string UnitCode)
        {
            try
            {
                DBBUSINESSPARTNER entity = new DBBUSINESSPARTNER();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select  a.""bplogo"" from public.""BUSINESSPARTNER"" a  
                                        inner join public.""BUSINESSUNIT"" b on a.""BPCode""=b.""BPCode"" where b.""UnitCode"" = '" + UnitCode + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            entity.bplogo = reader["bplogo"] == DBNull.Value ? new byte[0] : (byte[])reader["bplogo"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return entity;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }
       
        public string Encrypt(string encryptext)
        {
            try
            {
                //string textToEncrypt = encryptext;
                string ToReturn = "";
                string publickey = "12345678";
                string secretkey = "87654321";
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(encryptext);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

           
        }
        public string Decrypt(string decryptedText)
        {
            try
            {
                //string textToDecrypt = decryptedText;
                string ToReturn = "";
                string publickey = "12345678";
                string secretkey = "87654321";
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[decryptedText.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(decryptedText.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
        private int GetSaltSize(byte[] passwordBytes)
        {
            var key = new Rfc2898DeriveBytes(passwordBytes, passwordBytes, 1000);
            byte[] ba = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToInt32(ba[i]).ToString());
            }
            int saltSize = 0;
            string s = sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize = saltSize + intc;
            }

            return saltSize;
        }
        public byte[] GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        public bool InsertGroupMenu(List<SYSUSERGROUPMENUMODEL> sYSUSERGROUPMENUMODEL)
        {
            bool result = false;

            
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                con.ConnectionString = stringCon;
               
                try
                {
                    string strSqldelete = @" delete from public.""SYSGROUPMENU"" where ""idgroup"" = "+ sYSUSERGROUPMENUMODEL[0].idgroup + "";
                    con.Open();
                    NpgsqlTransaction trans = con.BeginTransaction();                    
                    using (NpgsqlCommand cmddelete = new NpgsqlCommand(strSqldelete, con))
                    {
                        cmddelete.CommandType = CommandType.Text;                       
                        cmddelete.Transaction = trans;
                        int affetecdCount = cmddelete.ExecuteNonQuery();
                           
                    }
                    foreach (var item in sYSUSERGROUPMENUMODEL)
                    {
                        string strSql = @"call public.sp_insertgroupmenu(
                                    :p_idgroup
                                    ,:p_idmenu                                    
									)";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                        {
                                        
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("p_idgroup", DbType.Int32).Value = item.idgroup;
                            cmd.Parameters.AddWithValue("p_idmenu", DbType.Int32).Value = item.idmenu;
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();                           
                        }
                        
                    }
                    trans.Commit();
                    con.Close();
                    result = true;

                }
                catch (NpgsqlException err)
                {
                    throw new NpgsqlException(err.Message);
                }
                return result;
            }
        }
    }
}