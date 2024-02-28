using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Interface;
using System.Configuration;
using Npgsql;
using System.Data;
namespace Weighplatation.Interface
{
    public interface IReceipt
    {
        List<ReceiptModel> GetAllReceiptByToday();
        ReceiptModelNd GetAllReceiptByTicket(string TicketNo);

        double GetWeight1st(string TicketNo);

        double GetOtherWeight1st(string TicketNo);

        bool PostingReceipt();
        List<WBTRXBLOCK1st> GetAllReceipDetailOneSttByTicket(string TicketNo);
        List<WBTRXGRADING> GetAllReceipDetailSecondNdByTicket(string TicketNo);

        List<ReceiptModel> GetAllReceipByFilter(string TicketNo,string ContractNo, string StartDate, string EndDate);
        List<ReceiptModel> GetAllOtherReceipByFilter(string TicketNo, string StartDate, string EndDate);

        bool InsertReceiptHeader(WBTRXModel listReceipt, List<WBTRXBLOCK> listReceiptDetail);

        bool InsertOtherReceiptHeader(WBTRXETCModel listReceipt, List<WBTRXBLOCK> listReceiptDetail);

        Tuple<bool, List<NetWeightModel>,NetReceiptTotalModel > UpdateReceipt(WBTRXModel listReceipt, List<WBTRXGRADING2nd> ReceiptDetail2Nd);

        Tuple<bool, List<NetWeightModel>, NetReceiptTotalModel> ApprovalReceipt(WBTRXModel listReceipt, List<WBTRXBLOCK> listReceiptDetail, List<WBTRXGRADING2nd> ReceiptDetail2Nd);

        Tuple<bool> ApproveByTicket(WBTRXModel listReceipt);
        Tuple<bool, List<NetWeightModel>, NetReceiptTotalModel> UpdateOtherReceipt(WBTRXETCModel listReceipt, List<WBTRXGRADING2nd> ReceiptDetail2Nd);
        bool CancelReceipt(string TicketNo);

        string GenerateTicketNo(string UnitCode);
        string Check();
        List<VehicleDllModel> GetDllVehicle(string BPCode);

        VehicleDllModel GetDriver(string VehicleId);

        BusinessUnitModel GetUnitByCode(string BPCode);
        string  GetCompanyName(string BPCode);


        bool DeleteBlock(string TicketNo);

        bool DeleteGrading(string TicketNo);

        List<ReceipDetailOneSTtModel> GetDetailReceiptST();

        TranspoterModel GetTransporter(string VehicleId);

        Tuple<List<NetWeightModel>, NetReceiptTotalModel> GetNetWeight(string TicketNo);

        Tuple<List<NetWeightModel>, NetReceiptTotalModel> GetOtherNetWeight(string TicketNo);
         string GetYOP(string BlockId);

        double GetIDGrading();
        double GetIDBlock();



        WBCONTRACT GetWBCONTRACT(string ContractNo);

        List<BusinessUnitModel> GetUnitByContract(string BPCode);

        List<CONTRACTPRODUCT> GetContractProduct(string ContractNo);

        List<WBBLOCKModel> GetBlockUnit(string UnitCode);

        double GetSumQtyDespact(string ContractNo);

        bool CountingPrint(string TicketNo);

        double GetDeducation(string TicketNo);

        List<OtherReceiptModel> GetAllOtherReceiptByToday();

    }
}
