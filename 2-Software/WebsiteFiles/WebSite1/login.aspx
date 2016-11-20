<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: left;
            margin-left: auto;
            margin-right: auto;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="auto-style1">
        <h2>Right Alert Admin Panel Login</h2>
        <div class="auto-style1">
        <asp:Label ID="Label1" runat="server" Text="Please log in below to access the administration portal."></asp:Label>
        <br />
        <br />
        <asp:Login ID="LoginControl" runat="server" 
            onauthenticate="LoginControl_Authenticate" Width="329px">
        </asp:Login>
        </div>
    </div>
    </form>
</body>
</html>