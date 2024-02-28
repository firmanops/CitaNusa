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
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Weighplatation.View
{
    public partial class ManualDespact : System.Web.UI.Page
    {
        public DespactRepo _despactRepo = new DespactRepo();
        public static ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridData();
            }
            else
            {
                List<DespactModel> lt = new List<DespactModel>();
                lt = Session["Data"] as List<DespactModel>;
                grid.DataSource = lt;

            }
            //if (Session["Approval"].ToString() == "true")
            //{
            //    btnPosting.Visible = true;
            //}
            //else
            //{
            //    btnPosting.Visible = false;
            //}
        }

        void GridData()
        {
            List<DespactModel> receiptModel = _despactRepo.GetAllDespactByToday();
            grid.DataSource = receiptModel;
            Session["Data"] = receiptModel;
            grid.DataBind();
        }

        protected void btnNewWB_Click(object sender, EventArgs e)
        {
            Response.Redirect("/View/ManualDespactFirst.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<DespactModel> despactModels = new List<DespactModel>();
            despactModels = _despactRepo.GetAllReceipByFilter(txtTicketNo.Text, txtContract.Text, txtStartDate.Text, txtEndDate.Text);
            grid.DataSource = despactModels;
            Session["Data"] = despactModels;
            grid.DataBind();
        }
        protected void btnPosting_Click(object sender, EventArgs e)
        {
            try
            {
                bool result = _despactRepo.PostingDespacth();
                //bool resultoddo = SendDataOddoToday();
                if (result )
                {
                    MessageSuccess(this, "success", "Success!");
                    Response.Redirect("/View/ManualDespact.aspx");
                }
            }
            catch (Exception err)
            {

                Console.WriteLine(err.Message);
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
            List<DespactModel> lt = new List<DespactModel>();
            lt = Session["Data"] as List<DespactModel>;
            grid.DataSource = lt;
        }
        bool SendDataOddoToday()
        {
            try
            {
                List<WBTRXModel> wBTRXModelsDespacth = new List<WBTRXModel>();
                wBTRXModelsDespacth = apiOddoRepo.GetAllSendApiDespact();
                foreach (var item in wBTRXModelsDespacth)
                {
                    SendToOddoDespact(item.TicketNo);
                }
                return true;
            }
            catch (Exception err)
            {

                Console.WriteLine(err.Message);
                return false;
            }

        }
        //public static void SendToOddoDespact(string TicketNo)
        //{
        //    try
        //    {
        //        string url = ConfigurationManager.AppSettings["endpointoddo"].ToString();
        //        string dboddo = ConfigurationManager.AppSettings["dboddo"].ToString();
        //        string methododdo = ConfigurationManager.AppSettings["methododdo"].ToString();
        //        string unitwb = ConfigurationManager.AppSettings["unitwb"].ToString();
        //        string pass = ConfigurationManager.AppSettings["passoddo"].ToString();
        //        bool checkinet = apiOddoRepo.CheckForInternetConnection(1000, url.Replace("/jsonrpc", ""));
        //        if (checkinet)
        //        {
        //            //WriteLogFile($"checkinet");
        //            //Grading
        //            List<WBTRXGRADING> wBTRXGRADING2Nds = new List<WBTRXGRADING>();
        //            wBTRXGRADING2Nds = apiOddoRepo.GetAllDespacthDetailSecondNdByTicket(TicketNo);
        //            List<ApiQualitiy> QualitiyModel = new List<ApiQualitiy>();
        //            List<string> listgrading = new List<string>();
        //            ApiQualitiy QualitiyModelRow = new ApiQualitiy();
        //            //WriteLogFile($"wBTRXGRADING2Nds = {wBTRXGRADING2Nds}");
        //            if (wBTRXGRADING2Nds.Count > 0)
        //            {
        //                for (int i = 0; i < wBTRXGRADING2Nds.Count; i++)
        //                {


        //                    ApiQualitiy apiGrading = new ApiQualitiy();
        //                    //WriteLogFile($"wBTRXGRADING2Nds = True");
        //                    if (wBTRXGRADING2Nds[i].GradingName.ToString() == "DOBI")
        //                    {
        //                        QualitiyModelRow.dobi = wBTRXGRADING2Nds[i].Quantity.ToString() == null ? "false" : wBTRXGRADING2Nds[i].Quantity.ToString();
        //                    }
        //                    if (wBTRXGRADING2Nds[i].GradingName.ToString() == "FFA")
        //                    {
        //                        QualitiyModelRow.ffa = wBTRXGRADING2Nds[i].Quantity.ToString() == null ? "false" : wBTRXGRADING2Nds[i].Quantity.ToString();
        //                    }
        //                    if (wBTRXGRADING2Nds[i].GradingName.ToString() == "MOIST")
        //                    {
        //                        QualitiyModelRow.moisture = wBTRXGRADING2Nds[i].Quantity.ToString() == null ? "false" : wBTRXGRADING2Nds[i].Quantity.ToString();
        //                    }
        //                    if (wBTRXGRADING2Nds[i].GradingName.ToString() == "IMPURITIES")
        //                    {
        //                        QualitiyModelRow.impurities = wBTRXGRADING2Nds[i].Quantity.ToString() == null ? "false" : wBTRXGRADING2Nds[i].Quantity.ToString();
        //                    }

        //                    QualitiyModelRow.no_segel_1 = wBTRXGRADING2Nds[i].NoSegel1;
        //                    QualitiyModelRow.no_segel_2 = wBTRXGRADING2Nds[i].NoSegel2;


        //                }
        //            }
        //            else
        //            {
        //                //WriteLogFile($"wBTRXGRADING2Nds = False");
        //                QualitiyModelRow.dobi = "false";
        //                QualitiyModelRow.ffa = "false";
        //                QualitiyModelRow.moisture = "false";
        //                QualitiyModelRow.impurities = "false";
        //                QualitiyModelRow.no_segel_1 = "false";
        //                QualitiyModelRow.no_segel_2 = "false";
        //            }
        //            QualitiyModel.Add(QualitiyModelRow);

        //            //WriteLogFile($"QualitiyModel");

        //            JavaScriptSerializer arrgrading = new JavaScriptSerializer();
        //            string arrgradingstring = arrgrading.Serialize(QualitiyModel[0]);
        //            string[] grading = { "0", "0", arrgradingstring };

        //            JavaScriptSerializer arrgradinglast = new JavaScriptSerializer();
        //            string arrgradingstringlast = arrgradinglast.Serialize(grading);
        //            listgrading.Add(arrgradingstringlast);

        //            JavaScriptSerializer arrgradingend = new JavaScriptSerializer();
        //            string arrgradingstringend = arrgradingend.Serialize(listgrading);

        //            string sendgrading = (arrgradingstringend.Replace("\\", "").ToString()).Replace(@"""[", "[").Replace(@"]""", "]");
        //            sendgrading = sendgrading.Substring(1, sendgrading.Length - 1);
        //            sendgrading = sendgrading.Remove(sendgrading.Length - 1);
        //            //WriteLogFile($"sendgrading = {sendgrading}");
        //            //string[] plant_weighbridge_line = new string[] { sendblock };
        //            //string[] weighbridge_grade_line = new string[] { sendgrading };
        //            string[] weighbridge_quality_line = new string[] { sendgrading };
        //            //Header
        //            List<despactHeader> header = new List<despactHeader>();
        //            despactHeader headerrow = new despactHeader();
        //            headerrow = apiOddoRepo.GetDespactApiModel(TicketNo, weighbridge_quality_line);
        //            header.Add(headerrow);

        //            JavaScriptSerializer jss = new JavaScriptSerializer();
        //            string headerstring = jss.Serialize(header);

        //            //WriteLogFile($"GetDespactApiModel={headerstring}");

        //            parentsub _parentsub = new parentsub();
        //            receiptmodel _receiptmodel = new receiptmodel();
        //            _parentsub.service = "object";
        //            _parentsub.method = methododdo;

        //            context _context = new context();
        //            _context.unit_code = unitwb.ToString();

        //            JavaScriptSerializer contextss = new JavaScriptSerializer();
        //            string contextstring = contextss.Serialize(_context);

        //            string[] args = new string[] { dboddo, "2", pass, "plant.weighbridge", "create", headerstring.Replace("despact", "despach"), "{" + @"""context"":" + contextstring + "}" };
        //            _parentsub.args = args;

        //            parent _parent = new parent { jsonrpc = "2.0", param = _parentsub };

        //            string output = jss.Serialize(_parent);
        //            string send = (output.Replace("\\", "").ToString()).Replace(@"""[", "[").Replace(@"]""", "]").Replace("param", "params");

        //            List<string> ls = new List<string>();
        //            ls.Add(send);

        //            string json = JsonConvert.SerializeObject(ls);
        //            string jsonend = (json.Replace("\\", "").ToString()).Replace(@"""[", "[").Replace(@"]""", "]").Replace(@"""""", "").Replace(@"""]", "]").Replace(@"""{", "{").Replace(@"""{""", "{").Replace(@"""}""", "}");
        //            //string jsonend = string jsonend.Replace(@"""vehicle_id"":",@
        //            jsonend = jsonend.Substring(1, jsonend.Length - 1);
        //            jsonend = jsonend.Remove(jsonend.Length - 1);
        //            jsonend = jsonend.Replace(@"""false""", "false");
        //            StringContent sc = new StringContent(jsonend, Encoding.UTF8, "application/json");
        //            //WriteLogFile($"Proses Send : {TicketNo} ");
        //            //WriteLogFile($"Body Send : {jsonend} ");
        //            HttpClient c = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
        //            var x = c.PostAsync(url, sc).Result;

        //            if (x.IsSuccessStatusCode)
        //            {
        //                //WriteLogFile($"Respon Send Oddo 1st = {"Success!!!"}");
        //                var result = x.Content.ReadAsStringAsync();
        //                string bodyresult = result.Result;
        //                //WriteLogFile($"Response 1st = {bodyresult}");
        //                dynamic data = JObject.Parse(bodyresult);
        //                Console.WriteLine(data.result);
        //                //WriteLogFile($"Data 1st = {data}");
        //                int msg = data.result;
        //                string[] resultarr = new string[] { data.result };
        //                List<int> lsresultarr = new List<int>();
        //                lsresultarr.Add(msg);
        //                string jsonresult = JsonConvert.SerializeObject(lsresultarr);
        //                //WriteLogFile($"Status Send Oddo 1st = {"Success!!!"}");
        //                //WriteLogFile($"Status Send Oddo 1st = {resultarr}");
        //                //WriteLogFile($"Message 1st = {msg}");
        //                if (msg > 0)
        //                {

        //                    parentsub _parentsub2 = new parentsub();
        //                    receiptmodel _receiptmodel2 = new receiptmodel();
        //                    _parentsub2.service = "object";
        //                    _parentsub2.method = "execute_kw";
        //                    string[] args2 = new string[] { dboddo, "2", pass, "plant.weighbridge", "button_confirm_second", jsonresult, "{}" };
        //                    _parentsub2.args = args2;
        //                    parent _parent2 = new parent { jsonrpc = "2.0", param = _parentsub2 };
        //                    HttpClient d = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });

        //                    string json2 = JsonConvert.SerializeObject(_parent2);
        //                    string jsonend2 = (json2.Replace("\\", "").ToString()).Replace("param", "params").Replace(@"""[", "[").Replace(@"]""", "]").Replace(@"""""", "").Replace(@"""]", "]").Replace(@"""{", "{").Replace(@"""{""", "{").Replace(@"""}""", "}");
        //                    //WriteLogFile($"Send method button submit = {jsonend2}");
        //                    StringContent sc2 = new StringContent(jsonend2, Encoding.UTF8, "application/json");
        //                    var xx = d.PostAsync(url, sc2).Result;
        //                    //WriteLogFile($"Message 2nd = {xx}");
        //                    if (xx.IsSuccessStatusCode)
        //                    {
        //                        var result2 = xx.Content.ReadAsStringAsync();
        //                        string bodyresult2 = result2.Result;


        //                        //WriteLogFile($"Response method button submit = {bodyresult2}");
        //                        bool UpdOddoWb = apiOddoRepo.UpdateStatusOddo(TicketNo);
        //                        //WriteLogFile($"Update Status Ticket = {UpdOddoWb}");
        //                        //if (UpdOddoWb)
        //                        //{
        //                        //    WriteLogFile($"Update Status WB = {"Success!!!"}");
        //                        //}
        //                        //else
        //                        //{
        //                        //    WriteLogFile($"Update Status WB = {"Failed!!!"}");
        //                        //}
        //                        //dynamic data2 = JObject.Parse(bodyresult2);
        //                        //Console.WriteLine(data2.result);
        //                        //string msg2 = data2.result.message;
        //                        //WriteLogFile($"Status Send Oddo 2nd = {"Success!!!"}");
        //                    }
        //                }
        //                else
        //                {
        //                    //WriteLogFile($"Body Jason To Oddo 2 = {jsonend}");
        //                }
        //            }
        //            else
        //            {
        //                var result = x.Content.ReadAsStringAsync();
        //                string bodyresult = result.Result;
        //                dynamic data = JObject.Parse(bodyresult);
        //                Console.WriteLine(data.error.data.message);
        //                string msg = data.error.data.message;

        //                //WriteLogFile($"Body Jason To Oddo 3 = {jsonend}");

        //                //MessageError(this, msg, "");
        //            }
        //            c.Dispose();
        //            //}
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        throw new Exception(err.Message);
        //        //WriteLogFile($"Body Jason To Oddo 4 = {err.Message.ToString()}");
        //    }

        //}

        public static void SendToOddoDespact(string TicketNo)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["endpointoddo"].ToString();
                string dboddo = ConfigurationManager.AppSettings["dboddo"].ToString();
                string methododdo = ConfigurationManager.AppSettings["methododdo"].ToString();
                string unitwb = ConfigurationManager.AppSettings["unitwb"].ToString();
                string pass = ConfigurationManager.AppSettings["passoddo"].ToString();
                bool checkinet = apiOddoRepo.CheckForInternetConnection(1000, url.Replace("/jsonrpc", ""));
                if (checkinet)
                {
                    //WriteLogFile($"checkinet");
                    //Grading
                    List<WBTRXGRADING> wBTRXGRADING2Nds = new List<WBTRXGRADING>();
                    wBTRXGRADING2Nds = apiOddoRepo.GetAllDespacthDetailSecondNdByTicket(TicketNo);
                    List<ApiQualitiy> QualitiyModel = new List<ApiQualitiy>();
                    List<string> listgrading = new List<string>();
                    ApiQualitiy QualitiyModelRow = new ApiQualitiy();
                    //WriteLogFile($"wBTRXGRADING2Nds = {wBTRXGRADING2Nds}");
                    if (wBTRXGRADING2Nds.Count > 0)
                    {
                        for (int i = 0; i < wBTRXGRADING2Nds.Count; i++)
                        {


                            ApiQualitiy apiGrading = new ApiQualitiy();
                            //WriteLogFile($"wBTRXGRADING2Nds = True");
                            if (wBTRXGRADING2Nds[i].GradingName.ToString() == "DOBI")
                            {
                                QualitiyModelRow.dobi = wBTRXGRADING2Nds[i].Quantity.ToString() == null ? "false" : wBTRXGRADING2Nds[i].Quantity.ToString();
                            }
                            if (wBTRXGRADING2Nds[i].GradingName.ToString() == "FFA")
                            {
                                QualitiyModelRow.ffa = wBTRXGRADING2Nds[i].Quantity.ToString() == null ? "false" : wBTRXGRADING2Nds[i].Quantity.ToString();
                            }
                            if (wBTRXGRADING2Nds[i].GradingName.ToString() == "MOIST")
                            {
                                QualitiyModelRow.moisture = wBTRXGRADING2Nds[i].Quantity.ToString() == null ? "false" : wBTRXGRADING2Nds[i].Quantity.ToString();
                            }
                            if (wBTRXGRADING2Nds[i].GradingName.ToString() == "IMPURITIES")
                            {
                                QualitiyModelRow.impurities = wBTRXGRADING2Nds[i].Quantity.ToString() == null ? "false" : wBTRXGRADING2Nds[i].Quantity.ToString();
                            }

                            QualitiyModelRow.no_segel_1 = wBTRXGRADING2Nds[i].NoSegel1;
                            QualitiyModelRow.no_segel_2 = wBTRXGRADING2Nds[i].NoSegel2;


                        }
                    }
                    else
                    {
                        //WriteLogFile($"wBTRXGRADING2Nds = False");
                        QualitiyModelRow.dobi = "false";
                        QualitiyModelRow.ffa = "false";
                        QualitiyModelRow.moisture = "false";
                        QualitiyModelRow.impurities = "false";
                        QualitiyModelRow.no_segel_1 = "false";
                        QualitiyModelRow.no_segel_2 = "false";
                    }
                    QualitiyModel.Add(QualitiyModelRow);

                    //WriteLogFile($"QualitiyModel");

                    JavaScriptSerializer arrgrading = new JavaScriptSerializer();
                    string arrgradingstring = arrgrading.Serialize(QualitiyModel[0]);
                    string[] grading = { "0", "0", arrgradingstring };

                    JavaScriptSerializer arrgradinglast = new JavaScriptSerializer();
                    string arrgradingstringlast = arrgradinglast.Serialize(grading);
                    listgrading.Add(arrgradingstringlast);

                    JavaScriptSerializer arrgradingend = new JavaScriptSerializer();
                    string arrgradingstringend = arrgradingend.Serialize(listgrading);

                    string sendgrading = (arrgradingstringend.Replace("\\", "").ToString()).Replace(@"""[", "[").Replace(@"]""", "]");
                    sendgrading = sendgrading.Substring(1, sendgrading.Length - 1);
                    sendgrading = sendgrading.Remove(sendgrading.Length - 1);
                    //WriteLogFile($"sendgrading = {sendgrading}");
                    //string[] plant_weighbridge_line = new string[] { sendblock };
                    //string[] weighbridge_grade_line = new string[] { sendgrading };
                    string[] weighbridge_quality_line = new string[] { sendgrading };
                    //Header
                    List<despactHeader> header = new List<despactHeader>();
                    despactHeader headerrow = new despactHeader();
                    headerrow = apiOddoRepo.GetDespactApiModel(TicketNo, weighbridge_quality_line);
                    header.Add(headerrow);

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string headerstring = jss.Serialize(header);

                    //WriteLogFile($"GetDespactApiModel={headerstring}");

                    parentsub _parentsub = new parentsub();
                    receiptmodel _receiptmodel = new receiptmodel();
                    _parentsub.service = "object";
                    _parentsub.method = methododdo;

                    context _context = new context();
                    _context.unit_code = unitwb.ToString();

                    JavaScriptSerializer contextss = new JavaScriptSerializer();
                    string contextstring = contextss.Serialize(_context);

                    string[] args = new string[] { dboddo, "2", pass, "plant.weighbridge", "create", headerstring.Replace("despact", "despach"), "{" + @"""context"":" + contextstring + "}" };
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
                    HttpClient c = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                    var x = c.PostAsync(url, sc).Result;

                    if (x.IsSuccessStatusCode)
                    {
                        //WriteLogFile($"Respon Send Oddo 1st = {"Success!!!"}");
                        var result = x.Content.ReadAsStringAsync();
                        string bodyresult = result.Result;
                        //WriteLogFile($"Response 1st = {bodyresult}");
                        dynamic data = JObject.Parse(bodyresult);
                        //Console.WriteLine(data.result);
                        //WriteLogFile($"Data 1st = {data}");
                        int msg = data.result;
                        string[] resultarr = new string[] { data.result };
                        List<int> lsresultarr = new List<int>();
                        lsresultarr.Add(msg);
                        string jsonresult = JsonConvert.SerializeObject(lsresultarr);
                        //WriteLogFile($"Status Send Oddo 1st = {"Success!!!"}");
                        //WriteLogFile($"Status Send Oddo 1st = {resultarr}");
                        //WriteLogFile($"Message 1st = {msg}");
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
                            //WriteLogFile($"Message 2nd = {xx}");
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
                                //string msg2 = data2.result.message;
                                //WriteLogFile($"Status Send Oddo 2nd = {"Success!!!"}");
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
                        //string bodyresult = result.Result;
                        //dynamic data = JObject.Parse(bodyresult);
                        //Console.WriteLine(data.error.data.message);
                        //string msg = data.error.data.message;

                        //WriteLogFile($"Body Jason To Oddo 3 = {jsonend}");

                        //MessageError(this, msg, "");
                    }
                    c.Dispose();
                    //}
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
                //WriteLogFile($"Body Jason To Oddo 4 = {err.Message.ToString()}");
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

        protected void btnSend_Click(object sender, EventArgs e)
        {
            bool resultoddo = SendDataOddoToday();
            if (resultoddo)
            {
                MessageSuccess(this, "success", "Success!");
                //Response.Redirect("/View/ManualDespact.aspx");
            }
        }
    }
}