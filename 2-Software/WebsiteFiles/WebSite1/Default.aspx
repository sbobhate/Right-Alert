<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Right Alert</title>
    <link rel="icon" type="image/png" href="img/favicon.png" />
    <link rel="apple-touch-icon-precomposed" href="img/apple-touch-favicon.png" />
    <link href="libs/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="http://fonts.googleapis.com/css?family=Roboto:400,100,100italic,300,300italic,400italic,500,500italic,700,700italic,900,900italic" rel="stylesheet" type="text/css" />
    <link href="libs/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="libs/jquery.scrollbar/jquery.scrollbar.css" rel="stylesheet" />
    <link href="libs/ionrangeslider/css/ion.rangeSlider.css" rel="stylesheet" />
    <link href="libs/ionrangeslider/css/ion.rangeSlider.skinFlat.css" rel="stylesheet" />
    <link href="libs/bootstrap-switch/dist/css/bootstrap3/bootstrap-switch.min.css" rel="stylesheet" />
    <link href="libs/morris.js/morris.css" rel="stylesheet" />
    <link href="libs/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="css/right.css" rel="stylesheet" />
    <link href="css/demo.css" rel="stylesheet" />
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <script src="libs/jquery/jquery.min.js"></script>   
    <script src="libs/bootstrap/js/bootstrap.min.js"></script>
    <script src="libs/jquery.scrollbar/jquery.scrollbar.min.js"></script>
    <script src="libs/bootstrap-tabdrop/bootstrap-tabdrop.min.js"></script>
    <script src="libs/sparkline/jquery.sparkline.min.js"></script>
    <script src="libs/ionrangeslider/js/ion.rangeSlider.min.js"></script>
    <script src="libs/inputNumber/js/inputNumber.js"></script>
    <script src="libs/bootstrap-switch/dist/js/bootstrap-switch.min.js"></script>
    <script src="libs/raphael/raphael-min.js"></script>
    <script src="libs/morris.js/morris.min.js"></script>
    <script src="libs/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="js/template/Default.js"></script>
    <script src="js/main.js"></script>
    <!-- Two scripts below used for signal r, removed <script src="Scripts/jquery-1.6.4.min.js" type="text/javascript"></script> -->    
    <script src="Scripts/jquery.signalR-2.2.0.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>

    <script> <!-- This script runs to update values on the webpage in real time if the database is changed :) -->
        $(function () {
            var notificationHub = $.connection.notificationHub;
            
            notificationHub.client.displayNotification = function (msg, msg2, status, datetime, batteryData) {
                document.getElementById("LiveTodayCount").innerHTML = msg;
                document.getElementById("LiveOverallCount").innerHTML = msg2;

                document.getElementById("ActivityFeed1").innerHTML = status[0];
                document.getElementById("ActivityFeed2").innerHTML = status[1];
                document.getElementById("ActivityFeed3").innerHTML = status[2];
                document.getElementById("ActivityFeed4").innerHTML = status[3];
                document.getElementById("DT1").innerHTML = datetime[0];
                document.getElementById("DT2").innerHTML = datetime[1];
                document.getElementById("DT3").innerHTML = datetime[2];
                document.getElementById("DT4").innerHTML = datetime[3];

                document.getElementById("BatteryLevelLabel").innerHTML = Math.round(100 - ((12.6 - batteryData[0]) / 0.0158)) + " %";
                
            };
            $.connection.hub.logging = true;
            $.connection.hub.start();

        });
    </script>    
  </head>
