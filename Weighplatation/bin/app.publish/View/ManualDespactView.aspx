﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManualDespactView.aspx.cs" Inherits="Weighplatation.View.ManualDespactView" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../src/plugins/sweetalert2.all.min.js"></script>
    <script src="../src/plugins/sweetalert2.min.js"></script>
    <link rel="stylesheet" href="../src/plugins/sweetalert2.min.css" />
     <link href="../src/plugins/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../src/scripts/jquery.min.js"></script>
    <script src="../src/plugins/WebCam.js" type="text/javascript"></script>
    <style>
        .readonly * {
            background-color: white;
        }
    </style>    
   
    <script type="text/javascript">
        function ShowMessage(message, messagetype) {
            var cssclass;
            switch (messagetype) {
                case 'Success':
                    cssclass = 'alert-success'
                    break;
                case 'Error':
                    cssclass = 'alert-danger'
                    break;
                case 'Warning':
                    cssclass = 'alert-warning'
                    break;
                default:
                    cssclass = 'alert-info'
            }
            $('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
        }
    </script>  
    <script type="text/javascript">
        function warningalert() {
            swal.fire({
                title: 'Warning!',
                icon: 'warning',
                text: 'Qty can not zero',
                type: 'warning',

            });
        }
        function successalert(msg) {
            swal.fire({
                title: 'Succes',
                icon: 'success',
                text: msg,
                type: 'success',

            });
        }
        function erroralert(msg) {
            swal.fire({
                title: 'Error!',
                icon: 'error',
                text: msg,
                type: 'error',

            });
        }
    </script>   
    <script type="text/javascript">
        var start;
        function grid_Init(s, e) {
            grid.Refresh();
        }
        function grid_BeginCallback(s, e) {
            start = new Date();
            ClientCommandLabel.SetText(e.command);
            ClientTimeLabel.SetText("working...");
        }
        function grid_EndCallback(s, e) {
            ClientTimeLabel.SetText(new Date() - start);
        }
    </script>
    <script type="text/javascript" language="javascript">
        function Validate(s, e) {
            if (ASPxClientEdit.ValidateGroup('Validation'))
                ClientCallbackPanelDemo.PerformCallback('');
        }
    </script>   
    <style>    
        input[type=button], input[type=submit], input[type=reset] {
        background-color: #3933ff;
        border: none;
        color: white;
        padding: 16px 32px;
        text-decoration: none;
        margin: 4px 2px;
        cursor: pointer;
    </style>
    <style>
    table, th, td {
      border: none;
      border-collapse: collapse;
      align-content:center;
    
    }
    </style>
    <style>
    .customClass input{  
        font-size: 50px;
        height:auto;
        width:400px;
        text-align:right;
    }  
    </style>

    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div class="main-container p-xl-0">
        <div class="pd-ltr-20 xs-pd-20-10">
            <div class="min-height-100px">
                <div class="pd-20 card-box mb-30">
                    <div class="clearfix">
                        <h4 class="text-blue h4">Manual Despacth 2nd</h4>
                    </div>
                    <div class="wizard-content">
                     <section>
                        <formview>                                               
                            <div class="form-group row">                                                        
                                    <div class="col-lg-4 d-flex align-items-stretch">
                                            <div class="card card-box w-100">
                                                <div class="card-body w-100">                                                              
                                                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                                        <tr align="center">
                                                            <th align="center"><u>Manual Despact 1st</u></th>
                                                            <th></th>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <div align="center">
                                                                    <dx:ASPxBinaryImage ID="img1" runat="server"></dx:ASPxBinaryImage>
                                                                </div>                                                                    
                                                            </td>
                                                        </tr>
                                                    </table>
                                                              
                                                </div>
                                            </div>
                                    </div>
                                    <div class="col-lg-4 d-flex align-items-stretch">
                                            <div class="card card-box w-100 ">
                                                <div class="card-body w-100">
                                                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                                        <tr align="center">
                                                            <th align="center"><u>Manual Despact 2nd</u></th>
                                                            <th></th>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <div align="center">
                                                                    <dx:ASPxBinaryImage ID="img2" runat="server"></dx:ASPxBinaryImage>
                                                                </div>                                                                    
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    <div class="col-md-4"> 
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">                                    
                                            <ContentTemplate>
                                                <div class="card card-box">
                                                    <div class="card-body">
                                                        <div class="form-group col-lg-1 pull-right"></div>
                                                            <table Width="100%">
                                                                <tr>
                                                                    <td><h1>1ST</h1></td>
                                                                    <td><asp:TextBox runat="server" CssClass="h1 pull-right" style="width:300px;text-align: right"  ID="txtWB1" ReadOnly="true"></asp:TextBox></td>
                                                                    <td><asp:Label runat="server" CssClass="h1 pull-right" Text="Kg"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><h1>2ND</h1></td>
                                                                    <td><asp:TextBox runat="server" CssClass="h1 pull-right" style="width:300px;text-align: right"  ID="txtWB2" ReadOnly="true"></asp:TextBox></td>
                                                                    <td><asp:Label runat="server" CssClass="h1 pull-right" Text="Kg"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                    </div>
                                                </div>
                                            </ContentTemplate>                                                
                                        </asp:UpdatePanel>
                                    </div>                                                                                                                
                                </div>          
                        </formview>                                       
                    </section>         
                    <section>
                        <formview>
                            <asp:UpdatePanel ID="updatepnl" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">                                    
                                <ContentTemplate>
                                    <div class="pd-20 card-box mb-30">										                                                            
                                        <div class="row">                                                
                                                <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Ticket Number</label>
                                                    <div class="col-sm-10 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtTicketNo" NullText="Commpany Name..."   runat="server" ReadOnly="true">                                                          
                                                        </dx:BootstrapTextBox>
                                                    </div>
                                                </div>
                                                <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Transporter</label>
                                                    <div class="col-sm-10 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtTransporter" NullText="Transporter..."  runat="server" ReadOnly="true">                                                           
                                                    </dx:BootstrapTextBox>
                                                    </div>
                                                </div>
                                        </div>
                                        <div class="row">
                                            <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Contract Number</label>
							                    <div class="col-sm-12 col-md-9">
                                                    <dx:BootstrapTextBox ID="txtContract" NullText="Contract..."  runat="server" ReadOnly="true">                                                           
                                                    </dx:BootstrapTextBox>
							                    </div>
                                            </div>
                                            <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Vehicle</label>
                                                    <div class="col-sm-10 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtVehicle" runat="server" NullText="Vehicle..."  ReadOnly="true">                                                            
                                                        </dx:BootstrapTextBox>
                                                    </div>
                                                </div>
                                            
                                                
                                                
                                        </div>
                                        <div class="row">
                                                <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Company</label>
                                                    <div class="col-sm-10 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtCompanyName" NullText="Commpany Name..."   runat="server" ReadOnly="true">                                                          
                                                        </dx:BootstrapTextBox>
                                                    </div>
                                                </div>
                                                <div class=" col-lg-6 form-group row">
							                        <label class="col-sm-12 col-md-3 col-form-label">Driver</label>
							                        <div class="col-sm-12 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtDriver" NullText="Driver Name..."  runat="server" ReadOnly="true">                                                           
                                                        </dx:BootstrapTextBox>
							                        </div>
                                                </div>
                                        </div>
                                        <div class="row">
                                                <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Unit</label>
                                                    <div class="col-sm-10 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtUnit" NullText="Unit..."  runat="server" ReadOnly="true">                                                           
                                                        </dx:BootstrapTextBox>                                                              
                                                    </div>
                                                </div>
                                                <div class=" col-lg-6 form-group row">
							                        <label class="col-sm-12 col-md-3 col-form-label">Driver Lisense</label>
							                        <div class="col-sm-12 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtLisensiNo" NullText="Lisense No..."  runat="server" ReadOnly="true">                                                                
                                                        </dx:BootstrapTextBox>
							                        </div>
                                                </div>
                                        </div>
                                        <div class="row">
                                                <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Product</label>
                                                    <div class="col-sm-10 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtItem" NullText="Unit..."  runat="server" ReadOnly="true">                                                           
                                                        </dx:BootstrapTextBox>                                                              
                                                    </div>
                                                </div>
                                                <div class=" col-lg-6 form-group row">
							                        <label class="col-sm-12 col-md-3 col-form-label">Transaction Date</label>
							                        <div class="col-sm-12 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtTransactionDate" NullText="Contract..."  runat="server" ReadOnly="true">                                                          
                                                        </dx:BootstrapTextBox>
							                        </div>
                                                </div>
                                        </div>                                                                                
                                    </div>                               
                                    <div class="pd-20 card-box mb-30">                                               
                                                 <div class="row pull-left">
                                                    <div class="col-md-12">
                                                        <div class="float-right">
                                                            <table style="text-align:left">
                                                                <tr style="text-align:left">
                                                                    <td class="p-sm-3">
                                                                        <div class="align-bottom">
                                                                            <dx:BootstrapButton runat="server" ID="btnCancel" Text="Back" OnClick="btnCancel_Click">
                                                                                <SettingsBootstrap RenderOption="Primary" />
                                                                            </dx:BootstrapButton>
                                                                        </div>
                                                                    </td>                                                                    
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <br>
                                                    </div>
                                                </div>       
                                            </div>   
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </formview>
                        <asp:SqlDataSource ID="SqlDataSourceGrading" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
                            SelectCommand="select &quot;GradingTypeID&quot; , &quot;GradingName&quot;  from public.&quot;WBGRADINGTYPE&quot;;"> 
                        </asp:SqlDataSource>
                    </section>                       
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- add sweet alert js & css in footer -->
    <script src="src/plugins/sweetalert2/sweetalert2.all.js"></script>
    <script src="src/plugins/sweetalert2/sweet-alert.init.js"></script>
</asp:Content>