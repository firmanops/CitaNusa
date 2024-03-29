﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Receipt.aspx.cs" Inherits="Weighplatation.View.Receipt" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link href="../src/plugins/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="../src/styles/style.css" rel="stylesheet"/>
    <script src="../src/plugins/sweetalert2/sweetalert2.all.js"></script>
    <script src="../src/plugins/sweetalert2/sweetalert2.all.min.js"></script>
    <script src="../src/plugins/sweetalert2/sweet-alert.init.js"></script>
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
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="main-container p-xl-0">
        <div class="pd-ltr-20 xs-pd-20-10">
            <div class="min-height-100px">
                <div class="pd-20 card-box mb-30">
                    <div class="clearfix">
                        <h4 class="text-blue h4">Receipt</h4>
                    </div>
                    <div class="wizard-content">
                        <formview runat="server">
                              <asp:UpdatePanel ID="updatepnl" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
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
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <dx:BootstrapTextBox ID="txtTicketNo" NullText="Ticket No..." Caption="Ticket" runat="server">                                              
                                            </dx:BootstrapTextBox>   
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                             <dx:BootstrapTextBox ID="txtContract" NullText="Contract No..." Caption="Contract No." runat="server">                                              
                                            </dx:BootstrapTextBox>   
                                           <%-- <dx:BootstrapComboBox ID="txtContract" runat="server" DataSourceID="SqlDataSourceContract" 
                                                ValueField="ContractNo" TextField="ContractNo"  Caption="Contract No.">                                                                                                                    
                                            </dx:BootstrapComboBox>  --%> 
                                        </div>
                                    </div>
                                    <asp:SqlDataSource ID="SqlDataSourceContract" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" SelectCommand="select &quot;ContractNo&quot; from public.&quot;WBCONTRACT&quot;"></asp:SqlDataSource>
                                </div>
                                <p></p>
                                <div class="row">
                                     <div class="col-md-12">                                         
                                        <div class="col-md-6 pull-left">                                            
                                            <dx:BootstrapButton runat="server"   ID="btnSearch"  Text="Search" OnClick="btnSearch_Click">
                                                <SettingsBootstrap RenderOption="Primary" />
                                            </dx:BootstrapButton>
                                            <dx:BootstrapButton runat="server"   ID="btnNewWB" OnClick="btnNewWB_Click" Text="Add New">
                                                <SettingsBootstrap RenderOption="Primary" />
                                            </dx:BootstrapButton>
                                             <dx:BootstrapButton runat="server"   ID="btnPosting" OnClick="btnPosting_Click" Text="Completed">
                                                <SettingsBootstrap RenderOption="Success" />
                                            </dx:BootstrapButton>                             
                                            <dx:BootstrapButton runat="server" ID="btnSend" Text="Synchronize" OnClick="btnSend_Click">
                                                <SettingsBootstrap RenderOption="Warning" />                                                       
                                            </dx:BootstrapButton>                                                                                                     
                                    </div>
                                </div>
                                <p></p>
                                <div class="row>">                                                                       
                                    <div class="col-md-12">
                                      <%--  <asp:SqlDataSource ID="SqlDataReceipt" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
                                            SelectCommand="select &quot;ContractNo&quot;  from public.&quot;WBTRX&quot; where &quot;WBStatus&quot; &lt;&gt; 'R'  and &quot;WBType&quot;='Receipt'"></asp:SqlDataSource>--%>

                                        <dx:BootstrapGridView ID="grid" runat="server"  KeyFieldName="TicketNo" Width="100%" AutoGenerateColumns="false" OnDataBinding="grid_DataBinding">
                                             <Settings ShowHeaderFilterButton="True"/>
                                            <Columns>
                                                <dx:BootstrapGridViewDataColumn Width="250px" CssClasses-HeaderCell="centerText" Caption="Action"  VisibleIndex="0" HorizontalAlign="Center">                                                    
                                                    <DataItemTemplate>
                                                        <asp:HyperLink Visible='<%# Eval("WBStatus").ToString() == "Complete" || Eval("WBStatus").ToString() == "Second" ? false : true %>'  CssClass="btn btn-info" runat="server" NavigateUrl='<%# string.Format("ReceiptSecond.aspx?Ticket={0}",
															HttpUtility.UrlEncode(Eval("TicketNo").ToString())) %>'>Weight 2nd</asp:HyperLink>                                                       
                                                        <asp:HyperLink Visible='<%# Eval("WBStatus").ToString() == "First"  ? false : true %>'  CssClass="btn btn-warning" runat="server" NavigateUrl='<%# string.Format("ReceiptView.aspx?Ticket={0}",
															HttpUtility.UrlEncode(Eval("TicketNo").ToString())) %>'>View</asp:HyperLink>
                                                         <asp:HyperLink Visible='<%#   Session["Approval"].ToString() == "true" && Eval("WBStatus").ToString() == "Second" ? true : false%>' CssClass="btn btn-success" runat="server" NavigateUrl='<%# string.Format("ReceiptEdit.aspx?Ticket={0}",
															HttpUtility.UrlEncode(Eval("TicketNo").ToString()))   %>'>Edit</asp:HyperLink>
                                                        <%-- <asp:HyperLink  CssClass="btn btn-danger" runat="server" on>Reject</asp:HyperLink>--%>
                                                    </DataItemTemplate>
                                                </dx:BootstrapGridViewDataColumn>
                                                <dx:BootstrapGridViewDataColumn Caption="Ticket No." FieldName="TicketNo">                                                    
                                                </dx:BootstrapGridViewDataColumn>
                                                <dx:BootstrapGridViewDateColumn Caption="Date" FieldName="Created">
                                                     <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy">  </PropertiesDateEdit>  
                                                </dx:BootstrapGridViewDateColumn>
                                                <dx:BootstrapGridViewDataColumn Caption="Unit Name" FieldName="UnitName" />
                                                <dx:BootstrapGridViewDataColumn Caption="Product Name" FieldName="ProductName" />
                                                <dx:BootstrapGridViewDataColumn Caption="Contract No." FieldName="ContractNo" />
                                                <dx:BootstrapGridViewDataColumn Caption="Business Partner" FieldName="BPName" />
                                                <dx:BootstrapGridViewDataColumn Caption="Vehicle" FieldName="VehicleID" />
                                                 <dx:BootstrapGridViewDataColumn Caption="Weight1st" FieldName="weight1st" /> 
                                                 <dx:BootstrapGridViewDataColumn Caption="Weight2nd" FieldName="weight2nd" /> 
                                                <dx:BootstrapGridViewDataColumn Caption="Status" FieldName="WBStatus" />                                              
                                            </Columns>                                           
                                            <Settings ShowFooter="true" />                                            
                                            <SettingsPager EnableAdaptivity="True" PageSize="5">
                                                <PageSizeItemSettings Position="Right" Visible="True"   />
                                            </SettingsPager>                                            
                                            <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
                                        </dx:BootstrapGridView>
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
   
</asp:Content>
