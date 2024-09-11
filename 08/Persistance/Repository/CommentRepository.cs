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
        protected RADBContext DemoContext { get { return context as RADBContext; } }

        public IEnumerable<Comment> GetAll(int pageIndex, int pageSize)
        {
            return DemoContext.Comments.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}