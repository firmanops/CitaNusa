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
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO.Ports;

namespace Weighplatation.View
{
    public partial class OtherReceiptSecond : System.Web.UI.Page
    {
        DataSet ds = null;
        public enum MessageType { Success, Error, Info, Warning };
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        public PortRepo _portRepo = new PortRepo();
        public ReportRepo reportRepo = new ReportRepo();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ID"] = "0";
            txtTransactionDate.Text = DateTime.Now.ToString("dd-MM-yyy HH:mm:ss");
            Session["TicketNo"] = Request.QueryString["Ticket"];
            Session["Sum1ST"] = "";
            Session["SumDeducation"] = "";
            Session["Sum2ND"] = "";
            Session["Potongan="] = "0";
            Session["Unit="] = "";

            if (!IsPostBack)
            {
                txtTransactionDate.Text = DateTime.Now.ToString("dd-MM-yyy HH:mm:ss");
                ds = new DataSet();
                DataTable receiptDetail = new DataTable();
                receiptDetail.Columns.Add("ID", typeof(double));
                receiptDetail.Columns.Add("TicketNo", typeof(string));
                receiptDetail.Columns.Add("GradingTypeID", typeof(string));
                receiptDetail.Columns.Add("Quantity", typeof(double));
                receiptDetail.Columns.Add("NoSegel1", typeof(string));
                receiptDetail.Columns.Add("NoSegel2", typeof(string));
                receiptDetail.PrimaryKey = new DataColumn[] { receiptDetail.Columns["ID"] };

                ds.Tables.AddRange(new DataTable[] { receiptDetail });
                Session["DataSet"] = ds;

                GetAllWbTrx(Session["TicketNo"].ToString());
                GetAllReceipDetailOneSttByTicket(Session["TicketNo"].ToString());
                btnPrint.Enabled = false;
            }
            else
            {
                ds = (DataSet)Session["DataSet"];
                griddetail2nd.DataSource = ds.Tables[0];
                griddetail2nd.DataBind();
                //resetObject();
            }
        }

