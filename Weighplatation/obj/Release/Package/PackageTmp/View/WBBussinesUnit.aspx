﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WBBussinesUnit.aspx.cs" Inherits="Weighplatation.View.WBBussinesUnit" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-20 card-box mb-30">    
        <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" AutoGenerateColumns="False" EnableRowsCache="False" DataSourceID="SqlDataSource1" KeyFieldName="UnitCode">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" />
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Unit Code" ColumnName="UnitCode">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Unit Name" ColumnName="UnitName" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Bussines Partner" ColumnName="BPCode" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Unit Type" ColumnName="UnitType" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Address1"  ColumnName="Address1" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Address2"  ColumnName="Address2">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="City"  ColumnName="City">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Province"  ColumnName="Province">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Postal Code"  ColumnName="Postalcode">
                        </dx:BootstrapGridViewColumnLayoutItem> 
                         <dx:BootstrapGridViewColumnLayoutItem Caption="Active"  ColumnName="Active">
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
                   <SettingsAdaptivity MaxWidth="1000" />                
               </EditForm>            
            </SettingsPopup>  
            <Columns>
                <dx:BootstrapGridViewCommandColumn ShowDeleteButton="False" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0"></dx:BootstrapGridViewCommandColumn>
                <dx:BootstrapGridViewTextColumn FieldName="UnitCode"  VisibleIndex="1">
                </dx:BootstrapGridViewTextColumn>            
                <dx:BootstrapGridViewTextColumn FieldName="UnitName"  VisibleIndex="2">              
                </dx:BootstrapGridViewTextColumn>           
               <dx:BootstrapGridViewComboBoxColumn Caption="Bussines Partner" FieldName="BPCode"  Visible="true" VisibleIndex="3">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataSourceBP" TextField="BPName" ValueField="BPCode">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn> 
                 <dx:BootstrapGridViewComboBoxColumn Caption="Unit Type" FieldName="UnitType"  Visible="true" VisibleIndex="4">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataUnitType" TextField="UnitType" ValueField="UnitType">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn>  
                <dx:BootstrapGridViewTextColumn FieldName="Address1"  VisibleIndex="5">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Address2"  VisibleIndex="6">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="City" VisibleIndex="7">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Province" VisibleIndex="8">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Postalcode" VisibleIndex="9">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewCheckColumn FieldName="Active" VisibleIndex="10">
                </dx:BootstrapGridViewCheckColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
            DeleteCommand="UPDATE public.&quot;BUSINESSUNIT&quot; SET &quot;Active&quot; = 'false' where &quot;UnitCode&quot; = @UnitCode"
            InsertCommand="INSERT INTO public.&quot;BUSINESSUNIT&quot; (&quot;UnitCode&quot;,&quot;UnitName&quot; , &quot;BPCode&quot;,&quot;UnitType&quot; , &quot;Address1&quot;
                            , &quot;Address2&quot;,&quot;City&quot;,&quot;Province&quot;, &quot;Postalcode&quot;,&quot;Active&quot;) values (@UnitCode,@UnitName, @BPCode,@UnitType,@Address1
                            ,  @Address2,@City,@Province, @Postalcode,@Active)" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="SELECT * FROM public.&quot;BUSINESSUNIT&quot;"
            UpdateCommand="UPDATE public.&quot;BUSINESSUNIT&quot; SET &quot;UnitName&quot; = @UnitName, &quot;BPCode&quot;= @BPCode, &quot;UnitType&quot;= @UnitType
                            ,&quot;Address2&quot;= @Address2,&quot;City&quot; = @City,&quot;Province&quot;=@Province, &quot;Postalcode&quot; = @Postalcode,&quot;Active&quot; =@Active where &quot;UnitCode&quot; = @UnitCode">
            <DeleteParameters>
                <asp:Parameter Name="UnitCode" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="UnitCode" />
                <asp:Parameter Name="UnitName" />
                <asp:Parameter Name="BPCode" />
                <asp:Parameter Name="UnitType" />
                <asp:Parameter Name="Address1" />
                <asp:Parameter Name="Address2" />
                <asp:Parameter Name="City" />
                <asp:Parameter Name="Province" />
                <asp:Parameter Name="Postalcode" />          
                <asp:Parameter Name="Active" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="UnitCode" />
                <asp:Parameter Name="UnitName" />
                <asp:Parameter Name="BPCode" />
                <asp:Parameter Name="UnitType" />
                <asp:Parameter Name="Address1" />
                <asp:Parameter Name="Address2" />
                <asp:Parameter Name="City" />
                <asp:Parameter Name="Province" />
                <asp:Parameter Name="Postalcode" />          
                <asp:Parameter Name="Active" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceBP" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select &quot;BPCode&quot; ,&quot;BPName&quot;  from public.&quot;BUSINESSPARTNER&quot;">
        </asp:SqlDataSource>
      <asp:SqlDataSource ID="SqlDataUnitType" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select &quot;UnitType&quot; from public.&quot;UNITTYPE&quot;">
        </asp:SqlDataSource>
    </div>
</asp:Content>
