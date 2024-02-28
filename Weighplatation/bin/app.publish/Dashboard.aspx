<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Weighplatation.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pd-ltr-20 xs-pd-20-10">
			<div class="min-height-200px">
				<div class="page-header">
					<div class="row">
						<div class="col-md-6 col-sm-12">
							<%--<div class="title">
								<h4>Form Welcome</h4>
							</div>--%>
							<nav aria-label="breadcrumb" role="navigation">
								<ol class="breadcrumb">
									<li class="breadcrumb-item"><a href="Dashboard.aspx">Home</a></li>
									<li class="breadcrumb-item active" aria-current="page">Welcome</li>
								</ol>
							</nav>
						</div>						
						</div>
					</div>
				</div>

				<div class="pd-ltr-20">
			<div class="card-box pd-20 height-100-p mb-30">
				<div class="row align-items-center">
					<div class="col-md-4">
						<img src="vendors/images/stock_vector.png" alt="">
					</div>
					<div class="col-md-8">
						<div class="row weight-600 font-30 text-blue">
							<h4 class="font-30 weight-800 mb-10 text-capitalize text-blue">WEIGHBRIDGE MANAGEMENT SYSTEM</h4>
						</div>
						<div class="row weight-600 font-30 text-blue">
							<h4 class="font-30 weight-800 mb-10 text-capitalize text-blue"><%=  Session["UnitNameOW"].ToString()  %></h4>
						</div>
						<%--<div class="row weight-600 font-30 text-blue">
							<h4 class="font-30 weight-800 mb-10 text-capitalize text-blue"><%=  Session["UserName"].ToString()  %></h4>
						</div>--%>
						<%--<div class="row weight-600 font-30 text-blue">WELCOME TO WEIGHTBRIDGE MANAGEMENT SYSTEM</div>
						<h4 class="font-30 weight-800 mb-10 text-capitalize text-blue">
							WELCOME TO WEIGHTBRIDGE MANAGEMENT SYSTEM  
							<div class="weight-600 font-30 text-blue"><%=  Session["UnitNameOW"].ToString()  %></div>
                            <div class="weight-600 font-30 text-blue"><%=  Session["UserName"].ToString()  %></div>
						</h4>--%>
					</div>
				</div>
			</div>

			</div>
		</div>
</asp:Content>
