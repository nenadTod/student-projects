using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        public CommentRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Comment> GetCommentsOfUser(int userId)
        {
            return context.Set<Comment>().Where(c => c.UserId == userId);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }

        

    }
}