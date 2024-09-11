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
using RepoDemo.Persistance.UnitOfWork;

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
            return unitOfWork.Comments.GetAll();
        }


        public IEnumerable<Comment> GetCommentsService(int serviceId, int pageIndex, int pageSize)
        {
            List<Comment> comments = new List<Comment>(unitOfWork.Comments.GetAll().Where(b => b.ServiceId == serviceId).OrderBy(b => b.Date).Skip((pageIndex - 1) * pageSize).Take(pageSize));
            for (int i = 0; i < comments.Count; i++)
            {
                comments[i].AppUser = unitOfWork.AppUsers.Get(comments[i].AppUserId);
            }
            return comments;
        }


        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(int id)
        {
            Comment comment = unitOfWork.Comments.Get(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }


        // PUT: api/Comments/5
        [Authorize(Roles = "AppUser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComment(int id, Comment comment)
        {
            if (unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id != comment.AppUserId)
            {
                return BadRequest("Nonauthorized change.\nYou can not modify comment you did not add.");
            }

            comment.AppUser = null;
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out text field of comment.");
            }

            if (id != comment.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.Comments.Update(comment);
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

            return Ok(unitOfWork.Comments.GetAll().Where(c => c.ServiceId == comment.ServiceId).Count());
        }


        // POST: api/Comments
        [Authorize(Roles = "AppUser")]
        [ResponseType(typeof(Comment))]
        public IHttpActionResult PostComment(Comment comment)
        {
            comment.AppUserId = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id;

            List<Branch> branches = new List<Branch>(unitOfWork.Branches.GetAll().Where(b => b.ServiceId == comment.ServiceId));

            //da li je user koji zeli nda postavi komentar rezervisao vec u nekom od branceva servisa (tj u servisu) koji zeli da kom.
            if (unitOfWork.Rents.GetAll().Where(c => c.UserId == comment.AppUserId && branches.Find(b=>b.Id == c.ReturnBranchId)!=null).Count() == 0)
            {

                return BadRequest("You do not have reservations yet. When your first reservation exipres then you can post comment.");
            }

             if(unitOfWork.Rents.GetAll().Where(c => c.UserId == comment.AppUserId && c.ReturnBranch.ServiceId == comment.ServiceId && DateTime.Compare((DateTime)c.End, (DateTime)DateTime.Now)<0).Count() == 0)
            {
                return BadRequest("Your first reservation has not expired yet. When your first reservation exipres then you can post comment.");
            }

            comment.AppUser = null;
            comment.Date = DateTime.Now;
          

            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out text field of comment.");
            }

            unitOfWork.Comments.Add(comment);
            unitOfWork.Complete();

            comment.AppUser = unitOfWork.AppUsers.Get(comment.AppUserId);

            return Ok(unitOfWork.Comments.GetAll().Where(c => c.ServiceId == comment.ServiceId).Count());
        }


        // DELETE: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(int id)
        {
            
            Comment comment = unitOfWork.Comments.Get(id);

            AppUser loggedUser = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name);
            if (loggedUser.Id != comment.AppUserId)
            {
                return BadRequest("Nonauthorized delete.\nYou can not delete comment you did not add.");
            }

            if (comment == null)
            {
                return NotFound();
            }

            unitOfWork.Comments.Remove(comment);
            unitOfWork.Complete();

            return Ok(unitOfWork.Comments.GetAll().Where(c => c.ServiceId == comment.ServiceId).Count());
        }


        private bool CommentExists(int id)
        {
            return unitOfWork.Comments.Get(id) != null;
        }
    }
}