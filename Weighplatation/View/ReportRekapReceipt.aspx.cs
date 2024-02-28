using System;
using System.Collections.Generic;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Repository;

namespace Weighplatation.View
{
    public partial class ReportRekapReceipt : System.Web.UI.Page
    {
        ReportRepo reportRepo = new ReportRepo();
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Dashboard.aspx");
        }

        protected void btnPrints_Click(object sender, EventArgs e)
        {

            List<RptRkpRcptMuatanModel> rptReceiptMtn = new List<RptRkpRcptMuatanModel>();
            List<RptRkpRcptSupplierModel> rptReceiptSpl = new List<RptRkpRcptSupplierModel>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();

            rptReceiptMtn = reportRepo.GetRekapReceiptMtn(txtStartDate.Text, txtEndDate.Text);
            rptReceiptSpl = reportRepo.GetRekapReceiptSpl(txtStartDate.Text, txtEndDate.Text);
            Companyls.Add(_receiptRepo.GetUnitByCode(Session["UnitCode"].ToString()));

            string Ext = "xls";
            string FileName = "ReportRekapReceipt";
            HttpContext.Current.Items["Ext"] = Ext;
            HttpContext.Current.Items["FileName"] = FileName;
            HttpContext.Current.Items["lsmtn"] = rptReceiptMtn;
            HttpContext.Current.Items["lsspl"] = rptReceiptSpl;
            HttpContext.Current.Items["startdate"] = txtStartDate.Text;
            HttpContext.Current.Items["finishdate"] = txtEndDate.Text;
            HttpContext.Current.Items["Companyls"] = Companyls;
            HttpContext.Current.Items["User"] = Session["UserName"];
            Server.Transfer("/Report/RptRekapReceipt.aspx");
        }
    }
}