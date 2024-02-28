using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Weighplatation.Model;
using Weighplatation.Repository;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;


namespace Weighplatation.View
{
    public partial class OtherReceiptView : System.Web.UI.Page
    {
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        public ReportRepo reportRepo = new ReportRepo();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["TicketNo"] = Request.QueryString["Ticket"];
            Session["Sum1ST"] = "";
            Session["SumDeducation"] = "";
            Session["Sum2ND"] = "";
            Session["Potongan"] = "";
            Session["CutWeigh="] = 0;
            //if (Session["GroupID"].ToString() != "2")
            //{
            //    btnSend.Visible = false;
            //}
            if (!IsPostBack)
            {

                GetAllWbTrx(Session["TicketNo"].ToString());
                GetAllReceipDetailRecdeiptTicket(Session["TicketNo"].ToString());
            }
            else
            {
                Session["Sum1ST"] = "";
                Session["SumDeducation"] = "";
                Session["Sum2ND"] = "";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Receipt.aspx");
        }

        void GetAllWbTrx(string TicketNo)
        {
            try
            {
                ReceiptModelNd receiptModelNd = new ReceiptModelNd();
                TranspoterModel transpoterModel = new TranspoterModel();
                Session["weight1st"] = "";
                Session["weight2nd"] = "";

                receiptModelNd = _receiptRepo.GetAllOtherReceiptByTicket(TicketNo);
                if (receiptModelNd != null)
                {
                    txtTicketNo.Text = receiptModelNd.TicketNo;
                    txtTransactionDate.Text = receiptModelNd.Created.ToString();
                    txtLetter.Text = receiptModelNd.LetterNo;
                    txtItem.Text = receiptModelNd.ProductName;
                    txtCompanyName.Text = receiptModelNd.BPName;
                    txtVehicle.Text = receiptModelNd.VehicleID;
                    txtUnit.Text = receiptModelNd.UnitName;
                    transpoterModel = _receiptRepo.GetTransporter(txtVehicle.Text);
                    txtTransporter.Text = transpoterModel.BPName;
                    //txtContract.Text = receiptModelNd.ContractNo;
                    txtDriver.Text = receiptModelNd.DriverName;
                    txtLisensiNo.Text = receiptModelNd.Lisense;
                    txtWB1.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", double.Parse(receiptModelNd.Weight1st.ToString()));
                    txtWB2.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", double.Parse(receiptModelNd.Weight2nd.ToString()));
                    Session["weight1st"] = receiptModelNd.Weight1st;
                    Session["weight2nd"] = receiptModelNd.Weight2nd;
                    img1.ContentBytes = receiptModelNd.WBImagefront1;
                    img2.ContentBytes = receiptModelNd.WBImagefront2;
                    Session["CutWeigh"] = receiptModelNd.Potongan;
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

        void GetAllReceipDetailRecdeiptTicket(string TicketNo)
        {
            try
            {
                List<WBTRXBLOCK1st> wBTRXBLOCK = new List<WBTRXBLOCK1st>();
                wBTRXBLOCK = _receiptRepo.GetAllReceipDetailOneSttByTicket(TicketNo);
                if (wBTRXBLOCK != null)
                {
                    //wBTRXBLOCK[0].Weight = double.Parse(Session["weight1st"].ToString());
                    griddetail1st.DataSource = wBTRXBLOCK;
                    griddetail1st.DataBind();
                }
                else
                {
                    throw new Exception("Data not found please another Ticket");
                }

                List<WBTRXGRADING> WBGrading = new List<WBTRXGRADING>();
                WBGrading = _receiptRepo.GetAllReceipDetailSecondNdByTicket(TicketNo).Count == 0 ? null : _receiptRepo.GetAllReceipDetailSecondNdByTicket(TicketNo);
                if (WBGrading != null)
                {
                    griddetail2nd.DataSource = WBGrading;
                    griddetail2nd.DataBind();
                }
                //else
                //{
                //    throw new Exception("Data not found please another Ticket");
                //}

                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                NetReceiptTotalModel listNetTotal = new NetReceiptTotalModel();
                Tuple<List<NetWeightModel>, NetReceiptTotalModel> resultTuple = _receiptRepo.GetOtherNetWeight(TicketNo);

                listNetWeight = resultTuple.Item1;
                listNetTotal = resultTuple.Item2;
                griddetailnet.DataSource = listNetWeight;
                griddetailnet.DataBind();

                double deduc = Math.Ceiling(_receiptRepo.GetDeducation(txtTicketNo.Text.Trim()));
                double stnet1 = Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString());
                Session["Sum1ST"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()));
                double cutpresent = double.Parse(Session["CutWeigh"].ToString()) / 100;
                double potongan = stnet1 * cutpresent;
                double tRotasi = stnet1 * deduc;
                Session["Potongan"] = potongan;
                Session["SumDeducation"] = String.Format("{0,15:#,##0 ;(#,##0);0  }", tRotasi.ToString());
                Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()));
               // Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - listNetTotal.Quantity - potongan);


                //double deduc = Math.Ceiling(_receiptRepo.GetDeducation(txtTicketNo.Text.Trim()));
                //Session["Sum1ST"] = String.Format("{0,15:#,##0 ;(#,##0);-   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()));
                //Session["SumDeducation"] = String.Format("{0,15:#,##0 ;(#,##0);-   }", deduc.ToString());
                //Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);-   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - listNetTotal.Quantity);


            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);
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

