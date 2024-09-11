using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class TransactionRepository : Repository<Transaction, int>, ITransactionRepository
    {
        public TransactionRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Transaction> GetAll(int pageIndex, int pageSize)
        {
            return RadbContext.Transactions.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext RadbContext { get { return context as RADBContext; } }
    }
}