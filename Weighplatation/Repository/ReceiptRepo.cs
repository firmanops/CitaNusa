using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Interface;
using System.Configuration;
using Npgsql;
using System.Data;
using System.Globalization;

namespace Weighplatation.Repository
{
    public class ReceiptRepo : IReceipt
    {
        private readonly string stringCon = ConfigurationManager.ConnectionStrings["dbcon"].ToString();

        public bool CancelReceipt(string TicketNo)
        {
            throw new NotImplementedException();
        }

        public string GenerateTicketNo(string UnitCode)
        {
           
            double TranNow = double.Parse(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString());

           
            
            try
            {


                Random generator = new Random();
                int r = generator.Next(100000, 999999);
                int r1 = generator.Next(100000, 1000000);
                string result = "";

                result = UnitCode + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + r.ToString();

                //using (NpgsqlConnection con = new NpgsqlConnection())
                //{

                  

                    //con.ConnectionString = stringCon;
                    //con.Open();

                    //string result = "";

                    //string strSql = @"select max(Substring(""TicketNo"",9,6)) as counter from public.""WBTRX""";
                    //int TicketNo = 0;
                    //int Counter = 0;
                    //using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    //{
                    //    cmd.CommandType = CommandType.Text;
                    //    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    //    {
                    //        reader.Read();
                    //        Counter = int.Parse(reader["counter"].ToString());
                    //    }
                    //    cmd.Dispose();
                    //}

                    //con.Close();

                    //string sMaxTran = Check();
                    //if (sMaxTran == "")
                    //{
                    //    sMaxTran = DateTime.Now.ToString();
                    //}
                    //double maxTran = Double.Parse(Convert.ToDateTime(sMaxTran).Year.ToString() + Convert.ToDateTime(sMaxTran).Month.ToString());
                    //if (maxTran == TranNow)
                    //{
                    //    TicketNo = Counter + 1;
                    //    result = UnitCode + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + TicketNo.ToString("000000") + System.Environment.NewLine;
                    //}
                    //else
                    //{
                    //    TicketNo = 1;
                    //    result = UnitCode + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + TicketNo.ToString("000000") + System.Environment.NewLine;
                    //}


                    return result.Trim();
                //}
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }

        }

        public string Check()
        {
            string Result = "";
           
            try
            {


                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = @"select max(""Created"") as maxdate from public.""WBTRX""; ";
                  
                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            Result = reader["maxdate"].ToString();
                        }
                        cmd.Dispose();
                    }

                    con.Close();
                 

