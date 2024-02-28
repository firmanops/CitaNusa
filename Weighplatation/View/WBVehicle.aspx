﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WBVehicle.aspx.cs" Inherits="Weighplatation.View.WBVehicle" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-20 card-box mb-30">
        <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" AutoGenerateColumns="False" EnableRowsCache="False" DataSourceID="SqlDataSource1" KeyFieldName="VehicleID">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" />
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Vehicle ID" ColumnName="VehicleID" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Vehicle Type" ColumnName="VehType" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Bussines Partner" ColumnName="BPCode" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="DriverName"  ColumnName="DriverName" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="LicenseNo"  ColumnName="LicenseNo" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Veh Flag"  ColumnName="VehFlag">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Active"  ColumnName="Active" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapEditModeCommandLayoutItem>
                                <%--Buat Button --%>
                        </dx:BootstrapEditModeCommandLayoutItem>

                    </Items>
                </FormLayoutProperties>
            </SettingsEditing>                           
            <SettingsCommandButton>
                <NewButton IconCssClass="fa fa-plus-circle" />
                <UpdateButton IconCssClass="fa fa-save" Text="Save"/>
                <CancelButton IconCssClass="fa fa-close"/>
                <EditButton IconCssClass="fa fa-edit" />
                <DeleteButton IconCssClass="fa fa-trash-o" />
            </SettingsCommandButton>      
             <SettingsPopup>  
             
               <EditForm>  
                   <SettingsAdaptivity MaxWidth="900" />                
               </EditForm>            
            </SettingsPopup>  
            <Columns>
                <dx:BootstrapGridViewCommandColumn ShowDeleteButton="False" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0"></dx:BootstrapGridViewCommandColumn>
                <dx:BootstrapGridViewTextColumn FieldName="VehicleID"  VisibleIndex="1"  >
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewComboBoxColumn  Caption="Vehicle Type" FieldName="VehType"  Visible="true" VisibleIndex="2">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataVehType" TextField="Description" ValueField="VehType">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn>
                <dx:BootstrapGridViewComboBoxColumn  Caption="Bussines Partner" FieldName="BPCode"  Visible="true" VisibleIndex="3">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataSourceBP" TextField="BPName" ValueField="BPCode">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn>                 
                <dx:BootstrapGridViewTextColumn FieldName="DriverName" VisibleIndex="4">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="LicenseNo" VisibleIndex="5">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="VehFlag" VisibleIndex="6" ReadOnly="true">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewCheckColumn FieldName="Active" VisibleIndex="7">
                </dx:BootstrapGridViewCheckColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
            DeleteCommand="UPDATE public.&quot;WBVEHICLE&quot;  SET &quot;Active&quot; = 'false' where &quot;VehicleID&quot; = @VehicleID"
            InsertCommand="INSERT INTO public.&quot;WBVEHICLE&quot;  (&quot;VehicleID&quot; ,&quot;VehType&quot;, &quot;BPCode&quot;, &quot;DriverName&quot;,&quot;LicenseNo&quot;,&quot;VehFlag&quot;,&quot;Active&quot;) VALUES
                            ( @VehicleID,@VehType, @BPCode,@DriverName,@LicenseNo,'0',@Active)" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="SELECT * FROM public.&quot;WBVEHICLE&quot;"
            UpdateCommand="UPDATE public.&quot;WBVEHICLE&quot; SET &quot;VehType&quot; = @VehType, &quot;BPCode&quot;= @BPCode, &quot;DriverName&quot;= @DriverName
            ,&quot;LicenseNo&quot;= @LicenseNo,&quot;Active&quot; =@Active where &quot;VehicleID&quot; = @VehicleID">
            <DeleteParameters>
                <asp:Parameter Name="VehicleID" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="VehicleID" />
                <asp:Parameter Name="VehType" />
                <asp:Parameter Name="BPCode" />
                <asp:Parameter Name="DriverName" />           
                <asp:Parameter Name="LicenseNo" />
                <asp:Parameter Name="VehFlag" />
                <asp:Parameter Name="Active" />            
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="VehicleID" />
                <asp:Parameter Name="VehType" />
                <asp:Parameter Name="BPCode" />
                <asp:Parameter Name="DriverName" />           
                <asp:Parameter Name="LicenseNo" />
                <asp:Parameter Name="VehFlag" />
                <asp:Parameter Name="Active" />   
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceBP" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select &quot;BPCode&quot; ,&quot;BPName&quot;  from public.&quot;BUSINESSPARTNER&quot;">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataVehType" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select * from public.&quot;WBVEHICLETYPE&quot;">
        </asp:SqlDataSource>
    </div>
</asp:Content>
