using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using Weighplatation.Model;

namespace Weighplatation.Report
{
    public partial class rptRekapReceipt : DevExpress.XtraReports.UI.XtraReport
    {
        public rptRekapReceipt()
        {
            InitializeComponent();
            BindingData();
        }
        private void BindingData()
        {
            HttpContext _contex = HttpContext.Current;

            List<RptRkpRcptMuatanModel> rptRkpRcptMuatanModel = new List<RptRkpRcptMuatanModel>();
            List<RptRkpRcptSupplierModel> rptRkpRcptSupplierModels = new List<RptRkpRcptSupplierModel>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();

            if (_contex.Items["lsmtn"] != null)
            {
                rptRkpRcptMuatanModel = _contex.Items["lsmtn"] as List<RptRkpRcptMuatanModel>;
                Detailmuatan.DataSource = rptRkpRcptMuatanModel;



                Companyls = _contex.Items["Companyls"] as List<BusinessUnitModel>;
                xrCompany.Text = Companyls[0].UnitName;

                txtstartdate.Text = _contex.Items["startdate"].ToString();
                txtfinishdate.Text = _contex.Items["finishdate"].ToString();
                txtUser.Text = _contex.Items["User"] as string;
            }


            if (_contex.Items["lsspl"] != null)
            {
                rptRkpRcptSupplierModels = _contex.Items["lsspl"] as List<RptRkpRcptSupplierModel>;
                DetailSupplier.DataSource = rptRkpRcptSupplierModels;
            }

        }
    }
}
