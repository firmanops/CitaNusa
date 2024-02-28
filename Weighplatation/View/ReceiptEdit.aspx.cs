using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO.Ports;
using Newtonsoft.Json;
using static DevExpress.Xpo.Helpers.CommandChannelHelper;
using System.Net.Http;
using System.Configuration;
using System.Web;

namespace Weighplatation.View
{
    public partial class ReceiptEdit : System.Web.UI.Page
    {
        DataSet ds = null;
        DataSet ds2 = null;
        public enum MessageType { Success, Error, Info, Warning };
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        public PortRepo _portRepo = new PortRepo();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        public ReportRepo reportRepo = new ReportRepo();
        protected void Page_Load(object sender, EventArgs e)
        {

        
                Session["TicketNo"] = Request.QueryString["Ticket"];
                Session["Sum1ST"] = "";
                Session["SumDeducation"] = "";
                Session["Sum2ND"] = "";
                Session["Potongan"] = "";
                Session["CutWeigh="] = 0;
               
                if (!IsPostBack)
                {
                    GetAllWbTrx(Session["TicketNo"].ToString());
                    GetAllReceipDetailRecdeiptTicket(Session["TicketNo"].ToString());                   
              
                }
               
        }
        void GetAllWbTrx(string TicketNo)
        {
            try
            {
                ReceiptModelNd receiptModelNd = new ReceiptModelNd();
                TranspoterModel transpoterModel = new TranspoterModel();
                Session["weight1st"] = "";
                Session["weight2nd"] = "";

                receiptModelNd = _receiptRepo.GetAllReceiptByTicket(TicketNo);
                if (receiptModelNd != null)
                {
                    txtTicketNo.Text = receiptModelNd.TicketNo;
                    txtTransactionDate.Text = receiptModelNd.Created.ToString();
                    txtLetter.Text = receiptModelNd.LetterNo;
                    txtProductName.Text = receiptModelNd.ProductName;
                    txtCompanyName.Text = receiptModelNd.BPName;
                    ddlVehicle.Text = receiptModelNd.VehicleID;
                    ddlUnit.Text = receiptModelNd.UnitName;
                    transpoterModel = _receiptRepo.GetTransporter(ddlVehicle.Text);
                    txtTransporter.Text = transpoterModel.BPName;
                    ddlContract.Text = receiptModelNd.ContractNo;
                    txtDriver.Text = receiptModelNd.DriverName;
                    txtLisensiNo.Text = receiptModelNd.Lisense;
                    txtWB1st.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", double.Parse(receiptModelNd.Weight1st.ToString()));
                    txtWB2nd.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", double.Parse(receiptModelNd.Weight2nd.ToString()));
                    Session["weight1st"] = receiptModelNd.Weight1st;
                    Session["weight2nd"] = receiptModelNd.Weight2nd;
                    img1.ContentBytes = receiptModelNd.WBImagefront1;
                    img2.ContentBytes = receiptModelNd.WBImagefront2;
                    Session["CutWeigh"] = receiptModelNd.Potongan;

                   


                    string BPCode = GetdataUnit(receiptModelNd.UnitCode.ToString());
                    txtCompanyName.Text = _receiptRepo.GetCompanyName(BPCode);
                    GetdatadllVehicle(BPCode);
                    Session["UnitCodeBlock"] = _receiptRepo.GetUnitByCode(receiptModelNd.UnitCode.ToString()).UnitCode.ToString(); ;
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
                List<WBTRXBLOCK1stEdit> wBTRXBLOCK1stEdit = new List<WBTRXBLOCK1stEdit>();
                wBTRXBLOCK = _receiptRepo.GetAllReceipDetailOneSttByTicket(TicketNo);
                if ( wBTRXBLOCK.Count > 0)
                {
                    ds = new DataSet();
                    DataTable receiptDetail = new DataTable();
                    receiptDetail.Columns.Add("ID", typeof(double));
                    receiptDetail.Columns.Add("TicketNo", typeof(string));
                    receiptDetail.Columns.Add("BlockID", typeof(string));
                    receiptDetail.Columns.Add("Divison", typeof(string));
                    receiptDetail.Columns.Add("YoP", typeof(string));
                    receiptDetail.Columns.Add("BunchesQty", typeof(string));
                    receiptDetail.Columns.Add("LFQty", typeof(string));
                    receiptDetail.Columns.Add("Estimation", typeof(string));
                    receiptDetail.Columns.Add("Weight", typeof(string));

                    foreach (var item in wBTRXBLOCK)
                    {
                        var rand = new Random();
                        int value = rand.Next(1000);
                        string txtID = value.ToString("000");

                        WBTRXBLOCK1stEdit wBTRXBLOCK1stEditRow = new WBTRXBLOCK1stEdit();
                        wBTRXBLOCK1stEditRow.ID = txtID;
                        wBTRXBLOCK1stEditRow.TicketNo = item.TicketNo;
                        wBTRXBLOCK1stEditRow.BlockID = item.BlockID;
                        wBTRXBLOCK1stEditRow.Division = item.Division;
                        wBTRXBLOCK1stEditRow.BunchesQty = item.BunchesQty;
                        wBTRXBLOCK1stEditRow.LFQty = item.LFQty;
                        wBTRXBLOCK1stEditRow.Estimation = item.Estimation;
                        wBTRXBLOCK1stEditRow.Weight = item.Weight;

                        wBTRXBLOCK1stEdit.Add(wBTRXBLOCK1stEditRow);
                    }

                    ListtoDataTableConverter converter = new ListtoDataTableConverter();
                    receiptDetail = converter.ToDataTable(wBTRXBLOCK1stEdit);
                    ds.Tables.AddRange(new DataTable[] { receiptDetail });

                    Session["DataSet"] = ds;

                    griddetail.DataSource = wBTRXBLOCK1stEdit;
                    griddetail.DataBind();
                }

                else
                {
                    Session["YoP"] = "";
                    //txtTransactionDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    //griddetail.StartEdit(3);
                    ds = new DataSet();
                    DataTable receiptDetail = new DataTable();
                    receiptDetail.Columns.Add("ID", typeof(double));
                    receiptDetail.Columns.Add("TicketNo", typeof(string));
                    receiptDetail.Columns.Add("BlockID", typeof(string));
                    receiptDetail.Columns.Add("Divison", typeof(string));
                    receiptDetail.Columns.Add("YoP", typeof(string));
                    receiptDetail.Columns.Add("BunchesQty", typeof(string));
                    receiptDetail.Columns.Add("LFQty", typeof(string));
                    receiptDetail.Columns.Add("Estimation", typeof(string));
                    receiptDetail.Columns.Add("Weight", typeof(string));

                    receiptDetail.PrimaryKey = new DataColumn[] { receiptDetail.Columns["ID"] };
                    ds.Tables.AddRange(new DataTable[] { receiptDetail });
                    Session["DataSet"] = ds;
                    Session["Idx"] = "0";
                    griddetail.DataSource = ds.Tables[0];
                    griddetail.DataBind();
                }

                List<WBTRXGRADING> WBGrading = new List<WBTRXGRADING>();
                List<WBTRXGRADINGEdit> WBGradingEdit = new List<WBTRXGRADINGEdit>();
               
                WBGrading = _receiptRepo.GetAllReceipDetailSecondNdByTicket(TicketNo).Count == 0 ? null : _receiptRepo.GetAllReceipDetailSecondNdByTicket(TicketNo);
                if (WBGrading != null)
                {

                    ds2 = new DataSet();
                    DataTable receiptDetail = new DataTable();
                    receiptDetail.Columns.Add("ID", typeof(double));
                    receiptDetail.Columns.Add("TicketNo", typeof(string));
                    receiptDetail.Columns.Add("GradingTypeID", typeof(string));
                    receiptDetail.Columns.Add("Quantity", typeof(double));
                    receiptDetail.Columns.Add("NoSegel1", typeof(string));
                    receiptDetail.Columns.Add("NoSegel2", typeof(string));
                    receiptDetail.PrimaryKey = new DataColumn[] { receiptDetail.Columns["ID"] };

                   
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
                    receiptDetail = converter.ToDataTable(WBGradingEdit);
                    ds2.Tables.AddRange(new DataTable[] { receiptDetail });

                    Session["DataSet2"] = ds2;
                    
                    
                    griddetail2nd.DataSource = WBGrading;
                    griddetail2nd.DataBind();
                }
                else
                {
                    ds2 = new DataSet();
                    DataTable receiptDetail = new DataTable();
                    receiptDetail.Columns.Add("ID", typeof(double));
                    receiptDetail.Columns.Add("TicketNo", typeof(string));
                    receiptDetail.Columns.Add("GradingTypeID", typeof(string));
                    receiptDetail.Columns.Add("Quantity", typeof(double));
                    receiptDetail.Columns.Add("NoSegel1", typeof(string));
                    receiptDetail.Columns.Add("NoSegel2", typeof(string));
                    receiptDetail.PrimaryKey = new DataColumn[] { receiptDetail.Columns["ID"] };
                    ds2.Tables.AddRange(new DataTable[] { receiptDetail });

                    griddetail2nd.DataSource = ds2.Tables[0];
                    griddetail2nd.DataBind();
                    Session["DataSet2"] = ds2;
                }

                List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                NetReceiptTotalModel listNetTotal = new NetReceiptTotalModel();
                Tuple<List<NetWeightModel>, NetReceiptTotalModel> resultTuple = _receiptRepo.GetNetWeight(TicketNo);

                listNetWeight = resultTuple.Item1;
                listNetTotal = resultTuple.Item2;
                griddetailnet.DataSource = listNetWeight;
                griddetailnet.DataBind();

                //string date = txtTransactionDate.Text;
                //DateTime myDate = Convert.ToDateTime(Convert.ToDateTime(date).ToString("dd/MM/yyyy HH:mm:ss"));

                //DateTime myDateLest = Convert.ToDateTime(Convert.ToDateTime("14/06/2022 09:10:41").ToString("dd/MM/yyyy HH:mm:ss"));

                double deduc = _receiptRepo.GetDeducation(txtTicketNo.Text.Trim());
                double stnet1 = Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString());
                Session["Sum1ST"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()));
                double cutpresent = double.Parse(Session["CutWeigh"].ToString()) / 100;
                double potongan = stnet1 * cutpresent; //stnet1 * (myDate > myDateLest ? cutpresent : 0); //double potongan = stnet1 * (myDate > myDateLest ? cutpresent : 0); 
                double tRotasi = stnet1 * deduc;
                Session["Potongan"] = potongan;
                Session["SumDeducation"] = String.Format("{0,15:#,##0 ;(#,##0);0  }", tRotasi.ToString());
                Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - tRotasi - potongan);


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

        protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtTicketNo.Text = _receiptRepo.GenerateTicketNo(ddlUnit.Value.ToString());
            string BPCode = GetdataUnit(ddlUnit.Value.ToString());
            txtCompanyName.Text = _receiptRepo.GetCompanyName(BPCode);
            GetdatadllVehicle(BPCode);
            Session["UnitCodeBlock"] = ddlUnit.Value.ToString();
        }
        void GetdatadllVehicle(string BPCode)
        {
            List<VehicleDllModel> listVehicle = new List<VehicleDllModel>();
            listVehicle = _receiptRepo.GetDllVehicle(BPCode);
            if (listVehicle.Count != 0)
            {
                ddlVehicle.DataSource = listVehicle;
                ddlVehicle.DataBindItems();
                Session["BPCode"] = listVehicle[0].BPCode;
            }
            else
            {
                ddlVehicle.DataSource = null;
                ddlVehicle.DataBindItems();
                Session["BPCode"] = BPCode;
            }
        }


        void GetBlockUnit(string UnitCode)
        {

            List<WBBLOCKModel> listVehicle = new List<WBBLOCKModel>();
            listVehicle = _receiptRepo.GetBlockUnit(UnitCode);
            ddlVehicle.DataSource = listVehicle;
            ddlVehicle.DataBindItems();
        }
        protected void ddlVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            VehicleDllModel listVehicle = new VehicleDllModel();
            listVehicle = _receiptRepo.GetDriver(ddlVehicle.Value.ToString());

