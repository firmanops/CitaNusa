using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class WBTRXGRADING
    {
        public Int32 ID { get; set; }
        public string TicketNo { get; set; }
        public string GradingTypeID { get; set; }
        public string GradingName { get; set; }
        public double Quantity { get; set; }
        public string UOM { get; set; }
        public string NoSegel1 { get; set; }
        public string NoSegel2 { get; set; }
    }

    public class WBTRXGRADINGEdit
    {
        public Int32 ID { get; set; }
        public string TicketNo { get; set; }
        public string GradingTypeID { get; set; }
        public double Quantity { get; set; }        
        public string NoSegel1 { get; set; }
        public string NoSegel2 { get; set; }
    }
}