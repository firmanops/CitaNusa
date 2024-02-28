using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class WBTRXBLOCK
    {
        public string TicketNo { get; set; }
        public string BlockID { get; set; }
        public double BunchesQty { get; set; }
        public double LFQty { get; set; }
        public double Estimation { get; set; }
        public double Weight { get; set; }

    }
}