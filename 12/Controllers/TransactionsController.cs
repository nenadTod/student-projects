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
       
        // GET: api/Transactions/5
       
        // POST: api/Transactions
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction trans)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Transaction t = new Transaction() { OrderID = trans.OrderID, PayerID = trans.PayerID, PaymentID = trans.PaymentID };

            unitOfWork.Transaction.Add(t);
            unitOfWork.Complete();


            

            return CreatedAtRoute("DefaultApi", new { id = trans.Id }, trans);
        }

        // DELETE: api/Transactions/5
       
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}