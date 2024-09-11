using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using RentApp.Persistance.Repository.Interfaces;
using System.Data.Entity.Migrations;

namespace RentApp.Persistance.Repository.Implementations
{
    public class Repository<TEntity, TPKey> : IRepository<TEntity, TPKey> where TEntity : class
    {
        protected readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }
        public virtual TEntity Get(TPKey id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate);
        }

        public virtual void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {           
            //context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}