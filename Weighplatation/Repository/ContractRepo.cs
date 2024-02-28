using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Interface;
using Npgsql;
using System.Configuration;
using System.Data;

namespace Weighplatation.Repository
{
    public class ContractRepo : IContract
    {
        private readonly string stringCon = ConfigurationManager.ConnectionStrings["dbcon"].ToString();
        public List<ContractModel> GetAllContract()
        {
            //throw new NotImplementedException();
            List<ContractModel> listContract = new List<ContractModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = @"select * from public.get_contract()";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ContractModel entity = new ContractModel();
                                entity.ContractNo = (string)reader["contractno"];
                                entity.ContractDate = (DateTime)reader["contractdate"];
                                entity.ExpDate = (DateTime)reader["expdate"];
                                entity.ProductCode = (string)reader["productcode"];
                                entity.BPCode = (string)reader["bpcode"];
                                entity.Qty = (reader["qty"] == DBNull.Value) ? 0 : double.Parse(reader["qty"].ToString());
                                entity.UnitPrice = (reader["unitprice"] == DBNull.Value) ? 0 : double.Parse(reader["unitprice"].ToString());
                                entity.PremiumPrice = (reader["premiumprice"] == DBNull.Value) ? 0 : double.Parse(reader["premiumprice"].ToString());
                                entity.Toleransi = (reader["toleransi"] == DBNull.Value) ? 0 : double.Parse(reader["toleransi"].ToString());
                                entity.TotalPrice = (reader["totalprice"] == DBNull.Value) ? 0 : double.Parse(reader["totalprice"].ToString());
                                entity.PPN = (reader["ppn"] == DBNull.Value) ? 0 : double.Parse(reader["ppn"].ToString());
                                entity.FinalUnitPrice = (reader["finalunitprice"] == DBNull.Value) ? 0 : double.Parse(reader["finalunitprice"].ToString());
                                entity.DespatchQty = (reader["despatchqty"] == DBNull.Value) ? 0 : double.Parse(reader["despatchqty"].ToString());
                                entity.DeliveryStatus = (reader["deliverystatus"] == DBNull.Value) ? "" : (string)(reader["deliverystatus"].ToString());
                                entity.oddoid = (reader["oddoid"] == DBNull.Value) ? 0 : int.Parse(reader["oddoid"].ToString());
                                listContract.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return listContract;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }

        public bool InsertContract(ContractModel contractModel)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    string strSql = @"call public.sp_insertcontract(:p_contractno,
							        :p_contractdate,
							        :p_expdate,
							        :p_productcode,
							        :p_bpcode,
							        :p_qty,
							        :p_toleransi,
							        :p_unitprice,
							        :p_premiumprice,
							        :p_ppn,
							        :p_finalunitprice,
							        :p_totalprice,
							        :p_despatchqty,
							        :p_deliverystatus,
							        :p_oddoid)";
                                    //:p_refno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = contractModel.ContractNo.Trim();
                        cmd.Parameters.AddWithValue("p_contractdate", DbType.DateTime).Value = Convert.ToDateTime(contractModel.ContractDate);
                        cmd.Parameters.AddWithValue("p_expdate", DbType.DateTime).Value = Convert.ToDateTime(contractModel.ExpDate);
                        cmd.Parameters.AddWithValue("p_productcode", DbType.String).Value = contractModel.ProductCode;
                        cmd.Parameters.AddWithValue("p_bpcode", DbType.String).Value = contractModel.BPCode;
                        cmd.Parameters.AddWithValue("p_qty", DbType.Int64).Value = Int64.Parse(contractModel.Qty.ToString());
                        cmd.Parameters.AddWithValue("p_toleransi", DbType.Int32).Value = Int32.Parse(contractModel.Toleransi.ToString());
                        cmd.Parameters.AddWithValue("p_unitprice", DbType.Int64).Value = Int64.Parse(contractModel.UnitPrice.ToString());
                        cmd.Parameters.AddWithValue("p_premiumprice", DbType.Int32).Value = Int32.Parse(contractModel.PremiumPrice.ToString());
                        cmd.Parameters.AddWithValue("p_ppn", DbType.Int32).Value = Int32.Parse(contractModel.PPN.ToString());
                        cmd.Parameters.AddWithValue("p_finalunitprice", DbType.Int64).Value = Int64.Parse(Math.Ceiling(contractModel.FinalUnitPrice).ToString());
                        cmd.Parameters.AddWithValue("p_totalprice", DbType.Int64).Value = Int64.Parse(contractModel.TotalPrice.ToString());
                        cmd.Parameters.AddWithValue("p_despatchqty", DbType.Int64).Value = Int64.Parse(contractModel.DespatchQty.ToString());
                        cmd.Parameters.AddWithValue("p_deliverystatus", DbType.String).Value = contractModel.DeliveryStatus;
                        cmd.Parameters.AddWithValue("p_oddoid", DbType.Int32).Value = Int32.Parse(contractModel.oddoid.ToString());
                       //cmd.Parameters.AddWithValue("p_refno", DbType.String).Value = "";//(contractModel.RefNo == null ? "": contractModel.RefNo);
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

