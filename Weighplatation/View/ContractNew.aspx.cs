using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weighplatation.Model;
using Weighplatation.Repository;
using DevExpress.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Text;
using System.Configuration;
using System.IO;

namespace Weighplatation.View
{
    public partial class ContractNew : System.Web.UI.Page
    {
        public ContractRepo contractRepo = new ContractRepo();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Contract.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string err = "";

                if (txtContractNo.Text == "") {
                    throw new Exception("Contract No. is required");
                }
                if (txtContractDate.Text == "")
                {
                    throw new Exception("Contract Date is required");
                }
                if (txtExpDate.Text == "")
                {
                    throw new Exception("Contract Date Exp. is required");
                }
                if (txtToleransi.Text == "")
                {
                    throw new Exception("Toleransi is required");
                }
                if (ddlProduct.Value.ToString() == "")
                {
                    throw new Exception("Product is required");
                }
                if (ddlBP.Value.ToString()== "")
                {
                    throw new Exception("Business Partner required");
                }

                if (txtPPN.Text == "")
                {
                    throw new Exception("PPN required");
                }

                if (txtUnitPrice.Text == "")
                {
                    throw new Exception("Unit Price required");
                }

                if (txtQty.Text == "")
                {
                    throw new Exception("Quantity required");
                }
                if (err == "") {
                    ContractModel contractModel = new ContractModel();

                    contractModel.ContractNo = txtContractNo.Text;
                    contractModel.ContractDate = DateTime.Parse(txtContractDate.Text);
                    contractModel.ExpDate = DateTime.Parse(txtExpDate.Text);
                    contractModel.ProductCode = ddlProduct.Value.ToString();
                    contractModel.BPCode = ddlBP.Value.ToString();
                    contractModel.Qty = double.Parse(txtQty.Text);
                    contractModel.Toleransi = double.Parse(txtToleransi.Text);
                    contractModel.UnitPrice = double.Parse(txtUnitPrice.Text);
                    contractModel.PremiumPrice = 0;
                    contractModel.PPN = int.Parse(txtPPN.Text);
                    contractModel.FinalUnitPrice = Math.Ceiling(double.Parse(txtFinalPrice.Text));
                    contractModel.TotalPrice = double.Parse(txtTotalPrice.Text);
                    contractModel.DespatchQty = 0;
                    contractModel.DeliveryStatus = "1";
                    contractModel.oddoid = 0;
                    contractModel.RefNo = txtRefNo.Text;

                    bool result = contractRepo.InsertContract(contractModel);

                    if (result)
                    {
                        MessageSuccess(this, "success", "Success!");
                        Response.Redirect("/View/Contract.aspx");
                    }
                    else
                    {
                        MessageSuccess(this, "failed", "Failed!");
                        throw new Exception();
                    }
                }
                
            }
            catch (Exception err)
            {

                MessageError(this, err.Message, "Error!");
            }
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
    }
}