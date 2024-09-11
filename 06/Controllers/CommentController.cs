using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RentApp.Controllers
{
    public class CommentController : ApiController
    {
        private IUnitOfWork _uow;

        public CommentController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("comment/get")]
        [HttpGet]
        public IHttpActionResult Get(int serviceId)
        {
            var retVal = _uow.Comments.GetComments(serviceId);
            return Ok(retVal);
        }

        [Route("comment/add")]
        [HttpPost]
        public IHttpActionResult Add(Comment comment)
        {
            _uow.Comments.Add(comment);
            _uow.Complete();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
