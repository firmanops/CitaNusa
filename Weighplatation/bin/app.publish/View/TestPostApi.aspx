<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestPostApi.aspx.cs" Inherits="Weighplatation.View.TestPostApi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <dx:ASPxButton runat="server" ID="btnSend" Text="Send Oddo" OnClick="btnSend_Click"></dx:ASPxButton>
    </div>
</asp:Content>