        const string BITMAP_ID_BLOCK = "BM";
        const string JPG_ID_BLOCK = "\u00FF\u00D8\u00FF";
        const string PNG_ID_BLOCK = "\u0089PNG\r\n\u001a\n";
        const string GIF_ID_BLOCK = "GIF8";
        const string TIFF_ID_BLOCK = "II*\u0000";
        const int DEFAULT_OLEHEADERSIZE = 78;

        public static byte[] ConvertOleObjectToByteArray(object content)
        {
            if (content != null && !(content is DBNull))
            {
                byte[] oleFieldBytes = (byte[])content;
                byte[] imageBytes = null;
                // Get a UTF7 Encoded string version
                Encoding u8 = Encoding.UTF7;
                string strTemp = u8.GetString(oleFieldBytes);
                // Get the first 300 characters from the string
                string strVTemp = strTemp.Substring(0, 300);
                // Search for the block
                int iPos = -1;
                if (strVTemp.IndexOf(BITMAP_ID_BLOCK) != -1)
                {
                    iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK);
                }
                else if (strVTemp.IndexOf(JPG_ID_BLOCK) != -1)
                {
                    iPos = strVTemp.IndexOf(JPG_ID_BLOCK);
                }
                else if (strVTemp.IndexOf(PNG_ID_BLOCK) != -1)
                {
                    iPos = strVTemp.IndexOf(PNG_ID_BLOCK);
                }
                else if (strVTemp.IndexOf(GIF_ID_BLOCK) != -1)
                {
                    iPos = strVTemp.IndexOf(GIF_ID_BLOCK);
                }
                else if (strVTemp.IndexOf(TIFF_ID_BLOCK) != -1)
                {
                    iPos = strVTemp.IndexOf(TIFF_ID_BLOCK);
                }
                // From the position above get the new image
                if (iPos == -1)
                {
                    iPos = DEFAULT_OLEHEADERSIZE;
                }
                //Array.Copy(
                imageBytes = new byte[oleFieldBytes.LongLength - iPos];
                MemoryStream ms = new MemoryStream();
                ms.Write(oleFieldBytes, iPos, oleFieldBytes.Length - iPos);
                imageBytes = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return imageBytes;
            }
            return null;
        }

        protected void btnPrints_Click(object sender, EventArgs e)
        {
            bool result = _receiptRepo.CountingPrint(txtTicketNo.Text);
            if (result)
            {
                Console.WriteLine("Success");
            }

            List<ReceiptModelNd> receiptModelNd = new List<ReceiptModelNd>();
            List<WBTRXBLOCK1st> wBTRXBLOCK1Sts = new List<WBTRXBLOCK1st>();
            List<WBTRXGRADING2nd> wBTRXGRADING2Nds = new List<WBTRXGRADING2nd>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();
            List<NetWeightModel> netWeightModels = new List<NetWeightModel>();

            receiptModelNd = reportRepo.GetFormOtherReceiptHeader(txtTicketNo.Text);
            wBTRXBLOCK1Sts = reportRepo.GetFromReceiptBlock(txtTicketNo.Text);

            wBTRXGRADING2Nds = reportRepo.GetFromReceiptGrading(txtTicketNo.Text).Count == 0 ? null : reportRepo.GetFromReceiptGrading(txtTicketNo.Text);
            Companyls.Add(_receiptRepo.GetUnitByCode(Session["UnitCode"].ToString()));
            netWeightModels = reportRepo.GetOtherNetWeightModel(txtTicketNo.Text);
            double QtyDeducation = _receiptRepo.GetDeducation(txtTicketNo.Text);


            string Ext = "pdf";
            string Filename = "TicketReceipt";
            HttpContext.Current.Items["Ext"] = Ext;
            HttpContext.Current.Items["Filename"] = Filename;
            HttpContext.Current.Items["Ticketls"] = receiptModelNd;
            HttpContext.Current.Items["Blockls"] = wBTRXBLOCK1Sts;
            HttpContext.Current.Items["Gradingls"] = wBTRXGRADING2Nds;
            HttpContext.Current.Items["Companyls"] = Companyls;
            HttpContext.Current.Items["NetWeightls"] = netWeightModels;
            HttpContext.Current.Items["QtyDeducation"] = QtyDeducation;
            HttpContext.Current.Items["User"] = Session["UserName"];
            HttpContext.Current.Items["Type"] = "Automatic";
            Server.Transfer("/Report/FornTicketOtherReceipt.aspx");
        }

