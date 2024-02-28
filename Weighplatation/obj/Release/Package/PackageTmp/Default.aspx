<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WeighingSystem.Default" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
	<!-- Basic Page Info -->
	<meta charset="utf-8" />
	<title>MILL MANAGEMENT SYSTEM</title>

	<!-- Site favicon -->
	<link rel="icon" sizes="180x180" href="/vendors/images/apple-touch-icon.png" />
	<link rel="icon" type="image/png" sizes="32x32" href="/vendors/images/favicon-32x32.png" />
	<link rel="icon" type="image/png" sizes="16x16" href="/vendors/images/favicon-16x16.png" />

	<!-- Mobile Specific Metas -->
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />

	<!-- Google Font -->
	<link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap" rel="stylesheet" />
	<!-- CSS -->
	<link rel="stylesheet" type="text/css" href="vendors/styles/core.css" />
	<link rel="stylesheet" type="text/css" href="vendors/styles/icon-font.min.css" />
	<link rel="stylesheet" type="text/css" href="vendors/styles/style.css" />
	 <link rel="stylesheet" href="/src/plugins/sweetalert2.min.css" />
     <link href="../src/plugins/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="/src/scripts/jquery.min.js"></script>
	<!-- Global site tag (gtag.js) - Google Analytics -->
	<script async src="https://www.googletagmanager.com/gtag/js?id=UA-119386393-1"></script>
	 <script src="../src/plugins/sweetalert2.all.min.js"></script>
    <script src="../src/plugins/sweetalert2.min.js"></script>
	<script>
		window.dataLayer = window.dataLayer || [];
		function gtag(){dataLayer.push(arguments);}
		gtag('js', new Date());

		gtag('config', 'UA-119386393-1');
	</script>
	    <script type="text/javascript">
            function warningalert() {
                swal.fire({
                    title: 'Warning!',
                    icon: 'warning',
                    text: 'Qty can not zero',
                    type: 'warning',

                });
            }
            function successalert(msg) {
                swal.fire({
                    title: 'Succes',
                    icon: 'success',
                    text: msg,
                    type: 'success',

                });
            }
            function erroralert(msg) {
                swal.fire({
                    title: 'Error!',
                    icon: 'error',
                    text: msg,
                    type: 'error',

                });
            }
</script>   
</head>
<body class="login-page">
	<div class="login-wrap d-flex align-items-center flex-wrap justify-content-center">
		<div class="container">
			<div class="row align-items-center">
				<div class="col-md-6 col-lg-6">
					<img src="vendors/images/login-page-img.png" alt="" />
				</div>
				<div class="col-md-6 col-lg-5">
					<div class="login-box bg-white box-shadow border-radius-10">
						<div class="login-title">
							<h4 class="text-center text-primary">Login</h4><br />
							<h4 class="text-center text-primary">WEIGHTBRIDGE MANAGEMENT SYSTEM</h4>
						</div>
						<form runat="server">
							<div class="input-group custom">
								<asp:TextBox  runat="server" ID="txtuser" class="form-control form-control-lg" placeholder="Username"></asp:TextBox>
								<div class="input-group-append custom">
									<span class="input-group-text"><i class="icon-copy dw dw-user1"></i></span>
								</div>
							</div>
							<div class="input-group custom">
								<asp:TextBox  runat="server" ID="txtPassword" TextMode="Password" class="form-control form-control-lg" placeholder="**********"></asp:TextBox>
								<div class="input-group-append custom">
									<span class="input-group-text"><i class="dw dw-padlock1"></i></span>
								</div>
							</div>
							<div class="input-group custom">																	
									<div class="forgot-password"><a href="forgot-password.html">Forgot Password</a></div>
							</div>
							<div class="row">
								<div class="col-sm-12">
									<div class="input-group mb-0">
										<!--
											use code for form submit
											<input class="btn btn-primary btn-lg btn-block" type="submit" value="Sign In">
										-->
                                        <%--										<a class="btn btn-primary btn-lg btn-block" href="main.aspx">Sign In</a>--%>
                                        <asp:Button runat="server" ID="btnlogin" class="btn btn-primary btn-lg btn-block" OnClick="btnlogin_Click" Text="Sign In" />
									</div>			
								</div>
							</div>
							<p></p>
							<p>
                            </p>
							<div class="row">
								<div class="col-sm-12">
									<div class="form-group">
                                            <dx:BootstrapComboBox ID="dllwbSource" runat="server" Caption="WB Model" SelectedIndex="0" DataSourceID="SqlDataSourceWB" TextField="WBSOURCE" ValueField="WBSOURCE">
                                                <ClientSideEvents Validation="" />
                                                <ValidationSettings ValidationGroup="Validation">
                                                    <RequiredField IsRequired="false" ErrorText="Unit Name is required" />
                                                </ValidationSettings>
                                            </dx:BootstrapComboBox>
											<asp:SqlDataSource ID="SqlDataSourceWB" runat="server" ConnectionString="<%$ ConnectionStrings:ILGDbConn %>" ProviderName="<%$ ConnectionStrings:ILGDbConn.ProviderName %>" 
															 SelectCommand="select &quot;WBSOURCE&quot;  from public.&quot;WBCONFIG&quot; order by &quot;WBSOURCE&quot;  ASC"></asp:SqlDataSource>

                                        </div>			
								</div>
							</div>
						</form>
					</div>
				</div>
			</div>
		</div>
	</div>
	<!-- js -->
	<script src="vendors/scripts/core.js"></script>
	<script src="vendors/scripts/script.min.js"></script>
	<script src="vendors/scripts/process.js"></script>
	<script src="vendors/scripts/layout-settings.js"></script>
</body>

</html>
