using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class RptReceiptDtlModel
    {
        public string unitname  { get;set;}
        public DateTime createddt { get; set; }

        public string hourlytime { get;set;}
        public string ticketNo { get;set;}
        public string mode { get;set;}
        public string productname{ get;set;}
        public string vehicleid { get;set;}
        public string BlockID { get;set;}
        public string drivername { get;set;}
        public string yop { get;set;}
        public double weight1st { get;set;}
        public double lfqty { get;set;}
        public double weight2nd { get;set;}
        public double abw { get; set; }
        public double netweight { get;set;}
        public double bunches { get;set;}
        public double deductionqty { get;set;}
        public double ffbwt { get; set; }
        public double estimat { get;set;}
        public double diff   { get;set;}
        public string arrivetime { get;set;}
        public string timein { get;set;}
        public string timeout { get;set;}
        public string waittime { get;set;}
        public string unloadtime { get;set;}
        public string inttnt { get; set; }
        public decimal potongan { get; set; }
        public double nettotal { get; set; }

    }
}