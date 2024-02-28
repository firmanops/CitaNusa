using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class ReceiptModelNd
    {
        public string TicketNo { get; set; }
        public DateTime Created { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string ProductName { get; set; }
        public string ContractNo { get; set; }
        public string LetterNo { get; set; }
        public string BPName { get; set; }
        public string VehicleID { get; set; }
        public string DriverName { get; set; }
        public string Transporter { get; set; }
        public string WBStatus { get; set; }
        public string Lisense { get; set; }
        public double Weight1st { get; set; }
        public double Weight2nd { get; set; }
        public byte[] WBImagefront1 { get; set; }
        public byte[] WBImagefront2 { get; set; }
        public int Print { get; set; }
        public double Potongan { get; set; }

        public string RefNo { get; set; }

    }
}