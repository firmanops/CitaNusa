using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class PortModel
	{
        public string WBSOURCE { get; set; }
		public string Description { get; set; }
        public string ComPort { get; set; }
        public double Bautrate { get; set; }
        public double DataBits { get; set; }
        public double StopBits { get; set; }
        public string Parity { get; set; }
    }
}