<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="Weighplatation.View.Config" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content">
        <div class="box box-primary">
            <div class="box-body">
                <div class="table-responsive">
                    <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" AutoGenerateColumns="False"   DataSourceID="SqlDataSource1" KeyFieldName="WBSOURCE">
                        <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
                        <Settings ShowHeaderFilterButton="True" />
                        <SettingsEditing Mode="PopupEditForm">
                            <FormLayoutProperties LayoutType="Vertical">
                                <Items>
                                    <dx:BootstrapGridViewColumnLayoutItem Caption="WB Source" ColumnName="WBSOURCE">
                                    </dx:BootstrapGridViewColumnLayoutItem>
                                    <dx:BootstrapGridViewColumnLayoutItem Caption="Description" ColumnName="Description" RequiredMarkDisplayMode="Required">
                                    </dx:BootstrapGridViewColumnLayoutItem>
                                    <dx:BootstrapGridViewColumnLayoutItem Caption="Com Port" ColumnName="ComPort" RequiredMarkDisplayMode="Required">
                                    </dx:BootstrapGridViewColumnLayoutItem>
                                    <dx:BootstrapGridViewColumnLayoutItem Caption="Baut Rate"  ColumnName="Bautrate" RequiredMarkDisplayMode="Required">
                                    </dx:BootstrapGridViewColumnLayoutItem>
                                    <dx:BootstrapGridViewColumnLayoutItem Caption="Data Bits"  ColumnName="DataBits" RequiredMarkDisplayMode="Required">
                                    </dx:BootstrapGridViewColumnLayoutItem>
                                    <dx:BootstrapGridViewColumnLayoutItem Caption="Stop Bits"  ColumnName="StopBits" RequiredMarkDisplayMode="Required">
                                    </dx:BootstrapGridViewColumnLayoutItem>
                                    <dx:BootstrapGridViewColumnLayoutItem Caption="Parity"  ColumnName="Parity" RequiredMarkDisplayMode="Required">
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
                            <CancelButton IconCssClass="fa fa-close" Text="Cancel" />
                            <EditButton IconCssClass="fa fa-edit" />
                            <DeleteButton IconCssClass="fa fa-trash-o" />
                        </SettingsCommandButton>
                         <SettingsPopup>  
                           <EditForm>  
                               <SettingsAdaptivity MaxWidth="800" />  
                           </EditForm>  
                        </SettingsPopup>  
                        <Columns>
                            <dx:BootstrapGridViewCommandColumn ShowDeleteButton="False" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0"></dx:BootstrapGridViewCommandColumn>
                            <dx:BootstrapGridViewTextColumn  FieldName="WBSOURCE"  VisibleIndex="1">
                            </dx:BootstrapGridViewTextColumn>
                            <dx:BootstrapGridViewTextColumn  FieldName="Description"  VisibleIndex="2" >
                            </dx:BootstrapGridViewTextColumn>
                            <dx:BootstrapGridViewTextColumn  FieldName="ComPort"  VisibleIndex="3" >
                            </dx:BootstrapGridViewTextColumn>
                            <dx:BootstrapGridViewTextColumn  FieldName="Bautrate"  VisibleIndex="4" >
                            </dx:BootstrapGridViewTextColumn>
                            <dx:BootstrapGridViewTextColumn  FieldName="DataBits"  VisibleIndex="5" >
                            </dx:BootstrapGridViewTextColumn>
                            <dx:BootstrapGridViewTextColumn  FieldName="StopBits"  VisibleIndex="6" >
                            </dx:BootstrapGridViewTextColumn>
                            <dx:BootstrapGridViewTextColumn  FieldName="Parity"  VisibleIndex="7" >
                            </dx:BootstrapGridViewTextColumn>
                            <dx:BootstrapGridViewCheckColumn  FieldName="Active" VisibleIndex="8" >
                            </dx:BootstrapGridViewCheckColumn>
                        </Columns>
                        <SettingsSearchPanel ShowClearButton="True" Visible="True" />
                       <%-- <SettingsPager NumericButtonCount="4">
                            <PageSizeItemSettings Visible="true" Items="5,10" />
                        </SettingsPager>--%>
                    </dx:BootstrapGridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
                    DeleteCommand="UPDATE public.&quot;WBCONFIG&quot; SET &quot;Active&quot; = 'false' Where &quot;WBSOURCE&quot; = @WBSOURCE"
                    InsertCommand="INSERT INTO public.&quot;WBCONFIG&quot; (&quot;WBSOURCE&quot;,&quot;Description&quot;,&quot;ComPort&quot;,&quot;Bautrate&quot;,&quot;DataBits&quot;,&quot;StopBits&quot;,&quot;Parity&quot;,&quot;Active&quot;) VALUES (@WBSOURCE,@Description,@ComPort,@Bautrate,@DataBits,@StopBits,@Parity,@Active) " ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
                    SelectCommand="SELECT * FROM public.&quot;WBCONFIG&quot;"
                    UpdateCommand="UPDATE public.&quot;WBCONFIG&quot; SET &quot;Description&quot; = @Description,&quot;ComPort&quot; = @ComPort,&quot;Bautrate&quot; = @Bautrate,&quot;DataBits&quot; = @DataBits,&quot;StopBits&quot; = @StopBits,&quot;Parity&quot; = @Parity,&quot;Active&quot; = @Active Where &quot;WBSOURCE&quot; = @WBSOURCE">
                    <DeleteParameters>
                        <asp:Parameter Name="wbsource" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="wbsource" />
                        <asp:Parameter Name="Description" />
                        <asp:Parameter Name="ComPort" />
                        <asp:Parameter Name="Bautrate" />
                        <asp:Parameter Name="DataBits" />
                        <asp:Parameter Name="StopBits" />
                        <asp:Parameter Name="Parity" />
                        <asp:Parameter Name="Active" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="wbsource" />
                        <asp:Parameter Name="Description" />
                        <asp:Parameter Name="ComPort" />
                        <asp:Parameter Name="Bautrate" />
                        <asp:Parameter Name="DataBits" />
                        <asp:Parameter Name="StopBits" />
                        <asp:Parameter Name="Parity" />
                        <asp:Parameter Name="Active" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                </div>
            </div>
        </div>
    </section>
    
</asp:Content>
