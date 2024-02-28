using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class BusinessModel
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
        public byte bplogo { get; set; }
        public int oddoid { get; set; }
    }
}