using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Data.Entity;
using RentApp.Models.Entities;
using System.Threading.Tasks;

namespace RentApp.Hubs
{
    [HubName("notifications")]
    public class NotificationsHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
        DbContext db { get; set; }

        public NotificationsHub(DbContext db)
        {
            this.db = db;
        }
  
        public void Hello()
        {
            Clients.All.hello();
        }

        public static void Notify(Notification notification)
        {
            hubContext.Clients.All.clickNotification(notification);
        }

        public override Task OnConnected()
        {
            Groups.Add(Context.ConnectionId, "Admins");

            //if (Context.User.IsInRole("Admin"))
            //{
            //    Groups.Add(Context.ConnectionId, "Admins");
            //}

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Groups.Remove(Context.ConnectionId, "Admins");

            //if (Context.User.IsInRole("Admin"))
            //{
            //    Groups.Remove(Context.ConnectionId, "Admins");
            //}

            return base.OnDisconnected(stopCalled);
        }

    }
}