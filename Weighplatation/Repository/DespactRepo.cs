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
    public class DespactRepo : IDespact
    {
        private readonly string stringCon = ConfigurationManager.ConnectionStrings["dbcon"].ToString();
        public bool CancelDespact(string TicketNo)
        {
            throw new NotImplementedException();
        }

        public string GenerateTicketNo(string UnitCode)
        {
            string Result = "";

            Random generator = new Random();
            int r = generator.Next(100000, 999999);
            int r1 = generator.Next(100000, 1000000);
            string result = "";

            result = UnitCode + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + r.ToString();
            return result;
            //try
            //{
            //    using (NpgsqlConnection con = new NpgsqlConnection())
            //    {

            //        con.ConnectionString = stringCon;
            //        con.Open();

            //        string result = "";

            //        string strSql = @"select counter from public.get_counterreceipt();";
            //        int TicketNo = 0;
            //        int Counter = 0;
            //        using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
            //        {
            //            cmd.CommandType = CommandType.Text;
            //            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            //            {
            //                reader.Read();
            //                Counter = int.Parse(reader["counter"].ToString());
            //            }
            //            cmd.Dispose();
            //        }

            //        con.Close();
            //        TicketNo = Counter + 1;
            //        result = UnitCode + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + TicketNo.ToString("000000") + System.Environment.NewLine;

            //        return result;
            //    }
            //}
            //catch (Exception err)
            //{
            //    Console.WriteLine(err.Message);
            //    Result = "True";
            //    return Result;
            //    throw;
            //}
        }

        public DespactModelNd GetAllDespactByTicket(string TicketNo)
        {
            DespactModelNd wBTRXes = new DespactModelNd();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.get_descpactbyticket(:ticketno)";





                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();

                            wBTRXes.TicketNo = (string)reader["TicketNo"];
                            wBTRXes.Created = (DateTime)reader["Created"];
                            //wBTRXes.UnitCode= (string)reader["UnitCode"];
                            wBTRXes.UnitName = (string)reader["UnitName"];
                            wBTRXes.ProductName = (string)reader["ProductName"];
                            wBTRXes.ContractNo = (string)reader["ContractNo"];
                            wBTRXes.BPName = (reader["BPName"] == DBNull.Value) ? "" : (string)reader["BPName"];
                            wBTRXes.VehicleID = (reader["VehicleID"] == DBNull.Value) ? "" : (string)reader["VehicleID"];
                            //wBTRXes.Transporter = (reader["transporter"] == DBNull.Value) ? "" : (string)reader["transporter"];
                            wBTRXes.DriverName = (reader["DriverName"] == DBNull.Value) ? "" : (string)reader["DriverName"];
                            wBTRXes.WBStatus = (reader["WBStatus"] == DBNull.Value) ? "" : (string)reader["WBStatus"];
                            wBTRXes.Lisense = (reader["lisense"] == DBNull.Value) ? "" : (string)reader["lisense"];
                            wBTRXes.DnNo = (string)reader["DnNo"];
                            wBTRXes.Weight1st = double.Parse(reader["weight1st"].ToString());
                            wBTRXes.Weight2nd = double.Parse(reader["weight2nd"].ToString());
                            wBTRXes.WBImagefront1 = reader["wbImagefront1"] == DBNull.Value ? new byte[0] : (byte[])reader["wbImagefront1"];
                            wBTRXes.WBImagefront2 = reader["wbImagefront2"] == DBNull.Value ? new byte[0] : (byte[])reader["wbImagefront2"];

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

        public List<DespactModel> GetAllDespactByToday()
        {
            List<DespactModel> wBTRXes = new List<DespactModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    WBOWNER getUnitWBOwner = GetUnitWBOwner();
                    string strSql = @"select * from get_Despact()";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DespactModel entity = new DespactModel();
                                entity.TicketNo = (string)reader["TicketNo"];
                                entity.Created = (DateTime)reader["Created"];
                                entity.UnitName = getUnitWBOwner.UnitName;// (string)reader["UnitName"];
                                entity.ProductName = (string)reader["ProductName"];
                                entity.ContractNo = (string)reader["ContractNo"];
                                entity.BPName = (reader["BPName"] == DBNull.Value) ? "" : (string)reader["BPName"];
                                entity.VehicleID = (reader["VehicleID"] == DBNull.Value) ? "" : (string)reader["VehicleID"];
                                entity.WBStatus = (reader["WBStatus"] == DBNull.Value) ? "" : (string)reader["WBStatus"];
                                entity.weight1st = (reader["weight1st"] == DBNull.Value) ? "" : (string)reader["weight1st"];
                                entity.weight2nd = (reader["weight2nd"] == DBNull.Value) ? "" : (string)reader["weight2nd"];
                                entity.status = (reader["weight1st"] == DBNull.Value) ? "" : (string)reader["weight1st"];
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

        public List<WBTRXBLOCK1st> GetAllDespactDetailOneSttByTicket(string TicketNo)
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

        public CONTRACTPRODUCT GetCompanyContract(string ContractNo)
        {
            try
            {
                CONTRACTPRODUCT _CONTRACTPRODUCT = new CONTRACTPRODUCT();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();



                    string strSql = @"select * from public.getbpcontract(:p_contractno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_contractno", ContractNo);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            _CONTRACTPRODUCT.BPCode = (string)reader["BPCode"];
                            _CONTRACTPRODUCT.ProductCode = (string)reader["ProductCode"];
                            _CONTRACTPRODUCT.ProductName = (string)reader["ProductName"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return _CONTRACTPRODUCT;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
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

        public CONTRACTPRODUCT GetCONTRACTPRODUCT(string ContractNo)
        {
            try
            {
                CONTRACTPRODUCT _CONTRACTPRODUCT = new CONTRACTPRODUCT();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();



                    string strSql = @"select * from public.getbpcontract(:p_contractno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_contractno", ContractNo);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            _CONTRACTPRODUCT.BPCode = (string)reader["BPCode"];
                            _CONTRACTPRODUCT.ProductCode = (string)reader["ProductCode"];
                            _CONTRACTPRODUCT.ProductName = (string)reader["ProductName"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return _CONTRACTPRODUCT;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public List<ReceipDetailOneSTtModel> GetDetailDespactST()
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
                                entity.LicenseNo = (string)reader["BPCode"];
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


        public List<DOBPCODEModel> GetDllDO(string ContractNo)
        {
            List<DOBPCODEModel> wBTRXes = new List<DOBPCODEModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getbpdo(@ContractNo)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ContractNo", ContractNo);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DOBPCODEModel entity = new DOBPCODEModel();
                                entity.DoNo = (string)reader["dono"];
                                entity.BPCode = (string)reader["bpcode"];
                                entity.DoCompany = (string)reader["bpname"];

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

        public List<DOBPCODEModel> GetDONo(string ContractNo)
        {
            try
            {
                List<DOBPCODEModel> listDOBPCODEModel = new List<DOBPCODEModel>();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();



                    string strSql = @"select * from public.getbpdo(@ContractNo)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ContractNo", ContractNo);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            DOBPCODEModel _DOBPCODEModel = new DOBPCODEModel();
                            while (reader.Read())
                            {
                                _DOBPCODEModel.DoNo = (string)reader["dono"];
                                _DOBPCODEModel.BPCode = (string)reader["bpcode"];
                                _DOBPCODEModel.DoCompany = (string)reader["bpname"];
                                listDOBPCODEModel.Add(_DOBPCODEModel);
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return listDOBPCODEModel;
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public WBDOModel GetDONoBPCode(string DONo)
        {
            try
            {
                WBDOModel wBDO = new WBDOModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();



                    string strSql = @"select * from public.""WBDO"" where ""DONo"" ='" + DONo + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            wBDO.DONo = (string)reader["DoNo"];
                            wBDO.ContractNo = (string)reader["ContractNo"];
                            wBDO.DODate = DateTime.Parse(reader["DODate"].ToString());
                            wBDO.BPCode = (string)reader["BPCode"];
                            wBDO.Qty = reader["DespatchQty"] == DBNull.Value ? 0 : double.Parse(reader["Qty"].ToString());
                            wBDO.DespatchQty = reader["DespatchQty"] == DBNull.Value ? 0 : double.Parse(reader["DespatchQty"].ToString());
                            wBDO.DeliveryStatus = (string)reader["DeliveryStatus"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBDO;
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
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

        public WBGRADINGTYPE GetGradingTypeByID(string GradingTypeID)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();

                    WBGRADINGTYPE _WBGRADINGTYPE = new WBGRADINGTYPE();
                    string strSql = @"select * from public.""WBGRADINGTYPE"" where ""GradingTypeID""= '" + GradingTypeID + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            _WBGRADINGTYPE.GradingTypeID = (string)reader["GradingTypeID"];
                            _WBGRADINGTYPE.GradingName = (string)reader["GradingName"];
                            _WBGRADINGTYPE.UomID = (string)reader["UomID"];
                            _WBGRADINGTYPE.DeductionsValue = double.Parse(reader["DeductionsValue"].ToString());
                            _WBGRADINGTYPE.ProductCode = reader["ProductCode"] == DBNull.Value ? "" : (string)reader["ProductCode"];
                            _WBGRADINGTYPE.Status = bool.Parse(reader["Status"].ToString());
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return _WBGRADINGTYPE;
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
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

        public Tuple<List<NetWeightModel>, NetReceiptTotalModel> GetNetWeightApi(string TicketNo)
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

        public WBOWNER GetUnitWBOwner()
        {
            try
            {
                WBOWNER entity = new WBOWNER();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select  a.""UnitCode"" ,a.""MillManager"", a.""MillKTU"", b.""UnitName""  from public.""WBOWNER"" a
                                      inner join public.""BUSINESSUNIT"" b on a.""UnitCode"" =b.""UnitCode"" ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            entity.UnitCode = (string)reader["UnitCode"];
                            entity.UnitName = (string)reader["UnitName"];
                            entity.MillManager = (string)reader["MillManager"];
                            entity.MillKTU = (string)reader["MillKTU"];
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

        public WBUOM GetUOM(string UomID)
        {

            try
            {
                WBUOM entity = new WBUOM();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string[] arrUomID = UomID.Split('-');

                    string strSql = @"select * from public.""WBUom"" where ""UomID"" = '" + arrUomID[0].ToString() + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            entity.UomID = (string)reader["UomID"];
                            entity.UomName = (string)reader["UomName"];
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
                            entity.Toleransi = double.Parse(reader["Toleransi"].ToString());
                            entity.UnitPrice = double.Parse(reader["UnitPrice"].ToString());
                            entity.PremiumPrice = double.Parse(reader["PremiumPrice"].ToString());
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

        public bool InsertDespactHeader(WBTRXModel listDespact)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                                
                    string strSql = @"call public.sp_InsertDespactHeader(
                                    :p_ticketno
                                    ,:p_wbsource
                                    ,:p_wbtype
									,:p_trxdatein
									,:p_trxdateout
									,:p_unitcode
									,:p_nocoverletter
									,:p_productcode
									,:p_contractno
									,:p_dono
									,:p_vehicleid
									,:p_drivername
									,:p_licenseno
									,:p_weight1st
									,:p_weight2nd
                                    ,:p_reason
									,:p_wbflag
									,:p_wbstatus
                                    ,:p_wbimagefront1
									,:p_useridweight1st
									,:p_useridweight2nd                                   
									,:p_useridapproval
                                    ,:p_created
                                    ,:p_updated)";
 

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listDespact.TicketNo.Trim();
                        cmd.Parameters.AddWithValue("p_wbsource", DbType.String).Value = listDespact.WBSOURCE.Trim();
                        cmd.Parameters.AddWithValue("p_wbtype", DbType.String).Value = listDespact.WBType.Trim();
                        cmd.Parameters.AddWithValue("p_trxdatein", DbType.DateTime).Value = Convert.ToDateTime(listDespact.TrxDateIn);
                        cmd.Parameters.AddWithValue("p_trxdateout", DbType.DateTime).Value = Convert.ToDateTime(listDespact.TrxDateOut);
                        cmd.Parameters.AddWithValue("p_unitcode", DbType.String).Value = listDespact.UnitCode.Trim();
                        cmd.Parameters.AddWithValue("p_nocoverletter", DbType.String).Value = listDespact.NoCoverLetter.Trim();
                        cmd.Parameters.AddWithValue("p_productcode", DbType.String).Value = listDespact.ProductCode.Trim();
                        cmd.Parameters.AddWithValue("p_contractno", DbType.String).Value = listDespact.ContractNo.Trim();
                        cmd.Parameters.AddWithValue("p_dono", DbType.String).Value = listDespact.DONo.Trim();
                        cmd.Parameters.AddWithValue("p_vehicleid", DbType.String).Value = listDespact.VehicleID.Trim();
                        cmd.Parameters.AddWithValue("p_drivername", DbType.String).Value = listDespact.DriverName.Trim();
                        cmd.Parameters.AddWithValue("p_licenseno", DbType.String).Value = listDespact.LicenseNo.Trim();
                        cmd.Parameters.AddWithValue("p_weight1st", DbType.Int32).Value = Int32.Parse(listDespact.Weight1ST.ToString());
                        cmd.Parameters.AddWithValue("p_weight2nd", DbType.Int32).Value = Int32.Parse(listDespact.Weight2ND.ToString());
                        cmd.Parameters.AddWithValue("p_reason", DbType.String).Value = listDespact.Reason;
                        cmd.Parameters.AddWithValue("p_wbflag", DbType.String).Value = listDespact.WBFlag1.Trim();
                        cmd.Parameters.AddWithValue("p_wbstatus", DbType.String).Value = listDespact.WBStatus.Trim();
                        cmd.Parameters.AddWithValue("p_wbimagefront1", DbType.Binary).Value = listDespact.WBImagefront1;
                        cmd.Parameters.AddWithValue("p_useridweight1st", DbType.String).Value = listDespact.UserIDWeight1ST.Trim();
                        cmd.Parameters.AddWithValue("p_useridweight2nd", DbType.String).Value = listDespact.UserIDWeight2ND.Trim();                       
                        cmd.Parameters.AddWithValue("p_useridapproval", DbType.String).Value = listDespact.UserIDApproval.Trim();
                        cmd.Parameters.AddWithValue("p_created", DbType.Date).Value = Convert.ToDateTime(listDespact.Created);
                        cmd.Parameters.AddWithValue("p_updated", DbType.Date).Value = Convert.ToDateTime(listDespact.Updated);
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();

                            string strSqlVehicle = @"update public.""WBVEHICLE""  set ""VehFlag"" = '1' where ""VehicleID"" = '" + listDespact.VehicleID.Trim() + "' ";
                            using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSqlVehicle, con))
                            {
                                cmdVehicle.CommandType = CommandType.Text;
                                cmdVehicle.ExecuteNonQuery();
                            }

                            trans.Commit();
                            con.Close();
                            result = true;
                            return result;
                        }
                        catch (Exception err)
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

        public Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> UpdateDespact(WBTRXModel listDespact, List<WBTRXGRADING2nd> DespactDetail2Nd)
        {
            bool result = false;
            try
            {
                List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();
                NetDespactTotalModel _NetReceiptTotalModel = new NetDespactTotalModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    string strSql = @"call public.sp_updatedespact(
                                     :p_ticketno
									,:p_trxdateout
									,:p_nocoverletter
                                    ,:p_dono
									,:p_weight2nd
									,:p_wbstatus
                                    ,:p_WBImagefront2
                                    ,:p_wbflag
                                    ,:p_useridweight2nd
									,:p_useridapproval
									,:p_updated)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listDespact.TicketNo.Trim();
                        cmd.Parameters.AddWithValue("p_trxdateout", DbType.DateTime).Value = Convert.ToDateTime(listDespact.TrxDateOut); //Convert.ToDateTime(listDespact.TrxDateIn);
                        cmd.Parameters.AddWithValue("p_nocoverletter", DbType.String).Value = listDespact.NoCoverLetter.Trim();
                        cmd.Parameters.AddWithValue("p_dono", DbType.String).Value = listDespact.DONo.Trim();
                        cmd.Parameters.AddWithValue("p_weight2nd", DbType.Int32).Value = Int32.Parse(listDespact.Weight2ND.ToString());
                        cmd.Parameters.AddWithValue("p_wbstatus", DbType.String).Value = listDespact.WBStatus.Trim();
                        cmd.Parameters.AddWithValue("p_WBImagefront2", DbType.Binary).Value = listDespact.WBImagefront2;
                        cmd.Parameters.AddWithValue("p_wbflag", DbType.String).Value = listDespact.WBFlag2.Trim();
                        cmd.Parameters.AddWithValue("p_useridweight2nd", DbType.String).Value = listDespact.UserIDWeight2ND.Trim();
                        cmd.Parameters.AddWithValue("p_useridapproval", DbType.String).Value = listDespact.UserIDApproval.Trim();
                        cmd.Parameters.AddWithValue("p_updated", DbType.Date).Value = Convert.ToDateTime(listDespact.Updated);//Convert.ToDateTime(listDespact.Updated);
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();


                            if (DespactDetail2Nd != null)
                            {
                                foreach (var item in DespactDetail2Nd)
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
                            }

                                string strSqlVehicle = @"update public.""WBVEHICLE""  set ""VehFlag"" = '0' where ""VehicleID"" = '" + listDespact.VehicleID.Trim() + "' ";
                                using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSqlVehicle, con))
                                {
                                    cmdVehicle.CommandType = CommandType.Text;
                                    cmdVehicle.ExecuteNonQuery();
                                }

                                string strSqlNet = @"select * from public.""WBTRX"" where ""TicketNo"" = '" + listDespact.TicketNo.Trim() + "' ";
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

                                    //string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                                    //using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                                    //{
                                    //    cmdTotalNet.CommandType = CommandType.Text;
                                    //    cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listDespact.TicketNo.Trim();
                                    //    using (NpgsqlDataReader reader = cmdTotalNet.ExecuteReader())
                                    //    {
                                    //        reader.Read();
                                    //        _NetReceiptTotalModel.TicketNo = (string)reader["TicketNo"].ToString();
                                    //        _NetReceiptTotalModel.Quantity = double.Parse(reader["Quantity"].ToString());
                                    //    }
                                    //}

                                    double Net = _NetWeightModel[1].WeightHeavy - _NetWeightModel[0].WeightHeavy;


                                    double DesctpacthQty = 0;// Math.Ceiling(GetSumQtyDespact(listDespact.ContractNo.Trim())) + Net;

                                    double QtyContract = GetWBCONTRACT(listDespact.ContractNo.Trim()).Qty;

                                    string DeliveryStatus = "1";//DesctpacthQty >= QtyContract ? "0" : "1";

                                    string strSqContract = @"update public.""WBCONTRACT""  set ""DeliveryStatus"" = " + DeliveryStatus + @", ""DespatchQty"" = '" + DesctpacthQty.ToString() + "' " + @"where ""ContractNo"" = '" + listDespact.ContractNo.Trim() + "' ";
                                    using (NpgsqlCommand cmdContract = new NpgsqlCommand(strSqContract, con))
                                    {
                                        cmdContract.CommandType = CommandType.Text;
                                        cmdContract.ExecuteNonQuery();
                                    }
                                }

                            //}

                            result = true;
                            trans.Commit();
                            con.Close();

                        }
                        catch (Exception err)
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

        public Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> ApproveDespact(WBTRXModel listDespact, List<WBTRXGRADING2nd> DespactDetail2Nd)
        {
            bool result = false;
            try
            {
                bool DelGrading = DeleteGrading(listDespact.TicketNo.Trim());
                if (!DelGrading)
                {
                    throw new Exception("Failed Deleted Grading");
                }

                List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();
                NetDespactTotalModel _NetReceiptTotalModel = new NetDespactTotalModel();
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    string strSql = @"call public.sp_approvedescpatch(
                                     :p_ticketno								
                                    ,:p_nocoverletter  
                                    ,:p_dono
                                    ,:p_weight1st
                                    ,:p_weight2nd
                                    ,:p_driver
                                    ,:p_lisensi                                 
                                    ,:p_vehicle
                                    ,:p_status
                                    ,:p_useridapproval
                                    ,:p_updated)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;                       
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listDespact.TicketNo.Trim();
                        cmd.Parameters.AddWithValue("p_nocoverletter", DbType.String).Value = (listDespact.NoCoverLetter == null ? "" : listDespact.NoCoverLetter.Trim());
                        cmd.Parameters.AddWithValue("p_dono", DbType.String).Value = (listDespact.DONo == null ? "" : listDespact.DONo.Trim());
                        cmd.Parameters.AddWithValue("p_weight1st", DbType.Int32).Value = Int32.Parse(listDespact.Weight1ST.ToString());
                        cmd.Parameters.AddWithValue("p_weight2nd", DbType.Int32).Value = Int32.Parse(listDespact.Weight2ND.ToString());
                        cmd.Parameters.AddWithValue("p_driver", DbType.String).Value = listDespact.DriverName.ToString();
                        cmd.Parameters.AddWithValue("p_lisensi", DbType.String).Value = listDespact.LicenseNo.ToString();
                        cmd.Parameters.AddWithValue("p_vehicle", DbType.String).Value = listDespact.VehicleID.Trim();
                        cmd.Parameters.AddWithValue("p_status", DbType.String).Value = listDespact.Status.Trim();
                        cmd.Parameters.AddWithValue("p_useridapproval", DbType.String).Value = "";//listDespact.UserIDApproval.Trim();
                        cmd.Parameters.AddWithValue("p_updated", DbType.Date).Value = Convert.ToDateTime(listDespact.Updated); //Convert.ToDateTime(listDespact.Updated);
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        try
                        {
                            NpgsqlTransaction trans = con.BeginTransaction();
                            cmd.Transaction = trans;
                            int affetecdCount = cmd.ExecuteNonQuery();


                            if (DespactDetail2Nd != null)
                            {
                                foreach (var item in DespactDetail2Nd)
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
                            }

                                string strSqlVehicle = @"update public.""WBVEHICLE""  set ""VehFlag"" = '0' where ""VehicleID"" = '" + listDespact.VehicleID.Trim() + "' ";
                                using (NpgsqlCommand cmdVehicle = new NpgsqlCommand(strSqlVehicle, con))
                                {
                                    cmdVehicle.CommandType = CommandType.Text;
                                    cmdVehicle.ExecuteNonQuery();
                                }

                                string strSqlNet = @"select * from public.""WBTRX"" where ""TicketNo"" = '" + listDespact.TicketNo.Trim() + "' ";
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

                                        string x1 = reader["Weight1ST"].ToString();
                                        string x2 = reader["Weight2ND"].ToString();


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

                                    //string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                                    //using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                                    //{
                                    //    cmdTotalNet.CommandType = CommandType.Text;
                                    //    cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = listDespact.TicketNo.Trim();
                                    //    using (NpgsqlDataReader reader = cmdTotalNet.ExecuteReader())
                                    //    {
                                    //        reader.Read();
                                    //        _NetReceiptTotalModel.TicketNo = (string)reader["TicketNo"].ToString();
                                    //        _NetReceiptTotalModel.Quantity = double.Parse(reader["Quantity"].ToString());
                                    //    }
                                    //}

                                    double Net = double.Parse(_NetWeightModel[1].WeightHeavy.ToString()) - double.Parse(_NetWeightModel[0].WeightHeavy.ToString());


                                    double DesctpacthQty = 0;//GetSumQtyDespact(listDespact.ContractNo.Trim()) + Net;

                                    double QtyContract = GetWBCONTRACT(listDespact.ContractNo.Trim()).Qty;

                                    string DeliveryStatus = "1";//DesctpacthQty >= QtyContract ? "0" : "1";

                                    string strSqContract = @"update public.""WBCONTRACT""  set ""DeliveryStatus"" = " + DeliveryStatus + @", ""DespatchQty"" = '" + DesctpacthQty.ToString() + "' " + @"where ""ContractNo"" = '" + listDespact.ContractNo.Trim() + "' ";
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
                        catch (Exception err)
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
        public List<DespactModel> GetAllReceipByFilter(string TicketNo, string ContractNo, string StartDate, string EndDate)
        {
            List<DespactModel> wBTRXes = new List<DespactModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSql = "";

                    if (TicketNo != "" && ContractNo == "" && StartDate == "" && EndDate == "")
                    {
                        strSql = @"select * from public.get_descpactfilterbyticket(:p_ticketno)";
                    }

                    if (TicketNo == "" && ContractNo != "" && StartDate == "" && EndDate == "")
                    {
                        strSql = @"select * from public.get_descpactbycontract(:p_contractno)";
                    }
                    if (TicketNo == "" && ContractNo == "" && StartDate != "" && EndDate != "")
                    {
                        strSql = strSql + @"select * from public.get_descpactbydate(:p_startdate,:p_enddate)";
                    }

                    if (TicketNo == "" && ContractNo == "" && StartDate == "" && EndDate == "")
                    {
                        strSql = strSql + @"select * from public.get_despact()";
                    }

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
                            cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(StartDate.Trim());
                            cmd.Parameters.AddWithValue("p_enddate", DbType.DateTime).Value = DateTime.Parse(EndDate.Trim());
                        }

                        cmd.CommandType = CommandType.Text;

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DespactModel entity = new DespactModel();
                                entity.TicketNo = (string)reader["ticketNo"];
                                entity.Created = (DateTime)reader["created"];
                                entity.UnitName = (string)reader["unitname"];
                                entity.ProductName = (string)reader["productname"];
                                entity.ContractNo = (string)reader["contractno"];
                                entity.BPName = (reader["bpname"] == DBNull.Value) ? "" : (string)reader["bpname"];
                                entity.VehicleID = (reader["vehicleid"] == DBNull.Value) ? "" : (string)reader["vehicleid"];
                                entity.WBStatus = (reader["wbstatus"] == DBNull.Value) ? "" : (string)reader["wbstatus"];
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

        public List<WBTRXGRADING> GetAllDespactDetailSecondNdByTicket(string TicketNo)
        {
            List<WBTRXGRADING> wBTRXes = new List<WBTRXGRADING>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getdespactdetailrsecondnd(:p_ticketno)";

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
                                entity.NoSegel1 = (string)reader["nosegel1"];
                                entity.NoSegel2 = (string)reader["nosegel2"];
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

        public List<WBCONTRACT> GetAllWBCONTRACT()
        {
            try
            {
                List<WBCONTRACT> wBCONTRACTs = new List<WBCONTRACT>();

                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select * from public.""WBCONTRACT""";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WBCONTRACT entity = new WBCONTRACT();
                                entity.ContractNo = (string)reader["ContractNo"];
                                entity.ContractDate = DateTime.Parse(reader["ContractDate"].ToString());
                                entity.ExpDate = DateTime.Parse(reader["ExpDate"].ToString());
                                entity.ProductCode = (string)reader["ProductCode"];
                                entity.BPCode = (string)reader["BPCode"];
                                entity.Qty = double.Parse(reader["Qty"].ToString());
                                entity.Toleransi = double.Parse(reader["Toleransi"].ToString());
                                entity.UnitPrice = double.Parse(reader["UnitPrice"].ToString());
                                entity.PremiumPrice = double.Parse(reader["PremiumPrice"].ToString());
                                entity.PPN = double.Parse(reader["PPN"].ToString());
                                entity.FinalUnitPrice = double.Parse(reader["FinalUnitPrice"].ToString());
                                entity.TotalPrice = double.Parse(reader["TotalPrice"].ToString());
                                entity.DespatchQty = double.Parse(reader["DespatchQty"].ToString());
                                entity.DeliveryStatus = (string)reader["DeliveryStatus"];
                                wBCONTRACTs.Add(entity);
                            }

                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return wBCONTRACTs;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public bool PostingDespacth()
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
                    string strSqlDelGrading = @"update public.""WBTRX"" set ""WBStatus"" = 'D' , ""status"" ='C' where  ""WBStatus"" = 'S' and ""WBType"" = 'Despact'";
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

        public string GenerateDONo()
        {

            double TranNow = double.Parse(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString());
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();

                    string result = "";

                    string strSql = @"select counter from public.get_counterDo();";
                    int DONo = 0;
                    int Counter = 0;
                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            Counter = int.Parse(reader["counter"].ToString());
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    string sMaxTran = Check();
                    double maxTran = Double.Parse(Convert.ToDateTime(sMaxTran).Year.ToString() + Convert.ToDateTime(sMaxTran).Month.ToString());

                    if (sMaxTran == "")
                    {
                        sMaxTran = DateTime.Now.ToString();
                    }

                    if (maxTran == TranNow)
                    {
                        DONo = Counter + 1;
                        result = "DO" + "/" + DateTime.Now.ToString("yy") + "/" + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + "/" + DONo.ToString("0000") + System.Environment.NewLine;
                    }
                    else
                    {
                        DONo = 1;
                        result = "DO" + "/" + DateTime.Now.ToString("yy") + "/" + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + "/" + DONo.ToString("0000") + System.Environment.NewLine;
                    }


                    return result;
                }
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
                    string strSql = @"select max(""DODate"") as maxdate from public.""WBDO""; ";

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
    }
}