using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RentApp.Persistance.Repository.Interfaces
{
    public interface IRepository<TEntity, TPKey> where TEntity : class
    {
        TEntity Get(TPKey id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}