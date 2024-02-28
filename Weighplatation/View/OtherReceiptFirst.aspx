<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OtherReceiptFirst.aspx.cs" Inherits="Weighplatation.View.OtherReceiptFirst" %>
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
    <%--<script type="text/javascript">
        $(function () {
            Webcam.set({
                width: 150,
                height: 100,
                image_format: 'jpeg',
                jpeg_quality: 90
            });
            Webcam.attach('#webcam');
            $("#btnCapture").click(function () {
                Webcam.snap(function (data_uri) {
                    $("#imgCapture")[0].src = data_uri;
                });
                $.ajax({
                    type: "POST",
                    url: "ReceiptFirst.aspx/SaveCapturedImage",
                    data: "{data: '" + $("#imgCapture")[0].src + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) { }
                });
            });
        });
    </script>--%>
    <script type="text/javascript">
        $(function () {
            Webcam.set({
                width: 150,
                height: 100,
                image_format: 'jpeg',
                jpeg_quality: 90
            });
            Webcam.attach('#webcam');
            $("#btnCapture").click(function () {
                Webcam.snap(function (data_uri) {
                    $("#imgCapture")[0].src = data_uri;
                });
                $.ajax({
                    type: "POST",
                    url: "ReceiptFirst.aspx/SaveCapturedImage",
                    data: "{data: '" + $("#imgCapture")[0].src + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) { }
                });
            });
        });
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
    <script>  
        function showhide() {
            var div = document.getElementById("webcam");
            var divc = document.getElementById("capture");
            var btncapture = document.getElementById("btnCapture");
            var btnreset = document.getElementById("btnReset");
            if (div.style.display !== "none") {
                div.style.display = "none";
                divc.style.display = "block";
                btnreset.style.display = "block";
                btncapture.style.display = "none";
            }
            else {
                div.style.display = "block";
                btncapture.style.display = "block";
                divc.style.display = "none";
                btnreset.style.display = "none";

            }
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
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    
    <div class="main-container p-xl-0">
        <div class="pd-ltr-20 xs-pd-20-10">
            <div class="min-height-100px">
                <div class="pd-20 card-box mb-30">
                    <div class="clearfix">
                        <h4 class="text-blue h4">Other Receipt 1st</h4>
                    </div>
                    <div class="wizard-content">                       
                            <section>
                                <formview>                                               
                                    <div class="form-group row">                                                        
                                            <div class="col-lg-4 d-flex align-items-stretch">
                                                    <%--<h5 class="card-header weight-500">Cam 2</h5>--%>
                                                    <div class="card card-box w-100 h-30">
                                                        <div class="card-body w-100 h-30">                                                              
                                                            <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                                                <tr align="center">
                                                                    <th align="center"><u>FRONT</u></th>
                                                                    <th></th>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><div id="webcam" align="center"></div>
                                                                        <div id="capture" style="display:block"> 
                                                                            <image id = "imgCapture" />
                                                                        </div>           
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align = "center">
                                                                        <input type="button" id="btnCapture" value="Capture" style="font-size:small; text-align:center"  onclick="showhide()" />
                                                                        <input type="button" id="btnReset" style="display:none" value="Reset" onclick="showhide()"/>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                              
                                                        </div>
                                                    </div>                                              
                                            </div>
                                            <div class="col-lg-4 d-flex align-items-stretch">
                                                    <%--<h5 class="card-header weight-500">Cam 2</h5>--%>
                                                    <div class="card card-box w-100 h-30" ">
                                                        <div class="card-body w-100 h-30"">
                                                            <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                                                <tr align="center">
                                                                        <th align="center"><u>BACK</u></th>
                                                                    <td>
                                                                        <div id="webcam2"></div>
                                                                    </td>
                                                                    <td align="center">
                                                                        <image id="imgCapture2" />
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
                                                                            <td><asp:TextBox runat="server" CssClass="h1 pull-right" style="width:350px;text-align: right"  ID="txtWB"  >0</asp:TextBox></td>
                                                                            <td><asp:Label runat="server" CssClass="h1 pull-right" Text="Kg"></asp:Label></td>
                                                                        </tr>
                                                                        <tr  align="right">
                                                                            <td> <dx:BootstrapButton runat="server" ID="btnWB" OnClick="btnWB_Click" Text="Get WB">
                                                                                    <SettingsBootstrap RenderOption="Primary" />                                                                           
                                                                                </dx:BootstrapButton></td>
                                                                            </tr>
                                                                    </table>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                     <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnWB" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>                                                                                                                
                                        </div>                                        
                                </formview>                                       
                            </section> 

                            <formview>
                                <asp:UpdatePanel ID="updatepnl" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">                                    
                                <ContentTemplate>
                                    <section>                        
                                        <div class="pd-20 card-box mb-30">										            
                                        <div class="row">
                                            <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Ticket No</label>
                                                <div class="col-sm-12 col-md-9">
                                                    <dx:BootstrapTextBox class="form-control" ID="txtTicketNo" NullText="Ticket..."  runat="server" ReadOnly="true">
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Ticket is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapTextBox>
                                                </div>
						                    </div>
                                             <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Transporter</label>
                                                    <div class="col-sm-12 col-md-9">
                                                        <dx:BootstrapTextBox ID="txtTransporter" NullText="Transporter..." ReadOnly="true" runat="server">
                                                            <ValidationSettings ValidationGroup="Validation">
                                                                <RequiredField IsRequired="true" ErrorText="Transporter is required" />
                                                            </ValidationSettings>
                                                        </dx:BootstrapTextBox>
                                                    </div>
                                            </div>	
                                        </div>
						                <div class="row">
                                            <%-- <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Contract Number</label>
                                                <div class="col-sm-12 col-md-9">
                                                    <dx:BootstrapComboBox ID="ddlContract" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceContract" OnSelectedIndexChanged="ddlContract_SelectedIndexChanged" ValueField="ContractNo" TextField="ContractNo">
                                                        <Items>
                                                            <dx:BootstrapListEditItem Text="" Value="" Selected="true"></dx:BootstrapListEditItem>
                                                        </Items>
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Contract Number is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapComboBox>
                                                    <asp:SqlDataSource ID="SqlDataSourceContract" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
                                                        SelectCommand="select a.&quot;ContractNo&quot;  from  public.&quot;WBCONTRACT&quot; a
                                                                    inner join public.&quot;BUSINESSPARTNER&quot; b on a.&quot;BPCode&quot; =b.&quot;BPCode&quot; 
                                                                    where b.&quot;BPType&quot; ='1' and a.&quot;DeliveryStatus&quot;='1'
                                                                    and &quot;DespatchQty&quot; &lt; &quot;Qty&quot; and &quot;ExpDate&quot; &gt;= now() ">
                                                    </asp:SqlDataSource>
                                                </div>
                                            </div>--%>
                                            <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Unit</label>
							                    <div class="col-sm-12 col-md-9">                                           
								                    <dx:BootstrapComboBox ID="ddlUnit" AutoPostBack="true" DataSourceID="SqlDataSourceUnit"  OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged"  TextField="UnitName" ValueField="UnitCode" runat="server">
                                                        <Items>
                                                            <dx:BootstrapListEditItem Text="" Value="" Selected="true"></dx:BootstrapListEditItem>
                                                        </Items>                                                           
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Unit Name is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapComboBox>                                          
                                                    <asp:SqlDataSource ID="SqlDataSourceUnit" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
                                                        SelectCommand="select a.&quot;UnitCode&quot;, a.&quot;UnitName&quot; from public.&quot;BUSINESSUNIT&quot; a
                                                        inner join public.&quot;BUSINESSPARTNER&quot; b on a.&quot;BPCode&quot; = b.&quot;BPCode&quot;
                                                        where &quot;UnitCode&quot; not in (select &quot;UnitCode&quot; from  public.&quot;WBOWNER&quot; )
                                                        and b.&quot;oddoid&quot;=0"></asp:SqlDataSource>
							                    </div>
						                    </div>
                                            <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Vehicle</label>
							                    <div class="col-sm-12 col-md-9">
								                    <dx:BootstrapComboBox ID="ddlVehicle" runat="server" TextField="VehicleId" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicle_SelectedIndexChanged" ValueField="VehicleId">
                                                        <Items>
                                                            <dx:BootstrapListEditItem Text="" Value="" Selected="true"></dx:BootstrapListEditItem>
                                                        </Items>                                                            
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Vehicle is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapComboBox>
							                    </div>
						                    </div>                                        						                
                                        </div>
                                        <div class="row">
                                            <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Supplier Name</label>
							                    <div class="col-sm-12 col-md-9">
								                    <dx:BootstrapTextBox ID="txtCompanyName" NullText="Commpany Name..."  runat="server" ReadOnly="true">
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Ticket is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapTextBox>
							                    </div>
						                    </div>
                                            <div class=" col-lg-6 form-group row">
                                                <label class="col-sm-12 col-md-3 col-form-label">Driver</label>
                                                <div class="col-sm-12 col-md-9">
                                                    <dx:BootstrapTextBox ID="txtDriver" NullText="Driver Name..."  runat="server">
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Driver Name is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapTextBox>
                                                </div>
                                            </div>
                                        </div>                                    
                                        <div class="row">
                                           <div class=" col-lg-6 form-group row">
							                    <label class="col-sm-12 col-md-3 col-form-label">Product</label>
                                                    <div class="col-sm-10 col-md-9">
                                                       <%-- <dx:BootstrapTextBox ID="txtProductName" NullText="Product..."   runat="server" ReadOnly="true">
                                                    
                                                        </dx:BootstrapTextBox>
                                                        <dx:BootstrapTextBox ID="txtProductCode"    runat="server" Visible="false">
                                                    
                                                        </dx:BootstrapTextBox>--%>
                                                        <dx:BootstrapComboBox  class="form-control" ID ="ddlItem" DataSourceID="SqlDataSourceWBItem" runat="server" ValueField="ProductCode" TextField="ProductName">
                                                            <Items>
                                                                <dx:BootstrapListEditItem Text="" Value="" Selected="true"></dx:BootstrapListEditItem>
                                                            </Items>
                                                            <ValidationSettings ValidationGroup="Validation">
                                                                <RequiredField IsRequired="true" ErrorText="WB Item is required" />
                                                            </ValidationSettings>                                                            
                                                        </dx:BootstrapComboBox>
                                                    </div>
                                                    <asp:SqlDataSource ID="SqlDataSourceWBItem" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
                                                    SelectCommand="select &quot;ProductCode&quot;, &quot;ProductName&quot; from public.&quot;WBPRODUCT&quot;  where oddoid=0"></asp:SqlDataSource>
						                    </div>
                                            <div class=" col-lg-6 form-group row">
                                                    <label class="col-sm-12 col-md-3 col-form-label">Driver Lisense</label>
                                                <div class="col-sm-12 col-md-9">
                                                    <dx:BootstrapTextBox ID="txtLisensiNo" NullText="Lisense No..."  runat="server">
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Driver Name is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapTextBox>
                                                </div>
                                            </div>
                                           
                                            </div>
                                          <div class="row">   
                                               <div class=" col-lg-6 form-group row">
                                                <label class="col-sm-12 col-md-3 col-form-label">Transaction Date</label>
                                                <div class="col-sm-12 col-md-9">
                                                    <dx:BootstrapTextBox ID="txtTransactionDate" NullText="TransactionDate..." DisplayFormatString="dd-mm-yyyy"  runat="server" ReadOnly="true">
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Driver Name is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapTextBox>
                                            </div>
                                            </div>
                                            <div class=" col-lg-6 form-group row">
                                                <label class="col-sm-12 col-md-3 col-form-label">Letter No.</label>
                                                <div class="col-sm-12 col-md-9">
                                                    <dx:BootstrapTextBox ID="txtLetter" NullText="No CoverLetter..."  runat="server">
                                                        <ValidationSettings ValidationGroup="Validation">
                                                            <RequiredField IsRequired="true" ErrorText="Letter No. is required" />
                                                        </ValidationSettings>
                                                    </dx:BootstrapTextBox>
                                            </div>
                                            </div>
                                            </div>                
				                        </div>
                                        <div class="pd-20 card-box mb-30">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <dx:BootstrapPageControl runat="server" TabAlign="Justify" ActiveTabIndex="0">
                                                        <TabPages>
                                                            <dx:BootstrapTabPage Text="Receipt 1st" ActiveTabIconCssClass="fa fa-check">
                                                                <ContentCollection>
                                                                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">                                                           
                                                                                <dx:BootstrapGridView 
                                                                                    ID="griddetail" 
                                                                                    runat="server" 
                                                                                    ClientInstanceName="grid"  
                                                                                    OnCustomCallback="griddetail_CustomCallback" 
                                                                                    KeyFieldName="ID"   
                                                                                    OnRowInserting="griddetail_RowInserting"
                                                                                    OnRowDeleting="griddetail_RowDeleting"
                                                                                    OnRowUpdating="griddetail_RowUpdating"
                                                                                    OnCellEditorInitialize="griddetail_CellEditorInitialize"                                                                                  
                                                                                    OnBeforePerformDataSelect="griddetail_BeforePerformDataSelect"
                                                                                    OnCustomButtonCallback="griddetail_CustomButtonCallback">                                                                                      
                                                                                    <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
                                                                                    <Settings ShowHeaderFilterButton="True" /> 
                                                                                    <SettingsEditing Mode="Inline"/>  
                                                                                    <SettingsCommandButton>
                                                                                        <NewButton IconCssClass="fa fa-plus-circle" />
                                                                                        <UpdateButton IconCssClass="fa fa-save"  Text="Save" />
                                                                                        <CancelButton IconCssClass="fa fa-close"  />
                                                                                        <EditButton IconCssClass="fa fa-edit" />
                                                                                        <DeleteButton IconCssClass="fa fa-trash-o" />
                                                                                    </SettingsCommandButton>
                                                                                        <Columns>
                                                                                        <dx:BootstrapGridViewTextColumn FieldName="TicketNo"  VisibleIndex="0"  Visible="false">
                                                                                        </dx:BootstrapGridViewTextColumn>
                                                                                        <dx:BootstrapGridViewCommandColumn Caption="Action" ShowDeleteButton="True" ShowEditButton="True"  ShowNewButtonInHeader="True" VisibleIndex="1"></dx:BootstrapGridViewCommandColumn>                                                                                        
                                                                                            <dx:BootstrapGridViewComboBoxColumn   FieldName="BlockID"  Visible="true" VisibleIndex="2">
                                                                                                <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                                                                                                  DataSourceID="SqlDataSourceWBBlock"  TextField="BlockID" ValueField="BlockID">
                                                                                                <clientsideevents SelectedIndexChanged="onSelectedIndexChanged" /> 
                                                                                                </PropertiesComboBox>                                                                                                
                                                                                            </dx:BootstrapGridViewComboBoxColumn>                                                                                               
                                                                                            <dx:BootstrapGridViewTextColumn Caption="Year of Planting" FieldName="YoP"  VisibleIndex="3">
                                                                                                <PropertiesTextEdit>
                                                                                                    <ValidationSettings>
                                                                                                        <RequiredField IsRequired="True" ErrorText="Please input Year Of Plantation"  />
                                                                                                    </ValidationSettings>                                                                                                    
                                                                                                </PropertiesTextEdit>  
                                                                                                <EditItemTemplate>
                                                                                                    <dx:BootstrapTextBox ID="YoP"  runat="server"></dx:BootstrapTextBox>
                                                                                                </EditItemTemplate>
                                                                                            </dx:BootstrapGridViewTextColumn>                                                                                                                                                                                                          
                                                                                            <dx:BootstrapGridViewTextColumn Caption="Bunches (JJG)" FieldName="BunchesQty" VisibleIndex="4">
                                                                                               <%-- <PropertiesTextEdit>
                                                                                                    <ValidationSettings>
                                                                                                        <RequiredField IsRequired="True" ErrorText="Please input Bunches"  />
                                                                                                    </ValidationSettings>
                                                                                                </PropertiesTextEdit>--%>
                                                                                            </dx:BootstrapGridViewTextColumn>
                                                                                            <dx:BootstrapGridViewTextColumn Caption="Lose Fruit (Kg)" FieldName="LFQty" VisibleIndex="5">
                                                                                                <%--<PropertiesTextEdit>
                                                                                                    <ValidationSettings>
                                                                                                        <RequiredField IsRequired="True" ErrorText="Please input Lost Fruit"  />
                                                                                                    </ValidationSettings>
                                                                                                </PropertiesTextEdit>--%>
                                                                                            </dx:BootstrapGridViewTextColumn>
                                                                                            <dx:BootstrapGridViewTextColumn Caption="Estimation (Kg)" FieldName="Estimation" VisibleIndex="6">
                                                                                               <%-- <PropertiesTextEdit>
                                                                                                    <ValidationSettings>
                                                                                                        <RequiredField IsRequired="True" ErrorText="Please input Estimation"  />
                                                                                                    </ValidationSettings>
                                                                                                </PropertiesTextEdit>--%>
                                                                                            </dx:BootstrapGridViewTextColumn>
                                                                                    </Columns>             
                                                                                        <ClientSideEvents EndCallback="onEndCallback" />
                                                                                   <%-- <SettingsPager NumericButtonCount="4" PageSize="2">                                                                                        
                                                                                        <PageSizeItemSettings Visible="true" Items="2" />
                                                                                    </SettingsPager>--%>
                                                                                </dx:BootstrapGridView>
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
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:BootstrapTabPage>
                                                        </TabPages>
                                                    </dx:BootstrapPageControl>                                                          
                                                </div>
                                            </div>                                   
                                        </div>                                                                         
                                    </section>    
                                </ContentTemplate>
                                <%--<Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        </Triggers>--%>
                                    </asp:UpdatePanel>        
					        </formview>					   
                                <asp:SqlDataSource ID="SqlDataSourceWBBlock" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
                                    SelectCommand="select *  from public.&quot;WBBLOCK&quot; where &quot;UnitCode&quot; = @UnitCode ">
                                    <SelectParameters>
                                         <asp:SessionParameter Name="UnitCode" SessionField="UnitCodeBlock" />  
                                    </SelectParameters>
                                </asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
