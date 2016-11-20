using HashLibrary;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void LoginControl_Authenticate(object sender, AuthenticateEventArgs e)
    {
        bool authenticated = this.ValidateCredentials(LoginControl.UserName, LoginControl.Password);
        /// UPDATE ACtIVITY FEED WITH LOGON
        if (authenticated)
        {
            FormsAuthentication.RedirectFromLoginPage(LoginControl.UserName, LoginControl.RememberMeSet);
            try
            {
                using (var client = new SshClient("ra-jetson.duckdns.org", "ubuntu", "savingL1v3s"))
                {
                    client.ConnectionInfo.RetryAttempts = 1;
                    client.Connect();
                    client.RunCommand("sudo /home/ubuntu/Desktop/bandhan/SQL-Scripts/./UserLogon-SQL " + LoginControl.UserName.Trim().ToLower()); // SSH Command Here            
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Down.html");
            }
        }
    }

    public bool IsAlphaNumeric(string text)
    {
        return Regex.IsMatch(text, "^[a-zA-Z0-9]+$");
    }

    private bool ValidateCredentials(string userName, string password)
    {
        bool returnValue = false;

        if (this.IsAlphaNumeric(userName) && userName.Length <= 50 && password.Length <= 50)
        {
            SqlConnection conn = null;

            try
            {
                string sql = "select count(*) from users where username = @username and password = @password";

                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RightAlertDB"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlParameter user = new SqlParameter();
                user.ParameterName = "@username";
                user.Value = userName.Trim();
                cmd.Parameters.Add(user);

                SqlParameter pass = new SqlParameter();
                pass.ParameterName = "@password";
                pass.Value = Hasher.HashString(password.Trim());
                //pass.Value = password.Trim();
                cmd.Parameters.Add(pass);

                conn.Open();

                int count = (int)cmd.ExecuteScalar();

                if (count > 0) returnValue = true;
            }
            catch (Exception ex)
            {
                // Log your error
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }
        else
        {
            // Log error - user name not alpha-numeric or 
            // username or password exceed the length limit!
        }

        return returnValue;
    }


    protected void GuestButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Guest.aspx");
    }
}