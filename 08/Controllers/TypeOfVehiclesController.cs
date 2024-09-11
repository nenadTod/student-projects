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
    public class TypeOfVehiclesController : ApiController
    {
        private RADBContext db = new RADBContext();
        private readonly IUnitOfWork unitOfWork;

        public TypeOfVehiclesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("api/GetVehicleTypes")]
        public IEnumerable<TypeOfVehicle> GetVehicleTypes()
        {
            return unitOfWork.TypesOfVehicle.GetAll();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/AddVehicleType")]
        public IHttpActionResult PutTypeOfVehicle(string type)
        {
            IEnumerable<TypeOfVehicle> types = unitOfWork.TypesOfVehicle.GetAll();
            
            if(type=="undefined")
            {
                return Ok();
            }

            foreach (TypeOfVehicle t in types)
            {
                if(t.Name==type)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }

            TypeOfVehicle newType = new TypeOfVehicle() { Name = type };
            unitOfWork.TypesOfVehicle.Add(newType);
            unitOfWork.Complete();

            return Ok();
        }

        // GET: api/TypeOfVehicles/5
        [ResponseType(typeof(TypeOfVehicle))]
        public IHttpActionResult GetTypeOfVehicle(int id)
        {
            TypeOfVehicle typeOfVehicle = db.TypesOfVehicle.Find(id);
            if (typeOfVehicle == null)
            {
                return NotFound();
            }

            return Ok(typeOfVehicle);
        }

        // PUT: api/TypeOfVehicles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTypeOfVehicle(int id, TypeOfVehicle typeOfVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != typeOfVehicle.Id)
            {
                return BadRequest();
            }

            db.Entry(typeOfVehicle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeOfVehicleExists(id))
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

        // POST: api/TypeOfVehicles
        [ResponseType(typeof(TypeOfVehicle))]
        public IHttpActionResult PostTypeOfVehicle(TypeOfVehicle typeOfVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TypesOfVehicle.Add(typeOfVehicle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = typeOfVehicle.Id }, typeOfVehicle);
        }

        // DELETE: api/TypeOfVehicles/5
        [ResponseType(typeof(TypeOfVehicle))]
        public IHttpActionResult DeleteTypeOfVehicle(int id)
        {
            TypeOfVehicle typeOfVehicle = db.TypesOfVehicle.Find(id);
            if (typeOfVehicle == null)
            {
                return NotFound();
            }

            db.TypesOfVehicle.Remove(typeOfVehicle);
            db.SaveChanges();

            return Ok(typeOfVehicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TypeOfVehicleExists(int id)
        {
            return db.TypesOfVehicle.Count(e => e.Id == id) > 0;
        }
    }
}