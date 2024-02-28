<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ContractNew.aspx.cs" Inherits="Weighplatation.View.ContractNew" %>
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
    <script>
        function onSelectedIndexChanged(s, e) {
            grid.PerformCallback(s.GetText());
        }
        function onEndCallback(s, e) {
            if (s.cpPopulate == null || !s.IsEditing())
                return;
            s.SetEditValue('BlocKlID', s.cpPopulate);
        }
    </script>   
    <style> 
      input[type=button], input[type=submit], input[type=reset] {
      background-color: #3933ff;
      border: none;
      color: white;
      padding: 0px 0px;    
      margin: 0px 0px;
      cursor: pointer;
      width:100px;
      height:35px;
      text-align:inherit
    }
    </style>
    <style>
    table, th, td {
      border: none;
      border-collapse: collapse;
      align-content:center;
    
    }
    </style>
    <script type="text/javascript">
        function calculatetotalprice(s, e) {
            var vtxtqty = txtqty.GetText();
            var vtxtunitprice = txtunitprice.GetText();
            var vtxtppn = txtppn.GetText();
            if (vtxtqty != "" && vtxtunitprice != "") {
                txttotalprice.SetText(vtxtqty * vtxtunitprice);
                var vtotalprice = vtxtqty * vtxtunitprice;
                var vppn = vtotalprice * vtxtppn / 100;
                txtfinalprice.SetText(vtotalprice + vppn);
            }
            txttoleransi.SetText(0);
            txtdelivery.SetText("Active");
            txttotalnetwb.SetText(0);
        }

    </script>
        <script type="text/javascript">
            function calculatetotalplusppn(s, e) {             
                var vtxtppn = txtppn.GetText();
                if (vtxtppn != "" ) {                   
                    var vtotalprice = vtxtqty * vtxtunitprice;
                    var vppn = vtotalprice * vtxtppn / 100;
                    txtfinalprice.SetText(vtotalprice + vppn);
                }               
            }
        </script>
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
     <div class="main-container p-xl-0">
        <div class="pd-ltr-20 xs-pd-20-10">
            <div class="min-height-100px">
                <div class="pd-20 card-box mb-30">
                    <div class="clearfix">
                        <h4 class="text-blue h4">New Contract</h4>
                    </div>
                    <div class="wizard-content">
                        <section>
                            <formview runat="server">
                                  <asp:UpdatePanel ID="updatepnl" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">                                
                                      <ContentTemplate>                              
                                            <div class="row">
                                                <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Contract No</label>
                                                <div class="col-sm-12 col-md-9">
                                                        <dx:BootstrapTextBox class="form-control" ID="txtContractNo" NullText="Contract No..."  runat="server">
                                                            <ValidationSettings ValidationGroup="Validation">
                                                                <RequiredField IsRequired="true" ErrorText="Contract is required" />
                                                            </ValidationSettings>
                                                        </dx:BootstrapTextBox>
                                                    </div>
                                                </div>
                                                <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Ref No</label>
                                                <div class="col-sm-12 col-md-9">
                                                        <dx:BootstrapTextBox class="form-control" ID="txtRefNo" NullText="Ref No..."  runat="server">
                                                            <ValidationSettings ValidationGroup="Validation">
                                                                <RequiredField IsRequired="true" ErrorText="Contract is required" />
                                                            </ValidationSettings>
                                                        </dx:BootstrapTextBox>
                                                    </div>
                                                </div>                                                                                     
						                    </div>
                                          <div class="row">
                                            <div class=" col-lg-6 form-group row">
							                <label class="col-sm-12 col-md-3 col-form-label">Product</label>
                                            <div class="col-sm-12 col-md-9">
                                                    <dx:BootstrapComboBox ID="ddlProduct" runat="server" DataSourceID="SqlDataSourceProduct"  ValueField="ProductCode" TextField="ProductName">
                                                            <Items>
                                                            <dx:BootstrapListEditItem Text="" Value="" Selected="true"></dx:BootstrapListEditItem>
                                                        </Items>
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Product is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapComboBox>
                                                        <asp:SqlDataSource ID="SqlDataSourceProduct" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
                                                            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
                                                            SelectCommand="select &quot;ProductCode&quot; ,&quot;ProductName&quot;  from public.&quot;WBPRODUCT&quot;">
                                                        </asp:SqlDataSource>
                                                </div>
                                            </div> 
                                              <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Contract Date</label>
                                                <div class="col-sm-12 col-md-9">                                         
                                                        <dx:BootstrapDateEdit ID="txtContractDate" NullText="Select Date" runat="server">
                                                            <ClientSideEvents Validation="" />                                                
                                                            <CssClasses IconDropDownButton="fa fa-calendar" />
                                                        </dx:BootstrapDateEdit>
                                                    </div>
                                                </div>                                                                                                                                                                                
						                    </div>                                      
                                            <div class="row">
                                                <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Exp. Date</label>
                                                <div class="col-sm-12 col-md-9">                                         
                                                        <dx:BootstrapDateEdit ID="txtExpDate" NullText="Select Date" runat="server">
                                                            <ClientSideEvents Validation="" />                                                
                                                            <CssClasses IconDropDownButton="fa fa-calendar" />
                                                        </dx:BootstrapDateEdit>
                                                    </div>
                                                </div>          
                                                <div class=" col-lg-6 form-group row">
							                        <label class="col-sm-12 col-md-3 col-form-label">Business Partner</label>
							                        <div class="col-sm-12 col-md-9">
								                        <dx:BootstrapComboBox ID="ddlBP" runat="server"  DataSourceID="SqlDataSourceBP"  ValueField="BPCode" TextField="BPName">
                                                            <Items>
                                                                <dx:BootstrapListEditItem Text="" Value="" Selected="true"></dx:BootstrapListEditItem>
                                                            </Items>
                                                            <ValidationSettings ValidationGroup="Validation">
                                                                <RequiredField IsRequired="true" ErrorText="Product is required" />
                                                            </ValidationSettings>
                                                        </dx:BootstrapComboBox>
                                                        <asp:SqlDataSource ID="SqlDataSourceBP" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
                                                            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
                                                            SelectCommand="select &quot;BPCode&quot; ,&quot;BPName&quot;  from public.&quot;BUSINESSPARTNER&quot;">
                                                        </asp:SqlDataSource>
							                        </div>
						                        </div>	                                                                                           					                    
                                            </div>
                                            <div class="row">
                                                <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Quantity</label>
							                        <div class="col-sm-12 col-md-9">
								                        <dx:BootstrapTextBox ID="txtQty" ClientInstanceName="txtqty" DisplayFormatString="#,#;-#,#;0" runat="server" NullText="Qty..." AutoPostBack="false">
                                                            <ValidationSettings ValidationGroup="Validation">
                                                                <RequiredField IsRequired="true" ErrorText="Quantity is required" />
                                                            </ValidationSettings>                                                                    
                                                        </dx:BootstrapTextBox>
							                        </div>							                  
						                        </div>	
                                                <div class=" col-lg-6 form-group row">
							                        <label class="col-sm-12 col-md-3 col-form-label">Toleransi</label>
                                                    <div class="col-sm-10 col-md-9">
                                                     <dx:BootstrapTextBox ID="txtToleransi"  ClientInstanceName="txttoleransi" runat="server" NullText="Toleransi..." >
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Toleransi is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapTextBox>
                                                    </div>
						                        </div>                                                                            
                                            </div>
                                            <div class="row">
                                                <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Unit Price</label>
                                                    <div class="col-sm-12 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtUnitPrice" DisplayFormatString="#,#;-#,#;0" ClientInstanceName="txtunitprice" runat="server" AutoPostBack="false" NullText="Unit Price...">
                                                            <ValidationSettings ValidationGroup="Validation">
                                                                <RequiredField IsRequired="true" ErrorText="UnitPrice is required" />
                                                            </ValidationSettings>
                                                            <ClientSideEvents TextChanged="calculatetotalprice" />
                                                        </dx:BootstrapTextBox>
                                                    </div>
                                                </div>  
                                                <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Total Net. Weight</label>
                                                    <div class="col-sm-10 col-md-9">
                                                       <dx:BootstrapTextBox ID="txtTotalNetWB" DisplayFormatString="#,#;-#,#;0" runat="server" NullText="Total Net. Weight...">                                                       
                                                      </dx:BootstrapTextBox>
                                                    </div>
						                        </div>                                                                                          
                                            </div>                                        
                                            <div class="row">
                                                <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Total Price</label>
                                                    <div class="col-sm-10 col-md-9">
                                                       <dx:BootstrapTextBox ID="txtTotalPrice" ClientInstanceName="txttotalprice" DisplayFormatString="#,#;-#,#;0"  runat="server" NullText="Total Net. Weight...">                                                       
                                                      </dx:BootstrapTextBox>
                                                    </div>
						                        </div>  
                                                <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Delivery Status</label>
                                                    <div class="col-sm-10 col-md-9">
                                                       <dx:BootstrapTextBox ID="txtDeliveryStatus" DisplayFormatString="#,#;-#,#;0" runat="server" NullText="Delivery Status...">                                                       
                                                      </dx:BootstrapTextBox>
                                                    </div>
						                        </div>                                                                                       
                                            </div>
                                            <div class="row">
                                                <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">PPN</label>
                                                    <div class="col-sm-10 col-md-9">
                                                       <dx:BootstrapTextBox ID="txtPPN" DisplayFormatString="#,#;-#,#;0" ClientInstanceName="txtppn" runat="server" NullText="PPN...">  
                                                           <ClientSideEvents  TextChanged="calculatetotalplusppn"/>
                                                      </dx:BootstrapTextBox>
                                                    </div>
						                        </div>     
                                                <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Final Unit Price</label>
                                                    <div class="col-sm-10 col-md-9">
                                                       <dx:BootstrapTextBox ID="txtFinalPrice" ClientInstanceName="txtfinalprice" DisplayFormatString="#,#;-#,#;0" runat="server" NullText="Final Unit Price...">                                                       
                                                      </dx:BootstrapTextBox>
                                                    </div>
						                        </div>                                                                                 
                                            </div>                                        
                                                                                         
                                            </ContentTemplate>    
                                        </asp:UpdatePanel>                                                   
                                    </formview>		
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">                                    
                                <ContentTemplate>
                                    <section>                        
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
                                                                    <td>
                                                                        <div class="align-bottom" horizontalalign="Right">
                                                                            <dx:BootstrapButton runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save">
                                                                                <SettingsBootstrap RenderOption="Primary" />
                                                                                    <ClientSideEvents Click="Validate" />  
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
                        </section>       
                    </div>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
