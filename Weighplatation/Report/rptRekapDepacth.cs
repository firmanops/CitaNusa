using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using Weighplatation.Model;

namespace Weighplatation.Report
{
    public partial class rptRekapDepacth : DevExpress.XtraReports.UI.XtraReport
    {
        public rptRekapDepacth()
        {
            InitializeComponent();
            BindingData();
        }
        private void BindingData()
        {
            HttpContext _contex = HttpContext.Current;

        
            List<RptRkpDespacth> rptRkpRcptSupplierModels = new List<RptRkpDespacth>();
            List<BusinessUnitModel> Companyls = new List<BusinessUnitModel>();

            if (_contex.Items["lsspl"] != null)
            {
                rptRkpRcptSupplierModels = _contex.Items["lsspl"] as List<RptRkpDespacth>;
                DetailCust.DataSource = rptRkpRcptSupplierModels;



                Companyls = _contex.Items["Companyls"] as List<BusinessUnitModel>;
                xrCompany.Text = Companyls[0].UnitName;

                txtstartdate.Text = _contex.Items["startdate"].ToString();
                txtfinishdate.Text = _contex.Items["finishdate"].ToString();
                txtUser.Text = _contex.Items["User"] as string;
               
            }

        }
    }

}

