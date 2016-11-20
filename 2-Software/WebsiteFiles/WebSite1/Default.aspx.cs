using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Renci.SshNet;
using System.IO;
using System.Text.RegularExpressions;
//using System.Threading;
using System.Web.Security;
using System.Web.Script.Serialization;


public partial class _Default : System.Web.UI.Page
{
    //public string cs = @"server=52.2.128.19;userid=wp;password=team2;database=bikedata-db";
    // SQL Connection string
    string conStr = ConfigurationManager.ConnectionStrings["RightAlertDB"].ConnectionString;

    // Array to update graph on front page
    public int[] sevenBikeData = new int[7];

    // Bool to note whether this is the first load of SendNotifications()
    //bool firstLoadSendNotifications = true;
    // Not using cause it's unnecessary 

    protected void Page_Load(object sender, EventArgs e)
    {
        //// OPEN DATABASE CONNECTION HERE ON LOAD     
        SqlConnection conn = null;
        SqlDataReader rdr = null;
        try
        {
            conn = new SqlConnection(conStr);
            conn.Open();

            // cmd2 RETRIEVE DATA
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "SELECT COUNT(*) FROM [dbo].[rightalertmaster];";
            cmd2.Prepare();
            rdr = cmd2.ExecuteReader();
            while (rdr.Read())
            {
                int numBikesOverall = rdr.GetInt32(0);
                //numBikesOverall++;
                //AllTimeBikeCount.Text = (numBikesOverall.ToString());
                //LiveOverallCount.Text = AllTimeBikeCount.Text;
            }
            rdr.Close();
            // Count last seven days' data:
            for (int i = 0; i < 7; i++)
            {
                rdr.Close();
                rdr = null;
                cmd2.CommandText = "SELECT COUNT(*) FROM [dbo].[rightalertmaster] WHERE Date = convert(date,dateadd(hh,-4,getdate()) - " + i + " );";
                cmd2.Prepare();
                rdr = cmd2.ExecuteReader();
                while (rdr.Read())
                {
                    if (i == 0)
                    {
                        int numBikesToday = rdr.GetInt32(0);
                        //numBikesToday++;
                        //TodayBikeCount.Text = (numBikesToday.ToString());
                        //LiveTodayCount.Text = TodayBikeCount.Text;
                        sd0.Value = (numBikesToday.ToString());
                    }
                    if (i != 0)
                    {
                        int count = rdr.GetInt32(0);
                        //count++;
                        sevenBikeData[i] = count;
                    }
                }
            }
            sd1.Value = sevenBikeData[1].ToString();
            sd2.Value = sevenBikeData[2].ToString();
            sd3.Value = sevenBikeData[3].ToString();
            sd4.Value = sevenBikeData[4].ToString();
            sd5.Value = sevenBikeData[5].ToString();
            sd6.Value = sevenBikeData[6].ToString();
            rdr.Close();
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error: {0}", ex.ToString());

        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }

        }
        ///// DATABASE CONNECTION CLOSED    

        ///
        /// Reserved space for debugging
        /*
        TestCheckBox.InputAttributes["class"] = "bs-switch";
        TestCheckBox.InputAttributes["data-size"] = "mini";
        TestCheckBox.InputAttributes["data-on-color"] = "success";
        TestCheckBox.InputAttributes["data-off-color"] = "danger";
        */
        /// End reserved space

        //LEDOnButton.Click += new EventHandler(this.LEDOnButton_Click);
        //LEDOnBikeButton.Click += new EventHandler(this.LEDOnBikeButton_Click);
        SendNotifications(); // For REAL TIME MYSQL    
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
                /*if (firstLoadSendNotifications)
                {
                    reader.Close();
                    reader = null;
                    firstLoadSendNotifications = false;
                }
                /*command.CommandText = @"SELECT COUNT([Detection Distance]) FROM [dbo].[rightalertmaster] WHERE Date = convert(date,dateadd(hh,-4,getdate()));";
                command.Prepare();
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    todayCount = reader[0].ToString();
                    sd0.Value = todayCount;
                }
                */
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
                        sd0.Value = todayCount;
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
                        batteryData[0] = rdr3[0].ToString(); // Bus voltage                        
                        batteryData[1] = rdr3[1].ToString();
                        batteryData[2] = rdr3[2].ToString();
                        batteryData[3] = rdr3[3].ToString();
                        batteryData[4] = rdr3[4].ToString();
                        //batteryData[5] = ((12 - rdr3.GetDouble(0)) / 0.0158).ToString();
                        
                        //i++;
                    }
                }
                rdr3.Close();
                conn3.Close();
                /// End Query 4

            }
        }
        NotificationHub nHub = new NotificationHub();
        nHub.NotifyAllClients(todayCount, allCount, statusUpdate, datetime, batteryData);
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

    protected void LEDOnBikeButton_Click(object sender, EventArgs e)
    {
        using (var client = new SshClient("ra-jetson.duckdns.org", "ubuntu", "savingL1v3s"))
        {
            client.Connect();
            client.RunCommand("sudo /home/ubuntu/Desktop/bandhan/./ledOnDB"); // SSH Command Here            
            client.Disconnect();
        }
    }

    protected void LEDOnButton_Click(object sender, EventArgs e)
    {
        using (var client = new SshClient("ra-jetson.duckdns.org", "ubuntu", "savingL1v3s"))
        {
            client.Connect();
            client.RunCommand("sudo /home/ubuntu/Desktop/bandhan/./ledOn"); // SSH Command Here            
            client.Disconnect();
        }

    }

    protected void LiveVideoOn_Click(object sender, EventArgs e)
    {
        using (var client = new SshClient("ra-jetson.duckdns.org", "ubuntu", "savingL1v3s"))
        {
            client.Connect();
            client.RunCommand("nohup /home/ubuntu/Desktop/bandhan/CameraScripts/./LiveCameraOff"); // SSH Command Here     
            client.RunCommand("nohup /home/ubuntu/Desktop/bandhan/CameraScripts/./LiveCameraOn &"); // SSH Command Here            
            client.Disconnect();
        }
    }

    protected void LiveVideoOff_Click(object sender, EventArgs e)
    {
        using (var client = new SshClient("ra-jetson.duckdns.org", "ubuntu", "savingL1v3s"))
        {
            client.Connect();
            client.RunCommand("nohup /home/ubuntu/Desktop/bandhan/CameraScripts/./LiveCameraOff"); // SSH Command Here            
            client.Disconnect();
        }
    }

}