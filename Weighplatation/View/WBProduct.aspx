<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WBProduct.aspx.cs" Inherits="Weighplatation.View.WBProduct" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-20 card-box mb-30">
        <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" AutoGenerateColumns="False" EnableRowsCache="False" DataSourceID="SqlDataSource1" KeyFieldName="ProductCode">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" />
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Product Code" ColumnName="ProductCode" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Product Name" ColumnName="ProductName" RequiredMarkDisplayMode="Required">
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
                <dx:BootstrapGridViewTextColumn FieldName="ProductCode"  VisibleIndex="1">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="ProductName"  VisibleIndex="2">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewCheckColumn FieldName="Active" VisibleIndex="3">
                </dx:BootstrapGridViewCheckColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
            DeleteCommand="UPDATE public.&quot;WBPRODUCT&quot;  SET &quot;Active&quot; = 'false' where &quot;ProductCode&quot; = @ProductCode"
            InsertCommand="INSERT INTO public.&quot;WBPRODUCT&quot; (&quot;ProductCode&quot;,&quot;ProductName&quot; ,&quot;Active&quot;) VALUES
                            ( @ProductCode,@ProductName,@Active)" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="SELECT * FROM public.&quot;WBPRODUCT&quot;"
            UpdateCommand="UPDATE public.&quot;WBPRODUCT&quot; SET &quot;ProductName&quot; = @ProductName,&quot;Active&quot; =@Active where &quot;ProductCode&quot; = @ProductCode">
            <DeleteParameters>
                <asp:Parameter Name="ProductCode" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ProductCode" />
                <asp:Parameter Name="ProductName" />
                <asp:Parameter Name="Active" />            
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ProductCode" />
                <asp:Parameter Name="ProductName" />
                <asp:Parameter Name="Active" />            
            </UpdateParameters>
        </asp:SqlDataSource>        
    </div>
</asp:Content>
