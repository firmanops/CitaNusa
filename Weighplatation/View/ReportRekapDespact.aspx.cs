using System;
using System.Collections.Generic;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Repository;

namespace Weighplatation.View
{
    public partial class ReportRekapDespact : System.Web.UI.Page
    {
        ReportRepo reportRepo = new ReportRepo();
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Dashboard.aspx");
        }

        protected void btnPrints_Click(object sender, EventArgs e)
        {

         
            List<RptRkpDespacth> rptReceiptCust = new List<RptRkpDespacth>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();

            rptReceiptCust = reportRepo.GetRekapDespacth(txtStartDate.Text, txtEndDate.Text);
            Companyls.Add(_receiptRepo.GetUnitByCode(Session["UnitCode"].ToString()));

            string Ext = "xls";
            string FileName = "ReportRekapDespacth";
            HttpContext.Current.Items["Ext"] = Ext;
            HttpContext.Current.Items["FileName"] = FileName;          
            HttpContext.Current.Items["lsspl"] = rptReceiptCust;
            HttpContext.Current.Items["startdate"] = txtStartDate.Text;
            HttpContext.Current.Items["finishdate"] = txtEndDate.Text;
            HttpContext.Current.Items["Companyls"] = Companyls;
            HttpContext.Current.Items["User"] = Session["UserName"];
            Server.Transfer("/Report/RptRekapDespacth.aspx");
        }
    }
}