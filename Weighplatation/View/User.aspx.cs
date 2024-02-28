using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using Weighplatation.Model;
using Weighplatation.Repository;
using DevExpress.Web.Bootstrap;
using System.Data;
using DevExpress.Web;
using DevExpress.Data;
using System.Collections;
using Newtonsoft.Json;


namespace Weighplatation.View
{
    public partial class User : System.Web.UI.Page
    {
        UserRepo userRepo = new UserRepo();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            Session["DataSet"] = null;
            try
            {
                if (!IsPostBack || (Session["DataSet"] == null))
                {

                    DataTable userTable = new DataTable();
                    userTable.Columns.Add("userid", typeof(string));
                    userTable.Columns.Add("username", typeof(string));
                    userTable.Columns.Add("password", typeof(string));
                    userTable.Columns.Add("groupid", typeof(int));
                    userTable.Columns.Add("unitcode", typeof(string));
                    userTable.Columns.Add("active", typeof(bool));

                    userTable.PrimaryKey = new DataColumn[] { userTable.Columns["userid"] };


                    //ds.Tables.AddRange(new DataTable[] { userTable });

                    List<SYSUSERMODEL> _sYSUSERMODELs = new List<SYSUSERMODEL>();
                    List<SYSUSERMODEL> _sYSUSERMODELEdit = new List<SYSUSERMODEL>();
                    _sYSUSERMODELs = userRepo.GetUser();

                    foreach (var item in _sYSUSERMODELs)
                    {
                        SYSUSERMODEL sYSUSERMODELRow = new SYSUSERMODEL();
                        sYSUSERMODELRow.userid = item.userid;
                        sYSUSERMODELRow.username = item.username;
                        sYSUSERMODELRow.unitcode = item.unitcode;
                        sYSUSERMODELRow.password = item.password;
                        sYSUSERMODELRow.groupid = item.groupid;
                        sYSUSERMODELRow.active = item.active;
                        _sYSUSERMODELEdit.Add(sYSUSERMODELRow);
                    }

                    ListtoDataTableConverter converter = new ListtoDataTableConverter();
                    userTable = converter.ToDataTable(_sYSUSERMODELEdit);
                    ds.Tables.AddRange(new DataTable[] { userTable });

                    Session["DataSet"] = ds;

                    BootstrapGridView1.DataSource = ds;
                    BootstrapGridView1.DataBind();
                }
                else
                {
                    ds = (DataSet)Session["DataSet"];
                    List<SYSUSERMODEL> sYSUSERMODELs = new List<SYSUSERMODEL>();
                    sYSUSERMODELs = userRepo.GetUser();
                    //foreach (var item in sYSUSERMODELs)
                    //{
                    //    ds.Tables[0].Columns["userid"].DefaultValue = item.userid;
                    //};
                    BootstrapGridView1.DataSource = sYSUSERMODELs;
                    BootstrapGridView1.DataBind();

                }
            }
            catch (Exception err)
            {

                throw  new Exception(err.Message);
            }
            
        }

        protected void BootstrapGridView1_CustomColumnDisplayText(object sender, DevExpress.Web.Bootstrap.BootstrapGridViewColumnDisplayTextEventArgs e)
        {
          
                //if (e.Column.FieldName != "password") return;
                //if (e.Value != null){
                //    e.DisplayText = new  string ('*', e.DisplayText.ToString().Length);
                //}

        }

        protected void BootstrapGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)Session["DataSet"];
            BootstrapGridView gridView = (BootstrapGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            DataRow row = dataTable.NewRow();
            //e.NewValues["userid"] = 0;
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                if (enumerator.Key.ToString() != "Count")
                    row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            dataTable.Rows.Add(row);

            SYSUSERMODEL sYSUSERMODEL = new SYSUSERMODEL();

           
            foreach (DataRow item in dataTable.Rows)
            {
                sYSUSERMODEL.userid = item[0].ToString();
                sYSUSERMODEL.username = item[1].ToString();              
                sYSUSERMODEL.password = item[2].ToString(); 
                sYSUSERMODEL.groupid = int.Parse(item[3].ToString());
                sYSUSERMODEL.unitcode = item[4].ToString();
                sYSUSERMODEL.active= bool.Parse(item[5].ToString());
            }

         

            bool result = userRepo.InsertUser(sYSUSERMODEL);
            List<SYSUSERMODEL> sYSUSERMODELs = new List<SYSUSERMODEL>();
            sYSUSERMODELs = userRepo.GetUser();
            BootstrapGridView1.DataSource = sYSUSERMODELs;
            BootstrapGridView1.DataBind();



        }

        protected void BootstrapGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)Session["DataSet"];
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["userid"] };
            DataRow row = dataTable.Rows.Find(e.Keys[0]);                   
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();          
            enumerator.Reset();
            while (enumerator.MoveNext())
                row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;


            List<SYSUSERMODEL> sYSUSERMODELs = new List<SYSUSERMODEL>();
            SYSUSERMODEL sYSUSERMODEL = new SYSUSERMODEL();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                sYSUSERMODEL.userid = item[0].ToString();
                sYSUSERMODEL.username = item[1].ToString();
                sYSUSERMODEL.password = item[2].ToString();
                sYSUSERMODEL.groupid = int.Parse(item[3].ToString());
                sYSUSERMODEL.unitcode = item[4].ToString();
                sYSUSERMODEL.active = bool.Parse(item[5].ToString());
                sYSUSERMODELs.Add(sYSUSERMODEL);
            }



            bool result = userRepo.UpdatetUser(sYSUSERMODEL);
            
            sYSUSERMODELs = userRepo.GetUser();
            BootstrapGridView1.DataSource = sYSUSERMODELs;
            BootstrapGridView1.DataBind();
           
        }

        protected void BootstrapGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Response.Redirect("/View/User.aspx");
        }
    }
}