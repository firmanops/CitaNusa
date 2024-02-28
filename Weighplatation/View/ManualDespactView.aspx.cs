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
    public partial class ManualDespactView : System.Web.UI.Page
    {
        public DespactRepo _despactRepo = new DespactRepo();
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
                DespactModelNd receiptModelNd = new DespactModelNd();
                TranspoterModel transpoterModel = new TranspoterModel();
                Session["weight1st"] = "";
                Session["weight2nd"] = "";

                receiptModelNd = _despactRepo.GetAllDespactByTicket(TicketNo);
                if (receiptModelNd != null)
                {
                    txtTicketNo.Text = receiptModelNd.TicketNo;
                    txtTransactionDate.Text = receiptModelNd.Created.ToString();
                    txtItem.Text = receiptModelNd.ProductName;
                    txtCompanyName.Text = receiptModelNd.BPName;
                    txtVehicle.Text = receiptModelNd.VehicleID;
                    txtUnit.Text = receiptModelNd.UnitName;
                    transpoterModel = _despactRepo.GetTransporter(txtVehicle.Text);
                    txtTransporter.Text = transpoterModel.BPName;
                    txtContract.Text = receiptModelNd.ContractNo;
                    txtDriver.Text = receiptModelNd.DriverName;
                    txtLisensiNo.Text = receiptModelNd.Lisense;
                    txtWB1.Text = receiptModelNd.Weight1st.ToString();
                    txtWB2.Text = receiptModelNd.Weight2nd.ToString();
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
            Response.Redirect("ManualDespact.aspx");
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

    }
}