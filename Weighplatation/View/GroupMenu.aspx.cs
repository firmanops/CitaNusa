using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weighplatation.Model;
using Weighplatation.Repository;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
namespace Weighplatation.View
{
    public partial class GroupMenu : System.Web.UI.Page
    {
        UserRepo userRepo = new UserRepo();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) {
                GetMenuGroup();
            }
        }

      

        void GetMenuGroup() {
            List<SYSMENU> listGroupMenu = new List<SYSMENU>();

            listGroupMenu = userRepo.GetMenuAll();
            foreach (var item in listGroupMenu)
            {
                lstlft.Items.Add(item.id.ToString() + "-" + item.menuname.ToString());
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string err = "";
                if (cmbGroup.Text == "") {
                    err = "Group is required!!!";
                   
                }

                if (err == "") {
                    int groupid = int.Parse(cmbGroup.Value.ToString());
                    List<SYSUSERGROUPMENUMODEL> listGroupMenu = new List<SYSUSERGROUPMENUMODEL>();

                    // GetSelectedIndices
                    for (int index = 0; index < lstrgt.Items.Count; index++)
                    {
                        string[] arrmenu = lstrgt.Items[index].Value.ToString().Split('-');
                        int idmenu = int.Parse(arrmenu[0].ToString());
                        SYSUSERGROUPMENUMODEL rowGroupMenu = new SYSUSERGROUPMENUMODEL();
                        rowGroupMenu.idgroup = groupid;
                        rowGroupMenu.idmenu = idmenu;
                        listGroupMenu.Add(rowGroupMenu);
                    }

                    bool result = userRepo.InsertGroupMenu(listGroupMenu);
                    if (result)
                    {
                        MessageSuccess(this, "Success!!!", "Success");
                    }
                    else
                    {
                        throw new Exception("Failed Save Data");
                    }
                }
                else
                {
                    throw new Exception(err);
                }
            }
            catch (Exception err)
            {
                MessageError(this, err.Message, "Failed");

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

        protected void btnLeft_Click(object sender, EventArgs e)
        {
            //List will hold items to be removed.
            List<ListItem> removedItems = new List<ListItem>();

            //Loop and transfer the Items to Destination ListBox.
            foreach (ListItem item in lstrgt.Items)
            {
                if (item.Selected)
                {
                    item.Selected = false;
                    lstlft.Items.Add(item);
                    removedItems.Add(item);
                }
            }

            //Loop and remove the Items from the Source ListBox.
            //For Each item As Object In temp
            foreach (ListItem item in removedItems)
            {
                lstrgt.Items.Remove(item);
            }
        }

        protected void btnRight_Click(object sender, EventArgs e)
        {
            //List will hold items to be removed.
            List<ListItem> removedItems = new List<ListItem>();

            //Loop and transfer the Items to Destination ListBox.
            foreach (ListItem item in lstlft.Items)
            {
                if (item.Selected)
                {
                    item.Selected = false;
                    lstrgt.Items.Add(item);
                    removedItems.Add(item);
                }
            }

            //Loop and remove the Items from the Source ListBox.
            foreach (ListItem item in removedItems)
            {
                lstlft.Items.Remove(item);
            }
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            Response.Redirect("../Dashboard.aspx");
        }
    }
}