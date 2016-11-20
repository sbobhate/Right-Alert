using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ActivityLog : System.Web.UI.Page
{
    // SQL Connection string
    string conStr = ConfigurationManager.ConnectionStrings["RightAlertDB"].ConnectionString;

    // Table for data
    StringBuilder table = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {

        SqlConnection syslogconn = null;
        SqlDataReader syslogrdr = null;
        syslogconn = new SqlConnection(conStr);
        syslogconn.Open();
        // cmd2 RETRIEVE DATA
        SqlCommand syslogcmd = new SqlCommand();
        syslogcmd.Connection = syslogconn;
        syslogcmd.CommandText = @"SELECT ID, Time, Date, Sender, Status FROM dbo.activity ORDER BY ID DESC;";
        syslogcmd.Prepare();
        syslogrdr = syslogcmd.ExecuteReader();
        if (syslogrdr.HasRows)
        {
            while (syslogrdr.Read())
            {
                string id = syslogrdr[0].ToString();
                string time;
                if (syslogrdr[1].ToString().Contains('.'))
                {
                    time = syslogrdr[1].ToString().Substring(0, syslogrdr[1].ToString().IndexOf("."));
                }
                else
                {
                    time = syslogrdr[1].ToString();
                }
                string date = Regex.Match(syslogrdr[2].ToString(), "^[^ ]+").Value; 
                string _sender = syslogrdr[3].ToString();
                string status = syslogrdr[4].ToString();
                table.Append("<tr>");
                table.Append("<td>" + id + "</td>");
                table.Append("<td>" + time + "</td>");
                table.Append("<td>" + date + "</td>");
                table.Append("<td>" + _sender + "</td>");
                table.Append("<td>" + status + "</td>");
                table.Append("</tr>");
            }
        }
        syslogrdr.Close();
        syslogconn.Close();

        ActivityLogTableDataPlaceHolder.Controls.Add(new Literal { Text = table.ToString() });
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