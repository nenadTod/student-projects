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
using RentApp.Models;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Rents")]
    public class RentsController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private object locker = new object();

        public RentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Route("GetRents")]
        public IEnumerable<Rent> GetRents()
        {
            return unitOfWork.Rents.GetAll().Where(r=>!r.Deleted);
        }

        [ResponseType(typeof(Rent))]
        public IHttpActionResult GetRent(int id)
        {
            Rent rent = unitOfWork.Rents.Get(id);
            if (rent == null)
            {
                return NotFound();
            }

            return Ok(rent);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutRent(int id, Rent rent)
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
                unitOfWork.Rents.Update(rent);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(id))
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
    
        [Authorize]
        [Route("PostRent")]
        public string PostRent(RentRequestModel rentRequest)
        {
            lock (locker)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState).ToString();
                }

                Rent r = new Rent();
                r.Start = rentRequest.Start;
                r.End = rentRequest.End;
                r.Vehicle = unitOfWork.Vehicles.Get(rentRequest.VehicleId);
                r.BranchOffice = unitOfWork.BranchOffices.Get(rentRequest.BranchOfficeId);
                r.Vehicle.Unavailable = true;
                r.Deleted = false;
                // Izvuci iz heada jwt i uzmi koji je user
                string jwt = Request.Headers.Authorization.Parameter.ToString();
                var decodedToken = unitOfWork.AppUserRepository.DecodeJwt(jwt);

                string userEmail = decodedToken.Claims.First(claim => claim.Type == "unique_name").Value;
                AppUser appUser = unitOfWork.AppUserRepository.GetAll().Where(u => u.Email == userEmail).FirstOrDefault();


                try
                {
                    unitOfWork.Rents.Add(r);
                    appUser.Rents.Add(r);
                    unitOfWork.Complete();
                }
                catch (Exception)
                {
                    return InternalServerError().ToString();
                }


                return "Ok";

            }
        }

        [Authorize(Roles = "Admin")]
        [Route("DeleteRent")]
        [ResponseType(typeof(Rent))]
        public string DeleteRent(int id)
        {
            lock (locker)
            {
                Rent rent = unitOfWork.Rents.Get(id);
                if (rent == null)
                {
                    return NotFound().ToString();
                }

                rent.Vehicle.Unavailable = false;
                rent.Deleted = true;
                //unitOfWork.Rents.Remove(rent);
                unitOfWork.Complete();

                return "Ok";
            }
        }

        private bool RentExists(int id)
        {
            return unitOfWork.Rents.Get(id) != null;
        }
    }
}