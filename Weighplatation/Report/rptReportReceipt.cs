using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Repository;
using System.Web.Script.Serialization;
using System.Linq;

namespace Weighplatation.Report
{
    public partial class rptReportReceipt : DevExpress.XtraReports.UI.XtraReport
    {
        public rptReportReceipt()
        {
            InitializeComponent();
            BindingData();

        }
        private void BindingData()
        {
            HttpContext _contex = HttpContext.Current;
          
            List<RptReceiptDtlModel> rptReceiptDtlModels = new List<RptReceiptDtlModel>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();


            if (_contex.Items["ls"] != null)
            {
                rptReceiptDtlModels = _contex.Items["ls"] as List<RptReceiptDtlModel>;
                DetailReport.DataSource = rptReceiptDtlModels;



                Companyls = _contex.Items["Companyls"] as List<BusinessUnitModel>;
                xrCompany.Text = Companyls[0].UnitName;

                txtstartdate.Text = _contex.Items["startdate"].ToString();
                txtfinishdate.Text = _contex.Items["finishdate"].ToString();

            }

        }
    }
}
