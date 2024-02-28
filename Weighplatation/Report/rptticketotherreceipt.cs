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
    public partial class rptticketotherreceipt : DevExpress.XtraReports.UI.XtraReport
    {
        UserRepo userRepo = new UserRepo();
        public rptticketotherreceipt()
        {
            InitializeComponent();
            BindingData();
        }
        private void BindingData()
        {
            HttpContext _contex = HttpContext.Current;
            List<ReceiptModelNd> receiptModelNd = new List<ReceiptModelNd>();
            List<WBTRXBLOCK1st> wBTRXBLOCK1Sts = new List<WBTRXBLOCK1st>();
            List<WBTRXGRADING2nd> wBTRXGRADING2Nds = new List<WBTRXGRADING2nd>();
            List<BusinessUnitModel> businessUnitModel = new List<BusinessUnitModel>();
            List<NetWeightModel> netWeightModels = new List<NetWeightModel>();

            if (_contex.Items["Ticketls"] != null)
            {
                receiptModelNd = _contex.Items["Ticketls"] as List<ReceiptModelNd>;
                RptDetail.DataSource = receiptModelNd;

                //wBTRXBLOCK1Sts = _contex.Items["Blockls"] as List<WBTRXBLOCK1st>;
                //RptBlock.DataSource = wBTRXBLOCK1Sts;

                //wBTRXGRADING2Nds = _contex.Items["Gradingls"] as List<WBTRXGRADING2nd>;
                //RptGrading.DataSource = wBTRXGRADING2Nds;

                netWeightModels = _contex.Items["NetWeightls"] as List<NetWeightModel>;
                RptNet.DataSource = netWeightModels;

                businessUnitModel = _contex.Items["Companyls"] as List<BusinessUnitModel>;
                xrCompany.Text = businessUnitModel[0].UnitName;


                double deduct = Math.Ceiling(double.Parse(_contex.Items["QtyDeducation"].ToString()));
                double potongan = (netWeightModels[0].WeightHeavy - netWeightModels[1].WeightHeavy) * 0.02;
                //txtTotalBunches.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", wBTRXBLOCK1Sts.Sum(x => x.BunchesQty));
                //txtTotalBlock.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", (wBTRXBLOCK1Sts.Sum(x => x.Weight)));
                //txtGrading.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", (wBTRXGRADING2Nds == null ? 0 : wBTRXGRADING2Nds.Sum(x => x.Quantity)));
                txtSubTotal.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", (netWeightModels[0].WeightHeavy - netWeightModels[1].WeightHeavy));
                //txtpotongan.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", potongan);
                //txtDeducation.Text = String.Format("{0,15:#,##0 ;(#,##0);0  }", (deduct));
                txtTotal.Text = String.Format("{0,15:#,##0 ;(#,##0);0   }", (double.Parse(txtSubTotal.Text)));
                txtUser.Text = _contex.Items["User"] as string;
                //txtType.Text = _contex.Items["Type"] as string;

            }

        }
    }
}
