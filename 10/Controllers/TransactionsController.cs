using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Persistance.UnitOfWork;

namespace RentApp.Controllers
{
    public class TransactionsController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        public TransactionsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Transactions
        public IEnumerable<Transaction> GetTransactions()
        {
            return unitOfWork.Transactions.GetAll();
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(int id)
        {
            Transaction transaction = unitOfWork.Transactions.Get(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(int id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.Transactions.Update(transaction);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Transactions
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rents = unitOfWork.Rents.GetAll();
            var users = unitOfWork.AppUsers.GetAll();

            var rent = new Rent();

            foreach (var item in rents)
            {
                if(item.Id == transaction.Rent.Id)
                {
                    rent = item;
                }
            }

            var user = new AppUser();

            foreach (var item in users)
            {
                if(item.Id == transaction.User.Id)
                {
                    user = item;
                }
            }

            Transaction t = new Transaction()
            {
                Rent = rent,
                User = user,
                Amount = transaction.Amount
            };

            unitOfWork.Transactions.Add(t);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction transaction = unitOfWork.Transactions.Get(id);
            if (transaction == null)
            {
                return NotFound();
            }

            unitOfWork.Transactions.Remove(transaction);
            unitOfWork.Complete();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return unitOfWork.Transactions.Get(id) != null;
        }
    }
}