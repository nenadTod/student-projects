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
    [RoutePrefix("api/BranchOffices")]
    public class BranchOfficesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private object locker = new object();

        public BranchOfficesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [Route("GetBranchOffices")]
        public IEnumerable<BranchOffice> GetBranchOffices()
        {
            List<Service> AuthorizedServices = unitOfWork.Services.GetAll().Where(s => s.Authorized == true).ToList();
            List<BranchOffice> enabledBranches = new List<BranchOffice>();
            foreach (var service in AuthorizedServices)
            {
                foreach (var branchOffice in service.BranchOffices)
                {
                    enabledBranches.Add(branchOffice);
                }
            }
            return enabledBranches;
        }

        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult GetBranchOffice(int id)
        {
            BranchOffice branchOffice = unitOfWork.BranchOffices.Get(id);
            if (branchOffice == null)
            {
                return NotFound();
            }

            return Ok(branchOffice);
        }

        [Authorize(Roles ="Manager, Admin")]
        [Route("PutBranchOffice")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranchOffice(int id, BranchOfficeRequestModel branchOfficeRequest)
        {
            lock (locker)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != branchOfficeRequest.Id)
                {
                    return BadRequest();
                }

                BranchOffice branchOffice = new BranchOffice();
                branchOffice.Id = branchOfficeRequest.Id;
                branchOffice.Name = branchOfficeRequest.Name;
                branchOffice.Latitude = Double.Parse(branchOfficeRequest.Latitude);
                branchOffice.Longitude = Double.Parse(branchOfficeRequest.Longitude);
                branchOffice.Address = branchOfficeRequest.Address;

                try
                {
                    unitOfWork.BranchOffices.Update(branchOffice);
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchOfficeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok();
            }
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("PostBranchOffice")]
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult PostBranchOffice([FromBody]BranchOfficeRequestModel branchOfficeRequest)
        {
            lock (locker)
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                BranchOffice branchOffice = new BranchOffice();
                branchOffice.Name = branchOfficeRequest.Name;
                branchOffice.Latitude = Double.Parse(branchOfficeRequest.Latitude);
                branchOffice.Longitude = Double.Parse(branchOfficeRequest.Longitude);
                //branchOffice.Logo = branchOfficeRequest.Logo;
                branchOffice.Address = branchOfficeRequest.Address;

                Service service = unitOfWork.Services.Get(branchOfficeRequest.ServiceId);

                try
                {
                    unitOfWork.BranchOffices.Add(branchOffice);
                    service.BranchOffices.Add(branchOffice);
                    unitOfWork.Complete();

                    return Ok(branchOffice.Id);
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
           
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("DeleteBranchOffice")]
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult DeleteBranchOffice(int id)
        {
            lock (locker)
            {
                BranchOffice branchOffice = unitOfWork.BranchOffices.Get(id);
                if (branchOffice == null)
                {
                    return NotFound();
                }

                unitOfWork.BranchOffices.Remove(branchOffice);
                unitOfWork.Complete();

                return Ok(branchOffice);
            }
        }

        private bool BranchOfficeExists(int id)
        {
            return unitOfWork.BranchOffices.Get(id) != null;
        }
    }
}