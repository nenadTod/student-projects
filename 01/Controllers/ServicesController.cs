using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RepoDemo.Persistance.UnitOfWork;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Linq;
using System;

namespace RentApp.Controllers
{
    public class ServicesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public ServicesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

      
        public IEnumerable<Service> GetServices()
        {
            return unitOfWork.Services.GetAll();
        }

       
        public IEnumerable<Service> GetServices(int pageIndex, int pageSize)
        {
            return unitOfWork.Services.GetAll(pageIndex, pageSize);
        }


        [ResponseType(typeof(Service))]
        public IHttpActionResult GetService(int id)
        {
            Service service = unitOfWork.Services.Get(id);
            if (service == null)
            {
                return NotFound();
            }
            
            return Ok(service);
        }

        public double GetServiceGrade(int ServiceGradeid)
        {
            List<Grade> grades = new List<Grade>(unitOfWork.Grades.GetAll().Where(g => g.ServiceId == ServiceGradeid));
            if (grades.Count() > 0)
            {
                double total = (double)grades.Sum(item => item.GradeNum) / (double)grades.Count();
                return Math.Round(total,1);
            }

            return 0;


        }


        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(int id, Service service)
        {
            if (unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id != service.CreatorId)
            {
                return BadRequest("Nonauthorized change.\nYou can not modify service you did not add.");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out all fields and enter correct values.");
            }

            if (id != service.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.Services.Update(service);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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


        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult PostService(Service service)
        {
            service.CreatorId = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id;
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out all fields and enter correct values.");
            }

            unitOfWork.Services.Add(service);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = service.Id }, service);
        }


        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = unitOfWork.Services.Get(id);

            if (unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id != service.CreatorId)
            {
                return BadRequest("Nonauthorized delete.\nYou can not delete service you did not add.");
            }


            List<Branch> branches = new List<Branch>(unitOfWork.Branches.GetAll());         
            if(branches.Where(i => i.ServiceId == id).FirstOrDefault()!=null)
            {
                return BadRequest("Service can't be deleted because there are branches which belong to this service.");
            }
   
            if (service == null)
            {
                return NotFound();
            }

            unitOfWork.Services.Remove(service);
            unitOfWork.Complete();

            return Ok(service);
        }

        private bool ServiceExists(int id)
        {
            return unitOfWork.Services.Get(id) != null;
        }
    }
}