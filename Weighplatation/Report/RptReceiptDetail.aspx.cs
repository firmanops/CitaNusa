using System;
using System.Collections.Generic;
using System.IO;
using DevExpress.XtraReports.Web;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;

namespace Weighplatation.Report
{
    public partial class RptReceiptDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string Ext = "";
                string FileName = "";
                List<RptReceiptDtlModel> lt = new List<RptReceiptDtlModel>();
                List<BusinessUnitModel> ltcompany = new List<BusinessUnitModel>();
                HttpContext _context = HttpContext.Current;
                if (_context.Items["Ext"] != null)
                {
                    Ext = _context.Items["Ext"].ToString();
                }
                if (_context.Items["FileName"] != null)
                {
                    FileName = _context.Items["FileName"].ToString();
                }
                if (_context.Items["ls"] != null)
                {
                    lt = _context.Items["ls"] as List<RptReceiptDtlModel>;
                }
                if (_context.Items["Companyls"] != null)
                {
                    ltcompany = HttpContext.Current.Items["Companyls"] as List<BusinessUnitModel>;
                }

                rptReportReceipt report = new rptReportReceipt();
                //report.ShowPrintStatusDialog=true;
                //PrinterSettings instance = new PrinterSettings();
                //string DefaultPrinter = instance.PrinterName;

                //' Buat Print Langsung
                //report.PrinterName = DefaultPrinter;
                //report.CreateDocument();
                //report.PrintingSystem.ShowMarginsWarning = false;
                //report.Print();
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language=javascript>self.close();</script>");


                ExportReport(report, FileName, Ext, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void ExportReport(rptReportReceipt report, string fileName, string ext, bool inline)
        {
            MemoryStream stream = new MemoryStream();

            Response.Clear();
            if (ext == "xls")
                report.ExportToXls(stream);
            if (ext == "pdf")
                report.ExportToPdf(stream);
            if (ext == "rtf")
                report.ExportToRtf(stream);
            if (ext == "csv")
                report.ExportToCsv(stream);

            

            //lucky
            Response.ContentType = "application/" + ext;
            Response.AddHeader("Accept-Header", stream.Length.ToString());
            Response.AddHeader("Content-Disposition", (inline ? "Inline" : "Attachment") + "; filename=" + fileName + "." + ext);
            Response.AddHeader("Content-Length", stream.Length.ToString());
            //Response.ContentEncoding = System.Text.Encoding.Default;
            Response.BinaryWrite(stream.ToArray());
            Response.End();

        }
    }
}