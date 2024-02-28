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
using System.Net.Http;
using System.Configuration;

namespace Weighplatation.View
{
    public partial class DespactFirst : System.Web.UI.Page
    {
        public DespactRepo _DespactRepo = new DespactRepo();
        public PortRepo _portRepo = new PortRepo();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                txtTransactionDate.Text = DateTime.Now.ToString("dd-MM-yyy HH:mm:ss");
                //refreshObject();
                
            }
            //else {
            //    //resetObject();
            //}
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Enabled==true) {
                    bool result = false;
                    byte[] cam1 = SaveImage();
                    WBTRXModel _WBTRXModel = new WBTRXModel();

                    //Session["WBSOURCE"] = "WB1";
                    //Session["UserID"] = "Firman";
                    _WBTRXModel.TicketNo = txtTicketNo.Text.Trim();
                    _WBTRXModel.WBSOURCE = Session["WBSOURCE"].ToString();
                    _WBTRXModel.WBType = "Despact";
                    _WBTRXModel.TrxDateIn = DateTime.Now;
                    _WBTRXModel.TrxDateOut = DateTime.Parse("1900-01-01");
                    _WBTRXModel.UnitCode = Session["UnitCodeOwner"].ToString();
                    _WBTRXModel.NoCoverLetter = "";
                    _WBTRXModel.ProductCode = txtProductCode.Text;
                    _WBTRXModel.ContractNo = ddlContract.Value.ToString();
                    _WBTRXModel.DONo = dllDelivery.Value.ToString();
                    _WBTRXModel.VehicleID = ddlVehicle.Value.ToString();
                    _WBTRXModel.DriverName = txtDriver.Text;
                    _WBTRXModel.LicenseNo = txtLisense.Text;
                    _WBTRXModel.Weight1ST = Math.Ceiling(double.Parse(txtWB.Text));
                    _WBTRXModel.Weight2ND = 0;
                    _WBTRXModel.WBFlag1 = "A";
                    _WBTRXModel.WBStatus = "F";
                    _WBTRXModel.WBImagefront1 = cam1;
                    _WBTRXModel.UserIDWeight1ST = Session["UserID"].ToString();
                    _WBTRXModel.UserIDWeight2ND = "";
                    _WBTRXModel.UserIDApproval = "";
                    _WBTRXModel.Created = DateTime.Now;
                    _WBTRXModel.Updated = DateTime.Parse("1900-01-01");
                    _WBTRXModel.Reason = "";

                    result = _DespactRepo.InsertDespactHeader(_WBTRXModel);
                    if (result)
                    {
                        MessageSuccess(this, "success", "Success!");
                        refreshObject();
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
        void GetTransporter(string BPCode)
        {

            txtTransporter.Text =_DespactRepo.GetCompanyName(BPCode);
        }
    

        protected void btnWB_Click(object sender, EventArgs e)
        {
            try
            {
                txtWB.Text = apiOddoRepo.Getscale();
                //GetScaleNumber();
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
                //txtWB.Text = mySerialPort.ReadLine().ToString().Substring(6, 6);

                //mySerialPort.Close();
            }
            catch (Exception err)
            {
                MessageError(this, "Error", err.Message);
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/View/Despact.aspx");
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

        protected void ddlContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                WBCONTRACT wBContract = new WBCONTRACT();

                wBContract = _DespactRepo.GetWBCONTRACT(ddlContract.Value.ToString());

                List<DOBPCODEModel> listDo = new List<DOBPCODEModel>();
                listDo = _DespactRepo.GetDllDO(ddlContract.Value.ToString());
                dllDelivery.DataSource = listDo;
                dllDelivery.DataBindItems();


                WBOWNER wBOWNER = new WBOWNER();
                wBOWNER = _DespactRepo.GetUnitWBOwner(); 

                CONTRACTPRODUCT cONTRACTPRODUCT = new CONTRACTPRODUCT();
                cONTRACTPRODUCT = _DespactRepo.GetCONTRACTPRODUCT(ddlContract.Value.ToString());

                txtProduct.Text = cONTRACTPRODUCT.ProductName.ToString();
                txtProductCode.Text = cONTRACTPRODUCT.ProductCode.ToString();
                List<BusinessUnitModel> businessUnitModels = new List<BusinessUnitModel>();
                businessUnitModels = _receiptRepo.GetUnitByContract(wBContract.BPCode.ToString());
                txtCompanyName.Text = businessUnitModels[0].UnitName;
                txtTicketNo.Text = _DespactRepo.GenerateTicketNo(wBOWNER.UnitCode.ToString());
                Session["UnitCodeOwner"] = businessUnitModels[0].UnitCode; ;


            }
            catch (Exception err)
            {
                MessageError(this, err.Message, "Error");
                //throw;
            }
        }
        protected void dllDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            WBDOModel wBDO = new WBDOModel();
            wBDO = _DespactRepo.GetDONoBPCode(dllDelivery.Value.ToString());

            List<VehicleDllModel> listVehicle = new List<VehicleDllModel>();
            listVehicle = _DespactRepo.GetDllVehicle(wBDO.BPCode.ToString());
            ddlVehicle.DataSource = listVehicle;
            ddlVehicle.DataBindItems();
            GetTransporter(wBDO.BPCode.ToString());
        }

        protected void ddlVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            VehicleDllModel _VehicleDllModel = new VehicleDllModel();
            _VehicleDllModel = _DespactRepo.GetDriver(ddlVehicle.Value.ToString());           
            txtDriver.Text = _VehicleDllModel.DriverName;
            txtLisense.Text = _VehicleDllModel.LicenseNo;
        }
        void refreshObject() {
            btnSave.Enabled=false;
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