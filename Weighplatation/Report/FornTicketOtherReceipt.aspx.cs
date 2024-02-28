using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.IO;
using System.Web;
using Weighplatation.Model;

namespace Weighplatation.Report
{
    public partial class FornTicketOtherReceipt : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string Ext = "";
                string FileName = "";
                List<ReceiptModelNd> lt = new List<ReceiptModelNd>();
                List<WBTRXBLOCK1st> ltblock = new List<WBTRXBLOCK1st>();
                List<WBTRXGRADING2nd> ltgrading = new List<WBTRXGRADING2nd>();
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
                if (_context.Items["Ticketls"] != null)
                {
                    lt = HttpContext.Current.Items["Ticketls"] as List<ReceiptModelNd>;
                }
                if (_context.Items["Blockls"] != null)
                {
                    ltblock = HttpContext.Current.Items["Blockls"] as List<WBTRXBLOCK1st>;
                }
                if (_context.Items["Gradingls"] != null)
                {
                    ltgrading = HttpContext.Current.Items["Gradingls"] as List<WBTRXGRADING2nd>;
                }
                if (_context.Items["Companyls"] != null)
                {
                    ltcompany = HttpContext.Current.Items["Companyls"] as List<BusinessUnitModel>;
                }


                //string PrinterName = ConfigurationManager.AppSettings["printername"].ToString();
                //rptticketotherreceipt report = new rptticketotherreceipt();
                //PrinterSettings instance = new PrinterSettings();
                //string DefaultPrinter = instance.PrinterName;
                ////' THIS IS TO PRINT THE REPORT
                //report.PrinterName = DefaultPrinter;
                //report.CreateDocument();
                //report.PrintingSystem.ShowMarginsWarning = false;
                //report.Print(PrinterName);

                //Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language=javascript>self.close();</script>");

                PrinterSettings instance = new PrinterSettings();
                string DefaultPrinter = instance.PrinterName;
                rptticketotherreceipt report = new rptticketotherreceipt();
                ExportReport(report, FileName, Ext, true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void ExportReport(rptticketotherreceipt report, string fileName, string ext, bool inline)
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