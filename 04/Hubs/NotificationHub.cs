using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace RentApp.Hubs
{
    [HubName("notifications")]
    [Authorize(Roles = "Admin")]
    public class NotificationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

        public NotificationHub()
        {
           
        }


        public static void Notify(Notification notification)
        {
           
            hubContext.Clients.Group("Admins").newNotification(notification);
            //hubContext.Clients.Group("Admins").clickNotification($"Clicks: {clickCount}");
        }

        

        public override Task OnConnected()
        {
            //Ako vam treba pojedinacni User
            var identityName = Context.User.Identity.Name;

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