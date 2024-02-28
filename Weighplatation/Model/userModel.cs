using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class SYSUSERMODEL
    {
        public string userid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int groupid { get; set; }
        public string unitcode { get; set; }
        public bool active { get; set; }
    }

    public class SYSUSERGROUPMODEL
    {
         public int groupid { get; set; }
        public string groupname { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
    }

    public class SYSUSERGROUPMENUMODEL
    {
        public int id { get; set; }
        public int idgroup { get; set; }
        public int idmenu { get; set; }
        public bool active { get; set; }
    }

    public class SYSMENU
    {
        public int id { get; set; }
        public string menuname { get; set; }
        public string description { get; set; }      
    }


    public class DBBUSINESSPARTNER
    {
        public string BPCode { get; set; }
        public string BPName { get; set; }
        public string BPType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Postalcode { get; set; }
        public string TaxID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PICName { get; set; }
        public bool Active { get; set; }
        public byte[] bplogo { get; set; }
    }
}
