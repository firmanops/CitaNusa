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
using System.Text;

namespace Weighplatation.View
{
    public partial class ManualReceiptView : System.Web.UI.Page
    {
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        public ReportRepo reportRepo = new ReportRepo();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["TicketNo"] = Request.QueryString["Ticket"];
          

            if (!IsPostBack)
            {
                GetAllWbTrx(Session["TicketNo"].ToString());               
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
                    txtItem.Text = receiptModelNd.ProductName;
                    txtCompanyName.Text = receiptModelNd.BPName;
                    txtVehicle.Text = receiptModelNd.VehicleID;
                    txtUnit.Text = receiptModelNd.UnitName;
                    transpoterModel = _receiptRepo.GetTransporter(txtVehicle.Text);
                    txtTransporter.Text = transpoterModel.BPName;
                    txtContract.Text = receiptModelNd.ContractNo;
                    txtDriver.Text = receiptModelNd.DriverName;
                    txtLisensiNo.Text = receiptModelNd.Lisense;
                    txtWB1.Text = receiptModelNd.Weight1st.ToString();
                    txtWB2.Text = receiptModelNd.Weight2nd.ToString();
                    txtLetter.Text = receiptModelNd.LetterNo;
                    Session["weight1st"] = receiptModelNd.Weight1st;
                    Session["weight2nd"] = receiptModelNd.Weight2nd;
                    img1.ContentBytes = receiptModelNd.WBImagefront1;
                    img2.ContentBytes = receiptModelNd.WBImagefront2;
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManualReceipt.aspx");
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
            wBTRXGRADING2Nds = reportRepo.GetFromReceiptGrading(txtTicketNo.Text);
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
            HttpContext.Current.Items["User"] = Session["UserName"];
            HttpContext.Current.Items["QtyDeducation"] = QtyDeducation;
            HttpContext.Current.Items["Type"] = "Manual";
            Server.Transfer("/Report/FormTicketReceipt.aspx");
        }
    }
}