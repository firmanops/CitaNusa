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
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Weighplatation.View
{
    public partial class DespactEdit : System.Web.UI.Page
    {
        DataSet ds = null;
        public enum MessageType { Success, Error, Info, Warning };
        public DespactRepo _despactRepo = new DespactRepo();
        public ReportRepo reportRepo = new ReportRepo();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ID"] = "0";

            txtTransactionDate.Text = DateTime.Now.ToString("dd-MM-yyy HH:mm:ss");
            Session["TicketNo"] = Request.QueryString["Ticket"];
            Session["NetWeight"] = "";
            Session["Sum1ST"] = "";
            Session["SumDeducation"] = "";
            Session["Sum2ND"] = "";
            if (!IsPostBack)
            {
              
                GetAllWbTrx(Session["TicketNo"].ToString());
                GetAllReceipDetailDespactTicket(Session["TicketNo"].ToString());
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
                    ddlVehicle.Value= despactModelNd.VehicleID;
                    txtUnit.Text = getUnitWBOwner.UnitName; 
                    transpoterModel = _despactRepo.GetTransporter(ddlVehicle.Text);
                    txtTransporter.Text = transpoterModel.BPName;
                    txtContract.Text = despactModelNd.ContractNo;
                    txtDriver.Text = despactModelNd.DriverName;
                    txtLisensiNo.Text = despactModelNd.Lisense;
                    txtWB1st.Text = despactModelNd.Weight1st.ToString();
                    txtWB2nd.Text = despactModelNd.Weight2nd.ToString();
                    ddlDelivery.Text = despactModelNd.DnNo;
                    Session["weight1st"] = despactModelNd.Weight1st;

                    List<DOBPCODEModel> listDo = new List<DOBPCODEModel>();
                    listDo = _despactRepo.GetDllDO(despactModelNd.ContractNo.ToString());
                    ddlDelivery.DataSource = listDo;
                    ddlDelivery.DataBindItems();
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
                List<WBTRXGRADINGEdit> WBGradingEdit = new List<WBTRXGRADINGEdit>();
                WBGrading = _despactRepo.GetAllDespactDetailSecondNdByTicket(TicketNo).Count == 0 ? null : _despactRepo.GetAllDespactDetailSecondNdByTicket(TicketNo);
                if (WBGrading != null)
                {
                    ds = new DataSet();
                    DataTable despacthDetail = new DataTable();
                    despacthDetail.Columns.Add("ID", typeof(double));
                    despacthDetail.Columns.Add("TicketNo", typeof(string));
                    despacthDetail.Columns.Add("GradingTypeID", typeof(string));
                    despacthDetail.Columns.Add("Quantity", typeof(double));
                    despacthDetail.Columns.Add("NoSegel1", typeof(string));
                    despacthDetail.Columns.Add("NoSegel2", typeof(string));
                    despacthDetail.PrimaryKey = new DataColumn[] { despacthDetail.Columns["ID"] };

                    foreach (var item in WBGrading)
                    {
                        WBTRXGRADINGEdit WBGradingEditRow = new WBTRXGRADINGEdit();
                        WBGradingEditRow.ID = item.ID;
                        WBGradingEditRow.TicketNo = item.TicketNo;
                        WBGradingEditRow.GradingTypeID = item.GradingTypeID;
                        WBGradingEditRow.Quantity = item.Quantity;
                        WBGradingEditRow.NoSegel1 = item.NoSegel1;
                        WBGradingEditRow.NoSegel2 = item.NoSegel2;
                        WBGradingEdit.Add(WBGradingEditRow);
                    }

                    ListtoDataTableConverter converter = new ListtoDataTableConverter();
                    despacthDetail = converter.ToDataTable(WBGradingEdit);
                    ds.Tables.AddRange(new DataTable[] { despacthDetail });

                    Session["DataSet"] = ds;


                    griddetail2nd.DataSource = WBGrading;
                    griddetail2nd.DataBind();
                }
                else
                {
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
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };
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
                bool checkinet = apiOddoRepo.CheckForInternetConnection(1000, url.Replace("/jsonrpc", ""));
                if (checkinet)
                {

                    //Grading
                    List<WBTRXGRADING> wBTRXGRADING2Nds = new List<WBTRXGRADING>();
                    wBTRXGRADING2Nds = apiOddoRepo.GetAllDespacthDetailSecondNdByTicket(txtTicketNo.Text);
                    List<ApiQualitiy> QualitiyModel = new List<ApiQualitiy>();
                    List<string> listgrading = new List<string>();
                    ApiQualitiy QualitiyModelRow = new ApiQualitiy();
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

                    string[] args = new string[] { dboddo, "2", "admin", "plant.weighbridge", "create", headerstring.Replace("despact", "despach"), "{" + @"""context"":" + contextstring + "}" };
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
                            string[] args2 = new string[] { dboddo, "2", "admin", "plant.weighbridge", "button_confirm_second", jsonresult, "{}" };
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
        protected void dllDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            WBDOModel wBDO = new WBDOModel();
            wBDO = _despactRepo.GetDONoBPCode(ddlDelivery.Value.ToString());

            List<VehicleDllModel> listVehicle = new List<VehicleDllModel>();
            listVehicle = _despactRepo.GetDllVehicle(wBDO.BPCode.ToString());
            ddlVehicle.DataSource = listVehicle;
            ddlVehicle.DataBindItems();
            GetTransporter(wBDO.BPCode.ToString());
        }
        void GetTransporter(string BPCode)
        {
            txtTransporter.Text = _despactRepo.GetCompanyName(BPCode);
        }
        protected void ddlVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            VehicleDllModel _VehicleDllModel = new VehicleDllModel();
            _VehicleDllModel = _despactRepo.GetDriver(ddlVehicle.Value.ToString());
            txtDriver.Text = _VehicleDllModel.DriverName;
            txtLisensiNo.Text = _VehicleDllModel.LicenseNo;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                double Weight1st = _despactRepo.GetWeight1st(txtTicketNo.Text.Trim());
                if (Weight1st > Math.Ceiling(double.Parse(txtWB2nd.Text)))
                {
                    throw new Exception("The first weigh is greater than the two weigh!!! ");
                }
             

                bool result = false;
                byte[] cam1 = SaveImage();
                WBTRXModel _WBTRXModel = new WBTRXModel();
                List<WBTRXGRADING2nd> _WBTRXGRADING2nd = new List<WBTRXGRADING2nd>();

                string s = DateTime.Now.ToString("HH:mm:ss");
                _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                _WBTRXModel.NoCoverLetter = txtLisensiNo.Text;
                _WBTRXModel.DONo = ddlDelivery.Text;
                _WBTRXModel.Weight1ST = Math.Ceiling(double.Parse(txtWB1st.Text));
                _WBTRXModel.Weight2ND = Math.Ceiling(double.Parse(txtWB2nd.Text));
                _WBTRXModel.DriverName= txtDriver.Text;
                _WBTRXModel.LicenseNo = txtLisensiNo.Text;
                _WBTRXModel.VehicleID = ddlVehicle.Text;
                _WBTRXModel.Status = "P";
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
                Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> resultTuple = _despactRepo.ApproveDespact(_WBTRXModel, _WBTRXGRADING2nd);

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
            //btnSave.Enabled = false;
        }
        void resetObject()
        {
            //btnSave.Enabled = true;
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
            Response.Redirect("/View/Despact.aspx");
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


        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFileWB.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                double Weight1st = _despactRepo.GetWeight1st(txtTicketNo.Text.Trim());
                if (double.Parse(txtWB1st.Text) > Math.Ceiling(double.Parse(txtWB2nd.Text)))
                {
                    throw new Exception("The first weigh is greater than the two weigh!!! ");
                }
                bool result = false;
                byte[] cam1 = SaveImage();
                WBTRXModel _WBTRXModel = new WBTRXModel();
                List<WBTRXGRADING2nd> _WBTRXGRADING2nd = new List<WBTRXGRADING2nd>();

                _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                _WBTRXModel.ContractNo = txtContract.Text;
                _WBTRXModel.VehicleID = ddlVehicle.Text;
                _WBTRXModel.DONo = ddlDelivery.Text;
                _WBTRXModel.DriverName = txtDriver.Text;
                _WBTRXModel.LicenseNo = txtLisensiNo.Text;
                _WBTRXModel.Weight1ST = Math.Ceiling(double.Parse(txtWB1st.Text));
                _WBTRXModel.Weight2ND = Math.Ceiling(double.Parse(txtWB2nd.Text));
                _WBTRXModel.WBStatus = "S";
                _WBTRXModel.Status = "P";
                _WBTRXModel.UserIDApproval = Session["UserID"].ToString();
                _WBTRXModel.Updated = DateTime.Now;

                //Detail
                ds = (DataSet)Session["DataSet"];
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
                }
                else
                {
                    _WBTRXGRADING2nd = null;
                }

                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                NetDespactTotalModel listNetTotal = new NetDespactTotalModel();
                Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> resultTuple = _despactRepo.ApproveDespact(_WBTRXModel, _WBTRXGRADING2nd);

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
                    throw new Exception("Please Input Data Mutu!!!");
                }




            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);

                Console.WriteLine(err.Message);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                double Weight1st = _despactRepo.GetWeight1st(txtTicketNo.Text.Trim());
                if (double.Parse(txtWB1st.Text) > Math.Ceiling(double.Parse(txtWB2nd.Text)))
                {
                    throw new Exception("The first weigh is greater than the two weigh!!! ");
                }
                bool result = false;
                byte[] cam1 = SaveImage();
                WBTRXModel _WBTRXModel = new WBTRXModel();
                List<WBTRXGRADING2nd> _WBTRXGRADING2nd = new List<WBTRXGRADING2nd>();

                _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                _WBTRXModel.ContractNo = txtContract.Text;
                _WBTRXModel.VehicleID = ddlVehicle.Text;
                _WBTRXModel.DONo = ddlDelivery.Text;
                _WBTRXModel.DriverName = txtDriver.Text;
                _WBTRXModel.LicenseNo = txtLisensiNo.Text;
                _WBTRXModel.Weight1ST = Math.Ceiling(double.Parse(txtWB1st.Text));
                _WBTRXModel.Weight2ND = Math.Ceiling(double.Parse(txtWB2nd.Text));
                _WBTRXModel.WBStatus = "D";
                _WBTRXModel.Status = "R";
                _WBTRXModel.UserIDApproval = Session["UserID"].ToString();
                _WBTRXModel.Updated = DateTime.Now;

                //Detail
                ds = (DataSet)Session["DataSet"];
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
                }
                else
                {
                    _WBTRXGRADING2nd = null;
                }

                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                NetDespactTotalModel listNetTotal = new NetDespactTotalModel();
                Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> resultTuple = _despactRepo.ApproveDespact(_WBTRXModel, _WBTRXGRADING2nd);

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
                    throw new Exception("Please Input Data Mutu!!!");
                }




            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);

                Console.WriteLine(err.Message);
            }
        }
    }
}