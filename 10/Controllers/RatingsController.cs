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
    public class RatingsController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public RatingsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Ratings
        public IEnumerable<Rating> GetRatings()
        {
            return unitOfWork.Ratings.GetAll();
        }

        // GET: api/Ratings/5
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

        // PUT: api/Ratings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRating(int id, Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rating.Id)
            {
                return BadRequest();
            }

            var ratings = unitOfWork.Ratings.GetAll();

            Rating rat = new Rating();

            foreach (var item in ratings)
            { 
                if(item.Id == id)
                {
                    rat = item;
                }
            }

            rat.TypeOfVote = rating.TypeOfVote;

            try
            {
                unitOfWork.Ratings.Update(rat);
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

        // POST: api/Ratings
        [ResponseType(typeof(Rating))]
        public IHttpActionResult PostRating(Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var services = unitOfWork.Services.GetAll();

            var service = new Service();

            foreach(var item in services)
            {
                if(item.Id == rating.Service.Id)
                {
                    service = item;
                }
            }

            var users = unitOfWork.AppUsers.GetAll();

            var user = new AppUser();

            foreach (var item in users)
            {
                if (item.Id == rating.User.Id)
                {
                    user = item;
                }
            }

            Rating rat = new Rating()
            {
                Service = service,
                User = user,
                TypeOfVote = rating.TypeOfVote
            };

            unitOfWork.Ratings.Add(rat);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = rating.Id }, rating);
        }

        // DELETE: api/Ratings/5
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