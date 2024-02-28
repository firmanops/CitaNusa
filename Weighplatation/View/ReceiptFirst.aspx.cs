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
using System.IO.Ports;
using Newtonsoft.Json;
using static DevExpress.Xpo.Helpers.CommandChannelHelper;
using System.Net.Http;
using System.Configuration;

namespace Weighplatation.View
{
    public partial class ReceiptFirst : System.Web.UI.Page
    {
        DataSet ds = null;
        public enum MessageType { Success, Error, Info, Warning };
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        public PortRepo _portRepo = new PortRepo();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();

       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack || (Session["DataSet"] == null))
            {
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
                resetObject();

            }
            else
            {
                if (Session["Idx"].ToString() == "0")
                {
                    ds = (DataSet)Session["DataSet"];
                    griddetail.DataSource = ds.Tables[0];
                    griddetail.DataBind();
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string err = "";
                //if (Double.Parse(txtWB.Text) <= 0) {
                //    err = "Weigh Can't be zero!!!";
                //    MessageWarning(this, err, "Warning");
                //}
                if (err == "") {
                    bool result = false;

                    byte[] cam1 = SaveImage();

                    WBTRXModel _WBTRXModel = new WBTRXModel();
                    List<WBTRXBLOCK> _WBTRXBLOCK = new List<WBTRXBLOCK>();

                    _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                    _WBTRXModel.WBSOURCE = Session["WBSOURCE"].ToString();
                    _WBTRXModel.WBType = "Receipt";
                    _WBTRXModel.TrxDateIn = DateTime.Now;
                    _WBTRXModel.TrxDateOut = DateTime.Parse("1900-01-01");
                    _WBTRXModel.UnitCode = ddlUnit.Value.ToString();
                    _WBTRXModel.NoCoverLetter = txtLetter.Text;
                    _WBTRXModel.ProductCode = txtProductCode.Text;
                    _WBTRXModel.ContractNo = ddlContract.Value.ToString();
                    _WBTRXModel.DONo = "";
                    _WBTRXModel.VehicleID = ddlVehicle.Value.ToString();
                    _WBTRXModel.DriverName = txtDriver.Text;
                    _WBTRXModel.LicenseNo = txtLisensiNo.Text;
                    _WBTRXModel.Weight1ST = Math.Ceiling(double.Parse(txtWB.Text));
                    _WBTRXModel.Weight2ND = 0;
                    _WBTRXModel.WBFlag1 = "A";
                    _WBTRXModel.WBStatus = "F";
                    _WBTRXModel.Status= "P";
                    _WBTRXModel.WBImagefront1 = cam1;
                    _WBTRXModel.Reason = "";
                    _WBTRXModel.UserIDWeight1ST = Session["UserID"].ToString();
                    _WBTRXModel.UserIDWeight2ND = "";
                    _WBTRXModel.UserIDApproval = "";
                    _WBTRXModel.Created = DateTime.Now;
                    _WBTRXModel.Updated = DateTime.Parse("1900-01-01");
                    _WBTRXModel.StatusOddo = false;
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

                    result = _receiptRepo.InsertReceiptHeader(_WBTRXModel, _WBTRXBLOCK);

                    if (result)
                    {
                        MessageSuccess(this, "success", "Success!");
                        refreshObject();
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

        protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTicketNo.Text = _receiptRepo.GenerateTicketNo(ddlUnit.Value.ToString());
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
            else {
                ddlVehicle.DataSource =null;
                ddlVehicle.DataBindItems();
                Session["BPCode"] = BPCode;
            }
        }


        void GetBlockUnit(string UnitCode) {

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
            else {
                maxID = int.Parse(p_ID.ToString()) +  1;
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

        protected void btnWB_Click(object sender, EventArgs e)
        {
            try
            {


                //// Create a Process Object here. 
                //System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                ////Working Directory Of .exe File. 
                //process1.StartInfo.WorkingDirectory = Request.MapPath("~/Exe/");
                ////exe File Name. 
                //process1.StartInfo.FileName = Request.MapPath("../Exe/WindowsFormsApp1.exe");
                ////Argement Which you have tp pass. 
                //process1.StartInfo.Arguments = " ";
                //process1.StartInfo.LoadUserProfile = true;
                ////Process Start on exe.
                //process1.Start();
                //process1.WaitForExit();
                //process1.Close();


                txtWB.Text = apiOddoRepo.Getscale(); // Dekstop


                //GetScaleNumber();
                ////btnWB.Text = "Process Weigh ...";

                ////Console.WriteLine("");
                //PortModel _portModel = new PortModel();

                //_portModel = _portRepo.GetPort(Session["WBSource"].ToString());

                //SerialPort mySerialPort = new SerialPort("COM" + _portModel.ComPort);

                //mySerialPort.BaudRate = int.Parse(_portModel.Bautrate.ToString());
                //mySerialPort.Parity = Parity.None;
                //mySerialPort.StopBits = StopBits.One;
                //mySerialPort.DataBits = int.Parse(_portModel.DataBits.ToString());
                //mySerialPort.Handshake = Handshake.None;
                //mySerialPort.Open();
                //btnWB.Text = "Reading...";
                //////mySerialPort.ReadTimeout = 1000;
                ////string Scale = mySerialPort.ReadLine();
                ////Scale = Scale.ToString().Replace("kg", "").Trim();
                ////Scale = Scale.ToString().Replace(".", "");
                //txtWB.Text = mySerialPort.ReadLine();

                //mySerialPort.Close();
                //btnWB.Text = "Get WB";
                //txtWB.Text = mySerialPort.ReadLine().ToString().Substring(7, 6);

                ////string test = "ST,NT,+000000.Kg";
                ////txtWB.Text = test.ToString().Substring(7, 6);





                //btnWB.Text = "Process WB";
            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);
            }

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

        protected void griddetail_CellEditorInitialize(object sender, BootstrapGridViewEditorEventArgs e)
        {
            Session["Idx"] = "1";

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Receipt.aspx");
        }

        void refreshObject()
        {
            btnSave.Enabled = false;
        }
        void resetObject()
        {
            btnSave.Enabled = true;
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
                txtProductName.Text= cONTRACTPRODUCT[0].ProductName.ToString();
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
            if (e.CommandArgs.CommandName == "BlockID") {
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

                    txtWB.Text = bodyresult;//Math.Ceiling( double.Parse(bodyresult.Replace(@"""", "").Replace(@"kg\r\n", "").Trim().ToString())).ToString("###,###,###");

                }
            }
            catch (Exception)
            {

                MessageError(this,"Get Scale Failed...!!!","Error Get Scale");
            }

        }

       
    }
}