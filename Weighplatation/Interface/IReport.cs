using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weighplatation.Model;


namespace Weighplatation.Interface
{
    public interface IReport
    {
        List<ReceiptModelNd> GetFormReceiptHeader(string TicketNo);
        List<ReceiptModelNd> GetFormOtherReceiptHeader(string TicketNo);

        List<WBTRXBLOCK1st> GetFromReceiptBlock(string TicketNo);

        List<WBTRXGRADING2nd> GetFromReceiptGrading(string TicketNo);
        string GetCompany();

        List<NetWeightModel> GetNetWeightModel (string TicketNo);

        List<NetWeightModel> GetOtherNetWeightModel(string TicketNo);

        NetReceiptTotalModel _NetReceiptTotalModel(string TicketNo);

        List<DespactModelNd> GetFormDespachHeader(string TicketNo);
        List<WBTRXGRADING2nd> GetFromDespachGrading(string TicketNo);
        NetDespactTotalModel _NetDespactTotalModel(string TicketNo);

        List<RptReceiptDtlModel> GetRptReportDetail(string startdate, string finisdate);
        List<RptDespacthDtlModel> GetRptReportDespactDetail(string startdate, string finisdate);
        List<RptRkpRcptMuatanModel> GetRekapReceiptMtn(string startdate, string finisdate);
        List<RptRkpRcptSupplierModel> GetRekapReceiptSpl(string startdate, string finisdate);
        List<RptRkpDespacth> GetRekapDespacth(string startdate, string finisdate);


    }
}
