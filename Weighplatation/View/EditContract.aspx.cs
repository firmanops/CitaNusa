using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weighplatation.Model;
using Weighplatation.Repository;

namespace Weighplatation.View
{
    public partial class EditContract : System.Web.UI.Page
    {
        public ContractRepo contractRepo = new ContractRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ContractNo"] = Request.QueryString["ContractNo"];
            if (!IsPostBack)
            {
                GetContractByNo(Session["ContractNo"].ToString());
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Contract.aspx");
        }

        private void GetContractByNo(string ContractNo)
        {
            try
            {
                ContractModel contract = new ContractModel();
                contract = contractRepo.GetByContractNo(ContractNo);
                if (contract != null) {
                    txtContractNo.Text = contract.ContractNo;
                    ddlProduct.Value = contract.ProductCode;
                    txtContractDate.Value = contract.ContractDate.ToString();
                    txtExpDate.Value = contract.ExpDate.ToString();
                    ddlBP.Value = contract.BPCode;
                    txtQty.Text = contract.Qty.ToString();
                    txtToleransi.Text = contract.Toleransi.ToString();
                    txtUnitPrice.Text = contract.UnitPrice.ToString();
                    txtTotalNetWB.Text = contract.DespatchQty.ToString();
                    txtTotalPrice.Text = contract.TotalPrice.ToString();
                    txtDeliveryStatus.Text = contract.DeliveryStatus.ToString();
                    txtPPN.Text = contract.PPN.ToString();
                    txtFinalPrice.Text = contract.FinalUnitPrice.ToString();
                    txtRefNo.Text = contract.RefNo;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ContractModel contractModel = new ContractModel();

                contractModel.ContractNo = txtContractNo.Text;               
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
                contractModel.RefNo = txtRefNo.Text;

                bool result = contractRepo.UpdateContract(contractModel);

                if (result)
                {
                    MessageSuccess(this, "success", "Success!");
                    Response.Redirect("/View/Contract.aspx");
                }
                else
                {

                    throw new Exception();
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