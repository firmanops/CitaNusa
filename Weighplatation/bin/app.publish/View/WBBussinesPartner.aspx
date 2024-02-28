<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WBBussinesPartner.aspx.cs" Inherits="Weighplatation.View.WBBussinesPartner" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-20 card-box mb-30">
          <div class="row>
                <%--<div class="col-md-12">--%>                                         
                <div class="col-md-12">                                                               
                    <dx:BootstrapButton runat="server"   ID="btnBusiness"  Text="Pull Business Partner" OnClick="btnBusiness_Click"  >
                        <SettingsBootstrap RenderOption="Primary" />                                               
                    </dx:BootstrapButton>                  
                </div>                                        
            <%--</div>--%>
        </div>
        <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" AutoGenerateColumns="False" EnableRowsCache="False" DataSourceID="SqlDataSource1" KeyFieldName="BPCode">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" />
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="BP Code" ColumnName="BPCode">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="BP Name" ColumnName="BPName" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>                      
                        <dx:BootstrapGridViewColumnLayoutItem Caption="BP Type" ColumnName="BPType" RequiredMarkDisplayMode="Required">                          
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Address1"  ColumnName="Address1" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Address2"  ColumnName="Address2">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="City"  ColumnName="City" >
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Province"  ColumnName="Province" >
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Postal Code"  ColumnName="Postalcode" >
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Tax ID"  ColumnName="TaxID" >
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Phone"  ColumnName="Phone" >
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Email"  ColumnName="Email" >
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="PIC Name"  ColumnName="PICName" >
                        </dx:BootstrapGridViewColumnLayoutItem>
                         <dx:BootstrapGridViewColumnLayoutItem Caption="Potongan"  ColumnName="potongan" >
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
                   <SettingsAdaptivity MaxWidth="900" />                
               </EditForm>            
            </SettingsPopup>  
            <Columns>
                <dx:BootstrapGridViewCommandColumn ShowDeleteButton="False" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0"></dx:BootstrapGridViewCommandColumn>
                <dx:BootstrapGridViewTextColumn FieldName="BPCode"  VisibleIndex="1">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="BPName"  VisibleIndex="2">
                </dx:BootstrapGridViewTextColumn>
                 <dx:BootstrapGridViewComboBoxColumn Caption="BP Type" FieldName="BPType"  Visible="true" VisibleIndex="3">
                       <PropertiesComboBox>
                           <Items>
                                <dx:BootstrapListEditItem Text="Internal" Value="0" />
                                <dx:BootstrapListEditItem Text="Supplier" Value="1" />
                                 <dx:BootstrapListEditItem Text="Customer" Value="2" />
                           </Items>
                       </PropertiesComboBox>
                </dx:BootstrapGridViewComboBoxColumn>               
                <dx:BootstrapGridViewTextColumn FieldName="Address1"  VisibleIndex="4">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Address2"  VisibleIndex="5">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="City"  VisibleIndex="6">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Province" VisibleIndex="7">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Postalcode" VisibleIndex="8">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="TaxID" VisibleIndex="9">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Phone" VisibleIndex="10">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Email" VisibleIndex="11">
                </dx:BootstrapGridViewTextColumn>
                 <dx:BootstrapGridViewTextColumn FieldName="PICName" VisibleIndex="12">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewCheckColumn FieldName="Active" VisibleIndex="13">
                </dx:BootstrapGridViewCheckColumn>                                              
                <dx:BootstrapGridViewTextColumn Caption="Potongan %" FieldName="potongan" VisibleIndex="14">
                </dx:BootstrapGridViewTextColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
            DeleteCommand="UPDATE public.&quot;BUSINESSPARTNER&quot; SET &quot;Active&quot; = 'false' where &quot;BPCode&quot; = @BPCode"
            InsertCommand="INSERT INTO public.&quot;BUSINESSPARTNER&quot; (&quot;BPCode&quot;,&quot;BPName&quot; , &quot;BPType&quot;, &quot;Address1&quot;
                            , &quot;Address2&quot;,&quot;City&quot;,&quot;Province&quot;, &quot;Postalcode&quot;,&quot;TaxID&quot;
                            ,&quot;Phone&quot;,&quot;Email&quot;,&quot;PICName&quot;,&quot;potongan&quot;,&quot;Active&quot;) values ( @BPCode,@BPName, @BPType,@Address1
                            ,  @Address2,@City,@Province, @Postalcode,@TaxID
                            ,@Phone,@Email,@PICName,@potongan,@Active)" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="SELECT * FROM public.&quot;BUSINESSPARTNER&quot;"
            UpdateCommand="UPDATE public.&quot;BUSINESSPARTNER&quot; SET &quot;BPCode&quot;= @BPCode,&quot;BPName&quot; = @BPName, &quot;BPType&quot;= @BPType, &quot;Address1&quot;= @Address1
                            , &quot;Address2&quot;= @Address2,&quot;City&quot; = @City,&quot;Province&quot;=@Province, &quot;Postalcode&quot; = @Postalcode,&quot;TaxID&quot;=@TaxID
                            ,&quot;Phone&quot;=@Phone,&quot;Email&quot; = @Email,&quot;PICName&quot;=@PICName, &quot;potongan&quot;=@potongan, &quot;Active&quot; =@Active where &quot;BPCode&quot; = @BPCode">
            <DeleteParameters>
                <asp:Parameter Name="BPCode" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="BPCode" />
                <asp:Parameter Name="BPName" />
                <asp:Parameter Name="BPType" />
                <asp:Parameter Name="Address1" />
                <asp:Parameter Name="Address2" />
                <asp:Parameter Name="City" />
                <asp:Parameter Name="Province" />
                <asp:Parameter Name="Postalcode" />
                <asp:Parameter Name="TaxID" />
                <asp:Parameter Name="Phone" />
                <asp:Parameter Name="Email" />
                <asp:Parameter Name="PICName" />
                <asp:Parameter Name="potongan" />
                <asp:Parameter Name="Active" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="BPCode" />
                <asp:Parameter Name="BPName" />
                <asp:Parameter Name="BPType" />
                <asp:Parameter Name="Address1" />
                <asp:Parameter Name="Address2" />
                <asp:Parameter Name="City" />
                <asp:Parameter Name="Province" />
                <asp:Parameter Name="Postalcode" />
                <asp:Parameter Name="TaxID" />
                <asp:Parameter Name="Phone" />
                <asp:Parameter Name="Email" />
                <asp:Parameter Name="PICName" />
                <asp:Parameter Name="potongan" />
                <asp:Parameter Name="Active" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
