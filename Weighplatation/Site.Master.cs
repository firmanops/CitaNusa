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
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace Weighplatation
{
    public partial class Site : System.Web.UI.MasterPage
    {
       
        UserRepo userRepo = new UserRepo();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["UnitCode"] = "";
            //Session["GroupID"] = "";
            Master.Visible = false;
            Transaction.Visible = false;
          
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!this.IsPostBack)
            {
                DBBUSINESSPARTNER dBBUSINESSPARTNER = new DBBUSINESSPARTNER();
                dBBUSINESSPARTNER = userRepo.GetLogo(Session["UnitCode"].ToString());
                img1.ContentBytes = dBBUSINESSPARTNER.bplogo;
                Session["Reset"] = true;
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
                SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
                int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);
            }
            SchemaUser();
        }

      

        void SchemaUser() {
            try
            {
                int groupid = int.Parse(Session["GroupID"].ToString());
                List<SYSUSERGROUPMENUMODEL> sYSUSERGROUPMENUMODELs = new List<SYSUSERGROUPMENUMODEL>();
                sYSUSERGROUPMENUMODELs = userRepo.GetMenuUser(groupid);
                foreach (var item in sYSUSERGROUPMENUMODELs)
                {
                    SYSMENU sYSMENU = new SYSMENU();
                    sYSMENU = userRepo.GetMenu(item.idmenu);
                    if (sYSMENU != null)
                    {
                        switch (sYSMENU.menuname)
                        {
                            case "Master":
                                Master.Visible = true;
                                break;
                            case "Approval":
                                Session["Approval"] = "true";
                                break;
                            case "Transaction":
                                Transaction.Visible = true;
                                break;
                            default:
                                // code block
                                break;
                        }

                    }

                }
            }
            catch (Exception err)
            {

                MessageError(this, err.Message, "Error");
            }
        }
        protected void MessageSuccess(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal.fire('" + Title + "','" + Message + "','success');", true);
        }

        protected void MessageError(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal.fire('" + Title + "','" + Message + "','error');", true);
        }

        protected void MessageWarning(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "warningalert();", true);
        }
    }
}