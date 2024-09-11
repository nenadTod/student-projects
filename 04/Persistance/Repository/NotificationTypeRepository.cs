using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using RentApp.Models.Entities;
using System.Data.Entity;

namespace RentApp.Persistance.Repository
{
    public class NotificationTypeRepository : Repository<NotificationType, int>, INotificationTypeRepository
    {
        public NotificationTypeRepository(DbContext context) : base(context)
        {
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }

    }
}