                    return Result;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                //Result = "True";
                //return Result;
                throw;
            }

        }

        public List<ReceiptModel> GetAllReceiptByToday()
        {
            List<ReceiptModel> wBTRXes = new List<ReceiptModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from get_receipt()";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReceiptModel entity = new ReceiptModel();
                                entity.TicketNo = (string)reader["TicketNo"];
                                entity.Created = (DateTime)reader["Created"];
                                entity.UnitName = (string)reader["UnitName"];
                                entity.ProductName = (string)reader["ProductName"];
                                entity.ContractNo = (string)reader["ContractNo"];
                                entity.BPName = (reader["BPName"] == DBNull.Value) ? "" : (string)reader["BPName"];
                                entity.VehicleID = (reader["VehicleID"] == DBNull.Value) ? "" : (string)reader["VehicleID"];
                                entity.WBStatus = (reader["WBStatus"] == DBNull.Value) ? "" : (string)reader["WBStatus"];
                                entity.weight1st= (reader["weight1st"] == DBNull.Value) ? "" : (string)reader["weight1st"];
                                entity.weight2nd = (reader["weight2nd"] == DBNull.Value) ? "" : (string)reader["weight2nd"];
                                entity.status= (reader["weight1st"] == DBNull.Value) ? "" : (string)reader["weight1st"];

                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }
        public List<OtherReceiptModel> GetAllOtherReceiptByToday()
        {
            List<OtherReceiptModel> wBTRXes = new List<OtherReceiptModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from get_otherreceipt()";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OtherReceiptModel entity = new OtherReceiptModel();
                                entity.TicketNo = (string)reader["TicketNo"];
                                entity.Created = (DateTime)reader["Created"];
                                entity.UnitName = (string)reader["UnitName"];
                                entity.ProductName = (string)reader["ProductName"];                              
                                entity.BPName = (reader["BPName"] == DBNull.Value) ? "" : (string)reader["BPName"];
                                entity.VehicleID = (reader["VehicleID"] == DBNull.Value) ? "" : (string)reader["VehicleID"];
                                entity.WBStatus = (reader["WBStatus"] == DBNull.Value) ? "" : (string)reader["WBStatus"];
                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }
        public List<BusinessUnitModel> GetUnitByContract(string BPCode)
        {

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();
                    List<BusinessUnitModel> businessUnitModels = new List<BusinessUnitModel>();

                    string strSql = @"select * from public.""BUSINESSUNIT"" where ""BPCode"" = '" + BPCode + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessUnitModel entity = new BusinessUnitModel();
                                entity.UnitCode = (string)reader["UnitCode"];
                                entity.UnitName = (string)reader["UnitName"];
                                entity.Province = reader["Province"] == DBNull.Value ? "": (string)reader["Province"];
                                entity.Postalcode = reader["Postalcode"] == DBNull.Value ? "": (string)reader["Postalcode"];
                                entity.City = reader["City"] == DBNull.Value ? "" : (string)reader["City"];
                                entity.BPCode = reader["BPCode"] == DBNull.Value ? "" : (string)reader["BPCode"];
                                entity.Address1 = reader["Address1"] == DBNull.Value ? "" : (string)reader["Address1"];
                                entity.Address2 = reader["Address2"]== DBNull.Value ? "" : (string)reader["Address2"];
                                entity.Active = bool.Parse(reader["Active"].ToString());
                                businessUnitModels.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return businessUnitModels;
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public string GetCompanyName(string BPCode)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();

                    string result = "";

                    string strSql = @"select * from public.getBPName(@BPCode)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@BPCode", BPCode);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            result = (string)reader["BPName"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return result;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public List<ReceipDetailOneSTtModel> GetDetailReceiptST()
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();
                    List<ReceipDetailOneSTtModel> wBTRXes = new List<ReceipDetailOneSTtModel>();

                    string strSql = @"select * from public.getReceiptDetail()";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        //cmd.Parameters.AddWithValue("@BPCode", BPCode);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReceipDetailOneSTtModel entity = new ReceipDetailOneSTtModel();
                                entity.TicketNo = (string)reader["TicketNo"];
                                entity.BlockID = (string)reader["BlockID"];
                                entity.Divison = "";
                                entity.YoP = (string)reader["YoP"];
                                entity.Bunches = (double)reader["Bunches"];
                                entity.LFQty = (double)reader["LFQty"];
                                entity.Estimation = (double)reader["Estimation"];
                                entity.Weight = (double)reader["Weight"];
                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public List<VehicleDllModel> GetDllVehicle(string BPCode)
        {
            List<VehicleDllModel> wBTRXes = new List<VehicleDllModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.""WBVEHICLE""";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@BPCode", BPCode);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VehicleDllModel entity = new VehicleDllModel();
                                entity.VehicleID = (string)reader["VehicleID"];
                                entity.DriverName = (string)reader["DriverName"];
                                entity.BPCode = (string)reader["BPCode"];
                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

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

        public bool InsertReceiptHeader(WBTRXModel listReceipt, List<WBTRXBLOCK> listReceiptDetail)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    string strSql = @"call public.sp_InsertReceiptHeader(
                                    :p_TicketNo
                                    ,:p_WBSOURCE
                                    ,:p_WBType
									,:p_TrxDateIn
									,:p_TrxDateOut
									,:p_UnitCode
									,:p_NoCoverLetter
									,:p_ProductCode
									,:p_ContractNo
									,:p_DONo
									,:p_VehicleID
									,:p_DriverName
									,:p_LicenseNo
									,:p_Weight1ST
									,:p_Weight2ND
									,:p_WBFlag
									,:p_WBStatus
                                    ,:p_WBImagefront1
                                    ,:p_reason
									,:p_UserIDWeight1ST
									,:p_UserIDWeight2ND
									,:p_UserIDApproval
                                    ,:p_Created
                                    ,:p_Updated)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_TicketNo", DbType.String).Value = listReceipt.TicketNo.Trim();
                        cmd.Parameters.AddWithValue("p_WBSOURCE", DbType.String).Value = listReceipt.WBSOURCE.Trim();
                        cmd.Parameters.AddWithValue("p_WBType", DbType.String).Value = listReceipt.WBType.Trim();
                        cmd.Parameters.AddWithValue("p_TrxDateIn", DbType.DateTime).Value = Convert.ToDateTime(listReceipt.TrxDateIn);
                        cmd.Parameters.AddWithValue("p_TrxDateOut", DbType.Date).Value = Convert.ToDateTime(listReceipt.TrxDateOut);
                        cmd.Parameters.AddWithValue("p_UnitCode", DbType.String).Value = listReceipt.UnitCode.Trim();
                        cmd.Parameters.AddWithValue("p_NoCoverLetter", DbType.String).Value = listReceipt.NoCoverLetter.Trim();
                        cmd.Parameters.AddWithValue("p_ProductCode", DbType.String).Value = listReceipt.ProductCode.Trim();
                        cmd.Parameters.AddWithValue("p_ContractNo", DbType.String).Value = listReceipt.ContractNo.Trim();
                        cmd.Parameters.AddWithValue("p_DONo", DbType.String).Value = listReceipt.DONo.Trim();
                        cmd.Parameters.AddWithValue("p_VehicleID", DbType.String).Value = listReceipt.VehicleID.Trim();
                        cmd.Parameters.AddWithValue("p_DriverName", DbType.String).Value = listReceipt.DriverName.Trim();
                        cmd.Parameters.AddWithValue("p_LicenseNo", DbType.String).Value = listReceipt.LicenseNo.Trim();
                        cmd.Parameters.AddWithValue("p_Weight1ST", DbType.Int32).Value = Int32.Parse(listReceipt.Weight1ST.ToString());
                        cmd.Parameters.AddWithValue("p_Weight2ND", DbType.Int32).Value = Int32.Parse(listReceipt.Weight2ND.ToString() == "" ? "0" : listReceipt.Weight2ND.ToString());
                        cmd.Parameters.AddWithValue("p_WBFlag", DbType.String).Value = listReceipt.WBFlag1.Trim();
                        cmd.Parameters.AddWithValue("p_WBStatus", DbType.String).Value = listReceipt.WBStatus.Trim();
                        cmd.Parameters.AddWithValue("p_WBImagefront1", DbType.Binary).Value = listReceipt.WBImagefront1;
                        cmd.Parameters.AddWithValue("p_reason", DbType.String).Value = listReceipt.Reason == null ? "" : listReceipt.Reason ;
                        cmd.Parameters.AddWithValue("p_UserIDWeight1ST", DbType.String).Value = listReceipt.UserIDWeight1ST.Trim();
                        cmd.Parameters.AddWithValue("p_UserIDWeight2ND", DbType.String).Value = listReceipt.UserIDWeight2ND.Trim();
                        cmd.Parameters.AddWithValue("p_UserIDApproval", DbType.String).Value = listReceipt.UserIDApproval.Trim();
                        cmd.Parameters.AddWithValue("p_Created", DbType.Date).Value = Convert.ToDateTime(listReceipt.Created);
                        cmd.Parameters.AddWithValue("p_Updated", DbType.Date).Value = Convert.ToDateTime(listReceipt.Updated);
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();

                            string strSqlVehicle = @"update public.""WBVEHICLE""  set ""VehFlag"" = '1' where ""VehicleID"" = '" + listReceipt.VehicleID.Trim() + "' ";
                            using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSqlVehicle, con))
                            {
                                cmdVehicle.CommandType = CommandType.Text;
                                cmdVehicle.ExecuteNonQuery();
                            }

                            string strSqContract = @"update public.""WBCONTRACT""  set ""DeliveryStatus"" = '1' where ""ContractNo"" = '" + listReceipt.ContractNo.Trim() + "' ";
                            using (NpgsqlCommand cmdContract = new NpgsqlCommand(strSqContract, con))
                            {
                                cmdContract.CommandType = CommandType.Text;
                                cmdContract.ExecuteNonQuery();
                            }

                            if (listReceiptDetail != null)
                            {
                                foreach (var item in listReceiptDetail)
                                {
                                    string strSqlDetail = @"call public.sp_InsertReceiptDetail(:p_TicketNo, :p_BlockID, :p_BunchesQty, :p_LFQty, :p_Estimation, :p_Weight)";

                                    using (NpgsqlCommand cmddetail = new NpgsqlCommand(strSqlDetail, con))
                                    {
                                        cmddetail.CommandType = CommandType.Text;
                                        cmddetail.Parameters.AddWithValue("p_TicketNo", DbType.String).Value = item.TicketNo.Trim();
                                        cmddetail.Parameters.AddWithValue("p_BlockID", DbType.String).Value = item.BlockID.Trim();
                                        cmddetail.Parameters.AddWithValue("p_BunchesQty", DbType.Int32).Value = Int32.Parse(item.BunchesQty.ToString());
                                        cmddetail.Parameters.AddWithValue("p_LFQty", DbType.Int32).Value = Int32.Parse(item.LFQty.ToString());
                                        cmddetail.Parameters.AddWithValue("p_Estimation", DbType.Int32).Value = Int32.Parse(item.Estimation.ToString());
                                        cmddetail.Parameters.AddWithValue("p_Weight", DbType.Int32).Value = Int32.Parse(item.Weight.ToString());
                                        cmddetail.CommandType = CommandType.Text;

                                        int affetecdCountDetail = cmddetail.ExecuteNonQuery();

                                    }
                                }                                
                            }
                            result = true;
                            trans.Commit();
                            con.Close();
                            return result;
                        }
                        catch (Exception err)
                        {

                            throw new NpgsqlException(err.Message);
                        }

                        //result = true;
                    }
                }
            }
            catch (NpgsqlException err)
            {


                throw new NpgsqlException(err.Message);
            }
        }

        public bool InsertOtherReceiptHeader(WBTRXETCModel listReceipt, List<WBTRXBLOCK> listReceiptDetail)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    string strSql = @"call public.sp_InsertOtherReceiptHeader(
                                    :p_TicketNo
                                    ,:p_WBSOURCE
                                    ,:p_WBType
									,:p_TrxDateIn
									,:p_TrxDateOut
									,:p_UnitCode
									,:p_NoCoverLetter
									,:p_ProductCode									
									,:p_VehicleID
									,:p_DriverName
									,:p_LicenseNo
									,:p_Weight1ST
									,:p_Weight2ND
									,:p_WBFlag
									,:p_WBStatus
                                    ,:p_WBImagefront1
									,:p_UserIDWeight1ST
									,:p_UserIDWeight2ND
									,:p_UserIDApproval
                                    ,:p_Created
                                    ,:p_Updated)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_TicketNo", DbType.String).Value = listReceipt.TicketNo.Trim();
                        cmd.Parameters.AddWithValue("p_WBSOURCE", DbType.String).Value = listReceipt.WBSOURCE.Trim();
                        cmd.Parameters.AddWithValue("p_WBType", DbType.String).Value = listReceipt.WBType.Trim();
                        cmd.Parameters.AddWithValue("p_TrxDateIn", DbType.DateTime).Value = Convert.ToDateTime(listReceipt.TrxDateIn);
                        cmd.Parameters.AddWithValue("p_TrxDateOut", DbType.Date).Value = Convert.ToDateTime(listReceipt.TrxDateOut);
                        cmd.Parameters.AddWithValue("p_UnitCode", DbType.String).Value = listReceipt.UnitCode.Trim();
                        cmd.Parameters.AddWithValue("p_NoCoverLetter", DbType.String).Value = listReceipt.NoCoverLetter.Trim();
                        cmd.Parameters.AddWithValue("p_ProductCode", DbType.String).Value = listReceipt.ProductCode.Trim();
                        //cmd.Parameters.AddWithValue("p_ContractNo", DbType.String).Value = listReceipt.ContractNo.Trim();
                        //cmd.Parameters.AddWithValue("p_DONo", DbType.String).Value = listReceipt.DONo.Trim();
                        cmd.Parameters.AddWithValue("p_VehicleID", DbType.String).Value = listReceipt.VehicleID.Trim();
                        cmd.Parameters.AddWithValue("p_DriverName", DbType.String).Value = listReceipt.DriverName.Trim();
                        cmd.Parameters.AddWithValue("p_LicenseNo", DbType.String).Value = listReceipt.LicenseNo.Trim();
                        cmd.Parameters.AddWithValue("p_Weight1ST", DbType.Int32).Value = Int32.Parse(listReceipt.Weight1ST.ToString());
                        cmd.Parameters.AddWithValue("p_Weight2ND", DbType.Int32).Value = Int32.Parse(listReceipt.Weight2ND.ToString() == "" ? "0" : listReceipt.Weight2ND.ToString());
                        cmd.Parameters.AddWithValue("p_WBFlag", DbType.String).Value = listReceipt.WBFlag1.Trim();
                        cmd.Parameters.AddWithValue("p_WBStatus", DbType.String).Value = listReceipt.WBStatus.Trim();
                        cmd.Parameters.AddWithValue("p_WBImagefront1", DbType.Binary).Value = listReceipt.WBImagefront1;
                        cmd.Parameters.AddWithValue("p_UserIDWeight1ST", DbType.String).Value = listReceipt.UserIDWeight1ST.Trim();
                        cmd.Parameters.AddWithValue("p_UserIDWeight2ND", DbType.String).Value = listReceipt.UserIDWeight2ND.Trim();
                        cmd.Parameters.AddWithValue("p_UserIDApproval", DbType.String).Value = listReceipt.UserIDApproval.Trim();
                        cmd.Parameters.AddWithValue("p_Created", DbType.Date).Value = Convert.ToDateTime(listReceipt.Created);
                        cmd.Parameters.AddWithValue("p_Updated", DbType.Date).Value = Convert.ToDateTime(listReceipt.Updated);
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();

                            string strSqlVehicle = @"update public.""WBVEHICLE""  set ""VehFlag"" = '1' where ""VehicleID"" = '" + listReceipt.VehicleID.Trim() + "' ";
                            using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSqlVehicle, con))
                            {
                                cmdVehicle.CommandType = CommandType.Text;
                                cmdVehicle.ExecuteNonQuery();
                            }

                            //string strSqContract = @"update public.""WBCONTRACT""  set ""DeliveryStatus"" = '1' where ""ContractNo"" = '" + listReceipt.ContractNo.Trim() + "' ";
                            //using (NpgsqlCommand cmdContract = new NpgsqlCommand(strSqContract, con))
                            //{
                            //    cmdContract.CommandType = CommandType.Text;
                            //    cmdContract.ExecuteNonQuery();
                            //}

                            if (listReceiptDetail != null)
                            {
                                foreach (var item in listReceiptDetail)
                                {
                                    string strSqlDetail = @"call public.sp_InsertReceiptDetail(:p_TicketNo, :p_BlockID, :p_BunchesQty, :p_LFQty, :p_Estimation, :p_Weight)";

                                    using (NpgsqlCommand cmddetail = new NpgsqlCommand(strSqlDetail, con))
                                    {
                                        cmddetail.CommandType = CommandType.Text;
                                        cmddetail.Parameters.AddWithValue("p_TicketNo", DbType.String).Value = item.TicketNo.Trim();
                                        cmddetail.Parameters.AddWithValue("p_BlockID", DbType.String).Value = item.BlockID.Trim();
                                        cmddetail.Parameters.AddWithValue("p_BunchesQty", DbType.Int32).Value = Int32.Parse(item.BunchesQty.ToString());
                                        cmddetail.Parameters.AddWithValue("p_LFQty", DbType.Int32).Value = Int32.Parse(item.LFQty.ToString());
                                        cmddetail.Parameters.AddWithValue("p_Estimation", DbType.Int32).Value = Int32.Parse(item.Estimation.ToString());
                                        cmddetail.Parameters.AddWithValue("p_Weight", DbType.Int32).Value = Int32.Parse(item.Weight.ToString());
                                        cmddetail.CommandType = CommandType.Text;

                                        int affetecdCountDetail = cmddetail.ExecuteNonQuery();

                                    }
                                }
                            }
                            result = true;
                            trans.Commit();
                            con.Close();
                            return result;
                        }
                        catch (Exception err)
                        {

                            throw new NpgsqlException(err.Message);
                        }

                        //result = true;
                    }
                }
            }
            catch (NpgsqlException err)
            {


                throw new NpgsqlException(err.Message);
            }
        }

        public Tuple<bool, List<NetWeightModel>, NetReceiptTotalModel> UpdateReceipt(WBTRXModel listReceipt, List<WBTRXGRADING2nd> ReceiptDetail2Nd)
        {
            bool result = false;

            try
            {
                List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();
                NetReceiptTotalModel _NetReceiptTotalModel = new NetReceiptTotalModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    string strSql2nd = @"call public.sp_updatereceipt(
                                    :p_ticketno
									,:p_trxdateout
                                    ,:p_nocoverletter                                    
                                     ,:p_dono
                                    ,:p_weight2nd
                                    ,:p_wbstatus
                                    ,:p_wbimagefront2
                                    ,:p_wbflag
                                    ,:p_useridweight2nd
                                    ,:p_useridapproval
                                    ,:p_updated)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql2nd, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listReceipt.TicketNo.Trim();
                        cmd.Parameters.AddWithValue("p_trxdateout", DbType.DateTime).Value = Convert.ToDateTime(listReceipt.TrxDateOut); //DateTime.Parse(listReceipt.TrxDateOut.ToString());
                        cmd.Parameters.AddWithValue("p_nocoverletter", DbType.String).Value = listReceipt.NoCoverLetter.Trim();
                        cmd.Parameters.AddWithValue("p_dono", DbType.String).Value = listReceipt.DONo.Trim();
                        cmd.Parameters.AddWithValue("p_weight2nd", DbType.Int32).Value = Int32.Parse(listReceipt.Weight2ND.ToString());
                        cmd.Parameters.AddWithValue("p_wbstatus", DbType.String).Value = listReceipt.WBStatus.Trim();
                        cmd.Parameters.AddWithValue("p_wbimagefront2", DbType.Binary).Value = listReceipt.WBImagefront2;
                        cmd.Parameters.AddWithValue("p_wbflag", DbType.String).Value = listReceipt.WBFlag2.Trim();
                        cmd.Parameters.AddWithValue("p_useridweight2nd", DbType.String).Value = listReceipt.UserIDWeight2ND.Trim();
                        cmd.Parameters.AddWithValue("p_useridapproval", DbType.String).Value = listReceipt.UserIDApproval.Trim();
                        cmd.Parameters.AddWithValue("p_updated", DbType.Date).Value = Convert.ToDateTime(listReceipt.Updated); // DateTime.Parse(listReceipt.Updated.ToString());
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();
                            var x = ReceiptDetail2Nd;
                            if (ReceiptDetail2Nd != null)
                            {
                                foreach (var item in ReceiptDetail2Nd)
                                {
                                    string strSqlDetail = @"call public.sp_insertreceiptdetail2nd(:p_TicketNo, :p_gradingTypeID, :p_qty, :p_nosegel1, :p_nosegel2)";

                                    using (NpgsqlCommand cmddetail = new NpgsqlCommand(strSqlDetail, con))
                                    {
                                        cmddetail.CommandType = CommandType.Text;
                                        cmddetail.Parameters.AddWithValue("p_TicketNo", DbType.String).Value = item.TicketNo.Trim();
                                        cmddetail.Parameters.AddWithValue("p_gradingTypeID", DbType.String).Value = item.GradingTypeId.Trim();
                                        cmddetail.Parameters.AddWithValue("p_qty", DbType.Decimal).Value = Decimal.Parse(item.Quantity.ToString());
                                        cmddetail.Parameters.AddWithValue("p_nosegel1", DbType.String).Value = item.NoSegel2.ToString();
                                        cmddetail.Parameters.AddWithValue("p_nosegel2", DbType.String).Value = item.NoSegel2.ToString();
                                        cmddetail.CommandType = CommandType.Text;
                                        //con.Open();
                                        int affetecdCountDetail = cmddetail.ExecuteNonQuery();
                                    }
                                }

                                string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                                using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                                {
                                    cmdTotalNet.CommandType = CommandType.Text;
                                    cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listReceipt.TicketNo.Trim();
                                    using (NpgsqlDataReader reader = cmdTotalNet.ExecuteReader())
                                    {
                                        reader.Read();
                                        _NetReceiptTotalModel.TicketNo = (string)reader["TicketNo"].ToString();
                                        _NetReceiptTotalModel.Quantity = double.Parse(reader["Quantity"] == DBNull.Value ? "0" : reader["Quantity"].ToString());
                                    }
                                }
                            }
                            else
                            {

                                _NetReceiptTotalModel.TicketNo = listReceipt.TicketNo.Trim();
                                _NetReceiptTotalModel.Quantity = 0;
                            }


                            string strSqlVehicle = @"update public.""WBVEHICLE""  set ""VehFlag"" = '0' where ""VehicleID"" = '" + listReceipt.VehicleID.Trim() + "' ";
                            using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSqlVehicle, con))
                            {
                                cmdVehicle.CommandType = CommandType.Text;
                                cmdVehicle.ExecuteNonQuery();
                            }

                            string strSqlNet = @"select * from public.""WBTRX"" where ""TicketNo"" = '" + listReceipt.TicketNo.Trim() + "' ";
                            using (NpgsqlCommand cmdNet = new NpgsqlCommand(strSqlNet, con))
                            {
                                cmdNet.CommandType = CommandType.Text;

                                using (NpgsqlDataReader reader = cmdNet.ExecuteReader())
                                {
                                    reader.Read();

                                    DateTime dt1st = Convert.ToDateTime(reader["TrxDateIn"]);
                                    //DateTime v_dt1s = DateTime.ParseExact(dt1st.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);
                                    DateTime dt2nd = Convert.ToDateTime(reader["TrxDateOut"]);
                                    //DateTime v_dt2nd = DateTime.ParseExact(dt2nd.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);


                                    string Weight1st = "1ST";
                                    string Date1ST = dt1st.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    string Hour1ST = dt1st.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                                    string Mode1St = ((string)reader["WBFlag1"] == "A") ? "Normal" : "Manual";
                                    double Heavy1ST = double.Parse(reader["Weight1ST"].ToString());

                                    NetWeightModel _NetWeightModel1ST = new NetWeightModel();
                                    _NetWeightModel1ST.Weight = Weight1st;
                                    _NetWeightModel1ST.DateTransaction = Date1ST;
                                    _NetWeightModel1ST.Hours = Hour1ST;
                                    _NetWeightModel1ST.Mode = Mode1St;
                                    _NetWeightModel1ST.WeightHeavy = Heavy1ST;
                                    _NetWeightModel.Add(_NetWeightModel1ST);

                                    NetWeightModel _NetWeightModel2ND = new NetWeightModel();
                                    string Weight2ND = "2ND";
                                    string Date2ND = dt2nd.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    string Hour2ND = dt2nd.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                                    string Mode2ND = ((string)reader["WBFlag2"] == "A") ? "Normal" : "Manual";
                                    double Heavy2ND = double.Parse(reader["Weight2ND"].ToString());

                                    _NetWeightModel2ND.Weight = Weight2ND;
                                    _NetWeightModel2ND.DateTransaction = Date2ND;
                                    _NetWeightModel2ND.Hours = Hour2ND;
                                    _NetWeightModel2ND.Mode = Mode2ND;
                                    _NetWeightModel2ND.WeightHeavy = Heavy2ND;
                                    _NetWeightModel.Add(_NetWeightModel2ND);

                                }


                                double Net = _NetWeightModel[0].WeightHeavy - _NetWeightModel[1].WeightHeavy - _NetReceiptTotalModel.Quantity;
                                double DesctpacthQty = Math.Ceiling(GetSumQtyDespact(listReceipt.ContractNo.Trim()) + Net);

                                double QtyContract = GetWBCONTRACT(listReceipt.ContractNo.Trim()).Qty;

                                string DeliveryStatus = DesctpacthQty >= QtyContract ? "0" : "1";
                                string strSqContract = @"update public.""WBCONTRACT""  set ""DeliveryStatus"" =" + DeliveryStatus + @", ""DespatchQty"" = '" + DesctpacthQty.ToString() + "' " + @"where ""ContractNo"" = '" + listReceipt.ContractNo.Trim() + "' ";
                                using (NpgsqlCommand cmdContract = new NpgsqlCommand(strSqContract, con))
                                {
                                    cmdContract.CommandType = CommandType.Text;
                                    cmdContract.ExecuteNonQuery();
                                }
                            }



                            result = true;
                            trans.Commit();
                            con.Close();
                        }
                        catch (NpgsqlException err)
                        {

                            throw new NpgsqlException(err.Message);
                        }

                    }

                    return Tuple.Create(result, _NetWeightModel, _NetReceiptTotalModel);
                }
            }
            catch (NpgsqlException err)
            {
                throw new NpgsqlException(err.Message);
            }
        }

        public Tuple<bool, List<NetWeightModel>, NetReceiptTotalModel> UpdateOtherReceipt(WBTRXETCModel listReceipt, List<WBTRXGRADING2nd> ReceiptDetail2Nd)
        {
            bool result = false;
         
            try
            {
                List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();
                NetReceiptTotalModel _NetReceiptTotalModel = new NetReceiptTotalModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {                   
                    con.ConnectionString = stringCon;
                    string strSql2nd = @"call public.sp_updateotherreceipt(
                                    :p_ticketno
									,:p_trxdateout
                                    ,:p_nocoverletter                                                                      
                                    ,:p_weight2nd
                                    ,:p_wbstatus
                                    ,:p_wbimagefront2
                                    ,:p_wbflag
                                    ,:p_useridweight2nd
                                    ,:p_useridapproval
                                    ,:p_updated)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql2nd, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listReceipt.TicketNo.Trim();
                        cmd.Parameters.AddWithValue("p_trxdateout", DbType.Date).Value = Convert.ToDateTime(listReceipt.TrxDateOut); //DateTime.Parse(listReceipt.TrxDateOut.ToString());
                        cmd.Parameters.AddWithValue("p_nocoverletter", DbType.String).Value = listReceipt.NoCoverLetter.Trim();
                        //cmd.Parameters.AddWithValue("p_dono", DbType.String).Value = listReceipt.DONo.Trim();
                        cmd.Parameters.AddWithValue("p_weight2nd", DbType.Int32).Value = Int32.Parse(listReceipt.Weight2ND.ToString());
                        cmd.Parameters.AddWithValue("p_wbstatus", DbType.String).Value = listReceipt.WBStatus.Trim();
                        cmd.Parameters.AddWithValue("p_wbimagefront2", DbType.Binary).Value = listReceipt.WBImagefront2;
                        cmd.Parameters.AddWithValue("p_wbflag", DbType.String).Value = listReceipt.WBFlag2.Trim();
                        cmd.Parameters.AddWithValue("p_useridweight2nd", DbType.String).Value = listReceipt.UserIDWeight2ND.Trim();
                        cmd.Parameters.AddWithValue("p_useridapproval", DbType.String).Value = listReceipt.UserIDApproval.Trim();
                        cmd.Parameters.AddWithValue("p_updated", DbType.Date).Value = Convert.ToDateTime(listReceipt.Updated); // DateTime.Parse(listReceipt.Updated.ToString());
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();
                            var x = ReceiptDetail2Nd;
                            if (ReceiptDetail2Nd != null)
                            {
                                foreach (var item in ReceiptDetail2Nd)
                                {
                                    string strSqlDetail = @"call public.sp_insertreceiptdetail2nd(:p_TicketNo, :p_gradingTypeID, :p_qty, :p_nosegel1, :p_nosegel2)";

                                    using (NpgsqlCommand cmddetail = new NpgsqlCommand(strSqlDetail, con))
                                    {
                                        cmddetail.CommandType = CommandType.Text;
                                        cmddetail.Parameters.AddWithValue("p_TicketNo", DbType.String).Value = item.TicketNo.Trim();
                                        cmddetail.Parameters.AddWithValue("p_gradingTypeID", DbType.String).Value = item.GradingTypeId.Trim();
                                        cmddetail.Parameters.AddWithValue("p_qty", DbType.Int32).Value = Int32.Parse(item.Quantity.ToString());
                                        cmddetail.Parameters.AddWithValue("p_nosegel1", DbType.String).Value = item.NoSegel2.ToString();
                                        cmddetail.Parameters.AddWithValue("p_nosegel2", DbType.String).Value = item.NoSegel2.ToString();
                                        cmddetail.CommandType = CommandType.Text;
                                        //con.Open();
                                        int affetecdCountDetail = cmddetail.ExecuteNonQuery();
                                    }
                                }

                                string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                                using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                                {
                                    cmdTotalNet.CommandType = CommandType.Text;
                                    cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listReceipt.TicketNo.Trim();
                                    using (NpgsqlDataReader reader = cmdTotalNet.ExecuteReader())
                                    {
                                        reader.Read();
                                        _NetReceiptTotalModel.TicketNo = (string)reader["TicketNo"].ToString();
                                        _NetReceiptTotalModel.Quantity = double.Parse(reader["Quantity"] == DBNull.Value ? "0" : reader["Quantity"].ToString());
                                    }
                                }
                            }
                            else
                            {

                                _NetReceiptTotalModel.TicketNo = listReceipt.TicketNo.Trim();
                                _NetReceiptTotalModel.Quantity = 0;
                            }


                            string strSqlVehicle = @"update public.""WBVEHICLE""  set ""VehFlag"" = '0' where ""VehicleID"" = '" + listReceipt.VehicleID.Trim() + "' ";
                            using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSqlVehicle, con))
                            {
                                cmdVehicle.CommandType = CommandType.Text;
                                cmdVehicle.ExecuteNonQuery();
                            }

                            string strSqlNet = @"select * from public.""WBTRXETC"" where ""TicketNo"" = '" + listReceipt.TicketNo.Trim() + "' ";
                            using (NpgsqlCommand cmdNet = new NpgsqlCommand(strSqlNet, con))
                            {
                                cmdNet.CommandType = CommandType.Text;

                                using (NpgsqlDataReader reader = cmdNet.ExecuteReader())
                                {
                                    reader.Read();

                                    DateTime dt1st = Convert.ToDateTime(reader["TrxDateIn"]);
                                    //DateTime v_dt1s = DateTime.ParseExact(dt1st.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);
                                    DateTime dt2nd = Convert.ToDateTime(reader["TrxDateOut"]);
                                    //DateTime v_dt2nd = DateTime.ParseExact(dt2nd.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);


                                    string Weight1st = "1ST";
                                    string Date1ST = dt1st.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    string Hour1ST = dt1st.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                                    string Mode1St = ((string)reader["WBFlag1"] == "A") ? "Normal" : "Manual";
                                    double Heavy1ST = double.Parse(reader["Weight1ST"].ToString());

                                    NetWeightModel _NetWeightModel1ST = new NetWeightModel();
                                    _NetWeightModel1ST.Weight = Weight1st;
                                    _NetWeightModel1ST.DateTransaction = Date1ST;
                                    _NetWeightModel1ST.Hours = Hour1ST;
                                    _NetWeightModel1ST.Mode = Mode1St;
                                    _NetWeightModel1ST.WeightHeavy = Heavy1ST;
                                    _NetWeightModel.Add(_NetWeightModel1ST);

                                    NetWeightModel _NetWeightModel2ND = new NetWeightModel();
                                    string Weight2ND = "2ND";
                                    string Date2ND = dt2nd.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    string Hour2ND = dt2nd.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                                    string Mode2ND = ((string)reader["WBFlag2"] == "A") ? "Normal" : "Manual";
                                    double Heavy2ND = double.Parse(reader["Weight2ND"].ToString());

                                    _NetWeightModel2ND.Weight = Weight2ND;
                                    _NetWeightModel2ND.DateTransaction = Date2ND;
                                    _NetWeightModel2ND.Hours = Hour2ND;
                                    _NetWeightModel2ND.Mode = Mode2ND;
                                    _NetWeightModel2ND.WeightHeavy = Heavy2ND;
                                    _NetWeightModel.Add(_NetWeightModel2ND);

                                }


                                double Net = _NetWeightModel[0].WeightHeavy - _NetWeightModel[1].WeightHeavy - _NetReceiptTotalModel.Quantity;
                                //double DesctpacthQty = GetSumQtyDespact(listReceipt.ContractNo.Trim()) + Net;

                                //double QtyContract = GetWBCONTRACT(listReceipt.ContractNo.Trim()).Qty;

                                //string DeliveryStatus = DesctpacthQty >= QtyContract ? "0" : "1";
                                //string strSqContract = @"update public.""WBCONTRACT""  set ""DeliveryStatus"" =" + DeliveryStatus + @", ""DespatchQty"" = '" + DesctpacthQty.ToString() + "' " + @"where ""ContractNo"" = '" + listReceipt.ContractNo.Trim() + "' ";
                                //using (NpgsqlCommand cmdContract = new NpgsqlCommand(strSqContract, con))
                                //{
                                //    cmdContract.CommandType = CommandType.Text;
                                //    cmdContract.ExecuteNonQuery();
                                //}
                            }



                            result = true;
                            trans.Commit();
                            con.Close();
                        }
                        catch (NpgsqlException err)
                        {

                            throw new NpgsqlException(err.Message);
                        }

                    }

                    return Tuple.Create(result, _NetWeightModel, _NetReceiptTotalModel);
                }
            }
            catch (NpgsqlException err)
            {
                throw new NpgsqlException(err.Message);
            }
        }

        public ReceiptModelNd GetAllReceiptByTicket(string TicketNo)
        {
            ReceiptModelNd wBTRXes = new ReceiptModelNd();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from get_receiptbyticket(:ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();

                            wBTRXes.TicketNo = (string)reader["ticketno"];
                            wBTRXes.Created = (DateTime)reader["created"];
                            wBTRXes.UnitCode = (string)reader["unitcode"];
                            wBTRXes.UnitName = (string)reader["unitname"];
                            wBTRXes.LetterNo = (string)reader["letterno"]; 
                            wBTRXes.ProductName = (string)reader["productname"];
                            wBTRXes.ContractNo = (string)reader["contractno"];
                            wBTRXes.BPName = (reader["bpname"] == DBNull.Value) ? "" : (string)reader["bpname"];
                            wBTRXes.VehicleID = (reader["vehicleid"] == DBNull.Value) ? "" : (string)reader["vehicleid"];
                            wBTRXes.Transporter = "";//(reader["transporter"] == DBNull.Value) ? "" : (string)reader["transporter"];
                            wBTRXes.DriverName = (reader["drivername"] == DBNull.Value) ? "" : (string)reader["drivername"];
                            wBTRXes.WBStatus = (reader["wbstatus"] == DBNull.Value) ? "" : (string)reader["wbstatus"];
                            wBTRXes.Lisense = (reader["lisense"] == DBNull.Value) ? "" : (string)reader["lisense"];
                            wBTRXes.Weight1st = double.Parse(reader["weight1st"].ToString());
                            wBTRXes.Weight2nd = double.Parse(reader["weight2nd"].ToString());
                            wBTRXes.WBImagefront1 = reader["wbimagefront1"] == DBNull.Value ? new byte[0] : (byte[])reader["wbimagefront1"];
                            wBTRXes.WBImagefront2 = reader["wbimagefront2"] == DBNull.Value ? new byte[0] : (byte[])reader["wbImagefront2"];
                            wBTRXes.Potongan= double.Parse(reader["potongan"].ToString());
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }

        public ReceiptModelNd GetAllOtherReceiptByTicket(string TicketNo)
        {
            ReceiptModelNd wBTRXes = new ReceiptModelNd();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from get_otherreceiptbyticket(:ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();

                            wBTRXes.TicketNo = (string)reader["ticketno"];
                            wBTRXes.Created = (DateTime)reader["created"];
                            wBTRXes.UnitCode = (string)reader["unitcode"];
                            wBTRXes.UnitName = (string)reader["unitname"];
                            wBTRXes.LetterNo = (string)reader["letterno"];
                            wBTRXes.ProductName = (string)reader["productname"];
                            //wBTRXes.ContractNo = (string)reader["contractno"];
                            wBTRXes.BPName = (reader["bpname"] == DBNull.Value) ? "" : (string)reader["bpname"];
                            wBTRXes.VehicleID = (reader["vehicleid"] == DBNull.Value) ? "" : (string)reader["vehicleid"];
                            wBTRXes.Transporter = "";//(reader["transporter"] == DBNull.Value) ? "" : (string)reader["transporter"];
                            wBTRXes.DriverName = (reader["drivername"] == DBNull.Value) ? "" : (string)reader["drivername"];
                            wBTRXes.WBStatus = (reader["wbstatus"] == DBNull.Value) ? "" : (string)reader["wbstatus"];
                            wBTRXes.Lisense = (reader["lisense"] == DBNull.Value) ? "" : (string)reader["lisense"];
                            wBTRXes.Weight1st = double.Parse(reader["weight1st"].ToString());
                            wBTRXes.Weight2nd = double.Parse(reader["weight2nd"].ToString());
                            wBTRXes.WBImagefront1 = reader["wbimagefront1"] == DBNull.Value ? new byte[0] : (byte[])reader["wbimagefront1"];
                            wBTRXes.WBImagefront2 = reader["wbimagefront2"] == DBNull.Value ? new byte[0] : (byte[])reader["wbImagefront2"];
                            wBTRXes.Potongan = double.Parse(reader["potongan"].ToString());
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }

        public List<WBTRXBLOCK1st> GetAllReceipDetailOneSttByTicket(string TicketNo)
        {
            List<WBTRXBLOCK1st> wBTRXes = new List<WBTRXBLOCK1st>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getreceiptdetailrsonest(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                WBTRXBLOCK1st entity = new WBTRXBLOCK1st();
                                entity.BlockID = (string)reader["BlockID"];
                                entity.TicketNo = (string)reader["TicketNo"];
                                entity.Division = (string)reader["Division"];
                                entity.YoP = (int)reader["Yop"];
                                entity.BunchesQty = double.Parse(reader["BunchesQty"].ToString());
                                entity.LFQty = double.Parse(reader["LFQty"].ToString());
                                entity.Estimation = double.Parse(reader["Estimation"].ToString());
                                entity.Weight = (reader["Weight"] == DBNull.Value) ? 0 : double.Parse(reader["Weight"].ToString());
                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }

        public List<WBTRXGRADING> GetAllReceipDetailSecondNdByTicket(string TicketNo)
        {
            List<WBTRXGRADING> wBTRXes = new List<WBTRXGRADING>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getreceiptdetailrsecondnd(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                WBTRXGRADING entity = new WBTRXGRADING();
                                entity.ID = Int32.Parse(reader["id"].ToString());
                                entity.TicketNo = (string)reader["ticketno"];
                                entity.GradingTypeID = (string)reader["gradingtypeid"];
                                entity.GradingName = (string)reader["gradingname"];
                                entity.Quantity = double.Parse(reader["quantity"].ToString());                              
                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;

                }
                catch (NpgsqlException err)
                {
                    throw new NpgsqlException(err.Message);
                }
            }
        }

        public TranspoterModel GetTransporter(string VehicleId)
        {
            try
            {
                TranspoterModel entity = new TranspoterModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getTransporter(:p_vehicleid)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_vehicleid", VehicleId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            entity.VehicleID = (string)reader["VehicleID"];
                            entity.DriverName = (string)reader["DriverName"];
                            entity.BPCode = (string)reader["BPCode"];
                            entity.BPName = (string)reader["BPName"];
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

        public Tuple<List<NetWeightModel>, NetReceiptTotalModel> GetNetWeight(string TicketNo)
        {
            try
            {
                List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();
                NetReceiptTotalModel _NetReceiptTotalModel = new NetReceiptTotalModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                

                    string strSql = @"select * from public.""WBTRX"" where ""TicketNo"" = '" + TicketNo + "' ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();


                            DateTime dt1st = Convert.ToDateTime(reader["TrxDateIn"]);
                            //DateTime v_dt1s = DateTime.ParseExact(dt1st.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);
                            DateTime dt2nd = Convert.ToDateTime(reader["TrxDateOut"]);
                            //DateTime v_dt2nd = DateTime.ParseExact(dt2nd.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);


                            string Weight1st = "1ST";
                            string Date1ST = dt1st.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string Hour1ST = dt1st.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
                            string Mode1St = ((string)reader["WBFlag1"] == "A") ? "Normal" : "Manual";
                            double Heavy1ST = double.Parse(reader["Weight1ST"].ToString());

                            NetWeightModel _NetWeightModel1ST = new NetWeightModel();
                            _NetWeightModel1ST.Weight = Weight1st;
                            _NetWeightModel1ST.DateTransaction = Date1ST;
                            _NetWeightModel1ST.Hours = Hour1ST;
                            _NetWeightModel1ST.Mode = Mode1St;
                            _NetWeightModel1ST.WeightHeavy = Heavy1ST;
                            _NetWeightModel.Add(_NetWeightModel1ST);

                            NetWeightModel _NetWeightModel2ND = new NetWeightModel();
                            string Weight2ND = "2ND";
                            string Date2ND = dt2nd.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string Hour2ND = dt2nd.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
                            string Mode2ND = ((string)reader["WBFlag2"] == "A") ? "Normal" : "Manual";
                            double Heavy2ND = double.Parse(reader["Weight2ND"].ToString());

                            _NetWeightModel2ND.Weight = Weight2ND;
                            _NetWeightModel2ND.DateTransaction = Date2ND;
                            _NetWeightModel2ND.Hours = Hour2ND;
                            _NetWeightModel2ND.Mode = Mode2ND;
                            _NetWeightModel2ND.WeightHeavy = Heavy2ND;
                            _NetWeightModel.Add(_NetWeightModel2ND);
                        }

                        

                        cmd.Dispose();
                    }
                    string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                    using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                    {
                        cmdTotalNet.CommandType = CommandType.Text;
                        cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo;
                        using (NpgsqlDataReader readernet = cmdTotalNet.ExecuteReader())
                        {
                            readernet.Read();
                            
                            var x = readernet.HasRows;
                            if (readernet.HasRows)
                            {
                                _NetReceiptTotalModel.TicketNo = TicketNo;
                                _NetReceiptTotalModel.Quantity = double.Parse(readernet["quantity"].ToString());
                            }
                            else {
                                _NetReceiptTotalModel.TicketNo = TicketNo;
                                _NetReceiptTotalModel.Quantity = 0;
                            }
                          
                        }
                        cmdTotalNet.Dispose();
                    }
                   
                    con.Close();

                    return Tuple.Create(_NetWeightModel, _NetReceiptTotalModel); 
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public Tuple<List<NetWeightModel>, NetReceiptTotalModel> GetOtherNetWeight(string TicketNo)
        {
            try
            {
                List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();
                NetReceiptTotalModel _NetReceiptTotalModel = new NetReceiptTotalModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();



                    string strSql = @"select * from public.""WBTRXETC"" where ""TicketNo"" = '" + TicketNo + "' ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();


                            DateTime dt1st = Convert.ToDateTime(reader["TrxDateIn"]);
                            //DateTime v_dt1s = DateTime.ParseExact(dt1st.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);
                            DateTime dt2nd = Convert.ToDateTime(reader["TrxDateOut"]);
                            //DateTime v_dt2nd = DateTime.ParseExact(dt2nd.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);


                            string Weight1st = "1ST";
                            string Date1ST = dt1st.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string Hour1ST = dt1st.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
                            string Mode1St = ((string)reader["WBFlag1"] == "A") ? "Normal" : "Manual";
                            double Heavy1ST = double.Parse(reader["Weight1ST"].ToString());

                            NetWeightModel _NetWeightModel1ST = new NetWeightModel();
                            _NetWeightModel1ST.Weight = Weight1st;
                            _NetWeightModel1ST.DateTransaction = Date1ST;
                            _NetWeightModel1ST.Hours = Hour1ST;
                            _NetWeightModel1ST.Mode = Mode1St;
                            _NetWeightModel1ST.WeightHeavy = Heavy1ST;
                            _NetWeightModel.Add(_NetWeightModel1ST);

                            NetWeightModel _NetWeightModel2ND = new NetWeightModel();
                            string Weight2ND = "2ND";
                            string Date2ND = dt2nd.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string Hour2ND = dt2nd.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
                            string Mode2ND = ((string)reader["WBFlag2"] == "A") ? "Normal" : "Manual";
                            double Heavy2ND = double.Parse(reader["Weight2ND"].ToString());

                            _NetWeightModel2ND.Weight = Weight2ND;
                            _NetWeightModel2ND.DateTransaction = Date2ND;
                            _NetWeightModel2ND.Hours = Hour2ND;
                            _NetWeightModel2ND.Mode = Mode2ND;
                            _NetWeightModel2ND.WeightHeavy = Heavy2ND;
                            _NetWeightModel.Add(_NetWeightModel2ND);
                        }



                        cmd.Dispose();
                    }
                    string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                    using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                    {
                        cmdTotalNet.CommandType = CommandType.Text;
                        cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo;
                        using (NpgsqlDataReader readernet = cmdTotalNet.ExecuteReader())
                        {
                            readernet.Read();

                            var x = readernet.HasRows;
                            if (readernet.HasRows)
                            {
                                _NetReceiptTotalModel.TicketNo = TicketNo;
                                _NetReceiptTotalModel.Quantity = double.Parse(readernet["quantity"].ToString());
                            }
                            else
                            {
                                _NetReceiptTotalModel.TicketNo = TicketNo;
                                _NetReceiptTotalModel.Quantity = 0;
                            }

                        }
                        cmdTotalNet.Dispose();
                    }

                    con.Close();

                    return Tuple.Create(_NetWeightModel, _NetReceiptTotalModel);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public string GetYOP(string BlockId)
        {
            string YoP = "";
            try
            {
                WBBLOCKModel entity = new WBBLOCKModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select * from public.""WBBLOCK"" where ""BlockID"" = '" + BlockId + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            entity.BlockID = (string)reader["BlockID"];
                            entity.MoP = (int)reader["MoP"];
                            entity.YoP = (int)reader["YoP"];
                            entity.CurrentPlanted = double.Parse(reader["CurrentPlanted"].ToString());
                            entity.UnitCode = (string)reader["UnitCode"];
                            entity.Active = (bool)reader["Active"];

                        }
                        cmd.Dispose();
                    }

                    con.Close();
                    YoP = entity.YoP.ToString();
                    return YoP;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public VehicleDllModel GetDriver(string VehicleId)
        {
            try
            {
                VehicleDllModel entity = new VehicleDllModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getDriverByVehicle(:p_vehicleid)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_vehicleid", VehicleId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            entity.VehicleID = (string)reader["VehicleID"];
                            entity.DriverName = (string)reader["DriverName"];
                            entity.BPCode = (string)reader["BPCode"];
                            entity.LicenseNo = (string)reader["LicenseNo"];
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

        public double GetIDGrading()
        {
            int p_ID = 0;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select max(""ID"") as ID from public.""WBTRXGRADING""";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            p_ID = int.Parse((reader["ID"] == DBNull.Value ? "0" : reader["ID"].ToString()));

                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return p_ID;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public double GetIDBlock()
        {
            int p_ID = 0;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select max(""ID"") as ID from public.""WBTRXBLOCK""";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            p_ID = int.Parse((reader["ID"] == DBNull.Value ? "0" : reader["ID"].ToString()));

                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return p_ID;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public List<ReceiptModel> GetAllReceipByFilter(string TicketNo, string ContractNo, string StartDate, string EndDate)
        {
            string strSql = "";

            if (TicketNo != "" && ContractNo == "" && StartDate == "" && EndDate == "")
            {
                strSql = @"select * from public.get_receiptfilterbyticket(:p_ticketno)";
            }

            if (TicketNo == "" && ContractNo != "" && StartDate == "" && EndDate == "")
            {
                strSql = @"select * from public.get_receiptbyContract(:p_contractno)";
            }
            if (TicketNo == "" && ContractNo == "" && StartDate != "" && EndDate != "")
            {
                strSql = strSql + @"select * from public.get_receiptbyDate(:p_startdate,:p_enddate)";
            }

            if (TicketNo == "" && ContractNo == "" && StartDate == "" && EndDate == "")
            {
                strSql = strSql + @"select * from public.get_receipt()";
            }

            List<ReceiptModel> wBTRXes = new List<ReceiptModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        if (TicketNo != "" && ContractNo == "" && StartDate == "" && EndDate == "")
                        {
                            cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        }

                        if (TicketNo == "" && ContractNo != "" && StartDate == "" && EndDate == "")
                        {
                            cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = ContractNo.Trim();
                        }

                        if (TicketNo == "" && ContractNo == "" && StartDate != "" && EndDate != "")
                        {
                            cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(StartDate);
                            cmd.Parameters.AddWithValue("p_enddate", DbType.DateTime).Value = DateTime.Parse(EndDate);
                        }

                        //else 
                        //{
                        //    cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(StartDate.Trim());
                        //    cmd.Parameters.AddWithValue("p_enddate", DbType.DateTime).Value = DateTime.Parse(EndDate.Trim());
                        //}

                        cmd.CommandType = CommandType.Text;

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReceiptModel entity = new ReceiptModel();
                                entity.TicketNo = (string)reader["TicketNo"];
                                entity.Created = (DateTime)reader["Created"];
                                entity.UnitName = (string)reader["UnitName"];
                                entity.ProductName = (string)reader["ProductName"];
                                entity.ContractNo = (string)reader["ContractNo"];
                                entity.BPName = (reader["BPName"] == DBNull.Value) ? "" : (string)reader["BPName"];
                                entity.VehicleID = (reader["VehicleID"] == DBNull.Value) ? "" : (string)reader["VehicleID"];
                                entity.WBStatus = (reader["WBStatus"] == DBNull.Value) ? "" : (string)reader["WBStatus"];
                                entity.weight1st = (reader["weight1st"] == DBNull.Value) ? "" : (string)reader["weight1st"];
                                entity.weight2nd = (reader["weight2nd"] == DBNull.Value) ? "" : (string)reader["weight2nd"];
                                entity.status = (reader["status"] == DBNull.Value) ? "" : (string)reader["status"];
                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }

        public List<ReceiptModel> GetAllOtherReceipByFilter(string TicketNo, string StartDate, string EndDate)
        {
            List<ReceiptModel> wBTRXes = new List<ReceiptModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = "";

                    //if (TicketNo != "" && ContractNo == "" && StartDate == "" && EndDate == "")
                    //{
                    //    strSql = @"select * from public.get_otherreceiptfilterbyticket(:p_ticketno)";
                    //}

                    //else if (TicketNo == "" && ContractNo != "" && StartDate == "" && EndDate == "")
                    //{
                    //    strSql = @"select * from public.get_receiptbyContract(:p_contractno)";
                    //}
                     if (TicketNo == ""  && StartDate != "" && EndDate != "")
                    {
                        strSql = strSql + @"select * from public.get_otherreceiptbyDate(:p_startdate,:p_enddate)";
                    }
                    else
                    {
                        strSql = strSql + @"select * from public.get_otherreceipt()";
                    }

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        if (TicketNo != "" && StartDate == "" && EndDate == "")
                        {
                            cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        }

                        //if (TicketNo == ""  && StartDate == "" && EndDate == "")
                        //{
                        //    cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = ContractNo.Trim();
                        //}

                        else if (TicketNo == ""  && StartDate != "" && EndDate != "")
                        {
                            cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(StartDate.Trim());
                            cmd.Parameters.AddWithValue("p_enddate", DbType.DateTime).Value = DateTime.Parse(EndDate.Trim());
                        }

                        //else 
                        //{
                        //    cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(StartDate.Trim());
                        //    cmd.Parameters.AddWithValue("p_enddate", DbType.DateTime).Value = DateTime.Parse(EndDate.Trim());
                        //}

                        cmd.CommandType = CommandType.Text;

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReceiptModel entity = new ReceiptModel();
                                entity.TicketNo = (string)reader["ticketNo"];
                                entity.Created = (DateTime)reader["created"];
                                entity.UnitName = (string)reader["unitname"];
                                entity.ProductName = (string)reader["productname"];
                                //entity.ContractNo = (string)reader["contractno"];
                                entity.BPName = (reader["bpname"] == DBNull.Value) ? "" : (string)reader["bpname"];
                                entity.VehicleID = (reader["vehicleid"] == DBNull.Value) ? "" : (string)reader["vehicleid"];
                                entity.WBStatus = (reader["wbstatus"].ToString() == "F" || reader["wbstatus"].ToString() == "First") ? "First" : "Done";
                                wBTRXes.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBTRXes;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }
            }
        }
        public WBCONTRACT GetWBCONTRACT(string ContractNo)
        {
            try
            {
                WBCONTRACT entity = new WBCONTRACT();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select * from public.""WBCONTRACT"" where ""ContractNo"" = '" + ContractNo + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            entity.ContractNo = (string)reader["ContractNo"];
                            entity.ContractDate = DateTime.Parse(reader["ContractDate"].ToString());
                            entity.ExpDate = DateTime.Parse(reader["ExpDate"].ToString());
                            entity.ProductCode = (string)reader["ProductCode"];
                            entity.BPCode = (string)reader["BPCode"];
                            entity.Qty = double.Parse(reader["Qty"].ToString());
                            entity.Toleransi =reader["Toleransi"] == DBNull.Value ? 0 : double.Parse(reader["Toleransi"].ToString());
                            entity.UnitPrice = double.Parse(reader["UnitPrice"].ToString());
                            entity.PremiumPrice = reader["PremiumPrice"] == DBNull.Value ? 0 : double.Parse(reader["PremiumPrice"].ToString());
                            entity.PPN = double.Parse(reader["PPN"].ToString());
                            entity.FinalUnitPrice = double.Parse(reader["FinalUnitPrice"].ToString());
                            entity.TotalPrice = double.Parse(reader["TotalPrice"].ToString());
                            entity.DespatchQty = double.Parse(reader["DespatchQty"].ToString());
                            entity.DeliveryStatus = (string)reader["DeliveryStatus"];

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

        public List<CONTRACTPRODUCT> GetContractProduct(string ContractNo)
        {

            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    List<CONTRACTPRODUCT> cONTRACTPRODUCTs = new List<CONTRACTPRODUCT>();
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getbpcontract(:p_contractno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = ContractNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                CONTRACTPRODUCT entity = new CONTRACTPRODUCT();
                                entity.BPCode = (string)reader["bpcode"].ToString();
                                entity.ProductCode = (string)reader["productcode"];
                                entity.ProductName = (string)reader["ProductName"];
                                cONTRACTPRODUCTs.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return cONTRACTPRODUCTs;

                }
                catch (NpgsqlException err)
                {
                    throw new NpgsqlException(err.Message);
                }
            }
        }

        public List<WBBLOCKModel> GetBlockUnit(string UnitCode)
        {
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    List<WBBLOCKModel> wBBLOCKModel = new List<WBBLOCKModel>();
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getblockbyunit(:p_unitcode)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_unitcode", DbType.String).Value = UnitCode.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                WBBLOCKModel entity = new WBBLOCKModel();
                                entity.BlockID = (string)reader["blockid"].ToString();
                                entity.MoP = Int32.Parse(reader["mop"].ToString());
                                entity.YoP = Int32.Parse(reader["yop"].ToString());
                                entity.CurrentPlanted = double.Parse(reader["currentplanted"].ToString());
                                entity.Active = bool.Parse(reader["active"].ToString());
                                wBBLOCKModel.Add(entity);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBBLOCKModel;

                }
                catch (NpgsqlException err)
                {
                    throw new NpgsqlException(err.Message);
                }
            }
        }

        public double GetSumQtyDespact(string ContractNo)
        {
            double totalDespact = 0;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select * from public.get_descpactqtycontract(:p_contractno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                       
                        cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = ContractNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();                        
                            totalDespact = double.Parse((reader["totalnet"] == DBNull.Value ? "0" : reader["totalnet"].ToString()));
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return totalDespact;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public bool CountingPrint(string TicketNo)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = @"update public.""WBTRX""  set ""print"" = ""print"" + 1 where ""TicketNo"" = '" + TicketNo.Trim() + "' ";
                    using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSql, con))
                    {
                        cmdVehicle.CommandType = CommandType.Text;
                        cmdVehicle.ExecuteNonQuery();
                    }
                    result = true;
                }
                return result;
            }
            catch (NpgsqlException err)
            {
                result = false;
                Console.WriteLine(err.Message);
                return result;
            }

            
        }

        public double GetDeducation(string TicketNo)
        {
            double Qty = 0;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = @"select * from  public.form_SumRecDeduction(:p_ticketno)";
                  
                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.Parameters.AddWithValue("p_ticketno", TicketNo);
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                           Qty = double.Parse(reader["Quantity"] ==  DBNull.Value ? "0" : reader["Quantity"].ToString());
                           
                        }
                        cmd.Dispose();
                    }
                }
                return Qty;
            }
            catch (NpgsqlException err)
            {
                throw new NpgsqlException(err.Message);
                //Console.WriteLine(err.Message);
            }
        }

        public double GetWeight1st(string TicketNo)
        {
            double Heavy1ST = 0;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSqlNet = @"select * from public.""WBTRX"" where ""TicketNo"" = '" + TicketNo + "' ";
                    using (NpgsqlCommand cmdNet = new NpgsqlCommand(strSqlNet, con))
                    {
                        cmdNet.CommandType = CommandType.Text;

                        using (NpgsqlDataReader reader = cmdNet.ExecuteReader())
                        {
                            reader.Read();


                            Heavy1ST = double.Parse(reader["Weight1ST"].ToString());

                        }

                    }

                    return Heavy1ST;
                }
            }
            catch (Exception)
            {
                return Heavy1ST;
                throw;
            }
           

        }

        public double GetOtherWeight1st(string TicketNo)
        {
            double Heavy1ST = 0;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSqlNet = @"select * from public.""WBTRXETC"" where ""TicketNo"" = '" + TicketNo + "' ";
                    using (NpgsqlCommand cmdNet = new NpgsqlCommand(strSqlNet, con))
                    {
                        cmdNet.CommandType = CommandType.Text;

                        using (NpgsqlDataReader reader = cmdNet.ExecuteReader())
                        {
                            reader.Read();


                            Heavy1ST = double.Parse(reader["Weight1ST"].ToString());

                        }

                    }

                    return Heavy1ST;
                }
            }
            catch (Exception)
            {
                return Heavy1ST;
                throw;
            }


        }

        public Tuple<bool, List<NetWeightModel>, NetReceiptTotalModel> ApprovalReceipt(WBTRXModel listReceipt, List<WBTRXBLOCK> listReceiptDetail, List<WBTRXGRADING2nd> ReceiptDetail2Nd)
        {
            bool result = false;

            try
            {

                bool DelBlock = DeleteBlock(listReceipt.TicketNo.Trim());
                if (!DelBlock)
                {
                    throw new Exception("Failed Deleted Block");
                }

                bool DelGrading = DeleteGrading(listReceipt.TicketNo.Trim());
                if (!DelGrading)
                {
                    throw new Exception("Failed Deleted Grading");
                }



                List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();
                NetReceiptTotalModel _NetReceiptTotalModel = new NetReceiptTotalModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;

                    string strSql2nd = @"call public.sp_approvereceipt(
                                     :p_ticketno								
                                    ,:p_nocoverletter  
                                    ,:p_weight1st
                                    ,:p_weight2nd
                                    ,:p_driver
                                    ,:p_lisensi                                 
                                    ,:p_vehicle
                                    ,:p_status
                                    ,:p_useridapproval
                                    ,:p_updated)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql2nd, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listReceipt.TicketNo.Trim();                      
                        cmd.Parameters.AddWithValue("p_nocoverletter", DbType.String).Value = listReceipt.NoCoverLetter.Trim();
                        cmd.Parameters.AddWithValue("p_weight1st", DbType.Int32).Value = Int32.Parse(listReceipt.Weight1ST.ToString());
                        cmd.Parameters.AddWithValue("p_weight2nd", DbType.Int32).Value = Int32.Parse(listReceipt.Weight2ND.ToString());
                        cmd.Parameters.AddWithValue("p_driver", DbType.String).Value = listReceipt.DriverName.ToString();
                        cmd.Parameters.AddWithValue("p_lisensi", DbType.String).Value = listReceipt.LicenseNo.ToString();                        
                        cmd.Parameters.AddWithValue("p_vehicle", DbType.String).Value = listReceipt.VehicleID.Trim();
                        cmd.Parameters.AddWithValue("p_status", DbType.String).Value = listReceipt.Status.Trim();
                        cmd.Parameters.AddWithValue("p_useridapproval", DbType.String).Value = "";//listReceipt.UserIDApproval.Trim();
                        cmd.Parameters.AddWithValue("p_updated", DbType.Date).Value = Convert.ToDateTime(listReceipt.Updated); // DateTime.Parse(listReceipt.Updated.ToString());
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();

                            if (listReceiptDetail != null)
                            {
                                foreach (var item in listReceiptDetail)
                                {
                                    string strSqlDetail = @"call public.sp_InsertReceiptDetail(:p_TicketNo, :p_BlockID, :p_BunchesQty, :p_LFQty, :p_Estimation, :p_Weight)";

                                    using (NpgsqlCommand cmddetail = new NpgsqlCommand(strSqlDetail, con))
                                    {
                                        cmddetail.CommandType = CommandType.Text;
                                        cmddetail.Parameters.AddWithValue("p_TicketNo", DbType.String).Value = item.TicketNo.Trim();
                                        cmddetail.Parameters.AddWithValue("p_BlockID", DbType.String).Value = item.BlockID.Trim();
                                        cmddetail.Parameters.AddWithValue("p_BunchesQty", DbType.Int32).Value = Int32.Parse(item.BunchesQty.ToString());
                                        cmddetail.Parameters.AddWithValue("p_LFQty", DbType.Int32).Value = Int32.Parse(item.LFQty.ToString());
                                        cmddetail.Parameters.AddWithValue("p_Estimation", DbType.Int32).Value = Int32.Parse(item.Estimation.ToString());
                                        cmddetail.Parameters.AddWithValue("p_Weight", DbType.Int32).Value = Int32.Parse(item.Weight.ToString());
                                        cmddetail.CommandType = CommandType.Text;

                                        int affetecdCountDetail = cmddetail.ExecuteNonQuery();

                                    }
                                }
                            }


                         
                            if (ReceiptDetail2Nd != null)
                            {
                                foreach (var item in ReceiptDetail2Nd)
                                {
                                    string strSqlDetail = @"call public.sp_insertreceiptdetail2nd(:p_TicketNo, :p_gradingTypeID, :p_qty, :p_nosegel1, :p_nosegel2)";

                                    using (NpgsqlCommand cmddetail = new NpgsqlCommand(strSqlDetail, con))
                                    {
                                        cmddetail.CommandType = CommandType.Text;
                                        cmddetail.Parameters.AddWithValue("p_TicketNo", DbType.String).Value = item.TicketNo.Trim();
                                        cmddetail.Parameters.AddWithValue("p_gradingTypeID", DbType.String).Value = item.GradingTypeId.Trim();
                                        cmddetail.Parameters.AddWithValue("p_qty", DbType.Decimal).Value = Decimal.Parse(item.Quantity.ToString());
                                        cmddetail.Parameters.AddWithValue("p_nosegel1", DbType.String).Value = item.NoSegel2.ToString();
                                        cmddetail.Parameters.AddWithValue("p_nosegel2", DbType.String).Value = item.NoSegel2.ToString();
                                        cmddetail.CommandType = CommandType.Text;
                                        //con.Open();
                                        int affetecdCountDetail = cmddetail.ExecuteNonQuery();
                                    }
                                }

                                string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                                using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                                {
                                    cmdTotalNet.CommandType = CommandType.Text;
                                    cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listReceipt.TicketNo.Trim();
                                    using (NpgsqlDataReader reader = cmdTotalNet.ExecuteReader())
                                    {
                                        reader.Read();
                                        _NetReceiptTotalModel.TicketNo = (string)reader["TicketNo"].ToString();
                                        _NetReceiptTotalModel.Quantity = double.Parse(reader["Quantity"] == DBNull.Value ? "0" : reader["Quantity"].ToString());
                                    }
                                }
                            }
                            else
                            {

                                _NetReceiptTotalModel.TicketNo = listReceipt.TicketNo.Trim();
                                _NetReceiptTotalModel.Quantity = 0;
                            }


                            string strSqlVehicle = @"update public.""WBVEHICLE""  set ""VehFlag"" = '0' where ""VehicleID"" = '" + listReceipt.VehicleID.Trim() + "' ";
                            using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSqlVehicle, con))
                            {
                                cmdVehicle.CommandType = CommandType.Text;
                                cmdVehicle.ExecuteNonQuery();
                            }

                            string strSqlNet = @"select * from public.""WBTRX"" where ""TicketNo"" = '" + listReceipt.TicketNo.Trim() + "' ";
                            using (NpgsqlCommand cmdNet = new NpgsqlCommand(strSqlNet, con))
                            {
                                cmdNet.CommandType = CommandType.Text;

                                using (NpgsqlDataReader reader = cmdNet.ExecuteReader())
                                {
                                    reader.Read();

                                    DateTime dt1st = Convert.ToDateTime(reader["TrxDateIn"]);
                                    //DateTime v_dt1s = DateTime.ParseExact(dt1st.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);
                                    DateTime dt2nd = Convert.ToDateTime(reader["TrxDateOut"]);
                                    //DateTime v_dt2nd = DateTime.ParseExact(dt2nd.ToString(), "dd/MM/yyyy HH.mm.ss", CultureInfo.InvariantCulture);


                                    string Weight1st = "1ST";
                                    string Date1ST = dt1st.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    string Hour1ST = dt1st.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                                    string Mode1St = ((string)reader["WBFlag1"] == "A") ? "Normal" : "Manual";
                                    double Heavy1ST = double.Parse(reader["Weight1ST"].ToString());

                                    NetWeightModel _NetWeightModel1ST = new NetWeightModel();
                                    _NetWeightModel1ST.Weight = Weight1st;
                                    _NetWeightModel1ST.DateTransaction = Date1ST;
                                    _NetWeightModel1ST.Hours = Hour1ST;
                                    _NetWeightModel1ST.Mode = Mode1St;
                                    _NetWeightModel1ST.WeightHeavy = Heavy1ST;
                                    _NetWeightModel.Add(_NetWeightModel1ST);

                                    NetWeightModel _NetWeightModel2ND = new NetWeightModel();
                                    string Weight2ND = "2ND";
                                    string Date2ND = dt2nd.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    string Hour2ND = dt2nd.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                                    string Mode2ND = ((string)reader["WBFlag2"] == "A") ? "Normal" : "Manual";
                                    double Heavy2ND = double.Parse(reader["Weight2ND"].ToString());

                                    _NetWeightModel2ND.Weight = Weight2ND;
                                    _NetWeightModel2ND.DateTransaction = Date2ND;
                                    _NetWeightModel2ND.Hours = Hour2ND;
                                    _NetWeightModel2ND.Mode = Mode2ND;
                                    _NetWeightModel2ND.WeightHeavy = Heavy2ND;
                                    _NetWeightModel.Add(_NetWeightModel2ND);

                                }


                                double Net = _NetWeightModel[0].WeightHeavy - _NetWeightModel[1].WeightHeavy - _NetReceiptTotalModel.Quantity;
                                double DesctpacthQty = Math.Ceiling(GetSumQtyDespact(listReceipt.ContractNo.Trim()) + Net);

                                double QtyContract = GetWBCONTRACT(listReceipt.ContractNo.Trim()).Qty;

                                string DeliveryStatus = DesctpacthQty >= QtyContract ? "0" : "1";
                                string strSqContract = @"update public.""WBCONTRACT""  set ""DeliveryStatus"" =" + DeliveryStatus + @", ""DespatchQty"" = '" + DesctpacthQty.ToString() + "' " + @"where ""ContractNo"" = '" + listReceipt.ContractNo.Trim() + "' ";
                                using (NpgsqlCommand cmdContract = new NpgsqlCommand(strSqContract, con))
                                {
                                    cmdContract.CommandType = CommandType.Text;
                                    cmdContract.ExecuteNonQuery();
                                }
                            }

                         

                            result = true;
                            trans.Commit();
                            con.Close();
                        }
                        catch (NpgsqlException err)
                        {

                            throw new NpgsqlException(err.Message);
                        }

                    }

                    return Tuple.Create(result, _NetWeightModel, _NetReceiptTotalModel);
                }
            }
            catch (NpgsqlException err)
            {
                throw new NpgsqlException(err.Message);
            }
        }

        public Tuple<bool> ApproveByTicket(WBTRXModel listReceipt)
        {
            bool result = false;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;

                    string strSql2nd = @"update public.""WBTRX""  set  ""WBStatus"" ='D', ""status"" = 'C' where ""TicketNo"" = '" + listReceipt.TicketNo  +"'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql2nd, con))
                    {                      
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();

                            result = true;
                            trans.Commit();
                            con.Close();
                        }
                        catch (NpgsqlException err)
                        {

                            throw new NpgsqlException(err.Message);
                        }

                    }

                    return Tuple.Create(result);
                }
            }
            catch (NpgsqlException err)
            {
                throw new NpgsqlException(err.Message);
            }
        }

        public bool DeleteBlock(string TicketNo)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSqlDelBlock = @"delete from public.""WBTRXBLOCK"" where ""TicketNo"" = '" + TicketNo + "'";
                    using (NpgsqlCommand cmdDelBlock = new NpgsqlCommand(strSqlDelBlock, con))
                    {
                        cmdDelBlock.CommandType = CommandType.Text;                       
                        cmdDelBlock.ExecuteNonQuery();                       
                    }
                }
                return true;
            }
            catch (NpgsqlException err)
            {
                return false;
                throw new NpgsqlException(err.Message);
            }
          
        }

        public bool DeleteGrading(string TicketNo)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSqlDelGrading = @"delete from public.""WBTRXGRADING"" where ""TicketNo"" = '" + TicketNo + "'";
                    using (NpgsqlCommand cmdDelGrading = new NpgsqlCommand(strSqlDelGrading, con))
                    {
                        cmdDelGrading.CommandType = CommandType.Text;                       
                        cmdDelGrading.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (NpgsqlException err)
            {
                return false;
                throw new NpgsqlException(err.Message);
            }
        }

        public bool PostingReceipt()
        {
            try
            {
                DateTime date = DateTime.Now;
                // converting to string format
                string date_str = date.ToString("yyyy-MM-dd");
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSqlDelGrading = @"update public.""WBTRX"" set ""WBStatus"" = 'D' , ""status"" ='C' where  ""WBStatus"" = 'S' and ""WBType"" = 'Receipt' and ""status"" != 'R'";
                    using (NpgsqlCommand cmdDelGrading = new NpgsqlCommand(strSqlDelGrading, con))
                    {
                        cmdDelGrading.CommandType = CommandType.Text;
                        cmdDelGrading.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (NpgsqlException err)
            {
                return false;
                throw new NpgsqlException(err.Message);
            }
        }
    }
}