        //protected void btnSend_Click(object sender, EventArgs e)
        //{
        //    SendToOddo();
        //}

        void SendToOddo()
        {
            try
            {
                string url = ConfigurationManager.AppSettings["endpointoddo"].ToString();
                string dboddo = ConfigurationManager.AppSettings["dboddo"].ToString();
                string methododdo = ConfigurationManager.AppSettings["methododdo"].ToString();
                string unitwb = Session["UnitCode"].ToString();


                WriteLogFile($"Body Jason To Oddo 0 ");

                WriteLogFile($"Status : Mulai Looping");

                bool checkinet = apiOddoRepo.CheckForInternetConnection(2000, url.Replace("/jsonrpc", ""));
                if (checkinet)
                {

                    WriteLogFile($"Proses Block : {txtTicketNo.Text} ");
                    //string txtTicketNo.Text = txtTicketNo.Text;
                    //Block
                    List<WBTRXBLOCK1st> wBTRXBLOCK1St = new List<WBTRXBLOCK1st>();
                    wBTRXBLOCK1St = apiOddoRepo.GetAllReceipDetailOneSttByTicket(txtTicketNo.Text);

                    List<string> listblock = new List<string>();
                    for (int i = 0; i < wBTRXBLOCK1St.Count; i++)
                    {
                        ApiBlock apiBlock = new ApiBlock();
                        WriteLogFile($"Status : {txtTicketNo.Text}");
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
                    WriteLogFile($"Proses Grading : {txtTicketNo.Text} ");
                    List<WBTRXGRADING> wBTRXGRADING2Nds = new List<WBTRXGRADING>();
                    wBTRXGRADING2Nds = apiOddoRepo.GetAllReceipDetailSecondNdByTicket(txtTicketNo.Text);
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

                    WriteLogFile($"Proses Header : {txtTicketNo.Text} ");
                    //Header
                    List<receiptHeader> header = new List<receiptHeader>();
                    receiptHeader headerrow = new receiptHeader();
                    headerrow = apiOddoRepo.GetReceiptApiModel(txtTicketNo.Text, plant_weighbridge_line, weighbridge_grade_line);
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

                    string[] args = new string[] { dboddo, "2", "admin", "plant.weighbridge", "create", headerstring, "{" + @"""context"":" + contextstring + "}" };
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
                    WriteLogFile($"Proses Send : {txtTicketNo.Text} ");
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
                            string[] args2 = new string[] { dboddo, "2", "admin", "plant.weighbridge", "button_confirm_second", jsonresult, "{}" };
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
                                bool UpdOddoWb = apiOddoRepo.UpdateStatusOddo(txtTicketNo.Text);
                                WriteLogFile($"Update Status Ticket = {UpdOddoWb}");
                                if (UpdOddoWb)
                                {
                                    WriteLogFile($"Status Send Oddo 2nd = {"Success!!!"}");
                                }
                                else
                                {
                                    WriteLogFile($"Update Update WB = {"Failed!!!"}");
                                }
                                var result2 = xx.Content.ReadAsStringAsync();
                                string bodyresult2 = result2.Result;

                                WriteLogFile($"Response method button submit = {bodyresult2}");
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



                }




            }
            catch (Exception err)
            {
                WriteLogFile($"Body Jason To Oddo 4 = {err.Message.ToString()}");
            }
        }


        //protected void btnSend_Click1(object sender, EventArgs e)
        //{
        //    SendToOddo();
        //}
        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileWB.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }
    }
}
