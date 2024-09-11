using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Implementations;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RentApp.Controllers
{
    public class AppUserController : ApiController
    {
        private readonly IUnitOfWork uow;

        public AppUserController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [Route("user/uploadImage/{id}")]
        [HttpPost]
        public IHttpActionResult Upload(int id)
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;

            var postedFile = httpRequest.Files["Image"];

            string extension = Path.GetExtension(postedFile.FileName);
            //Create custom filename
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + extension;
            var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
            postedFile.SaveAs(filePath);

            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            string base64 = Convert.ToBase64String(imageArray);
            string prefix = "data:image/" + extension + ";base64,";
            string base64String = prefix+base64;
            uow.AppUsers.UploadImage(base64String, id);
            return Ok(base64String);
        }

        [Route("user/getRange")]
        [HttpGet]
        public IHttpActionResult GetRange(int page, int size)
        {
            uow.AppUsers.GetRange(page, size);
            return Ok();
        }

        [Route("user/getAll")]
        [HttpGet]
        public IHttpActionResult GetAllUsers()  
        {
            var retVal = uow.AppUsers.GetAll();
            return Ok(retVal);
        }

        [Route("user/get")]
        [HttpGet]
        public IHttpActionResult GetUser(int id)
        {
            var retVal = uow.AppUsers.Get(id);
            return Ok(retVal);
        }

        [Route("user/remove")]
        [HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            try
            {
                uow.AppUsers.Remove(uow.AppUsers.Get(id));
                uow.Complete();
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        [Route("user/update")]
        [HttpPut]
        public IHttpActionResult Update(AppUser user)
        {
            uow.AppUsers.Update(user);
            uow.Complete();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            uow.Dispose();
            base.Dispose(disposing);
        }

        [Route("user/getAllUnapproved")]
        [HttpGet]
        public IHttpActionResult GetAllUnapprovedUsers()
        {
            return Ok(uow.AppUsers.GetAllUnapprovedUsers());
        }

        [Route("user/approve")]
        [HttpPut]
        public IHttpActionResult ApproveUser(AppUser user)
        {
            uow.AppUsers.ApproveUser(user);
            return Ok();
        }

        [Route("user/getAllManagers")]
        [HttpGet]
        public IHttpActionResult GetAllManagers()
        {
            return Ok(uow.AppUsers.GetAllManagers());
        }

        [Route("user/getUserDetails")]
        [HttpGet]
        public IHttpActionResult GetUserDetail(string username)
        {
            var user = uow.AppUsers.GetUserDetails(username);
            return Ok(user);
        }

        [Route("user/blockManager")]
        [HttpPut]
        public IHttpActionResult BlockManager(AppUser manager)
        {
            uow.AppUsers.BlockManager(manager);
            uow.Complete();
            return Ok();
        }
    }
}
