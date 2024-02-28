using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weighplatation.Model;
using Weighplatation.Repository;
namespace WeighingSystem
{
    public partial class Default : System.Web.UI.Page
    {
        public UserRepo _userRepo = new UserRepo();
        

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                SYSUSERMODEL sYSUSERMODEL = new SYSUSERMODEL();
                sYSUSERMODEL = _userRepo.GetUserLogin(txtuser.Text,txtPassword.Text);
                //sYSUSERMODEL = _userRepo.GetUserLogin(txtuser.Text, txtPassword.Text);
                if (sYSUSERMODEL != null)
                {
                    Session["WBSource"] = dllwbSource.Value;
                    Session["UserID"] = txtuser.Text;
                    Session["UserName"] = sYSUSERMODEL.username;
                    Session["GroupID"] = sYSUSERMODEL.groupid;
                    Session["UnitCode"] = sYSUSERMODEL.unitcode.ToString();
                    Session["UnitNameOW"] = _userRepo.GetUnitByCode(sYSUSERMODEL.unitcode.ToString()).UnitName.ToString();
                    Session["Approval"] = "";
                    Response.Redirect("Dashboard.aspx");
                }
                else {
                    throw new Exception("Invalid User or Password...!!!");
                }
            }
            catch (Exception err)
            {
                MessageError(this,err.Message,"Error");

            }
           
        }

        protected void btnSetup_Click(object sender, EventArgs e)
        {
           
        }

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
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

        public static bool CheckForInternetConnection(int timeoutMs = 1000, string url = null)
        {
            try
            {             
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (var response = (HttpWebResponse)request.GetResponse())
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}