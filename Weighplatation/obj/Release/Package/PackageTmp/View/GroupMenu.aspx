﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GroupMenu.aspx.cs" Inherits="Weighplatation.View.GroupMenu" %>
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
     <script type="text/javascript">
        function AddSelectedItems() {
            MoveSelectedItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            var items = srcListBox.GetSelectedItems();
            for (var i = items.length - 1; i >= 0; i = i - 1) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
          
        }
        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
                lbChoosen.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
            
        }
        function UpdateButtonState() {
            btnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
            btnMoveAllItemsToLeft.SetEnabled(lbChoosen.GetItemCount() > 0);
            btnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
            btnMoveSelectedItemsToLeft.SetEnabled(lbChoosen.GetSelectedItems().length > 0);
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
      <style>
        .container {
            display: table;
        }
        .contentButtons {
            padding-top:20px;
            padding-bottom:10px;
        }
        .button {
                width:100% !important;
        }
        @media(min-width:790px) {
            .contentEditors, .contentButtons {
                display: table-cell;
                width: 33.33333333%;
            }
            .button {
                width:170px !important;
            }
            .contentEditors {
                vertical-align: top;
            }
            .contentButtons {
                vertical-align: bottom;
                text-align: middle;
                position :center;
            }
        }
    </style>
  <%--  <style>
    .vertical-center {
      margin: 0;
      position: absolute;
      top: 50%;
      -ms-transform: translateY(-50%);
      transform: translateY(-50%);
    }
    </style>--%>
   <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
        <div class="pd-20 card-box mb-30">					
		 <div class="main-container p-xl-0">
            <div class="pd-ltr-20 xs-pd-20-10">
                <div class="min-height-100px">                      
                    <formview>
                        <asp:UpdatePanel ID="updatepnl" runat="server">      
                            
                            <ContentTemplate>
                                <div class=" col-lg-6 form-group row">
							        <label class="col-sm-12 col-md-3 col-form-label">Group Member</label>
							        <div class="col-sm-12 col-md-9">
								            <dx:BootstrapComboBox ID="cmbGroup"  AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceGroup" TextField="groupname" ValueField="groupid">
                                        <Items></Items>
                                    </dx:BootstrapComboBox>
                                    <asp:SqlDataSource ID="SqlDataSourceGroup" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
                                        ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
                                        SelectCommand="select &quot;groupid&quot; ,&quot;groupname&quot;  from public.&quot;SYSUSERGROUP&quot;">
                                    </asp:SqlDataSource>
							        </div>
						        </div>               
                                <asp:Table runat="server" CellPadding="3" BorderWidth="0">
                                    <asp:TableRow Width="100%">
                                        <asp:TableCell ColumnSpan="3">
                                                                                                              
                                        </asp:TableCell>                                        
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Left">
                                             <asp:ListBox ID="lstlft"  runat="server" Height="247px" Width="228px"></asp:ListBox>       
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle">
                                             <div>
                                           <dx:BootstrapButton  ID="btnLft" Text="<<" runat="server" OnClick="btnLeft_Click" >   
                                                <SettingsBootstrap RenderOption="Primary" />    
                                            </dx:BootstrapButton>
                                             
                                        </div>                                       
                                        <div class="TopPadding">
                                            <dx:BootstrapButton ID="btnRgt" Text=">>" runat="server" OnClick="btnRight_Click">
                                                    <SettingsBootstrap RenderOption="Primary" />
                                            </dx:BootstrapButton>
                                        </div>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:ListBox  ID="lstrgt" runat="server" SelectionMode="Multiple" Height="247px" Width="228px"></asp:ListBox>
                                        </asp:TableCell>                                        
                                    </asp:TableRow>                                    
                                </asp:Table>
                                 <p></p>
                                         <dx:BootstrapButton   ID="btnSve" runat="server" OnClick="btnSave_Click" Text="Save">
                                                <SettingsBootstrap RenderOption="Primary" />
                                            </dx:BootstrapButton>
                                            <dx:BootstrapButton ID="btnCancel" runat="server" OnClick="btnCancel_Click1" Text="Cancel">
                                                <SettingsBootstrap RenderOption="Primary" />
                                            </dx:BootstrapButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                     </formview>
                    </div>            
			  
            </div>
        </div>
        </div>
</asp:Content>
