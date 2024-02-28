<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserGroup.aspx.cs" Inherits="Weighplatation.UserGroup" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-20 card-box mb-30">
        <dx:BootstrapGridView ID="BootstrapGridView1" runat="server" AutoGenerateColumns="False" EnableRowsCache="False" DataSourceID="SqlDataSource1" KeyFieldName="groupid">
            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
            <Settings ShowHeaderFilterButton="True" />
            <SettingsEditing Mode="PopupEditForm">
                <FormLayoutProperties LayoutType="Vertical">
                    <Items>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Group ID" ColumnName="groupid" Visible="false">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Group Name" ColumnName="groupname" RequiredMarkDisplayMode="Required">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Description"  ColumnName="description">
                        </dx:BootstrapGridViewColumnLayoutItem>
                        <dx:BootstrapGridViewColumnLayoutItem Caption="Active"  ColumnName="active">
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
                <dx:BootstrapGridViewTextColumn FieldName="groupid"  VisibleIndex="0">
                    <SettingsEditForm Visible="False" />
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="groupname"  VisibleIndex="1">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="description" VisibleIndex="2">
                </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewCheckColumn FieldName="active" VisibleIndex="3">
                </dx:BootstrapGridViewCheckColumn>
            </Columns>
            <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <SettingsPager NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" Items="5,10" />
            </SettingsPager>
        </dx:BootstrapGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>"
            DeleteCommand="UPDATE public.&quot;SYSUSERGROUP&quot; SET &quot;active&quot; = 'false' where &quot;groupid&quot; = @groupid"
            InsertCommand="INSERT INTO public.&quot;SYSUSERGROUP&quot; (&quot;groupname&quot; ,&quot;description&quot; ,&quot;active&quot;) VALUES
                            ( @groupname,@description,@active)" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>"
            SelectCommand="SELECT * FROM public.&quot;SYSUSERGROUP&quot;"
            UpdateCommand="UPDATE public.&quot;SYSUSERGROUP&quot; SET &quot;groupname&quot; = @groupname, &quot;description&quot; =@description,&quot;active&quot; =@active where &quot;groupid&quot; = @groupid">
            <DeleteParameters>
                <asp:Parameter Name="groupid" />
            </DeleteParameters>
            <InsertParameters>               
                <asp:Parameter Name="groupname" />
                <asp:Parameter Name="description" />
                <asp:Parameter Name="active" />            
            </InsertParameters>
            <UpdateParameters>
                 <asp:Parameter Name="groupid" />
                <asp:Parameter Name="groupname" />
                <asp:Parameter Name="description" />
                <asp:Parameter Name="active" />        
            </UpdateParameters>
        </asp:SqlDataSource>        
    </div>
</asp:Content>
