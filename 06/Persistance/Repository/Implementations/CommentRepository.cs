using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository.Implementations
{
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        public CommentRepository(DbContext context) : base(context)
        {
        }
        public IEnumerable<Comment> GetComments(int serviceId)
        {
            return Context.Comments.Where(x => x.ServiceId == serviceId).ToList();
        }

        protected RADBContext Context => context as RADBContext;
    }
}