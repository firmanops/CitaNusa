<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormTicketReceipt.aspx.cs" Inherits="Weighplatation.Report.FormTicketReceipt" %>
<%@ Register Assembly="DevExpress.XtraReports.v23.1.Web.WebForms, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <script type="text/javascript">
    function closeWindow() {
        // Check if the window was opened by the script
        if (window.opener != null && !window.opener.closed) {
            window.opener.postMessage('closeWindow', '*');
        }
        // Close the current window
        window.close();
    }
    </script>
<body>
    <form id="form1" runat="server">
        <div>
             <dx:ASPxWebDocumentViewer ID="ASPxWebDocumentViewer1" runat="server" Height="1591px"></dx:ASPxWebDocumentViewer>
        </div>
    </form>
</body>

</html>