<body class="transparent_lilac">
  <form id="form1" runat="server">
    <div class="wrapper">
      <nav class="navbar navbar-static-top header-navbar">
        <div class="header-navbar-mobile">
          <div class="header-navbar-mobile__menu">
            <button type="button" class="btn"><i class="fa fa-bars"></i></button>
          </div>
          <div class="header-navbar-mobile__title"><span>Dashboard</span></div>
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
            <li class="active"><a href="./"><span>Dashboard</span></a></li>
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
            <li class="dropdown"><a href="" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" class="userbar__settings dropdown-toggle"><i class="fa fa-power-off"></i></a>
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
                    <li class="active"><a href="./">
                      <div class="nav-menu__ico"><i class="fa fa-fw fa-cube"></i></div>
                      <div class="nav-menu__text"><span>Dashboard</span></div></a></li>
                    <li><a href="activitylog.aspx">
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
                <div class="pages pages_dashboard">
                  <div class="row">
                    <div class="col-md-12">
                      <div class="panel panel-danger">
                        <div class="panel-heading">
                          <h3 class="panel-title">Right Alert statistics</h3>
                        </div>
                        <div class="panel-body">
                          <div class="ld-widget">
                            <div class="ld-widget__cont">
                              <div id="testdiv" class="ld-widget-main">
                                <div class="ld-widget-main__title">Seven Day Biking Stats</div>                               
                                <div id="testdiv2" class="ld-widget-main__chart"></div>                        
                              </div>                              
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    
                  </div>
                  <div class="row">
                    <div class="col-md-5">
                      <div class="panel panel-success">
                        <div class="panel-heading">
                          <h3 class="panel-title">Overview</h3>
                        </div>
                        <div class="panel-body">
                          <div class="ov-widget">
                            <div class="ov-widget__list">
                              <div class="ov-widget__item ov-widget__item_inc">
                                <div class="ov-widget__value"><asp:Label ID="LiveTodayCount" runat="server" Text=""></asp:Label></div>
                                <div class="ov-widget__info">
                                  <div class="ov-widget__title">Today's Bike Count</div>
                                </div>
                              </div>
                              <div class="ov-widget__item ov-widget__item_inc">
                                <div class="ov-widget__value"><asp:Label ID="LiveOverallCount" runat="server" Text=""></asp:Label></div>
                                <div class="ov-widget__info">
                                  <div class="ov-widget__title">Overall Bike Count</div>
                                </div>
                              </div>
                              <div class="ov-widget__item ov-widget__item_inc">
                                <div class="ov-widget__value"><asp:Label ID="BatteryLevelLabel" runat="server" Text=""/></div>
                                <div class="ov-widget__info">
                                  <div class="ov-widget__title">Battery Level</div>
                                </div>
                              </div>
                              <div class="ov-widget__item ov-widget__item_inc">
                                <div class="ov-widget__value"><asp:Label ID="BatteryVoltageLabel" runat="server" Text="0"/></div>
                                <div class="ov-widget__info">
                                  <div class="ov-widget__title">Voltage</div>
                                </div>
                              </div>
                              <div class="ov-widget__item ov-widget__item_inc">
                                <div class="ov-widget__value"><asp:Label ID="Label1" runat="server" Text="0"/></div>
                                <div class="ov-widget__info">
                                  <div class="ov-widget__title">Voltage 2</div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="col-md-7">
                      <div class="panel panel-info">
                        <div class="panel-heading">
                          <h3 class="panel-title"><a href="ActivityLog.aspx">Activity feed</a></h3>
                        </div>
                        <div class="feed-widget">
                          <div class="feed-widget__wrap scrollable scrollbar-macosx">
                            <div class="feed-widget__cont">
                              <div class="feed-widget__list">
                                <div class="feed-widget__item feed-widget__item_user">
                                  <div class="feed-widget__ico"><i class="fa fa-fw"></i></div>
                                  <div class="feed-widget__info">
                                    <div class="feed-widget__text"><asp:Label ID="ActivityFeed1" runat="server" Text=""></asp:Label></div>
                                    <div class="feed-widget__date"><asp:Label ID="DT1" runat="server" Text=""></asp:Label></div>
                                  </div>
                                </div>
                                <div class="feed-widget__item feed-widget__item_user">
                                  <div class="feed-widget__ico"><i class="fa fa-fw"></i></div>
                                  <div class="feed-widget__info">
                                    <div class="feed-widget__text"><asp:Label ID="ActivityFeed2" runat="server" Text=""></asp:Label></div>
                                    <div class="feed-widget__date"><asp:Label ID="DT2" runat="server" Text=""></asp:Label></div>
                                  </div>
                                </div>
                                <div class="feed-widget__item feed-widget__item_user">
                                  <div class="feed-widget__ico"><i class="fa fa-fw"></i></div>
                                  <div class="feed-widget__info">
                                    <div class="feed-widget__text"><asp:Label ID="ActivityFeed3" runat="server" Text=""></asp:Label></div>
                                    <div class="feed-widget__date"><asp:Label ID="DT3" runat="server" Text=""></asp:Label></div>
                                  </div>
                                </div>
                                <div class="feed-widget__item feed-widget__item_user">
                                  <div class="feed-widget__ico"><i class="fa fa-fw"></i></div>
                                  <div class="feed-widget__info">
                                    <div class="feed-widget__text"><asp:Label ID="ActivityFeed4" runat="server" Text=""></asp:Label></div>
                                    <div class="feed-widget__date"><asp:Label ID="DT4" runat="server" Text=""></asp:Label></div>
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
            </div>
          </div>
        </div>
       </div>
          <asp:HiddenField ID="sd0" runat="server" Value=""/>
          <asp:HiddenField ID="sd1" runat="server" Value=""/>
          <asp:HiddenField ID="sd2" runat="server" Value=""/>
          <asp:HiddenField ID="sd3" runat="server" Value=""/>
          <asp:HiddenField ID="sd4" runat="server" Value=""/>
          <asp:HiddenField ID="sd5" runat="server" Value=""/>
          <asp:HiddenField ID="sd6" runat="server" Value=""/>
      </form>

    <!-- Commenting out theme changer button >
    <div class="demo">
      <div class="demo__ico"></div>
      <div class="demo__cont">
        <div class="demo__settings">
          <div class="demo__group">
            <div class="demo__label">Color theme:</div>
            <div class="demo__themes">
              <div data-theme="transparent_lilac" title="Lilac" class="demo__theme demo__theme_active demo__theme_lilac"></div>
              <div data-theme="solid_dark" title="Dark" class="demo__theme demo__theme_dark"></div>              
            </div>
          </div>
        </div>
      </div>
    </div>
    < end theme changer button comment -->
      
  </body>  
</html>