using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Interface;
using Weighplatation.Repository;
using Npgsql;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Weighplatation.Repository
{
    public class ApiOddoRepo : IApiOddo
    {
        private readonly string stringCon = ConfigurationManager.ConnectionStrings["dbcon"].ToString();
        DespactRepo _despactRepo = new DespactRepo();
        ReceiptRepo receiptRepo = new ReceiptRepo();
        public receiptHeader GetReceiptApiModel(string TicketNo, string[] plant_weighbridge_line, string[] weighbridge_grade_line)
        {

            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.get_receiptApi(:p_ticketno)";

                    receiptHeader entity = new receiptHeader();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();

                            {

                                Tuple<List<NetWeightModel>, NetReceiptTotalModel> resultTuple = receiptRepo.GetNetWeight(TicketNo);
                                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                                listNetWeight = resultTuple.Item1;
                             
                                double nOnest = double.Parse(listNetWeight[0].WeightHeavy.ToString());                              
                                double nSecondsnd = double.Parse(listNetWeight[1].WeightHeavy.ToString());
                             
                                //double deduc = Math.Ceiling(receiptRepo.GetDeducation(TicketNo.Trim()) ) ;
                                double deduc = receiptRepo.GetDeducation(TicketNo.Trim());
                                double nOnestNet = nOnest - nSecondsnd;
                                double tRotasi = nOnestNet * deduc;
                               
                                double potongan = nOnestNet * (double.Parse(reader["potongan"].ToString()) / 100);
                                double nSecondsndNet = nOnestNet - tRotasi - potongan;

                                string onest = nOnestNet.ToString(); //listNetWeight[0].WeightHeavy.ToString();
                                string secondsnd = nSecondsndNet.ToString();//listNetWeight[1].WeightHeavy.ToString();

                                //{24/03/2022 14.29.31}
                                DateTime firstdate = Convert.ToDateTime(reader["trxdatein"]).AddHours(- 7);
                                DateTime seconddate = Convert.ToDateTime(reader["trxdateout"]).AddHours(-7);
                                string first_date = firstdate.ToString("yyyy-MM-dd HH:mm:ss");
                                string second_date = seconddate.ToString("yyyy-MM-dd HH:mm:ss");
                                //DateTime date = DateTime.ParseExact(second_date.ToString(), "dd-MM-yyyy HH.mm.ss", CultureInfo.InvariantCulture);

                                entity.partner_id = reader["bpcode"].ToString() == "0" ? "false" : reader["bpcode"].ToString();
                                entity.partner_branch_id = (reader["unitcode"].ToString() == "0") ? "false" : reader["unitcode"].ToString();
                                entity.name = (string)reader["ticketno"];
                                entity.first_date = first_date.Replace(".", ":");
                                entity.second_date = second_date.Replace(".", ":");
                                entity.product_id = reader["productcode"].ToString();
                                entity.contract_id = reader["contractno"].ToString();
                                entity.first_weight = reader["Weight1ST"] == DBNull.Value ? "" : Math.Ceiling(double.Parse(reader["Weight1ST"].ToString())).ToString();
                                entity.wb_type = (reader["WBType"] == DBNull.Value) ? "" : (string)reader["WBType"].ToString().Trim().ToLower();
                                entity.first_net_weight = Math.Ceiling(double.Parse(onest)).ToString();
                                entity.second_net_weight = Math.Ceiling(double.Parse(secondsnd)).ToString();
                                entity.weight_deduction = Math.Ceiling(deduc).ToString();
                                entity.second_weight = (reader["Weight2ND"] == DBNull.Value) ? "" : Math.Ceiling(double.Parse(reader["Weight2ND"].ToString())).ToString();
                                entity.trx_type = (reader["WBFlag2"] == DBNull.Value) ? "" : (string)reader["WBFlag2"] == "A" ? "automatic" : "manual";
                                entity.state = "second";
                                entity.plant_weighbridge_line = plant_weighbridge_line;
                                entity.weighbridge_grade_line = weighbridge_grade_line;
                                entity.vehicle_id = "false";//(reader["vehicleid"].ToString() == "0" ? "false" : reader["vehicleid"].ToString()); //@"""" + @"""";//(string)reader["vehicleid"];
                                entity.transporter_id = (reader["transporterid"].ToString() == "0" ? "false" : reader["transporterid"].ToString());

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
        }
        public despactHeader GetDespactApiModel(string TicketNo, string[] weighbridge_quality_line)
        {
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.get_despacthapi(:p_ticketno)";

                    despactHeader entity = new despactHeader();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();

                            {
                                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                                NetReceiptTotalModel listNetTotal = new NetReceiptTotalModel();
                                Tuple<List<NetWeightModel>, NetReceiptTotalModel> resultTuple = _despactRepo.GetNetWeightApi(TicketNo);
                                listNetWeight = resultTuple.Item1;
                                listNetTotal = resultTuple.Item2;
                                string onest = listNetWeight[0].WeightHeavy.ToString();
                                string secondsnd = listNetWeight[1].WeightHeavy.ToString();



                                //{24/03/2022 14.29.31}
                                DateTime firstdate = Convert.ToDateTime(reader["trxdatein"]).AddHours(-7);
                                DateTime seconddate = Convert.ToDateTime(reader["trxdateout"]).AddHours(-7);
                                string first_date = firstdate.ToString("yyyy-MM-dd HH:mm:ss");
                                string second_date = seconddate.ToString("yyyy-MM-dd HH:mm:ss");

                                entity.partner_id = reader["bpcode"].ToString() == "0" ? "false" : reader["bpcode"].ToString();
                                entity.partner_branch_id = "false";//(reader["unitcode"].ToString() == "0") ? "false" : reader["unitcode"].ToString();
                                entity.name = (string)reader["ticketno"];
                                entity.first_date = second_date.Replace(".", ":");
                                entity.second_date = second_date.Replace(".", ":");
                                entity.product_id = reader["productcode"].ToString();
                                entity.contract_id = reader["contractno"].ToString();
                                entity.first_weight = reader["Weight1ST"] == DBNull.Value ? "" : (double.Parse(reader["Weight1ST"].ToString())).ToString();
                                entity.wb_type = (reader["WBType"] == DBNull.Value) ? "" : (string)reader["WBType"].ToString().Trim().ToLower();
                                entity.first_net_weight = onest;
                                entity.second_net_weight = secondsnd;
                                double deduc = Math.Ceiling(receiptRepo.GetDeducation(TicketNo.Trim()));
                                entity.weight_deduction = "0";//deduc.ToString();
                                entity.second_weight = (reader["Weight2ND"] == DBNull.Value) ? "" : (double.Parse(reader["Weight2ND"].ToString())).ToString();
                                entity.trx_type = (reader["WBFlag2"] == DBNull.Value) ? "" : (string)reader["WBFlag2"] == "A" ? "automatic" : "manual";
                                entity.state = "second";
                                entity.weighbridge_quality_line = weighbridge_quality_line;
                                entity.vehicle = (string)reader["vehicle"];
                                entity.transporter = (reader["transporter"].ToString() == "0" ? "-" : reader["transporter"].ToString());
                                entity.driver = (reader["driver"].ToString() == "0" ? "-" : reader["driver"].ToString());
                                entity.reference = (string)reader["dono"];
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
        }

        public List<WBTRXModel> GetAllSendApiReceipt()
        {
            List<WBTRXModel> wBTRXes = new List<WBTRXModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select ""TicketNo"" from public.""WBTRX"" where ""statusoddo"" = false and ""WBType""='Receipt' and  ""status"" ='C' and SUBSTRING(""TicketNo"",1,3) <> 'BBM' ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WBTRXModel entity = new WBTRXModel();
                                entity.TicketNo = (string)reader["ticketno"];
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
        public List<WBTRXModel> GetAllSendApiDespact()
        {
            List<WBTRXModel> wBTRXes = new List<WBTRXModel>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select ""TicketNo"" from public.""WBTRX"" where ""statusoddo"" = false and ""WBType""='Despact' and  ""status"" ='C' and ""UnitCode"" <> 'BBM' ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WBTRXModel entity = new WBTRXModel();
                                entity.TicketNo = (string)reader["ticketno"];
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

        public List<WBTRXBLOCK1st> GetAllReceipDetailOneSttByTicket(string TicketNo)
        {
            List<WBTRXBLOCK1st> wBTRXes = new List<WBTRXBLOCK1st>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getreceiptdetailrsonestApi(:p_ticketno)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_ticketno", DbType.String).Value = TicketNo.Trim();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                WBTRXBLOCK1st entity = new WBTRXBLOCK1st();
                                entity.BlockID = reader["blockid"].ToString() == "0" ? "false" : (string)reader["blockid"];
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


                    string strSql = @"select * from public.getreceiptdetailrsecondndApi(:p_ticketno)";

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
                                entity.GradingTypeID = reader["gradingtypeid"].ToString() == "0" ? "false" : (string)reader["gradingtypeid"];
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

        public bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (var response = (HttpWebResponse)request.GetResponse())
                    return true;
            }
            catch (Exception err)
            {
                return false;
                throw new Exception(err.Message);               
            }

        }

        public string Getscale()
        {
            try
            {
                string scaleno = "";
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select scaleno from public.""scale""";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            scaleno = (string)reader["scaleno"];
                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return scaleno;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public bool Addscale(string scaleno)
        {
            throw new NotImplementedException();
        }

   
        public List<WBTRXGRADING> GetAllDespacthDetailSecondNdByTicket(string TicketNo)
        {
            List<WBTRXGRADING> wBTRXes = new List<WBTRXGRADING>();
            using (NpgsqlConnection con = new NpgsqlConnection())
            {
                try
                {
                    con.ConnectionString = stringCon;
                    con.Open();


                    string strSql = @"select * from public.getdespacthdetailrsecondndapi(:p_ticketno)";

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
                                entity.NoSegel1= reader["nosegel1"] == DBNull.Value || reader["nosegel1"].ToString() == "" ? "-" : (string)reader["nosegel1"];
                                entity.NoSegel2 = reader["nosegel2"] == DBNull.Value || reader["nosegel1"].ToString() == "" ? "-" : (string)reader["nosegel2"];
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

        public string GetUnitByCode(int idOddo)
        {
            string bpcode = "";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select ""BPCode"" from public.""BUSINESSPARTNER"" where  oddoid = '" + idOddo + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();

                            bpcode = (string)reader["BPCode"];

                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return bpcode;
                }
            }
            catch (NpgsqlException err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public bool UpdateStatusOddo(string TicketNo)
        {
            bool result = false;
            try
            {
               
                using (NpgsqlConnection con = new NpgsqlConnection())
                {
                    con.ConnectionString = stringCon;
                    con.Open();
                    string strSqContract = @"update public.""WBTRX""  set ""statusoddo"" = True where ""TicketNo"" = '" + TicketNo.Trim() + "' ";
                    using (NpgsqlCommand cmdContract = new NpgsqlCommand(strSqContract, con))
                    {
                        cmdContract.CommandType = CommandType.Text;
                        cmdContract.ExecuteNonQuery();
                    }
                }
                return result = true;
            }
            catch (NpgsqlException err)
            {
                return result = false;
            }
        }

        public string SendToOddoReceipt(string TicketNo)
        {

            try
            {
                string url = ConfigurationManager.AppSettings["endpointoddo"].ToString();
                string dboddo = ConfigurationManager.AppSettings["dboddo"].ToString();
                string methododdo = ConfigurationManager.AppSettings["methododdo"].ToString();
                string unitwb = ConfigurationManager.AppSettings["unitwb"].ToString();
                string pass = ConfigurationManager.AppSettings["passoddo"].ToString();


                WriteLogFile($"Body Jason To Oddo 0 ");

                WriteLogFile($"Status : Mulai Looping");

                //bool checkinet = CheckForInternetConnection(2000, url.Replace("/jsonrpc", ""));
                //if (checkinet)
                //{

                    WriteLogFile($"Proses Block : {TicketNo} ");
                    //string TicketNo = TicketNo;
                    //Block
                    List<WBTRXBLOCK1st> wBTRXBLOCK1St = new List<WBTRXBLOCK1st>();
                    wBTRXBLOCK1St = GetAllReceipDetailOneSttByTicket(TicketNo);

                    List<string> listblock = new List<string>();
                    for (int i = 0; i < wBTRXBLOCK1St.Count; i++)
                    {
                        ApiBlock apiBlock = new ApiBlock();
                        WriteLogFile($"Status : {TicketNo}");
                        apiBlock.block_id = wBTRXBLOCK1St[i].BlockID.ToString();
                        apiBlock.year_of_planting = wBTRXBLOCK1St[i].YoP.ToString();
                        apiBlock.bunches = wBTRXBLOCK1St[i].BunchesQty.ToString();
                        apiBlock.lose_fruit = wBTRXBLOCK1St[i].LFQty.ToString();
                        apiBlock.divisi = "Divisi";
                        JavaScriptSerializer arrblock = new JavaScriptSerializer();
                        string arrblockstring = arrblock.Serialize(apiBlock);
                        string[] block = { "0", "0", arrblockstring };

                        JavaScriptSerializer arrblocklast = new JavaScriptSerializer();
                        string arrblockstringlast = arrblocklast.Serialize(block);
                        listblock.Add(arrblockstringlast);

                    }

                    //Grading
                    WriteLogFile($"Proses Grading : {TicketNo} ");
                    List<WBTRXGRADING> wBTRXGRADING2Nds = new List<WBTRXGRADING>();
                    wBTRXGRADING2Nds = GetAllReceipDetailSecondNdByTicket(TicketNo);
                    List<string> listgrading = new List<string>();
                    for (int i = 0; i < wBTRXGRADING2Nds.Count; i++)
                    {
                        ApiGrading apiGrading = new ApiGrading();

                        apiGrading.grade_id = wBTRXGRADING2Nds[i].GradingTypeID.ToString();
                        apiGrading.Quantity = wBTRXGRADING2Nds[i].Quantity.ToString();


                        JavaScriptSerializer arrgrading = new JavaScriptSerializer();
                        string arrgradingstring = arrgrading.Serialize(apiGrading);
                        string[] grading = { "0", "0", arrgradingstring };

                        JavaScriptSerializer arrgradinglast = new JavaScriptSerializer();
                        string arrgradingstringlast = arrgradinglast.Serialize(grading);
                        listgrading.Add(arrgradingstringlast);
                    }

                    JavaScriptSerializer arrblockend = new JavaScriptSerializer();
                    string arrblockstringend = arrblockend.Serialize(listblock.ToList());
                    JavaScriptSerializer arrgradingend = new JavaScriptSerializer();
                    string arrgradingstringend = arrgradingend.Serialize(listgrading);

                    string sendblock = (arrblockstringend.Replace("\\", "").ToString()).Replace(@"""[", "[").Replace(@"]""", "]");
                    sendblock = sendblock.Substring(1, sendblock.Length - 1);
                    sendblock = sendblock.Remove(sendblock.Length - 1);
                    string sendgrading = (arrgradingstringend.Replace("\\", "").ToString()).Replace(@"""[", "[").Replace(@"]""", "]");
                    sendgrading = sendgrading.Substring(1, sendgrading.Length - 1);
                    sendgrading = sendgrading.Remove(sendgrading.Length - 1);

                    string[] plant_weighbridge_line = new string[] { sendblock };
                    string[] weighbridge_grade_line = new string[] { sendgrading };

                    WriteLogFile($"Proses Header : {TicketNo} ");
                    //Header
                    List<receiptHeader> header = new List<receiptHeader>();
                    receiptHeader headerrow = new receiptHeader();
                    headerrow = GetReceiptApiModel(TicketNo, plant_weighbridge_line, weighbridge_grade_line);
                    header.Add(headerrow);

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string headerstring = jss.Serialize(header);

                    parentsub _parentsub = new parentsub();
                    receiptmodel _receiptmodel = new receiptmodel();
                    _parentsub.service = "object";
                    _parentsub.method = methododdo;

                    context _context = new context();
                    _context.unit_code = unitwb.ToString();

                    JavaScriptSerializer contextss = new JavaScriptSerializer();
                    string contextstring = contextss.Serialize(_context);

                    string[] args = new string[] { dboddo, "2", pass, "plant.weighbridge", "create", headerstring, "{" + @"""context"":" + contextstring + "}" };
                    _parentsub.args = args;

                    parent _parent = new parent { jsonrpc = "2.0", param = _parentsub };

                    string output = jss.Serialize(_parent);
                    string send = (output.Replace("\\", "").ToString()).Replace(@"""[", "[").Replace(@"]""", "]").Replace("param", "params");

                    List<string> ls = new List<string>();
                    ls.Add(send);

                    string json = JsonConvert.SerializeObject(ls);
                    string jsonend = (json.Replace("\\", "").ToString()).Replace(@"""[", "[").Replace(@"]""", "]").Replace(@"""""", "").Replace(@"""]", "]").Replace(@"""{", "{").Replace(@"""{""", "{").Replace(@"""}""", "}");
                    //string jsonend = string jsonend.Replace(@"""vehicle_id"":",@
                    jsonend = jsonend.Substring(1, jsonend.Length - 1);
                    jsonend = jsonend.Remove(jsonend.Length - 1);
                    jsonend = jsonend.Replace(@"""false""", "false");
                    StringContent sc = new StringContent(jsonend, Encoding.UTF8, "application/json");
                    WriteLogFile($"Proses Send : {TicketNo} ");
                    WriteLogFile($"Body Send : {jsonend} ");
                    //using (var client = new HttpClient())
                    //{
                    WriteLogFile($"Send Data: {jsonend} ");
                    HttpClient c = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                    var x = c.PostAsync(url, sc).Result;

                    if (x.IsSuccessStatusCode)
                    {
                        WriteLogFile($"Respon Send Oddo 1st = {"Success!!!"}");
                        var result = x.Content.ReadAsStringAsync();

                        string bodyresult = result.Result;

                        WriteLogFile($"Respon oddo Body = {bodyresult}");
                        dynamic data = JObject.Parse(bodyresult);

                        Console.WriteLine(data.result);
                        int msg = data.result;
                        string[] resultarr = new string[] { data.result };
                        List<int> lsresultarr = new List<int>();
                        lsresultarr.Add(msg);
                        string jsonresult = JsonConvert.SerializeObject(lsresultarr);
                        WriteLogFile($"Status Send Oddo 1st = {"Success!!!"}");
                        WriteLogFile($"Status Send Oddo 1st = {resultarr}");
                        if (msg > 0)
                        {
                            parentsub _parentsub2 = new parentsub();
                            receiptmodel _receiptmodel2 = new receiptmodel();
                            _parentsub2.service = "object";
                            _parentsub2.method = "execute_kw";
                            string[] args2 = new string[] { dboddo, "2", pass, "plant.weighbridge", "button_confirm_second", jsonresult, "{}" };
                            _parentsub2.args = args2;
                            parent _parent2 = new parent { jsonrpc = "2.0", param = _parentsub2 };
                            HttpClient d = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });

                            string json2 = JsonConvert.SerializeObject(_parent2);
                            string jsonend2 = (json2.Replace("\\", "").ToString()).Replace("param", "params").Replace(@"""[", "[").Replace(@"]""", "]").Replace(@"""""", "").Replace(@"""]", "]").Replace(@"""{", "{").Replace(@"""{""", "{").Replace(@"""}""", "}");
                            WriteLogFile($"Send method button submit = {jsonend2}");
                            StringContent sc2 = new StringContent(jsonend2, Encoding.UTF8, "application/json");
                            var xx = d.PostAsync(url, sc2).Result;
                            if (xx.IsSuccessStatusCode)
                            {

                                var result2 = xx.Content.ReadAsStringAsync();
                                string bodyresult2 = result2.Result;

                                WriteLogFile($"Response method button submit = {bodyresult2}");
                                bool UpdOddoWb = UpdateStatusOddo(TicketNo);
                                WriteLogFile($"Update Status Ticket = {UpdOddoWb}");
                                if (UpdOddoWb)
                                {
                                    WriteLogFile($"Update Status WB = {"Success!!!"}");
                                }
                                else
                                {
                                    WriteLogFile($"Update Status WB = {"Failed!!!"}");
                                }
                                dynamic data2 = JObject.Parse(bodyresult2);
                                Console.WriteLine(data2.result);


                                string msg2 = data2.result.message;
                                WriteLogFile($"Status Send Oddo 2nd = {"Success!!!"}");
                                //MessageSuccess(this, msg2, "");
                            }
                        }
                        else
                        {
                            WriteLogFile($"Body Jason To Oddo 2 = {jsonend}");
                        }
                    }
                    else
                    {
                        var result = x.Content.ReadAsStringAsync();
                        string bodyresult = result.Result;
                        dynamic data = JObject.Parse(bodyresult);
                        Console.WriteLine(data.error.data.message);
                        string msg = data.error.data.message;
                        WriteLogFile($"Body Jason To Oddo 3 = {msg}");
                    }
                    c.Dispose();
                //}

                return "success";

                //}
            }
            catch (Exception err)
            {
                return "failed";
                WriteLogFile($"Body Jason To Oddo 4 = {err.Message.ToString()}");
            }
        }

        public static void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileWB.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }
    }
}