        protected void griddetail2nd_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {

                double p_ID = _receiptRepo.GetIDGrading();
                int maxID = 0;
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int accountLevel = int.Parse(dr.ItemArray[0].ToString());
                        maxID = Math.Max(maxID, accountLevel);
                    }
                }
                else
                {
                    maxID = int.Parse(p_ID.ToString()) + 1;
                }


                ds = (DataSet)Session["DataSet"];
                Session["ID"] = p_ID + ds.Tables[0].Rows.Count + 1;
                ASPxGridView gridView = (ASPxGridView)sender;
                DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
                DataRow row = dataTable.NewRow();
                e.NewValues["ID"] = maxID + 1;
                e.NewValues["TicketNo"] = txtTicketNo.Text;
                IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                    if (enumerator.Key.ToString() != "Count")
                        row[enumerator.Key.ToString()] = enumerator.Value;
                gridView.CancelEdit();
                e.Cancel = true;
                dataTable.Rows.Add(row);
            }
            catch (Exception err)
            {

                MessageError(this, err.Message, "Error");
            };

        }

        protected void griddetail2nd_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Session["ID"] = "";
            int i = griddetail2nd.FindVisibleIndexByKeyValue(e.Keys[griddetail2nd.KeyFieldName]);
            Control c = griddetail2nd.FindDetailRowTemplateControl(i, "griddetail2nd");
            e.Cancel = true;
            ds = (DataSet)Session["DataSet"];
            ds.Tables[0].Rows.Remove(ds.Tables[0].Rows.Find(e.Keys[griddetail2nd.KeyFieldName]));
            griddetail2nd.DataSource = ds.Tables[0];
            griddetail2nd.DataBind();
        }

        protected void griddetail2nd_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ds = (DataSet)Session["DataSet"];
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            DataRow row = dataTable.Rows.Find(e.Keys[0]);
            //string x = Session["BlockID"].ToString();
            //e.NewValues["ID"] = Session["ID"];
            e.NewValues["TicketNo"] = txtTicketNo.Text;
            //e.NewValues["Yop"] = _receiptRepo.GetYOP(Session["BlockID"].ToString());
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            griddetail2nd.DataSource = ds.Tables[0];
            griddetail2nd.DataBind();
        }

        protected void wbGradingType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                double Weight1st = _receiptRepo.GetOtherWeight1st(txtTicketNo.Text.Trim());
                if (Weight1st < Math.Ceiling(double.Parse(txtWB.Text)))
                {
                    throw new Exception("The first weigh is smaller than the two weigh!!! ");
                }

                bool result = false;
                byte[] cam1 = SaveImage();

                WBTRXETCModel _WBTRXModel = new WBTRXETCModel();
                List<WBTRXGRADING2nd> _WBTRXGRADING2nd = new List<WBTRXGRADING2nd>();

                _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                _WBTRXModel.TrxDateOut = DateTime.Now;
                //_WBTRXModel.ContractNo = txtContract.Text.Trim();
                _WBTRXModel.VehicleID = txtVehicleID.Text;
                _WBTRXModel.NoCoverLetter = "";
                //_WBTRXModel.DONo = "";
                _WBTRXModel.Weight2ND = Math.Ceiling(double.Parse(txtWB.Text));
                _WBTRXModel.WBFlag2 = "A";
                _WBTRXModel.WBStatus = "D";
                _WBTRXModel.WBImagefront2 = cam1;
                _WBTRXModel.UserIDWeight2ND = Session["UserID"].ToString();
                _WBTRXModel.UserIDApproval = "";
                _WBTRXModel.Updated = DateTime.Now;


                ds = (DataSet)Session["DataSet"];
                double deduc = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        WBTRXGRADING2nd _WBTRXGRADINGDetail = new WBTRXGRADING2nd();
                        _WBTRXGRADINGDetail.TicketNo = txtTicketNo.Text;
                        _WBTRXGRADINGDetail.GradingTypeId = item.ItemArray[2].ToString();//ds.Tables[0].Rows[0]["GradingTypeId"].ToString();                       
                        _WBTRXGRADINGDetail.Quantity = double.Parse(item.ItemArray[3].ToString());//double.Parse(ds.Tables[0].Rows[0]["Quantity"].ToString());
                        _WBTRXGRADINGDetail.NoSegel1 = item.ItemArray[4].ToString();//ds.Tables[0].Rows[0]["NoSegel1"].ToString();
                        _WBTRXGRADINGDetail.NoSegel2 = item.ItemArray[5].ToString();//ds.Tables[0].Rows[0]["NoSegel2"].ToString();
                        _WBTRXGRADING2nd.Add(_WBTRXGRADINGDetail);

                    }

                    deduc = _receiptRepo.GetDeducation(txtTicketNo.Text.Trim());
                }
                else
                {
                    _WBTRXGRADING2nd = null;
                }

                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                NetReceiptTotalModel listNetTotal = new NetReceiptTotalModel();
                Tuple<bool, List<NetWeightModel>, NetReceiptTotalModel> resultTuple = _receiptRepo.UpdateOtherReceipt(_WBTRXModel, _WBTRXGRADING2nd);

                result = resultTuple.Item1;
                listNetWeight = resultTuple.Item2;
                listNetTotal = resultTuple.Item3;

                deduc = _receiptRepo.GetDeducation(txtTicketNo.Text.Trim());

                if (result)
                {
                    griddetailnet.DataSource = listNetWeight;
                    griddetailnet.DataBind();
                    double cutpresent = double.Parse(Session["CutWeigh"].ToString()) / 100;
                    double stnet1 = Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString());
                    Session["Sum1ST"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()));
                    double potongan = stnet1 * cutpresent;
                    double tRotasi = stnet1 * deduc;
                    Session["Potongan"] = potongan;
                    Session["SumDeducation"] = String.Format("{0,15:#,##0 ;(#,##0);0  }", tRotasi.ToString());
                    Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()));
                    //Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - listNetTotal.Quantity - potongan);

                    //SendToOddo();
                    MessageSuccess(this, "success", "Success!");
                    refreshObject();
                    btnPrint.Enabled = true;

                }
                else
                {
                    throw new Exception("Faild Record");
                }
                //}
                //else 
                //{
                //    throw new Exception("Please Input Data Mutu!!!");
                //}


            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);

                Console.WriteLine(err.Message);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("OtherReceipts.aspx");
        }
        protected void btnPrint_Click(object sender, EventArgs e)
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

        void GetAllWbTrx(string TicketNo)
        {
            try
            {
                ReceiptModelNd receiptModelNd = new ReceiptModelNd();
                TranspoterModel transpoterModel = new TranspoterModel();
                Session["weight1st"] = "";


                receiptModelNd = _receiptRepo.GetAllOtherReceiptByTicket(TicketNo);
                if (receiptModelNd != null)
                {
                    txtTicketNo.Text = receiptModelNd.TicketNo;
                    txtItem.Text = receiptModelNd.ProductName;
                    txtLetter.Text = receiptModelNd.LetterNo;
                    txtCompanyName.Text = receiptModelNd.BPName;
                    txtVehicleID.Text = receiptModelNd.VehicleID;
                    txtVehicle.Text = receiptModelNd.VehicleID;
                    Session["Unit="] = receiptModelNd.UnitCode;
                    txtUnit.Text = receiptModelNd.UnitName;
                    transpoterModel = _receiptRepo.GetTransporter(txtVehicle.Text);
                    txtTransporter.Text = transpoterModel.BPName;
                   // txtContract.Text = receiptModelNd.ContractNo;
                    txtDriver.Text = receiptModelNd.DriverName;
                    txtLisensiNo.Text = receiptModelNd.Lisense;
                    Session["weight1st"] = receiptModelNd.Weight1st;
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

        void GetAllReceipDetailOneSttByTicket(string TicketNo)
        {
            try
            {
                List<WBTRXBLOCK1st> wBTRXBLOCK = new List<WBTRXBLOCK1st>();
                wBTRXBLOCK = _receiptRepo.GetAllReceipDetailOneSttByTicket(TicketNo);
                if (wBTRXBLOCK != null)
                {
                    wBTRXBLOCK[0].Weight = double.Parse(Session["weight1st"].ToString());
                    griddetail1st.DataSource = wBTRXBLOCK;
                    griddetail1st.DataBind();
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
        void refreshObject()
        {
            btnSave.Enabled = false;
        }
        void resetObject()
        {
            btnSave.Enabled = true;
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

        protected void btnWB_Click(object sender, EventArgs e)
        {
            try
            {

                txtWB.Text = apiOddoRepo.Getscale(); // Dekstop

                ////txtWB.Text = apiOddoRepo.Getscale();
                ////GetScaleNumber();
                //btnWB.Text = "Process Weigh ...";

                //Console.WriteLine("");
                //PortModel _portModel = new PortModel();

                //_portModel = _portRepo.GetPort(Session["WBSource"].ToString());

                //SerialPort mySerialPort = new SerialPort("COM" + _portModel.ComPort);

                //mySerialPort.BaudRate = int.Parse(_portModel.Bautrate.ToString());
                //mySerialPort.Parity = Parity.None;
                //mySerialPort.StopBits = StopBits.One;
                //mySerialPort.DataBits = int.Parse(_portModel.DataBits.ToString());
                //mySerialPort.Handshake = Handshake.None;
                //mySerialPort.Open();
                //mySerialPort.ReadTimeout = 100000;
                ////txtWB.Text = mySerialPort.ReadLine().ToString();
                //txtWB.Text = mySerialPort.ReadExisting();

                ////string test = "ST,NT,+000000.Kg";
                ////txtWB.Text = test.ToString().Substring(7, 6);

                //mySerialPort.Close();

                //btnWB.Text = "Process WB";

                //btnWB.Text = "Process WB";
            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);
            }
        }

        [WebMethod(EnableSession = true)]
        public static bool SaveCapturedImage(string data)
        {
            string fileName = "front";

            //Convert Base64 Encoded string to Byte Array.
            byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);

            //Save the Byte Array as Image File.
            string filePath = HttpContext.Current.Server.MapPath(string.Format("~/Captures/{0}.png", fileName));
            File.WriteAllBytes(filePath, imageBytes);
            return true;
        }
        public string GetBase64String(string PathToFile)
        {
            try
            {
                byte[] binData = File.ReadAllBytes(PathToFile);
                return Convert.ToBase64String(binData);
            }
            catch
            {
                return "";
            }

        }
        public byte[] GetFromBase64String(string base64string)
        {
            try
            {
                return Convert.FromBase64String(base64string);
            }
            catch
            {
                return (byte[])null;
            }
        }
        public string GetFilePath()
        {
            return HttpContext.Current.Server.MapPath("/Captures/front.png");
        }

        byte[] SaveImage()
        {
            try
            {
                //if (Request.InputStream.Length > 0)
                //{
                string filePath = GetFilePath();

                string imgcapture = GetBase64String(filePath);

                byte[] toArrBytes = GetFromBase64String(imgcapture);

                if (File.Exists(Path.Combine(filePath)))
                {
                    // If file found, delete it    
                    File.Delete(Path.Combine(filePath));
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");

                //Untuk Test Hasil Convert
                var json = JsonConvert.SerializeObject(toArrBytes);
                return toArrBytes;
                //}
            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);
                return null;
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

                                var result2 = xx.Content.ReadAsStringAsync();
                                string bodyresult2 = result2.Result;
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

        private void GetScaleNumber()
        {
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["endpointwb"].ToString();
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(apiUrl + "/GetNumberScale").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    string bodyresult = result.Result;

                    txtWB.Text = Math.Ceiling(double.Parse(bodyresult.Replace(@"""", "").Replace(@"kg\r\n", "").Trim().ToString())).ToString("###,###,###");

                }
            }
            catch (Exception)
            {

                MessageError(this, "Get Scale Failed...!!!", "Error Get Scale");
            }

        }

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