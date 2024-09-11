using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace RentApp.Persistance.Repository
{
    public interface ICommentRepository : IRepository <Comment, int>
    {
        IEnumerable<Comment> GetAll(int pageIndex, int pageSize);
        
  
    }
}
