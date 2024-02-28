<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WBBlock.aspx.cs" Inherits="Weighplatation.View.WBBlock" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-20 card-box mb-30">
        <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" AutoGenerateColumns="False" EnableRowsCache="False" DataSourceID="SqlDataSource1" KeyFieldName="BlockID">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" />
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Block ID" ColumnName="BlockID" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Unit" ColumnName="UnitCode" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="MoP" ColumnName="MoP" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="YoP"  ColumnName="YoP" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Current Planted"  ColumnName="CurrentPlanted" RequiredMarkDisplayMode="Required">
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
                <dx:BootstrapGridViewTextColumn FieldName="BlockID"  VisibleIndex="1">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewComboBoxColumn Caption="Unit" FieldName="UnitCode"  Visible="true" VisibleIndex="2">
                    <PropertiesComboBox  EnableSynchronization="False" IncrementalFilteringMode="StartsWith"           
                        DataSourceID="SqlDataSourceUnit" TextField="UnitName" ValueField="UnitCode">                    
                    </PropertiesComboBox>                                                         
                </dx:BootstrapGridViewComboBoxColumn>             
                <dx:BootstrapGridViewTextColumn FieldName="MoP"  VisibleIndex="3">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="YoP" VisibleIndex="4">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="CurrentPlanted" VisibleIndex="5">
                    <PropertiesTextEdit DisplayFormatString="{0,15:#,##0.00 ;(#,##0.00);-   }"></PropertiesTextEdit>
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewCheckColumn FieldName="Active" VisibleIndex="6">
                </dx:BootstrapGridViewCheckColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
            DeleteCommand="UPDATE public.&quot;WBBLOCK&quot;  SET &quot;Active&quot; = 'false' where &quot;BlockID&quot; = @BlockID"
            InsertCommand="INSERT INTO public.&quot;WBBLOCK&quot; (&quot;BlockID&quot;,&quot;UnitCode&quot; , &quot;MoP&quot;, &quot;YoP&quot;,&quot;CurrentPlanted&quot;,&quot;Active&quot;) VALUES
                            ( @BlockID,@UnitCode, @MoP,@YoP,@CurrentPlanted,@Active)" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="SELECT * FROM public.&quot;WBBLOCK&quot;"
            UpdateCommand="UPDATE public.&quot;WBBLOCK&quot; SET &quot;UnitCode&quot; = @UnitCode, &quot;MoP&quot;= @MoP, &quot;YoP&quot;= @YoP
     ,&quot;CurrentPlanted&quot;= @CurrentPlanted,&quot;Active&quot; =@Active where &quot;BlockID&quot; = @BlockID">
            <DeleteParameters>
                <asp:Parameter Name="BlockID" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="BlockID" />
                <asp:Parameter Name="UnitCode" />
                <asp:Parameter Name="MoP" />
                <asp:Parameter Name="YoP" />
                <asp:Parameter Name="CurrentPlanted" />
                <asp:Parameter Name="Active" />            
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="BlockID" />
                <asp:Parameter Name="UnitCode" />
                <asp:Parameter Name="MoP" />
                <asp:Parameter Name="YoP" />
                <asp:Parameter Name="CurrentPlanted" />
                <asp:Parameter Name="Active" />   
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceUnit" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select &quot;UnitCode&quot; ,&quot;UnitName&quot;  from public.&quot;BUSINESSUNIT&quot;">
        </asp:SqlDataSource>
    </div>
</asp:Content>
