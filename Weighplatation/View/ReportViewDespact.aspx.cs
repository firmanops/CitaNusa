using System;
using System.Collections.Generic;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Repository;


namespace Weighplatation.View
{
    public partial class ReportViewDespact : System.Web.UI.Page
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

            List<RptDespacthDtlModel> rptReceiptDtlModels = new List<RptDespacthDtlModel>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();

            rptReceiptDtlModels = reportRepo.GetRptReportDespactDetail(txtStartDate.Text, txtEndDate.Text);
            Companyls.Add(_receiptRepo.GetUnitByCode(Session["UnitCode"].ToString()));


            string Ext = "xls";
            string Filename = "ReportDespacth";
            HttpContext.Current.Items["Ext"] = Ext;
            HttpContext.Current.Items["Filename"] = Filename;
            HttpContext.Current.Items["ls"] = rptReceiptDtlModels;
            HttpContext.Current.Items["Companyls"] = Companyls;
            HttpContext.Current.Items["startdate"] = txtStartDate.Text;
            HttpContext.Current.Items["finishdate"] = txtEndDate.Text;
            Server.Transfer("/Report/RptDespacthDetail.aspx");
        }
    }
}