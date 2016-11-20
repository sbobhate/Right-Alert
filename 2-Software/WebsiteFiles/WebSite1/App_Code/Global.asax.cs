using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

/// <summary>
/// Summary description for Global
/// </summary>
/// 

    public class Global : System.Web.HttpApplication
    {

        string conStr = ConfigurationManager.ConnectionStrings["RightAlertDB"].ConnectionString;

        public Global()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            SqlDependency.Start(conStr);
            //MySql.Data.MySqlClient.Sql
        }
        /// Many other events like begin request...e.t.c, e.t.c
        /// 
        void Application_End()
        {
            //Stop SQL dependency
            SqlDependency.Stop(conStr);
        }
    }