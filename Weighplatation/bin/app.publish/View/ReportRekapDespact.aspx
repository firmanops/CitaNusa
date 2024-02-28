﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportRekapDespact.aspx.cs" Inherits="Weighplatation.View.ReportRekapDespact" %>
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
        function SetTargetView() {
            document.forms[0].target = "_blank";
        }
    </script>
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
    }
    </style>
    <style>
    table, th, td {
      border: none;
      border-collapse: collapse;
      align-content:center;
    
    }
    </style>
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div class="main-container p-xl-0">
        <div class="pd-ltr-20 xs-pd-20-10">
            <div class="min-height-100px">
                <div class="pd-20 card-box mb-30">
                    <div class="clearfix">
                        <h4 class="text-blue h4">Report Rekapitulasi Despacth</h4>
                    </div>
                    <div class="wizard-content">
                        <formview runat="server">
                              <asp:UpdatePanel ID="updatepnl" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnPrints" />
                                </Triggers>
                                  <ContentTemplate>
                                    <section>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">                                            
                                            <dx:BootstrapDateEdit ID="txtStartDate" NullText="Select Date" Caption="Start Date" runat="server">
                                                <ClientSideEvents Validation="" />                                                
                                                <CssClasses IconDropDownButton="fa fa-calendar" />
                                            </dx:BootstrapDateEdit>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <dx:BootstrapDateEdit ID="txtEndDate" NullText="Select Date" Caption="End Date" runat="server">
                                                <ClientSideEvents Validation="" />
                                                <ValidationSettings ValidationGroup="Validation">
                                                    <RequiredField IsRequired="false" ErrorText="Input Date is required" />
                                                </ValidationSettings>
                                                <CssClasses IconDropDownButton="fa fa-calendar" />

                                            </dx:BootstrapDateEdit>
                                        </div>
                                    </div>
                                </div>                              
                                <p></p>
                                <div class="row">
                                     <div class="col-md-12">                                         
                                        <div class="col-md-6 pull-left">                                            
                                            <dx:BootstrapButton runat="server" ID="btnPrints" Text="Print" OnClick="btnPrints_Click">
                                                <SettingsBootstrap RenderOption="Primary" />
                                                <ClientSideEvents Click="SetTargetView()" />
                                            </dx:BootstrapButton>
                                            <dx:BootstrapButton runat="server"   ID="btnClose" OnClick="btnClose_Click" Text="Close">
                                                <SettingsBootstrap RenderOption="Primary" />
                                            </dx:BootstrapButton>
                                        </div>                                                                                                                       
                                    </div>
                                </div>
                            </section>
                                  </ContentTemplate>
                              </asp:UpdatePanel>            
                        </formview>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- add sweet alert js & css in footer -->
    <script src="src/plugins/sweetalert2/sweetalert2.all.js"></script>
    <script src="src/plugins/sweetalert2/sweet-alert.init.js"></script>
</asp:Content>