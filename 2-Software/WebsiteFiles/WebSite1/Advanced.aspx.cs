using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using System.IO;
using System.Text.RegularExpressions;
//using System.Threading;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Text;

public partial class Advanced : System.Web.UI.Page
{
    // SQL Connection string
    string conStr = ConfigurationManager.ConnectionStrings["RightAlertDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        OutputTextBox.Rows = 4;
        OutputTextBox.ReadOnly = true;
        SendNotifications(); // For REAL TIME MYSQL    
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

    protected void ExecuteCommandButton_Click(object sender, EventArgs e)
    {
        try
        {
            string command = CommandTextBox.Text;
            if (SudoCheckBox.Checked) command = "sudo " + command;

            using (var client = new SshClient("ra-jetson.duckdns.org", "ubuntu", "savingL1v3s"))
            {
                client.Connect();
                client.RunCommand(command); // SSH Command Here                             
                client.Disconnect();
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Down.html");
        }
    }

    protected void CountBikesButton_Click(object sender, EventArgs e)
    {

        StringBuilder count = new StringBuilder();
        string fDateTime = FromDate.Text + " " + FromDateTime.Text;
        string tDateTime = ToDate.Text + " " + ToDateTime.Text;

        SqlConnection advbikecountconn = null;
        SqlDataReader advbikecountrdr = null;
        advbikecountconn = new SqlConnection(conStr);
        advbikecountconn.Open();
        // cmd2 RETRIEVE DATA
        SqlCommand advbikecountcmd = new SqlCommand();
        advbikecountcmd.Connection = advbikecountconn;
        advbikecountcmd.CommandText = @"SELECT COUNT(*) FROM rightalertmaster WHERE DateTime >= '" + fDateTime + "' AND DateTime <= '" + tDateTime + "';";
        advbikecountcmd.Prepare();
        advbikecountrdr = advbikecountcmd.ExecuteReader();
        if (advbikecountrdr.HasRows)
        {
            while (advbikecountrdr.Read())
            {
                string num = advbikecountrdr[0].ToString();
                //count.Append("<div class=\"ov - widget__value\"><label>");
                count.Append("Bike Count: " + num.ToString());
                //count.Append("</label></div><div class=\"ov-widget__info\"><div class=\"ov-widget__title\">Bike Count</div></div>");
            }
        }
        advbikecountrdr.Close();
        advbikecountconn.Close();

        CountBikesPlaceHolder.Controls.Add(new Literal { Text = count.ToString() });
    }

    protected void SendNotifications()
    {
        string todayCount = string.Empty;
        string allCount = string.Empty;
        string[] statusUpdate = new string[4];
        string[] datetime = new string[4];
        string[] batteryData = new string[5];

        using (SqlConnection connection = new SqlConnection(conStr))
        {
            string query = @"SELECT [rightalertmaster].[Detection Distance], [activity].[Status] FROM [dbo].[rightalertmaster], [dbo].[activity]";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Notification = null;
                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    //message = reader[0].ToString();                    
                }

                /// QUERY 1:
                SqlConnection conn0 = new SqlConnection(conStr);
                conn0.Open();
                // cmd2 RETRIEVE DATA
                SqlCommand cmd0 = new SqlCommand();
                cmd0.Connection = conn0;
                cmd0.CommandText = @"SELECT COUNT([Detection Distance]) FROM [dbo].[rightalertmaster] WHERE Date = convert(date,dateadd(hh,-4,getdate()));";
                cmd0.Prepare();

                SqlDataReader rdr0 = cmd0.ExecuteReader();
                //rdr0 = command.ExecuteReader();
                if (rdr0.HasRows)
                {
                    while (rdr0.Read())
                    {
                        todayCount = rdr0[0].ToString();                        
                    }
                }
                rdr0.Close();
                conn0.Close();

                ///QUERY 2:
                SqlConnection conn1 = new SqlConnection(conStr);
                conn1.Open();
                // cmd2 RETRIEVE DATA
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn1;
                cmd1.CommandText = @"SELECT COUNT([ID]) FROM [dbo].[rightalertmaster];";
                cmd1.Prepare();
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.HasRows)
                {
                    while (rdr1.Read())
                    {
                        allCount = rdr1[0].ToString();
                    }
                }
                rdr1.Close();
                conn1.Close();
                /// End Query 2

                // Query 3: FOR LIVE ACTIVITY FEED:
                int i = 0;
                SqlConnection conn2 = new SqlConnection(conStr);
                conn2.Open();
                // cmd2 RETRIEVE DATA
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = conn2;
                cmd2.CommandText = @"SELECT TOP 4 Sender, Status, Date, Time FROM [dbo].[activity] ORDER BY ID DESC;";
                cmd2.Prepare();
                SqlDataReader rdr2 = cmd2.ExecuteReader();
                if (rdr2.HasRows)
                {
                    while (rdr2.Read())
                    {
                        statusUpdate[i] = "<b>" + rdr2[0].ToString() + ": </b>" + rdr2[1].ToString();
                        datetime[i] = rdr2[3].ToString().Substring(0, rdr2[3].ToString().IndexOf(".")) + "  -  " + Regex.Match(rdr2[2].ToString(), "^[^ ]+").Value;
                        i++;
                    }
                }
                rdr2.Close();
                conn2.Close();
                // END LIVE ACTIVITY FEED SECTION

                ///QUERY 4:
                //i = 0;
                SqlConnection conn3 = new SqlConnection(conStr);
                conn3.Open();
                // cmd4 RETRIEVE DATA
                SqlCommand cmd3 = new SqlCommand();
                cmd3.Connection = conn3;
                cmd3.CommandText = @"SELECT TOP 1 [Bus Voltage], [Shunt Voltage], [Load Voltage], [Current], [Watts] FROM [dbo].[powerdata] ORDER BY ID DESC;";
                cmd3.Prepare();
                SqlDataReader rdr3 = cmd3.ExecuteReader();
                if (rdr3.HasRows)
                {
                    while (rdr3.Read())
                    {
                        batteryData[0] = rdr3[0].ToString();
                        batteryData[1] = rdr3[1].ToString();
                        batteryData[2] = rdr3[2].ToString();
                        batteryData[3] = rdr3[3].ToString();
                        batteryData[4] = rdr3[4].ToString();
                        //i++;
                    }
                }
                rdr3.Close();
                conn3.Close();
                /// End Query 4
                NotificationHub nHub = new NotificationHub();
                nHub.NotifyAllClients(todayCount, allCount, statusUpdate, datetime, batteryData);
            }
        }
    }

    public void dependency_OnChange(object sender, SqlNotificationEventArgs e)
    {
        if (e.Type == SqlNotificationType.Change)
        {
            // We reach this point if a change is observed         
            SendNotifications();
        }
        if (e.Type == SqlNotificationType.Subscribe)
        {
            // We reach this point if a change is observed         
            SendNotifications();
        }
    }


}