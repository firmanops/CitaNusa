using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using Weighplatation.Model;
using Weighplatation.Repository;
using DevExpress.Web.Bootstrap;
using System.Data;
using DevExpress.Web;
using DevExpress.Data;
using System.Collections;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace Weighplatation.View
{
    public partial class DescpatView : System.Web.UI.Page
    {
        public DespactRepo _despactRepo = new DespactRepo();
        public ReportRepo reportRepo = new ReportRepo();
        //public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        public static ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            txtTransactionDate.Text = DateTime.Now.ToString("dd-MM-yyy HH:mm:ss");
            Session["TicketNo"] = Request.QueryString["Ticket"];
            Session["NetWeight"] = "";
            Session["Sum1ST"] = "";
            Session["SumDeducation"] = "";
            Session["Sum2ND"] = "";

            GetAllWbTrx(Session["TicketNo"].ToString());
            GetAllReceipDetailDespactTicket(Session["TicketNo"].ToString());
        }
        void GetAllWbTrx(string TicketNo)
        {
            try
            {
                DespactModelNd despactModelNd = new DespactModelNd();
                TranspoterModel transpoterModel = new TranspoterModel();
                List<WBTRXGRADING2nd> _WBTRXGRADING2nd = new List<WBTRXGRADING2nd>();
                Session["weight1st"] = "";
                WBOWNER getUnitWBOwner = _despactRepo.GetUnitWBOwner();

                despactModelNd = _despactRepo.GetAllDespactByTicket(TicketNo);
                if (despactModelNd != null)
                {
                    txtTicketNo.Text = despactModelNd.TicketNo;
                    txtItem.Text = despactModelNd.ProductName;
                    txtCompanyName.Text = despactModelNd.BPName;
                    txtVehicle.Text = despactModelNd.VehicleID;
                    //txtUnitCode.Text = despactModelNd.UnitCode;
                    txtUnit.Text = getUnitWBOwner.UnitName;
                    transpoterModel = _despactRepo.GetTransporter(txtVehicle.Text);
                    txtTransporter.Text = transpoterModel.BPName;
                    txtContract.Text = despactModelNd.ContractNo;
                    txtDriver.Text = despactModelNd.DriverName;
                    txtLisensiNo.Text = despactModelNd.Lisense;
                    txtWB1.Text = String.Format("{0,15:#,##0 ;(#,##0);-   }", despactModelNd.Weight1st);
                    txtWB2.Text = String.Format("{0,15:#,##0 ;(#,##0);-   }", despactModelNd.Weight2nd);
                    txtDelivery.Text = despactModelNd.BPName;
                    txtDnNo.Text = despactModelNd.DnNo;                  
                    Session["weight1st"] = despactModelNd.Weight1st;
                    Session["weight2nd"] = despactModelNd.Weight2nd;
                    img1.ContentBytes = despactModelNd.WBImagefront1;
                    img2.ContentBytes = despactModelNd.WBImagefront2;
                }
                else
                {
                    throw new Exception("Data not found please another Ticket");
                }
            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);
            }

        }

        void GetAllReceipDetailDespactTicket(string TicketNo)
        {
            try
            {
               

                List<WBTRXGRADING> WBGrading = new List<WBTRXGRADING>();
                WBGrading = _despactRepo.GetAllDespactDetailSecondNdByTicket(TicketNo);
                if (WBGrading != null)
                {
                    griddetail2nd.DataSource = WBGrading;
                    griddetail2nd.DataBind();
                }
                else
                {
                    throw new Exception("Data not found please another Ticket");
                }

                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                NetReceiptTotalModel listNetTotal = new NetReceiptTotalModel();
                Tuple<List<NetWeightModel>, NetReceiptTotalModel> resultTuple = _despactRepo.GetNetWeight(TicketNo);

                listNetWeight = resultTuple.Item1;
                listNetTotal = resultTuple.Item2;
                griddetailnet.DataSource = listNetWeight;
                griddetailnet.DataBind();


                Session["Sum1ST"] = String.Format("{0,15:#,##0 ;(#,##0);-   }", Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[0].WeightHeavy.ToString()));
                Session["SumDeducation"] = String.Format("{0,15:#,##0 ;(#,##0);-   }", listNetTotal.Quantity);
                Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);-   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - listNetTotal.Quantity);
                Session["NetWeight"] = String.Format("{0,15:#,##0 ;(#,##0);-   }", double.Parse(listNetWeight[1].WeightHeavy.ToString()) - double.Parse(listNetWeight[0].WeightHeavy.ToString()));

            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            bool result = _despactRepo.CountingPrint(txtTicketNo.Text);
            if (result)
            {
                Console.WriteLine("Success");
            }

            List<DespactModelNd> despachModelNd = new List<DespactModelNd>();
            List<WBTRXGRADING2nd> wBTRXGRADING2Nds = new List<WBTRXGRADING2nd>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();
            List<NetWeightModel> netWeightModels = new List<NetWeightModel>();

            despachModelNd = reportRepo.GetFormDespachHeader(txtTicketNo.Text);
            wBTRXGRADING2Nds = reportRepo.GetFromDespachGrading(txtTicketNo.Text);
            Companyls.Add(_despactRepo.GetUnitByCode(Session["UnitCode"].ToString()));
            netWeightModels = reportRepo.GetNetWeightModel(txtTicketNo.Text);



            string Ext = "pdf";
            string Filename = "TicketDespach";
            HttpContext.Current.Items["Ext"] = Ext;
            HttpContext.Current.Items["Filename"] = Filename;
            HttpContext.Current.Items["Ticketls"] = despachModelNd;
            //HttpContext.Current.Items["Blockls"] = wBTRXBLOCK1Sts;
            HttpContext.Current.Items["Gradingls"] = wBTRXGRADING2Nds;
            HttpContext.Current.Items["Companyls"] = Companyls;
            HttpContext.Current.Items["NetWeightls"] = netWeightModels;
            HttpContext.Current.Items["User"] = Session["UserName"];
            Server.Transfer("/Report/FormTicketDespach.aspx");



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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Despact.aspx");
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {

                string err = "";
                if (Double.Parse(txtWB1.Text) > Double.Parse(txtWB1.Text))
                {
                    err = "Weigh 1st Can't be biger with Weigh 2nd";
                    MessageWarning(this, err, "Warning");
                }


                if (err == "")
                {
                    bool result = false;

                    WBTRXModel _WBTRXModel = new WBTRXModel();
                    _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                    double deduc = 0;
                    Tuple<bool> resultTuple = _receiptRepo.ApproveByTicket(_WBTRXModel);

                    result = resultTuple.Item1;

                    if (result)
                    {
                        MessageSuccess(this, "success", "Success!");
                        btnPrints.Enabled = true;
                        Response.Redirect("/View/Despact.aspx");
                    }
                    else
                    {
                        throw new Exception("Faild Record");
                    }
                }
            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);

                Console.WriteLine(err.Message);
            }

        }
        void SendToOddo()
        {
            try
            {
                string url = ConfigurationManager.AppSettings["endpointoddo"].ToString();
                string dboddo = ConfigurationManager.AppSettings["dboddo"].ToString();
                string methododdo = ConfigurationManager.AppSettings["methododdo"].ToString();
                string unitwb = Session["UnitCode"].ToString();
                string pass = ConfigurationManager.AppSettings["passoddo"].ToString();
                bool checkinet = apiOddoRepo.CheckForInternetConnection(1000, url.Replace("/jsonrpc", ""));
                if (checkinet)
                {

                    //Grading
                    List<WBTRXGRADING> wBTRXGRADING2Nds = new List<WBTRXGRADING>();
                    wBTRXGRADING2Nds = apiOddoRepo.GetAllDespacthDetailSecondNdByTicket(txtTicketNo.Text);
                    List<ApiQualitiy> QualitiyModel = new List<ApiQualitiy>();
                    List<string> listgrading = new List<string>();
                    ApiQualitiy QualitiyModelRow = new ApiQualitiy();
                    if (wBTRXGRADING2Nds.Count > 0)
                    {
                        for (int i = 0; i < wBTRXGRADING2Nds.Count; i++)
                        {
                            ApiQualitiy apiGrading = new ApiQualitiy();

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
                        QualitiyModelRow.dobi = "false";
                        QualitiyModelRow.ffa = "false";
                        QualitiyModelRow.moisture = "false";
                        QualitiyModelRow.impurities = "false";
                        QualitiyModelRow.no_segel_1 = "false";
                        QualitiyModelRow.no_segel_2 = "false";
                    }
                    QualitiyModel.Add(QualitiyModelRow);
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

                    //string[] plant_weighbridge_line = new string[] { sendblock };
                    //string[] weighbridge_grade_line = new string[] { sendgrading };
                    string[] weighbridge_quality_line = new string[] { sendgrading };
                    //Header
                    List<despactHeader> header = new List<despactHeader>();
                    despactHeader headerrow = new despactHeader();
                    headerrow = apiOddoRepo.GetDespactApiModel(txtTicketNo.Text, weighbridge_quality_line);
                    header.Add(headerrow);

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string headerstring = jss.Serialize(header);

                    parentsub _parentsub = new parentsub();
                    receiptmodel _receiptmodel = new receiptmodel();
                    _parentsub.service = "object";
                    _parentsub.method = methododdo;

                    context _context = new context();
                    _context.unit_code = Session["UnitCode"].ToString();

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
                    //using (var client = new HttpClient())
                    //{
                    HttpClient c = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                    var x = c.PostAsync(url, sc).Result;

                    if (x.IsSuccessStatusCode)
                    {
                        var result = x.Content.ReadAsStringAsync();
                        string bodyresult = result.Result;
                        dynamic data = JObject.Parse(bodyresult);
                        Console.WriteLine(data.result);
                        int msg = data.result;
                        string[] resultarr = new string[] { data.result };
                        List<int> lsresultarr = new List<int>();
                        lsresultarr.Add(msg);
                        string jsonresult = JsonConvert.SerializeObject(lsresultarr);
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

                            StringContent sc2 = new StringContent(jsonend2, Encoding.UTF8, "application/json");
                            var xx = d.PostAsync(url, sc2).Result;
                            if (xx.IsSuccessStatusCode)
                            {
                                var result2 = xx.Content.ReadAsStringAsync();
                                string bodyresult2 = result2.Result;
                                dynamic data2 = JObject.Parse(bodyresult2);
                                Console.WriteLine(data2.result);
                                string msg2 = data2.result.message;
                                bool UpdOddoWb = apiOddoRepo.UpdateStatusOddo(txtTicketNo.Text);
                                MessageSuccess(this, msg2, "");
                            }
                        }
                    }
                    else
                    {
                        var result = x.Content.ReadAsStringAsync();
                        string bodyresult = result.Result;
                        dynamic data = JObject.Parse(bodyresult);
                        Console.WriteLine(data.error.data.message);
                        string msg = data.error.data.message;
                        MessageError(this, msg, "");
                    }
                    c.Dispose();
                    //}
                }
            }
            catch (Exception err)
            {
                MessageError(this, err.Message, "Error");
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            SendToOddoDespact(txtTicketNo.Text);
            MessageSuccess(this, "success", "Success!");
        }

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
                        Console.WriteLine(data.result);
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
                        string bodyresult = result.Result;
                        dynamic data = JObject.Parse(bodyresult);
                        Console.WriteLine(data.error.data.message);
                        string msg = data.error.data.message;

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
    }
}