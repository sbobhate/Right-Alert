using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Alerts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RebootButton_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new SshClient("ra-jetson.duckdns.org", "ubuntu", "savingL1v3s"))
            {
                client.Connect();
                client.RunCommand("sudo /home/ubuntu/Desktop/bandhan/SQL-Scripts/./RightAlertRemoteRebootIssued-SQL"); // SSH Command Here 
                client.RunCommand("sudo reboot"); // SSH Command Here             
                client.Disconnect();
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Down.html");
        }

    }

    protected void LogoutButton_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Response.Redirect(Request.RawUrl);
    }

}