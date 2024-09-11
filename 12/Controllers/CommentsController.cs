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
    public class CommentsController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public CommentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Comments
        public IEnumerable<Comment> GetComments()
        {
            var c = unitOfWork.Comment.GetAll();
            return unitOfWork.Comment.GetAll();
        }

        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(int id)
        {
            Comment comment = unitOfWork.Comment.Get(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/Comments/5
        [Authorize(Roles = "Manager, Admin, AppUser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.Comment.Update(comment);
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

        [AllowAnonymous]
        [Route("api/Comments/ReturnCommentsByServiceId")]
        [HttpGet]
        public List<Comment> ReturnCommentsByServiceId(string name)
        {
            //int id = Int32.Parse(model);
            var comments = unitOfWork.Comment.GetAll();
            List<Comment> lista = new List<Comment>();

            foreach (var item in comments)
            {
                if(item.ServiceName == name)
                {
                    lista.Add(item);
                }
            }

            return lista;
        }

        // POST: api/Comments
        //[Authorize(Roles = "Manager, Admin, AppUser")]
        [AllowAnonymous]
        [ResponseType(typeof(Comment))]
        public IHttpActionResult PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            unitOfWork.Comment.Add(comment);

            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = comment.Id }, comment);
        }

        [ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = unitOfWork.Comment.Get(id);
            if (comment == null)
            {
                return NotFound();
            }

            unitOfWork.Comment.Remove(comment);
            unitOfWork.Complete();

            return Ok(comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return unitOfWork.Comment.Get(id) != null;
        }
    }
}