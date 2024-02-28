<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WBContract.aspx.cs" Inherits="Weighplatation.View.WBContract" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function OnTextChangedHandler(s) {
            var valQty = grid.GetEditValue("Qty");
            var valUnitPrice = grid.GetEditValue("UnitPrice");
            var valTotalPrice = Number(valQty) * Number(valUnitPrice);
            grid.GetEditor('TotalPrice').SetText(valTotalPrice);


        };
    </script>
   <script type="text/javascript">
       function OnTextChangedPpn(s) {
           var valTotalPrice = grid.GetEditValue("TotalPrice");
           var valPpn = grid.GetEditValue("PPN");
           var valFinalPrice = Number(valTotalPrice) + (Number(valPpn) / 100 * Number(valTotalPrice));
           grid.GetEditor('FinalUnitPrice').SetText(valFinalPrice);
       };
   </script>
    <div class="pd-20 card-box mb-auto">        
        <dx:BootstrapGridView ID="BootstrapGridView1"  ClientInstanceName="grid"    runat="server"    AutoGenerateColumns="False" EnableRowsCache="False" DataSourceID="SqlDataSource1" KeyFieldName="ContractNo">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" HorizontalScrollBarMode="Auto"  />
            
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>                        
                        <dx:BootstrapGridViewColumnLayoutItem   Caption="Contract No" ColumnName="ContractNo"  RequiredMarkDisplayMode="Required">                                                
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Contract Date" ColumnName="ContractDate" RequiredMarkDisplayMode="Required">                        
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Exp Date"  ColumnName="ExpDate" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Product Code" ColumnName="ProductCode" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Business Partner"  ColumnName="BPCode" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Qty (Kg)"  ColumnName="Qty" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Toleransi"  ColumnName="Toleransi" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Unit Price"  ColumnName="UnitPrice" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Total Net Qty"  ColumnName="DespatchQty" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>   
                        <dx:BootstrapGridViewColumnLayoutItem  Caption="Total Price"  ColumnName="TotalPrice"   RequiredMarkDisplayMode="Required">                             
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Delivery Status"  ColumnName="deliverystatus">
                        </dx:BootstrapGridViewColumnLayoutItem>   
                        <dx:BootstrapGridViewColumnLayoutItem Caption="PPN"  ColumnName="PPN" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>                                        
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Final Unit Price"  ColumnName="FinalUnitPrice" RequiredMarkDisplayMode="Required">
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
                   <SettingsAdaptivity MaxWidth="1500" />                
               </EditForm>            
            </SettingsPopup>  
          <Columns>
              <dx:BootstrapGridViewCommandColumn ShowDeleteButton="False" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0"></dx:BootstrapGridViewCommandColumn>
                <dx:BootstrapGridViewTextColumn  FieldName="ContractNo"  VisibleIndex="1">                   
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewDateColumn  FieldName="ContractDate"  VisibleIndex="2">
                </dx:BootstrapGridViewDateColumn>                              
                <dx:BootstrapGridViewDateColumn  FieldName="ExpDate" VisibleIndex="3">
                </dx:BootstrapGridViewDateColumn>
                <dx:BootstrapGridViewTextColumn FieldName="ProductCode" VisibleIndex="4"  >                      
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="BPCode" VisibleIndex="5">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="Qty" VisibleIndex="6">
                </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="Toleransi" VisibleIndex="7">
                </dx:BootstrapGridViewTextColumn>              
               <dx:BootstrapGridViewTextColumn FieldName="UnitPrice" VisibleIndex="8">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="PPN" VisibleIndex="9">                     
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="FinalUnitPrice" VisibleIndex="10" >                 
                </dx:BootstrapGridViewTextColumn>               
                <dx:BootstrapGridViewTextColumn FieldName="TotalPrice" VisibleIndex="11" >
                </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="DespatchQty" VisibleIndex="12" >
                 
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="deliverystatus" VisibleIndex="13">
              </dx:BootstrapGridViewTextColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
            DeleteCommand="UPDATE public.&quot;WBCONTRACT&quot; SET &quot;DeliveryStatus&quot; = '@DeliveryStatus' where &quot;ContractNo&quot; = @ContractNo"
            InsertCommand="INSERT INTO public.&quot;WBCONTRACT&quot; (&quot;ContractNo&quot;, &quot;ContractDate&quot; , &quot;ExpDate&quot;, &quot;ProductCode&quot;,&quot;BPCode&quot;, &quot;Qty&quot;, &quot;Toleransi&quot;, &quot;UnitPrice&quot;,&quot;PremiumPrice&quot;, &quot;PPN&quot;, &quot;FinalUnitPrice&quot;,&quot;TotalPrice&quot;, &quot;DespatchQty&quot;,&quot;DeliveryStatus&quot;) values (@ContractNo,@ContractDate, @ExpDate,@ProductCode,@BPCode
                            ,  @Qty,0,@UnitPrice, 0,@PPN,@FinalUnitPrice,@TotalPrice,0, '1' )" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="SELECT &quot;ContractNo&quot; ,&quot;ContractDate&quot; ,&quot;ExpDate&quot; ,&quot;ProductCode&quot; ,&quot;BPCode&quot;  
                            ,&quot;Qty&quot; ,&quot;Toleransi&quot; ,&quot;UnitPrice&quot;  ,&quot;PPN&quot; ,&quot;FinalUnitPrice&quot; ,&quot;TotalPrice&quot; 
                            ,&quot;DespatchQty&quot; ,case when &quot;DeliveryStatus&quot;  ='1' then 'Open' else 'Close' end DeliveryStatus
                            FROM public.&quot;WBCONTRACT&quot;"
            UpdateCommand="UPDATE public.&quot;WBCONTRACT&quot; 
                            SET  
                            &quot;ContractDate&quot;= @ContractDate
                            ,&quot;ExpDate&quot;= @ExpDate  
                            ,&quot;BPCode&quot;= @BPCode
                            ,&quot;ProductCode&quot; = @ProductCode
                            ,&quot;Toleransi&quot;=0
                            ,&quot;Qty&quot; = @Qty
                            ,&quot;UnitPrice&quot; =@UnitPrice
                            ,&quot;PremiumPrice&quot;=0
                            ,&quot;PPN&quot;=@PPN
                            ,&quot;FinalUnitPrice&quot;=@FinalUnitPrice
                            ,&quot;TotalPrice&quot;=@TotalPrice
                            ,&quot;DespatchQty&quot;=@DespatchQty where &quot;ContractNo&quot; = @ContractNo">
            <DeleteParameters>
                <asp:Parameter Name="ContractNo" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ContractNo" />
                <asp:Parameter Name="ContractDate" />
                <asp:Parameter Name="ExpDate" />
                <asp:Parameter Name="ProductCode" />
                <asp:Parameter Name="UnitType" />
                <asp:Parameter Name="BPCode" />
                <asp:Parameter Name="Qty" />
                <asp:Parameter Name="Toleransi" />
                <asp:Parameter Name="UnitPrice" />
                <asp:Parameter Name="PremiumPrice" />          
                <asp:Parameter Name="PPN" />
                <asp:Parameter Name="FinalUnitPrice" />
                <asp:Parameter Name="TotalPrice" />
                <asp:Parameter Name="DespatchQty" />
                <asp:Parameter Name="DeliveryStatus" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ContractNo" />
                <asp:Parameter Name="ContractDate" />
                <asp:Parameter Name="ExpDate" />
                <asp:Parameter Name="ProductCode" />
                <asp:Parameter Name="UnitType" />
                <asp:Parameter Name="BPCode" />
                <asp:Parameter Name="Qty" />
                <asp:Parameter Name="Toleransi" />
                <asp:Parameter Name="UnitPrice" />
                <asp:Parameter Name="PremiumPrice" />          
                <asp:Parameter Name="PPN" />
                <asp:Parameter Name="FinalUnitPrice" />
                <asp:Parameter Name="TotalPrice" />
                <asp:Parameter Name="DespatchQty" />
                <asp:Parameter Name="DeliveryStatus" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceBP" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select &quot;BPCode&quot; ,&quot;BPName&quot;  from public.&quot;BUSINESSPARTNER&quot;">
        </asp:SqlDataSource>
      <asp:SqlDataSource ID="SqlDataSourceProduct" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" 
            ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
            SelectCommand="select &quot;ProductCode&quot; ,&quot;ProductName&quot;  from public.&quot;WBPRODUCT&quot;">
        </asp:SqlDataSource>
    </div>
</asp:Content>

