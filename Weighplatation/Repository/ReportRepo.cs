using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Interface;
using Npgsql;
using System.Data;
using System.Configuration;
using System.Globalization;

namespace Weighplatation.Repository
{
    public class ReportRepo : IReport
    {
        private readonly string stringCon = ConfigurationManager.ConnectionStrings["dbcon"].ToString();
        public List<ReceiptModelNd> GetFormReceiptHeader(string TicketNo)
        {
            List<ReceiptModelNd> wBTRXes = new List<ReceiptModelNd>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.form_receiptticketheader(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    ReceiptModelNd entity = new ReceiptModelNd();
                                    entity.TicketNo = (string)reader["ticketno"];
                                    entity.Created = (DateTime)reader["created"];
                                    entity.UnitName = (string)reader["unitname"];
                                    entity.ProductName = (string)reader["productname"];
                                    entity.ContractNo = (string)reader["contractno"];
                                    entity.LetterNo = reader["LetterNo"] == DBNull.Value ? "" : (string)reader["LetterNo"];
                                    entity.Transporter = (string)reader["unitname"];
                                    entity.BPName = (reader["bpname"] == DBNull.Value) ? "" : (string)reader["bpname"];
                                    entity.VehicleID = (reader["vehicleid"] == DBNull.Value) ? "" : (string)reader["vehicleid"];
                                    entity.WBStatus = (reader["wbStatus"] == DBNull.Value) ? "" : (string)reader["wbStatus"];
                                    entity.DriverName = (reader["driver"] == DBNull.Value) ? "" : (string)reader["driver"];
                                    entity.Print = (reader["print"] == DBNull.Value) ? 0 : int.Parse(reader["print"].ToString());
                                    entity.RefNo = "";//(reader["refno"] == DBNull.Value) ? "" : (string)reader["refno"];
                                    wBTRXes.Add(entity);
                                }

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

        public List<ReceiptModelNd> GetFormOtherReceiptHeader(string TicketNo)
        {
            List<ReceiptModelNd> wBTRXes = new List<ReceiptModelNd>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.form_otherreceiptticketheader(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    ReceiptModelNd entity = new ReceiptModelNd();
                                    entity.TicketNo = (string)reader["ticketno"];
                                    entity.Created = (DateTime)reader["created"];
                                    entity.UnitName = (string)reader["unitname"];
                                    entity.ProductName = (string)reader["productname"];
                                    // entity.ContractNo = (string)reader["contractno"];
                                    entity.LetterNo = reader["LetterNo"] == DBNull.Value ? "" : (string)reader["LetterNo"];
                                    entity.Transporter = (string)reader["unitname"];
                                    entity.BPName = (reader["bpname"] == DBNull.Value) ? "" : (string)reader["bpname"];
                                    entity.VehicleID = (reader["vehicleid"] == DBNull.Value) ? "" : (string)reader["vehicleid"];
                                    entity.WBStatus = (reader["wbStatus"] == DBNull.Value) ? "" : (string)reader["wbStatus"];
                                    entity.DriverName = (reader["driver"] == DBNull.Value) ? "" : (string)reader["driver"];
                                    entity.Print = (reader["print"] == DBNull.Value) ? 0 : int.Parse(reader["print"].ToString());
                                    wBTRXes.Add(entity);
                                }

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

        public string GetCompany()
        {
            throw new NotImplementedException();
        }

        public List<WBTRXBLOCK1st> GetFromReceiptBlock(string TicketNo)
        {
            List<WBTRXBLOCK1st> wBTRXes = new List<WBTRXBLOCK1st>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.form_receiptblock(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    WBTRXBLOCK1st entity = new WBTRXBLOCK1st();
                                    entity.TicketNo = (string)reader["ticketno"];
                                    entity.BlockID = (string)reader["blockid"];
                                    entity.BunchesQty = reader["bunches"] == DBNull.Value ? 0 : double.Parse(reader["bunches"].ToString());
                                    entity.LFQty = reader["lfqty"] == DBNull.Value ? 0 : double.Parse(reader["lfqty"].ToString());
                                    entity.Estimation = reader["estimation"] == DBNull.Value ? 0 : double.Parse(reader["estimation"].ToString());
                                    entity.YoP = reader["yop"] == DBNull.Value ? 0 : int.Parse(reader["yop"].ToString());
                                    entity.Weight = reader["weight"] == DBNull.Value ? 0 : double.Parse(reader["weight"].ToString());
                                    wBTRXes.Add(entity);
                                }

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

        public List<WBTRXGRADING2nd> GetFromReceiptGrading(string TicketNo)
        {
            List<WBTRXGRADING2nd> wBTRXes = new List<WBTRXGRADING2nd>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.form_receiptgrading(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    WBTRXGRADING2nd entity = new WBTRXGRADING2nd();
                                    entity.TicketNo = (string)reader["ticketno"];
                                    entity.GradingTypeId = reader["gradingtypeid"].ToString();
                                    entity.GradingName = reader["gradingname"] == DBNull.Value ? "" : reader["gradingname"].ToString();
                                    entity.Quantity = reader["quantity"] == DBNull.Value ? 0 : double.Parse(reader["quantity"].ToString());
                                    wBTRXes.Add(entity);
                                }
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

        public List<NetWeightModel> GetNetWeightModel(string TicketNo)
        {
            List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();

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

                            DateTime dt1st = Convert.ToDateTime(reader["TrxDateIn"]);

                            DateTime dt2nd = Convert.ToDateTime(reader["TrxDateOut"]);



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
                    }
                    return _NetWeightModel;
                }
            }
            catch (NpgsqlException)
            {

                return _NetWeightModel = null;
            }

        }

        public List<NetWeightModel> GetOtherNetWeightModel(string TicketNo)
        {
            List<NetWeightModel> _NetWeightModel = new List<NetWeightModel>();

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

                            DateTime dt1st = Convert.ToDateTime(reader["TrxDateIn"]);

                            DateTime dt2nd = Convert.ToDateTime(reader["TrxDateOut"]);



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
                    }
                    return _NetWeightModel;
                }
            }
            catch (NpgsqlException)
            {

                return _NetWeightModel = null;
            }

        }

        public NetReceiptTotalModel _NetReceiptTotalModel(string TicketNo)
        {
            NetReceiptTotalModel _NetReceiptTotalModel = new NetReceiptTotalModel();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                    using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                    {
                        cmdTotalNet.CommandType = CommandType.Text;
                        cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo;
                        using (NpgsqlDataReader reader = cmdTotalNet.ExecuteReader())
                        {
                            reader.Read();
                            _NetReceiptTotalModel.TicketNo = (string)reader["TicketNo"].ToString();
                            _NetReceiptTotalModel.Quantity = double.Parse(reader["Quantity"] == DBNull.Value ? "0" : reader["Quantity"].ToString());
                        }
                    }
                }
                return _NetReceiptTotalModel = null;
            }
            catch (NpgsqlException)
            {

                return _NetReceiptTotalModel = null;
            }
        }

        public List<DespactModelNd> GetFormDespachHeader(string TicketNo)
        {
            List<DespactModelNd> wBTRXes = new List<DespactModelNd>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.form_despachticketheader(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    DespactModelNd entity = new DespactModelNd();
                                    entity.TicketNo = (string)reader["ticketno"];
                                    entity.Created = (DateTime)reader["created"];
                                    entity.UnitName = (string)reader["unitname"];
                                    entity.ProductName = (string)reader["productname"];
                                    entity.ContractNo = (string)reader["contractno"];
                                    entity.Transporter = (string)reader["unitname"];
                                    entity.BPName = (reader["bpname"] == DBNull.Value) ? "" : (string)reader["bpname"];
                                    entity.VehicleID = (reader["vehicleid"] == DBNull.Value) ? "" : (string)reader["vehicleid"];
                                    entity.WBStatus = (reader["wbStatus"] == DBNull.Value) ? "" : (string)reader["wbStatus"];
                                    entity.DriverName = (reader["driver"] == DBNull.Value) ? "" : (string)reader["driver"];
                                    entity.Print = (reader["print"] == DBNull.Value) ? 0 : int.Parse(reader["print"].ToString());
                                    entity.RefNo = "";// (reader["refno"] ==  DBNull.Value) ? "" : (string)reader["refno"];
                                    wBTRXes.Add(entity);
                                }

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

        public List<WBTRXGRADING2nd> GetFromDespachGrading(string TicketNo)
        {
            List<WBTRXGRADING2nd> wBTRXes = new List<WBTRXGRADING2nd>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.form_despachgrading(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    WBTRXGRADING2nd entity = new WBTRXGRADING2nd();
                                    entity.TicketNo = (string)reader["ticketno"];
                                    entity.GradingTypeId = reader["gradingtypeid"].ToString();
                                    entity.GradingName = reader["gradingname"] == DBNull.Value ? "" : reader["gradingname"].ToString();
                                    entity.Quantity = reader["quantity"] == DBNull.Value ? 0 : double.Parse(reader["quantity"].ToString());
                                    entity.NoSegel1 = reader["nosegel1"] == DBNull.Value ? "" : reader["nosegel1"].ToString();
                                    entity.NoSegel2 = reader["nosegel1"] == DBNull.Value ? "" : reader["nosegel2"].ToString();
                                    wBTRXes.Add(entity);
                                }
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

        public NetDespactTotalModel _NetDespactTotalModel(string TicketNo)
        {
            NetDespactTotalModel _NetDespactTotalModel = new NetDespactTotalModel();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    string strSqlTotalNet = @"select * from public.getNetReceipt(:p_ticketno)";
                    using (NpgsqlCommand cmdTotalNet = new NpgsqlCommand(strSqlTotalNet, con))
                    {
                        cmdTotalNet.CommandType = CommandType.Text;
                        cmdTotalNet.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo;
                        using (NpgsqlDataReader reader = cmdTotalNet.ExecuteReader())
                        {
                            reader.Read();
                            _NetDespactTotalModel.TicketNo = (string)reader["TicketNo"].ToString();
                            _NetDespactTotalModel.Quantity = double.Parse(reader["Quantity"] == DBNull.Value ? "0" : reader["Quantity"].ToString());
                        }
                    }
                }
                return _NetDespactTotalModel = null;
            }
            catch (NpgsqlException)
            {

                return _NetDespactTotalModel = null;
            }
        }

        public List<RptReceiptDtlModel> GetRptReportDetail(string startdate, string finisdate)
        {
            List<RptReceiptDtlModel> rptReceiptDtlModels = new List<RptReceiptDtlModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.rpt_ffbbuschesreceiptdetail(:p_startdate,:p_finishdate)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(startdate.Trim());
                        cmd.Parameters.AddWithValue("p_finishdate", DbType.DateTime).Value = DateTime.Parse(finisdate.Trim()); 
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    RptReceiptDtlModel entity = new RptReceiptDtlModel();
                                    entity.unitname = (string)reader["unitname"];
                                    entity.createddt = DateTime.Parse(reader["created"].ToString());
                                    entity.hourlytime = (string)reader["hourlytime"];
                                    entity.ticketNo = (string)reader["ticketNo"];
                                    entity.mode = (string)reader["mode"];
                                    entity.productname = (string)reader["productname"];
                                    entity.vehicleid = (string)reader["vehicleid"];
                                    entity.BlockID = (string)reader["BlockID"];
                                    entity.drivername = (string)reader["drivername"];
                                    entity.yop = (string)reader["yop"];
                                    entity.weight1st = double.Parse(reader["weight1st"].ToString());
                                    entity.lfqty = double.Parse(reader["lfqty"].ToString());
                                    entity.weight2nd = double.Parse(reader["weight2nd"].ToString());
                                    entity.abw = double.Parse(reader["abw"].ToString());
                                    entity.netweight = double.Parse(reader["netweight"].ToString());
                                    entity.bunches = double.Parse(reader["bunches"].ToString());
                                    entity.deductionqty = double.Parse(reader["deductionqty"].ToString());
                                    entity.ffbwt = double.Parse(reader["ffbwt"].ToString());
                                    entity.estimat = double.Parse(reader["estimat"].ToString());
                                    entity.diff = double.Parse(reader["diff"].ToString());
                                    entity.arrivetime = (string)reader["arrivetime"];
                                    entity.timein = (string)reader["timein"];
                                    entity.timeout = (string)reader["timeout"];
                                    entity.waittime = (string)reader["waittime"];
                                    entity.unloadtime = (string)reader["unloadtime"];
                                    entity.inttnt = (string)reader["inttnt"];
                                    entity.potongan =decimal.Parse(reader["potongan"].ToString());
                                    //double  sNetTotal = Convert.ToDouble(reader["nettotal"].ToString());
                                    entity.nettotal = Math.Floor(double.Parse(reader["nettotal"].ToString()));
                                    rptReceiptDtlModels.Add(entity);
                                }
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return rptReceiptDtlModels;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }

            }
        }

        public List<RptDespacthDtlModel> GetRptReportDespactDetail(string startdate, string finisdate)
        {
            List<RptDespacthDtlModel> rptDespactDtlModels = new List<RptDespacthDtlModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                                                           
                    string strSql = @"select * from public.rpt_ffbbuschesdespacthdetail(:p_startdate,:p_finishdate)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(startdate.Trim());
                        cmd.Parameters.AddWithValue("p_finishdate", DbType.DateTime).Value = DateTime.Parse(finisdate.Trim());
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    RptDespacthDtlModel entity = new RptDespacthDtlModel();
                                    entity.unitname = (string)reader["unitname"];
                                    entity.createddt = DateTime.Parse(reader["created"].ToString());
                                    entity.hourlytime = (string)reader["hourlytime"];
                                    entity.ticketNo = (string)reader["ticketNo"];
                                    entity.mode = (string)reader["mode"];
                                    entity.productname = (string)reader["productname"];
                                    entity.vehicleid = (string)reader["vehicleid"];
                                    entity.BlockID = (string)reader["BlockID"];
                                    entity.drivername = (string)reader["drivername"];
                                    entity.yop = (string)reader["yop"];
                                    entity.weight1st = double.Parse(reader["weight1st"].ToString());
                                    entity.lfqty = double.Parse(reader["lfqty"].ToString());
                                    entity.weight2nd = double.Parse(reader["weight2nd"].ToString());
                                    entity.abw = double.Parse(reader["abw"].ToString());
                                    entity.netweight = double.Parse(reader["netweight"].ToString());
                                    entity.bunches = double.Parse(reader["bunches"].ToString());
                                    entity.deductionqty = double.Parse(reader["deductionqty"].ToString());
                                    entity.ffbwt = double.Parse(reader["ffbwt"].ToString());
                                    entity.estimat = double.Parse(reader["estimat"].ToString());
                                    entity.diff = double.Parse(reader["diff"].ToString());
                                    entity.arrivetime = (string)reader["arrivetime"];
                                    entity.timein = (string)reader["timein"];
                                    entity.timeout = (string)reader["timeout"];
                                    entity.waittime = (string)reader["waittime"];
                                    entity.unloadtime = (string)reader["unloadtime"];
                                    entity.inttnt = (string)reader["inttnt"];
                                    entity.potongan = Math.Ceiling(double.Parse(reader["potongan"].ToString()));
                                    //double  sNetTotal = Convert.ToDouble(reader["nettotal"].ToString());
                                    entity.nettotal = Convert.ToDouble(reader["nettotal"].ToString());
                                    rptDespactDtlModels.Add(entity);
                                }
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return rptDespactDtlModels;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }

            }
        }

        public List<RptRkpRcptMuatanModel> GetRekapReceiptMtn(string startdate, string finisdate)
        {
            List<RptRkpRcptMuatanModel> rptRekapReceipModels = new List<RptRkpRcptMuatanModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.get_rptrekapreceiptmuatan(:p_startdate,:p_finishdate)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(startdate.Trim());
                        cmd.Parameters.AddWithValue("p_finishdate", DbType.DateTime).Value = DateTime.Parse(finisdate.Trim());
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    RptRkpRcptMuatanModel entity = new RptRkpRcptMuatanModel();
                                    entity.muatan = (string)reader["vmuatan"];
                                    entity.jmltkt = double.Parse(reader["vjmltkt"].ToString());
                                    entity.netto1 = double.Parse(reader["vnetto1"].ToString());
                                    entity.sortasi = decimal.Parse(reader["vdeduction"].ToString());
                                    entity.potongan = decimal.Parse(reader["vpotongan"].ToString());
                                    entity.netto2 = Math.Floor(double.Parse(reader["vnetto2"].ToString()));

                                    rptRekapReceipModels.Add(entity);
                                }
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return rptRekapReceipModels;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }

            }
        }

        public List<RptRkpRcptSupplierModel> GetRekapReceiptSpl(string startdate, string finisdate)
        {
            List<RptRkpRcptSupplierModel> rptRekapReceipModels = new List<RptRkpRcptSupplierModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.get_rptrekapreceiptsupplier(:p_startdate,:p_finishdate)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(startdate.Trim());
                        cmd.Parameters.AddWithValue("p_finishdate", DbType.DateTime).Value = DateTime.Parse(finisdate.Trim());
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    RptRkpRcptSupplierModel entity = new RptRkpRcptSupplierModel();
                                    entity.supplier = (string)reader["vsupplier"];
                                    entity.muatan = (string)reader["vmuatan"];
                                    entity.jmltkt = double.Parse(reader["vjmltkt"].ToString());
                                    entity.netto1 = double.Parse(reader["vnetto1"].ToString());
                                    entity.sortasi = decimal.Parse(reader["vdeduction"].ToString());
                                    entity.potongan = decimal.Parse(reader["vpotongan"].ToString());
                                    entity.netto2 = Math.Floor(double.Parse(reader["vnetto2"].ToString()));
                                    rptRekapReceipModels.Add(entity);
                                }
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return rptRekapReceipModels;

                }
                catch (NpgsqlException err)
                {
                    Console.WriteLine(err.Message);
                    throw;
                }

            }
        }

        public List<RptRkpDespacth> GetRekapDespacth(string startdate, string finisdate)
        {
            List<RptRkpDespacth> rptRekapReceipModels = new List<RptRkpDespacth>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.get_rptrekapdespactcustomer(:p_startdate,:p_finishdate)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_startdate", DbType.DateTime).Value = DateTime.Parse(startdate.Trim());
                        cmd.Parameters.AddWithValue("p_finishdate", DbType.DateTime).Value = DateTime.Parse(finisdate.Trim());
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                {
                                    RptRkpDespacth entity = new RptRkpDespacth();
                                    entity.customer = (string)reader["vcustomer"];
                                    entity.muatan = (string)reader["vmuatan"];
                                    entity.jmltkt = double.Parse(reader["vjmltkt"].ToString());
                                    entity.netto1 = double.Parse(reader["vnetto1"].ToString());
                                    entity.sortasi = double.Parse(reader["vdeduction"].ToString());
                                    entity.potongan = double.Parse(reader["vpotongan"].ToString());
                                    entity.netto2 = double.Parse(reader["vnetto2"].ToString());
                                    rptRekapReceipModels.Add(entity);
                                }
                            }
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return rptRekapReceipModels;

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