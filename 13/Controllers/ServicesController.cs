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
using System.Web.Http.ModelBinding;
using RentApp.Models;

namespace RentApp.Controllers
{
    public class ServicesController : ApiController
    {



    private readonly IUnitOfWork unitOfWork;

    public ServicesController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IEnumerable<Service> GetServices()
    {
        return unitOfWork.Services.GetAll();
    }

    [ResponseType(typeof(Service))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult GetService(int id)
    {
            lock (unitOfWork.Services)
            {
                Service service = unitOfWork.Services.Get(id);
                if (service == null)
                {
                    return NotFound();
                }

                return Ok(service);
            }
    }

    [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PutService(int id, Service service)
    {
            lock (unitOfWork.Services)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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
    }


        [HttpPut]
        [Route("api/Services/Rate/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult RateService(int id, RateBindingModel rate)
        {
            lock (unitOfWork.Services)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var services = unitOfWork.Services.GetAll();
                var serviceEdit = new Service();

                foreach (var s in services)
                {
                    if (s.Id == id)
                    {
                        serviceEdit = s;
                    }
                }

                string name = User.Identity.Name;


                var appu = new AppUser();


                var appusers = unitOfWork.AppUsers.GetAll();

                foreach (var au in appusers)
                {
                    if (au.Username == name)
                    {
                        appu = au;
                    }
                }


                bool canComment = false;

                foreach (var r in appu.Renting)
                {
                    int result = DateTime.Compare((DateTime)r.Start, (DateTime)r.End);

                    if (result <= 0)
                    {
                        canComment = true;
                    }

                }

                if (!canComment)
                    return null;

                float gradeValue = ((serviceEdit.AverageGrade + (float)rate.Rating)) / (serviceEdit.NumberOfGrades + 1);
                serviceEdit.AverageGrade = gradeValue;
                serviceEdit.NumberOfGrades++;

                try
                {
                    unitOfWork.Services.Update(serviceEdit);
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
        }

        [HttpPut]
        [Route("api/Services/Approve/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult ApproveService(int id, RateBindingModel rate)
        {
            lock (unitOfWork.Services)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var services = unitOfWork.Services.GetAll();
                var serviceEdit = new Service();

                foreach (var s in services)
                {
                    if (s.Id == id)
                    {
                        serviceEdit = s;
                    }
                }

                serviceEdit.Approved = true;

                try
                {
                    unitOfWork.Services.Update(serviceEdit);
                    unitOfWork.Complete();

                    //string your_id = "kristijansalaji20@gmail.com";
                    //string your_password = PASSWORD;

                    //SmtpClient client = new SmtpClient();
                    //client.Port = 587;
                    //client.Host = "smtp.gmail.com";
                    //client.EnableSsl = true;
                    //client.Timeout = 10000;
                    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //client.UseDefaultCredentials = false;
                    //client.Credentials = new System.Net.NetworkCredential(your_id, your_password);

                    //MailMessage mm = new MailMessage(your_id, "kristijan.salaji@outlook.com");
                    //mm.BodyEncoding = UTF8Encoding.UTF8;
                    //mm.Subject = "CODE FOR FORUM";
                    //mm.Body = "NALOG JE ODOBREN!";
                    //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    //client.Send(mm);
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
        }

        [ResponseType(typeof(Service))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PostService(Service service)
    {
            lock (unitOfWork.Services)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                service.AverageGrade = 0;
                service.Branches = new List<Branch>();
                service.Comments = new List<Comment>();
                service.NumberOfGrades = 0;
                service.Vehicles = new List<Vehicle>();

                unitOfWork.Services.Add(service);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = service.Id }, service);
            }
    }

    [ResponseType(typeof(Service))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult DeleteService(int id)
    {
            lock (unitOfWork.Services)
            {
                Service service = unitOfWork.Services.Get(id);
                if (service == null)
                {
                    return NotFound();
                }

                foreach (var item in service.Branches)
                {
                    item.Deleted = true;
                }

                foreach (var item in service.Vehicles)
                {
                    item.Deleted = true;
                }

                foreach (var item in service.Comments)
                {
                    item.Deleted = true;
                }

                service.Deleted = true;

                unitOfWork.Services.Update(service);
                unitOfWork.Complete();

                return Ok(service);
            }
    }

    private bool ServiceExists(int id)
    {
            lock (unitOfWork.Services)
            {
                return unitOfWork.Services.Get(id) != null;
            }
    }
}
}