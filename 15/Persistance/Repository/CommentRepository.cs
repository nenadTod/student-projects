using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RentApp.Persistance.Repository
{
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        public CommentRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Comment> GetAll(int pageIndex, int pageSize, int idService)
        {
            return RADBContext.Comments.Where<Comment>(v => v.ServiceId == idService).OrderBy(v => v.CommentDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}