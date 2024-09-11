using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    public class RateController : ApiController
    {

        private IUnitOfWork db;

        public RateController(IUnitOfWork context)
        {
            db = context;
        }

        // GET: api/Services
        public IEnumerable<Rate> GetRates()
        {
            return db.Rates.GetAll();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Rate))]
        public IHttpActionResult GetService(int id)
        {
            Rate rate = db.Rates.Get(id);
            if (rate == null)
            {
                return NotFound();
            }

            return Ok(rate);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(int id, Rate rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rate.RateID)
            {
                return BadRequest();
            }

            try
            {
                db.Rates.Update(rate);
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RateExists(id))
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

        // POST: api/Services
        [ResponseType(typeof(Rate))]
        public IHttpActionResult PostService(Rate rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Rates.Add(rate);
            db.Complete();

            return CreatedAtRoute("DefaultApi", new { id = rate.RateID }, rate);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Rate))]
        public IHttpActionResult DeleteService(int id)
        {
            Rate rate = db.Rates.Get(id);
            if (rate == null)
            {
                return NotFound();
            }

            db.Rates.Remove(rate);
            db.Complete();

            return Ok(rate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RateExists(int id)
        {
            return db.Rates.Get(id) != null;
        }


    }
}
