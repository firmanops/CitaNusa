using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weighplatation.Repository;
namespace Weighplatation.View
{
    public partial class WBDO : System.Web.UI.Page
    {
        DespactRepo despactRepo = new DespactRepo();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string DONo = despactRepo.GenerateDONo();
            Session["DONo"] = DONo;
        }
    }
}