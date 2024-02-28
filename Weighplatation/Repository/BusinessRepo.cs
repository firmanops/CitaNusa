using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Interface;
using Npgsql;
using System.Data;
using System.Configuration;

namespace Weighplatation.Repository
{
    public class BusinessRepo : IBusiness
    {
        private readonly string stringCon = ConfigurationManager.ConnectionStrings["dbcon"].ToString();
        public int CheckDuplicateBusiness(string IdOddo)
        {
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    int row = 0;
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = @"select count(*) as row from  public.""BUSINESSPARTNER"" where ""oddoid""= '" + IdOddo + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            row = int.Parse(reader["row"].ToString());
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return row;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }

            }
        }

        public bool InsertBusiness(BusinessModel businessModel)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    // :p_bplogo ,
                    con.ConnectionString = stringCon;
                    string strSql = @"call public.sp_insertbusiness(	
                                    :p_bpcode,
	                                :p_bpname,
	                                :p_bptype,
	                                :p_address1,
	                                :p_address2,
	                                :p_city,
	                                :p_province,
	                                :p_postalcode,
	                                :p_taxid,
	                                :p_phone,
	                                :p_email,
	                                :p_picname,
	                                :p_active ,	                               
	                                :p_oddoid)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_bpcode", DbType.String).Value = businessModel.BPCode.Trim();
                        cmd.Parameters.AddWithValue("p_bpname", DbType.String).Value = businessModel.BPName;
                        cmd.Parameters.AddWithValue("p_bptype", DbType.String).Value = businessModel.BPType;
                        cmd.Parameters.AddWithValue("p_address1", DbType.String).Value = businessModel.Address1 == "false" ? "" : businessModel.Address1;
                        cmd.Parameters.AddWithValue("p_address2", DbType.String).Value = businessModel.Address2 == "false" ? "" : businessModel.Address2;
                        cmd.Parameters.AddWithValue("p_city", DbType.String).Value = businessModel.City == "false" ? "" : businessModel.City;
                        cmd.Parameters.AddWithValue("p_province", DbType.String).Value = businessModel.Province == "false" ? "" :businessModel.Province;
                        cmd.Parameters.AddWithValue("p_postalcode", DbType.String).Value = businessModel.Postalcode == "false" ? "" : businessModel.Postalcode;
                        cmd.Parameters.AddWithValue("p_taxid", DbType.String).Value =  businessModel.TaxID == "false" ? "" : businessModel.TaxID;
                        cmd.Parameters.AddWithValue("p_phone", DbType.String).Value = businessModel.Phone == "false" ? "" : businessModel.Phone;
                        cmd.Parameters.AddWithValue("p_email", DbType.String).Value = businessModel.Email == "false" ? "" : businessModel.Email;
                        cmd.Parameters.AddWithValue("p_picname", DbType.String).Value = businessModel.PICName;
                        cmd.Parameters.AddWithValue("p_active", DbType.Boolean).Value = businessModel.Active;
                        //cmd.Parameters.AddWithValue("p_bplogo", DbType.Binary).Value = new byte[0];// businessModel.bplogo;
                        cmd.Parameters.AddWithValue("p_oddoid", DbType.Int32).Value = businessModel.oddoid;

                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();
                            trans.Commit();
                            con.Close();
                            result = true;
                            return result;
                        }
                        catch (NpgsqlException err)
                        {

                            throw new NpgsqlException(err.Message + ": " + businessModel.oddoid);
                        }

                        //result = true;
                    }

                    //con.Close();

                    //return result;
                }
            }
            catch (NpgsqlException err)
            {


                throw new NpgsqlException(err.Message + ": " + businessModel.oddoid);
            }
        }
    }
}