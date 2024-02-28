using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Collections;
using DevExpress.Web.Bootstrap;

namespace Weighplatation.View
{
    public partial class GridTest : System.Web.UI.Page
    {
        DataSet ds = null;
       
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack || (Session["DataSet"] == null))
            {
                ds = new DataSet();
                DataTable receiptDetail = new DataTable();
                receiptDetail.Columns.Add("TicketNo", typeof(string));
                receiptDetail.Columns.Add("BlockID", typeof(string));
                receiptDetail.Columns.Add("Divison", typeof(string));
                receiptDetail.Columns.Add("YoP", typeof(string));
                receiptDetail.Columns.Add("BunchesQty", typeof(string));
                receiptDetail.Columns.Add("LFQty", typeof(string));
                receiptDetail.Columns.Add("Estimation", typeof(string));
                receiptDetail.Columns.Add("Weight", typeof(string));

                receiptDetail.PrimaryKey = new DataColumn[] { receiptDetail.Columns["TicketNo"] };
                ds.Tables.AddRange(new DataTable[] { receiptDetail });
                Session["DataSet"] = ds;

                Session["Idx"] = "0";


            }
            else
            {
                if (Session["Idx"].ToString() == "0")
                {
                   
                    ds = (DataSet)Session["DataSet"];
                   
                    griddetail.DataSource = ds.Tables[0];
                    griddetail.DataBind();
                }


            };





        }
        protected void griddetail_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ds = (DataSet)Session["DataSet"];
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            DataRow row = dataTable.NewRow();
            e.NewValues["TicketNo"] = Session["TicketNo"];
            e.NewValues["BlockID"] = Session["BlockID"];
            e.NewValues["YoP"] = Session["YoP"];
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                if (enumerator.Key.ToString() != "Count")
                    row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            dataTable.Rows.Add(row);

            ds = (DataSet)Session["DataSet"];

            griddetail.DataSource = ds.Tables[0];
            griddetail.DataBind();
            Session["Idx"] = "1";
        }


        protected void griddetail_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            griddetail.JSProperties["cpPopulate"] = e.Parameters;
            Session["BlockID"] = e.Parameters;



            Session["Idx"] = "0";

          
        }

        protected void griddetail_CellEditorInitialize(object sender, BootstrapGridViewEditorEventArgs e)
        {
            Session["Idx"] = "1";
        }
    }
}