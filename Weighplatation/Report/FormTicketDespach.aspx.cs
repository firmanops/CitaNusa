using System;
using System.Collections.Generic;
using System.IO;
using DevExpress.XtraReports.Web;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using DevExpress.XtraPrinting;
using System.Drawing.Printing;
using System.Configuration;

namespace Weighplatation.Report
{
    public partial class FormTicketDespach : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {              
                string Ext = "";
                string FileName = "";
                List<DespactModelNd> lt = new List<DespactModelNd>();               
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
                    lt = HttpContext.Current.Items["Ticketls"] as List<DespactModelNd>;
                }               
                if (_context.Items["Gradingls"] != null)
                {
                    ltgrading = HttpContext.Current.Items["Gradingls"] as List<WBTRXGRADING2nd>;
                }
                if (_context.Items["Gradingls"] != null)
                {
                    ltcompany = HttpContext.Current.Items["Companyls"] as List<BusinessUnitModel>;
                }

                string PrinterName = ConfigurationManager.AppSettings["printername"].ToString();

                // THIS IS TO TAKE THE DEFAULT LOCAL PRINT
                rptticketDespach report = new rptticketDespach();
                PrinterSettings instance = new PrinterSettings();
                string DefaultPrinter = instance.PrinterName;
                //' THIS IS TO PRINT THE REPORT
                report.PrinterName = DefaultPrinter;
                report.CreateDocument();
                report.PrintingSystem.ShowMarginsWarning = false;
                report.Print(PrinterName);
                Response.Redirect("Despact.aspx");
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language=javascript>self.close();</script>");

                //rptticketDespach report = new rptticketDespach();
                //PrinterSettings instance = new PrinterSettings();
                //string DefaultPrinter = instance.PrinterName;
                //ExportReport(report, FileName, Ext, true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void ExportReport(rptticketDespach report, string fileName, string ext, bool inline)
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