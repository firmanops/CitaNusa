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
    public interface IDespact
    {
        List<DespactModel> GetAllDespactByToday();
        DespactModelNd GetAllDespactByTicket(string TicketNo);

        //List<WBTRXBLOCK1st> GetAllReceipDetailOneSttByTicket(string TicketNo);

        bool InsertDespactHeader(WBTRXModel listDespact);

        Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> UpdateDespact(WBTRXModel listDespact, List<WBTRXGRADING2nd> DespactDetail2Nd);

        Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> ApproveDespact(WBTRXModel listDespact, List<WBTRXGRADING2nd> DespactDetail2Nd);

        //Tuple<bool, List<NetWeightModel>, NetDespactTotalModel> UpdateDespact(WBTRXModel listDespact, List<WBTRXGRADING2nd> DespactDetail2Nd);

        //bool CancelDespact(string TicketNo);

        string GenerateTicketNo(string UnitCode);

        bool PostingDespacth();

        List<VehicleDllModel> GetDllVehicle(string BPCode);

        VehicleDllModel GetDriver(string VehicleId);

        BusinessUnitModel GetUnitByCode(string BPCode);
        string GetCompanyName(string BPCode);

        CONTRACTPRODUCT GetCompanyContract(string ContractNo);

        List<DOBPCODEModel> GetDONo(string ContractNo);

        WBDOModel GetDONoBPCode(string DONo);

        WBGRADINGTYPE GetGradingTypeByID(string GradingTypeID);

        //List<NetWeightModel> GetNetWeight(string TicketNo);
        WBUOM GetUOM(string UomID);

        WBOWNER GetUnitWBOwner();

        WBCONTRACT GetWBCONTRACT(string ContractNo);

        CONTRACTPRODUCT GetCONTRACTPRODUCT(string ContractNo);

        List<DespactModel> GetAllReceipByFilter(string TicketNo, string ContractNo, string StartDate, string EndDate);

        Tuple<List<NetWeightModel>, NetReceiptTotalModel> GetNetWeight(string TicketNo);

        List<WBTRXGRADING> GetAllDespactDetailSecondNdByTicket(string TicketNo);

        List<WBCONTRACT> GetAllWBCONTRACT();

        double GetSumQtyDespact(string ContractNo);

        bool CountingPrint(string TicketNo);

        string GenerateDONo();

        string Check();

        double GetWeight1st(string TicketNo);
    }
}
