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
using RentApp.Models;
using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Persistance.UnitOfWork.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace RentApp.Controllers
{
    public class CommentsController : ApiController
    {

        private readonly IUnitOfWork unitOfWork;

        public CommentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IEnumerable<Comment> GetComments()
        {
            return unitOfWork.Comments.GetAll();
        }

        [ResponseType(typeof(Comment))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult GetComments(int id)
        {
            lock (unitOfWork.Comments)
            {
                Comment branch = unitOfWork.Comments.Get(id);
                if (branch == null)
                {
                    return NotFound();
                }

                return Ok(branch);
            }
        }

        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PutComment(int id, Comment branch)
        {
            lock (unitOfWork.Comments)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != branch.Id)
                {
                    return BadRequest();
                }

                try
                {
                    unitOfWork.Comments.Update(branch);
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(id))
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

        [ResponseType(typeof(Branch))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PostComments(CommentBindingModel branch)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            lock (unitOfWork.Comments)
            {
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

                var services = unitOfWork.Services.GetAll();



                var serAdd = new Service();

                foreach (var service in services)
                {
                    if (service.Name == branch.ServiceName)
                    {
                        serAdd = service;
                    }
                }

                Comment com = new Comment()
                {
                    Text = branch.Text,
                    DateTime = DateTime.Now,
                    Author = appu
                };


                serAdd.Comments.Add(com);

                unitOfWork.Comments.Add(com);
                unitOfWork.Services.Update(serAdd);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = com.Id }, com);
            }
        }

        [ResponseType(typeof(Comment))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult DeleteComment(int id)
        {
            lock (unitOfWork.Comments)
            {
                Comment branch = unitOfWork.Comments.Get(id);
                if (branch == null)
                {
                    return NotFound();
                }

                branch.Deleted = true;
                unitOfWork.Comments.Update(branch);
                unitOfWork.Complete();

                return Ok(branch);
            }
        }

        private bool CommentExists(int id)
        {
            lock (unitOfWork.Comments)
            {
                return unitOfWork.Branches.Get(id) != null;
            }
        }
    }
}