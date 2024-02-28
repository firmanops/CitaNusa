﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WBDO.aspx.cs" Inherits="Weighplatation.View.WBDO" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-20 card-box mb-30">
        <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" AutoGenerateColumns="False" EnableRowsCache="False" DataSourceID="SqlDataSource1" KeyFieldName="DONo">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" />
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="DO No" ColumnName="DONo" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Contract No" ColumnName="ContractNo" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="DO Date" ColumnName="DODate" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Bussines Partner" ColumnName="BPCode" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Quantity"  ColumnName="Qty" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Despatch Qty"  ColumnName="DespatchQty" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Delivery Status"  ColumnName="DeliveryStatus" RequiredMarkDisplayMode="Required">
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
                <dx:BootstrapGridViewTextColumn FieldName="DONo"  VisibleIndex="1" ReadOnly="true">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewComboBoxColumn Caption="Contract No." FieldName="ContractNo"  Visible="true" VisibleIndex="2">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataSourceContract" TextField="ContractNo" ValueField="ContractNo">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn> 
                <dx:BootstrapGridViewDateColumn Caption="DO Date" FieldName="DODate" VisibleIndex="3">                                     
                </dx:BootstrapGridViewDateColumn>
                <dx:BootstrapGridViewComboBoxColumn Caption="Business Partner." FieldName="BPCode"  Visible="true" VisibleIndex="4">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataSourceBP" TextField="BPName" ValueField="BPCode">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn>
                <dx:BootstrapGridViewTextColumn Caption="Quantity" FieldName="Qty" VisibleIndex="5" >
                    <PropertiesTextEdit NullDisplayText="0"  DisplayFormatString="###,###.#0;-#.#;0"></PropertiesTextEdit>
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn Caption="Despatch Qty" FieldName="DespatchQty" VisibleIndex="6">
                    <PropertiesTextEdit NullDisplayText="0" DisplayFormatString="###,###.#0;-#.#;0" ></PropertiesTextEdit>
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn Caption="DeliveryStatus" FieldName="DeliveryStatus"  VisibleIndex="7">
                    <PropertiesTextEdit NullDisplayText="F"></PropertiesTextEdit>
                </dx:BootstrapGridViewTextColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Server=localhost;port=5432;Database=plantationuat;User Id=postgres;Password=root;"
            DeleteCommand="UPDATE public.&quot;WBDO&quot;  SET &quot;DeliveryStatus&quot; = 'D' where &quot;DONo&quot; = @DONo"
            InsertCommand="INSERT INTO public.&quot;WBDO&quot;  (&quot;DONo&quot; , &quot;ContractNo&quot; , &quot;DODate&quot; , &quot;BPCode&quot; , &quot;Qty&quot; , &quot;DespatchQty&quot; , &quot;DeliveryStatus&quot;) VALUES
                            ( @DONo,@ContractNo, @DODate,@BPCode,COALESCE(@Qty,0),COALESCE(@DespatchQty,0),COALESCE(@DeliveryStatus,'F'))" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="SELECT * FROM public.&quot;WBDO&quot;"
            UpdateCommand="UPDATE public.&quot;WBDO&quot; SET &quot;ContractNo&quot; = @ContractNo, &quot;DODate&quot; = @DODate, &quot;BPCode&quot; = @BPCode
            , &quot;Qty&quot; = @Qty , &quot;DespatchQty&quot; = @DespatchQty , &quot;DeliveryStatus&quot; = @DeliveryStatus where &quot;DONo&quot; = @DONo">
            <DeleteParameters>
                <asp:Parameter Name="DONo" />
            </DeleteParameters>
            <InsertParameters>
                <asp:SessionParameter Name="DONo" SessionField="DONo" Type="String" />
                <%--<asp:Parameter Name="DONo" />--%>
                <asp:Parameter Name="ContractNo" />
                <asp:Parameter Name="DODate" />
                <asp:Parameter Name="BPCode" />           
                <asp:Parameter Name="Qty" />
                <asp:Parameter Name="DespatchQty" />
                <asp:Parameter Name="DeliveryStatus" />            
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="DONo" />
                <asp:Parameter Name="ContractNo" />
                <asp:Parameter Name="DODate" />
                <asp:Parameter Name="BPCode" />           
                <asp:Parameter Name="Qty" />
                <asp:Parameter Name="DespatchQty" />
                <asp:Parameter Name="DeliveryStatus" />            
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceBP" runat="server" ConnectionString="Server=localhost;port=5432;Database=plantationuat;User Id=postgres;Password=root;" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select &quot;BPCode&quot; ,&quot;BPName&quot;  from public.&quot;BUSINESSPARTNER&quot;">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceContract" runat="server" ConnectionString="Server=localhost;port=5432;Database=plantationuat;User Id=postgres;Password=root;" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
        SelectCommand="select &quot;ContractNo&quot;  from  public.&quot;WBCONTRACT&quot;"></asp:SqlDataSource>
    </div>
</asp:Content>
