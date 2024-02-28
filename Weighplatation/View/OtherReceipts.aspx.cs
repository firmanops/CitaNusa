using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weighplatation.Model;
using Weighplatation.Repository;

namespace Weighplatation.View
{
    public partial class OtherReceipts : System.Web.UI.Page
    {
        public ReceiptRepo _receiptRepo = new ReceiptRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            GridData();
        }
        void GridData()
        {
            List<OtherReceiptModel> receiptModel = _receiptRepo.GetAllOtherReceiptByToday();
            grid.DataSource = receiptModel;
            grid.DataBind();
        }

        protected void btnNewWB_Click(object sender, EventArgs e)
        {
            Response.Redirect("/View/OtherReceiptFirst.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<ReceiptModel> receiptModels = new List<ReceiptModel>();
            receiptModels = _receiptRepo.GetAllOtherReceipByFilter(txtTicketNo.Text,txtStartDate.Text, txtEndDate.Text);
            grid.DataSource = receiptModels;
            grid.DataBind();
        }
    }
}