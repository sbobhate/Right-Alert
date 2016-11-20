﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivityLog.aspx.cs" Inherits="ActivityLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Right Alert - Activity Log</title>
    <link rel="icon" type="image/png" href="img/favicon.png" />
    <link rel="apple-touch-icon-precomposed" href="img/apple-touch-favicon.png" />
    <link href="libs/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="http://fonts.googleapis.com/css?family=Roboto:400,100,100italic,300,300italic,400italic,500,500italic,700,700italic,900,900italic" rel="stylesheet" type="text/css" />
    <link href="libs/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="libs/jquery.scrollbar/jquery.scrollbar.css" rel="stylesheet" />
    <link href="libs/ionrangeslider/css/ion.rangeSlider.css" rel="stylesheet" />
    <link href="libs/ionrangeslider/css/ion.rangeSlider.skinFlat.css" rel="stylesheet" />
    <link href="libs/bootstrap-switch/dist/css/bootstrap3/bootstrap-switch.min.css" rel="stylesheet" />
    <link href="css/right.css" rel="stylesheet" />
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script><![endif]-->
</head>
<body class="transparent_lilac">
  <form id="form1" runat="server">
    <div class="wrapper">
      <nav class="navbar navbar-static-top header-navbar">
        <div class="header-navbar-mobile">
          <div class="header-navbar-mobile__menu">
            <button type="button" class="btn"><i class="fa fa-bars"></i></button>
          </div>
          <div class="header-navbar-mobile__title"><span>Activity Log</span></div>
          <div class="header-navbar-mobile__settings dropdown"><a href="" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" class="btn dropdown-toggle"><i class="fa fa-power-off"></i></a>
            <ul class="dropdown-menu dropdown-menu-right">
              <li><asp:Button ID="RebootButton0" runat="server" Text="Reboot" BackColor="Transparent" BorderStyle="None" OnClick="RebootButton_Click"/></li>
              <li><asp:Button ID="LogoutButton0" runat="server" Text="Logout" BackColor="Transparent" BorderStyle="None" OnClick="LogoutButton_Click"/></li>
            </ul>
          </div>
        </div>
        <div class="navbar-header"><a href="./" class="navbar-brand">
            <div class="logo text-nowrap">
              <div class="logo__img"><i class="fa fa-chevron-right"></i></div><span class="logo__text">Right Alert</span>
            </div></a></div>
        <div class="topnavbar">
          <ul class="nav navbar-nav navbar-left">
            <li class="active"><a href="./"><span>Activity Log</span></a></li>
            <!--li class="dropdown"><a href="#" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" class="dropdown-toggle"><span>Pages&nbsp;<i class="caret"></i></span></a>
              <ul class="dropdown-menu">
                <li><a href="products.html"><span>Products</span></a></li>
                <li><a href="orders.html"><span>Orders</span></a></li>
                <li><a href="users.html"><span>Users</span></a></li>
                <li role="separator" class="divider"></li>
                <li><a href="login.html">Login</a></li>
                <li><a href="blank.html">Blank</a></li>
              </ul>
            </li-->
          </ul>
          <ul class="userbar nav navbar-nav">
            <li class="dropdown"><a href="./" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" class="userbar__settings dropdown-toggle"><i class="fa fa-power-off"></i></a>
              <ul class="dropdown-menu">
                <li><asp:Button ID="RebootButton" runat="server" Text="Reboot" BackColor="Transparent" BorderStyle="None" OnClick="RebootButton_Click"/></li>
                <li><asp:Button ID="LogoutButton" runat="server" Text="Logout" BackColor="Transparent" BorderStyle="None" OnClick="LogoutButton_Click"/></li>
              </ul>
            </li>
          </ul>
        </div>
      </nav>
      <div class="dashboard">
        <div class="sidebar">
                <div class="sidebar__title">Pages</div>
                <ul class="nav nav-menu">
                    <li><a href="default.aspx">
                      <div class="nav-menu__ico"><i class="fa fa-fw fa-cube"></i></div>
                      <div class="nav-menu__text"><span>Dashboard</span></div></a></li>
                    <li class="active"><a href="activitylog.aspx">
                      <div class="nav-menu__ico"><i class="fa fa-fw fa-table"></i></div>
                      <div class="nav-menu__text"><span>Activity Log</span></div></a></li>    
                    <li><a href="alerts.aspx">
                      <div class="nav-menu__ico"><i class="fa fa-fw fa-bell-o"></i></div>
                      <div class="nav-menu__text"><span>Alerts</span></div></a></li>     
                    <li><a href="users.aspx">
                      <div class="nav-menu__ico"><i class="fa fa-fw fa-user"></i></div>
                      <div class="nav-menu__text"><span>Users</span></div></a></li>
                    <li><a href="advanced.aspx">
                      <div class="nav-menu__ico"><i class="fa fa-fw fa-gear"></i></div>
                      <div class="nav-menu__text"><span>Advanced</span></div></a></li>               
                </ul>               
        </div>
        <div class="main">
          <div class="scrollable scrollbar-macosx">
            <div class="main__cont">
              <div class="main-heading">
                <div class="main-title">
                  <ol class="breadcrumb">
                      <li class=""><a href="http://www.bu.edu">Boston University</a></li>
                      <li class="active">Cummington Mall Project</li>
                  </ol>
                </div>
                <!--<div class="main-filter"> </div>-->
                </div> 
              <div class="container-fluid half-padding">
                <div class="template template__blank">
                  <div class="row">
                    <div class="col-md-12">
                      <div class="panel panel-danger">
                        <div class="panel-heading">
                          <h3 class="panel-title">Activity Log</h3>
                        </div>
                        <div class="panel-body">
                          <div class="scrollable scrollbar-macosx"><table id="ActivityLog" class="table datatable display table-hover" cellspacing="0" width="100%">
    <thead>
        <tr>
            <th>ID</th>
            <th>Time</th>
            <th>Date</th>
            <th>Sender</th>
            <th>Message</th>
        </tr>
    </thead>
   <!-- <tfoot>
        <tr>
            <th>ID</th>
            <th>Time</th>
            <th>Date</th>
            <th>Sender</th>
            <th>Status</th>
        </tr>
    </tfoot> -->
    <tbody>
        <asp:PlaceHolder ID="ActivityLogTableDataPlaceHolder" runat="server"></asp:PlaceHolder>

    </tbody>
</table>
                          </div>
                        </div>                       
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <script src="libs/jquery/jquery.min.js"></script>
    <script src="libs/bootstrap/js/bootstrap.min.js"></script>
    <script src="libs/jquery.scrollbar/jquery.scrollbar.min.js"></script>
    <script src="libs/bootstrap-tabdrop/bootstrap-tabdrop.min.js"></script>
    <script src="libs/sparkline/jquery.sparkline.min.js"></script>
    <script src="libs/ionrangeslider/js/ion.rangeSlider.min.js"></script>
    <script src="libs/inputNumber/js/inputNumber.js"></script>
    <script src="libs/bootstrap-switch/dist/js/bootstrap-switch.min.js"></script>
    <script src="js/main.js"></script>
        
    </form>
</body>
</html>