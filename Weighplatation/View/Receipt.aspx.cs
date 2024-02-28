using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Npgsql;
using DevExpress.Web;
using Weighplatation.Model;
using Weighplatation.Repository;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Web.Services;

namespace Weighplatation.View
{
    public partial class Receipt : System.Web.UI.Page
    {
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        public static ApiOddoRepo apiOddoRepo = new ApiOddoRepo();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"] == "")
            //{
            //    Response.Redirect("/View/Default.aspx");
            //}
            if (!IsPostBack)
            {
                
                GridData();
            }
            else
            {
                List<ReceiptModel> lt = new List<ReceiptModel>();
                lt = Session["Data"] as List<ReceiptModel>;
                grid.DataSource = lt;

            }
            //if (Session["Approval"].ToString() == "true")
            //{
            //    btnPosting.Visible = true;
            //}
            //else {
            //    btnPosting.Visible = false;
            //}
        }

        void GridData() {
            List<ReceiptModel> receiptModel = _receiptRepo.GetAllReceiptByToday();
            grid.DataSource = receiptModel;
            Session["Data"] = receiptModel;
            grid.DataBind();
        }

        protected void btnNewWB_Click(object sender, EventArgs e)
        {
            Response.Redirect("/View/ReceiptFirst.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<ReceiptModel> receiptModels = new List<ReceiptModel>();
            receiptModels = _receiptRepo.GetAllReceipByFilter(txtTicketNo.Text, txtContract.Text, txtStartDate.Text, txtEndDate.Text);
            Session["Data"] = receiptModels;
            grid.DataSource = receiptModels;
            grid.DataBind();

        }


        protected void btnPosting_Click(object sender, EventArgs e)
        {
            try
            {
                bool result = _receiptRepo.PostingReceipt();
                //bool resultoddo = SendDataOddoToday();
                if (result)
                {
                    MessageSuccess(this, "success", "Success!");
                    Response.Redirect("/View/Receipt.aspx");
                }              
            }
            catch (Exception err)
            {

                Console.WriteLine(err.Message);
            }
           
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {

            try
            {
                bool resultoddo = SendDataOddoToday();
                Console.WriteLine("start");
                if (resultoddo)
                {
                    Console.WriteLine("Finish");
                    MessageSuccess(this, "Synchronize success", "Success!");
                    //Response.Redirect("/View/ManualReceipt.aspx");
                }
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }


        }

        bool SendDataOddoToday()
        {
            try
            {
                List<WBTRXModel> wBTRXModels = new List<WBTRXModel>();
                wBTRXModels = apiOddoRepo.GetAllSendApiReceipt();
                //WriteLogFile($"Status : Get Data Transaksi");
                Console.WriteLine(wBTRXModels);
                foreach (var item in wBTRXModels)
                {
                    SendToOddoReceipt(item.TicketNo);
                }
                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;

            }

        }

        public static void SendToOddoReceipt(string TicketNo)
        {

            try
            {
                string url = ConfigurationManager.AppSettings["endpointoddo"].ToString();
                string dboddo = ConfigurationManager.AppSettings["dboddo"].ToString();
                string methododdo = ConfigurationManager.AppSettings["methododdo"].ToString();
                string unitwb = ConfigurationManager.AppSettings["unitwb"].ToString();
                string pass = ConfigurationManager.AppSettings["passoddo"].ToString();


                //WriteLogFile($"Body Jason To Oddo 0 ");

                //WriteLogFile($"Status : Mulai Looping");

                bool checkinet = apiOddoRepo.CheckForInternetConnection(2000, url.Replace("/jsonrpc", ""));
                if (checkinet)
                {

                    //WriteLogFile($"Proses Block : {TicketNo} ");
                    //string TicketNo = TicketNo;
                    //Block
                    List<WBTRXBLOCK1st> wBTRXBLOCK1St = new List<WBTRXBLOCK1st>();
                    wBTRXBLOCK1St = apiOddoRepo.GetAllReceipDetailOneSttByTicket(TicketNo);

                    List<string> listblock = new List<string>();
                    for (int i = 0; i < wBTRXBLOCK1St.Count; i++)
                    {
                        ApiBlock apiBlock = new ApiBlock();
                        //WriteLogFile($"Status : {TicketNo}");
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
                    //WriteLogFile($"Proses Grading : {TicketNo} ");
                    List<WBTRXGRADING> wBTRXGRADING2Nds = new List<WBTRXGRADING>();
                    wBTRXGRADING2Nds = apiOddoRepo.GetAllReceipDetailSecondNdByTicket(TicketNo);
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

                    //WriteLogFile($"Proses Header : {TicketNo} ");
                    //Header
                    List<receiptHeader> header = new List<receiptHeader>();
                    receiptHeader headerrow = new receiptHeader();
                    headerrow = apiOddoRepo.GetReceiptApiModel(TicketNo, plant_weighbridge_line, weighbridge_grade_line);
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
                    //WriteLogFile($"Proses Send : {TicketNo} ");
                    //WriteLogFile($"Body Send : {jsonend} ");
                    //using (var client = new HttpClient())
                    //{
                    //WriteLogFile($"Send Data: {jsonend} ");
                    HttpClient c = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                    var x = c.PostAsync(url, sc).Result;

                    if (x.IsSuccessStatusCode)
                    {
                        //WriteLogFile($"Respon Send Oddo 1st = {"Success!!!"}");
                        var result = x.Content.ReadAsStringAsync();

                        string bodyresult = result.Result;

                        //WriteLogFile($"Respon oddo Body = {bodyresult}");
                        dynamic data = JObject.Parse(bodyresult);

                        Console.WriteLine(data.result);
                        int msg = data.result;
                        string[] resultarr = new string[] { data.result };
                        List<int> lsresultarr = new List<int>();
                        lsresultarr.Add(msg);
                        string jsonresult = JsonConvert.SerializeObject(lsresultarr);
                        //WriteLogFile($"Status Send Oddo 1st = {"Success!!!"}");
                        //WriteLogFile($"Status Send Oddo 1st = {resultarr}");
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
                            //WriteLogFile($"Send method button submit = {jsonend2}");
                            StringContent sc2 = new StringContent(jsonend2, Encoding.UTF8, "application/json");
                            var xx = d.PostAsync(url, sc2).Result;
                            if (xx.IsSuccessStatusCode)
                            {

                                var result2 = xx.Content.ReadAsStringAsync();
                                string bodyresult2 = result2.Result;

                                //WriteLogFile($"Response method button submit = {bodyresult2}");
                                bool UpdOddoWb = apiOddoRepo.UpdateStatusOddo(TicketNo);
                                //WriteLogFile($"Update Status Ticket = {UpdOddoWb}");
                                //if (UpdOddoWb)
                                //{
                                //    WriteLogFile($"Update Status WB = {"Success!!!"}");
                                //}
                                //else
                                //{
                                //    WriteLogFile($"Update Status WB = {"Failed!!!"}");
                                //}
                                //dynamic data2 = JObject.Parse(bodyresult2);
                                //Console.WriteLine(data2.result);


                                //string msg2 = data2.message;
                                //WriteLogFile($"Status Send Oddo 2nd = {"Success!!!"}");
                                //MessageSuccess(this, msg2, "");
                            }
                        }
                        else
                        {
                            //WriteLogFile($"Body Jason To Oddo 2 = {jsonend}");
                        }
                    }
                    else
                    {
                        var result = x.Content.ReadAsStringAsync();
                        string bodyresult = result.Result;
                        dynamic data = JObject.Parse(bodyresult);
                        //Console.WriteLine(data.error.data.message);
                        //string msg = data.error.data.message;
                        //WriteLogFile($"Body Jason To Oddo 3 = {msg}");
                    }
                    c.Dispose();
                    //}
                    //Console.WriteLine("xxxx");
                    //string result = apiOddoRepo.SendToOddoReceipt(TicketNo);
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
                //WriteLogFile($"Body Jason To Oddo 4 = {err.Message.ToString()}");
            }
        }


        protected void MessageSuccess(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal.fire('" + Title + "','" + Message + "','success');", true);
        }

        protected void MessageError(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal.fire('" + Title + "','" + Message + "','error');", true);
        }

        protected void MessageWarning(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "warningalert();", true);
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            List<ReceiptModel> lt = new List<ReceiptModel>();
            lt = Session["Data"] as List<ReceiptModel>;
            grid.DataSource = lt;
            //grid.DataBind();
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