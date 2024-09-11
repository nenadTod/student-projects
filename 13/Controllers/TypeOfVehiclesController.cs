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
using RentApp.Persistance.UnitOfWork.Interface;

namespace RentApp.Controllers
{
    public class TypeOfVehiclesController : ApiController
    {
   

        private readonly IUnitOfWork unitOfWork;

        public TypeOfVehiclesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IEnumerable<TypeOfVehicle> GetTypes()
        {
            return unitOfWork.Types.GetAll();
        }

        [ResponseType(typeof(TypeOfVehicle))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult GetTypes(int id)
        {
            lock (unitOfWork.Types)
            {
                TypeOfVehicle rent = unitOfWork.Types.Get(id);
                if (rent == null)
                {
                    return NotFound();
                }

                return Ok(rent);
            }
        }

        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PutType(int id, TypeOfVehicle rent)
        {
            lock (unitOfWork.Types)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != rent.Id)
                {
                    return BadRequest();
                }

                try
                {
                    unitOfWork.Types.Update(rent);
                    unitOfWork.Complete();
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
        }

        [ResponseType(typeof(TypeOfVehicle))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PostTypes(TypeOfVehicle rent)
        {
            lock (unitOfWork.Types)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                unitOfWork.Types.Add(rent);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = rent.Id }, rent);
            }
        }

        [ResponseType(typeof(TypeOfVehicle))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult DeleteTypeOfVehicle(int id)
        {
            lock (unitOfWork.Types)
            {
                TypeOfVehicle rent = unitOfWork.Types.Get(id);
                if (rent == null)
                {
                    return NotFound();
                }

                foreach (var item in rent.Vehicles)
                {
                    item.Deleted = true;
                }

                rent.Deleted = true;

                unitOfWork.Types.Update(rent);
                unitOfWork.Complete();

                return Ok(rent);
            }
        }

        private bool TypeOfVehicleExists(int id)
        {
            lock (unitOfWork.Types)
            {
                return unitOfWork.Types.Get(id) != null;
            }
        }
    }
}