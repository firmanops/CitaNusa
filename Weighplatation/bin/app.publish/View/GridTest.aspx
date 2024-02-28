<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GridTest.aspx.cs" Inherits="Weighplatation.View.GridTest" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--<script type="text/javascript">
    function RetriveValues ()
    {
     if (catCombo != null)
        {
           var parameter = catCombo.GetValue().toString();
         grid.PerformCallback(parameter);

        }
    }
  </script>
    <script type="text/javascript">
        function our_function(s, e) {
            tb.SetText(s.GetSelectedItem().text);
        }
    </script>--%>

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
     <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
   <asp:UpdatePanel runat="server" ID="updatepnl1">
            <ContentTemplate>
                <div class="row">
                    <asp:SqlDataSource ID="SqlDataSourceWBBlock" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
                                            SelectCommand="select &quot;BlockID&quot;, &quot;YoP&quot;  from public.&quot;WBBLOCK&quot;;"> 
                                        </asp:SqlDataSource>
                    <dx:BootstrapGridView 
                        ID="griddetail" 
                        runat="server" 
                        ClientInstanceName="grid"  
                        OnCustomCallback="griddetail_CustomCallback" 
                        KeyFieldName="TicketNo"   
                        OnRowInserting="griddetail_RowInserting"
                        OnCellEditorInitialize="griddetail_CellEditorInitialize"
                        >                                                                                   
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
                                <dx:BootstrapGridViewComboBoxColumn  HorizontalAlign="Center"   FieldName="BlockID"  Visible="true" VisibleIndex="1">
                                    <PropertiesComboBox            
                                        DataSourceID="SqlDataSourceWBBlock" TextField="BlockID" ValueField="BlockID">
                                    <clientsideevents SelectedIndexChanged="onSelectedIndexChanged" />             
                                    </PropertiesComboBox>                                     
                                </dx:BootstrapGridViewComboBoxColumn>                                                                                               
                                <dx:BootstrapGridViewTextColumn Caption="YOP" FieldName="YoP"  VisibleIndex="2">                                    
                                </dx:BootstrapGridViewTextColumn>                                                                                                                                                                                                          
 
                                <dx:BootstrapGridViewCommandColumn HorizontalAlign="Right" ShowDeleteButton="False" ShowEditButton="False" ShowNewButtonInHeader="True" VisibleIndex="0"></dx:BootstrapGridViewCommandColumn>

                        </Columns>
                         <ClientSideEvents EndCallback="onEndCallback" />
                        <SettingsPager NumericButtonCount="4" PageSize="2">                                                                                        
                            <PageSizeItemSettings Visible="true" Items="2" />
                        </SettingsPager>
                    </dx:BootstrapGridView>
                </div>

       
            </ContentTemplate>
        </asp:UpdatePanel>
      
</asp:Content>