                            throw new NpgsqlException(err.Message);
                        }

                        //result = true;
                    }

                    //con.Close();

                    //return result;
                }
            }
            catch (NpgsqlException err)
            {


                throw new NpgsqlException(err.Message);
            }
        }

        public int CheckDuplicateContract(string ContractNo)
        {

            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    int row = 0;
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = @"select count(*) as row from  public.""WBCONTRACT"" where ""ContractNo""= '" + ContractNo + "'";

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

        public bool UpdateContract(ContractModel contractModel)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    string strSql = @"call public.sp_updatecontract(:p_contractno,							      
							        :p_expdate,
							        :p_productcode,
							        :p_bpcode,
							        :p_qty,
							        :p_toleransi,
							        :p_unitprice,
							        :p_premiumprice,
							        :p_ppn,
							        :p_finalunitprice,
							        :p_totalprice,
							        :p_despatchqty,
							        :p_deliverystatus)";
                                    //:p_refno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = contractModel.ContractNo.Trim();                      
                        cmd.Parameters.AddWithValue("p_expdate", DbType.DateTime).Value = Convert.ToDateTime(contractModel.ExpDate);
                        cmd.Parameters.AddWithValue("p_productcode", DbType.String).Value = contractModel.ProductCode;
                        cmd.Parameters.AddWithValue("p_bpcode", DbType.String).Value = contractModel.BPCode;
                        cmd.Parameters.AddWithValue("p_qty", DbType.Int64).Value = Int64.Parse(contractModel.Qty.ToString());
                        cmd.Parameters.AddWithValue("p_toleransi", DbType.Int32).Value = Int32.Parse(contractModel.Toleransi.ToString());
                        cmd.Parameters.AddWithValue("p_unitprice", DbType.Int64).Value = Int64.Parse(contractModel.UnitPrice.ToString());
                        cmd.Parameters.AddWithValue("p_premiumprice", DbType.Int32).Value = Int32.Parse(contractModel.PremiumPrice.ToString());
                        cmd.Parameters.AddWithValue("p_ppn", DbType.Int32).Value = Int32.Parse(contractModel.PPN.ToString());
                        cmd.Parameters.AddWithValue("p_finalunitprice", DbType.Int64).Value = Int64.Parse(contractModel.FinalUnitPrice.ToString());
                        cmd.Parameters.AddWithValue("p_totalprice", DbType.Int64).Value = Int64.Parse(contractModel.TotalPrice.ToString());
                        cmd.Parameters.AddWithValue("p_despatchqty", DbType.Int64).Value = Int64.Parse(contractModel.DespatchQty.ToString());
                        cmd.Parameters.AddWithValue("p_deliverystatus", DbType.String).Value = contractModel.DeliveryStatus;
                        //cmd.Parameters.AddWithValue("p_refno", DbType.String).Value = contractModel.RefNo;
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

                            throw new NpgsqlException(err.Message);
                        }

                        //result = true;
                    }

                    //con.Close();

                    //return result;
                }
            }
            catch (NpgsqlException err)
            {


                throw new NpgsqlException(err.Message);
            }
        }

        public ContractModel GetByContractNo(string contractNo)
        {
            ContractModel listContract = new ContractModel();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = @"select * from public.get_contractbyno(:p_contractno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = contractNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                                listContract.ContractNo = (string)reader["contractno"];
                                listContract.ContractDate = (DateTime)reader["contractdate"];
                                listContract.ExpDate = (DateTime)reader["expdate"];
                                listContract.ProductCode = (string)reader["productcode"];
                                listContract.BPCode = (string)reader["bpcode"];
                                listContract.Qty = (reader["qty"] == DBNull.Value) ? 0 : double.Parse(reader["qty"].ToString());
                                listContract.UnitPrice = (reader["unitprice"] == DBNull.Value) ? 0 : double.Parse(reader["unitprice"].ToString());
                                listContract.PremiumPrice = (reader["premiumprice"] == DBNull.Value) ? 0 : double.Parse(reader["premiumprice"].ToString());
                                listContract.Toleransi = (reader["toleransi"] == DBNull.Value) ? 0 : double.Parse(reader["toleransi"].ToString());
                                listContract.TotalPrice = (reader["totalprice"] == DBNull.Value) ? 0 : double.Parse(reader["totalprice"].ToString());
                                listContract.PPN = (reader["ppn"] == DBNull.Value) ? 0 : double.Parse(reader["ppn"].ToString());
                                listContract.FinalUnitPrice = (reader["finalunitprice"] == DBNull.Value) ? 0 : double.Parse(reader["finalunitprice"].ToString());
                                listContract.DespatchQty = (reader["despatchqty"] == DBNull.Value) ? 0 : double.Parse(reader["despatchqty"].ToString());
                                listContract.DeliveryStatus = (reader["deliverystatus"] == DBNull.Value) ? "" : (string)(reader["deliverystatus"].ToString());
                                listContract.oddoid = (reader["oddoid"] == DBNull.Value) ? 0 : int.Parse(reader["oddoid"].ToString());
                                listContract.RefNo = "";//(reader["refno"] == DBNull.Value) ? "" : (string)reader["refno"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return listContract;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }

        public List<ContractModel> GetByFilterContractNo(string contractNo)
        {
            ContractModel listContract = new ContractModel();
            List<ContractModel> list = new List<ContractModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = @"select * from public.get_contractbyno(:p_contractno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = contractNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            listContract.ContractNo = (string)reader["contractno"];
                            listContract.ContractDate = (DateTime)reader["contractdate"];
                            listContract.ExpDate = (DateTime)reader["expdate"];
                            listContract.ProductCode = (string)reader["productcode"];
                            listContract.BPCode = (string)reader["bpcode"];
                            listContract.Qty = (reader["qty"] == DBNull.Value) ? 0 : double.Parse(reader["qty"].ToString());
                            listContract.UnitPrice = (reader["unitprice"] == DBNull.Value) ? 0 : double.Parse(reader["unitprice"].ToString());
                            listContract.PremiumPrice = (reader["premiumprice"] == DBNull.Value) ? 0 : double.Parse(reader["premiumprice"].ToString());
                            listContract.Toleransi = (reader["toleransi"] == DBNull.Value) ? 0 : double.Parse(reader["toleransi"].ToString());
                            listContract.TotalPrice = (reader["totalprice"] == DBNull.Value) ? 0 : double.Parse(reader["totalprice"].ToString());
                            listContract.PPN = (reader["ppn"] == DBNull.Value) ? 0 : double.Parse(reader["ppn"].ToString());
                            listContract.FinalUnitPrice = (reader["finalunitprice"] == DBNull.Value) ? 0 : double.Parse(reader["finalunitprice"].ToString());
                            listContract.DespatchQty = (reader["despatchqty"] == DBNull.Value) ? 0 : double.Parse(reader["despatchqty"].ToString());
                            listContract.DeliveryStatus = (reader["deliverystatus"] == DBNull.Value) ? "" : (string)(reader["deliverystatus"].ToString());
                            listContract.oddoid = (reader["oddoid"] == DBNull.Value) ? 0 : int.Parse(reader["oddoid"].ToString());
                            list.Add(listContract);
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return list;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }
    }
}