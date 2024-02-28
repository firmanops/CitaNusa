<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptDespacthDetail.aspx.cs" Inherits="Weighplatation.Report.RptDespacthDetail" %>
<%@ Register Assembly="DevExpress.XtraReports.v23.1.Web.WebForms, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript">
    function onCustomizeExportOptions(s, e) {
        e.HideExportOptionsPanel();
    }
    </script>
    <form id="form1" runat="server">
        <div>
              <dx:ASPxWebDocumentViewer ID="ASPxWebDocumentViewer1" runat="server" Height="1591px" AllowURLsWithJSContent="True" DisableHttpHandlerValidation="False">
                   <ClientSideEvents CustomizeExportOptions="onCustomizeExportOptions"/>
              </dx:ASPxWebDocumentViewer>
        </div>
    </form>
</body>
</html>