            txtDriver.Text = listVehicle.DriverName;
            txtLisensiNo.Text = listVehicle.LicenseNo;
            GetTransporter(Session["BPCode"].ToString());

        }

        void GetTransporter(string BPCode)
        {

            txtTransporter.Text = _receiptRepo.GetCompanyName(Session["BPCode"].ToString()); ;
        }

        string GetdataUnit(string UnitCode)
        {
            BusinessUnitModel listUnit = new BusinessUnitModel();
            listUnit = _receiptRepo.GetUnitByCode(UnitCode);
            string vBPCode = listUnit.BPCode;
            return vBPCode;
        }

        protected void griddetail_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            double p_ID = _receiptRepo.GetIDBlock();

            ds = (DataSet)Session["DataSet"];
            //Session["ID"] = p_ID + ds.Tables[0].Columns[0].ToString() + 1;
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

            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            DataRow row = dataTable.NewRow();
            e.NewValues["ID"] = maxID + 1;
            //int x = maxID + 1;
            e.NewValues["TicketNo"] = txtTicketNo.Text;
            e.NewValues["Yop"] = Session["YoP"].ToString();
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                if (enumerator.Key.ToString() != "Count")
                    row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            dataTable.Rows.Add(row);

            ds = (DataSet)Session["DataSet"];

            griddetail.DataSource = ds.Tables[0];
            griddetail.DataBind();
            Session["Idx"] = "0";
        }
        protected void griddetail_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Session["ID"] = "";
            int i = griddetail.FindVisibleIndexByKeyValue(e.Keys[griddetail.KeyFieldName]);
            Control c = griddetail.FindDetailRowTemplateControl(i, "griddetail");
            e.Cancel = true;
            ds = (DataSet)Session["DataSet"];
            ds.Tables[0].Rows.Remove(ds.Tables[0].Rows.Find(e.Keys[griddetail.KeyFieldName]));
            griddetail.DataSource = ds.Tables[0];
            griddetail.DataBind();
        }

        protected void griddetail_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ds = (DataSet)Session["DataSet"];
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };
            DataRow row = dataTable.Rows.Find(e.Keys[0]);
            string x = Session["BlockID"].ToString();
            //e.NewValues["ID"] = Session["ID"];
            e.NewValues["TicketNo"] = txtTicketNo.Text;
            e.NewValues["Yop"] = _receiptRepo.GetYOP(Session["BlockID"].ToString());
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            griddetail.DataSource = ds.Tables[0];
            griddetail.DataBind();
        }

        protected void griddetail_CellEditorInitialize(object sender, BootstrapGridViewEditorEventArgs e)
        {
            Session["Idx"] = "1";

        }
        protected void griddetail2nd_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                ds2 = (DataSet)Session["DataSet2"];
                double p_ID = _receiptRepo.GetIDGrading();
                int maxID = 0;
                if (ds2.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        int accountLevel = int.Parse(dr.ItemArray[0].ToString());
                        maxID = Math.Max(maxID, accountLevel);
                    }
                }
                else
                {
                    maxID = int.Parse(p_ID.ToString()) + 1;
                }


                //ds2 = (DataSet)Session["DataSet2"];
                Session["ID"] = p_ID + ds2.Tables[0].Rows.Count + 1;
                ASPxGridView gridView = (ASPxGridView)sender;
                DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds2.Tables[1] : ds2.Tables[0];
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
                griddetail2nd.DataSource = ds2.Tables[0];
                griddetail2nd.DataBind();
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
            ds2 = (DataSet)Session["DataSet2"];
            ds2.Tables[0].Rows.Remove(ds2.Tables[0].Rows.Find(e.Keys[griddetail2nd.KeyFieldName]));
            griddetail2nd.DataSource = ds2.Tables[0];
            griddetail2nd.DataBind();
        }

        protected void griddetail2nd_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
 
            ds2 = (DataSet)Session["DataSet2"];
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds2.Tables[1] : ds2.Tables[0];
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };
            DataRow row = dataTable.Rows.Find(e.Keys[0]);         
            e.NewValues["TicketNo"] = txtTicketNo.Text;
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            griddetail2nd.DataSource = ds2.Tables[0];
            griddetail2nd.DataBind();
        }
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
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

        protected void griddetail_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            griddetail.JSProperties["cpPopulate"] = e.Parameters;
            Session["BlockID"] = e.Parameters;
            Session["YoP"] = _receiptRepo.GetYOP(e.Parameters);
            Session["Idx"] = "0";

            GridViewDataColumn col = ((ASPxGridView)sender).Columns["UnitPrice"] as GridViewDataColumn;
            ASPxTextBox txt = ((ASPxGridView)sender).FindEditRowCellTemplateControl(col, "YoP") as ASPxTextBox;

            //string[] pars = e.Parameters.Split(';');
            txt.Text = _receiptRepo.GetYOP(e.Parameters);


        }

      
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/View/Receipt.aspx");
        }

        void refreshObject()
        {
            //btnSave.Enabled = false;
        }
        void resetObject()
        {
            //btnSave.Enabled = true;
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

                //Untuk Test Hasil Convert
                var json = JsonConvert.SerializeObject(toArrBytes);

                if (File.Exists(Path.Combine(filePath)))
                {
                    // If file found, delete it    
                    File.Delete(Path.Combine(filePath));
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");

                return toArrBytes;
                //}
            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);
                return null;
            }
        }

        protected void ddlContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                WBCONTRACT wBContract = new WBCONTRACT();

                wBContract = _receiptRepo.GetWBCONTRACT(ddlContract.Value.ToString());
                txtCompanyName.Text = _receiptRepo.GetCompanyName(wBContract.BPCode.ToString());
                List<BusinessUnitModel> businessUnitModels = new List<BusinessUnitModel>();
                businessUnitModels = _receiptRepo.GetUnitByContract(wBContract.BPCode.ToString());
                ddlUnit.DataSource = businessUnitModels;
                ddlUnit.DataBindItems();

                List<CONTRACTPRODUCT> cONTRACTPRODUCT = new List<CONTRACTPRODUCT>();
                cONTRACTPRODUCT = _receiptRepo.GetContractProduct(ddlContract.Value.ToString());
                txtProductCode.Text = cONTRACTPRODUCT[0].ProductCode.ToString();
                txtProductName.Text = cONTRACTPRODUCT[0].ProductName.ToString();
                //ddlItem.DataSource = cONTRACTPRODUCT;
                //ddlItem.DataBindItems();

            }
            catch (Exception err)
            {
                MessageError(this, err.Message, "Error");
                //throw;
            }
        }

        protected void griddetail_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            if (e.CommandArgs.CommandName == "BlockID")
            {
                Console.WriteLine("");
            }
        }

        protected void griddetail_BeforePerformDataSelect(object sender, EventArgs e)
        {
            //ds = (DataSet)Session["DataSet"];
            //DataTable detailTable = ds.Tables[0];
            //DataView dv = new DataView(detailTable);
            //BootstrapGridView detailGridView = (BootstrapGridView)sender;
            //dv.RowFilter = "ID = " + detailGridView.GetMasterRowKeyValue();
            //detailGridView.DataSource = dv;
        }

        protected void griddetail_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
         
        }

        private void GetScaleNumber()
        {
            //try
            //{
            //    //string apiUrl = "https://localhost:44382/api/Apiwb";
            //    string apiUrl = ConfigurationManager.AppSettings["endpointwb"].ToString();
            //    HttpClient client = new HttpClient();
            //    HttpResponseMessage response = client.GetAsync(apiUrl + "/GetNumberScale").Result;
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var result = response.Content.ReadAsStringAsync();
            //        string bodyresult = result.Result;

            //        txtWB.Text = bodyresult;//Math.Ceiling( double.Parse(bodyresult.Replace(@"""", "").Replace(@"kg\r\n", "").Trim().ToString())).ToString("###,###,###");

            //    }
            //}
            //catch (Exception)
            //{

            //    MessageError(this, "Get Scale Failed...!!!", "Error Get Scale");
            //}

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

            receiptModelNd = reportRepo.GetFormReceiptHeader(txtTicketNo.Text);
            wBTRXBLOCK1Sts = reportRepo.GetFromReceiptBlock(txtTicketNo.Text);

            wBTRXGRADING2Nds = reportRepo.GetFromReceiptGrading(txtTicketNo.Text).Count == 0 ? null : reportRepo.GetFromReceiptGrading(txtTicketNo.Text);
            Companyls.Add(_receiptRepo.GetUnitByCode(Session["UnitCode"].ToString()));
            netWeightModels = reportRepo.GetNetWeightModel(txtTicketNo.Text);
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
            Server.Transfer("/Report/FormTicketReceipt.aspx");
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
               
                string err = "";
                //if (Double.Parse(txtWB1st.Text) <= 0)
                //{
                //    err = "Weigh 1st Can't be zero!!!";
                //    MessageWarning(this, err, "Warning");
                //}
                //if (Double.Parse(txtWB2nd.Text) <= 0)
                //{
                //    err = "Weigh 2nd Can't be zero!!!";
                //    MessageWarning(this, err, "Warning");
                //}
                if (Double.Parse(txtWB1st.Text) < Double.Parse(txtWB2nd.Text))
                {
                    err = "Weigh 1st Can't be small with Weigh 2nd";
                    MessageWarning(this, err, "Warning");
                }


                if (err == "")
                {


                    bool result = false;
                    byte[] cam1 = SaveImage();




                    WBTRXModel _WBTRXModel = new WBTRXModel();
                    List<WBTRXGRADING2nd> _WBTRXGRADING2nd = new List<WBTRXGRADING2nd>();
                    List<WBTRXBLOCK> _WBTRXBLOCK = new List<WBTRXBLOCK>();

                    _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                    _WBTRXModel.ContractNo = ddlContract.Text.Trim();
                    _WBTRXModel.VehicleID = ddlVehicle.Text;
                    _WBTRXModel.NoCoverLetter = txtLetter.Text;
                    _WBTRXModel.Weight1ST = Math.Ceiling(double.Parse(txtWB1st.Text));
                    _WBTRXModel.Weight2ND = Math.Ceiling(double.Parse(txtWB2nd.Text));
                    _WBTRXModel.DriverName = txtDriver.Text;
                    _WBTRXModel.LicenseNo = txtLisensiNo.Text;
                    //_WBTRXModel.WBStatus = "D";
                    _WBTRXModel.Status = "P";
                    _WBTRXModel.UserIDApproval = "";//Session["UserID"].ToString(); ;
                    _WBTRXModel.Updated = DateTime.Now;


                    ds = (DataSet)Session["DataSet"];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //Detail                  
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            WBTRXBLOCK wBTRXBLOCK = new WBTRXBLOCK();
                            wBTRXBLOCK.TicketNo = txtTicketNo.Text;
                            wBTRXBLOCK.BlockID = item.ItemArray[2].ToString();
                            wBTRXBLOCK.BunchesQty = item.ItemArray[5].ToString() == "" ? 0 : int.Parse(item.ItemArray[5].ToString());
                            wBTRXBLOCK.LFQty = item.ItemArray[6].ToString() == "" ? 0 : int.Parse(item.ItemArray[6].ToString());
                            wBTRXBLOCK.Estimation = item.ItemArray[7].ToString() == "" ? 0 : int.Parse(item.ItemArray[7].ToString());
                            wBTRXBLOCK.Weight = 0;//int.Parse((ds.Tables[0].Rows[0]["Weight"].ToString()) ="" ? 0 : ds.Tables[0].Rows[0]["Weight"].ToString())); 
                            _WBTRXBLOCK.Add(wBTRXBLOCK);

                        }
                    }
                    else
                    {
                        _WBTRXBLOCK = null;
                    }

                 

                    ds2 = (DataSet)Session["DataSet2"];                   
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds2.Tables[0].Rows)
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

                    double deduc = 0;
                    deduc = _receiptRepo.GetDeducation(txtTicketNo.Text.Trim());
                    List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                    NetReceiptTotalModel listNetTotal = new NetReceiptTotalModel();
                    Tuple<bool, List<NetWeightModel>, NetReceiptTotalModel> resultTuple = _receiptRepo.ApprovalReceipt(_WBTRXModel, _WBTRXBLOCK, _WBTRXGRADING2nd);

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
                        Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - tRotasi - potongan);
                        //Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - listNetTotal.Quantity - potongan);

                        //SendToOddo();
                        MessageSuccess(this, "success", "Success!");
                        refreshObject();
                        btnPrint.Enabled = true;
                        Response.Redirect("/View/Receipt.aspx");

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

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {

                string err = "";
                //if (Double.Parse(txtWB1st.Text) <= 0)
                //{
                //    err = "Weigh 1st Can't be zero!!!";
                //    MessageWarning(this, err, "Warning");
                //}
                //if (Double.Parse(txtWB2nd.Text) <= 0)
                //{
                //    err = "Weigh 2nd Can't be zero!!!";
                //    MessageWarning(this, err, "Warning");
                //}
                //if (Double.Parse(txtWB1st.Text) < Double.Parse(txtWB2nd.Text))
                //{
                //    err = "Weigh 1st Can't be small with Weigh 2nd";
                //    MessageWarning(this, err, "Warning");
                //}


                if (err == "")
                {


                    bool result = false;
                    byte[] cam1 = SaveImage();

                    WBTRXModel _WBTRXModel = new WBTRXModel();
                    List<WBTRXGRADING2nd> _WBTRXGRADING2nd = new List<WBTRXGRADING2nd>();
                    List<WBTRXBLOCK> _WBTRXBLOCK = new List<WBTRXBLOCK>();

                    _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                    _WBTRXModel.ContractNo = ddlContract.Text.Trim();
                    _WBTRXModel.VehicleID = ddlVehicle.Text;
                    _WBTRXModel.NoCoverLetter = txtLetter.Text;
                    _WBTRXModel.Weight1ST = Math.Ceiling(double.Parse(txtWB1st.Text));
                    _WBTRXModel.Weight2ND = Math.Ceiling(double.Parse(txtWB2nd.Text));
                    _WBTRXModel.WBStatus = "D";
                    _WBTRXModel.Status = "R";
                    _WBTRXModel.DriverName = txtDriver.Text;
                    _WBTRXModel.LicenseNo=txtLisensiNo.Text;
                    _WBTRXModel.UserIDApproval = Session["UserID"].ToString(); ;
                    _WBTRXModel.Updated = DateTime.Now;


                    _WBTRXBLOCK = null;
                    _WBTRXGRADING2nd = null;

                    double deduc = 0;
                    deduc = _receiptRepo.GetDeducation(txtTicketNo.Text.Trim());
                    List<NetWeightModel> listNetWeight = new List<NetWeightModel>();
                    NetReceiptTotalModel listNetTotal = new NetReceiptTotalModel();
                    Tuple<bool, List<NetWeightModel>, NetReceiptTotalModel> resultTuple = _receiptRepo.ApprovalReceipt(_WBTRXModel, _WBTRXBLOCK, _WBTRXGRADING2nd);

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
                        Session["Sum2ND"] = String.Format("{0,15:#,##0 ;(#,##0);0   }", Int32.Parse(listNetWeight[0].WeightHeavy.ToString()) - Int32.Parse(listNetWeight[1].WeightHeavy.ToString()) - tRotasi - potongan);
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
            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);

                Console.WriteLine(err.Message);
            }
        }

        private void NewGridBlock() {

            Session["YoP"] = "";
            txtTransactionDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            //griddetail.StartEdit(3);
            ds = new DataSet();
            DataTable receiptDetail = new DataTable();
            receiptDetail.Columns.Add("ID", typeof(double));
            receiptDetail.Columns.Add("TicketNo", typeof(string));
            receiptDetail.Columns.Add("BlockID", typeof(string));
            receiptDetail.Columns.Add("Divison", typeof(string));
            receiptDetail.Columns.Add("YoP", typeof(string));
            receiptDetail.Columns.Add("BunchesQty", typeof(string));
            receiptDetail.Columns.Add("LFQty", typeof(string));
            receiptDetail.Columns.Add("Estimation", typeof(string));
            receiptDetail.Columns.Add("Weight", typeof(string));

            receiptDetail.PrimaryKey = new DataColumn[] { receiptDetail.Columns["ID"] };
            ds.Tables.AddRange(new DataTable[] { receiptDetail });
            Session["DataSet"] = ds;
            Session["Idx"] = "0";
        }

        static DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }
    }
}