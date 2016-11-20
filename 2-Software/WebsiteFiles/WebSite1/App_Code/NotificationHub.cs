using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;


public class NotificationHub : Hub
{
    public void NotifyAllClients(string msg, string msg2, string[] status, string[] datetime, string[] batteryData)
    {
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.displayNotification(msg, msg2, status, datetime, batteryData);
    }

}