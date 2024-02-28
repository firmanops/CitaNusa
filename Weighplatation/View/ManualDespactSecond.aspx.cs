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

namespace Weighplatation.View
{
    public partial class ManualDespactSecond : System.Web.UI.Page
    {
        DataSet ds = null;
        public enum MessageType { Success, Error, Info, Warning };
        public DespactRepo _despactRepo = new DespactRepo();
        public ReportRepo reportRepo = new ReportRepo();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ID"] = "0";

            //txtTransactionDate.Text = DateTime.Now.ToString("dd-MM-yyy HH:mm:ss");
            Session["TicketNo"] = Request.QueryString["Ticket"];
            Session["NetWeight"] = "";
            Session["Sum1ST"] = "";
            Session["SumDeducation"] = "";
            Session["Sum2ND"] = "";
            if (!IsPostBack || (Session["DataSet"] == null))
            {
                //Session["UomID"] = "";
                //Session["UomName"] = "";
                ds = new DataSet();
                DataTable receiptDetail = new DataTable();
                receiptDetail.Columns.Add("ID", typeof(double));
                receiptDetail.Columns.Add("TicketNo", typeof(string));
                receiptDetail.Columns.Add("GradingTypeID", typeof(string));
                receiptDetail.Columns.Add("UomID", typeof(string));
                receiptDetail.Columns.Add("UomName", typeof(string));
                receiptDetail.Columns.Add("Quantity", typeof(double));
                receiptDetail.Columns.Add("NoSegel1", typeof(string));
                receiptDetail.Columns.Add("NoSegel2", typeof(string));
                receiptDetail.PrimaryKey = new DataColumn[] { receiptDetail.Columns["ID"] };

                ds.Tables.AddRange(new DataTable[] { receiptDetail });
                Session["DataSet"] = ds;
                //Session["Idx"] = "0";

                btnPrint.Enabled = false;
                GetAllWbTrx(Session["TicketNo"].ToString());

                resetObject();

            }
            else
            {
                //if (Session["Idx"].ToString() == "0")
                //{
                ds = (DataSet)Session["DataSet"];
                griddetail2nd.DataSource = ds.Tables[0];
                griddetail2nd.DataBind();
                //}

            }
        }
        void GetAllWbTrx(string TicketNo)
        {
            try
            {
                DespactModelNd despactModelNd = new DespactModelNd();
                TranspoterModel transpoterModel = new TranspoterModel();
                Session["weight1st"] = "";
                WBOWNER getUnitWBOwner = _despactRepo.GetUnitWBOwner();
                

                despactModelNd = _despactRepo.GetAllDespactByTicket(TicketNo);
                if (despactModelNd != null)
                {
                    txtTicketNo.Text = despactModelNd.TicketNo;
                    txtItem.Text = despactModelNd.ProductName;
                    txtCompanyName.Text = despactModelNd.BPName;
                    txtVehicle.Text = despactModelNd.VehicleID;
                    txtVehicleID.Text = despactModelNd.VehicleID;
                    txtUnit.Text = getUnitWBOwner.UnitName;//despactModelNd.UnitName;
                    transpoterModel = _despactRepo.GetTransporter(txtVehicle.Text);
                    txtTransporter.Text = transpoterModel.BPName;
                    txtContract.Text = despactModelNd.ContractNo;
                    txtDriver.Text = despactModelNd.DriverName;
                    txtLisensiNo.Text = despactModelNd.Lisense;
                    txtDelivery.Text = despactModelNd.BPName;
                    txtDnNo.Text = despactModelNd.DnNo;
                    Session["weight1st"] = despactModelNd.Weight1st;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                double Weight1st = _despactRepo.GetWeight1st(txtTicketNo.Text.Trim());
                if (Weight1st > Math.Ceiling(double.Parse(txtWB.Text)))
                {
                    throw new Exception("The first weigh is greater than the two weigh!!! ");
                }

                if (txtTransactionDate.Text == "")
                {
                    throw new Exception("Transaction Date is required !!!");
                }

                bool result = false;
                byte[] cam1 = SaveImage();
                WBTRXModel _WBTRXModel = new WBTRXModel();
                List<WBTRXGRADING2nd> _WBTRXGRADING2nd = new List<WBTRXGRADING2nd>();

                string s = DateTime.Now.ToString("HH:mm:ss");

                _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                _WBTRXModel.TrxDateOut = Convert.ToDateTime(txtTransactionDate.Text + " " + s);
                _WBTRXModel.ContractNo = txtContract.Text.Trim();
                _WBTRXModel.NoCoverLetter = "";
                _WBTRXModel.VehicleID = txtVehicleID.Text;
                _WBTRXModel.DONo = txtDnNo.Text;
                _WBTRXModel.Weight2ND = Math.Ceiling(double.Parse(txtWB.Text));
                _WBTRXModel.WBStatus = "S";
                _WBTRXModel.WBFlag2 = "M";
                _WBTRXModel.WBImagefront2 = cam1;
                _WBTRXModel.UserIDWeight2ND = Session["UserID"].ToString();
                _WBTRXModel.UserIDApproval = "";
                _WBTRXModel.Updated = DateTime.Now;

                //Detail
                ds = (DataSet)Session["DataSet"];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        WBTRXGRADING2nd entity = new WBTRXGRADING2nd();
                        entity.TicketNo = txtTicketNo.Text;
                        //string[] arrGrading = item[2].ToString().Split('-');
                        entity.GradingTypeId = item[2].ToString();
                        entity.Quantity = double.Parse(item[5].ToString());
                        entity.NoSegel1 = item[6].ToString();
                        entity.NoSegel2 = item[7].ToString();
                        _WBTRXGRADING2nd.Add(entity);
                    }
                }
                else
                {
                    _WBTRXGRADING2nd = null;
                    //throw new Exception("Please Input Data Mutu!!!");
                }

                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                    NetDespactTotalModel listNetTotal = new NetDespactTotalModel();
                    Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> resultTuple = _despactRepo.UpdateDespact(_WBTRXModel, _WBTRXGRADING2nd);

                    result = resultTuple.Item1;
                    listNetWeight = resultTuple.Item2;
                    listNetTotal = resultTuple.Item3;

                    griddetailnet.DataSource = listNetWeight;
                    griddetailnet.DataBind();

                    btnPrint.Enabled = true;
                    Session["NetWeight"] = String.Format("{0,15:#,##0 ;(#,##0);-   }", double.Parse(listNetWeight[1].WeightHeavy.ToString()) - double.Parse(listNetWeight[0].WeightHeavy.ToString()));

                    if (result)
                    {
                        //SendToOddo();
                        MessageSuccess(this, "success", "Success!");
                        refreshObject();

                    }
                    else
                    {
                        throw new Exception("Faild Record");
                    }
               




            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);

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

