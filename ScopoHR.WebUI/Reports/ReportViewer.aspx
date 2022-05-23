<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="ScopoHR.WebUI.Reports.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="ThemeBucket">
    <link rel="shortcut icon" href="images/favicon.png">
    <title>Welcome To ArunimaERP</title>
<link rel="stylesheet" href="/Content/theme/bootstrap/css/bootstrap.min.css">
    <link href="/Content/bootstrap-datepicker/css/datepicker.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="/Content/theme/dist/css/AdminLTE.min.css" />
    <link href="/Content/theme/plugins/iCheck/all.css" rel="stylesheet" />
    <link href="/Content/theme/plugins/timepicker/bootstrap-timepicker.min.css" rel="stylesheet" />
    <link href="/Content/theme/plugins/select2/select2.min.css" rel="stylesheet" />
    <link href="/Content/angular-select/select.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-timepicker/0.5.2/css/bootstrap-timepicker.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-timepicker/0.5.2/css/bootstrap-timepicker.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/selectize.js/0.8.5/css/selectize.default.css">
    
    <link href="/Content/angucomplete-alt/angucomplete-alt.css" rel="stylesheet" />

    <!-- AdminLTE Skins. Choose a skin from the css/skins
         folder instead of downloading all of them to reduce the load. -->
    <link rel="stylesheet" href="/Content/theme/dist/css/skins/_all-skins.min.css">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <!-- jQuery 2.2.0 -->
    
    <script src="/Content/theme/plugins/jQuery/jQuery-2.2.0.min.js"></script>
    
    <!--custom css-->
    <link href="/Content/Style.css" rel="stylesheet" />


</head>
<body>
    <div class="container">
        <!--main content start-->
        <section id="main-content">
            <section class="wrapper">
                <!-- page start-->

                <div class="row">

                    <form id="form1" runat="server">
                        <asp:ScriptManager ID="ScopoScriptManager" runat="server"></asp:ScriptManager>
                        <div>
                            <rsweb:ReportViewer ID="ScopoReportViewer" runat="server" Height="768px" Width="100%"></rsweb:ReportViewer>
                        </div>
                    </form>
                </div>

                <!-- page end-->
            </section>
        </section>
        <!--main content end-->
        </div>
</body>
</html>
