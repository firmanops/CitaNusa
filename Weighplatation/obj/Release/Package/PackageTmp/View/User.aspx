﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Weighplatation.View.User" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-20 card-box mb-30">
        <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" OnCustomCallback="BootstrapGridView1_CustomCallback" OnRowUpdating="BootstrapGridView1_RowUpdating"  OnRowInserting="BootstrapGridView1_RowInserting"  OnCustomColumnDisplayText="BootstrapGridView1_CustomColumnDisplayText" AutoGenerateColumns="False" KeyFieldName="userid" EnableRowsCache="False">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" />
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="User ID" ColumnName="userid" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="User Name" ColumnName="username" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Password" ColumnName="password" RequiredMarkDisplayMode="Required">
                            
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Group"  ColumnName="groupid" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Unit Code"  ColumnName="unitcode" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem  Caption="active"  ColumnName="active"  RequiredMarkDisplayMode="Required">                          
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
                <dx:BootstrapGridViewTextColumn Caption="User ID" FieldName="userid"  VisibleIndex="1">
                </dx:BootstrapGridViewTextColumn>                    
                <dx:BootstrapGridViewTextColumn Caption="User Name" FieldName="username"  VisibleIndex="2">
                </dx:BootstrapGridViewTextColumn>                
                <dx:BootstrapGridViewTextColumn Caption="Password" FieldName="password"  VisibleIndex="3" Visible="false">
                    
                    <PropertiesTextEdit DisplayFormatString="*"> </PropertiesTextEdit>
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewComboBoxColumn Caption="Group" FieldName="groupid"  VisibleIndex="4">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataSourceGroup" TextField="groupname" ValueField="groupid">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn> 
                 <dx:BootstrapGridViewComboBoxColumn Caption="Unit Code" FieldName="unitcode"  VisibleIndex="4">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataSourceUnit" TextField="UnitName" ValueField="UnitCode">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn>  
               <%-- <dx:BootstrapGridViewTextColumn Caption="Unit Code" FieldName ="unitcode" VisibleIndex="5" Visible="false">
                </dx:BootstrapGridViewTextColumn>--%>
                <dx:BootstrapGridViewCheckColumn Caption="Status" FieldName="active" VisibleIndex="6">
                </dx:BootstrapGridViewCheckColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSourceUnit" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" SelectCommand="select * from public.&quot;BUSINESSUNIT&quot;"></asp:SqlDataSource>
    <%--    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
            DeleteCommand="UPDATE public.&quot;SYSUSER&quot;  SET &quot;active&quot; = 'false' where &quot;userid&quot; = @userid"
            InsertCommand="INSERT INTO public.&quot;SYSUSER&quot;  (&quot;username&quot;,&quot;password&quot; , &quot;groupid&quot;, &quot;active&quot;) VALUES
                            ( @username,@password, @groupid,@active)" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="select * from public.&quot;SYSUSER&quot;"
            UpdateCommand="UPDATE  public.&quot;SYSUSER&quot; SET &quot;active&quot; =@active where &quot;userid&quot; = @userid">
            <DeleteParameters>
                <asp:Parameter Name="userid" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="userid" />
                <asp:Parameter Name="username" />
                <asp:Parameter Name="password" />
                <asp:Parameter Name="groupid" />
                 <asp:Parameter Name="unitcode" />
                <asp:Parameter Name="active" />            
            </InsertParameters>
            <UpdateParameters>
                 <asp:Parameter Name="userid" />
                <asp:Parameter Name="username" />
                <asp:Parameter Name="passwords" />
                <asp:Parameter Name="groupid" />
                 <asp:Parameter Name="unitcode" />
                <asp:Parameter Name="active" />    
            </UpdateParameters>
        </asp:SqlDataSource>--%>
        <asp:SqlDataSource ID="SqlDataSourceGroup" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select &quot;groupid&quot; ,&quot;groupname&quot;  from public.&quot;SYSUSERGROUP&quot;">
        </asp:SqlDataSource>
    </div>
</asp:Content>
