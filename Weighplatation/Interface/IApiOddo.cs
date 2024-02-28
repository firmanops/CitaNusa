using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using System.Configuration;
using Npgsql;
using System.Data;

namespace Weighplatation.Interface
{
    public interface IApiOddo
    {
        receiptHeader GetReceiptApiModel(string TicketNo, string[] plant_weighbridge_line, string[] weighbridge_grade_line);
        List<WBTRXBLOCK1st> GetAllReceipDetailOneSttByTicket(string TicketNo);
        List<WBTRXGRADING> GetAllReceipDetailSecondNdByTicket(string TicketNo);

        despactHeader GetDespactApiModel(string TicketNo, string[] weighbridge_quality_line);
        List<WBTRXGRADING> GetAllDespacthDetailSecondNdByTicket(string TicketNo);

        bool CheckForInternetConnection(int timeoutMs, string url = null);

        string Getscale();
        bool Addscale(string scaleno);

        string GetUnitByCode(int idOddo);

        bool UpdateStatusOddo(string TicketNo);

        string SendToOddoReceipt(string TicketNo);
    }
}
