using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using RentApp.Persistance.UnitOfWork;

namespace RentApp.Hubs
{
    [HubName("notifications")]
    public class NotificationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        
        public void NotifyAdmin()
        {
            hubContext.Clients.Group("Admins").sendNotification("New user created.");
        }

        public void NotifyAdminService()
        {
            hubContext.Clients.Group("Admins").sendNotification("New service created, waiting for evaluation.");
        }

        public override Task OnConnected()
        {
            var identityName = Context.User.Identity.Name;
            Groups.Add(Context.ConnectionId, "Admins");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Groups.Remove(Context.ConnectionId, "Admins");
            return base.OnDisconnected(stopCalled);
        }
    }
}