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
    public class PriceListController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public PriceListController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        public IEnumerable<PriceList> GetServices()
        {
            return unitOfWork.Pricelist.GetAll();
        }

        // GET: api/Services/5
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult GetPriceList(int id)
        {
            PriceList priceList = unitOfWork.Pricelist.Get(id);
            if (priceList == null)
            {
                return NotFound();
            }

            return Ok(priceList);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPriceList(int id, PriceList priceList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceList.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.Pricelist.Update(priceList);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceListExists(id))
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
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult PostPriceList(PriceList priceList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Pricelist.Add(priceList);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = priceList.Id }, priceList);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult DeletePriceList(int id)
        {
            PriceList priceList = unitOfWork.Pricelist.Get(id);
            if (priceList == null)
            {
                return NotFound();
            }

            unitOfWork.Pricelist.Remove(priceList);
            unitOfWork.Complete();

            return Ok(priceList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceListExists(int id)
        {
            return unitOfWork.Pricelist.Get(id) != null;
        }
    }
}
