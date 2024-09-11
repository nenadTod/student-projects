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
    public class RatingController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public RatingController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        public IEnumerable<Rating> GetRatings()
        {
            return unitOfWork.Ratings.GetAll();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Rating))]
        public IHttpActionResult GetRating(int id)
        {
            Rating rating = unitOfWork.Ratings.Get(id);
            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }

        // PUT: api/Services/5
        [Authorize(Roles = "Admin, Manager, AppUser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRating(int id, Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                rating.CommentDate = DateTime.Now;
                unitOfWork.Ratings.Update(rating);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
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
        [Authorize(Roles = "Admin, Manager, AppUser")]
        [ResponseType(typeof(Rating))]
        public IHttpActionResult PostRating(Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rating r = unitOfWork.Ratings.GetRatingUser(rating.AppUserId, rating.ServiceId);

            if (r == null)
            {
                rating.CommentDate = DateTime.Now;
                unitOfWork.Ratings.Add(rating);
                unitOfWork.Complete();
            }
            else
            {
                r.Grade = rating.Grade;
                PutRating(r.Id, r);
            }


            return CreatedAtRoute("DefaultApi", new { id = rating.Id }, rating);
        }

        // DELETE: api/Services/5
        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(Rating))]
        public IHttpActionResult DeleteRating(int id)
        {
            Rating rating = unitOfWork.Ratings.Get(id);
            if (rating == null)
            {
                return NotFound();
            }

            unitOfWork.Ratings.Remove(rating);
            unitOfWork.Complete();

            return Ok(rating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RatingExists(int id)
        {
            return unitOfWork.Ratings.Get(id) != null;
        }
    }
}
