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
    public partial class rptticketDespach : DevExpress.XtraReports.UI.XtraReport
    {
        public rptticketDespach()
        {
            InitializeComponent();
            BindingData();
        }
        private void BindingData()
        {
            try
            {
                HttpContext _contex = HttpContext.Current;
                List<DespactModelNd> receiptModelNd = new List<DespactModelNd>();
                List<WBTRXGRADING2nd> wBTRXGRADING2Nds = new List<WBTRXGRADING2nd>();
                List<BusinessUnitModel> businessUnitModel = new List<BusinessUnitModel>();
                List<NetWeightModel> netWeightModels = new List<NetWeightModel>();

                if (_contex.Items["Ticketls"] != null)
                {
                    receiptModelNd = _contex.Items["Ticketls"] as List<DespactModelNd>;
                    RptDetail.DataSource = receiptModelNd;

                    wBTRXGRADING2Nds = _contex.Items["Gradingls"] as List<WBTRXGRADING2nd>;
                    RptGrading.DataSource = wBTRXGRADING2Nds;

                    netWeightModels = _contex.Items["NetWeightls"] as List<NetWeightModel>;
                    RptNet.DataSource = netWeightModels;

                    businessUnitModel = _contex.Items["Companyls"] as List<BusinessUnitModel>;
                    xrCompany.Text = businessUnitModel[0].UnitName;

                    double potongan = 0;// (netWeightModels[0].WeightHeavy - netWeightModels[1].WeightHeavy) * 0.02;
                    txtSubTotal.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", netWeightModels[1].WeightHeavy - netWeightModels[0].WeightHeavy);
                    //txtpotongan.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", potongan);
                    txtTotal.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", (netWeightModels[1].WeightHeavy - netWeightModels[0].WeightHeavy - potongan));
                    txtUser.Text = _contex.Items["User"] as string;
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
                throw;
            }
          
            

        }
    }
}
