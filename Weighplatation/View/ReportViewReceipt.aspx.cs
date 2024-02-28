using System;
using System.Collections.Generic;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Repository;

namespace Weighplatation.View
{
    public partial class ReportViewReceipt : System.Web.UI.Page
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
           
            List<RptReceiptDtlModel> rptReceiptDtlModels = new List<RptReceiptDtlModel>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();

            rptReceiptDtlModels = reportRepo.GetRptReportDetail(txtStartDate.Text, txtEndDate.Text);
            Companyls.Add(_receiptRepo.GetUnitByCode(Session["UnitCode"].ToString()));
           

            string Ext = "xls";
            string FileName = "ReportReceipt";
            HttpContext.Current.Items["Ext"] = Ext;
            HttpContext.Current.Items["FileName"] = FileName;
            HttpContext.Current.Items["ls"] = rptReceiptDtlModels;
            HttpContext.Current.Items["Companyls"] = Companyls;
            HttpContext.Current.Items["startdate"] = txtStartDate.Text;
            HttpContext.Current.Items["finishdate"] = txtEndDate.Text;
            Server.Transfer("/Report/RptReceiptDetail.aspx");
        }
    }
}