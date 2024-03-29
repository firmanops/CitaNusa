﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contract.aspx.cs" Inherits="Weighplatation.View.Contract" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../src/plugins/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="../src/styles/style.css" rel="stylesheet"/>
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
           function ShowContractWindow() {
               newContract.Show();
           }
         
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

           function ShowContractEditWindow() {
               newContract.Show();
           }
       </script>
    <script>
        function OnClick(s, e) {
            //GetRowIndex
            var rowIndex = s.cpContainerIndex;
            console.log(rowIndex);

            //GetRowValues
            grid.GetRowValues(rowIndex, "ContractNo", OnGetRowValues);
            newContract.Show();
        }

        function OnGetRowValues(value) {
            console.log(value);
            var contract = value.GetText();
            alert(value);
            txtcontract.SetText(contract);
        }
    </script>
    <script>
        function OnCustomButtonClick(s, e) {
            OnDetailsClick(grid.GetRowKey(e.visibleIndex))
        }
        function OnDetailsClick(keyValue) {
            newContract.Show();
            newContract.PerformCallback(keyValue);
        }
        function OnEndCallback(s, e) {
            popup.Show();
            var keyValue = grid.cpKeyValue;
            newContract.PerformCallback(keyValue);
        }
    </script>
    <script type="text/javascript">
        txttotalnetwb.GetInputElement().readOnly = true;
    </script>
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="main-container p-xl-0">
        <div class="pd-ltr-20 xs-pd-20-10">
            <div class="min-height-100px">
                <div class="pd-20 card-box mb-30">
                    <div class="clearfix">
                        <h4 class="text-blue h4">Contract</h4>
                    </div>
                    <div class="wizard-content">
                        <formview runat="server">                          
                            <section>                               
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                             <dx:BootstrapTextBox ID="txtSearch" NullText="Search..." Caption="Contract No." runat="server">                                              
                                            </dx:BootstrapTextBox>   
                                        </div>
                                    </div>
                                </div>
                                <p></p>
                                <div class="row>
                                     <%--<div class="col-md-12">--%>                                         
                                        <div class="col-md-12">                                            
                                            <dx:BootstrapButton runat="server"   ID="btnSearch"  Text="Search" OnClick="btnSearch_Click" >
                                                <SettingsBootstrap RenderOption="Primary" />
                                               
                                            </dx:BootstrapButton>
                                            <dx:BootstrapButton runat="server"   ID="btnContract"  Text="Pull Contract Mill" OnClick="btnContract_Click" >
                                                <SettingsBootstrap RenderOption="Primary" />
                                               
                                            </dx:BootstrapButton>
                                            <dx:BootstrapButton runat="server"   ID="btnNew" OnClick="btnNew_Click" Text="Add New"  AutoPostBack="False" UseSubmitBehavior="false">
                                                <SettingsBootstrap RenderOption="Primary" />
                                                <%-- <ClientSideEvents Click="function(s, e) { ShowContractWindow(); }" />--%>
                                            </dx:BootstrapButton>
                                        </div>                                        
                                    <%--</div>--%>
                                </div>
                                <p></p>
                                <div class="row>">                                                                                                         
                                        <dx:BootstrapGridView ID="Grid" runat="server" ClientInstanceName="grid"  KeyFieldName="ContractNo" 
                                            Width="100%" AutoGenerateColumns="False" OnDataBinding="Grid_DataBinding">                                            
                                            <%--<ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick"/>--%>                                          
                                             <Settings ShowHeaderFilterButton="True" />
                                              <Columns>
                                                  <dx:BootstrapGridViewDataColumn Width="250px" CssClasses-HeaderCell="centerText" Caption="Action"  VisibleIndex="0" HorizontalAlign="Center">                                                    
                                                    <DataItemTemplate>                                                        
                                                         <asp:HyperLink  CssClass="btn btn-success" runat="server" NavigateUrl='<%# string.Format("EditContract.aspx?ContractNo={0}",
                                                    HttpUtility.UrlEncode(Eval("ContractNo").ToString()))   %>'>Edit</asp:HyperLink>
                                                    </DataItemTemplate>
                                                </dx:BootstrapGridViewDataColumn>                                              
                                                <dx:BootstrapGridViewTextColumn  Caption="Contract No" FieldName="ContractNo" VisibleIndex="1">
                                                </dx:BootstrapGridViewTextColumn>
                                                <dx:BootstrapGridViewDateColumn  Caption="Contract Date" FieldName="ContractDate" VisibleIndex="2">
                                                </dx:BootstrapGridViewDateColumn>
                                                <dx:BootstrapGridViewDateColumn Caption="Exp. Date" FieldName="ExpDate" VisibleIndex="3">
                                                </dx:BootstrapGridViewDateColumn>
                                                <dx:BootstrapGridViewTextColumn Caption="Product" FieldName="ProductCode" VisibleIndex="4">
                                                </dx:BootstrapGridViewTextColumn>
                                                <dx:BootstrapGridViewTextColumn Caption="Business Partner" FieldName="BPCode" VisibleIndex="5">
                                                </dx:BootstrapGridViewTextColumn>
                                                <dx:BootstrapGridViewTextColumn Caption="Quantity" FieldName="Qty" VisibleIndex="6">
                                                    <PropertiesTextEdit DisplayFormatString="#,#;-#,#;0" ></PropertiesTextEdit>
                                                </dx:BootstrapGridViewTextColumn>
                                                <dx:BootstrapGridViewTextColumn Caption="Unit Price" FieldName="UnitPrice" VisibleIndex="7">
                                                     <PropertiesTextEdit DisplayFormatString="#,#;-#,#;0" ></PropertiesTextEdit>
                                                </dx:BootstrapGridViewTextColumn>
                                                  <dx:BootstrapGridViewTextColumn Caption="TotalPrice" FieldName="TotalPrice" VisibleIndex="8">
                                                     <PropertiesTextEdit DisplayFormatString="#,#;-#,#;0" ></PropertiesTextEdit>
                                                </dx:BootstrapGridViewTextColumn>
                                                <dx:BootstrapGridViewTextColumn Caption="PPN" FieldName="PPN" VisibleIndex="9">
                                                     <PropertiesTextEdit DisplayFormatString="#,#;-#,#;0" ></PropertiesTextEdit>
                                                </dx:BootstrapGridViewTextColumn>
                                                <dx:BootstrapGridViewTextColumn Caption="Final Unit Price" FieldName="FinalUnitPrice" VisibleIndex="10">
                                                     <PropertiesTextEdit DisplayFormatString="#,#;-#,#;0" ></PropertiesTextEdit>
                                                </dx:BootstrapGridViewTextColumn>                                              
                                                <dx:BootstrapGridViewTextColumn Caption="Total Net Weight" FieldName="DespatchQty" VisibleIndex="11">
                                                     <PropertiesTextEdit DisplayFormatString="#,#;-#,#;0" ></PropertiesTextEdit>
                                                </dx:BootstrapGridViewTextColumn>
                                            </Columns>                                   
                                            <Settings ShowFooter="true" />                                            
                                            <SettingsPager EnableAdaptivity="True" PageSize="20">
                                                <PageSizeItemSettings Position="Right" Visible="True"   />
                                            </SettingsPager>                                            
                                           <%-- <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />--%>
                                        </dx:BootstrapGridView>
                                    </div>
                            </section>
                        </formview>
                    </div>
                </div>
            </div>
        </div>
  
    <script src="src/plugins/sweetalert2/sweetalert2.all.js"></script>
    <script src="src/plugins/sweetalert2/sweet-alert.init.js"></script>
</asp:Content>