        //protected void btnWB_Click(object sender, EventArgs e)
        //{
        //    txtWB.Text = apiOddoRepo.Getscale();
        //    //GetScaleNumber();
        //}

        void refreshObject()
        {
            btnSave.Enabled = false;
        }
        void resetObject()
        {
            btnSave.Enabled = true;
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["TicketNo"] = "";
            Response.Redirect("ManualDespact.aspx");
        }

        protected void griddetail2nd_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            griddetail2nd.JSProperties["cpPopulate"] = e.Parameters;

            string[] arrGrading = e.Parameters.Split('-');
            string GUomID = _despactRepo.GetGradingTypeByID(arrGrading[0].ToString()).UomID.ToString();
            Session["GradingTypeID"] = e.Parameters;
            //Session["UomID"] = _despactRepo.GetUOM(GUomID).UomID.ToString();
            //Session["UomName"] = _despactRepo.GetUOM(GUomID).UomName.ToString();
            Session["Idx"] = "0";

            //GridViewDataColumn col = ((ASPxGridView)sender).Columns["UnitPrice"] as GridViewDataColumn;
            //ASPxTextBox txtUOMName = ((ASPxGridView)sender).FindEditRowCellTemplateControl(col, "UomName") as ASPxTextBox;
            //ASPxTextBox txtUOMID = ((ASPxGridView)sender).FindEditRowCellTemplateControl(col, "UomID") as ASPxTextBox;

            //txtUOMID.Text = _despactRepo.GetUOM(GUomID).UomID.ToString();
            //txtUOMName.Text = _despactRepo.GetUOM(GUomID).UomName.ToString();

        }

        protected void griddetail2nd_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            double p_ID = _despactRepo.GetIDGrading();
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
            int ID = ds.Tables[0].Rows.Count + 1;
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            DataRow row = dataTable.NewRow();
            e.NewValues["TicketNo"] = txtTicketNo.Text;
            e.NewValues["ID"] = maxID + 1; ;
            //e.NewValues["UomID"] = Session["UomID"].ToString();
            //e.NewValues["UomName"] = Session["UomName"].ToString();
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                if (enumerator.Key.ToString() != "Count")
                    row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            dataTable.Rows.Add(row);



            ds = (DataSet)Session["DataSet"];

            griddetail2nd.DataSource = ds.Tables[0];
            griddetail2nd.DataBind();
            Session["Idx"] = "0";
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
        protected void griddetail2nd_CellEditorInitialize(object sender, BootstrapGridViewEditorEventArgs e)
        {
            Session["Idx"] = "1";
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
                                QualitiyModelRow.dobi = wBTRXGRADING2Nds[i].Quantity.ToString();
                            }
                            if (wBTRXGRADING2Nds[i].GradingName.ToString() == "FFA")
                            {
                                QualitiyModelRow.ffa = wBTRXGRADING2Nds[i].Quantity.ToString();
                            }
                            if (wBTRXGRADING2Nds[i].GradingName.ToString() == "MOIST")
                            {
                                QualitiyModelRow.moisture = wBTRXGRADING2Nds[i].Quantity.ToString();
                            }
                            if (wBTRXGRADING2Nds[i].GradingName.ToString() == "IMPURITIES")
                            {
                                QualitiyModelRow.impurities = wBTRXGRADING2Nds[i].Quantity.ToString();
                            }

                            QualitiyModelRow.no_segel_1 = wBTRXGRADING2Nds[i].NoSegel1;
                            QualitiyModelRow.no_segel_2 = wBTRXGRADING2Nds[i].NoSegel2;


                        }
                    }
                    else {                        
                        QualitiyModelRow.dobi = "";
                        QualitiyModelRow.ffa = "";
                        QualitiyModelRow.moisture = "";
                        QualitiyModelRow.impurities = "";
                        QualitiyModelRow.no_segel_1 = "";
                        QualitiyModelRow.no_segel_2 = "";
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

        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileWB.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }
        private void GetScaleNumber()
        {
            try
            {
                //string apiUrl = "https://localhost:44382/api/Apiwb";
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
    }
}