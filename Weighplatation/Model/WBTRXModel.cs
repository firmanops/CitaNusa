using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class WBTRXModel
    {
        public string TicketNo { get; set; }
        public string WBSOURCE { get; set; }
        public string WBType { get; set; }
        public DateTime TrxDateIn { get; set; }
        public DateTime TrxDateOut { get; set; }
        public string UnitCode { get; set; }
        public string NoCoverLetter { get; set; }
        public string ProductCode { get; set; }
        public string ContractNo { get; set; }
        public string DONo { get; set; }
        public string VehicleID { get; set; }
        public string DriverName { get; set; }
        public string LicenseNo { get; set; }
        public double Weight1ST { get; set; }
        public double Weight2ND { get; set; }
        public string WBFlag1 { get; set; }
        public string WBFlag2 { get; set; }
        public string WBStatus { get; set; }
        public byte[] WBImagefront1 { get; set; }
        public byte[] WBImageBack1 { get; set; }
        public byte[] WBImagefront2 { get; set; }
        public byte[] WBImageBack2 { get; set; }
        public string UserIDWeight1ST { get; set; }
        public string UserIDWeight2ND { get; set; }
        public string UserIDApproval { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public bool StatusOddo { get; set; }
    }

    public class WBTRXETCModel
    {
        public string TicketNo { get; set; }
        public string WBSOURCE { get; set; }
        public string WBType { get; set; }
        public DateTime TrxDateIn { get; set; }
        public DateTime TrxDateOut { get; set; }
        public string UnitCode { get; set; }
        public string NoCoverLetter { get; set; }
        public string ProductCode { get; set; }
       // public string ContractNo { get; set; }
        //public string DONo { get; set; }
        public string VehicleID { get; set; }
        public string DriverName { get; set; }
        public string LicenseNo { get; set; }
        public double Weight1ST { get; set; }
        public double Weight2ND { get; set; }
        public string WBFlag1 { get; set; }
        public string WBFlag2 { get; set; }
        public string WBStatus { get; set; }
        public byte[] WBImagefront1 { get; set; }
        public byte[] WBImageBack1 { get; set; }
        public byte[] WBImagefront2 { get; set; }
        public byte[] WBImageBack2 { get; set; }
        public string UserIDWeight1ST { get; set; }
        public string UserIDWeight2ND { get; set; }
        public string UserIDApproval { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool StatusOddo { get; set; }

       
    }




    public class ReceiptModel
    {
        public string TicketNo { get; set; }
        public DateTime Created { get; set; }
        public string UnitName { get; set; }
        public string ProductName { get; set; }
        public string ContractNo { get; set; }
        public string BPName { get; set; }
        public string VehicleID { get; set; }
        public string WBStatus { get; set; }

        public string weight1st { get; set; }

        public string weight2nd { get; set; }

        public string status { get; set; }

    }

    public class OtherReceiptModel
    {
        public string TicketNo { get; set; }
        public DateTime Created { get; set; }
        public string UnitName { get; set; }
        public string ProductName { get; set; }       
        public string BPName { get; set; }
        public string VehicleID { get; set; }
        public string WBStatus { get; set; }

    }

    public class NetReceiptTotalModel
    {
        public string TicketNo { get; set; }
        public double Quantity { get; set; }
    }
    public class VehicleDllModel
    {
        public string VehicleID { get; set; }
        public string DriverName { get; set; }
        public string BPCode { get; set; }
        public string LicenseNo { get; set; }
    }

    public class TranspoterModel
    {
        public string VehicleID { get; set; }
        public string DriverName { get; set; }
        public string BPCode { get; set; }
        public string BPName { get; set; }
    }

    public class BusinessUnitModel
    {
        public string UnitCode { get; set; }
        public string UnitName { get; set; }

        public string BPCode { get; set; }

        public string UnitType { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Postalcode { get; set; }
        public bool Active { get; set; }
        public int oddoid { get; set; }
    }

    public class NetWeightModel
    {
        public string Weight { get; set; }
        public string DateTransaction { get; set; }
        public string Hours { get; set; }
        public string Mode { get; set; }
        public double WeightHeavy { get; set; }
    }

    public class WBBLOCKModel {
        public string BlockID { get; set; }

        public string UnitCode { get; set; }

        public int MoP { get; set; }

        public int YoP { get; set; }

        public double CurrentPlanted { get; set; }

        public bool Active { get; set; }
    }

    public class DespactModel
    {
        public string TicketNo { get; set; }
        public DateTime Created { get; set; }
        public string UnitName { get; set; }
        public string ProductName { get; set; }
        public string ContractNo { get; set; }
        public string BPName { get; set; }
        public string VehicleID { get; set; }
        public string WBStatus { get; set; }
        public string weight1st { get; set; }
        public string weight2nd { get; set; }
        public string status { get; set; }
    }

   

    public class DespactDetailOneSTtModel
    {
        public string TicketNo { get; set; }
        public string BlockID { get; set; }
        public string Divison { get; set; }
        public string YoP { get; set; }
        public Double Bunches { get; set; }
        public Double LFQty { get; set; }
        public Double Estimation { get; set; }
        public Double Weight { get; set; }
    }

    public class NetDespactTotalModel
    {
        public string TicketNo { get; set; }
        public double Quantity { get; set; }
    }

    public class DOBPCODEModel
    {
        public string DoNo { get; set; }

        public string DoCompany { get; set; }
        public string BPCode { get; set; }
    }

    public class CONTRACTPRODUCT
    {
        public string BPCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

    }

    public class WBGRADINGTYPE
    {
        public string GradingTypeID { get; set; }
        public string GradingName { get; set; }
        public string UomID { get; set; }
        public string ProductCode { get; set; }
        public double DeductionsValue { get; set; }
        public bool Status { get; set; }

    }


    public class WBUOM
    {
        public string UomID { get; set; }
        public string UomName { get; set; }
    }

    public class WBOWNER
    {
        public string UnitCode { get; set; }
        public string UnitName { get; set; }

        public string MillManager { get; set; }

        public string MillKTU { get; set; }

    }

    public class WBCONTRACT
    {
        public string ContractNo { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime ExpDate { get; set; }
        public string ProductCode { get; set; }
        public string BPCode { get; set; }
        public double Qty { get; set; }
        public double Toleransi { get; set; }
        public double UnitPrice { get; set; }
        public double PremiumPrice { get; set; }
        public double PPN { get; set; }
        public double FinalUnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public double DespatchQty { get; set; }
        public string DeliveryStatus { get; set; }
    }

    public class WBDOModel
    {
        public string DONo { get; set; }

        public string ContractNo { get; set; }

        public DateTime DODate { get; set; }
        public string BPCode { get; set; }
        public double Qty { get; set; }
        public double DespatchQty { get; set; }
        public string DeliveryStatus { get; set; }
    }
   
}